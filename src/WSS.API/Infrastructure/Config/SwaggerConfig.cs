using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WSS.API.Infrastructure.Config;

/// <summary>
///     Swagger Config
/// </summary>
public static class SwaggerConfig
{
    /// <summary>
    ///     Register Swagger Module
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterSwaggerModule(this IServiceCollection services)
    {
        services.AddApiVersioning(x =>
        {
            x.DefaultApiVersion = new ApiVersion(1, 0);
            x.AssumeDefaultVersionWhenUnspecified = true;
            x.ReportApiVersions = true;
        });
        services.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });


        services.AddSwaggerGen(c =>
        {
            // Set Description Swagger
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Wedding Service API",
                Version = "v1",
                Description = "WSS API Endpoints for Owner",
                Contact = new OpenApiContact
                {
                    Name = "WSS Owner"
                }
            });
            
            c.SwaggerDoc("v2", new OpenApiInfo
            {
                Title = "Wedding Service API for Partner and Staff",
                Version = "v2",
                Description = "WSS API Endpoints Partner and Staff",
                Contact = new OpenApiContact
                {
                    Name = "WSS Partner and Staff"
                }
            });
            
            c.SwaggerDoc("v3", new OpenApiInfo
            {
                Title = "Wedding Service API for Customer",
                Version = "v3",
                Description = "WSS API Endpoints Customer",
                Contact = new OpenApiContact
                {
                    Name = "WSS Customer"
                }
            });
            
            // c.DocInclusionPredicate((docName, apiDesc) =>
            // {
            //     var versions = apiDesc.CustomAttributes()
            //         .OfType<ApiVersionAttribute>()
            //         .SelectMany(attr => attr.Versions);
            //
            //     return versions.Any(v => $"v{v.ToString()}" == docName);
            // });

            c.DescribeAllParametersInCamelCase();
            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);

            // c.SchemaFilter<EnumSchemaFilter>();

            // Set Authorize box to swagger
            var jwtSecuriyScheme = new OpenApiSecurityScheme
            {
                Scheme = "bearer",
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Description = "Put **_ONLY_** your token on textbox below!",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            c.AddSecurityDefinition(jwtSecuriyScheme.Reference.Id, jwtSecuriyScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecuriyScheme, Array.Empty<string>() }
            });
        });
        services.AddSwaggerGenNewtonsoftSupport();
        return services;
    }

    /// <summary>
    ///     Use Swagger
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseApplicationSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger(c => { c.RouteTemplate = "{documentName}/api-docs"; });
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/v1/api-docs", "WSS.API Owner");
            c.SwaggerEndpoint("/v2/api-docs", "WSS.API Partner and Staff");
            c.SwaggerEndpoint("/v3/api-docs", "WSS.API Customer");
            c.RoutePrefix = string.Empty;
        });

        return app;
    }
}