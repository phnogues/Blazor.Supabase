using Newtonsoft.Json;
using Supabase.Realtime.Models;

namespace BlazorSupabase.Web.Realtime
{
	public class CrepeBroadcast : BaseBroadcast
	{
		[JsonProperty("crepe_name")]
		public string CrepeName { get; set; }
	}
}
