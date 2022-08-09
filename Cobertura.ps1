Write-Output "Create Code Coverage Report"

$TESTING_PATH = [IO.Path]::Combine($PSScriptRoot, 'test')

$TESTING_FOLDERS = (Get-ChildItem -Path $TESTING_PATH -Directory).name
foreach ($TESTING_FOLDER in $TESTING_FOLDERS)
{
	$PATH = [IO.Path]::Combine($PSScriptRoot, 'test', $TESTING_FOLDER, 'TestResults')
	if (Test-Path -Path $PATH) {
		Remove-Item -Recurse $PATH
	}
}

dotnet restore
dotnet test -c Release --collect:"XPlat Code Coverage" --logger:"html;"

$coberturaFiles = ""
Get-ChildItem -Path $TESTING_PATH -recurse -Filter *.xml -File -Name | ForEach-Object { 
    $coberturaFiles += Join-Path $TESTING_PATH $_
	$coberturaFiles += ";"
}

reportgenerator -reports:"$coberturaFiles" -targetdir:"codecoverage" -reporttypes:Html