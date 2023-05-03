$webMultimediaDir = $PSScriptRoot.Substring(0, $PSScriptRoot.LastIndexOf("\"))

dotnet build $webMultimediaDir\Multimedia.Web\Multimedia.Web.csproj