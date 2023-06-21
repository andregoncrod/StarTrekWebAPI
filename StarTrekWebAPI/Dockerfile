#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["StarTrekWebAPI/StarTrekWebAPI.csproj", "StarTrekWebAPI/"]
RUN dotnet restore "StarTrekWebAPI/StarTrekWebAPI.csproj"
COPY . .
WORKDIR "/src/StarTrekWebAPI"
RUN dotnet build "StarTrekWebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StarTrekWebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StarTrekWebAPI.dll"]