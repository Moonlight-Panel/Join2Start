FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Join2Start/Join2Start.csproj", "Join2Start/"]
RUN dotnet restore "Join2Start/Join2Start.csproj"
COPY . .
WORKDIR "/src/Join2Start"
RUN dotnet build "Join2Start.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Join2Start.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Join2Start.dll"]
