using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using TestTask.Identity.Services.Filters;
using TestTask.Transaction.DB.Schema;

namespace TestTask.Transaction.Services
{
    public class Startup
    {

        public IConfiguration Configuration { get; }
        public string Environment { get; }
        public IContainer Container { get; private set; }

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Environment = env.EnvironmentName;
            Configuration = builder.Build();  
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetSection("AppSettings:ConnectionString").Value;


            Func<string, SqlConnection> getConnection = x =>
            {
                switch (x)
                {
                    case "dev": return new SqlConnection(connectionString);
                    case "test": return new SqlConnection(connectionString);
                    case "prod": return new SqlConnection(connectionString);
                    default: return new SqlConnection(connectionString);
                }
            };

            services.AddDbContext<DataContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(getConnection(Environment));
            });

            services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            DependencyResolver.Resolve(services, Configuration);

            services.AddControllersWithViews();
            services.AddMvc(option => option.EnableEndpointRouting = false);
       
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "Transaction API",
                    Description = "Controller \"Transaction\" contains all the necessary requests described in the \"Test case.pdf\"",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact { Name = "Vladyslav Slipchenko", Email = "shprotodav@gmail.com"}
                });
 
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the token from Identity API:
                                    Login at Identity API successfully -> Copy token from response -> Enter your token in the text input 'Value' below:",
                    Name = "resttoken",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
                });
            });

            services.ConfigureSwaggerGen(options =>
            {
                options.IncludeXmlComments("TestTask.Transaction.Services.xml");
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Transaction}/{action}");
            });
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger API V1");
            });
        }
    }


}
