set projectName=Harry.Common

set version=1.0.0
set suffix=alpha6

set fullVersion=%version%-%suffix%

del /Q /S  ..\%projectName%\bin\ForNuget\*.*

dotnet restore
dotnet pack -c Release ../%projectName%  -o ../%projectName%/bin/ForNuget --version-suffix %suffix%
nuget.exe push ../%projectName%/bin/ForNuget/%projectName%.%fullVersion%.symbols.nupkg -Source https://www.nuget.org/api/v2/package

pause