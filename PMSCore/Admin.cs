namespace PMSCore
{
    /// <summary>
    /// Represents the base class for all user types.
    /// </summary>
    public abstract class UserBase : IUser
    {
        public string UserID { get; private set; }
        public string UserName { get; private set; }
        public Permission UserPermission { get; private set; }

        public UserBase(string userName, Permission permission)
        {
            UserID = Guid.NewGuid().ToString();
            UserName = userName;
            UserPermission = permission;
        }

        public string GetUserID() => UserID;

        public bool Login(string userName)
        {
            // Placeholder until UserManager is implemented
            return !string.IsNullOrWhiteSpace(userName) && userName == UserName;
        }

        public void SetPermission(Permission permission)
        {
            if (Enum.IsDefined(typeof(Permission), permission))
            {
                UserPermission = permission;
            }
        }

        public Permission GetPermission() => UserPermission;
    }

    /// <summary>
    /// The Admin user responsible for managing other users.
    /// </summary>
    public class Admin : UserBase
    {
        private readonly List<UserBase> _managedUsers;

        /// <summary>
        /// Initializes an Admin with a username.
        /// </summary>
        public Admin(string userName) : base(userName, Permission.ADMIN)
        {
            _managedUsers = new List<UserBase>();
        }

        /// <summary>
        /// Creates a new user with a username and permission type.
        /// </summary>
        public UserBase? CreateUser(string userName, Permission permission)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                _logError("Username cannot be empty.");
                return null;
            }

            if (permission == Permission.DEFAULT || permission == Permission.ADMIN)
            {
                _logError("Invalid permission type for user creation.");
                return null;
            }

            UserBase newUser = permission switch
            {
                Permission.TESTER => new Tester(userName),
                Permission.DEVELOPER => new Developer(userName),
                Permission.PROJECT_OWNER => new ProjectOwner(userName),
                _ => null
            };

            if (newUser == null)
            {
                _logError("Failed to create user due to unsupported permission type.");
                return null;
            }

            _managedUsers.Add(newUser);
            _logInfo($"User {newUser.UserName} created with role {permission}.");
            return newUser;
        }

        /// <summary>
        /// Deletes a specified user from the list of managed users.
        /// </summary>
        public bool DeleteUser(UserBase user)
        {
            if (_managedUsers.Remove(user))
            {
                _logInfo($"User {user.UserName} has been removed.");
                return true;
            }

            _logError("Specified user not found.");
            return false;
        }

        /// <summary>
        /// Returns a copy of the list of all users managed by the admin.
        /// </summary>
        public List<UserBase> ListUsers() => new List<UserBase>(_managedUsers);

        /// <summary>
        /// Updates an existing user's info.
        /// </summary>
        public bool UpdateUser(UserBase user, string newUserName, Permission newPermission)
        {
            if (!_managedUsers.Contains(user))
            {
                _logError("Specified user not found.");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(newUserName))
            {
                user.UserName = newUserName;
                _logInfo($"Updated username for user {user.GetUserID()} to {newUserName}.");
            }

            if (Enum.IsDefined(typeof(Permission), newPermission) && newPermission != Permission.DEFAULT)
            {
                user.SetPermission(newPermission);
                _logInfo($"Updated permission for user {user.GetUserID()} to {newPermission}.");
                return true;
            }

            _logError("Invalid permission provided.");
            return false;
        }

        // Simple logger methods to replace Console.WriteLine
        private void _logInfo(string message) => Console.WriteLine($"INFO: {message}");
        private void _logError(string message) => Console.WriteLine($"ERROR: {message}");
    }

    // Specific user roles
    public class Tester : UserBase
    {
        public Tester(string userName) : base(userName, Permission.TESTER) { }
    }

    public class Developer : UserBase
    {
        public Developer(string userName) : base(userName, Permission.DEVELOPER) { }
    }

    public class ProjectOwner : UserBase
    {
        public ProjectOwner(string userName) : base(userName, Permission.PROJECT_OWNER) { }
    }
}