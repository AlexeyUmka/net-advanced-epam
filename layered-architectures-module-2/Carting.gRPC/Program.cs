using AutoMapper;
using Carting.BLL.Services.Implementations;
using Carting.BLL.Services.Interfaces;
using Carting.BLL.Validators;
using Carting.DAL.Repositories.Implementations;
using Carting.DAL.Repositories.Interfaces;
using Carting.gRPC.Configuration;
using Carting.gRPC.Services;
using FluentValidation;
using MongoDB.Driver;

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
    mc.AddProfile(new Carting.gRPC.MappingProfile());
});
builder.Services.AddSingleton(mapperConfiguration.CreateMapper());
// validators
builder.Services.AddScoped<IValidator<Carting.BLL.Models.CartItem>, CartItemValidator>();
// repositories
builder.Services.AddScoped<ICartRepository, CartRepository>();
// services
builder.Services.AddScoped<ICartService, CartService>();
// databases
var mongoConfig = configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>() ?? throw new ArgumentException("Mongo Connection string is not specified");;
builder.Services.AddScoped<IMongoDatabase>(_ => new MongoClient(mongoConfig.ConnectionString).GetDatabase(mongoConfig.DbName));

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<CartingService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();