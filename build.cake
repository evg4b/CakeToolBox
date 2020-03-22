var target = Argument<string>("Target");
var configuration = Argument<string>("Configuration", "Release");
var outputDirectory = Argument<string>("OutputDirectory", "Publish");
var nugetApiKey = EnvironmentVariable("NUGET_API_KEY");

Task("Clean")
    .Does(() => CleanDirectory(outputDirectory));

Task("Pack:Path")
    .Does(() =>  DotNetCorePack("CakeToolBox.Path", new DotNetCorePackSettings {
        Configuration = configuration,
        OutputDirectory = outputDirectory,
    }));

Task("Pack:IO")
    .Does(() =>  DotNetCorePack("CakeToolBox.IO", new DotNetCorePackSettings {
        Configuration = configuration,
        OutputDirectory = outputDirectory,
    }));

Task("Pack:Parameters")
    .Does(() =>  DotNetCorePack("CakeToolBox.Parameters", new DotNetCorePackSettings {
        Configuration = configuration,
        OutputDirectory = outputDirectory,
    }));

Task("Pack")
    .IsDependentOn("Clean")
    .IsDependentOn("Pack:Path")
    .IsDependentOn("Pack:IO")
    .IsDependentOn("Pack:Parameters")
    .Does(() =>  DotNetCorePack("CakeToolBox.Parameters", new DotNetCorePackSettings {
        Configuration = configuration,
        OutputDirectory = outputDirectory,
    }));

Task("Publish")
    .IsDependentOn("Pack")
    .Does(() => NuGetPush(GetFiles(System.IO.Path.Combine(outputDirectory, "*.nupkg")), new NuGetPushSettings {
        ApiKey = nugetApiKey,
        Source = "https://www.nuget.org",
		SkipDuplicate = true,
    }));


RunTarget(target);