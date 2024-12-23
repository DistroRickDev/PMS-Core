namespace PMSCore.Test;

public class StateManagerTest
{
    private string TestEntityId { get; } = "TestInstance";

    public StateManagerTest()
    {
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PMSCore");
        var destinationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PMSCore-PROD");
        PreserveProductionPersistence(path, destinationPath);
    }

    ~StateManagerTest()
    {
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PMSCore-PROD");
        var destinationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PMSCore");
        PreserveProductionPersistence(path, destinationPath);
    }

    private static void PreserveProductionPersistence(string path, string destinationPath)
    {
        if (!Directory.Exists(path)) return;
        if (!Directory.Exists(destinationPath))
        {
            Directory.Move(path, destinationPath);
            return;
        }
        Directory.Delete(path, true);
    }

    /// <summary>
    /// Helper function to clean up singleton on start of every test
    /// </summary>
    private void SetUp()
    {
        StateManager.ResetInstance();
        Thread.Sleep(100);
    }
    
    /// <summary>
    /// Tests single entity manipulation happy path
    /// </summary>
    [Fact]
    public void StateManagerTest_SingleEntityHandlingHappyPath()
    {
        SetUp();
        var sm = StateManager.GetInstance();
        Assert.NotNull(sm);
        Assert.Equal(sm, StateManager.GetInstance());
        Assert.True(sm.FileRead);
        Assert.Equal(EntityState.Ok, sm.CreateEntity(EntityType.Task, TestEntityId));
        Assert.Equal(EntityState.Ok,
            sm.UpdateEntityProperty(TestEntityId, EntityProperty.Description, "TestInstance Description"));
        Assert.Equal((EntityState.Ok, "TestInstance Description"),
            sm.GetEntityProperty(TestEntityId, EntityProperty.Description));
        Assert.Equal(EntityState.Ok, (sm.GetEntityProperty(TestEntityId, EntityProperty.CreatedDate)).Item1);
        Assert.Equal(EntityState.Ok, (sm.GetEntityProperty(TestEntityId, EntityProperty.Report)).Item1);
        Assert.Equal(EntityState.Ok,
            sm.UpdateEntityProperty(TestEntityId, EntityProperty.Description, "TestInstance Description 2"));
        Assert.Equal((EntityState.Ok, "TestInstance Description 2"),
            sm.GetEntityProperty(TestEntityId, EntityProperty.Description));
        Assert.Contains($"Attempting to update entity id: {TestEntityId} with TestInstance Description 2",
            sm.GetReport());
        Assert.Equal(EntityState.Ok,
            sm.UpdateEntityProperty(TestEntityId, EntityProperty.EntityStatus, EntityStatus.InProgress));
        Assert.Equal((EntityState.Ok, EntityStatus.InProgress),
            sm.GetEntityProperty(TestEntityId, EntityProperty.EntityStatus));
        var result = sm.GetEntityProperty(TestEntityId, EntityProperty.StartedDate).Item2;
        Assert.NotNull(result);
        Assert.True((result as DateTime?)!.Value.Date == DateTime.Today);
        Assert.Equal(EntityState.Ok,
            sm.UpdateEntityProperty(TestEntityId, EntityProperty.EntityStatus, EntityStatus.Done));
        result = sm.GetEntityProperty(TestEntityId, EntityProperty.FinishedDate).Item2;
        Assert.NotNull(result);
        Assert.True((result as DateTime?)!.Value.Date == DateTime.Today);
        Assert.Equal(EntityState.AlreadyExists,
            sm.UpdateEntityProperty(TestEntityId, EntityProperty.EntityStatus, EntityStatus.Done));
        Assert.Equal(EntityState.Ok,
            sm.UpdateEntityProperty(TestEntityId, EntityProperty.EntityPriority, EntityPriority.High));
        Assert.Equal((EntityState.Ok, EntityPriority.High),
            sm.GetEntityProperty(TestEntityId, EntityProperty.EntityPriority));
        Assert.Equal(EntityState.Forbidden,
            sm.UpdateEntityProperty(TestEntityId, EntityProperty.CreatedDate, DateTime.Today));
        Assert.Equal(EntityState.Forbidden,
            sm.UpdateEntityProperty(TestEntityId, EntityProperty.StartedDate, DateTime.Today));
        Assert.Equal(EntityState.Forbidden,
            sm.UpdateEntityProperty(TestEntityId, EntityProperty.FinishedDate, DateTime.Today));
        Assert.Equal(EntityState.Forbidden,
            sm.UpdateEntityProperty(TestEntityId, EntityProperty.Report, ""));
        Assert.Equal(EntityState.Ok, sm.RemoveEntity(TestEntityId));
    }

