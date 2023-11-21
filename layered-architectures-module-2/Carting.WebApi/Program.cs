using System.Reflection;
using Asp.Versioning;
using AutoMapper;
using Carting.BLL.Clients;
using Carting.BLL.Services.Implementations;
using Carting.BLL.Services.Interfaces;
using Carting.BLL.Validators;
using Carting.DAL.Repositories.Implementations;
using Carting.DAL.Repositories.Interfaces;
using Carting.WebApi.BackgroundServices;
using Carting.WebApi.Configuration;
using Carting.WebApi.Filters;
using FluentValidation;
using Messaging.RabbitMq.Client;
using Messaging.RabbitMq.Client.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.Development.json")
    .Build();

builder.Services.AddSingleton<IConfiguration>(configuration);
// mapping
var mapperConfiguration = new MapperConfiguration(mc =>
{
    mc.AddProfile(new Carting.BLL.MappingProfile());
    mc.AddProfile(new Carting.WebApi.MappingProfile());
});
builder.Services.AddSingleton(mapperConfiguration.CreateMapper());
// validators
builder.Services.AddScoped<IValidator<Carting.BLL.Models.CartItem>, CartItemValidator>();
// repositories
builder.Services.AddScoped<ICartRepository, CartRepository>();
// services
builder.Services.AddScoped<ICartService, CartService>();
// databases
var mongoConfig = configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();
builder.Services.AddScoped<IMongoDatabase>(_ => new MongoClient(mongoConfig.ConnectionString).GetDatabase(mongoConfig.DbName));

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo { Title = "Carting API v1", Version = "v1" });
    o.SwaggerDoc("v2", new OpenApiInfo { Title = "Carting API v2", Version = "v2" });
    o.DocInclusionPredicate((docName, apiDesc) =>
    {
        if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo))
            return false;
        // First select by the versions specified at each action
        var versions = methodInfo
            .GetCustomAttributes<MapToApiVersionAttribute>()
            .SelectMany(attr => attr.Versions);
        // If there are none, consider the versions specified at the controller
        if (!versions.Any())
        {
            versions = methodInfo.DeclaringType!
                .GetCustomAttributes<ApiVersionAttribute>()
                .SelectMany(attr => attr.Versions);
        }
        if (!versions.Any(v => $"v{v}" == docName))
            return false;
        // Resolve the {version} parameter to a fixed path
        if (apiDesc.RelativePath?.StartsWith("api/v{version}/") == true)
        {
            apiDesc.RelativePath = apiDesc.RelativePath.Replace("api/v{version}/", $"api/{docName}/");
            var versionParam = apiDesc.ParameterDescriptions
                .SingleOrDefault(p => p.Name == "version" && p.Source.Id == "Path");
            if (versionParam != null)
                apiDesc.ParameterDescriptions.Remove(versionParam);
        }
        return true;
    });
    // Optionally, include XML comments for your controllers and models if you have XML documentation enabled.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    o.IncludeXmlComments(xmlPath);
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

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1);
}).AddMvc();

builder.Services.AddSingleton(builder.Configuration.GetSection(nameof(RabbitMqConfig)).Get<RabbitMqConfig>());
builder.Services.AddScoped<IConnection>(x => new ConnectionFactory() { HostName = "localhost" }.CreateConnection());
builder.Services.AddScoped<IRabbitMqClient, CartingRabbitMqClient>();

builder.Services.AddHostedService<MessageProcessingService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:5001";

        options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
        options.TokenValidationParameters.ValidateAudience = false;
    })
    .AddOpenIdConnect("oidc", o =>
    {
        o.Authority = "https://localhost:5001";
        o.ClientId = "postman";
        o.GetClaimsFromUserInfoEndpoint = true;
        o.Scope.Add("profile");
    });
builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "Carting API v1");
        o.SwaggerEndpoint("/swagger/v2/swagger.json", "Carting API v2");
    });
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();