using System.Reflection;
using CardZoneCashbackManagementSystem.Clients;
using CardZoneCashbackManagementSystem.Clients.Abstractions;
using CardZoneCashbackManagementSystem.Clients.Settings;
using CardZoneCashbackManagementSystem.Database;
using CardZoneCashbackManagementSystem.Jobs;
using CardZoneCashbackManagementSystem.Mappers;
using CardZoneCashbackManagementSystem.Repositories;
using CardZoneCashbackManagementSystem.Repositories.Abstractions;
using CardZoneCashbackManagementSystem.Services;
using CardZoneCashbackManagementSystem.Services.Abstractions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders()
    .AddConsole();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryDb"));
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddHttpClient<ICashbackClient, CashbackClient>();
builder.Services.Configure<CashbackClientSettings>(builder.Configuration.GetSection(nameof(CashbackClient)));

builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey(nameof(CashbackApplierJob));
    q.AddJob<CashbackApplierJob>(opts => opts.WithIdentity(jobKey));

    var cronSchedule = builder.Configuration[$"Jobs:{nameof(CashbackApplierJob)}:CronSchedule"];
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity($"{nameof(CashbackApplierJob)}Trigger")
        .WithCronSchedule(cronSchedule ?? throw new NullReferenceException(nameof(cronSchedule))));
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

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