@echo off

echo MTDB Creator Distribution Package Make Script
echo Copyright (C) 2013 Pacific Northwest National Laboratory
echo.

if [%1]==[] goto usage

:make
echo Making MTDB Creator Distribution Package - %1 (%2)...
echo.

rem IF NOT EXIST "..\..\MTDBCreator\bin\%2\%1\MTDBCreator.exe" goto noreleasebuild
IF NOT EXIST "%3\MTDBCreator.exe" goto noreleasebuild

md bin

echo %3

echo Copying Binary Files...
echo.

rem xcopy "..\..\MTDBCreator\bin\%2\%1\MTDBCreator.exe" .\bin > nul
rem xcopy ..\..\MTDBCreator\bin\%2\%1\*.dll /s .\bin > nul
xcopy "%3MTDBCreator.exe" .\bin > nul
xcopy %3*.dll /s .\bin > nul
xcopy %4\MTDBFramework\unimod.xml .\bin > nul

echo Building Distribution Package in Zip Format...
echo.

IF NOT EXIST "..\..\builds\" md ..\..\builds > nul

IF EXIST "..\..\builds\MTDBCreator_Binary_%1_%2.zip" del ..\..\builds\MTDBCreator_Binary_%1_%2.zip

7za a -tzip ..\..\builds\MTDBCreator_Binary_%1_%2.zip .\bin\* -r

rd bin /s /q

echo.
echo Build Completed - MTDB Creator Distribution Package

goto quit

:noreleasebuild
echo MTDB Creator %1 (%2) build does not exist.
echo Please build the MTDB Creator in %1 (%2) configuration first!
goto quit

:usage
echo Usage: MakeDistPackage ^<Configuration: Debug^|Release^> ^<Platform: Any CPU^|x86^|x64^> ^<Build Dir ^(TargetDir^)^>
echo.

:quit