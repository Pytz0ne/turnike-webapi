using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LightQuery.Swashbuckle;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;
using turnike_webapi.Rdbms;

namespace turnike_webapi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddMemoryCache();
            services.AddResponseCompression();

            services.AddDbContext<postgresContext>(options => options.UseNpgsql(Program.SqlConnectionString));

            services.AddCors(_options =>
            {
                _options.AddPolicy(MyAllowSpecificOrigins,
                        builder =>
                        {
                            builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            //.WithExposedHeaders(new string[] { "X-Recaptchacount", "Content-Disposition" })


                            //.AllowAnyMethod()
                            //.AllowAnyHeader()
                            //.AllowCredentials();
                            .Build();
                        });
            });

            services.AddApiVersioning(
                options =>
                {
                    options.ReportApiVersions = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                });
            services.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddControllersWithViews().AddNewtonsoftJson(config =>
            {
                config.SerializerSettings.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;
                config.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

                config.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                //config.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
                config.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //config.SerializerSettings.ContractResolver = ShouldSerializeContractResolver.Instance;
            });
            services.AddRazorPages().AddNewtonsoftJson().AddXmlSerializerFormatters();


            services.AddControllers()
                   .AddNewtonsoftJson(options =>
                       options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
                   .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            ;

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(
               options =>
               {
                   // add a custom operation filter which sets default values
                   options.OperationFilter<LightQueryOperationFilter>();

                   options.SwaggerDoc("openapi30", new OpenApiInfo()
                   {
                       Description = "openapi30"
                   });

                   // Set the comments path for the Swagger JSON and UI.
                   var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                   var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                   options.IncludeXmlComments(xmlPath);
               });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseResponseCompression();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();


            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                        options.RoutePrefix = string.Empty;
                        options.DefaultModelExpandDepth(0);
                    }
                });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
