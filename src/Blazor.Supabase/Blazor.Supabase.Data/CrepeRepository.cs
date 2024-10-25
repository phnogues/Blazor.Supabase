using Blazor.Supabase.Models.Entities;
using Supabase;
using static Supabase.Postgrest.Constants;

namespace Blazor.Supabase.Data;

public class CrepeRepository : IDataRepository<Crepe>
{
	private readonly Client _client;

	public CrepeRepository(Client client)
	{
		_client = client;
	}

	public async Task<IEnumerable<Crepe>> GetAll()
	{
		var result = await _client.From<Crepe>()
						.Order(c=>c.Name, Ordering.Ascending)
						.Get();

		return result.Models;
	}

	public async Task<Crepe?> GetById(long id)
	{
		return await _client.From<Crepe>()
						.Where(c => c.Id == id)
						.Single();
	}

	public async Task<Crepe?> Insert(Crepe entity)
	{
		var response = await _client.From<Crepe>().Insert(entity);
		await _client.From<CrepeIngredient>().Insert(entity.Ingredients.Select(i=> new CrepeIngredient { CrepeId = response?.Model.Id ?? 0, IngredientId = i.Id}).ToList());

		if (response.Model != null)
		{
			return await GetById(response.Model.Id);
		}

		return null;
	}

	public async Task<Crepe?> Update(Crepe crepe)
	{
		var dbCrepe = await _client.From<Crepe>().Where(r => r.Id == crepe.Id).Single();

		// values to update
		dbCrepe.Name = crepe.Name;
		dbCrepe.Price = crepe.Price;
		dbCrepe.ImageUrl = crepe.ImageUrl;

		var crepeUpdated = await dbCrepe.Update<Crepe>();

		return crepeUpdated.Model;
	}

	public async Task Delete(Crepe entity)
	{
		await _client.From<Crepe>().Where(c => c.Id == entity.Id).Delete();
	}
}
