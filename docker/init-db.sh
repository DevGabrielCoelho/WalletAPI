#!/bin/bash

FLAG_FILE="/app/migrations_done.flag"

echo "Aguardando SQL Server iniciar..."
sleep 30

if [ ! -f "$FLAG_FILE" ]; then
    echo "Executando as migrações..."

    cd /src

    dotnet ef database update

    touch "$FLAG_FILE"

    echo "Migrações concluídas!"
else
    echo "As migrações já foram realizadas anteriormente."
fi

echo "Iniciando o aplicativo .NET..."
dotnet /app/publish/WalletApi.dll
