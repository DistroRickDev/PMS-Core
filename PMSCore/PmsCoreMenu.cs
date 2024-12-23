namespace PMSCore;

public static class PmsCoreMenu
{
    static PmsCoreMenu()
    {
        Console.CancelKeyPress += ShutDownEventHandler;
        Console.Title = "PMS Core";
    }

    private static string? GetIdFromInput(string displayMessage)
    {
        Console.WriteLine(displayMessage);
        var userId = Console.ReadLine();
        if (!string.IsNullOrEmpty(userId)) return userId;
        Console.WriteLine("Please enter a valid user id... press any key to continue");
        Console.ReadKey();
        return null;
    }


    private static void MenuHeader(string title)
    {
        Console.Clear();
        Console.WriteLine(Header);
        Console.WriteLine();
        Console.WriteLine(title);
        Console.WriteLine();
    }

    public static void StartMenu()
    {
        MenuHeader("Start-Up Menu:");
        Console.WriteLine("Please select an option:");
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
        MenuHeader("Login Menu:");
        var userId = GetIdFromInput("Please enter the userId you wish to log in:");
        if (userId == null)
        {
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
        MainMenu();
    }

    private static void RemoveUserMenuItem()
    {
        var userId = GetIdFromInput($"Please enter a user id to be removed");
        if (userId == null)
        {
            MainMenu();
        }

        Console.WriteLine($"Attempting to remove user {userId}...");
        Console.WriteLine(StateManager.GetInstance().GetCurrentUser().DeleteUser(userId!) == UserOperationResult.Ok
            ? $"User {userId} removed successfully... press any key to continue"
            : $"Removing user {userId} failed... press any key to continue");
        Console.ReadKey();
        MainMenu();
    }

    private static void AddRemoveUserPermission(bool removePermission = false)
    {
        var operation = removePermission ? "remove" : "add";
        var userId = GetIdFromInput($"Please enter a user id to {operation} permission:");
        if (userId == null)
        {
            MainMenu();
        }

        Console.WriteLine("Please select the desired permission:");
        foreach (var permission in Enum.GetValues(typeof(Permission)).Cast<Permission>())
        {
            Console.WriteLine($"{permission} - {permission.ToString()}");
        }

        try
        {
            Enum.TryParse(Console.ReadLine(), out Permission permission);
            var currentUser = StateManager.GetInstance().GetCurrentUser();
            var res = removePermission
                ? currentUser.RemoveUserPermission(userId!, permission)
                : currentUser.AddUserPermission(userId!, permission);
            if (res == UserOperationResult.Ok)
            {
                Console.WriteLine($"{operation} permission to ${userId} successfully changed");
            }
            else
            {
                Console.WriteLine($"{operation} permission to ${userId} failed");
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        MainMenu();
    }

    private static void GenerateUserReport()
    {
        var userId = GetIdFromInput("Please enter a user id to generate the report for:");

        if (userId == null)
        {
            UserManagementMenu();
        }

        var res = StateManager.GetInstance().GetCurrentUser().GenerateUserReport(userId!);
        if (res is UserOperationResult result)
        {
            Console.WriteLine(
                $"User report generation failed for userId ${userId} with reason {result.ToString()}, press any key to continue");
            Console.ReadKey();
            MainMenu();
        }

        Console.Clear();
        Console.WriteLine($"User report generation succeeded for userId ${userId}.");
        Console.WriteLine("#### REPORT:");
        Console.WriteLine((string)res);
        Console.WriteLine("Press any key to continue");
        Console.ReadKey();
        MainMenu();
    }

    private static void UserManagementMenu()
    {
        MenuHeader($"User Management Menu, [{StateManager.GetInstance().GetCurrentUser().GetUserId()}]:");
        Console.WriteLine("Please select an option:");
        Console.WriteLine();
        Console.WriteLine("1) Remove User");
        Console.WriteLine("2) Add User Permission");
        Console.WriteLine("3) Remove User Permission");
        Console.WriteLine("4) Generate User Report");
        Console.WriteLine("5) Back to Main Menu");
        Console.WriteLine("6) Exit");
        try
        {
            var option = int.Parse(Console.ReadLine() ?? "6");
            switch (option)
            {
                case 1:
                    RemoveUserMenuItem();
                    break;
                case 2:
                    AddRemoveUserPermission();
                    break;
                case 3:
                    AddRemoveUserPermission(true);
                    break;
                case 4:
                    GenerateUserReport();
                    break;
                case 5:
                    MainMenu();
                    break;
                case 6:
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        OnExit();
    }

    private static void AddRemoveEntity(EntityType entityType, bool removeEntity)
    {
        var action = removeEntity ? "remove" : "add";
        var entityId = GetIdFromInput($"Please enter an entity id to {action}:");
        if (entityId == null)
        {
            Console.WriteLine("Please enter a valid entity id to be removed... press any key to continue");
            Console.ReadKey();
            EntityManagementMenu(entityType);
        }

        var result = () =>
        {
            if (removeEntity)
            {
                return StateManager.GetInstance().RemoveEntity(entityId!);
            }

            Console.WriteLine("Please enter a description to be added to the entity");
            var description = Console.ReadLine();
            return StateManager.GetInstance().CreateEntity(entityType, entityId!, description);
        };
        Console.WriteLine(result.Invoke() == EntityState.Ok
            ? $"Entity {entityId} {action}ed successfully... press any key to continue"
            : $"Failed to {action} {entityId}... press any key to continue");
        Console.ReadKey();
        EntityManagementMenu(entityType);
    }

    private static void ChangeEntityProperty(EntityType entityType)
    {
        var entityId = GetIdFromInput($"Please enter an entity id to change property:");
        if (entityId == null)
        {
            Console.WriteLine("Please enter a valid entity id to be updated... press any key to continue");
            Console.ReadKey();
            EntityManagementMenu(entityType);
        }

        Console.WriteLine("Please select an option:");
        Console.WriteLine();
        Console.WriteLine("1) Change entity description");
        Console.WriteLine("2) Change entity status");
        Console.WriteLine("3) Change entity priority");
        try
        {
            var option = int.Parse(Console.ReadLine() ?? "1");
            switch (option)
            {
                case 1:
                    Console.WriteLine("Please enter a description to be changed to the entity");
                    var description = Console.ReadLine();
                    StateManager.GetInstance().GetCurrentUser().ChangeEntityProperty(entityType, entityId!,
                        EntityProperty.Description, description);
                    break;
                case 2:
                    Console.WriteLine("Please enter a new status to be changed to the entity");
                    try
                    {
                        Enum.TryParse(Console.ReadLine(), out EntityStatus status);
                        StateManager.GetInstance().GetCurrentUser().ChangeEntityProperty(entityType, entityId!,
                            EntityProperty.Description, status);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        EntityManagementMenu(entityType);
                    }

                    break;
                case 3:
                    Console.WriteLine("Please enter a new priority to be changed to the entity");
                    try
                    {
                        Enum.TryParse(Console.ReadLine(), out EntityPriority priority);
                        StateManager.GetInstance().GetCurrentUser().ChangeEntityProperty(entityType, entityId!,
                            EntityProperty.EntityPriority, priority);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        EntityManagementMenu(entityType);
                    }

                    break;
            }

            EntityManagementMenu(entityType);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private static void AssociateDisassociateToEntity(EntityType entityType, bool disassociate)
    {
        var action = disassociate ? "disassociate" : "associate";
        var currentUser = StateManager.GetInstance().GetCurrentUser();
        Console.WriteLine($"Please enter a user to ${action} with with (default [{currentUser.GetUserId()}]");
        var userId = Console.ReadLine();
        userId = string.IsNullOrEmpty(userId) ? currentUser.GetUserId() : userId;
        var entityId = GetIdFromInput($"Please enter an entity id to {action} with {userId}:");
        if (entityId == null)
        {
            Console.WriteLine($"Please enter a valid entity id to be {action}... press any key to continue");
            Console.ReadKey();
            EntityManagementMenu(entityType);
        }

        var result = disassociate
            ? currentUser.DisassociateUserWithEntity(userId, entityId!)
            : currentUser.AssociateUserWithEntity(userId, entityId!);
        Console.WriteLine(result == UserOperationResult.Ok
            ? $"Successfully {action}ed {userId} with {entityId}"
            : $"Failed to {action} {userId} with {entityId}" + "... press any key to continue");
        Console.ReadKey();
        EntityManagementMenu(entityType);
    }


    private static void AssociateDisassociateEntityToEntity(EntityType entityType, bool disassociate)
    {
        var action = disassociate ? "disassociate" : "associate";
        var currentUser = StateManager.GetInstance().GetCurrentUser();
        var entityIdA = GetIdFromInput("Please enter an entity id to for association");
        var entityIdB = GetIdFromInput("Please enter an second entity id to for association");
        if (entityIdA == null || entityIdB == null)
        {
            Console.WriteLine(
                $"Please enter a valid two valid entity ids for association... press any key to continue");
            Console.ReadKey();
            EntityManagementMenu(entityType);
        }

        var result = disassociate
            ? currentUser.DissociateEntityFromEntity(entityIdA!, entityIdB!)
            : currentUser.AssociateEntityToEntity(entityIdA!, entityIdB!);
        Console.WriteLine(result == UserOperationResult.Ok
            ? $"Successfully {action}ed {entityIdA} with {entityIdB}"
            : $"Failed to {action} {entityIdA} with {entityIdB}" + "... press any key to continue");
        Console.ReadKey();
        EntityManagementMenu(entityType);
    }

    private static void EntityManagementMenu(EntityType entityType)
    {
        var entityTypeString = entityType.ToString();
        MenuHeader($"{entityTypeString} Menu [{StateManager.GetInstance().GetCurrentUser().GetUserId()}]:");
        Console.WriteLine("Please select an option:");
        Console.WriteLine();
        Console.WriteLine($"1) Add {entityTypeString}");
        Console.WriteLine($"2) Remove {entityTypeString}");
        Console.WriteLine($"3) Change {entityTypeString} property");
        Console.WriteLine($"4) Associate User to {entityTypeString}");
        Console.WriteLine($"5) Disassociate User from {entityTypeString}");
        Console.WriteLine("6) Associate Project to Task");
        Console.WriteLine("7) Disassociate Project from Task");
        Console.WriteLine("8) Back to Main Menu");
        Console.WriteLine("9) Exit");
        try
        {
            var option = int.Parse(Console.ReadLine() ?? "9");
            switch (option)
            {
                case 1:
                    AddRemoveEntity(entityType, false);
                    break;
                case 2:
                    AddRemoveEntity(entityType, true);
                    break;
                case 3:
                    ChangeEntityProperty(entityType);
                    break;
                case 4:
                    AssociateDisassociateToEntity(entityType, false);
                    break;
                case 5:
                    AssociateDisassociateToEntity(entityType, true);
                    break;
                case 6:
                    AssociateDisassociateEntityToEntity(entityType, false);
                    break;
                case 7:
                    AssociateDisassociateEntityToEntity(entityType, true);
                    break;
                case 8:
                    MainMenu();
                    break;
                case 9:
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        OnExit();
    }

    private static void MainMenu()
    {
        MenuHeader($"User Menu, welcome [{StateManager.GetInstance().GetCurrentUser().GetUserId()}]:");
        Console.WriteLine("Please select an option:");
        Console.WriteLine();
        Console.WriteLine("1) User Management");
        Console.WriteLine("2) Project Management");
        Console.WriteLine("3) Task Management");
        Console.WriteLine("4) LogOff");
        Console.WriteLine("5) Exit");
        try
        {
            var option = int.Parse(Console.ReadLine() ?? "3");
            switch (option)
            {
                case 1:
                    UserManagementMenu();
                    break;
                case 2:
                    EntityManagementMenu(EntityType.Project);
                    break;
                case 3:
                    EntityManagementMenu(EntityType.Task);
                    break;
                case 4:
                    StateManager.GetInstance().UserLogout();
                    StartMenu();
                    break;
                case 5:
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

    private static void RegisterMenu()
    {
        MenuHeader("Register Menu:");
        var userId = GetIdFromInput("Please enter the userId you wish to register:");
        if (userId == null)
        {
            StartMenu();
        }

        IUser? userToRegister = CreateUserFromInput(userId!);
        if (userToRegister == null)
        {
            Console.WriteLine("Failed to create user to register... press any key to continue");
            Console.ReadKey();
            StartMenu();
        }

        var registerStatus = StateManager.GetInstance().UserRegister(userId!, userToRegister!);
        if (registerStatus != EntityState.Ok)
        {
            Console.WriteLine(
                $"Failed to register user: {userId} with status ${registerStatus.ToString()}... press any key to continue");
            Console.ReadKey();
            StartMenu();
        }

        Console.WriteLine($"Register Successful with user: {userId}... press any key to continue");
        Console.ReadKey();
        MainMenu();
    }

    private static IUser? CreateUserFromInput(string userId)
    {
        Console.WriteLine("Type in the type of user you wish to register:");
        Console.WriteLine($"{UserType.Admin}");
        Console.WriteLine($"{UserType.Developer}");
        Console.WriteLine($"{UserType.ProjectManager}");
        Console.WriteLine($"{UserType.Tester}");

        Enum.TryParse<UserType>(Console.ReadLine(), out var type);
        switch (type)
        {
            case UserType.Admin:
                return new Admin(userId);
            case UserType.ProjectManager:
                return new ProjectManager(userId);
            case UserType.Developer:
                return new Developer(userId);
            case UserType.Tester:
                return new Tester(userId);
            default:
                Console.WriteLine("Invalid user type, please try again...");
                return null;
        }
    }

    private static void OnExit()
    {
        MenuHeader("Exit Menu:");
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
}