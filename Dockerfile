FROM mcr.microsoft.com/dotnet/aspnet:6.0

ENV ASPNETCORE_ENVIRONMENT=Development
ENV DefaultConnection=User ID=cmddbuser;Password=pa55w0rd!;Host=host.docker.internal;Port=5432;Database=ShopbridgeDb;Pooling=true;

COPY bin/Release/net6.0/publish/ App/
WORKDIR /App
ENTRYPOINT ["dotnet", "Shopbridge_base.dll"]