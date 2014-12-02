$version = $args[0]
$formats = @("Html", "Dhtml", "Word", "Excel", "JSON")
$PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent

Try
{
    foreach ($format in $formats)
    {
        # Setup variables
        $FeatureDirectory = $PSScriptRoot + "\src\Pickles\Examples"
        $OutputDirectory = $PSScriptRoot + "\Output\" + $format
        $DocumentationFormat = $format
        $SystemUnderTestName = "Pickles"
        $SystemUnderTestVersion = $version
        $TestResultsFormat = ""
        $TestResultsFile = ""

        # Import the Pickles-comandlet
        $pathToModule = ".\deploy\pickles-" + $version + "\powershell\PicklesDoc.Pickles.PowerShell.dll"
        Import-Module $pathToModule

        # Call pickles
        Pickle-Features -FeatureDirectory $FeatureDirectory  `
                        -OutputDirectory $OutputDirectory  `
                        -DocumentationFormat $DocumentationFormat `
                        -SystemUnderTestName $SystemUnderTestName  `
                        -SystemUnderTestVersion $SystemUnderTestVersion  `
                        -TestResultsFormat $TestResultsFormat  `
                        -TestResultsFile $TestResultsFile
    }

    exit 0
}
Catch
{
    exit 1
}
