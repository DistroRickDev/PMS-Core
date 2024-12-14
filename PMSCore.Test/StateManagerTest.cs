namespace PMSCore.Test;

public class StateManagerTest
{
    private static string TestEntityId { get; set; } = "TestInstance";
    private Entity TestEntity { get; set; } = new EntityFake(TestEntityId, "TestInstance Description");

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
        Assert.Equal(sm, StateManager.GetInstance());
        Assert.Equal(EntityState.Ok, sm.CreateEntity(TestEntity));
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
        Assert.Equal(EntityState.Ok,
            sm.UpdateEntityProperty(TestEntityId, EntityProperty.EntityPriority, EntityPriority.High));
        Assert.Equal((EntityState.Ok, EntityPriority.High),
            sm.GetEntityProperty(TestEntityId, EntityProperty.EntityPriority));
        Assert.Equal(EntityState.Ok, sm.UpdateEntityProperty(TestEntityId, EntityProperty.StartedDate, DateTime.Today));
        Assert.Equal((EntityState.Ok, DateTime.Today), sm.GetEntityProperty(TestEntityId, EntityProperty.StartedDate));
        Assert.Equal(EntityState.Ok,
            sm.UpdateEntityProperty(TestEntityId, EntityProperty.FinishedDate, DateTime.Today));
        Assert.Equal((EntityState.Ok, DateTime.Today), StateManager.GetInstance().GetEntityProperty(TestEntityId, EntityProperty.FinishedDate));
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
        sm.CreateEntity(TestEntity);
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
        sm.CreateEntity(TestEntity);
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
        sm.CreateEntity(TestEntity);
        Assert.Equal(EntityState.Forbidden,
            sm.UpdateEntityProperty(TestEntityId, EntityProperty.Description, 2));
        Assert.Equal(EntityState.Forbidden, sm.UpdateEntityProperty(TestEntityId, EntityProperty.EntityStatus, ""));
        Assert.Equal(EntityState.Forbidden,
            sm.UpdateEntityProperty(TestEntityId, EntityProperty.EntityPriority, ""));
        Assert.Equal(EntityState.Forbidden, sm.UpdateEntityProperty(TestEntityId, EntityProperty.StartedDate, false));
        Assert.Equal(EntityState.Forbidden,
            sm.UpdateEntityProperty(TestEntityId, EntityProperty.FinishedDate, false));
    }
    
    /// <summary>
    /// Attempts to create same entity twice
    /// </summary>
    [Fact]
    public void StateManagerTest_CreateEntityErrorAlreadyExists()
    {
        SetUp();
        var sm = StateManager.GetInstance();
        Assert.Equal(EntityState.Ok, sm.CreateEntity(TestEntity));
        Assert.Equal(EntityState.AlreadyExists, sm.CreateEntity(TestEntity));
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
        Entity? project = Project.CreateProject(new LoggerFake(), TestEntityId);
        Entity? task = Task.CreateTask(new LoggerFake(), "TestInstance2");
        Assert.NotNull(project);
        Assert.NotNull(task);
        Assert.Equal(EntityState.Ok, sm.CreateEntity(project!));
        Assert.Equal(EntityState.Ok, sm.CreateEntity(task!));
        Assert.Equal(AssociationStatus.NoError ,sm.AssociateEntityToEntity(TestEntityId, "TestInstance2"));
        Assert.Equal(AssociationStatus.NoError, sm.DisassociateEntityToEntity(TestEntityId, "TestInstance2"));
        Assert.Equal(AssociationStatus.NoError ,sm.AssociateEntityToEntity("TestInstance2", TestEntityId));
        Assert.Equal(AssociationStatus.NoError, sm.DisassociateEntityToEntity("TestInstance2", TestEntityId));
    }
    
    /// <summary>
    /// Associate Entity to Entity but entity not found
    /// </summary>
    [Fact]
    public void StateManagerTest_EntityToEntityNotFound()
    {
        SetUp();
        var sm = StateManager.GetInstance();
        Assert.Equal(AssociationStatus.EntityNotFound ,sm.AssociateEntityToEntity(TestEntityId, "TestInstance2"));
        Assert.Equal(AssociationStatus.EntityNotFound ,sm.DisassociateEntityToEntity(TestEntityId, "TestInstance2"));
    }
    
    /// <summary>
    /// Associate Entity to Entity but it's an invalid association
    /// </summary>
    [Fact]
    public void StateManagerTest_InvalidAssociation()
    {
        SetUp();
        var sm = StateManager.GetInstance();
        Assert.Equal(EntityState.Ok, sm.CreateEntity(TestEntity));
        Assert.Equal(AssociationStatus.InvalidAssociation ,sm.AssociateEntityToEntity(TestEntityId, TestEntityId));
        Assert.Equal(AssociationStatus.InvalidAssociation ,sm.DisassociateEntityToEntity(TestEntityId, TestEntityId));
    }
    
    /// <summary>
    /// Associate Entity to Entity twice
    /// </summary>
    [Fact]
    public void StateManagerTest_EntityToEntityDuplicateAssociation()
    {
        SetUp();
        var sm = StateManager.GetInstance();
        Entity? project = Project.CreateProject(new LoggerFake(), TestEntityId);
        Entity? task = Task.CreateTask(new LoggerFake(), "TestInstance2");
        Assert.NotNull(project);
        Assert.NotNull(task);
        Assert.Equal(EntityState.Ok, sm.CreateEntity(project!));
        Assert.Equal(EntityState.Ok, sm.CreateEntity(task!));
        Assert.Equal(AssociationStatus.NoError ,sm.AssociateEntityToEntity(TestEntityId, "TestInstance2"));
        Assert.Equal(AssociationStatus.DuplicatedAssociation ,sm.AssociateEntityToEntity("TestInstance2", TestEntityId));
    }
    
    /// <summary>
    /// Associate Entity to Entity twice
    /// </summary>
    [Fact]
    public void StateManagerTest_EntityToEntityNoAssociation()
    {
        SetUp();
        Entity? project = Project.CreateProject(new LoggerFake(), TestEntityId);
        Entity? task = Task.CreateTask(new LoggerFake(), "TestInstance2");
        Assert.NotNull(project);
        Assert.NotNull(task);
        Assert.Equal(EntityState.Ok, StateManager.GetInstance().CreateEntity(project!));
        Assert.Equal(EntityState.Ok, StateManager.GetInstance().CreateEntity(task!));
        Assert.Equal(AssociationStatus.NoAssociation ,StateManager.GetInstance().DisassociateEntityToEntity("TestInstance2", TestEntityId));
    }
    
}