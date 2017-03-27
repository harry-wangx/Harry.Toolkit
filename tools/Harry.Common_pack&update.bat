set projectName=Harry.Common

set version=1.0.0
set suffix=alpha2

set fullVersion=%version%-%suffix%

del /Q /S  ..\src\%projectName%\bin\ForNuget\*.*

dotnet restore
dotnet pack -c Release ../src/%projectName%  -o ../src/%projectName%/bin/ForNuget --version-suffix %suffix%
nuget.exe push ../src/%projectName%/bin/ForNuget/%projectName%.%fullVersion%.symbols.nupkg -Source https://www.nuget.org/api/v2/package

pause