$monolithicMultimediaDir = $PSScriptRoot.Substring(0, $PSScriptRoot.LastIndexOf("\"))

dotnet build $monolithicMultimediaDir\MonolithicMultimedia\MonolithicMultimedia.csproj