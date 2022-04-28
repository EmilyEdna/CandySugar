chcp 65001
@echo 开始自动化发布
cd /d %~dp0
dotnet publish CandySugar.WPF\CandySugar.WPF.csproj -c Release -o ..\CandySugar\PublishSoft -f net6.0-windows10.0.17763.0 --sc true -r win-x64
dotnet publish CandySugar.Upgrade\CandySugar.Upgrade.csproj -c Release -o ..\CandySugar\PublishSoft -f net6.0-windows10.0.17763.0 --sc true -r win-x64

rd /S /Q CandySugar.WPF\obj CandySugar.WPF\bin\Release
rd /S /Q CandySugar.Core\obj CandySugar.Core\bin\Release
rd /S /Q CandySugar.Common\obj CandySugar.Common\bin\Release
rd /S /Q CandySugar.Controls\obj CandySugar.Controls\bin\Release
rd /S /Q CandySugar.Resource\obj CandySugar.Resource\bin\Release
rd /S /Q CandySugar.Upgrade\obj CandySugar.Upgrade\bin\Release
xcopy CandySugar.WPF\bin\Debug\net6.0-windows10.0.17763.0\Plugins PublishSoft\Plugins /e /s

cd PublishSoft
del *.pdb
ren CandySugar.WPF.exe CandySugar.exe
@echo 已完成处理
pause
