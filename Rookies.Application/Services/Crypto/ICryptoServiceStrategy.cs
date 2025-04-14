namespace Rookies.Application.Services.Crypto;

public interface ICryptoServiceStrategy
{
    void SetCryptoAlgorithm(string algorithm);

    string Encrypt(string data);

    string Decrypt(string encryptedData);
}