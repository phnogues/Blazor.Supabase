using Newtonsoft.Json;
using Supabase.Realtime.Models;

namespace BlazorSupabase.Web.Realtime;

public class Order : BaseBroadcast
{
	[JsonProperty("name")]
	public string Name { get; set; }

	[JsonProperty("price")]
	public float Price { get; set; }

	[JsonProperty("date")]
	public DateTime Date { get; set; }
}
