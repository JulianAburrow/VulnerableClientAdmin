namespace VulnerableClientAdminTest.Helpers;

using System.Security.Claims;
using Microsoft.AspNetCore.Http;

public static class FakeHttpContextAccessor
{
    public static IHttpContextAccessor Create(string userName = "TestUser")
    {
        var context = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new[] { new Claim(ClaimTypes.Name, userName) },
                    "TestAuth"))
        };

        return new HttpContextAccessor
        {
            HttpContext = context
        };
    }
}

