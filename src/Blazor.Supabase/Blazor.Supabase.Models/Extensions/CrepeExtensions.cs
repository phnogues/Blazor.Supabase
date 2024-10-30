using BlazorSupabase.Models.Dtos;
using BlazorSupabase.Models.Entities;

namespace BlazorSupabase.Models.Extensions;

public static class CrepeExtensions
{
	public static Crepe ToEntity(this CrepeDto dto)
	{
		return new Crepe
		{
			Id = dto.Id,
			Name = dto.Name,
			Price = dto.Price,
			ImageUrl = dto.ImageUrl,
			Ingredients = dto.Ingredients
		};
	}

	public static List<CrepeDto> ToDtos(this List<Crepe> entities)
	{
		return entities.Select(x => x.ToDto()).ToList();
	}

	public static CrepeDto ToDto(this Crepe entity)
	{
		return new CrepeDto
		{
			Id = entity.Id,
			Name = entity.Name,
			Price = entity.Price,
			ImageUrl = entity.ImageUrl,
			Ingredients = entity.Ingredients
		};
	}
}
