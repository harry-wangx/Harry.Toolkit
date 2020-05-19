<#
�ļ���build.ps1
��;�����ڴ����������nuget������
#>
$path = Get-Location

$baseDate=[datetime]"03/28/2017"
$currentDate=$(Get-Date)
$interval=New-TimeSpan -Start $baseDate -End $currentDate
$days=$interval.Days
$hours=$interval.Hours

Write-Host -Object $days

foreach($line in Get-Content .\projects.txt) {
    $projectName= "..\$line\$line.csproj"
    Write-Host $projectName

    <# ������Ŀ #>
    if (Test-Path "..\$line\bin")
    {
        rm -Recurse -Force "..\$line\bin"
    }
    if (Test-Path "..\$line\obj")
    {
        rm -Recurse -Force "..\$line\obj"
    }

    <# ��� #>
    dotnet pack --configuration Release --output "..\$line\bin\nupkgs" $projectName /p:Version="0.1.0-alpha1-$days"

    <# ������nuget������ #>
    dotnet nuget push "..\$line\bin\nupkgs\*.nupkg"  -s https://api.nuget.org/v3/index.json --skip-duplicate 

    <# ������Ŀ #>
    if (Test-Path "..\$line\bin")
    {
        rm -Recurse -Force "..\$line\bin"
    }
    if (Test-Path "..\$line\obj")
    {
        rm -Recurse -Force "..\$line\obj"
    }
}
