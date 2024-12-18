// Admin.cs
namespace PMSCore
{
    /// <summary>
    /// The Admin user responsible for managing other users.
    /// </summary>
    public class Admin : UserBase
    {
        private readonly StateManager _stateManager;

        /// <summary>
        /// Initializes an Admin with a username and permissions.
        /// </summary>
        public Admin(string userName) : base(userName, new HashSet<Permission> { Permission.ADMIN })
        {
            _stateManager = StateManager.GetInstance();
        }

        /// <summary>
        /// Creates a new user with a username and permissions.
        /// </summary>
        public UserBase? CreateUser(string userName, HashSet<Permission> permissions)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                Console.WriteLine("ERROR: Username cannot be empty.");
                return null;
            }

            if (permissions.Contains(Permission.ADMIN))
            {
                Console.WriteLine("ERROR: Cannot assign ADMIN permission.");
                return null;
            }

            UserBase newUser = new Tester(userName); // Default to Tester, changeable per requirement
            _stateManager.AddUser(newUser);
            Console.WriteLine($"INFO: User {newUser.UserName} created with permissions {string.Join(", ", permissions)}.");
            return newUser;
        }

        /// <summary>
        /// Deletes a specified user using StateManager.
        /// </summary>
        public bool DeleteUser(UserBase user)
        {
            if (_stateManager.RemoveUser(user))
            {
                Console.WriteLine($"INFO: User {user.UserName} has been removed.");
                return true;
            }

            Console.WriteLine("ERROR: Specified user not found.");
            return false;
        }

        /// <summary>
        /// Lists all users managed via StateManager.
        /// </summary>
        public List<UserBase> ListUsers() => _stateManager.GetUsers();

        /// <summary>
        /// Updates an existing user's information via StateManager.
        /// </summary>
        public bool UpdateUser(UserBase user, string newUserName, HashSet<Permission> newPermissions)
        {
            if (!_stateManager.UserExists(user))
            {
                Console.WriteLine("ERROR: Specified user not found.");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(newUserName))
            {
                user.UserName = newUserName;
                Console.WriteLine($"INFO: Updated username for user {user.GetUserID()} to {newUserName}.");
            }

            if (newPermissions != null && newPermissions.Count > 0)
            {
                user.UserPermissions = newPermissions;
                Console.WriteLine($"INFO: Updated permissions for user {user.GetUserID()}.");
            }

            _stateManager.UpdateUser(user);
            return true;
        }
    }
}