using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using WalletApi.Interfaces;

class PasswordHasher : IPasswordHasher
{
    private readonly int _saltSize;
    private readonly int _keySize;
    private readonly int _memorySize;
    private readonly int _iterations;
    private readonly int _degreeOfParallelism;
    private readonly char _delimiter;

    public PasswordHasher(IConfiguration configuration)
    {
        var settings = configuration.GetSection("PasswordHasherSettings");

        _saltSize = settings.GetValue<int>("SaltSize");
        _keySize = settings.GetValue<int>("KeySize");
        _memorySize = settings.GetValue<int>("MemorySize");
        _iterations = settings.GetValue<int>("Iterations");
        _degreeOfParallelism = settings.GetValue<int>("DegreeOfParallelism");
        var delimiterValue = settings.GetValue<string>("Delimiter");
        if (string.IsNullOrEmpty(delimiterValue)){
            throw new Exception();
        }
        _delimiter = delimiterValue[0];
    }

    public string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(_saltSize);
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

        using (var argon2 = new Argon2id(passwordBytes))
        {
            argon2.MemorySize = _memorySize;
            argon2.Iterations = _iterations;
            argon2.DegreeOfParallelism = _degreeOfParallelism;
            argon2.Salt = salt;

            byte[] hash = argon2.GetBytes(_keySize);
            return string.Join(_delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }
    }

    public string Hash(string password, byte[] salt)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

        using (var argon2 = new Argon2id(passwordBytes))
        {
            argon2.MemorySize = _memorySize;
            argon2.Iterations = _iterations;
            argon2.DegreeOfParallelism = _degreeOfParallelism;
            argon2.Salt = salt;

            byte[] hash = argon2.GetBytes(_keySize);
            return string.Join(_delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }
    }

    bool IPasswordHasher.Verify(string passwordHash, string inputPassword)
    {
        string[] elements = passwordHash.Split(_delimiter);
        byte[] salt = Convert.FromBase64String(elements[0]);
        byte[] hash = Convert.FromBase64String(elements[1]);

        var hashInput = Hash(inputPassword, salt);
        return CryptographicOperations.FixedTimeEquals(hash, Convert.FromBase64String(hashInput.Split(_delimiter)[1]));
    }
}
