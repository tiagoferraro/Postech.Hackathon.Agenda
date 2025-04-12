# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Postech.Hackathon.Agenda.Api/Postech.Hackathon.Agenda.Api.csproj", "Postech.Hackathon.Agenda.Api/"]
COPY ["Postech.Hackathon.Agenda.Application/Postech.Hackathon.Agenda.Application.csproj", "Postech.Hackathon.Agenda.Application/"]
COPY ["Postech.Hackathon.Agenda.Domain/Postech.Hackathon.Agenda.Domain.csproj", "Postech.Hackathon.Agenda.Domain/"]
COPY ["Postech.Hackathon.Agenda.Infra/Postech.Hackathon.Agenda.Infra.csproj", "Postech.Hackathon.Agenda.Infra/"]
COPY ["Postech.Hackathon.Agenda.Ioc/Postech.Hackathon.Agenda.Ioc.csproj", "Postech.Hackathon.Agenda.Ioc/"]
RUN dotnet restore "./Postech.Hackathon.Agenda.Api/Postech.Hackathon.Agenda.Api.csproj"
COPY . .
WORKDIR "/src/Postech.Hackathon.Agenda.Api"
RUN dotnet build "./Postech.Hackathon.Agenda.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Postech.Hackathon.Agenda.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Postech.Hackathon.Agenda.Api.dll"]