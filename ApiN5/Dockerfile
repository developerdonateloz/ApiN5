#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ApiN5/ApiN5.csproj", "ApiN5/"]
RUN dotnet restore "ApiN5/ApiN5.csproj"
COPY . .
WORKDIR "/src/ApiN5"
RUN dotnet build "ApiN5.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiN5.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiN5.dll"]