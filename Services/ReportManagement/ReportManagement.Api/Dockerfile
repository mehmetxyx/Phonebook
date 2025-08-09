# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/ReportManagement/ReportManagement.Api/ReportManagement.Api.csproj", "Services/ReportManagement/ReportManagement.Api/"]
COPY ["Shared/Shared.Api.Common/Shared.Api.Common.csproj", "Shared/Shared.Api.Common/"]
COPY ["Shared/Shared.Common/Shared.Common.csproj", "Shared/Shared.Common/"]
COPY ["Shared/Shared.Infrastructure.Messaging/Shared.Infrastructure.Messaging.csproj", "Shared/Shared.Infrastructure.Messaging/"]
COPY ["Shared/Shared.Application.Messaging/Shared.Application.Messaging.csproj", "Shared/Shared.Application.Messaging/"]
COPY ["Services/ReportManagement/ReportManagement.Application/ReportManagement.Application.csproj", "Services/ReportManagement/ReportManagement.Application/"]
COPY ["Shared/Shared.Contracts/Shared.Contracts.csproj", "Shared/Shared.Contracts/"]
COPY ["Services/ReportManagement/ReportManagement.Domain/ReportManagement.Domain.csproj", "Services/ReportManagement/ReportManagement.Domain/"]
COPY ["Services/ReportManagement/ReportManagement.Infrastructure.Data/ReportManagement.Infrastructure.Data.csproj", "Services/ReportManagement/ReportManagement.Infrastructure.Data/"]
RUN dotnet restore "./Services/ReportManagement/ReportManagement.Api/ReportManagement.Api.csproj"
COPY . .
WORKDIR "/src/Services/ReportManagement/ReportManagement.Api"
RUN dotnet build "./ReportManagement.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ReportManagement.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReportManagement.Api.dll"]