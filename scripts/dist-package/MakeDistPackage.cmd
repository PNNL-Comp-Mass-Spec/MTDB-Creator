@echo off

echo MTDB Creator Distribution Package Make Script
echo Copyright (C) 2013 Pacific Northwest National Laboratory
echo.

if [%1]==[] goto usage

setLocal EnableDelayedExpansion

set PlatformWithSpace=%~2

for /f "tokens=1-2 delims= " %%a in ("!PlatformWithSpace!") do (
set Platform=%%a%%b
)

setLocal DisableDelayedExpansion

set TargetDir=%~3
set SolutionDir=%~4

:make
echo Making MTDB Creator Distribution Package - %1 (%Platform%)...
echo.

IF NOT EXIST "%TargetDir%MTDBCreator.exe" goto noreleasebuild

IF NOT EXIST .\bin mkdir bin

echo %TargetDir%

echo Copying Binary Files...
echo.

xcopy "%TargetDir%MTDBCreator.exe" .\bin /Y /D
xcopy "%TargetDir%*.dll" .\bin /S /Y /D

echo Building Distribution Package in Zip Format...
echo.

IF NOT EXIST ..\..\builds mkdir ..\..\builds

IF EXIST "..\..\builds\MTDBCreator_Binary_%1_%Platform%.zip" del "..\..\builds\MTDBCreator_Binary_%1_%Platform%.zip"

7za a -tzip "..\..\builds\MTDBCreator_Binary_%1_%Platform%.zip" .\bin\* -r

rmdir bin /s /q

echo.
echo Build Completed - MTDB Creator Distribution Package

goto quit

:noreleasebuild
echo MTDB Creator %1 (%Platform%) build does not exist.
echo Please build the MTDB Creator in %1 (%Platform%) configuration first!
goto quit

:usage
echo Usage: MakeDistPackage ^<Configuration: Debug^|Release^> ^<Platform: Any CPU^|x86^|x64^> ^<Build Dir ^(TargetDir^)^>
echo.

:quit