using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Samhammer.AspNetCore.HealthChecks.FilesystemDirectory
{
    public static class DirectoryHealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddDirectory(
            this IHealthChecksBuilder builder,
            Action<DirectoryHealthCheckOptions> configure,
            string name = "directoryHealthCheck",
            HealthStatus? failureStatus = null,
            IEnumerable<string> tags = null,
            TimeSpan? timeout = null)
        {
            var options = new DirectoryHealthCheckOptions();
            configure(options);

            return builder.Add(new HealthCheckRegistration(
                name,
                _ => new DirectoryHealthCheck(options),
                failureStatus,
                tags,
                timeout));
        }

        public static IHealthChecksBuilder AddDirectory(
            this IHealthChecksBuilder builder,
            Func<IServiceProvider, DirectoryHealthCheckOptions> optionsProvider,
            string name = "directoryHealthCheck",
            HealthStatus? failureStatus = null,
            IEnumerable<string> tags = null,
            TimeSpan? timeout = null)
        {
            return builder.Add(
                new HealthCheckRegistration(
                    name,
                    sp =>
                    {
                        var options = optionsProvider(sp);
                        return new DirectoryHealthCheck(options);
                    },
                    failureStatus,
                    tags,
                    timeout));
        }
    }
}
