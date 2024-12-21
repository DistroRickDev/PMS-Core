namespace PMSCore;

internal enum CurrentMenu
{
    StartUpMenu = 0,
    LoginMenu,
    RegisterMenu,
    MainMenu,
}

public static class PmsCoreMenu
{
    static PmsCoreMenu()
    {
        Console.CancelKeyPress += ShutDownEventHandler;
        Console.Title = "PMS Core";
    }

    public static void StartMenu()
    {
        _currentMenu = CurrentMenu.StartUpMenu;
        Console.Clear();
        Console.WriteLine(Header);
        Console.WriteLine();
        Console.WriteLine("Welcome to PMS Core! Please select an option:");
        Console.WriteLine();
        Console.WriteLine("1) Login");
        Console.WriteLine("2) Register");
        Console.WriteLine("3) Exit");
        try
        {
            var option = int.Parse(Console.ReadLine() ?? "3");
            switch (option)
            {
                case 1:
                    LogInMenu();
                    break;
                case 2:
                    RegisterMenu();
                    break;
                case 3:
                default:
                    OnExit();
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            OnExit();
        }
    }

    private static void LogInMenu()
    {
        _currentMenu = CurrentMenu.LoginMenu;
        Console.Clear();
        Console.WriteLine(Header);
        Console.WriteLine();
        Console.WriteLine("LogIn Menu");
        Console.WriteLine();
        Console.WriteLine("Please enter the userId you wish to log in:");
        var userId = Console.ReadLine();
        if (string.IsNullOrEmpty(userId))
        {
            Console.WriteLine("Please enter a valid user id... press any key to continue");
            Console.ReadKey();
            StartMenu();
        }

        var loginStatus = StateManager.GetInstance().UserLogin(userId!);
        if (loginStatus != EntityState.Ok)
        {
            Console.WriteLine($"Log In failed with status {loginStatus}... press any key to continue");
            Console.ReadKey();
            StartMenu();
        }

        Console.WriteLine($"Log In Successful with user: {userId}... press any key to continue");
        Console.ReadKey();
    }

    private static void RegisterMenu()
    {
        _currentMenu = CurrentMenu.RegisterMenu;
        Console.Clear();
        Console.WriteLine(Header);
        Console.WriteLine();
        Console.WriteLine("Register Menu");
        Console.WriteLine();
        Console.WriteLine("Please enter the userId you wish to register:");
        var userId = Console.ReadLine();
        if (string.IsNullOrEmpty(userId))
        {
            Console.WriteLine("Please enter a valid user id...press any key to continue");
            Console.ReadKey();
            StartMenu();
        }

        // var registerStatus = StateManager.GetInstance().UserRegister(userId!);
        StartMenu();
    }

    private static void OnExit()
    {
        Console.Clear();
        Console.WriteLine(Header);
        Console.WriteLine();
        Console.WriteLine("Selected exit option, storing data to persistence ...");
        StateManager.GetInstance().StoreToPersistence().Wait();
        Console.WriteLine("Data stored into persistence... press any key to exit...");
        Environment.Exit(0);
    }

    private static void ShutDownEventHandler(object sender, ConsoleCancelEventArgs args)
    {
        args.Cancel = true;
        OnExit();
    }

    private static string Header { get; } = "#### PMS Core! ####";
    private static CurrentMenu _currentMenu = CurrentMenu.StartUpMenu;
}