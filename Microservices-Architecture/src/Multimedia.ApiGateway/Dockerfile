FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
EXPOSE 80

COPY ["Multimedia.ApiGateway/Multimedia.ApiGateway.csproj", "Multimedia.ApiGateway/"]
RUN dotnet restore "Multimedia.ApiGateway/Multimedia.ApiGateway.csproj"

COPY . ./
RUN dotnet publish -c release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Multimedia.ApiGateway.dll"]