FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY FunAppAdClientCore.csproj .
RUN dotnet restore FunAppAdClientCore.csproj
COPY . .
WORKDIR /src
RUN dotnet build FunAppAdClientCore.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish FunAppAdClientCore.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "FunAppAdClientCore.dll"]
