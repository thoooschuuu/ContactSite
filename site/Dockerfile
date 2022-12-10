FROM mcr.microsoft.com/dotnet/aspnet:7.0 as base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

RUN dotnet tool install --global wasm-tools

COPY ["TSITSolutions.ContactSite.sln","."]
COPY ["./src/Client/TSITSolutions.ContactSite.Client.csproj","src/Client/"]
COPY ["./src/Server/TSITSolutions.ContactSite.Server.csproj","src/Server/"]
COPY ["./src/Shared/TSITSolutions.ContactSite.Shared.csproj","src/Shared/"]
COPY ["./src/StringLocalizerSourceGenerator/TSITSolutions.StringLocalizerSourceGenerator.csproj","src/StringLocalizerSourceGenerator/"]
COPY ["./src/TSITSolutions.ImageResizer/TSITSolutions.ImageResizer.csproj","src/TSITSolutions.ImageResizer/"]
COPY ["./tests/TSITSolutions.StringLocalizerSourceGenerator.Tests.Integration/TSITSolutions.StringLocalizerSourceGenerator.Tests.Integration.csproj","tests/TSITSolutions.StringLocalizerSourceGenerator.Tests.Integration/"]

RUN dotnet restore

COPY . .

WORKDIR /app/src/Server
RUN dotnet build -c Release --no-restore

FROM build as publish
WORKDIR /app/src/Server
RUN dotnet publish -c Release -o publish --no-build

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=publish /app/src/Server/publish .
COPY ["src/Server/cert.pfx", "/https/cert.pfx"]
ENTRYPOINT ["dotnet", "TSITSolutions.ContactSite.Server.dll"]
