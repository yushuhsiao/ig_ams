@rem dotnet publish -c Release -r win10-x64 -o pub\win10-x64
dotnet clean
dotnet publish -c Release -o pub
pause