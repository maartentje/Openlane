FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OfferWorker/OfferWorker.csproj", "OfferWorker/"]
COPY ["BL/BL.csproj", "BL/"]
COPY ["DAL/DAL.csproj", "DAL/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "OfferWorker/OfferWorker.csproj"
COPY . .
WORKDIR "/src/OfferWorker"
RUN dotnet build "OfferWorker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OfferWorker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OfferWorker.dll"]
