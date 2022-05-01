using Orbit.Application.Services;
using Orbit.Application.Shared;
using Orbit.Models.Repositories;
using Orbit.NHibernate;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var connectionString = builder.Configuration["DevelopmentConnection"];
var productionDataPath = builder.Configuration["ProductionDataPath"];

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient(x => new ProductionDataPath(productionDataPath));
builder.Services.AddSingleton(x => new ProductionDataService());
builder.Services.AddSingleton(x => new DBContext(connectionString));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
        builder.AllowCredentials();
    });
});
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
