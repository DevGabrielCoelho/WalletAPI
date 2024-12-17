FROM mcr.microsoft.com/mssql/server:2022-latest AS sqlserver
EXPOSE 1433

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS aspnetcore
WORKDIR /app
EXPOSE 5236

RUN apt-get update && apt-get upgrade -y && \
    apt-get install -y wget curl sudo libterm-readline-perl-perl unixodbc-dev

RUN wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    rm packages-microsoft-prod.deb && \
    apt-get update && \
    apt-get install -y dotnet-sdk-9.0 aspnetcore-runtime-9.0

RUN curl https://packages.microsoft.com/keys/microsoft.asc | sudo tee /etc/apt/trusted.gpg.d/microsoft.asc && \
    sudo curl https://packages.microsoft.com/config/debian/12/prod.list > /etc/apt/sources.list.d/mssql-release.list && \
    sudo apt-get update && \
    ACCEPT_EULA=Y apt-get install -y mssql-tools && \
    echo 'export PATH="$PATH:/opt/mssql-tools/bin"' > /etc/profile.d/mssql-tools.sh

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

COPY . /src
WORKDIR /src

RUN dotnet publish -c Release -o /app/publish

COPY ./docker/init-db.sh /init-db.sh
COPY ./.env /app/.env

RUN chmod +x /init-db.sh

WORKDIR /app

CMD envsubst < /init-db.sh | bash
