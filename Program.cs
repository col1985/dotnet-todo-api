using System.Reflection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using TodoApi.App;
using TodoApi.Repositories;


var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ITodoRepository, TodoRepository>();

// Add health checks and register a custom check
builder.Services.AddHealthChecks()
    .AddCheck<AppVersionHealthCheck>("app_version");

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

// Map the custom health check endpoint
app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    // Our custom writer will format the response
    ResponseWriter = WriteHealthCheckResponse
});

await app.RunAsync();


// Program.cs
// This method should be placed at the bottom of the Program.cs file
// or in its own static helper class.

static Task WriteHealthCheckResponse(HttpContext httpContext, HealthReport result)
{
    httpContext.Response.ContentType = "application/json";

    var payload = new
    {
        status = result.Status.ToString(),
        version = result.Entries.FirstOrDefault().Value.Data["version"]
    };

    return httpContext.Response.WriteAsJsonAsync(payload);
}



