# Usar la imagen base de .NET 7.0 SDK para construir la aplicación
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copiar el archivo de proyecto y restaurar las dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar el resto del código y construir el proyecto
COPY . ./
RUN dotnet publish -c Release -o out

# Usar la imagen base de .NET 7.0 Runtime para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "DemoPilotoV1.dll"]

