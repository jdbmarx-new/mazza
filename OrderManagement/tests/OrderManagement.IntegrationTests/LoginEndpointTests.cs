using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace OrderManagement.IntegrationTests;

public sealed class LoginEndpointTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    [Fact]
    public async Task Given_ValidFixedUserCredentials_When_LoggingIn_Then_ShouldReturnAccessToken()
    {
        HttpResponseMessage response = await factory.CreateClient().PostAsJsonAsync("/auth/login", new
        {
            email = "dev@martech.com",
            password = "Senha@123"
        }, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        string body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.Contains("accessToken", body);
    }
}