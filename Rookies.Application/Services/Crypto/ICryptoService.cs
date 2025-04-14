namespace Rookies.Application.Services.Crypto;

public interface ICryptoService
{
    string Encrypt(string data);

    string Decrypt(string encryptedData);
}