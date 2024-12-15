namespace PMSCore;

/// <summary>
/// Abstract class the represents data (Project | Task)
/// </summary>
[Serializable]
public abstract class Entity
{
    protected Entity(string id, string? description = null)
    {
        Id = id;
        Description = description;
        Report = string.Empty;
        Status = EntityStatus.New;
        Priority = EntityPriority.Medium;
        CreatedDate = DateTime.UtcNow;
        StartedDate = null;
        FinishedDate = null;
        AppendMessageToReport($"Entity {Id} created");
    }
    
    /// <summary>
    /// Appends a given entity action to the entity report
    /// </summary>
    /// <param name="message"></param>
    protected void AppendMessageToReport(string message)
    {
        Report += $"[EntityLog:{Id}] {DateTime.Now} : {message}\n";
    }
    
    /// <summary>
    /// Returns the entity id
    /// </summary>
    /// <returns></returns>
    public string GetId() => Id;
    
    /// <summary>
    /// Returns entity description
    /// </summary>
    /// <returns></returns>
    public string GetDescription() => Description ?? string.Empty;

    /// <summary>
    /// Adds a new description to the entity
    /// </summary>
    /// <param name="description"></param>
    public void SetDescription(string description)
    {
        AppendMessageToReport($"Changed description from '{Description}' to '{description}'");
        Description = description;
    }
    
    /// <summary>
    /// Returns entity status
    /// </summary>
    /// <returns></returns>
    public EntityStatus GetStatus() => Status;

    /// <summary>
    /// Set's an entity new status
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public void SetStatus(EntityStatus status)
    {
        AppendMessageToReport($"Changed status from {Status} to {status}");
        Status = status;
    }
    
    /// <summary>
    /// Returns entity priority
    /// </summary>
    /// <returns></returns>
    public EntityPriority GetPriority() => Priority;
    
    /// <summary>
    /// Set's an entity new status
    /// </summary>
    /// <param name="priority"></param>
    /// <returns></returns>
    public void SetPriority(EntityPriority priority)
    {
        AppendMessageToReport($"Changed priority from {Priority} to {priority}");
        Priority = priority;
    }
    
    /// <summary>
    /// Returns an entity created date
    /// </summary>
    /// <returns></returns>
    public DateTime GetCreatedDate() => CreatedDate;

    /// <summary>
    /// Returns an entity started date
    /// </summary>
    /// <returns></returns>
    public DateTime? GetStartedDate() => StartedDate;
    
    /// <summary>
    /// Sets entity start date
    /// </summary>
    /// <param name="startedDate"></param>
    public void SetStartedDate(DateTime startedDate)
    {
        AppendMessageToReport($"Changed start date from {StartedDate.ToString() ?? "N/A"} to {startedDate}");
        StartedDate = startedDate;
    }
    
    /// <summary>
    /// Returns an entity Finished date
    /// </summary>
    /// <returns></returns>
    public DateTime? GetFinishedDate() => FinishedDate;
    
    /// <summary>
    /// Sets entity finished date
    /// </summary>
    /// <param name="finishedDate"></param>
    public void SetFinishedDate(DateTime finishedDate)
    {
        AppendMessageToReport($"Changed finished date from {FinishedDate.ToString() ?? "N/A"} to {finishedDate}");
        FinishedDate = finishedDate;
    }
    
    /// <summary>
    /// Helper method that displays returns entity details
    /// </summary>
    public string DisplayEntityDetails()
    {
        return $"{GetType()} Id: {Id}\nDescription: {Description}\nPriority: {Priority}\nStatus: {Status}";
    }
    
    /// <summary>
    /// Returns the entities report
    /// </summary>
    /// <returns></returns>
    public string GetReport() => Report;

    private string Id{get;}
    private string? Description{get;set;}
    private EntityStatus Status{get;set;}
    private EntityPriority Priority { get; set; }
    private DateTime CreatedDate{get;}
    private DateTime? StartedDate{get;set;}
    private DateTime? FinishedDate{get;set;}
    private string Report{get;set;}
}