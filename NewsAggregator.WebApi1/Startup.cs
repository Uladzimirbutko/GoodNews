using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using AutoMapper;
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
using NewsAggregator.Services.Implementation.Services;

namespace NewsAggregator.WebApi
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
            services.AddDbContext<NewsAggregatorContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            #region Repositories

            services.AddTransient<IBaseRepository<News>, NewsRepository>();
            services.AddTransient<IBaseRepository<Comment>, CommentRepository>();
            services.AddTransient<IBaseRepository<Role>, RoleRepository>();
            services.AddTransient<IBaseRepository<RssSource>, RssSourceRepository>();
            services.AddTransient<IBaseRepository<User>, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #endregion

            #region Servies

            services.AddScoped<INewsService, NewsCqsService>();

            services.AddScoped<IRssSourceService, RssSourceCqsService>();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<ICommentService, CommentService>();
            
            #endregion


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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NewsAggregator.WebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NewsAggregator.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
