@echo off
if "%1"==":publish" goto publish

%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe buildscripts\build.proj %*
goto end

:publish
powershell.exe -noprofile buildscripts\publish-nuget-packages.ps1

goto end

:end