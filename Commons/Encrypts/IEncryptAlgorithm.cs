namespace aspnetcore.Commons.Encrypts
{
    public interface IEncryptAlgorithm
    {
        public Task<string> EncryptAsync(string data, CancellationToken cancellationToken = default);
        public string Encrypt(string data);
        public Task<string> DecryptAsync(string data, CancellationToken cancellationToken = default);
        public string Decrypt(string data);
    }
}
