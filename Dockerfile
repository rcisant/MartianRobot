FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT Local
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/MartianRobot.Host/MartianRobot.Host.csproj", "src/MartianRobot.Host/"]
COPY ["src/MartianRobot.Api/MartianRobot.Api.csproj", "src/MartianRobot.Api/"]
COPY ["src/MartianRobot.Application/MartianRobot.Application.csproj", "src/MartianRobot.Application/"]
COPY ["src/MartianRobot.Domain/MartianRobot.Domain.csproj", "src/MartianRobot.Domain/"]
COPY ["src/MartianRobot.Infrastructure/MartianRobot.Infrastructure.csproj", "src/MartianRobot.Infrastructure/"]
COPY ./NuGet.config ./
RUN dotnet restore "src/MartianRobot.Host/MartianRobot.Host.csproj" --configfile ./NuGet.config
#COPY . .
#WORKDIR "/src/src/MartianRobot.Infrastructure"
#RUN dotnet build "MartianRobot.Infrastructure.csproj" -c Release -o /app/build
#RUN dotnet tool install --global dotnet-ef
#ENV PATH="${PATH}:/root/.dotnet/tools"
#RUN PATH="$PATH:/root/.dotnet/tools"
#RUN dotnet-ef database update --context MartianRobotContext --connection "Server=localhost;Database=IoTHub;Trusted_Connection=True;"
COPY . .
WORKDIR "/src/src/MartianRobot.Host"
RUN dotnet build "MartianRobot.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MartianRobot.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MartianRobot.Host.dll"]
