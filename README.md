# Подготовка к запуску
	- cmd: git clone https://github.com/Stupin97/YourWheel.git в созданную директорию, например YourWheel (далее корня репозитория == YourWheel)
	- Запросить у меня файлы .env и следовать инструкции которую скину вместе с .env
	- Проверить файл DockerEntrypoint.sh ! дожен быть формат LF (не CRLF) !
#

# Запуск k8s
	- kubectl apply -f namespace.yaml
	- kubectl apply -k ./postgresql -n yourwheel-dev
	- kubectl apply -k ./redis -n yourwheel-dev
	- kubectl apply -k ./rabbitmq -n yourwheel-dev
	- kubectl apply -k ./ console-consumer-rabbitmq -n yourwheel-dev

	- Чтобы запустить: kubectl port-forward service/your-wheel 5000:80 5001:443 -n yourwheel-dev
#

# Для локальной работы выполнить:
	- kubectl port-forward pod/redis-XXX 6379:6379 -n yourwheel-dev
	- kubectl port-forward pod/rabbitmq-XXX 5672:5672 15672:15672 -n yourwheel-dev
	- kubectl port-forward service/your-wheel 5000:80 5001:443 -n yourwheel-dev
#


# Подготовка к запуску (Не k8s)
	- cmd: git clone https://github.com/Stupin97/YourWheel.git в созданную директорию, например YourWheel (далее корня репозитория == YourWheel)
	- Запросить у меня файлы .env - расположить в YourWheel.Host и ReceiveRabbitMQ
	- Проверить файл DockerEntrypoint.sh ! дожен быть формат LF (не CRLF) !
#

# Создание https сертификата для окружения

	- Удалить существующие сертификаты: 
		- Запустить powershell и выполнить: dotnet dev-certs https --clean
	- Сгенерировать новый сертификат и экспортировать его в защищенный паролем .pfx файл:
		- В powershell выполнить: dotnet dev-certs https --trust -ep "$env:USERPROFILE\.aspnet\https\aspnetapp.pfx" -p pass1111
		(pass1111 - пароль так же указан в docker-compose.yml)
	- Скопировать .pfx файл в YourWheel.Host\certificates
#

# Развертывание docker контейнера для окружения (для работы необходим https сертификат)

	- Запустить консоль из корня репозитория (YourWheel)
	- Выполнить: docker-compose -f docker-compose.rabbit.yml up --build 
	- Выполнить: docker-compose -f docker-compose.yml up --build 
	- Сервер слушает по адресу: https://localhost:5001/
	- Swagger: https://localhost:5001/swagger/index.html
#