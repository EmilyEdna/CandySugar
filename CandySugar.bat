cd /d %~dp0
dotnet publish CandySugar\CandySugar.csproj -c Release -o ..\CandySugar\PublishSoft -f net6.0-windows --sc true -r win-x64
dotnet publish CandySugar.Upgrade\CandySugar.Upgrade.csproj -c Release -o ..\CandySugar\PublishSoft -f net6.0-windows --sc true -r win-x64
cd PublishSoft
del *.pdb
cd ..
rd /S /Q CandySugar\obj CandySugar\bin\Release
rd /S /Q CandySugar.Core\obj CandySugar.Core\bin\Release
rd /S /Q CandySugar.Common\obj CandySugar.Common\bin\Release
rd /S /Q CandySugar.Controls\obj CandySugar.Controls\bin\Release
rd /S /Q CandySugar.Resource\obj CandySugar.Resource\bin\Release
rd /S /Q CandySugar.Upgrade\obj CandySugar.Upgrade\bin\Release
xcopy CandySugar\bin\Debug\net6.0-windows\Plugins PublishSoft\Plugins /e /s
pause
