// StateManager.cs
namespace PMSCore
{
    /// <summary>
    /// Manages application state, including users and entity relationships.
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

        public void AddUser(UserBase user)
        {
            _users.Add(user);
        }

        public bool RemoveUser(UserBase user)
        {
            return _users.Remove(user);
        }

        public List<UserBase> GetUsers()
        {
            return new List<UserBase>(_users);
        }

        public void UpdateUser(UserBase user)
        {
            // Logic to update the user in state if necessary.
        }

        public bool UserExists(UserBase user)
        {
            return _users.Contains(user);
        }

        public void AssociateEntityToUser(UserBase user, string entityId)
        {
            // Logic to associate an entity to a user.
        }

        public void DisassociateEntityFromUser(UserBase user, string entityId)
        {
            // Logic to disassociate an entity from a user.
        }

        public void AssociateEntityToEntity(string sourceEntityId, string targetEntityId)
        {
            // Logic to associate one entity to another.
        }

        public void DissociateEntityFromEntity(string sourceEntityId, string targetEntityId)
        {
            // Logic to dissociate one entity from another.
        }
    }
}