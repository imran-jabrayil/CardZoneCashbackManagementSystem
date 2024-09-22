using Microsoft.EntityFrameworkCore;

using CardZoneCashbackManagementSystem.Database;
using CardZoneCashbackManagementSystem.Mappers;
using CardZoneCashbackManagementSystem.Models.Validators;
using CardZoneCashbackManagementSystem.Repositories;
using CardZoneCashbackManagementSystem.Repositories.Abstractions;
using CardZoneCashbackManagementSystem.Services;
using CardZoneCashbackManagementSystem.Services.Abstractions;
using FluentValidation;


var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders()
    .AddConsole();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryDb"));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllers();

builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCardRequestValidator>();

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