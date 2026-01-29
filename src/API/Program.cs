using Abysalto.Retail.API.Filters;
using Abysalto.Retail.API.Middleware;
using Abysalto.Retail.Mock;
using Abysalto.Retail.Modules.Cart.Application.Extensions;
using Abysalto.Retail.Modules.Cart.Contracts.Requests.Validation;
using Abysalto.Retail.Modules.Cart.Infrastructure.Data;
using Abysalto.Retail.Modules.Cart.Infrastructure.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// --- Logging ---
Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(builder.Configuration)
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
	.CreateLogger();

builder.Host.UseSerilog();

// --- Services ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Combine Controller and Filter registration here
builder.Services.AddControllers(options =>
{
	options.Filters.Add<ValidationFilter>();
})
.AddJsonOptions(options => {
	// Helpful for debugging GUIDs and Enums in Swagger
	options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
	options.SuppressModelStateInvalidFilter = true;
});

// --- Module Registrations ---
builder.Services.AddValidatorsFromAssemblyContaining<AddItemToCartRequestValidator>();
builder.Services.AddCartApplication();
builder.Services.AddCartInfrastructure(builder.Configuration);
builder.Services.AddScoped<IPriceService, PriceService>();

// --- Database Setup (Docker Friendly) ---
// If a connection string is provided in Environment Variables (Docker), use that.
// Otherwise, fall back to the local /db/ folder.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
	var dbFolder = Path.Combine(builder.Environment.ContentRootPath, "db");
	if (!Directory.Exists(dbFolder)) Directory.CreateDirectory(dbFolder);

	var dbPath = Path.Combine(dbFolder, "cart.db");
	connectionString = $"Data Source={dbPath}";
}

builder.Services.AddDbContext<CartDbContext>(options =>
	options.UseSqlite(connectionString)
);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();


app.UseAuthorization();
app.MapControllers();

try
{
	using var scope = app.Services.CreateScope();
	var db = scope.ServiceProvider.GetRequiredService<CartDbContext>();
	db.Database.Migrate();
}
catch (Exception ex)
{
	Log.Fatal(ex, "The Database Migration failed during startup.");
}

app.Run();