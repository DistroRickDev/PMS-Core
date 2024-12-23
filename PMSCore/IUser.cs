namespace PMSCore
{
    /// <summary>
    /// Defines a contract for user management, including authentication and permissions.
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Returns current user id
        /// </summary>
        /// <returns></returns>
        string GetUserId();

        /// <summary>
        /// Creates an entity
        /// </summary>
        /// <param name="type"></param>
        /// <param name="entityId"></param>
        /// <param name="entityDescription"></param>
        /// <returns></returns>
        UserOperationResult CreateEntity(EntityType type, string entityId, string? entityDescription);

        /// <summary>
        /// Changes a given entity property
        /// </summary>
        /// <param name="type"></param>
        /// <param name="entityId"></param>
        /// <param name="property"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        UserOperationResult ChangeEntityProperty(EntityType type, string entityId, EntityProperty property,
            object? propertyValue);

        /// <summary>
        /// Deletes an entity
        /// </summary>
        /// <param name="type"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        UserOperationResult DeleteEntity(EntityType type, string entityId);

        /// <summary>
        /// Associates an entity to another entity
        /// </summary>
        /// <param name="entityIdA"></param>
        /// <param name="entityIdB"></param>
        /// <returns></returns>
        UserOperationResult AssociateEntityToEntity(string entityIdA, string entityIdB);

        /// <summary>
        /// Disassociates two entities from each other
        /// </summary>
        /// <param name="entityIdA"></param>
        /// <param name="entityIdB"></param>
        /// <returns></returns>
        UserOperationResult DissociateEntityFromEntity(string entityIdA, string entityIdB);

        /// <summary>
        /// Generates an entity report
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        object GenerateEntityReport(string entityId);

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserOperationResult DeleteUser(string userId);

        /// <summary>
        /// Returns  a summary a user association
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        object GetUserAssociations(string userId);

        /// <summary>
        /// Associates a user to an entity
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        UserOperationResult AssociateUserWithEntity(string userId, string entityId);

        /// <summary>
        /// Disassociates a user from an entity
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        UserOperationResult DisassociateUserWithEntity(string userId, string entityId);

        /// <summary>
        /// Checks if user contains a given permission
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        UserOperationResult CheckUserPermission(Permission permission);

        /// <summary>
        ///  Adds a permission to a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        UserOperationResult AddUserPermission(string userId, Permission permission);

        /// <summary>
        /// Removes a permission from a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        UserOperationResult RemoveUserPermission(string userId, Permission permission);

        /// <summary>
        ///  Changes a user id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newUserId"></param>
        /// <returns></returns>
        UserOperationResult ChangeUserId(string userId, string newUserId);

        /// <summary>
        /// Generates a user report
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        object GenerateUserReport(string userId);
    }
}