using Supabase.Postgrest.Attributes;

namespace Blazor.Supabase.Models.Entities;

[Table("ingredients")]
public class Ingredient : BaseEntity
{
	[Column("name")]
	public string Name { get; set; }

	//[Reference(typeof(Crepe))] 
	//public List<Crepe>? Crepes { get; set; } = new();

}
