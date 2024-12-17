#!/bin/bash

if [ -f /app/.env ]; then
    while IFS= read -r line || [[ -n "$line" ]]; do
        [[ "$line" =~ ^#.*$ ]] && continue
        [[ -z "$line" ]] && continue
        if [[ "$line" == *=* ]]; then
            key=$(echo "$line" | cut -d '=' -f 1)
            value=$(echo "$line" | cut -d '=' -f 2-)
            export "$key"="$value"
        fi
    done < /app/.env
fi

FLAG_FILE="/app/flag_file/migrations_done.flag"

echo "Aguardando SQL Server iniciar..."

until eval "$SQLCMD_COMMAND" > /dev/null 2>&1; do
    echo "Aguardando SQL Server... Tentando novamente em 5 segundos."
    sleep 5
done

echo "SQL Server iniciado."

if [ ! -f "$FLAG_FILE" ]; then
    echo "Executando as migrações..."

    cd /src

    dotnet ef database update

    touch "$FLAG_FILE"
    
    echo "Migrações realizadas em $(date)" >> "$FLAG_FILE"

    echo "Migrações concluídas!"
else
    echo "As migrações já foram realizadas anteriormente."
    echo "Tentativa de migração em $(date) (já realizado)" >> "$FLAG_FILE"
fi

echo "Iniciando o aplicativo .NET..."

rm /app/.env

dotnet /app/publish/WalletApi.dll
