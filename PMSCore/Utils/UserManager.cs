using Microsoft.Extensions.Logging;
using PMSCore;

namespace PMSCore.Utils
{
    internal class UserManager

    {
        private User _currentUser; // Stores the currently logged-in user.
        private readonly ILogger _logger; // Logger for the UserManager class.

        /// <summary>
        /// Constructor for UserManager. Initializes the logger and attempts to log in the user.
        /// If login fails, it initiates user registration.
        /// </summary>
        /// <param name="logger">Optional logger instance.</param>
        internal UserManager(ILogger? logger)
        {
            // Use the provided logger or create a new one if null.
            _logger = logger ?? new LoggerFactory().CreateLogger("User_Tester");
            _logger.LogDebug("Setting up User Manager.");

            // Attempt login; if it fails, register a new user.
            if (!this.Login())
            {
                this.RegisterNewUser();
            }

        }

        /// <summary>
        /// Retrieves the permission level of the current user.
        /// </summary>
        /// <returns>A string representing the user's permissions.</returns>
        public string GetPermission()
        {
            return _currentUser.ViewUserPermissions();
        }

        /// <summary>
        /// Retrieves the user ID of the current user.
        /// </summary>
        /// <returns>A string representing the user ID.</returns>
        public string GetUserID()
        {
            return _currentUser.GetUserID();
        }


        /// <summary>
        /// Simulates the login process. If no user is currently logged in, it creates a test user.
        /// </summary>
        /// <returns>True if login succeeds, false otherwise.</returns>
        public bool Login()
        {
            if(_currentUser == null)
            {
                //Menu.DisplayLoginMenu();
                //userID = PMSUtils.Sanitize(Console.NextLine());
                //userpassword = PMSUtils.Sanitize(Console.NextLine());

                Console.WriteLine("Simulating Login Process..." +
                    "\n----------------------------------------------------------------------" +
                    "\n To Login please enter your username, followed by your password." +
                    "\nUsername: user1" +
                    "\nPassword: *****" +
                    "\n----------------------------------------------------------------------" +
                    "\nUser would enter username and password here to be sent to back end.");

                //*
                // response = StateManager.ValidateUser(username, userpassword);

                // if(response[0] == true && response[1] == true){
                //      this._user = StateManager.LoadUserProfile(username);
                //  }
                //  elif(response[1] == false)
                //  {
                //      _logger.LogWarning("Invalid Password for User Account.{username}", username);
                //  }
                //  else
                //  {
                //      this.Register();
                //  }
                //*

                // Simulates user login by creating a test user and assigning it a new Permission_Object.
                _currentUser = new User_Tester
                    (
                    new LoggerFactory()
                    .CreateLogger("User_Tester"),
                    new Permission_Object(2),
                    "J-Doe"
                    );
            }

            return _currentUser != null; // Return true if user is successfully created.
        }

        /// <summary>
        /// Simulates the registration process for a new user.
        /// </summary>
        /// <returns>True if registration succeeds, false otherwise.</returns>
        public bool RegisterNewUser()
        {
            _logger.LogDebug("Register New User page loaded.");
            // Menu.DisplayRegistrationMenu();

            Console.WriteLine("Simulating Registration process..." +
                "\n----------------------------------------------------------------------" +
                "\n To Register please enter your username, followed by your password." +
                "\nUsername: user1" +
                "\nPassword: *****" +
                "\n----------------------------------------------------------------------");

            //userID = PMSUtils.Sanitize(Console.NextLine());
            //userpassword = PMSUtils.Sanitize(Console.NextLine());


            //if(StateManager.AuthenticateNewUser(userID, userpassword))
            //{
            //_currentUser = StateManager.LoadUserProfile(userID);
            //}


            // Simulates user registration by creating a test user and passing in a test Permission_Object.
            _currentUser = new User_Tester
                (
                new LoggerFactory()
                .CreateLogger("User_Tester"),
                new Permission_Object(2),
                "J-Doe"
                );

            return _currentUser != null; // Return true if user is successfully created.
        }

    }
}
