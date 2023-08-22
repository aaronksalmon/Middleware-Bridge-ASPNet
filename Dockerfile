FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Middleware-Bridge-ASPNet.csproj", "./"]
RUN dotnet restore "Middleware-Bridge-ASPNet.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Middleware-Bridge-ASPNet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Middleware-Bridge-ASPNet.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Middleware-Bridge-ASPNet.dll"]
