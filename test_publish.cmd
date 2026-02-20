@echo off
dotnet test src/UniGetUI.Core.Tools.Tests/UniGetUI.Core.Tools.Tests.csproj -p:Platform=x64 --filter FullyQualifiedName~AccessibilityXamlTests -v q --nologo
if %errorlevel% neq 0 (
    echo "Accessibility tests failed."
    pause
)
rmdir /q /s src\UniGetUI\bin\x64\Release\net8.0-windows10.0.26100.0\win-x64\publish\
dotnet publish src/UniGetUI/UniGetUI.csproj /noLogo /property:Configuration=Release /property:Platform=x64 -v m
%signcommand% "src\UniGetUI\bin\x64\Release\net8.0-windows10.0.26100.0\win-x64\publish\UniGetUI.exe"
python3 scripts\generate_integrity_tree.py %cd%\src\UniGetUI\bin\x64\Release\net8.0-windows10.0.26100.0\win-x64\publish\
echo %cd%\src\UniGetUI\bin\x64\Release\net8.0-windows10.0.26100.0\win-x64\publish\
src\UniGetUI\bin\x64\Release\net8.0-windows10.0.26100.0\win-x64\publish\UniGetUI.exe
pause
