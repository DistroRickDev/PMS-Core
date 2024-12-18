// UserBase.cs
namespace PMSCore
{
    /// <summary>
    /// Represents the base class for all user types.
    /// </summary>
    public abstract class UserBase : IUser
    {
        public string UserID { get; private set; }
        public string UserName { get; private set; }
        public HashSet<Permission> UserPermissions { get; private set; }

        /// <summary>
        /// Initializes a UserBase with a username and permissions.
        /// Permissions are immutable once set in the constructor.
        /// </summary>
        public UserBase(string userName, HashSet<Permission> permissions)
        {
            UserID = Guid.NewGuid().ToString();
            UserName = userName;
            UserPermissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
        }

        public string GetUserID() => UserID;

        public bool HasPermission(Permission permission)
        {
            return UserPermissions.Contains(permission);
        }

        /// <summary>
        /// Default implementations of IUser methods for entity management.
        /// Can be overridden by derived classes if needed.
        /// </summary>
        public virtual void CreateEntity(string id, string description, EntityType entityType)
        {
            EntityManager.GetInstance().CreateInstance(id, description, entityType);
        }

        public virtual void ChangeEntityProperty(string entityId, string property, string newValue)
        {
            EntityManager.GetInstance().ChangeEntityProperty(entityId, property, newValue);
        }

        public virtual void ChangeEntityStatus(string entityId, EntityStatus newStatus)
        {
            EntityManager.GetInstance().ChangeEntityStatus(entityId, newStatus);
        }

        public virtual void DeleteEntity(string entityId)
        {
            EntityManager.GetInstance().DeleteInstance(entityId);
        }

        public virtual void AssociateWithEntity(string entityId)
        {
            StateManager.GetInstance().AssociateEntityToUser(this, entityId);
        }

        public virtual void DisassociateWithEntity(string entityId)
        {
            StateManager.GetInstance().DisassociateEntityFromUser(this, entityId);
        }

        public virtual void AssociateEntityToEntity(string sourceEntityId, string targetEntityId)
        {
            StateManager.GetInstance().AssociateEntityToEntity(sourceEntityId, targetEntityId);
        }

        public virtual void DissociateEntityFromEntity(string sourceEntityId, string targetEntityId)
        {
            StateManager.GetInstance().DissociateEntityFromEntity(sourceEntityId, targetEntityId);
        }
    }
}