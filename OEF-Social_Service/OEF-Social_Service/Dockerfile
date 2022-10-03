#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OEF-Social_Service/OEF-Social_Service.csproj", "OEF-Social_Service/"]
COPY ["OEF-Social_Service.Composition/OEF-Social_Service.Composition.csproj", "OEF-Social_Service.Composition/"]
COPY ["OEF-Social_Service.Services/OEF-Social_Service.Services.csproj", "OEF-Social_Service.Services/"]
COPY ["OEF-Social_Service.DataAccess/OEF-Social_Service.DataAccess.csproj", "OEF-Social_Service.DataAccess/"]
COPY ["OEF-Social_Service.Models/OEF-Social_Service.Models.csproj", "OEF-Social_Service.Models/"]
RUN dotnet restore "OEF-Social_Service/OEF-Social_Service.csproj"
COPY . .
WORKDIR "/src/OEF-Social_Service"
RUN dotnet build "OEF-Social_Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OEF-Social_Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OEF-Social_Service.dll"]