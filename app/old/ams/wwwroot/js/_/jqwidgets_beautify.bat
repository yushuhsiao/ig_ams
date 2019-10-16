@echo off
rem npm -g install js-beautify
rem cd jqwidgets
rem echo @echo off > tmp.bat
rem for %%i in (*.js) do echo ren %%i %%~ni.min.js >> tmp.bat
rem for %%i in (*.js) do echo call js-beautify -f %%~ni.min.js -o %%i >> tmp.bat
rem call tmp.bat
rem del tmp.bat
rem cd..
cd ../../lib/jqwidgets/jqwidgets
for %%i in (*.js) do  js-beautify -f %%i -o ..\..\..\js\jqwidgets\%%i
pause
