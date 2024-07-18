# Utiliza la imagen base de .NET para la construcción
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copia los archivos de tu proyecto y restaura las dependencias
COPY . ./
RUN dotnet restore

# Compila y publica la aplicación en la carpeta /out
RUN dotnet publish -c Release -o out

# Utiliza la imagen base de ASP.NET Core para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Configura la aplicación para que escuche en el puerto 80
ENV ASPNETCORE_URLS=http://+:80

# Expone el puerto 80 para acceso externo
EXPOSE 80

# Comando para ejecutar la aplicación
ENTRYPOINT ["dotnet", "DemoPilotoV1.dll"]