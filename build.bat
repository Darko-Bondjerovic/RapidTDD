taskkill /f /im RapidTDD.exe >nul 2>&1

rmdir /s /q WinFormApp\bin
rmdir /s /q WinFormApp\obj
rmdir /s /q FastColoredTextBox\bin
rmdir /s /q FastColoredTextBox\obj

dotnet restore RapidTDD.sln
dotnet build RapidTDD.sln -c Release --no-restore -tl:off -clp:ErrorsOnly -nologo && "WinFormApp\bin\Release\net9.0-windows\RapidTDD.exe"
