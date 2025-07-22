# Samhammer.AspNetCore.HealthChecks

A collection of health checks for AspDotnet core

## Usage

### FilesystemDirectory
[nuget-image-filesystemdirectory]:https://img.shields.io/nuget/v/Samhammer.AspNetCore.HealthChecks.FilesystemDirectory?label=Samhammer.AspNetCore.HealthChecks.FilesystemDirectory
[nuget-url-filesystemdirectory]:https://www.nuget.org/packages/Samhammer.AspNetCore.HealthChecks.FilesystemDirectory/

This check verifies if a directory exists. Also it can test write access by creating and deleting a test file (Filename: "randomGuid.health").

- Add nuget package [![Nuget][nuget-image-filesystemdirectory]][nuget-url-filesystemdirectory]
- Register in Program.cs as shown below

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddHealthChecks()
    .AddDirectory(o =>
    {
        o.DirectoryPath = "/opt/app/mydir"; // The path to a directory
        o.FailForEmptyPath = true // If set to true the check fails if the path is null or empty
        o.TryWrite = true; // If set to true the check creates a test file to verify write access
    });
```

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddHealthChecks()
    .AddDirectory((sp) =>
    {
        var path = sp.GetService<IOptions<FileImportOptions>>().Value.DirectoryPath;
        return new DirectoryHealthCheckOptions
        {
          DirectoryPath = path, // The path to a directory
          FailForEmptyPath = true, // If set to true the check fails if the path is null or empty
          TryWrite = true // If set to true the check creates a test file to verify write access
        }
    });
```

## How to publish package
- Create a tag and let the github action do the publishing for you
