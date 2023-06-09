namespace HiveFS.FruitData.Dtos;

public class Nutritions
{
    public decimal carbohydrates { get; set; }
    public decimal protein { get; set; }
    public decimal fat { get; set; }
    public decimal calories { get; set; }
    public decimal sugar { get; set; }
}

public class FruitDto
{
    public string genus { get; set; }
    public string name { get; set; }
    public int id { get; set; }
    public string family { get; set; }
    public string order { get; set; }
    public Nutritions nutritions { get; set; }
}
