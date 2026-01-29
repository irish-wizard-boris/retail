using System;
using Abysalto.Retail.API.Filters;
using Abysalto.Retail.API.Middleware;
using Abysalto.Retail.Mock;
using Abysalto.Retail.Modules.Cart.Application.Extensions;
using Abysalto.Retail.Modules.Cart.Application.Services;
using Abysalto.Retail.Modules.Cart.Contracts.Requests.Validation;
using Abysalto.Retail.Modules.Cart.Infrastructure.Data;
using Abysalto.Retail.Modules.Cart.Infrastructure.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(); // registers controller services

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
	// Disables the default [ApiController] validation so your Filter handles it
	options.SuppressModelStateInvalidFilter = true;
});


// FluentValidationFilter
builder.Services.AddControllers(options =>
{
	options.Filters.Add<ValidationFilter>();
});

builder.Services.AddValidatorsFromAssemblyContaining<AddItemToCartRequestValidator>();

builder.Services.AddCartApplication();
builder.Services.AddCartInfrastructure(builder.Configuration);

// Configure Serilog before building the app
Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(builder.Configuration) // optional: read settings from appsettings.json
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
	.CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddScoped<IPriceService, PriceService>();


// Build DB path
var dbFolder = Path.Combine(builder.Environment.ContentRootPath, "db");
Directory.CreateDirectory(dbFolder);

var dbPath = Path.Combine(dbFolder, "cart.db");
var connectionString = $"Data Source={dbPath}";
// Register DbContext
builder.Services.AddDbContext<CartDbContext>(options =>
	options.UseSqlite(connectionString)
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

//app.UseAuthorization();
app.MapControllers();
app.UseHttpsRedirection();


using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<CartDbContext>();
	db.Database.Migrate();
}


app.Run();



