namespace PMSCore.Test;

using Microsoft.Extensions.Logging;

public class EntityManagerTest1
{
    private ILogger _logger = new LoggerFake();
    private static readonly string TestEntityId = "TestEntityId";
    
    /// <summary>
    /// Helper function to clean up singletons on start of every test
    /// </summary>
    private void SetUp()
    {
        StateManager.ResetInstance();
        Thread.Sleep(100);
        EntityManager.ResetInstance();
        Thread.Sleep(100);
    }


    /// <summary>
    /// Tests entity handling happy path
    /// </summary>
    [Fact]
    public void EntityManagerTest_EntityHandlingHappyPath()
    {
        SetUp();
        Assert.Equal(EntityState.Ok, EntityManager.GetInstance().CreateEntity(TestEntityId, null, EntityType.Project));
        Assert.Equal(EntityState.Ok,
            EntityManager.GetInstance().CreateEntity("TestEntityIdTask", null, EntityType.Task));
        Assert.Equal(EntityState.Ok, EntityManager.GetInstance().GetEntityDescription(TestEntityId).Item1);
        Assert.Equal(EntityState.Ok, EntityManager.GetInstance().GetEntityCreationDate(TestEntityId).Item1);
        Assert.Equal(EntityState.Ok, EntityManager.GetInstance().GetEntityReport(TestEntityId).Item1);
        Assert.Equal(EntityState.Ok, EntityManager.GetInstance().GetEntityStartDate(TestEntityId).Item1);
        Assert.Equal(EntityState.Ok, EntityManager.GetInstance().GetEntityFinishDate(TestEntityId).Item1);
        Assert.Equal(EntityState.Ok, EntityManager.GetInstance().GetEntityReport(TestEntityId).Item1);
        Assert.Equal(EntityState.Ok, EntityManager.GetInstance().GetEntityStatus(TestEntityId).Item1);
        Assert.Equal(EntityState.Ok, EntityManager.GetInstance().GetEntityPriority(TestEntityId).Item1);
        Assert.Equal(EntityState.Ok, EntityManager.GetInstance().RemoveEntity(TestEntityId));
    }

    /// <summary>
    /// Tests entity updating happy path
    /// </summary>
    [Fact]
    public void EntityManagerTest_UpdateEntityHandlingHappyPath()
    {
        SetUp();
        Assert.Equal(EntityState.Ok, EntityManager.GetInstance().CreateEntity(TestEntityId, null, EntityType.Project));
        Assert.Equal(EntityState.Ok, EntityManager.GetInstance().UpdateEntityStatus(TestEntityId, EntityStatus.Closed));
        Assert.Equal(EntityState.Ok,
            EntityManager.GetInstance().UpdateEntityPriority(TestEntityId, EntityPriority.Low));
        Assert.Equal(EntityState.Ok, EntityManager.GetInstance().UpdateEntityStartDate(TestEntityId, DateTime.Today));
        Assert.Equal(EntityState.Ok,
            EntityManager.GetInstance().UpdateEntityFinishDate(TestEntityId, DateTime.Today.AddDays(2)));
        Assert.Equal(EntityState.Ok,
            EntityManager.GetInstance().UpdateEntityDescription(TestEntityId, "TestEntityId Description"));
        Assert.Equal(EntityState.Ok, EntityManager.GetInstance().RemoveEntity(TestEntityId));
    }
    
}