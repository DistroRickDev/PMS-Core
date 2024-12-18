// UserBase.cs
namespace PMSCore
{
    /// <summary>
    /// Represents the base class for all user types.
    /// </summary>
    public abstract class UserBase : IUser
    {
        public string UserID { get; private set; }
        public string UserName { get; set; } // Changed to set for updates
        public HashSet<Permission> UserPermissions { get; private set; } // Updated to use HashSet

        /// <summary>
        /// Initializes a UserBase with a username and permissions.
        /// </summary>
        public UserBase(string userName, HashSet<Permission> permissions)
        {
            UserID = Guid.NewGuid().ToString();
            UserName = userName;
            UserPermissions = permissions ?? new HashSet<Permission>();
        }

        public string GetUserID() => UserID;

        public bool Login(string userName)
        {
            // Placeholder until UserManager is implemented
            return !string.IsNullOrWhiteSpace(userName) && userName == UserName;
        }

        public void AddPermission(Permission permission)
        {
            if (Enum.IsDefined(typeof(Permission), permission))
            {
                UserPermissions.Add(permission);
            }
        }

        public void RemovePermission(Permission permission)
        {
            UserPermissions.Remove(permission);
        }

        public bool HasPermission(Permission permission) => UserPermissions.Contains(permission);
    }
}