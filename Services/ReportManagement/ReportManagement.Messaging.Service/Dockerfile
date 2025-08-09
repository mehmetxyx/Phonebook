# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
WORKDIR /app

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/ReportManagement/ReportManagement.Messaging.Service/ReportManagement.Messaging.Service.csproj", "Services/ReportManagement/ReportManagement.Messaging.Service/"]
COPY ["Shared/Shared.Contracts/Shared.Contracts.csproj", "Shared/Shared.Contracts/"]
COPY ["Shared/Shared.Infrastructure.Messaging/Shared.Infrastructure.Messaging.csproj", "Shared/Shared.Infrastructure.Messaging/"]
COPY ["Shared/Shared.Application.Messaging/Shared.Application.Messaging.csproj", "Shared/Shared.Application.Messaging/"]
COPY ["Services/ReportManagement/ReportManagement.Application/ReportManagement.Application.csproj", "Services/ReportManagement/ReportManagement.Application/"]
COPY ["Shared/Shared.Common/Shared.Common.csproj", "Shared/Shared.Common/"]
COPY ["Services/ReportManagement/ReportManagement.Domain/ReportManagement.Domain.csproj", "Services/ReportManagement/ReportManagement.Domain/"]
COPY ["Services/ReportManagement/ReportManagement.Infrastructure.Data/ReportManagement.Infrastructure.Data.csproj", "Services/ReportManagement/ReportManagement.Infrastructure.Data/"]
RUN dotnet restore "./Services/ReportManagement/ReportManagement.Messaging.Service/ReportManagement.Messaging.Service.csproj"
COPY . .
WORKDIR "/src/Services/ReportManagement/ReportManagement.Messaging.Service"
RUN dotnet build "./ReportManagement.Messaging.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ReportManagement.Messaging.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReportManagement.Messaging.Service.dll"]