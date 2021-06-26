using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Hangfire;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.Core.Services.Interfaces.ServicesInterfaces;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.CQRS.QueryHandlers.RssSourceQueryHandlers;
using NewsAggregator.DAL.Repositories.Implementation;
using NewsAggregator.DAL.Repositories.Implementation.Repositories;
using NewsAggregator.DAL.Repositories.Interfaces;
using NewsAggregator.Services.Implementation.CqsServices;
using NewsAggregator.Services.Implementation.Mapping;
using NewsAggregator.Services.Implementation.NewsRating;
using NewsAggregator.Services.Implementation.Services;

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

            services.AddScoped<INewsService, NewsCqsService>();
            services.AddScoped<INewsRatingService, NewsRatingService>();
            services.AddScoped<IRssSourceService, RssSourceCqsService>();
            
            #endregion

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


            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapping());
            });
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddMediatR(typeof(GetAllRssSourcesQueryHandler).GetTypeInfo().Assembly);




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
            RecurringJob.AddOrUpdate(() => newsService.RateNews(), " */3 * * * * ");
            RecurringJob.AddOrUpdate(() => newsService.AggregateNews(), " 0 * * * * ");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        }
    }
}
