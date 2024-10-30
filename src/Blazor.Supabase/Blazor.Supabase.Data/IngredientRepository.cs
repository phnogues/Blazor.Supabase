using BlazorSupabase.Models.Entities;
using FluentResults;
using Supabase;
using Supabase.Postgrest.Exceptions;
using static Supabase.Postgrest.Constants;

namespace BlazorSupabase.Data;

public class IngredientRepository : IDataRepository<Ingredient>
{
	private readonly Client _client;

	public IngredientRepository(Client client)
	{
		_client = client;
	}

	public async Task<Result> Delete(Ingredient entity)
	{
		try
		{
			await _client.From<Ingredient>().Where(c => c.Id == entity.Id).Delete();
			return Result.Ok();

		}
		catch (PostgrestException ex)
		{
			if(ex.Reason.ToString() == "ForeignKeyViolation")
			{
				return Result.Fail("This ingredient is using by crepes !");
			} 
			
			return Result.Fail(ex.Message);
		}
	}

	public async Task<IEnumerable<Ingredient>> GetAll()
	{
		var result = await _client.From<Ingredient>().Order(i => i.Name, Ordering.Ascending)
						.Get();

		return result.Models;
	}

	public async Task<Ingredient?> GetById(long id)
	{
		return await _client.From<Ingredient>()
						.Where(c => c.Id == id)
						.Single();
	}

	public async Task<Ingredient?> Insert(Ingredient entity)
	{
		var response = await _client.From<Ingredient>().Insert(entity);

		return response.Model;
	}

	public async Task<Ingredient?> Update(Ingredient entity)
	{
		var dbIngredient = await _client.From<Ingredient>().Where(r => r.Id == entity.Id).Single();

		// values to update
		dbIngredient.Name = entity.Name;

		var ingredientUpdated = await dbIngredient.Update<Ingredient>();

		return ingredientUpdated.Model;
	}
}
