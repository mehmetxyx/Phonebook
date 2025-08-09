# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/ContactManagement/ContactManagement.Api/ContactManagement.Api.csproj", "Services/ContactManagement/ContactManagement.Api/"]
COPY ["Shared/Shared.Api.Common/Shared.Api.Common.csproj", "Shared/Shared.Api.Common/"]
COPY ["Shared/Shared.Common/Shared.Common.csproj", "Shared/Shared.Common/"]
COPY ["Services/ContactManagement/ContactManagement.Application/ContactManagement.Application.csproj", "Services/ContactManagement/ContactManagement.Application/"]
COPY ["Shared/Shared.Contracts/Shared.Contracts.csproj", "Shared/Shared.Contracts/"]
COPY ["Services/ContactManagement/ContactManagement.Domain/ContactManagement.Domain.csproj", "Services/ContactManagement/ContactManagement.Domain/"]
COPY ["Services/ContactManagement/ContactManagement.Infrastructure.Data/ContactManagement.Infrastructure.Data.csproj", "Services/ContactManagement/ContactManagement.Infrastructure.Data/"]
RUN dotnet restore "./Services/ContactManagement/ContactManagement.Api/ContactManagement.Api.csproj"
COPY . .
WORKDIR "/src/Services/ContactManagement/ContactManagement.Api"
RUN dotnet build "./ContactManagement.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ContactManagement.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContactManagement.Api.dll"]