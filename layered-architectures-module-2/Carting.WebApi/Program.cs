using AutoMapper;
using Carting.BLL.Services.Implementations;
using Carting.BLL.Services.Interfaces;
using Carting.BLL.Validators;
using Carting.DAL.Repositories.Implementations;
using Carting.DAL.Repositories.Interfaces;
using Carting.WebApi.Configuration;
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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();