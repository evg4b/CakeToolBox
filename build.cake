var target = Argument<string>("Target");
var configuration = Argument<string>("Configuration", "Release");
var outputDirectory = Argument<string>("OutputDirectory", "Publish");

var packOptions = new DotNetCorePackSettings {
    ArgumentCustomization = args => args.Append("/p:TreatWarningsAsErros=true"),
    Configuration = configuration,
    OutputDirectory = outputDirectory,
};

Task("Clean")
    .Does(() => CleanDirectory(outputDirectory));

Task("Pack:Path")
    .Does(() =>  DotNetCorePack("CakeToolBox.Path", packOptions));

Task("Pack:Parameters")
    .Does(() =>  DotNetCorePack("CakeToolBox.Parameters", packOptions));

Task("Pack:Environment")
    .Does(() =>  DotNetCorePack("CakeToolBox.Environment", packOptions));

Task("Pack")
    .IsDependentOn("Clean")
    .IsDependentOn("Pack:Path")
    .IsDependentOn("Pack:Parameters")
    .IsDependentOn("Pack:Environment");

Task("Publish:NUGET")
    .Does(() => {
        NuGetSetApiKey(EnvironmentVariable("NUGET_API_KEY"), "https://www.nuget.org");
        NuGetPush(GetFiles(System.IO.Path.Combine(outputDirectory, "*.nupkg")), new NuGetPushSettings {
            Source = "https://www.nuget.org",
            SkipDuplicate = true,
        });
    });

Task("Publish:GITHUB")
    .Does(() => {
        NuGetSetApiKey(EnvironmentVariable("GITHUB_API_KEY"), "https://nuget.pkg.github.com/evg4b/index.json");
        NuGetPush(GetFiles(System.IO.Path.Combine(outputDirectory, "*.nupkg")), new NuGetPushSettings {
            Source = "https://nuget.pkg.github.com/evg4b/index.json",
            SkipDuplicate = true,
        });
    });

Task("Publish")
	.IsDependentOn("Tests")
    .IsDependentOn("Pack")
    .IsDependentOn("Publish:NUGET")
    .IsDependentOn("Publish:GITHUB");

Task("Tests")
	.IsDependentOn("Tests:Internal")
    .IsDependentOn("Tests:Path")
    .IsDependentOn("Tests:Parameters");

Task("Tests:Path")
    .Does(() => DotNetCoreTest("CakeToolBox.Path.Tests"));

Task("Tests:Parameters")
    .Does(() => DotNetCoreTest("CakeToolBox.Parameters.Tests"));

Task("Tests:Internal")
    .Does(() => DotNetCoreTest("CakeToolBox.Internal.Tests"));

RunTarget(target);
