FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["HealthBot/HealthBot.csproj","HealthBot/"]
RUN dotnet restore "HealthBot/HealthBot.csproj"
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
COPY . .
WORKDIR "/src/HealthBot"
RUN dotnet build "HealthBot.csproj" -c $configuration -o /app/build

FROM build as publish 
RUN dotnet publish "HealthBot.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HealthBot.dll"]
