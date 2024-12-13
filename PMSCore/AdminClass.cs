namespace PMSCore

{
    /// <summary>
    /// Represents the base class for all user types.
    /// </summary>
    public abstract class UserBase : IUser
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public Permission UserPermission { get; set; }

        public UserBase(string userName, Permission permission)
        {
            UserID = Guid.NewGuid().ToString();
            UserName = userName;
            UserPermission = permission;
        }

        public string GetUserID()
        {
            return UserID;
        }

        public bool Login(string userName)
        {
            return userName == UserName; //Placeholder until UserManager is done
        }

        public bool Register(string userName)
        {
            return false; // Also a placeholder
        }

        public void SetPermission(Permission permission)
        {
            UserPermission = permission;
        }

        public Permission GetPermission()
        {
            return UserPermission;
        }
    }

    /// <summary>
    /// The Admin user responsible for managing other users
    /// </summary>
    public class Admin : UserBase
    {
        public List<UserBase> ManagedUsers { get; private set; }

        /// <summary>
        /// Initializes an Admin with a username & a permission type.
        /// </summary>
        public Admin(string userName)
            : base(userName, Permission.ADMIN)
        {
            ManagedUsers = new List<UserBase>();
        }

        /// <summary>
        /// Creates a new user with a username and permission type.
        /// </summary>
        public UserBase? CreateUser(string userName, Permission permission)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                Console.WriteLine("Username can't be empty");
                return null;
            }

            if (permission == Permission.DEFAULT || permission == Permission.ADMIN)
            {
                Console.WriteLine("Invalid permission type for user creation.");
                return null;
            }

            UserBase? newUser = permission switch
            {
                Permission.TESTER => new Tester(userName),
                Permission.DEVELOPER => new Developer(userName),
                Permission.PROJECT_OWNER => new ProjectOwner(userName),
                _ => null
            };

            if (newUser == null)
            {
                Console.WriteLine("Failed to create user due to unsupported permission type.");
                return null;
            }

            ManagedUsers.Add(newUser);
            Console.WriteLine($"User {newUser.UserName} created with role {permission}.");
            return newUser;
        }

        /// <summary>
        /// Deletes a specified user from the list of managed users.
        /// </summary>
        public bool DeleteUser(UserBase user)
        {
            if (ManagedUsers.Remove(user))
            {
                Console.WriteLine($"User {user.UserName} has been removed.");
                return true;
            }

            Console.WriteLine("Specified user not found.");
            return false;
        }

        /// <summary>
        /// Returns a copy of the list of all users managed by the admin.
        /// </summary>
        public List<UserBase> ListUsers()
        {
            return new List<UserBase>(ManagedUsers);
        }

        /// <summary>
        /// Updates an existing user's info
        /// </summary>
        public bool UpdateUser(UserBase user, string newUserName, Permission newPermission)
        {
            if (!ManagedUsers.Contains(user))
            {
                Console.WriteLine("Specified user not found.");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(newUserName))
            {
                user.UserName = newUserName;
                Console.WriteLine($"Updated username for user {user.UserID} to {newUserName}.");
            }

            if (Enum.IsDefined(typeof(Permission), newPermission) && newPermission != Permission.DEFAULT)
            {
                user.SetPermission(newPermission);
                Console.WriteLine($"Updated permission for user {user.UserID} to {newPermission}.");
            }
            else
            {
                Console.WriteLine("Invalid permission provided.");
                return false;
            }

            return true;
        }
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
