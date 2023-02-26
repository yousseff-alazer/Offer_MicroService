using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Offer.API.Offer.DAL.DB;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Offer.API
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
.SetBasePath(env.ContentRootPath)
.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
.AddEnvironmentVariables();

            Configuration = builder.Build();
            Log.Logger = new LoggerConfiguration()
    .WriteTo.File(Configuration["Logging:LogPath"], rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 100000)
.MinimumLevel.Debug()
.CreateLogger();
            Log.Information("Inside Startup ctor");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                Log.Information("ConfigureServices");
                services.AddCors();
                Log.Information("dbConnectionString");
                var dbConnectionString = Configuration.GetValue<string>("DatabaseSettings:ConnectionString");
                Log.Information(dbConnectionString);
                services.AddDbContext<OfferDbContext>(options => options.UseNpgsql(Configuration.GetValue<string>("DatabaseSettings:ConnectionString")));
                services.AddControllers();
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Offer.API", Version = "v1" });
                    // Set the comments path for the Swagger JSON and UI.
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                });
                //                services.AddSwaggerGen(c =>
                //                {
                //                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Offer.API", Version = "v1" });
                //                    // Set the comments path for the Swagger JSON and UI.
                //                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                //                    // Set the comments path for the Swagger JSON and UI.//var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";//var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);//c.IncludeXmlComments(xmlPath);//c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme//{//    Description = "`Token only!!!` - without `Bearer_` prefix",//    Type = SecuritySchemeType.Http,//    BearerFormat = "JWT",//    In = ParameterLocation.Header,//    Scheme = "bearer"//});
                //                    var security = new Dictionary<string, IEnumerable<string>>
                //                {
                //                    { "Bearer", new string[] { } },
                //                };

                //                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //                    {
                //                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                //                        Name = "Authorization",
                //                        In = ParameterLocation.Header,
                //                        Type = SecuritySchemeType.ApiKey
                //                    });

                //                    // Im not sure how to get the token to apply to the header being called. It will get the token, but it doesnt apply it to the request

                //                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer"
                //            }
                //        },
                //        new string[] {}
                //    }
                //});
                //                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //                    c.IncludeXmlComments(xmlPath);
                //                    //var xmlDocFile = Path.Combine(AppContext.BaseDirectory, "MessageService.xml");
                //                    //if (File.Exists(xmlDocFile))
                //                    //{
                //                    //    var comments = new XPathDocument(xmlDocFile);
                //                    //    c.OperationFilter<XmlCommentsOperationFilter>(comments);
                //                    //    c.SchemaFilter<XmlCommentsSchemaFilter>(comments);
                //                    //}
                //                });
            }
            catch (Exception ex)
            {
                Log.Information(ex.Message + ex.StackTrace);
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {

            try
            {
                Log.Information("Configure");
                loggerFactory.AddSerilog();
                app.UseStaticFiles(); // For the wwwroot folder
                app.UseRouting();
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Offer.API v1"));


                app.UseCors(x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true) // allow any origin
                    .AllowCredentials()); // allow credentials

                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }
            catch (Exception ex)
            {
                Log.Information(ex.Message + ex.StackTrace);
            }
   
        }
    }
}