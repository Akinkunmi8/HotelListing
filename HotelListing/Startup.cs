using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.Options;
using HotelListing.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using HotelListing.Configurations;
using HotelListing.IRepository;
using HotelListing.Repository;
using Microsoft.AspNetCore.Identity;
using HotelListing.Services;
using AspNetCoreRateLimit;

namespace HotelListing
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
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SqlConn"))
            );
            // gor jwt seeting
            services.AddAuthentication();
            services.ConfigureIdentity();
            services.ConfigureJWT(Configuration);
            services.AddCors(c => 
            {
                c.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            
            });

            services.AddAutoMapper(typeof(MapperInitializer));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthManager, AuthManager>();
            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1", new  OpenApiInfo { Title = "HotelListing", Version = "v1", Description = " HotelListing Api Endpint" });
            
            });
            services.AddControllers( config =>
            {
                config.CacheProfiles.Add("120SecondsDuration", new CacheProfile
                {
                    Duration = 120
                });
            }).AddNewtonsoftJson(opt => 
            
                opt.SerializerSettings.ReferenceLoopHandling=
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddIdentity<ApiUser, IdentityRole>().AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();
            // API features
            services.ConfigureVersioning();
            services.ConfigureRateLimiting();
            services.AddHttpContextAccessor();
            services.AddHttpCacheHeaders();
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        { 
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HostelListing v1"));

            app.ConfigureExceptionHandler();
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");

            app.UseResponseCaching();
            app.UseHttpCacheHeaders();
            app.UseIpRateLimiting();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
