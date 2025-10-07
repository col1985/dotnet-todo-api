using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Reflection;

namespace TodoApi.App
{
  public class AppVersionHealthCheck : IHealthCheck
  {
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
      try
      {
        var version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString();

        if (string.IsNullOrEmpty(version))
        {
          return Task.FromResult(
              HealthCheckResult.Unhealthy("Application version not found."));
        }

        return Task.FromResult(
            HealthCheckResult.Healthy(
                "Application is healthy.",
                new Dictionary<string, object>
                {
                          { "version", version }
                }));
      }
      catch (Exception ex)
      {
        return Task.FromResult(
            HealthCheckResult.Unhealthy("Failed to get application version.", ex));
      }
    }
  }
}
