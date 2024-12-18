// StateManager.cs
namespace PMSCore
{
    /// <summary>
    /// Manages application state, including users.
    /// </summary>
    public class StateManager
    {
        private static StateManager? _instance;
        private readonly List<UserBase> _users;

        private StateManager()
        {
            _users = new List<UserBase>();
        }

        /// <summary>
        /// Gets the singleton instance of StateManager.
        /// </summary>
        public static StateManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new StateManager();
            }
            return _instance;
        }

        /// <summary>
        /// Adds a new user to the state.
        /// </summary>
        public void AddUser(UserBase user)
        {
            _users.Add(user);
        }

        /// <summary>
        /// Removes a user from the state.
        /// </summary>
        public bool RemoveUser(UserBase user)
        {
            return _users.Remove(user);
        }

        /// <summary>
        /// Gets all users in the state.
        /// </summary>
        public List<UserBase> GetUsers()
        {
            return new List<UserBase>(_users);
        }

        /// <summary>
        /// Updates a user's information in the state.
        /// </summary>
        public void UpdateUser(UserBase user)
        {
            // Logic to update the user in state (if necessary)
        }

        /// <summary>
        /// Checks if a user exists in the state.
        /// </summary>
        public bool UserExists(UserBase user)
        {
            return _users.Contains(user);
        }
    }
}
