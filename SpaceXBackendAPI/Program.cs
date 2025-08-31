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

builder.Services.AddHttpClient("SpaceX", (sp,c) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var baseUrl = config["SpaceXAPIEndpoint:BaseUrl"];

    if (string.IsNullOrWhiteSpace(baseUrl))
        throw new InvalidOperationException("SpaceXAPIEndpoint:BaseUrl must be configured in appsettings.json");

    c.BaseAddress = new Uri(baseUrl);
    c.DefaultRequestHeaders.UserAgent.ParseAdd("SpaceXBackend/1.0");
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

//app.UseHttpsRedirection();
app.UseCors("AllowAngular");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
