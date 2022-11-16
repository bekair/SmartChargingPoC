using SmartChargingPoC.Business.Services;
using SmartChargingPoC.Business.Services.Interfaces;
using SmartChargingPoC.Core.Constants;
using SmartChargingPoC.DataAccess.Contexts;
using SmartChargingPoC.DataAccess.Repositories.Contexts.SmartCharging.Implementations;
using SmartChargingPoC.DataAccess.Repositories.Contexts.SmartCharging.Implementations.Interfaces;
using SmartChargingPoC.DataAccess.UnitOfWorks;
using SmartChargingPoC.DataAccess.UnitOfWorks.Base;
using SmartChargingPoC.WebAPI.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<SmartChargingContext>(options => options.UseInMemoryDatabase(ApiConstants.General.SmartChargingDbName));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Repository Injections
builder.Services.AddScoped<IConnectorRepository, ConnectorRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IChargeStationRepository, ChargeStationRepository>();

//Service Injections
builder.Services.AddScoped<IConnectorService, ConnectorService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IChargeStationService, ChargeStationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseMiddleware<CustomExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();
