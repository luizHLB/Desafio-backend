#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["/Product.API/Product.API.csproj", "Product.API/"]
COPY ["/Product.Domain/Product.Domain.csproj", "Product.API/"]
COPY ["/Product.Service/Product.Service.csproj", "Product.API/"]
COPY ["/Product.Data/Product.Data.csproj", "Product.Data/"]
COPY ["/Product.API/appsettings.json", "Product.API/"]
RUN dotnet restore "./Product.API/Product.API.csproj"
COPY . .
WORKDIR "/src/Product.API"
RUN dotnet build "./Product.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Product.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Product.API.dll"]