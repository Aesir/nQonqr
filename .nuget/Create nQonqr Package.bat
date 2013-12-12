@ECHO OFF
NuGet Pack ../nQonqr/nQonqr.csproj -BasePath ../nQonqr/bin/Release -Symbols -Prop Configuration=Release
PAUSE