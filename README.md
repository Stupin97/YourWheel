# Подготовка к запуску
	- cmd: git clone https://github.com/Stupin97/YourWheel.git в созданную директорию, например YourWheel (далее корня репозитория == YourWheel)
	- Запросить у меня файл .env - расположить в YourWheel.Host
	- Проверить файл DockerEntrypoint.sh ! дожен быть формат LF (не CRLF) !
#

# Создание https сертификата для окружения

	- Удалить существующие сертификаты: 
		- Запустить powershell и выполнить: dotnet dev-certs https --clean
	- Сгенерировать новый сертификат и экспортировать его в защищенный паролем .pfx файл:
		- В powershell выполнить: dotnet dev-certs https --trust -ep "$env:USERPROFILE\.aspnet\https\aspnetapp.pfx" -p pass1111
		(pass1111 - пароль так же указан в docker-compose.yml)
	- Скопировать .pfx файл в YourWheel.Host\certificates

# Развертывание docker контейнера для окружения (для работы необходим https сертификат)

	- Запустить консоль из корня репозитория (YourWheel)
	- Выполнить: docker-compose -f docker-compose.yml up --build 
	- Сервер слушает по адресу: https://localhost:5001/
	- Swagger: https://localhost:5001/swagger/index.html