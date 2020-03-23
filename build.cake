#tool "nuget:?package=xunit.runner.console&version=2.2.0"

var target = Argument<string>("Target");
var configuration = Argument<string>("Configuration", "Release");
var outputDirectory = Argument<string>("OutputDirectory", "Publish");
var nugetApiKey = EnvironmentVariable("NUGET_API_KEY");

var packOptions = new DotNetCorePackSettings {
    ArgumentCustomization = args => args.Append("/p:TreatWarningsAsErros=true"),
    Configuration = configuration,
    OutputDirectory = outputDirectory,
};

Task("Clean")
    .Does(() => CleanDirectory(outputDirectory));

Task("Pack:Path")
    .IsDependentOn("Tests:Path")
    .Does(() =>  DotNetCorePack("CakeToolBox.Path", packOptions));

Task("Pack:IO")
    .Does(() =>  DotNetCorePack("CakeToolBox.IO", packOptions));

Task("Pack:Parameters")
    .IsDependentOn("Tests:Parameters")
    .Does(() =>  DotNetCorePack("CakeToolBox.Parameters", packOptions));

Task("Pack")
    .IsDependentOn("Clean")
    .IsDependentOn("Pack:Path")
    .IsDependentOn("Pack:IO")
    .IsDependentOn("Pack:Parameters")
    .Does(() =>  DotNetCorePack("CakeToolBox.Parameters", packOptions));

Task("Publish")
    .IsDependentOn("Pack")
    .Does(() => NuGetPush(GetFiles(System.IO.Path.Combine(outputDirectory, "*.nupkg")), new NuGetPushSettings {
        ApiKey = nugetApiKey,
        Source = "https://www.nuget.org",
		SkipDuplicate = true,
    }));

Task("Tests")
    .IsDependentOn("Tests:Path")
    .IsDependentOn("Tests:Parameters");

Task("Tests:Path")
    .Does(() => DotNetCoreTest("CakeToolBox.Path.Tests"));

Task("Tests:Parameters")
    .Does(() => DotNetCoreTest("CakeToolBox.Parameters.Tests"));

RunTarget(target);