chcp 65001
@echo 开始自动化发布
cd /d %~dp0
dotnet publish DesktopPlatform\CandySugar.WPF\CandySugar.WPF.csproj -c Release -o ..\CandySugar\PublishSoft -f net6.0-windows10.0.17763.0 --sc true -r win-x64
dotnet publish DesktopPlatform\CandySugar.Upgrade\CandySugar.Upgrade.csproj -c Release -o ..\CandySugar\PublishSoft -f net6.0-windows10.0.17763.0 --sc true -r win-x64

rd /S /Q DesktopPlatform\CandySugar.WPF\obj DesktopPlatform\CandySugar.WPF\bin\Release
rd /S /Q DesktopPlatform\CandySugar.Core\obj DesktopPlatform\CandySugar.Core\bin\Release
rd /S /Q DesktopPlatform\CandySugar.Common\obj DesktopPlatform\CandySugar.Common\bin\Release
rd /S /Q DesktopPlatform\CandySugar.Controls\obj DesktopPlatform\CandySugar.Controls\bin\Release
rd /S /Q DesktopPlatform\CandySugar.Resource\obj DesktopPlatform\CandySugar.Resource\bin\Release
rd /S /Q DesktopPlatform\CandySugar.Upgrade\obj DesktopPlatform\CandySugar.Upgrade\bin\Release
xcopy DesktopPlatform\CandySugar.WPF\bin\Debug\net6.0-windows10.0.17763.0\Plugins PublishSoft\Plugins /e /s

cd PublishSoft
del *.pdb
ren CandySugar.WPF.exe CandySugar.exe
@echo 已完成处理
pause
