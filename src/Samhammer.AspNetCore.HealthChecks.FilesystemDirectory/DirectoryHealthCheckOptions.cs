namespace Samhammer.AspNetCore.HealthChecks.FilesystemDirectory
{
    public class DirectoryHealthCheckOptions
    {
        public string DirectoryPath { get; set; }

        public bool TryWrite { get; set; }

        public bool FailForEmptyPath { get; set; }
    }
}
