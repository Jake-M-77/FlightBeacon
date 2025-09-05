using System.Threading.Tasks;

public class MenuHelper
{

    private readonly OpenSkyService openskyservice;

    public MenuHelper(OpenSkyService _openskyservice)
    {
        openskyservice = _openskyservice;
    }

    public async Task DisplayMenu()
    {

        Console.WriteLine("==- Welcome to FlightBeacon! -==");
        Console.WriteLine("1. Airport Menu");
        Console.WriteLine("2. Region Menu");
        Console.WriteLine("3.");

        Console.Write("Please choose a menu: ");
        string response = Console.ReadLine();



        switch (response)
        {
            case "1":
                await AirportMenu();
                break;
            case "2":
                await DisplayDataFromRegion();
                break;
            case "3":
            //Future feature
                break;

        }
    }

    public async Task DisplayDataFromRegion()
    {

        var box = await GetRegionFromUser();
        var states = await openskyservice.GetStatesByBoundingBoxAsync(lamin: box.LatMin, lamax: box.LatMax, lomin: box.LonMin, lomax: box.LonMax);

        if (states.Count == 0)
        {
            Console.WriteLine("No flights returned.");
        }
        else
        {
            Console.WriteLine("checkkkk\n");
            foreach (var state in states)
            {
                Console.WriteLine($"Callsign: {state.Callsign}, ICAO: {state.Icao24}, Lat: {state.Latitude}, Lon: {state.Longitude}, Altitude: {state.BaroAltitude}\n");
            }
        }

    }

    public async Task<BoundingBox> GetRegionFromUser()
    {
        Regions regions = new Regions();
        Console.WriteLine("Choose a region:");
        foreach (var key in regions.Presets.Keys)
        {
            Console.WriteLine($" - {key}");
        }

        string response = Console.ReadLine();
        response = response.ToUpper();

        if (regions.Presets.ContainsKey(response))
        {
            var x = regions.Presets.FirstOrDefault(x => x.Key == response);
            Console.WriteLine($"LatMin: {x.Value.LatMin}, LatMax: {x.Value.LatMax}, LonMin: {x.Value.LonMin}, LonMax: {x.Value.LonMax}"); //DEBUG PURPOSES
            
            return x.Value;

        }
        else
        {
            Console.WriteLine("Invalid Region!");
            return null;
        }
    }


    public async Task AirportMenu()
    {
        Console.WriteLine("1. UK");
        Console.WriteLine("2. Spain");
        Console.WriteLine("3. Germany");
        Console.WriteLine("4. France");
        Console.WriteLine("5. USA");

        Console.Write("Please choose a country: ");
        string response = Console.ReadLine();

        switch (response)
        {
            case "1":
                await DisplayAirportsByCountry("UK");
                break;
            case "2":
                await DisplayAirportsByCountry("Spain");
                break;
            case "3":
                await DisplayAirportsByCountry("Germany");
                break;
            case "4":
                await DisplayAirportsByCountry("France");
                break;
            case "5":
                await DisplayAirportsByCountry("USA");
                break;
            default:
                Console.WriteLine("Not a valid input!");
                break;
        }



    }

    public async Task DisplayAirportsByCountry(string Country)
    {
        if (AirportDictionary.Cities.ContainsKey(Country))
        {
            int i = 0;
            Console.WriteLine();
            Console.WriteLine($"==- Major Airports in {Country} -==");
            var airports = AirportDictionary.Cities[Country];
            foreach (var airport in airports)
            {
                i++;
                Console.WriteLine($"{i}. IATA: {airport.Key} - ICAO: {airport.Value}");
            }

            Console.Write("Please choose an airport by its code (IATA): ");
            string response = Console.ReadLine();
            response = response.ToUpper();
            if (airports.ContainsKey(response))
            {
                Console.WriteLine("if loop ran"); //DEBUG PURPOSES
                var result = airports.FirstOrDefault(x => x.Key == response);
                Console.WriteLine($"RESULT: {result.Key} - {result.Value}"); // DEBUG PURPOSES

                await ShowAirportActionsMenu(result.Key, result.Value);

            }
        }
        else
        {
            Console.WriteLine("No airports found for the selected country.");
        }
    }

