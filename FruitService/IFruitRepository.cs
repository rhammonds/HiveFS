using HiveFS.FruitData.Dtos;
using HiveFS.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiveFS.FruitData;

public interface IFruitRepository
{
    public Task<ApiResult<FruitDto>> GetFruit(string fruitName);
}
