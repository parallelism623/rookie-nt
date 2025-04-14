
using Microsoft.AspNetCore.Http;

namespace mvc_todolist.Tests.Commons.Fakes;

public class FakeResponseCookies : IResponseCookies
{
    public List<(string Key, string Value, CookieOptions Options)> AppendedCookies { get; } = new List<(string, string, CookieOptions)>();

    public void Append(string key, string value)
    {
        AppendedCookies.Add((key, value, null!));
    }

    public void Append(string key, string value, CookieOptions options)
    {
        AppendedCookies.Add((key, value, options));
    }

    public void Delete(string key)
    {

    }

    public void Delete(string key, CookieOptions options)
    {
        
    }
}
