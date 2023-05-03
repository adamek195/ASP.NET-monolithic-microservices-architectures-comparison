$usersMultimediaDir = $PSScriptRoot.Substring(0, $PSScriptRoot.LastIndexOf("\"))

dotnet build $usersMultimediaDir\Multimedia.Users\Multimedia.Users.csproj