# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Web/*.csproj ./Web/
COPY Services/*.csproj ./Services/
COPY Data/*.csproj ./Data/
COPY UnitTests/*.csproj ./UnitTests/
COPY IntegrationTests/*.csproj ./IntegrationTests/
RUN dotnet restore

# copy everything else and build app
COPY Web/. ./Web/
COPY Services/. ./Services/
COPY Data/. ./Data/
WORKDIR /source/Web
RUN dotnet publish -c release -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
ARG configuration=Release
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Web.dll"]