using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.Text;
using AutoMapper;
using Hangfire;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.CQRS.QueryHandlers.RssSourceQueryHandlers;
using NewsAggregator.Services.Implementation.CqsServices;
using NewsAggregator.Services.Implementation.Mapping;
using NewsAggregator.Services.Implementation.NewsRating;
using WebApplication.Authentication.JWT;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connString = Configuration.GetSection("ConnectionStrings")
                .GetValue<string>("DefaultConnection");
            services.AddDbContext<NewsAggregatorContext>(opt =>
                opt.UseSqlServer(connString));


            #region Servies
            services.AddScoped<ICommentService, CommentCqsService>();
            services.AddScoped<INewsService, NewsCqsService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenCqsService>();
            services.AddScoped<IRoleService, RoleCqsService>();
            services.AddScoped<IRssSourceService, RssSourceCqsService>();
            services.AddScoped<IUserService, UserCqsService>();
            services.AddScoped<IJwtAuthentication, JwtAuthentication>();

            services.AddScoped<INewsRatingService, NewsRatingService>();

            #endregion

            #region Hangfire

            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(10),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(10),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            services.AddHangfireServer();

            #endregion

            #region Other libraries

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapping());
            });
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddMediatR(typeof(GetAllRssSourcesQueryHandler).GetTypeInfo().Assembly);

            #endregion

            #region Authentication

            services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>
                {
                    opt.RequireHttpsMetadata = false;
                    opt.SaveToken = true;

                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        //true - encode our key:
                        ValidateIssuerSigningKey = true,
                        //Encode out key using this key:
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                        //we don't wanna validate for what this key was given:
                        ValidateIssuer = false,
                        //we don't wanna validate who(our app) gave this key:
                        ValidateAudience = false,
                        //Validate expireAt time
                        ClockSkew = TimeSpan.Zero 
                    };
                });

            services.AddCors(options =>
            {
                options.AddPolicy("Default", builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });

            #endregion


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApplication", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication v1"));


            app.UseHangfireServer();
            app.UseHangfireDashboard();

            var newsService = serviceProvider.GetService(typeof(INewsService)) as INewsService;
            RecurringJob.AddOrUpdate(() => newsService.RateNews(), " */30 * * * * ");
            RecurringJob.AddOrUpdate(() => newsService.AggregateNews(), " 0 * * * * ");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("Default");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        }
    }
}
