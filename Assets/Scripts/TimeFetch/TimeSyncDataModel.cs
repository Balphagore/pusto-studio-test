namespace PustoStudioTest.TimeFetch
{
	using Newtonsoft.Json;

	public partial class TimeSyncDataModel
	{
		[JsonProperty("time")]
		public long         Time;

		[JsonProperty("clocks")]
		public ClockInfo    Clocks;
	}

	public partial class ClockInfo
	{
		[JsonProperty("213")]
		public TownInfo     Town;
	}

	public partial class TownInfo
	{
		[JsonProperty("offset")]
		public long         Offset;
	}
}
