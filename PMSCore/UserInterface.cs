using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PMSCore
{
    /// <summary>
    /// Defines a contract for user management, including authentication and permissions.
    /// </summary>
    public interface IUserInterface
    {
        /// <summary>
        /// Retrieves the unique identifier for the user.
        /// </summary>
        /// <returns>
        /// A string representing the user's unique ID.
        /// </returns>
        public string GetUserID();

        /// <summary>
        /// Authenticates the user with the given username.
        /// </summary>
        /// <param name="userName">The username of the user attempting to log in.</param>
        /// <returns>
        /// <c>true</c> if the login is successful; otherwise, <c>false</c>.
        /// </returns>
        public bool Login(string userName);

        /// <summary>
        /// Registers a new user with the given username.
        /// </summary>
        /// <param name="userName">The username for the new user.</param>
        /// <returns>
        /// <c>true</c> if registration is successful; otherwise, <c>false</c>.
        /// </returns>
        public bool Register(string userName);

        /// <summary>
        /// Assigns a specific permission to the user.
        /// </summary>
        /// <param name="permission">The permission object to be assigned to the user.</param>
        public void SetPermission(Permission permission);

        /// <summary>
        /// Retrieves the user's assigned permission.
        /// </summary>
        /// <returns>
        /// A <see cref="Permission"/> object representing the user's permission.
        /// </returns>
        public Permission GetPermission();
    }
}
