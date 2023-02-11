FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TSITSolutions.ContactSite.Admin/TSITSolutions.ContactSite.Admin.csproj", "TSITSolutions.ContactSite.Admin/"]
RUN dotnet restore "TSITSolutions.ContactSite.Admin/TSITSolutions.ContactSite.Admin.csproj"
COPY . .
WORKDIR "/src/TSITSolutions.ContactSite.Admin"
RUN dotnet build "TSITSolutions.ContactSite.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TSITSolutions.ContactSite.Admin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TSITSolutions.ContactSite.Admin.dll"]
