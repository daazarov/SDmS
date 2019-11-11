FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY src/Services/Device Listener/SDmS.DeviceListener.Host/SDmS.DeviceListener.Host.csproj src/Services/Device Listener/SDmS.DeviceListener.Host/
COPY src/Services/Device Listener/SDmS.DeviceListener.Infrastructure/SDmS.DeviceListener.Infrastructure.csproj src/Services/Device Listener/SDmS.DeviceListener.Infrastructure/
COPY src/Services/Device Listener/SDmS.DeviceListener.Core/SDmS.DeviceListener.Core.csproj src/Services/Device Listener/SDmS.DeviceListener.Core/
COPY src/Messages/SDmS.Messages.Common/SDmS.Messages.Common.csproj src/Messages/SDmS.Messages.Common/
RUN dotnet restore src/Services/Device Listener/SDmS.DeviceListener.Host/SDmS.DeviceListener.Host.csproj
COPY . .
WORKDIR /src/src/Services/Device Listener/SDmS.DeviceListener.Host
RUN dotnet build SDmS.DeviceListener.Host.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SDmS.DeviceListener.Host.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SDmS.DeviceListener.Host.dll"]
