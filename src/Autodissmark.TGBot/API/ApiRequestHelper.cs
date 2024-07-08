using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace Autodissmark.TGBot.API;

public class ApiResponse<T>
{
    public T Data { get; set; }
}

public static class ApiRequestHelper
{
    public static async Task<TResponse> PostAsync<TRequest, TResponse> (HttpClient httpClient, string url, TRequest requestData)
    {
        var jsonContent = JsonSerializer.Serialize(requestData);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var apiResponse = JsonSerializer.Deserialize<TResponse>(responseString, options);
        return apiResponse;
    }

    public static async Task<TResponse> GetAsync<TResponse> (HttpClient httpClient, string url)
    {
        HttpResponseMessage response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var apiResponse = JsonSerializer.Deserialize<TResponse>(responseString, options);
        return apiResponse;
    }
}
