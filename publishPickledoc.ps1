# Setup variables
$Root = "C:\Users\X107107\.jenkins\workspace\AvidXchange.POC.Test\"
$FeatureDirectory = "$Root\framework\Features"
$OutputDirectory = "$Root\framework\Reports\Livingdocumentation"
$DocumentationFormat = "dhtml"
$SystemUnderTestName = "AvidXchange"
$SystemUnderTestVersion = "1.3.42"
$TestResultsFormat = "nunit3"
$TestResultsFile = "$Root\TestResult.xml"

# Import the Pickles-comandlet
Import-Module $Root\packages\Pickles.2.20.1\tools\PicklesDoc.Pickles.PowerShell.dll

# Call pickles
Pickle-Features -FeatureDirectory $FeatureDirectory  `
                -OutputDirectory $OutputDirectory  `
                -DocumentationFormat $DocumentationFormat `
                -SystemUnderTestName $SystemUnderTestName  `
                -SystemUnderTestVersion $SystemUnderTestVersion `
                -TestResultsFormat $TestResultsFormat  `
                -TestResultsFile $TestResultsFile
