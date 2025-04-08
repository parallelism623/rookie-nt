using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookies.Application.Services.Crypto;
public interface ICryptoServiceStrategy
{
    void SetCryptoAlgorithm(string algorithm);
    string Encrypt(string data);
    string Decrypt(string encryptedData);
}
