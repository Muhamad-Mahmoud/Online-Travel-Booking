using Mapster;

namespace OnlineTravel.Application.Mapping
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
