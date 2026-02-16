using GreenPipes.Internals.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravel.Domain.Entities.Flights.ValueObjects
{
    public class FlightMetadata
    {
        public string Gate { get; set; } = string.Empty;
        public string Terminal { get; set; } = string.Empty;
        public string QRCodeData { get; set; } = string.Empty;
        public string AircraftType { get; set; } = string.Empty;

      
        public FlightMetadata() { }

        public FlightMetadata(string gate, string terminal, string qrCodeData, string aircraftType)
        {
            Gate = gate;
            Terminal = terminal;
            QRCodeData = qrCodeData;
            AircraftType = aircraftType;
        }

    }
    }
