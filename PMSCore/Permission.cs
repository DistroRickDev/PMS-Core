using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSCore
{

    /// <summary>
    /// Represents a user's permission with a level and description.
    /// </summary>
    public class Permission
    {
        /// <summary>
        /// Gets the level of the permission.
        /// A higher value indicates more access or privileges.
        /// </summary>
        public int PermissionLevel { get; private set; }

        /// <summary>
        /// Gets the description of the permission.
        /// Provides additional context about what the permission level entails.
        /// </summary>
        public string PermissionDescription { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Permission"/> class with default values.
        /// </summary>
        public Permission()
        {
            PermissionLevel = 0; // Default permission level
            PermissionDescription = "No permissions assigned.";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Permission"/> class with specific values.
        /// </summary>
        /// <param name="level">The permission level. Must be non-negative.</param>
        /// <param name="description">The description of the permission.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="level"/> is negative.</exception>
        public Permission(int level, string description)
        {
            if (level < 0)
                throw new ArgumentException("Permission level must be a non-negative integer.", nameof(level));

            PermissionLevel = level;
            PermissionDescription = description;
        }

        /// <summary>
        /// Returns a string representation of the permission.
        /// </summary>
        /// <returns>A string combining the permission level and description.</returns>
        public override string ToString()
        {
            return $"Level {PermissionLevel}: {PermissionDescription}";
        }

    }
}