    public async Task ShowAirportActionsMenu(string IATA, string ICAO24)
    {
        Console.WriteLine($"==- AirportActionsMenu For {IATA} -==");
        Console.WriteLine("1. Get departures");
        Console.WriteLine("2. Get arrivals");
        Console.WriteLine("3. Return to Main Menu");

        Console.Write("Please choose a menu: ");
        string response = Console.ReadLine();

        switch (response)
        {
            case "1":
                await GetDeparturesAsync(ICAO24);
                break;
            case "2":
                await GetArrivalsAsync(ICAO24);
                break;
        }
    }

    public async Task GetDeparturesAsync(string ICAO24)
    {
        var DAH = await GetDepartureArrivalHelperMethod(ICAO24);

        var departures = await openskyservice.GetDeparturesByAirportAsync(airport: ICAO24, begin: DAH.begintime, end: DAH.endtime);

        if (departures.Count == 0)
        {
            Console.WriteLine("No departures found for this period.");
        }
        else
        {
            foreach (var flight in departures)
            {
                Console.WriteLine($"Callsign: {flight.Callsign}, ICAO: {flight.ICao24}, Departure: {flight.estDepartureAirport}, Arrival: {flight.estArrivalAirport}");
            }
        }
    }

    public async Task GetArrivalsAsync(string ICAO24)
    {
        var DAH = await GetDepartureArrivalHelperMethod(ICAO24);

        var departures = await openskyservice.GetArrivalsByAirportAsync(airport: ICAO24, begin: DAH.begintime, end: DAH.endtime);

        if (departures.Count == 0)
        {
            Console.WriteLine("No arrivals found for this period.");
        }
        else
        {
            foreach (var flight in departures)
            {
                Console.WriteLine($"Callsign: {flight.Callsign}, ICAO: {flight.ICao24}, Departure: {flight.estDepartureAirport}, Arrival: {flight.estArrivalAirport}");
            }
        }
    }

    public async Task<DepartureArrivalHelper> GetDepartureArrivalHelperMethod(string ICAO24)
    {
        Console.Write("Please enter Date (YYYY-MM-DD): ");
        string dateinput = Console.ReadLine();

        Console.Write("Please enter time from (HH:mm, 24-hour format): ");
        string fromtime = Console.ReadLine();

        Console.Write("Please enter time to (HH:mm, 24-hour format): ");
        string timeto = Console.ReadLine();

        bool fromdate = DateTime.TryParse($"{dateinput} {fromtime}", out DateTime datefrom);

        bool todate = DateTime.TryParse($"{dateinput} {timeto}", out DateTime dateto);

        if (fromdate && todate)
        {
            datefrom = datefrom.ToUniversalTime();
            dateto = dateto.ToUniversalTime();

            long epoch_datefrom = new DateTimeOffset(datefrom).ToUnixTimeSeconds();

            long epoch_dateto = new DateTimeOffset(dateto).ToUnixTimeSeconds();

            int epoch_df = Convert.ToInt32(epoch_datefrom);

            int epoch_dt = Convert.ToInt32(epoch_dateto);

            Console.WriteLine(epoch_datefrom); // DEBUG PURPOSES
            Console.WriteLine(epoch_dateto); // DEBUG PURPOSES

            DepartureArrivalHelper departureArrivalHelper = new DepartureArrivalHelper()
            {
                Icao24 = ICAO24,
                begintime = epoch_df,
                endtime = epoch_dt
            };

            return departureArrivalHelper;
        }
        else
        {
            Console.WriteLine("Invalid date or time format in either timefrom or timeto!");
            return null;
        }

    }
}

