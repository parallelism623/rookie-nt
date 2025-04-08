using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookies.Application.Services.Crypto;
public interface ICryptoService
{
    string Encrypt(string data);
    string Decrypt(string encryptedData);
}
