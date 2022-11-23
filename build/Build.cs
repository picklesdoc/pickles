using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;
using Microsoft.Extensions.Configuration;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild
{
    public static int Main()
    {
        if (IsLocalBuild)
        {
            config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddUserSecrets<Build>()
                .Build();
        }

        return Execute<Build>(x => x.PublishNuGet);
    }

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    //readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    readonly Configuration Configuration = Configuration.Release;

    static IConfiguration config;
    [Solution] readonly Solution Solution;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    AbsolutePath PublishDirectory => ArtifactsDirectory / "publish";
    AbsolutePath LibraryDirectory => PublishDirectory / "library";
    AbsolutePath CommandLineDirectory => PublishDirectory / "exe";
    AbsolutePath MsBuildDirectory => PublishDirectory / "msbuild";
    AbsolutePath PowerShellDirectory => PublishDirectory / "powershell";
    AbsolutePath DeployDirectory => ArtifactsDirectory / "deploy";

    AbsolutePath OutputDirectory => RootDirectory / "Output";
    AbsolutePath DocsOutputDirectory => RootDirectory/ "docs" / "Output";

    String AssemblyProduct = "Pickles";
    String AssemblyCompany = "Pickles";
    String Version = "4.0.3";
    String Copyright = "Copyright (c) Jeffrey Cameron 2010-2012, PicklesDoc 2012-present";
    String NuGetApiKey = "";
    //String tfm = "net6.0"; //currently only supporting one target framework for executables and tools
    String winRuntime = "win-x64"; //use for tools that are windows only

    Target Clean => _ => _
        .Before(Test)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
            EnsureCleanDirectory(DeployDirectory);
            EnsureCleanDirectory(DeployDirectory / "zip");
            EnsureCleanDirectory(DeployDirectory / "nuget");
            EnsureCleanDirectory(OutputDirectory);
            EnsureCleanDirectory(DocsOutputDirectory);
        });

    Target Test => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(RootDirectory / "Pickles.sln")
                .SetLoggers("trx;LogFileName=TestResults.xml")
            );
        });

    Target Publish => _ => _
        .DependsOn(Test)
        .Executes(() =>
        {
            DotNetPublish(p => p
                .SetProject(RootDirectory / "src" / "Pickles")
                .SetConfiguration(Configuration)
                .SetVersion(Version)
                .SetSelfContained(false)
                .SetOutput(LibraryDirectory)
            );

            UpdatePackageId(RootDirectory / "src" / "Pickles.CommandLine" / "Pickles.CommandLine.csproj", "Pickles.CommandLine", "");

            DotNetPublish(p => p
                .SetProject(RootDirectory / "src" / "Pickles.CommandLine")
                .SetConfiguration(Configuration)
                .SetVersion(Version)
                .SetRuntime(winRuntime)
                //.SetFramework(tfm)
                .SetSelfContained(false)
                .SetOutput(CommandLineDirectory / "win-framework")
            );
            
            foreach (var rt in new[] { "win-x64", "osx-x64", "linux-x64" })
            {
               string platform = GetPlatformFromRuntime(rt);

                UpdatePackageId(RootDirectory / "src" / "Pickles.CommandLine" / "Pickles.CommandLine.csproj", "Pickles.CommandLine", platform);

                DotNetPublish(p => p
                    .SetProject(RootDirectory / "src" / "Pickles.CommandLine")
                    .SetConfiguration(Configuration)
                    .SetVersion(Version)
                    .SetRuntime(rt)
                    //.SetFramework(tfm)
                    .SetSelfContained(false)
                    .SetOutput(CommandLineDirectory / platform)
                );

                ZipFile.CreateFromDirectory(CommandLineDirectory / platform,
                       DeployDirectory / "zip" / "Pickles-exe-" + rt + "-" + Version + ".zip");
            }

            //reset to the base packageid so it will be prepped for next test run
            UpdatePackageId(RootDirectory / "src" / "Pickles.CommandLine" / "Pickles.CommandLine.csproj", "Pickles.CommandLine", "");

            DotNetPublish(p => p
                .SetProject(RootDirectory / "src" / "Pickles.MsBuild")
                .SetConfiguration(Configuration)
                .SetVersion(Version)
                    .SetRuntime(winRuntime)
                    .SetOutput(MsBuildDirectory)
            );

            ZipFile.CreateFromDirectory(MsBuildDirectory,
               DeployDirectory / "zip" / "Pickles-msbuild-" + Version + ".zip");

            DotNetPublish(p => p
                .SetProject(RootDirectory / "src" / "Pickles.PowerShell")
                .SetConfiguration(Configuration)
                .SetVersion(Version)
                .SetRuntime(winRuntime)
                .SetOutput(PowerShellDirectory)
            );

            ZipFile.CreateFromDirectory(PowerShellDirectory,
               DeployDirectory / "zip" / "Pickles-powershell-" + Version + ".zip");
        });

    private string GetPlatformFromRuntime(string rt)
    {

        if (rt.Contains("osx"))
        {
            return "osx";
        }
        else if (rt.Contains("linux"))
        {
            return "linux";
        }
        else if (rt.Contains("win"))
        {
            return "win";
        }

        return "not supported";
    }
    private void UpdatePackageId(AbsolutePath projectFilePathAndName, string basePackageId, string platform = "")
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(projectFilePathAndName);

        XmlNode packageIdNode = doc.SelectSingleNode("/Project/PropertyGroup/PackageId");

        string packageId = basePackageId;

        if(!String.IsNullOrEmpty(platform))
        {
            packageId += "." + platform;
        } 
        
        packageIdNode.InnerText = packageId;

        doc.Save(projectFilePathAndName);
    }

    Target GenerateSampleOutput => _ => _
        .DependsOn(Publish)
        .Executes(() =>
        {
            string runtime = String.Empty;
            var formats = new List<string> {"Html", "Dhtml", "Word", "Excel", "JSON", "Cucumber", "Markdown"};
            string exampleSource = SourceDirectory / "Examples";
            string outputFolder = string.Empty;

            bool isMac = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

            if (isMac)
            {
                runtime = "osx-x64";
            }
            else if (isWindows)
            {
                runtime = "win10-x86";
            }
            else if (isLinux)
            {
                runtime = "linux-x64";
            }

            foreach (var format in formats)
            {
                outputFolder = OutputDirectory / format;
                string platform = GetPlatformFromRuntime(runtime);

                ProcessStartInfo processStartInfo =
                    new ProcessStartInfo(PublishDirectory / "exe" / platform / "Pickles",
                        $"-f={exampleSource} -o={outputFolder} -df={format} --sn=Pickles --sv={Version}");

                processStartInfo.CreateNoWindow = false;
                processStartInfo.UseShellExecute = false;
                Process p = Process.Start(processStartInfo);
                p.WaitForExit();
                Console.WriteLine(p.ExitCode);

                //TODO Repeat with experimental features
                //TODO Repeat with other runners
            }

            //Copy sample output to docs folder
            CopyFilesRecursively(new DirectoryInfo(RootDirectory / "Output"),
                new DirectoryInfo(RootDirectory / "docs" / "Output"));

            //Update version in docs index
            var index = File.ReadAllText(RootDirectory / "docs" / "index_template.html");
            index = index.Replace("VERSION_PLACEHOLDER", Version);
            File.WriteAllText(RootDirectory / "docs" / "index.html", index);
        });

    public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
    {
        foreach (DirectoryInfo dir in source.GetDirectories())
            CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
        foreach (FileInfo file in source.GetFiles())
            file.CopyTo(Path.Combine(target.FullName, file.Name));
    }

    Target Pack => _ => _
        .DependsOn(GenerateSampleOutput)
        .Executes(() =>
        {
            var fileLibrary = new FileInfo(RootDirectory / "src" / "Pickles" / "nupkg" / $"Pickles.Library.{Version}.nupkg").CopyTo(DeployDirectory / "nuget" / $"Pickles.Library.{Version}.nupkg");

            var fileCL = new FileInfo(RootDirectory / "src" / "Pickles.CommandLine" / "nupkg" / $"Pickles.CommandLine.{Version}.nupkg").CopyTo(DeployDirectory / "nuget" / $"Pickles.CommandLine.{Version}.nupkg");
            var fileWin = new FileInfo(RootDirectory / "src" / "Pickles.CommandLine" / "nupkg" / $"Pickles.CommandLine.win.{Version}.nupkg").CopyTo(DeployDirectory / "nuget" / $"Pickles.CommandLine.win.{Version}.nupkg");
            var fileOsx = new FileInfo(RootDirectory / "src" / "Pickles.CommandLine" / "nupkg" / $"Pickles.CommandLine.osx.{Version}.nupkg").CopyTo(DeployDirectory / "nuget" / $"Pickles.CommandLine.osx.{Version}.nupkg");
            var fileLinux = new FileInfo(RootDirectory / "src" / "Pickles.CommandLine" / "nupkg" / $"Pickles.CommandLine.linux.{Version}.nupkg").CopyTo(DeployDirectory / "nuget" / $"Pickles.CommandLine.linux.{Version}.nupkg");

            var filePowerShell = new FileInfo(RootDirectory / "src" / "Pickles.PowerShell" / "nupkg" / $"Pickles.{Version}.nupkg").CopyTo(DeployDirectory / "nuget" / $"Pickles.{Version}.nupkg");

            var fileMsBuild = new FileInfo(RootDirectory / "src" / "Pickles.MSBuild" / "nupkg" / $"Pickles.MSBuild.{Version}.nupkg").CopyTo(DeployDirectory / "nuget" / $"Pickles.MSBuild.{Version}.nupkg");
        });

    Target PublishNuGet => _ => _
        .DependsOn(Pack)
        .Executes(() =>
        {
            if (IsLocalBuild)
            {
                NuGetApiKey = config["NugetApiKey"];
            }

            NuGetTasks.NuGetPush(s => s
                .SetSource("https://www.nuget.org/api/v2/package")
                .SetApiKey(NuGetApiKey)
                .SetTargetPath(DeployDirectory / "nuget" / $"Pickles.Library.{Version}.nupkg")
            );

            NuGetTasks.NuGetPush(s => s
                .SetSource("https://www.nuget.org/api/v2/package")
                .SetApiKey(NuGetApiKey)
                .SetTargetPath(DeployDirectory / "nuget" / $"Pickles.{Version}.nupkg")
            );

            NuGetTasks.NuGetPush(s => s
                .SetSource("https://www.nuget.org/api/v2/package")
                .SetApiKey(NuGetApiKey)
                .SetTargetPath(DeployDirectory / "nuget" / $"Pickles.CommandLine.{Version}.nupkg")
            );

            NuGetTasks.NuGetPush(s => s
                .SetSource("https://www.nuget.org/api/v2/package")
                .SetApiKey(NuGetApiKey)
                .SetTargetPath(DeployDirectory / "nuget" / $"Pickles.CommandLine.win.{Version}.nupkg")
            );

            NuGetTasks.NuGetPush(s => s
                .SetSource("https://www.nuget.org/api/v2/package")
                .SetApiKey(NuGetApiKey)
                .SetTargetPath(DeployDirectory / "nuget" / $"Pickles.CommandLine.osx.{Version}.nupkg")
            );

            NuGetTasks.NuGetPush(s => s
                .SetSource("https://www.nuget.org/api/v2/package")
                .SetApiKey(NuGetApiKey)
                .SetTargetPath(DeployDirectory / "nuget" / $"Pickles.CommandLine.linux.{Version}.nupkg")
            );

            NuGetTasks.NuGetPush(s => s
                .SetSource("https://www.nuget.org/api/v2/package")
                .SetApiKey(NuGetApiKey)
                .SetTargetPath(DeployDirectory / "nuget" / $"Pickles.MsBuild.{Version}.nupkg")
            );
        });
}

