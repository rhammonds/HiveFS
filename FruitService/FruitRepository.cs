using HiveFS.FruitData.Dtos;
using HiveFS.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace HiveFS.FruitData;

public class FruitRepository : IFruitRepository
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger _logger;
    private readonly string _fruitApiEndpoint;

    public FruitRepository(IHttpClientFactory httpClientFactory, ILogger<FruitRepository> logger, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _fruitApiEndpoint = configuration["FruitApiEndPoint"];
    }

    public async Task<ApiResult<FruitDto>> GetFruit(string fruitName)
    {
        var apiResult = new ApiResult<FruitDto>();
        var requestUri = $"{_fruitApiEndpoint}/api/fruit/" + fruitName;
        using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

        _logger.LogDebug("Calling Get Fruit API with url: {url}", request.RequestUri);

        var httpClient = _httpClientFactory.CreateClient("FruitRepository");

        using var response = await httpClient.SendAsync(request);

        var responseContent = response.Content.ReadAsStringAsync().Result;
        apiResult.HttpStatusCode = response.StatusCode;
        apiResult.Succeeded = response.StatusCode == HttpStatusCode.OK;

        if (response.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogDebug($"Get Fruit API Call Results: {responseContent}");
            apiResult.Data = JsonSerializer.Deserialize<FruitDto>(responseContent);
        }
        else
        {
            _logger.LogError($"Get Fruit API Call Failed with status code {response.StatusCode}: {responseContent}");
            apiResult.Message = response.StatusCode.ToString();
        }

        return apiResult;
    }
}