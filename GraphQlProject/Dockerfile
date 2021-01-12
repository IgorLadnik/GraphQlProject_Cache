#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["GraphQlProject/GraphQlProject.csproj", "GraphQlProject/"]
COPY ["PersonModelLib/PersonModelLib.csproj", "PersonModelLib/"]
COPY ["JwtHelperLib/JwtHelperLib.csproj", "JwtHelperLib/"]
COPY ["RepoLib/RepoLib.csproj", "RepoLib/"]
COPY ["RepoInterfaceLib/RepoInterfaceLib.csproj", "RepoInterfaceLib/"]
COPY ["GraphQlHelperLib/GraphQlHelperLib.csproj", "GraphQlHelperLib/"]
COPY ["AsyncLockLib/AsyncLockLib.csproj", "AsyncLockLib/"]
RUN dotnet restore "GraphQlProject/GraphQlProject.csproj"
COPY . .
WORKDIR "/src/GraphQlProject"
RUN dotnet build "GraphQlProject.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GraphQlProject.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GraphQlProject.dll"]