PUSHD "%~dp0"

nuget pack Griffin.AdoNetFakes.csproj -Properties Configuration=Release

POPD