    /// <summary>
    /// Attempts to GetProperty when Entity does not exist
    /// </summary>
    [Fact]
    public void StateManagerTest_SingleEntityNotFoundErrorGetEntityProperty()
    {
        SetUp();
        var sm = StateManager.GetInstance();
        Assert.Equal(EntityState.NotFound, (sm.GetEntityProperty(TestEntityId, EntityProperty.Description)).Item1);
    }
    
    /// <summary>
    /// Attempts to UpdateEntityProperty with null value
    /// </summary>
    [Fact]
    public void StateManagerTest_UpdatingNullProperty()
    {
        SetUp();
        var sm = StateManager.GetInstance();
        sm.CreateEntity(EntityType.Task, TestEntityId);
        Assert.Equal(EntityState.Forbidden, sm.UpdateEntityProperty(TestEntityId, EntityProperty.Description, null));
    }

    /// <summary>
    /// Attempts to UpdateEntityProperty with invalid entityId
    /// </summary>
    [Fact]
    public void StateManagerTest_UpdatingPropertyInvalidEntity()
    {
        SetUp();
        var sm = StateManager.GetInstance();
        Assert.Equal(EntityState.NotFound, sm.UpdateEntityProperty("", EntityProperty.Description, ""));
    }

    /// <summary>
    /// Attempts to UpdateEntityProperty non settable property
    /// </summary>
    [Fact]
    public void StateManagerTest_UpdatingPropertyNoSettableProperty()
    {
        SetUp();
        var sm = StateManager.GetInstance();
        sm.CreateEntity(EntityType.Task, TestEntityId);
        Assert.Equal(EntityState.Forbidden, sm.UpdateEntityProperty(TestEntityId, EntityProperty.Report, ""));
    }

    /// <summary>
    /// Attempts to UpdateEntityProperty with invalid property type
    /// </summary>
    [Fact]
    public void StateManagerTest_UpdatingPropertyInvalidPropertyType()
    {
        SetUp();
        var sm = StateManager.GetInstance();
        sm.CreateEntity(EntityType.Task, TestEntityId);
        Assert.Equal(EntityState.Forbidden,
            sm.UpdateEntityProperty(TestEntityId, EntityProperty.Description, 2));
        Assert.Equal(EntityState.Forbidden, sm.UpdateEntityProperty(TestEntityId, EntityProperty.EntityStatus, ""));
        Assert.Equal(EntityState.Forbidden,
            sm.UpdateEntityProperty(TestEntityId, EntityProperty.EntityPriority, ""));
    }

    /// <summary>
    /// Attempts to create same entity twice
    /// </summary>
    [Fact]
    public void StateManagerTest_CreateEntityErrorAlreadyExists()
    {
        SetUp();
        var sm = StateManager.GetInstance();
        Assert.Equal(EntityState.Ok, sm.CreateEntity(EntityType.Task, TestEntityId));
        Assert.Equal(EntityState.AlreadyExists, sm.CreateEntity(EntityType.Task, TestEntityId));
    }

    /// <summary>
    /// Attempts to create a null entity
    /// </summary>
    [Fact]
    public void StateManagerTest_AttemptsToCreateNullEntity()
    {
        SetUp();
        Assert.Equal(EntityState.Forbidden, StateManager.GetInstance().CreateEntity(EntityType.Task, string.Empty));
    }

    /// <summary>
    /// Attempts to remove nonexistent entity twice
    /// </summary>
    [Fact]
    public void StateManagerTest_RemoveEntityErrorNonExisting()
    {
        SetUp();
        var sm = StateManager.GetInstance();
        Assert.Equal(EntityState.NotFound, sm.RemoveEntity(TestEntityId));
    }

