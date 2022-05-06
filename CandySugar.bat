chcp 65001
@echo 开始自动化发布
cd /d %~dp0
dotnet publish 桌面端\CandySugar.WPF\CandySugar.WPF.csproj -c Release -o ..\CandySugar\PublishSoft -f net6.0-windows10.0.17763.0 --sc true -r win-x64
dotnet publish 桌面端\CandySugar.Upgrade\CandySugar.Upgrade.csproj -c Release -o ..\CandySugar\PublishSoft -f net6.0-windows10.0.17763.0 --sc true -r win-x64

rd /S /Q 桌面端\CandySugar.WPF\obj 桌面端\CandySugar.WPF\bin\Release
rd /S /Q 桌面端\CandySugar.Core\obj 桌面端\CandySugar.Core\bin\Release
rd /S /Q 桌面端\CandySugar.Common\obj 桌面端\CandySugar.Common\bin\Release
rd /S /Q 桌面端\CandySugar.Controls\obj 桌面端\CandySugar.Controls\bin\Release
rd /S /Q 桌面端\CandySugar.Resource\obj 桌面端\CandySugar.Resource\bin\Release
rd /S /Q 桌面端\CandySugar.Upgrade\obj 桌面端\CandySugar.Upgrade\bin\Release
xcopy 桌面端\CandySugar.WPF\bin\Debug\net6.0-windows10.0.17763.0\Plugins PublishSoft\Plugins /e /s

cd PublishSoft
del *.pdb
ren CandySugar.WPF.exe CandySugar.exe
@echo 已完成处理
pause
