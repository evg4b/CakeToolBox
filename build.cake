var target = Argument<string>("Target");
var configuration = Argument<string>("Configuration", "Release");
var outputDirectory = Argument<string>("OutputDirectory", "Publish");
var nugetApiKey = EnvironmentVariable("NUGET_API_KEY");

Task("Clean")
    .Does(() => CleanDirectory(outputDirectory));

Task("Path:Pack")
    .IsDependentOn("Clean")
    .Does(() =>  DotNetCorePack("CakeToolBox.Path", new DotNetCorePackSettings {
        Configuration = configuration,
        OutputDirectory = outputDirectory,
    }));

Task("Path:IO")
    .IsDependentOn("Clean")
    .Does(() =>  DotNetCorePack("CakeToolBox.IO", new DotNetCorePackSettings {
        Configuration = configuration,
        OutputDirectory = outputDirectory,
    }));

Task("Publish")
    .IsDependentOn("Path:Pack")
    .IsDependentOn("Path:IO")
    .Does(() => NuGetPush(GetFiles(System.IO.Path.Combine(outputDirectory, "*.nupkg")), new NuGetPushSettings {
        ApiKey = nugetApiKey,
        Source = "https://www.nuget.org"
    }));


RunTarget(target);