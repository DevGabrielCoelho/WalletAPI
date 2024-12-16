FROM mcr.microsoft.com/mssql/server:2022-latest AS sqlserver

EXPOSE 1433

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS aspnetcore

WORKDIR /app

EXPOSE 5236

RUN apt-get update && apt-get upgrade -y && \
    apt-get install -y wget curl sudo

RUN wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    rm packages-microsoft-prod.deb && \
    apt-get update && \
    apt-get install -y dotnet-sdk-9.0 aspnetcore-runtime-9.0

COPY . /src

WORKDIR /src
RUN dotnet publish -c Release -o /app/publish

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

COPY ./docker/init-db.sh /init-db.sh

RUN chmod +x /init-db.sh

WORKDIR /app

CMD ["/init-db.sh"]  # Vai executar as migrações e depois iniciar o app
