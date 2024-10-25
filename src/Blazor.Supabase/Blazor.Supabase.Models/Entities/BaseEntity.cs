using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Blazor.Supabase.Models.Entities;

public class BaseEntity : BaseModel
{
	[PrimaryKey("id", false)]
	public long Id { get; set; }

	[Column("created_at")]
	public DateTime CreatedAt { get; set; }
}
