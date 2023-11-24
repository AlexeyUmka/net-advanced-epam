using Catalog.Application;
using Catalog.Application.Clients;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Persistence;
using Catalog.WebUI.Filters;
using FluentValidation.AspNetCore;
using Messaging.RabbitMq.Client;
using Messaging.RabbitMq.Client.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.Swagger;

namespace Catalog.WebUI;

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
        services.AddApplication();
        services.AddInfrastructure(Configuration);
        
        services.AddSingleton(Configuration.GetSection(nameof(RabbitMqConfig)).Get<RabbitMqConfig>());
        services.AddScoped<IConnection>(x => new ConnectionFactory() { HostName = "localhost" }.CreateConnection());
        services.AddScoped<IRabbitMqClient, CatalogRabbitMqClient>();

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddControllersWithViews(options =>
            options.Filters.Add<ApiExceptionFilterAttribute>())
                .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

        services.AddOpenApiDocument();
        services.AddSwaggerGen(o =>
        {
            // Add JWT Bearer
            o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            o.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });
        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "https://localhost:5001";

                options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                options.TokenValidationParameters.ValidateAudience = false;
            });
        services.AddAuthorization(o =>
        {
            o.AddPolicy(Constants.AuthorizationConstants.Policies.Create, policy =>
            {
                policy.RequireRole(Constants.AuthorizationConstants.Roles.Manager);
                policy.RequireClaim("scope", Constants.AuthorizationConstants.Permissions.Create);
            });
            o.AddPolicy(Constants.AuthorizationConstants.Policies.Read, policy =>
            {
                policy.RequireRole(Constants.AuthorizationConstants.Roles.Manager, Constants.AuthorizationConstants.Roles.Buyer);
                policy.RequireClaim("scope", Constants.AuthorizationConstants.Permissions.Read);
            });
            o.AddPolicy(Constants.AuthorizationConstants.Policies.Update, policy =>
            {
                policy.RequireRole(Constants.AuthorizationConstants.Roles.Manager);
                policy.RequireClaim("scope", Constants.AuthorizationConstants.Permissions.Update);
            });
            o.AddPolicy(Constants.AuthorizationConstants.Policies.Delete, policy =>
            {
                policy.RequireRole(Constants.AuthorizationConstants.Roles.Manager);
                policy.RequireClaim("scope", Constants.AuthorizationConstants.Permissions.Delete);
            });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
            app.UseSwagger(new SwaggerOptions());
            app.UseSwaggerUI();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHealthChecks("/health");
        app.UseHttpsRedirection();

        app.UseSwaggerUi3(settings =>
        {
            settings.Path = "/api";
            settings.DocumentPath = "/api/specification.json";
        });

        app.UseRouting();
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");
        });
    }
}
