using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Samhammer.AspNetCore.HealthChecks.FilesystemDirectory
{
    public class DirectoryHealthCheck : IHealthCheck
    {
        private readonly DirectoryHealthCheckOptions options;

        public DirectoryHealthCheck(DirectoryHealthCheckOptions options)
        {
            this.options = options;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(options.DirectoryPath))
            {
                return options.FailForEmptyPath
                    ? HealthCheckResult.Unhealthy("No path configured")
                    : HealthCheckResult.Healthy("No path configured");
            }

            var directoryExists = Directory.Exists(options.DirectoryPath);
            if (!directoryExists)
            {
                return HealthCheckResult.Unhealthy($"directory {options.DirectoryPath} does not exist");
            }

            if (options.TryWrite)
            {
                var wasWriteSuccessful = TryCreateTestFile(out var writeCheckResult);
                if (!wasWriteSuccessful)
                {
                    return writeCheckResult!.Value;
                }
            }

            return HealthCheckResult.Healthy("ok");
        }

        private bool TryCreateTestFile(out HealthCheckResult? checkResult)
        {
            try
            {
                var testFileName = Guid.NewGuid() + ".health";

                // Write test file
                var filePath = Path.Combine(options.DirectoryPath, testFileName);
                File.WriteAllText(filePath, "test");

                // Check existence
                if (!File.Exists(filePath))
                {
                    checkResult = HealthCheckResult.Unhealthy($"written test file '{filePath}' does not exist");
                    return false;
                }

                // Cleanup
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                checkResult = HealthCheckResult.Unhealthy($"file write failed with '{ex.GetType().Name}' and message '{ex.Message}'");
                return false;
            }

            checkResult = null;
            return true;
        }
    }
}
