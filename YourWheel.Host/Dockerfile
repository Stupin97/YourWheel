FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY YourWheel.Host/YourWheel.Host.csproj YourWheel.Host/
COPY YourWheel.Domain/YourWheel.Domain.csproj YourWheel.Domain/
COPY ObjectTitles/ObjectTitles.csproj ObjectTitles/
COPY RabbitMqConfiguration/RabbitMqConfiguration.csproj RabbitMqConfiguration/

#/src/.nuget
RUN dotnet restore YourWheel.Host/YourWheel.Host.csproj --source https://api.nuget.org/v3/index.json

COPY . .

#
RUN dotnet tool install --global dotnet-ef --version 9.0.2
ENV PATH="$PATH:/root/.dotnet/tools"

# /src/app/db_init/migrations.sql
RUN dotnet ef migrations script --idempotent --output /app/db_init/migrations.sql --project YourWheel.Host/YourWheel.Host.csproj --startup-project YourWheel.Host

# /src/app/publish/YourWheel.Host.dll
RUN dotnet publish YourWheel.Host/YourWheel.Host.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

RUN apt-get update && apt-get install -y redis-tools

# /app/publish/YourWheel.Host.dll
COPY --from=build /app/publish .

# /app/db_init/migrations.sql
COPY --from=build /app/db_init/migrations.sql ./db_init/migrations.sql

# /app/db_init/*.sql
COPY YourWheel.Domain/FilesOfDatabase/initdb/CreateSomeFunctionsBeforeMigration.sql ./db_init/CreateSomeFunctionsBeforeMigration.sql
COPY YourWheel.Domain/FilesOfDatabase/initdb/InitData.sql ./db_init/InitData.sql

EXPOSE 80
EXPOSE 443

# /app/certificates/aspnetapp.pfx
COPY YourWheel.Host/certificates/aspnetapp.pfx ./certificates/

# /app/DockerEntrypoint.sh
COPY DockerEntrypoint.sh ./DockerEntrypoint.sh
RUN chmod +x ./DockerEntrypoint.sh

RUN apt-get update && apt-get install -y netcat-openbsd postgresql-client

ENTRYPOINT ["/bin/bash", "DockerEntrypoint.sh"]