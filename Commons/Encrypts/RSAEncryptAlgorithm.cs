
namespace aspnetcore.Commons.Encrypts
{
    public class RSAEncryptAlgorithm : IEncryptAlgorithm
    {
        public string Decrypt(string data)
        {
            return data;
        }

        public async Task<string> DecryptAsync(string data, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(data);
        }

        public string Encrypt(string data)
        {
            return data;
        }

        public async Task<string> EncryptAsync(string data, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(data);
        }
    }
}