    /// <summary>
    /// Associate Entity to Entity happy path
    /// </summary>
    [Fact]
    public void StateManagerTest_EntityToEntityHappyPath()
    {
        SetUp();
        var sm = StateManager.GetInstance();
        var secondEntityId = "TestInstance2";
        Assert.Equal(EntityState.Ok, sm.CreateEntity(EntityType.Project, TestEntityId));
        Assert.Equal(EntityState.Ok, sm.CreateEntity(EntityType.Task, secondEntityId));
        Assert.Equal(AssociationStatus.NoError, sm.AssociateEntityToEntity(TestEntityId, secondEntityId));
        Assert.Equal(AssociationStatus.NoError, sm.DisassociateEntityFromEntity(TestEntityId, secondEntityId));
        Assert.Equal(AssociationStatus.NoError, sm.AssociateEntityToEntity(secondEntityId, TestEntityId));
        Assert.Equal(AssociationStatus.NoError, sm.DisassociateEntityFromEntity(secondEntityId, TestEntityId));
    }

    /// <summary>
    /// Associate Entity to Entity but entity not found
    /// </summary>
    [Fact]
    public void StateManagerTest_EntityToEntityNotFound()
    {
        SetUp();
        var sm = StateManager.GetInstance();
        Assert.Equal(AssociationStatus.EntityNotFound, sm.AssociateEntityToEntity(TestEntityId, "TestInstance2"));
        Assert.Equal(AssociationStatus.EntityNotFound, sm.DisassociateEntityFromEntity(TestEntityId, "TestInstance2"));
    }

    /// <summary>
    /// Associate Entity to Entity but it's an invalid association
    /// </summary>
    [Fact]
    public void StateManagerTest_InvalidAssociation()
    {
        SetUp();
        var sm = StateManager.GetInstance();
        Assert.Equal(EntityState.Ok, sm.CreateEntity(EntityType.Task, TestEntityId));
        Assert.Equal(AssociationStatus.InvalidAssociation, sm.AssociateEntityToEntity(TestEntityId, TestEntityId));
        Assert.Equal(AssociationStatus.InvalidAssociation, sm.DisassociateEntityFromEntity(TestEntityId, TestEntityId));
    }

    /// <summary>
    /// Associate Entity to Entity twice
    /// </summary>
    [Fact]
    public void StateManagerTest_EntityToEntityDuplicateAssociation()
    {
        SetUp();
        var sm = StateManager.GetInstance();
        Assert.Equal(EntityState.Ok, sm.CreateEntity(EntityType.Task, TestEntityId));
        Assert.Equal(EntityState.Ok, sm.CreateEntity(EntityType.Project, "TestInstance2"));
        Assert.Equal(AssociationStatus.NoError, sm.AssociateEntityToEntity(TestEntityId, "TestInstance2"));
        Assert.Equal(AssociationStatus.DuplicatedAssociation,
            sm.AssociateEntityToEntity("TestInstance2", TestEntityId));
    }

    /// <summary>
    /// Associate Entity to Entity twice
    /// </summary>
    [Fact]
    public void StateManagerTest_EntityToEntityNoAssociation()
    {
        SetUp();
        string projectId = "TestInstance2";
        Assert.Equal(EntityState.Ok, StateManager.GetInstance().CreateEntity(EntityType.Task, TestEntityId));
        Assert.Equal(EntityState.Ok, StateManager.GetInstance().CreateEntity(EntityType.Project, projectId));
        Assert.Equal(AssociationStatus.NoAssociation,
            StateManager.GetInstance().DisassociateEntityFromEntity(projectId, TestEntityId));
    }

