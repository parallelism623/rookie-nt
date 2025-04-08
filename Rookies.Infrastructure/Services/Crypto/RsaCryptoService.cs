﻿using Microsoft.Extensions.Options;
using Rookies.Application.Services.Crypto;
using Rookies.Infrastructure.Options;
using System.Security.Cryptography;
using System.Text;

namespace Rookies.Infrastructure.Services.Crypto
{
    public class RsaCryptoService : ICryptoService
    {
        private readonly RsaCryptoOptions _rsaOptions;
        public RsaCryptoService(IOptions<RsaCryptoOptions> rsaOptions)
        {
            _rsaOptions = rsaOptions.Value;
        }
        public string Decrypt(string encryptedData)
        {
            var privateKey = Convert.FromBase64String(_rsaOptions.PrivateKey);
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey(privateKey, out _);
                return Encoding.UTF8.GetString(rsa.Decrypt(Convert.FromBase64String(encryptedData), RSAEncryptionPadding.OaepSHA256));
            }
        }

        public string Encrypt(string data)
        {
            var publicKey = Convert.FromBase64String(_rsaOptions.PublicKey);
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportRSAPublicKey(publicKey, out _);
                return Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes(data), RSAEncryptionPadding.OaepSHA256));
            }
        }
    }
}

