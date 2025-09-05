public static class AirportDictionary
{
    public static readonly Dictionary<string, Dictionary<string, string>> Cities = new()
    {
        { "UK", new Dictionary<string, string>
            {
                { "LHR", "EGLL" }, // London Heathrow
                { "LGW", "EGKK" }, // London Gatwick
                { "STN", "EGSS" }, // London Stansted
                { "MAN", "EGCC" }, // Manchester
                { "EDI", "EGPH" }, // Edinburgh
            }
        },
        { "Spain", new Dictionary<string, string>
            {
                { "MAD", "LEMD" }, // Madrid
                { "BCN", "LEBL" }, // Barcelona
                { "AGP", "LEMG" }, // Malaga
                { "PMI", "LEPA" }, // Palma de Mallorca
                { "SVQ", "LEZL" }, // Seville
            }
        },
        { "Germany", new Dictionary<string, string>
            {
                { "FRA", "EDDF" }, // Frankfurt
                { "MUC", "EDDM" }, // Munich
                { "BER", "EDDB" }, // Berlin Brandenburg
                { "DUS", "EDDL" }, // Düsseldorf
                { "HAM", "EDDH" }, // Hamburg
            }
        },
        { "France", new Dictionary<string, string>
            {
                { "CDG", "LFPG" }, // Paris Charles de Gaulle
                { "ORY", "LFPO" }, // Paris Orly
                { "NCE", "LFMN" }, // Nice
                { "LYS", "LFLL" }, // Lyon
                { "MRS", "LFML" }, // Marseille
            }
        },
        { "USA", new Dictionary<string, string>
            {
                { "LAX", "KLAX" }, // Los Angeles
                { "JFK", "KJFK" }, // New York JFK
                { "ORD", "KORD" }, // Chicago O'Hare
                { "ATL", "KATL" }, // Atlanta
                { "DFW", "KDFW" }, // Dallas-Fort Worth
                { "SFO", "KSFO" }, // San Francisco
                { "MIA", "KMIA" }, // Miami
            }
        }
    };
}