<#
文件：build.ps1
用途：用于打包并推送至nuget服务器
#>
$path = Get-Location

$baseDate=[datetime]"03/28/2019"
$currentDate=$(Get-Date)
$interval=New-TimeSpan -Start $baseDate -End $currentDate
$days=$interval.Days
$hours=$interval.Hours
#版本号设置
#$version="$currentDate.Year-$days"
#$version="$(Get-Date -Format 'yyyy.M').1"
#$version="2022.3.22"
#在common.props文件中设置
$version="0.1.0-alpha1-$days"

Write-Host -Object $days

foreach($line in Get-Content .\projects.txt) {
    $projectName= "..\$line\$line.csproj"
    Write-Host $projectName

    <# 清理项目 #>
    if (Test-Path "..\$line\bin")
    {
        rm -Recurse -Force "..\$line\bin"
    }
    if (Test-Path "..\$line\obj")
    {
        rm -Recurse -Force "..\$line\obj"
    }

    <# 打包 #>
    $targetPath="..\..\..\..\Packages\$line"  
    dotnet pack --configuration Release --output $targetPath $projectName /p:Version=$version

    <# 推送至nuget服务器 #>
    #dotnet nuget push "$targetPath\$line.$version.nupkg"  -s https://api.nuget.org/v3/index.json --skip-duplicate 

    <# 清理项目 #>
    if (Test-Path "..\$line\bin")
    {
        rm -Recurse -Force "..\$line\bin"
    }
    if (Test-Path "..\$line\obj")
    {
        rm -Recurse -Force "..\$line\obj"
    }
}
