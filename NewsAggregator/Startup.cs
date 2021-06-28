using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewsAggregator.DAL.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using NewsAggregator.AuthorizationPolicies;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Implementation;
using NewsAggregator.DAL.Repositories.Implementation.Repositories;
using NewsAggregator.DAL.Repositories.Interfaces;
using NewsAggregator.Filters;
using NewsAggregator.Services.Implementation.Mapping;
using NewsAggregator.Services.Implementation.NewsParsers;
using NewsAggregator.Services.Implementation.NewsParsers.SourcesParsers;
using NewsAggregator.Services.Implementation.Services;

namespace NewsAggregator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime.
        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NewsAggregatorContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            #region Repository
            services.AddTransient<IBaseRepository<News>, NewsRepository>();
            services.AddTransient<IBaseRepository<Comment>, CommentRepository>();
            services.AddTransient<IBaseRepository<Role>, RoleRepository>();
            services.AddTransient<IBaseRepository<RssSource>, RssSourceRepository>();
            services.AddTransient<IBaseRepository<User>, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            #endregion

            #region Services
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<IRssSourceService, RssSourceService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ICommentService, CommentService>();

            #endregion

            #region Parsers
            services.AddTransient<IWebPageParser, OnlinerParser>();
            services.AddTransient<IWebPageParser, S13Parser>();
            services.AddTransient<IWebPageParser, WylsacomParser>();
            #endregion

            #region AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapping());
            });
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            #region Filters
            services.AddScoped<CustomExceptionFilterAttribute>();
            #endregion

            #region Authorization and Authentication

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opt =>
                {
                    opt.LoginPath = new PathString("/Account/Login");
                    opt.AccessDeniedPath = new PathString("/Account/NoAccessRights");
                });

            services.AddAuthorization(opt =>
                opt.AddPolicy("18+Content", policy =>
                    policy.Requirements.Add(new MinAgeRequirement(18))));
            services.AddSingleton<IAuthorizationHandler, MinAgeHandler>();
            #endregion

            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddControllersWithViews()
                .AddMvcOptions(opt =>
            {
                opt.Filters.Add(typeof(CustomExceptionFilterAttribute));
            });
        }

        // This method gets called by the runtime.
        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days.
                // You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });


        }
    }
}