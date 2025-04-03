#!/bin/bash
set -e

# Ожидание доступа к контейнеру db (DB)
echo "Waiting for db"
while ! nc -z $DB_HOST 5432; do
  sleep 0.1
done
echo "db is launched"

sleep 5

echo "CreateSomeFunctionsBeforeMigration.sql"
PGPASSWORD=$POSTGRES_PASSWORD psql -h "$DB_HOST" -U "$POSTGRES_USER" -d "$POSTGRES_DB" -f ./db_init/CreateSomeFunctionsBeforeMigration.sql

echo "UpdatedMigration.sql"
PGPASSWORD=$POSTGRES_PASSWORD psql -h "$DB_HOST" -U "$POSTGRES_USER" -d "$POSTGRES_DB" -f ./db_init/migrations.sql

echo "InitData.sql"
PGPASSWORD=$POSTGRES_PASSWORD psql -h "$DB_HOST" -U "$POSTGRES_USER" -d "$POSTGRES_DB" -f ./db_init/InitData.sql

echo "dotnet YourWheel.Host.dll"
dotnet YourWheel.Host.dll
