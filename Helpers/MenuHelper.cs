using System.Threading.Tasks;

public class MenuHelper
{
    
    private readonly OpenSkyService _OSS;

    public MenuHelper(OpenSkyService OSS)
    {
        _OSS = OSS;
    }

    public async Task DisplayMenu()
    {

        Console.WriteLine("==- Welcome to FlightBeacon! -==");
        Console.WriteLine("1. Airport Menu");
        Console.WriteLine("2.");
        Console.WriteLine("3.");

        Console.Write("Please choose a menu: ");
        string response = Console.ReadLine();

        if (response == "1")
        {
            await AirportMenu();
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

        if (response == "1")
        {
            await GetDepartures(ICAO24);
        }
    }

    public async Task GetDepartures(string ICAO24)
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

            

            var departures = await _OSS.GetDeparturesByAirportAsync(airport: ICAO24, begin: epoch_df, end: epoch_dt);

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
        else
        {
            Console.WriteLine("Invalid date or time format in either timefrom or timeto!");
        }
    }
}