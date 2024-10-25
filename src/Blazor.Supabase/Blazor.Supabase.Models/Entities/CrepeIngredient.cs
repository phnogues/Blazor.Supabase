using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Blazor.Supabase.Models.Entities;

[Table("crepes_ingredients")]
public class CrepeIngredient : BaseModel
{
    [Column("crepe_id")]
    public long CrepeId { get; set; }

    [Column("ingredient_id")]
    public long IngredientId { get; set; }
}