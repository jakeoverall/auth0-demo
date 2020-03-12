using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CoreDemo.Repositories;
using CoreDemo.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace CoreDemo
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
            //ADD USER AUTH through JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = $"https://{Configuration["Auth0:Domain"]}/";
                options.Audience = Configuration["Auth0:Audience"];
            });
            services.AddCors(options =>
            {
                options.AddPolicy("CorsDevPolicy", builder =>
                {
                    builder
                        .WithOrigins(new string[]
                        {
                            "http://localhost:8080"
                        })
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            services.AddControllers();

            //CONNECT TO DB
            services.AddScoped<IDbConnection>(x => CreateDbConnection());

            //NOTE REGISTER SERVICES AND REPOSITORIES
            services.AddTransient<BlogsService>();
            services.AddTransient<BlogsRepository>();
        }

        private IDbConnection CreateDbConnection()
        {
            string connectionString = Configuration.GetSection("DB").GetValue<string>("gearhost");
            return new MySqlConnection(connectionString);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("CorsDevPolicy");
            }
            else
            {
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}