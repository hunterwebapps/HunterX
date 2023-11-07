#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["HunterX.Trader.Workers.Executor/HunterX.Trader.Workers.Executor.csproj", "HunterX.Trader.Workers.Executor/"]
RUN dotnet restore "HunterX.Trader.Workers.Executor/HunterX.Trader.Workers.Executor.csproj"
COPY . .
WORKDIR "/src/HunterX.Trader.Workers.Executor"
RUN dotnet build "HunterX.Trader.Workers.Executor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HunterX.Trader.Workers.Executor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HunterX.Trader.Workers.Executor.dll"]
