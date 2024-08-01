using Serilog;
using Serilog.Events;
using TestUserApi.Interfaces;
using TestUserApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSerilog((services, lc) => lc
    .ReadFrom.Configuration(builder.Configuration)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();
builder.Services.AddTransient<IUserRepositoryService, UserRepositoryService>();
builder.Services.AddTransient<IGroupRepositoryService, GroupRepositoryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();