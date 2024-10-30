using Supabase.Postgrest.Attributes;

namespace BlazorSupabase.Models.Entities;

[Table("ingredients")]
public class Ingredient : BaseEntity
{
	[Column("name")]
	public string Name { get; set; }
}
