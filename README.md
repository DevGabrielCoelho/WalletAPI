# WalletApi

A digital wallet management API, enabling control of financial transactions, balances, and other related functionalities.

## 🛠️ Features

- Creation of users and digital wallets.
- Record and query of financial transactions.
- Real-time balance check.
- Transfer between accounts with the possibility of reversal.
- Secure authentication and authorization using JWT and Argon2.

## 📋 Prerequisites

Make sure you have the following tools installed on your machine before starting:

- **[Docker](https://www.docker.com/products/docker-desktop/)** (recommended latest version)

## 🚀 Installation and Setup

1. **Clone the repository:**

   ```bash
   git clone https://github.com/DevGabrielCoelho/WalletApi.git
   cd WalletApi
   ```

2. **Configure the environment file:**  
    Create the `.env` file in the root of the project and add the following configurations (adjust as needed):

   ```.env
   DEFAULT_CONNECTION="Server=msql,1433;Database=WalletDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"

   LOGGING_LOG_LEVEL_DEFAULT="Information"
   LOGGING_LOG_LEVEL_MICROSOFT_ASP_NET_CORE="Warning"

   ALLOWED_HOSTS="*"

   ISSUER="IssuerNameOrLink"
   AUDIENCE="AudienceNameOrLink"
   SIGNIN_KEY="YourStrongSigninKey(recommended 128 characters)"

   SALT_SIZE=IntSaltSize
   KEY_SIZE=IntSaltSize
   MEMORY_SIZE=IntMemorySize
   ITERATIONS=IntIterations
   DEGREE_OF_PARALLELISM=IntDegreeOfParallelism
   DELIMITER="CharDelimiter"

   DOCKER_SA_PASSWORD="YourStrong!Passw0rd"
   DOCKER_MSSQL_PID="Developer"

   DOCKER_ASP_NET_CORE_PORT=5236:5236
   DOCKER_MSQL_PORT=1433:1433

   URLS=http://+:5236

   SQLCMD_COMMAND=/opt/mssql-tools/bin/sqlcmd -S "mssql,1433" -U "sa" -P "YourStrong!Passw0rd" -Q "SELECT 1"
   ```

3. **Start Docker build:**

   ```bash
   docker-compose up -d --build
   ```

4. Access the API at:
   ```
   http://localhost:5236 (or the port configured in .env)
   ```

## 🧪 API Endpoints

Here are the available routes and their main functionalities:

- **`GET /api/account/balance/?id=uuid_string`**: Shows the balance of a user.
- **`POST /api/auth`**: Performs login.
- **`POST /api/refunding/refound/?createdBy=uuid_string&transactionId=uuid_string`**: Performs a refund.
- **`POST /api/transaction/transfer/?toAccountId=uuid_string&fromAccountId=uuid_string&value=decimal`**: Performs a transfer.
- **`POST /api/users/register`**: Creates a new user.
- **`PUT /api/users/edit-user/?id=uuid_string`**: Edits an existing user.

## 🛠️ Technologies Used

- **C#**: Programming language.
- **ASP.NET Core**: Framework for building RESTful APIs.
- **SQL Server**: Relational database.
- **JWT**: For secure authentication.
- **Argon2**: For secure password hashing.
- **Docker**: For containerization and application deployment.

## 📂 Project Structure

```
src/
├── Controllers/        # Controllers for the API routes
├── Data/               # Database context
├── docker/             # docker/.sh file
├── Dtos/               # Data transfer objects
├── Enums/              # Enum definitions
├── Interfaces/         # Contracts for abstractions
├── Mappers/            # DTO to Model conversions and vice versa
├── Models/             # Data models
├── Properties/         # Project configurations
└── Repository/         # Implementations of interfaces using DbContext
```

## 📞 Contact

For questions, suggestions, or collaborations, feel free to reach out:

- **Gabriel Coelho**
- GitHub: [DevGabrielCoelho](https://github.com/DevGabrielCoelho)
- Email: [gabriel.coelhosousasantos.pv@gmail.com](mailto:gabriel.coelhosousasantos.pv@gmail.com)
