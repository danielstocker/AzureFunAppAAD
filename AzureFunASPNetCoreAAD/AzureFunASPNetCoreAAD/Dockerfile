FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["AzureFunASPNetCoreAAD/AzureFunASPNetCoreAAD.csproj", "AzureFunASPNetCoreAAD/"]
RUN dotnet restore "AzureFunASPNetCoreAAD/AzureFunASPNetCoreAAD.csproj"
COPY . .
WORKDIR "/src/AzureFunASPNetCoreAAD"
RUN dotnet build "AzureFunASPNetCoreAAD.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AzureFunASPNetCoreAAD.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AzureFunASPNetCoreAAD.dll"]