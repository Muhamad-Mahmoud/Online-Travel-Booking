using Mapster;

namespace OnlineTravel.Application.Common.Mapping
{
	public static class MapsterConfig
	{
		public static void Register()
		{
			TypeAdapterConfig.GlobalSettings.Scan(
					typeof(MapsterConfig).Assembly
				);

		}
	}
}
