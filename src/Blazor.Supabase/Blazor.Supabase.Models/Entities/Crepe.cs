using Supabase.Postgrest.Attributes;

namespace Blazor.Supabase.Models.Entities;

[Table("crepes")]
public class Crepe : BaseEntity
{
    [Column("name")]	
	public string Name { get; set; }

	[Column("price")]
	public float Price { get; set; }

	[Column("image_url")]
	public string ImageUrl { get; set; }

	[Reference(typeof(Ingredient), ReferenceAttribute.JoinType.Left)]
	public List<Ingredient> Ingredients { get; set; } = new();
}