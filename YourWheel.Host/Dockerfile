# Используем многоэтапную сборку

# Этап 1: Сборка (build)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файл решения, чтобы восстановить зависимости
COPY *.sln .

# Копируем все csproj файлы и восстанавливаем
COPY YourWheel.Host/YourWheel.Host.csproj YourWheel.Host/
COPY YourWheel.Domain/YourWheel.Domain.csproj YourWheel.Domain/
COPY ObjectTitles/ObjectTitles.csproj ObjectTitles/

# Восстанавливаем зависимости для host проекта
RUN dotnet restore YourWheel.Host/YourWheel.Host.csproj --source https://api.nuget.org/v3/index.json

# Копируем все исходники
COPY . .

# Собираем host проект
RUN dotnet build YourWheel.Host/YourWheel.Host.csproj -c Release -o /app/build

# Этап 2: Публикация (publish)
FROM build AS publish
RUN dotnet publish YourWheel.Host/YourWheel.Host.csproj -c Release -o /app/publish

# Этап 3: Финальный образ (final)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Копируем опубликованные файлы из этапа publish
COPY --from=publish /app/publish .

# Указываем точку входа
ENTRYPOINT ["dotnet", "YourWheel.Host.dll"]