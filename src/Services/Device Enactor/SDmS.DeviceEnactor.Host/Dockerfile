FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY src/Services/Device Enactor/SDmS.DeviceEnactor.Host/SDmS.DeviceEnactor.Host.csproj src/Services/Device Enactor/SDmS.DeviceEnactor.Host/
COPY src/Messages/SDmS.Messages.Common/SDmS.Messages.Common.csproj src/Messages/SDmS.Messages.Common/
RUN dotnet restore src/Services/Device Enactor/SDmS.DeviceEnactor.Host/SDmS.DeviceEnactor.Host.csproj
COPY . .
WORKDIR /src/src/Services/Device Enactor/SDmS.DeviceEnactor.Host
RUN dotnet build SDmS.DeviceEnactor.Host.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SDmS.DeviceEnactor.Host.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SDmS.DeviceEnactor.Host.dll"]
