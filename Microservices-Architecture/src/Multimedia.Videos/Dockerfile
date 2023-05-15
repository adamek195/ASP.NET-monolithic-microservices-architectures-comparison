FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY ["Multimedia.Videos/Multimedia.Videos.csproj", "Multimedia.Videos/"]
RUN dotnet restore "Multimedia.Videos/Multimedia.Videos.csproj"

COPY . ./
RUN dotnet publish -c release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Multimedia.Videos.dll"]