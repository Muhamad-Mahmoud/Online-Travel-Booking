using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