    /// <summary>
    /// User registers, then logins, then manages entities
    /// </summary>
    /// <returns></returns>
    [Fact]
    public void StateManagerTest_UserHandling()
    {
        SetUp();
        const string testUserId = "TestUser";
        const string testProjectId = "TestProject";
        const string testTaskId = "TestTask";
        Assert.Equal(EntityState.NotFound, StateManager.GetInstance().UserLogin(testUserId));
        Assert.Equal(EntityState.Ok, StateManager.GetInstance().UserRegister(testUserId, new Admin(testUserId)));
        Assert.Equal(EntityState.AlreadyExists,
            StateManager.GetInstance().UserRegister(testUserId, new Admin(testUserId)));
        Assert.Equal(EntityState.Ok, StateManager.GetInstance().UserLogin(testUserId));
        Assert.NotNull(StateManager.GetInstance().GetCurrentUser());
        Assert.Equal(testUserId, StateManager.GetInstance().GetCurrentUser().GetUserId());
        Assert.Equal(EntityState.Ok, StateManager.GetInstance().UpdateUserProperty(testUserId, UserProperty.ChangeUserId, testUserId));
        Assert.Equal(EntityState.Forbidden, StateManager.GetInstance().UpdateUserProperty(testUserId, UserProperty.ChangeUserId, 2));
        Assert.Equal(EntityState.Forbidden, StateManager.GetInstance().UpdateUserProperty(testUserId, UserProperty.ChangeUserId, string.Empty));
        Assert.Equal(EntityState.NotFound, StateManager.GetInstance().UpdateUserProperty("NotAnId", UserProperty.ChangeUserId, "Won't work"));
        Assert.Equal(EntityState.Ok, StateManager.GetInstance().UpdateUserProperty(testUserId, UserProperty.RemovePermissions, Permission.CanDeleteProject));
        Assert.Equal(EntityState.Ok, StateManager.GetInstance().UpdateUserProperty(testUserId, UserProperty.AddPermissions, Permission.CanDeleteProject));
        Assert.Equal(EntityState.Forbidden, StateManager.GetInstance().UpdateUserProperty(testUserId, UserProperty.AddPermissions, 3.47));
        
        Assert.Equal(EntityState.Ok, StateManager.GetInstance().CreateEntity(EntityType.Project, testProjectId));
        Assert.Equal(AssociationStatus.NoError, StateManager.GetInstance().AssociateUserToEntity(testProjectId, testUserId));
        Assert.Equal(AssociationStatus.UserNotFound, StateManager.GetInstance().AssociateUserToEntity(testProjectId, "NotAnId"));
        Assert.Equal(AssociationStatus.EntityNotFound, StateManager.GetInstance().AssociateUserToEntity(testTaskId, testUserId));
        Assert.Equal(AssociationStatus.DuplicatedAssociation, StateManager.GetInstance().AssociateUserToEntity(testProjectId, testUserId));
        
        Assert.Equal(EntityState.Ok, StateManager.GetInstance().CreateEntity(EntityType.Task, testTaskId));
        Assert.Equal(AssociationStatus.NoError, StateManager.GetInstance().AssociateUserToEntity(testTaskId, testUserId));
        
        Assert.IsType<string>(StateManager.GetInstance().GetUserReport(testUserId));
        Assert.Equal(EntityState.NotFound, StateManager.GetInstance().GetUserReport("NotAnId"));
        
        Assert.IsType<string>(StateManager.GetInstance().GetUserAssociations(testUserId));
        Assert.Equal(EntityState.NotFound, StateManager.GetInstance().GetUserAssociations("NotAnId"));
        
        Assert.Equal(AssociationStatus.NoError, StateManager.GetInstance().DisassociateUserFromEntity(testProjectId, testUserId));
        Assert.Equal(AssociationStatus.UserNotFound, StateManager.GetInstance().DisassociateUserFromEntity(testProjectId, "NotAnId"));
        Assert.Equal(AssociationStatus.EntityNotFound, StateManager.GetInstance().DisassociateUserFromEntity("NotAnEntity", testUserId));
        Assert.Equal(AssociationStatus.NoError, StateManager.GetInstance().DisassociateUserFromEntity(testTaskId, testUserId));
        Assert.Equal(AssociationStatus.NoAssociation, StateManager.GetInstance().DisassociateUserFromEntity(testTaskId, testUserId));

        Assert.Equal(EntityState.NotFound, StateManager.GetInstance().GetUserAssociations(testUserId));
        
        Assert.Equal(EntityState.Ok, StateManager.GetInstance().UserLogout());   
        Assert.Equal(EntityState.Ok, StateManager.GetInstance().DeleteUser(testUserId));
        Assert.Equal(EntityState.NotFound, StateManager.GetInstance().UserLogout());
        Assert.Null(StateManager.GetInstance().GetCurrentUser());
        Assert.Equal(EntityState.NotFound, StateManager.GetInstance().DeleteUser(testUserId));
    }
}