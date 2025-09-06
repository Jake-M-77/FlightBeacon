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
        bool Is_Looping = true;

        while (Is_Looping)
        {
            Console.Clear();
            Console.WriteLine("==- Welcome to FlightBeacon! -==");
            Console.WriteLine("1. Airport Menu");
            Console.WriteLine("2. Region Menu");
            Console.WriteLine("3.");

            Console.Write("Please choose a menu: ");
            string response = Console.ReadLine();



            switch (response)
            {
                case "1":
                    Is_Looping = false;
                    await AirportMenu();
                    break;
                case "2":
                    Is_Looping = false;
                    await DisplayDataFromRegion();
                    break;
                case "3":
                    //Future feature
                    break;

            }
        }

    }

    public async Task DisplayDataFromRegion()
    {

        var box = await GetRegionFromUser();
        var states = await openskyservice.GetStatesByBoundingBoxAsync(lamin: box.LatMin, lamax: box.LatMax, lomin: box.LonMin, lomax: box.LonMax);

        if (states.Count == 0)
        {
            Console.WriteLine("No flights returned.");
            await Task.Delay(5);
            await DisplayMenu();
        }
        else
        {
            Console.WriteLine("checkkkk\n");
            foreach (var state in states)
            {
                Console.WriteLine($"Callsign: {state.Callsign}, ICAO: {state.Icao24}, Lat: {state.Latitude}, Lon: {state.Longitude}, Altitude: {state.BaroAltitude}\n");
            }

            await HandleMenuReturn();
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
            Task.Delay(500);

            int answer = await HandleInvalidInputAsync("Region");

            if (answer == 1)
            {
                return await GetRegionFromUser();
            }
            else
            {
                await DisplayMenu();
                return null;
            }


        }
    }


    public async Task AirportMenu()
    {
        int i = 0;
        foreach (var key in AirportDictionary.Cities)
        {
            i++;
            Console.WriteLine($"{i}. {key.Key}");
        }

        Console.Write("Please choose a Country: ");
        string response = Console.ReadLine();

        if (AirportDictionary.Cities.ContainsKey(response))
        {
            await DisplayAirportsByCountry(response);
        }
        else
        {
            Console.WriteLine("Not a valid input!");
            int answer = await HandleInvalidInputAsync("Country");

            if (answer == 1)
            {
                await AirportMenu();
            }
            else
            {
                await DisplayMenu();
            }
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
            else
            {
                Console.WriteLine("Invalid IATA code!");
                await Task.Delay(500);
                int answer = await HandleInvalidInputAsync("Airport");

                if (answer == 1)
                {
                    await DisplayAirportsByCountry(Country);
                }
                else
                {
                    await DisplayMenu();
                }
            }
        }
        else
        {
            Console.WriteLine("No airports found for the selected country.");

            int answer = await HandleInvalidInputAsync("Country");

            if (answer == 1)
            {
                await AirportMenu();
            }
            else
            {
                await DisplayMenu();
            }


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
            case "3":
                await DisplayMenu();
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
            await Task.Delay(5);
            await DisplayMenu();
        }
        else
        {
            foreach (var flight in departures)
            {
                Console.WriteLine($"Callsign: {flight.Callsign}, ICAO: {flight.ICao24}, Departure: {flight.estDepartureAirport}, Arrival: {flight.estArrivalAirport}");
            }

            await HandleMenuReturn();
        }
    }

    public async Task GetArrivalsAsync(string ICAO24)
    {
        var DAH = await GetDepartureArrivalHelperMethod(ICAO24);

        var departures = await openskyservice.GetArrivalsByAirportAsync(airport: ICAO24, begin: DAH.begintime, end: DAH.endtime);

        if (departures.Count == 0)
        {
            Console.WriteLine("No arrivals found for this period.");
            await Task.Delay(5);
            await DisplayMenu();
        }
        else
        {
            foreach (var flight in departures)
            {
                Console.WriteLine($"Callsign: {flight.Callsign}, ICAO: {flight.ICao24}, Departure: {flight.estDepartureAirport}, Arrival: {flight.estArrivalAirport}");
            }

            await HandleMenuReturn();
        }
    }

    public async Task<int> HandleInvalidInputAsync(string method_in_question)
    {
        Console.WriteLine();
        Console.WriteLine(" ==- Retry Menu -==");
        Console.WriteLine($"1. Re-enter a {method_in_question}");
        Console.WriteLine("2. Return to Main Menu");

        try
        {
            int answer = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            return answer;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return await HandleInvalidInputAsync(method_in_question);
        }
    }

    public async Task HandleMenuReturn()
    {
        Console.WriteLine(" ==- Menu selector -== ");
        Console.WriteLine("1. Return to Main Menu");
        Console.WriteLine("2. Quit program");

        try
        {
            int answer = Convert.ToInt32(Console.ReadLine());

            if (answer == 1)
            {
                await DisplayMenu();
            }
            else
            {
                Console.WriteLine("Program closed");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

    }

    public async Task<DepartureArrivalHelper> GetDepartureArrivalHelperMethod(string ICAO24)
    {
        //This method could be alot shorter, but i quite like it like this,
        //where its step by step, plus the time range can only be a 2 hour 
        //difference between start and to time.

        Console.WriteLine("INFO: Time from and Time to, can only be a maximum of two hours apart");

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
            //Bit of a hack, but it works
            await HandleMenuReturn();
            return null;
        }

    }
}

