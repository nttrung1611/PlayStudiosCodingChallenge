using Microsoft.EntityFrameworkCore;
using PlayStudiosCodingChallenge.Data;
using PlayStudiosCodingChallenge.Data.Repositories;
using PlayStudiosCodingChallenge.Services;
using PlayStudiosCodingChallenge.Services.ServiceModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database config
builder.Services.AddDbContext<QuestStateDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnection")),
    ServiceLifetime.Scoped);

// Quest variables config
builder.Services.Configure<QuestConfigurationOptions>(
    builder.Configuration.GetSection(QuestConfigurationOptions.QuestConfiguration));

// Repository registration
builder.Services.AddScoped<IPlayerQuestStateRepository, PlayerQuestStateRepository>();

// Service registration
builder.Services.AddScoped<IPlayerQuestService, PlayerQuestService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
