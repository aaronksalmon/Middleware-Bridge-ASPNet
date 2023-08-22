namespace Middleware_Bridge_ASPNet.Controllers;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;

public class MiddlewareBridge
{
    private readonly RequestDelegate _next;
    private readonly HttpClient _httpClient;

    public MiddlewareBridge(RequestDelegate next)
    {
        _next = next;
        _httpClient = new HttpClient(); // Initialize HTTP client for forwarding requests
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Read the incoming request
        var request = context.Request;

        // Create a new HttpRequestMessage that we will forward to the partner's platform
        var forwardRequest = new HttpRequestMessage
        {
            Method = new HttpMethod(request.Method),
            RequestUri = new System.Uri("https://developers.smartrecruiters.com/reference/apply-api" + request.Path), // Partner's API endpoint
        };

        // Copy headers and content from the original request to the forward request
        foreach (var header in request.Headers)
        {
            forwardRequest.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
        }

        if (request.Body != null)
        {
            forwardRequest.Content = new StreamContent(request.Body);
        }

        // Send the forward request to the partner's platform
        var response = await _httpClient.SendAsync(forwardRequest);

        // Copy the response from the partner's platform to the original response
        context.Response.StatusCode = (int)response.StatusCode;
        foreach (var header in response.Headers)
        {
            context.Response.Headers[header.Key] = header.Value.ToArray();
        }

        await response.Content.CopyToAsync(context.Response.Body);

        // Call the next middleware in the pipeline
        await _next(context);
    }
}

// Extension method to register the middleware
public static class MiddlewareBridgeExtensions
{
    public static IApplicationBuilder UseMiddlewareBridge(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<MiddlewareBridge>();
    }
}
