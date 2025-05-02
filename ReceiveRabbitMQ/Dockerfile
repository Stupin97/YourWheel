FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ReceiveRabbitMQ/ReceiveRabbitMQ.csproj ReceiveRabbitMQ/
COPY RabbitMqConfiguration/RabbitMqConfiguration.csproj RabbitMqConfiguration/

#/src/.nuget
RUN dotnet restore ./ReceiveRabbitMQ/ReceiveRabbitMQ.csproj --source https://api.nuget.org/v3/index.json

COPY . .

# /src/app/publish/ReceiveRabbitMQ.dll
RUN dotnet publish ./ReceiveRabbitMQ/ReceiveRabbitMQ.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app

# /app/publish/ReceiveRabbitMQ.dll
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ReceiveRabbitMQ.dll"]