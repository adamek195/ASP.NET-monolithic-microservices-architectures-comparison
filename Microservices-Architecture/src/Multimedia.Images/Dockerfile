FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY ["Multimedia.Images/Multimedia.Images.csproj", "Multimedia.Images/"]
RUN dotnet restore "Multimedia.Images/Multimedia.Images.csproj"

COPY . ./
RUN dotnet publish -c release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Multimedia.Images.dll"]