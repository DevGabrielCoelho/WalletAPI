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

- **[.NET](https://dotnet.microsoft.com/pt-br/download)** (recommended version: 8.0.11 or higher)
- **[SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)** or another compatible database.

## 🚀 Installation and Setup

1. **Clone the repository:**

   ```bash
   git clone https://github.com/DevGabrielCoelho/WalletApi.git
   cd WalletApi
   ```

2. **Install dependencies (if necessary):**
   Add the required packages to the project using the following commands:

   ```bash
   dotnet add package Konscious.Security.Cryptography.Argon2 --version 1.3.1
   dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson --version 8.0.11
   dotnet add package Microsoft.AspNetCore.OpenApi --version 8.0.10
   dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.0
   dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.0
   dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.0
   dotnet add package Newtonsoft.Json --version 13.0.3
   dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.11
   ```

3. **Configure the environment file:**  
    Create the `appsettings.json` file in the root of the project and add the following configurations (adjust as needed):

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "your_connection_string"
     },
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     },
     "AllowedHosts": "*",
     "JWT": {
       "Issuer": "your_issuer_route",
       "Audience": "your_audience_route",
       "SigninKey": "your_HS512_signin_key"
     }
   }
   ```

4. **Create your Hashing structure:**
   Create the `PasswordHasher.cs` file in the root of the project and create configurations by inheriting from
   `Interfaces/IPasswordHasher.cs`

   ```cs
   using System.Security.Cryptography;
   using System.Text;
   using Konscious.Security.Cryptography;
   using WalletApi.Interfaces;

   class PasswordHasher : IPasswordHasher
   {
   }
   ```

5. **Run the database migrations (if applicable):**

   ```bash
   dotnet ef database update
   ```

6. **Start the server:**

   ```bash
   dotnet watch run
   ```

7. Access the API at:
   ```
   http://localhost:5236 (or the port configured in Properties/launchSettings.json)
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

## 📂 Project Structure

```
src/
├── Controllers/        # Controllers for the API routes
├── Data/               # Database context
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
