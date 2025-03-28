# Запросить у меня файл .env

# Создание https сертификата для окружения

	- Удалить существующие сертификаты: 
		- Запустить powershell и выполнить: dotnet dev-certs https --clean
	- Сгенерировать новый сертификат и экспортировать его в защищенный паролем .pfx файл:
		- В powershell выполнить: dotnet dev-certs https --trust -ep "$env:USERPROFILE\.aspnet\https\aspnetapp.pfx" -p 11111
		(11111 - пароль так же указан в docker-compose.yml)

# Развертывание docker контейнера для окружения (для работы необходим https сертификат)

	- Запустить консоль из корня репозитория (YourWheel)
	- Выполнить: docker-compose -f docker-compose.yml up --build 
	- Сервер слушает по адресу: https://localhost:5001/
	- Swagger: https://localhost:5001/swagger/index.html

# (!ПРОПУСТИТЬ!) На время разработки 
	- Создать образ на основе файла Dockerfile и контекста (из YourWheel)
 		- docker build -t your-wheel -f YourWheel.Host/Dockerfile .
	- Запустить контейнер в фоновом режиме (из YourWheel)
		- docker-compose up -d