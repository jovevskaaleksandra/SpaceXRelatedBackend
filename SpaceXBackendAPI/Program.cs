using Microsoft.EntityFrameworkCore;
using SpaceXBackend.DataLayer.Data;
using SpaceXBackend.Services.Implementations;
using SpaceXBackend.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SpaceXDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("SpaceXDbConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("SpaceXDbConnection"))));

// Add services to the container.
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);


builder.Services.AddControllers();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<ISpaceXService, SpaceXService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("SpaceX", c =>
{
    c.BaseAddress = new Uri("https://api.spacexdata.com/v5/");
    c.DefaultRequestHeaders.UserAgent.ParseAdd("SpaceXBackend/1.0");
});

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
