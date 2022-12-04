FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy everything
COPY ["TSITSolutions.ContactSite.sln","."]
COPY ["./src/Client/TSITSolutions.ContactSite.Client.csproj","src/Client/"]
COPY ["./src/Server/TSITSolutions.ContactSite.Server.csproj","src/Server/"]
COPY ["./src/Shared/TSITSolutions.ContactSite.Shared.csproj","src/Shared/"]
COPY ["./src/StringLocalizerSourceGenerator/TSITSolutions.StringLocalizerSourceGenerator.csproj","src/StringLocalizerSourceGenerator/"]

RUN dotnet restore

COPY . .
RUN dotnet build ./src/Server -c Release --no-restore
RUN dotnet publish ./src/Server -c Release -o out --no-build

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .
COPY ["src/Server/cert.pfx", "/https/cert.pfx"]
ENTRYPOINT ["dotnet", "TSITSolutions.ContactSite.Server.dll"]
