FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY src/Services/Resource Api/SDmS.Resource.Api/SDmS.Resource.Api.csproj src/Services/Resource Api/SDmS.Resource.Api/
COPY src/Services/Resource Api/SDmS.Resource.DI/SDmS.Resource.DI.csproj src/Services/Resource Api/SDmS.Resource.DI/
COPY src/Services/Resource Api/SDmS.Resource.Infrastructure.Services/SDmS.Resource.Infrastructure.Services.csproj src/Services/Resource Api/SDmS.Resource.Infrastructure.Services/
COPY src/Services/Resource Api/SDmS.Resource.Common/SDmS.Resource.Common.csproj src/Services/Resource Api/SDmS.Resource.Common/
COPY src/Services/Resource Api/SDmS.Resource.Infrastructure/SDmS.Resource.Infrastructure.csproj src/Services/Resource Api/SDmS.Resource.Infrastructure/
COPY src/Services/Resource Api/SDmS.Resource.Domain.Services/SDmS.Resource.Domain.Services.csproj src/Services/Resource Api/SDmS.Resource.Domain.Services/
COPY src/Messages/SDmS.Messages.Common/SDmS.Messages.Common.csproj src/Messages/SDmS.Messages.Common/
COPY src/Services/Resource Api/SDmS.Resource.Domain/SDmS.Resource.Domain.csproj src/Services/Resource Api/SDmS.Resource.Domain/
RUN dotnet restore src/Services/Resource Api/SDmS.Resource.Api/SDmS.Resource.Api.csproj
COPY . .
WORKDIR /src/src/Services/Resource Api/SDmS.Resource.Api
RUN dotnet build SDmS.Resource.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SDmS.Resource.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SDmS.Resource.Api.dll"]
