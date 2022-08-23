FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Server/TSITSolutions.ContactSite.Server.csproj", "Server/"]
COPY ["src/Client/TSITSolutions.ContactSite.Client.csproj", "Client/"]
COPY ["src/Shared/TSITSolutions.ContactSite.Shared.csproj", "Shared/"]
COPY ["src/StringLocalizerSourceGenerator/TSITSolutions.StringLocalizerSourceGenerator.csproj", "StringLocalizerSourceGenerator/"]
RUN dotnet restore "src/Server/TSITSolutions.ContactSite.Server.csproj"
COPY . .
WORKDIR "/src/Server"
RUN dotnet build "TSITSolutions.ContactSite.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TSITSolutions.ContactSite.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TSITSolutions.ContactSite.Server.dll"]
