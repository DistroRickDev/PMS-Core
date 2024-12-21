namespace PMSCore.Test;

public class EntityFake : Entity
{
    public EntityFake(string id, string? description = null) : base(EntityType.Project, id, description)
    {
    }
}