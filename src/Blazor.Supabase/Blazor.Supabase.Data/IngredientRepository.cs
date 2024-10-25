using Blazor.Supabase.Models.Entities;
using Supabase;

namespace Blazor.Supabase.Data;

public class IngredientRepository : IDataRepository<Ingredient>
{
	private readonly Client _client;

	public IngredientRepository(Client client)
	{
		_client = client;
	}

	public Task Delete(Ingredient entity)
	{
		throw new NotImplementedException();
	}

	public async Task<IEnumerable<Ingredient>> GetAll()
	{
		var result = await _client.From<Ingredient>()
						.Get();

		return result.Models;
	}

	public async Task<Ingredient?> GetById(long id)
	{
		return await _client.From<Ingredient>()
						.Where(c => c.Id == id)
						.Single();
	}

	public Task<Ingredient?> Insert(Ingredient entity)
	{
		throw new NotImplementedException();
	}

	public Task<Ingredient?> Update(Ingredient entity)
	{
		throw new NotImplementedException();
	}
}
