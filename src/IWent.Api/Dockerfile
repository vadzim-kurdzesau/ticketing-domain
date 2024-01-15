# Build with the "docker build -f src/IWent.Api/Dockerfile ." command
# Where "." specifies the build context location and "-f" - the location of the Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy the source code to app/src
COPY ["src", "./src/"]

WORKDIR /app/src/IWent.Api
# Restore project dependencies
RUN dotnet restore
# Build the project
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app

EXPOSE 80
EXPOSE 443

COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "IWent.Api.dll"]