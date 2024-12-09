namespace PMSCore
{
    /// <summary>
    /// Represents a user's access permission levels.
    /// Uses an enum representing the access permission level.
    /// Uses a string to represent the access permission level's description.
    /// </summary>
    public class Permission
    {
        /// <summary>
        /// Enum representing the different levels of user access.
        /// Ranges from 0 (Default - No Access) to 5 (Superuser - Total Access).
        /// <summary>
        public enum PermissionLevel
        {
            DEFAULT = 0,
            USER = 1,
            EMPLOYEE = 2,
            MANAGER = 3,
            ADMIN = 4,
            SUPERUSER = 5
        }

        /// <summary>
        /// An array of strings describing the different permission levels. 
        /// Applied to Permission on Construction based on access level.
        /// </summary>
        public readonly string[] PermissionDescriptions = 
        { 
                "Default Permissions. No User Access.",
                "Basic User Permissions Applied.",
                "Employee Permissions Applied",
                "Managerial Permissions Applied.",
                "Administrative Permissions Applied.",
                "Full Permissions Applied."
        };

        /// <summary>
        /// Declares the Permission variables, an enum from PermissionLevels and a PermissionDescription.
        /// Variables set to private and readonly for encapsulation and security purposes.
        /// <summary>
        private readonly PermissionLevel Level;
        private readonly string Description;

        /// <summary>
        /// Initializes a new instance of the <see cref="Permission"/> class with default values.
        /// </summary>
        public Permission()
        {
            this.Level = PermissionLevel.DEFAULT;
            this.Description = PermissionDescriptions[0];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Permission"/> class with input values.
        /// </summary>
        /// <param name="Level">The permission level. Must be valid enum contained in PermissionLevels.</param>
        public Permission(int level)
        {
            try
            {
                // Check if the integer is a valid PermissionLevel
                if (!Enum.IsDefined(typeof(PermissionLevel), level))
                    throw new ArgumentOutOfRangeException(nameof(level), $"The value {level} is not a valid PermissionLevel.");

                // Assign PermissionLevel.
                this.Level = (PermissionLevel)level;

                // Assign description value.
                this.Description = PermissionDescriptions[level];

                // Print permission values to console.
                // TODO: Update to implement logging.
                Console.WriteLine($"Permission created: {this}");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // Catch ArgumentOutOfRangeException and print to console.
                // TODO: Update to use Logging.
                Console.WriteLine(ex);
            }
            catch (Exception ex)
            {
                // Catch any other general exceptions and print to console.
                // TODO: Update to use Logging.
                Console.WriteLine(ex);
            }   
        }
        
        /// <summary>
        /// Gets the name and level of the permission.
        /// A higher value indicates more access or privileges.
        /// </summary>
        public PermissionLevel GetPermissionLevel()
        {
            return this.Level;
        }

        /// <summary>
        /// Gets the description of the permission.
        /// Provides additional context about what the permission level entails.
        /// </summary>
        public string GetPermissionDescription()
        {
            return this.Description;
        }

        /// <summary>
        /// Returns a string representation of the permission.
        /// </summary>
        /// <returns>A string combining the permission level and description.</returns>
        public override string ToString()
        {
            return $"Level {(int)Level}: {Description}";
        }

    }
}