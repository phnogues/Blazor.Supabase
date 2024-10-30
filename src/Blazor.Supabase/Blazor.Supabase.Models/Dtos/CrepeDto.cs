using BlazorSupabase.Models.Entities;

namespace BlazorSupabase.Models.Dtos;

public class CrepeDto
{
    public int Id { get; set; }

    public string Name { get; set; }

	public float Price { get; set; }

	public string ImageUrl { get; set; }

	public List<Ingredient> Ingredients { get; set; } = new();

	public IEnumerable<int> IngredientIds
	{
		get
		{
			return Ingredients?.Select(x => x.Id);
		}

		set
		{
			Ingredients = value?.Select(x => new Ingredient { Id = x }).ToList();
		}
	}
}
