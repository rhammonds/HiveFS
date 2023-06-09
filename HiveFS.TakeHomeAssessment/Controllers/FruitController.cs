using HiveFS.FruitService;
using HiveFS.FruitService.Models;
using HiveFS.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HiveFS.TakeHomeAssessment.Controllers;

[ApiController]
[Route("[controller]")]
public class FruitController : HiveFsBaseController
{
    private readonly IFruitApi _fruitApi;
    private readonly IMemoryCache _memoryCache;
    private readonly int _cacheSeconds;

    public FruitController(IFruitApi fruitApi, IMemoryCache memoryCache, IConfiguration configuration)
    {
        _fruitApi = fruitApi;
        _memoryCache = memoryCache;
        _cacheSeconds = Convert.ToInt32(configuration["CacheSeconds"]);
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FruitNutrients))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("GetFruit/{fruitName}")]
    public async Task<IActionResult> GetFruit(string fruitName)
    {

        if (string.IsNullOrWhiteSpace(fruitName))
        {
            return BadRequest("Fruit name is required.");
        }

        if (!_memoryCache.TryGetValue($"GetFruit{fruitName}", out ApiResult<FruitNutrients> cacheValue))
        {
            cacheValue = await _fruitApi.GetFruit(fruitName);

            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(_cacheSeconds));

            _memoryCache.Set($"GetFruit{fruitName}", cacheValue, cacheEntryOptions);
        }

        return MapResult(cacheValue);

    }
}
