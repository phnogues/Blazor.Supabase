﻿using BlazorSupabase.Models.Dtos;
using BlazorSupabase.Models.Entities;
using BlazorSupabase.Models.Extensions;
using FluentResults;
using Supabase;

namespace BlazorSupabase.Data;

public class CrepeRepository : IDataRepository<CrepeDto>
{
	private readonly Client _client;

	public CrepeRepository(Client client)
	{
		_client = client;
	}

	public async Task<IEnumerable<CrepeDto>> GetAll()
	{
		var result = await _client.From<Crepe>()
						.Order(c => c.Name, Supabase.Postgrest.Constants.Ordering.Ascending)
						.Get();

		return result.Models.ToDtos();
	}

	public async Task<CrepeDto?> GetById(long id)
	{
		var result = await _client.From<Crepe>()
						.Where(c => c.Id == id)
						.Single();

		return result?.ToDto();
	}

	public async Task<CrepeDto?> Insert(CrepeDto dto)
	{
		var entity = dto.ToEntity();

		var response = await _client.From<Crepe>().Insert(entity);
		await _client.From<CrepeIngredient>()
			.Insert(entity.Ingredients.Select(i => new CrepeIngredient
			{
				CrepeId = response?.Model?.Id ?? 0,
				IngredientId = i.Id
			}).ToList());

		if (response.Model != null)
		{
			return await GetById(response.Model.Id);
		}

		return null;
	}

	public async Task<Result<CrepeDto?>> Update(CrepeDto dto)
	{
		// you can use RPC to update the ingredients in the database like : 
		// await _client.Rpc("update_crepe_ingredients", new { crepe_id = dbCrepe.Id, ingredient_ids = entity.IngredientIds });

		var entity = dto.ToEntity();
		var dbCrepe = await _client.From<Crepe>().Where(r => r.Id == entity.Id).Single();

		if (dbCrepe == null)
		{
			return Result.Fail("Crepe to update was not found in database");
		}

		// values to update
		dbCrepe.Name = entity.Name;
		dbCrepe.Price = entity.Price;
		dbCrepe.ImageUrl = entity.ImageUrl;

		await _client.From<CrepeIngredient>().Where(ci => ci.CrepeId == dbCrepe.Id).Delete();
		await _client.From<CrepeIngredient>()
			.Insert(entity.Ingredients.Select(i => new CrepeIngredient
			{
				CrepeId = dbCrepe.Id,
				IngredientId = i.Id
			}).ToList());

		var crepeUpdated = await dbCrepe.Update<Crepe>();

		return crepeUpdated?.Model?.ToDto();
	}

	public async Task<Result> Delete(CrepeDto dto)
	{
		await _client.From<Crepe>().Where(c => c.Id == dto.Id).Delete();

		return Result.Ok();
	}
}
