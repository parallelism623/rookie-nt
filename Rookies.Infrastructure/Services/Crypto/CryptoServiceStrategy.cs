using Microsoft.Extensions.DependencyInjection;
using Rookies.Application.Services.Crypto;
using Rookies.Contract.Exceptions;

namespace Rookies.Infrastructure.Services.Crypto;
public class CryptoServiceStrategy : ICryptoServiceStrategy
{
    private readonly IServiceProvider _serviceProvider;
    private ICryptoService? _cryptoService;

    private string? _currentAlgorithm;
    public CryptoServiceStrategy(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public string Decrypt(string encryptedData)
    {
        CheckExistsCryptoService();
        return _cryptoService?.Decrypt(encryptedData)
            ?? throw new InternalServerErrorException($"Cannot encrypt data with {_currentAlgorithm} algorithm");
    }

    public string Encrypt(string data)
    {
        CheckExistsCryptoService();
        return _cryptoService?.Encrypt(data)
            ?? throw new InternalServerErrorException($"Cannot encrypt data with {_currentAlgorithm} algorithm");
    }

    public void SetCryptoAlgorithm(string algorithm)
    {
        try
        {
            _cryptoService = _serviceProvider.GetRequiredKeyedService<ICryptoService>(algorithm);
            _currentAlgorithm = algorithm;
        }
        catch
        {

            throw new BadRequestException($"No support {algorithm} crypto algorithm"); 
        }
    }

    private void CheckExistsCryptoService()
    {
        if(_cryptoService == null)
        {
            throw new BadRequestException($"Crypto service can not use until it is initialized");
        }
    }
}
