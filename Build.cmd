@echo off
if "%1"=="PublishPackages" goto publish

%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe buildscripts\build.proj %*
goto end

:publish


goto end

:end