# @ECHO OFF
# powershell -file build.ps1

# setlocal
# set EnableNuGetPackageRestore=true
# set logOptions=/flp:Summary;Verbosity=normal;LogFile=msbuild.log /flp1:warningsonly;logfile=msbuild.wrn /flp2:errorsonly;logfile=msbuild.err

# export MONO_OPTIONS="--debug"
# export FrameworkPathOverride="/Library/Frameworks/Mono.framework/Versions/Current/lib/mono/4.5"

Write-host "OS $PSVersionTable.OS"
Write-host "PWD: $pwd"
if ($PSVersionTable.OS.Contains("Darwin")) {
   $env:FrameworkPathOverride="/Library/Frameworks/Mono.framework/Versions/Current/lib/mono/4.5"
}

if ($PSVersionTable.OS.Contains("Windows")) {
   # new-alias grep findstr
   dotnet --info | findstr RID 
} else {
   # \| as or
   nuget help | grep 'Version\|RID'
   dotnet --info | grep 'Version\|RID'
}

dotnet restore src\Microsoft.CodeDom.Providers.DotNetCompilerPlatform\DotNetCompilerPlatform.csproj 
dotnet msbuild  src\Microsoft.CodeDom.Providers.DotNetCompilerPlatform\DotNetCompilerPlatform.csproj 
# SC : error CS7027: Error signing output with public key - Assembly signing not supported. 
dotnet build  src\Microsoft.CodeDom.Providers.DotNetCompilerPlatform\DotNetCompilerPlatform.csproj -o ../../bin

# if %ERRORLEVEL% neq 0 goto BuildFail
# echo *** BUILD FAILED ***
# exit /B 999
