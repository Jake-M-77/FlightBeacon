public class LoginHelper
{
    public LoginClass LoginScreenAsync()
    {
        //I dont believe this needs a try catch, as this is just user input
        //However, I will add it just in case.
        try
        {
            var loginData = new LoginClass();
            Console.Write("Enter Client ID: ");
            loginData.ClientId = Console.ReadLine();

            Console.Write("Enter Client Secret: ");
            loginData.ClientSecret = Console.ReadLine();

            return loginData;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }



    }

}