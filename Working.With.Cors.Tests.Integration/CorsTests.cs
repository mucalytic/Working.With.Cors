using static Working.With.Cors.Api.Constants;
using Microsoft.AspNetCore.Mvc.Testing;
using Working.With.Cors.Api.Interfaces;
using FluentAssertions;
using System.Net;

namespace Working.With.Cors.Tests.Integration;

public class CorsTests(WebApplicationFactory<IApiMarker> factory) : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly HttpClient _client = factory.CreateClient();
    
    [Fact]
    public async Task GetRequest_ReturnsCorsHeaders()
    {
        // arrange
        var request = new HttpRequestMessage(HttpMethod.Get, TestApi.Endpoint);
        request.Headers.Add("Origin", TestApi.Host);

        // act
        var response = await _client.SendAsync(request);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Headers.Select(h => h.Key).Should().Contain("Access-Control-Allow-Origin");
        response.Headers.GetValues("Access-Control-Allow-Origin").Should().Contain(TestApi.Host);
    }
    
    [Fact]
    public async Task OptionsRequest_ReturnsCorsHeaders()
    {
        // arrange
        var request = new HttpRequestMessage(HttpMethod.Options, TestApi.Endpoint);
        request.Headers.Add("Origin", TestApi.Host);
        request.Headers.Add("Access-Control-Request-Method", "GET");
        request.Headers.Add("Access-Control-Request-Headers", "Content-Type");

        // act
        var response = await _client.SendAsync(request);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        response.Headers.Select(h => h.Key).Should().Contain("Access-Control-Allow-Origin");
        response.Headers.GetValues("Access-Control-Allow-Origin").Should().Contain(TestApi.Host);
        response.Headers.Select(h => h.Key).Should().Contain("Access-Control-Allow-Methods");
        response.Headers.GetValues("Access-Control-Allow-Methods").SelectMany(val => val.Split(',')).Should().Contain("GET");
        response.Headers.Select(h => h.Key).Should().Contain("Access-Control-Allow-Headers");
        response.Headers.GetValues("Access-Control-Allow-Headers").Should().Contain("Content-Type");
    }
    
    [Fact]
    public async Task GetRequest_WithInvalidOrigin_DoesNotReturnCorsHeaders()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, TestApi.Endpoint);
        request.Headers.Add("Origin", "https://notallowed.com");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Headers.Contains("Access-Control-Allow-Origin").Should().BeFalse();
    }
    
    [Fact]
    public async Task GetRequest_WithCredentials_ReturnsCredentialsHeader()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, TestApi.Endpoint);
        request.Headers.Add("Origin", TestApi.Host);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Headers.Select(h => h.Key).Should().Contain("Access-Control-Allow-Credentials");
        response.Headers.GetValues("Access-Control-Allow-Credentials").Should().Contain("true");
    }
}
