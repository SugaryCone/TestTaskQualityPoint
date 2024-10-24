using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Runtime;
using TestsProd;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddControllers();

builder.Host.UseSerilog(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Creating a CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});


builder.Services.AddAutoMapper(typeof(AddressProfile));
builder.Services.Configure<APISettings>
        (builder.Configuration.GetSection("APISettings"));

builder.Services.AddHttpClient();
//registering a new service
builder.Services.AddTransient<ICleanAddressService, CleanAddressClient>(); 

builder.Services.AddTransient<ExceptionHandlingMiddleware>();


var app = builder.Build();
app.UseCors("AllowAll");
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapSwagger().RequireAuthorization();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



app.Run();
