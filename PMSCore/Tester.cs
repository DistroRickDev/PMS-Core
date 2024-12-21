using System.Text.Json.Serialization;

namespace PMSCore;

/// <summary>
/// Represents a Tester user, with permissions specific to tasks.
/// </summary>
[JsonConverter(typeof(UserJsonConverter))]
public class Tester : User
{
    /// <summary>
    /// Initializes a Tester with permissions to validate and report on tasks.
    /// </summary>
    public Tester(string userId)
        : base(UserType.Tester, userId, new HashSet<Permission>
        {
            Permission.CanChangeTaskProperty,
            Permission.CanGenerateEntityReport
        })
    {
    }
}