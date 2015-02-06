@echo off

echo MTDB Creator Distribution Package Make Script
echo Copyright (C) 2013 Pacific Northwest National Laboratory
echo.

if [%1]==[] goto usage

set Configuration=%1

setLocal EnableDelayedExpansion

rem Variable PercentSign2 will typically be "Any CPU" (with the double quotes)
rem We want to change it to be AnyCpu (no double quotes)
rem The tilde sign in the following command removes the double quotes
set PlatformWithSpace=%~2

rem This for loop removes the space
for /f "tokens=1-2 delims= " %%a in ("!PlatformWithSpace!") do (
set Platform=%%a%%b
)

setLocal DisableDelayedExpansion

rem Remove the double quotes from the 3rd and 4th variables (folder paths)
set TargetDir=%~3
set SolutionDir=%~4

:make
echo Making MTDB Creator Distribution Package - %Configuration% (%Platform%)...
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

IF EXIST "..\..\builds\MTDBCreator_Binary_%Configuration%_%Platform%.zip" del "..\..\builds\MTDBCreator_Binary_%Configuration%_%Platform%.zip"

7za.exe a -tzip "..\..\builds\MTDBCreator_Binary_%Configuration%_%Platform%.zip" .\bin\* -r

rmdir bin /s /q

echo.
echo Build Completed - MTDB Creator Distribution Package

goto quit

:noreleasebuild
echo MTDB Creator %Configuration% (%Platform%) build does not exist.
echo Please build the MTDB Creator in %Configuration% (%Platform%) configuration first!
goto quit

:usage
echo Usage: MakeDistPackage ^<Configuration: Debug^|Release^> ^<Platform: Any CPU^|x86^|x64^> ^<Build Dir^> ^<TargetDir^>
echo.

:quit