
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
namespace mvc_todolist.Tests.Commons.Fakes;

public class FakeResponseCookiesFeature : IResponseCookiesFeature
{
    public IResponseCookies Cookies { get; set; }

    public FakeResponseCookiesFeature()
    {
        Cookies = new FakeResponseCookies();
    }
}

