@ECHO OFF

REM folder location of target
SET targetApp=%~dp0TargetApplication

REM IIS Express Executable
SET IISExecutable=%PROGRAMFILES%\IIS Express\iisexpress.exe

REM Get OpenCover Executable
for /R "%~dp0packages" %%a in (*) do if /I "%%~nxa"=="OpenCover.Console.exe" SET OpenCoverExe=%%~dpnxa

REM Get Report Generator
for /R "%~dp0packages" %%a in (*) do if /I "%%~nxa"=="ReportGenerator.exe" SET ReportGeneratorExe=%%~dpnxa

REM Create a 'GeneratedReports' folder if it does not exist
if not exist "%~dp0GeneratedReports" mkdir "%~dp0GeneratedReports"

call :RunIISWithOpenCover
exit /b %errorlevel%

:RunIISWithOpenCover
"%OpenCoverExe%" ^
 -register:user ^
 -filter:"+[TargetApplication*]* -[TargetApplication.Tests*]*" ^
 -targetdir:"%targetApp%\bin" ^
 -output:"%~dp0GeneratedReports\CoverageReport.xml" ^
 -target:"%IISExecutable%" ^
 -targetargs:"/path:%targetApp% /port:2020"

"%ReportGeneratorExe%" ^
 -reports:"%~dp0GeneratedReports\CoverageReport.xml" ^
 -targetdir:"%~dp0GeneratedReports\Output"

start "report" "%~dp0GeneratedReports\Output\index.htm"