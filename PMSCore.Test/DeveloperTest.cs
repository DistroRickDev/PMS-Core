using Xunit;

namespace PMSCore.Test;

/// <summary>
/// Unit tests for the Developer class.
/// </summary>
public class DeveloperTest
{
    [Fact]
    public void Developer_Constructor_ValidPermissions()
    {
        // Arrange & Act
        var developer = new Developer("DevUser");

        // Assert
        Assert.Contains(Permission.CanCreateTask, developer.UserPermissions);
        Assert.Contains(Permission.CanChangeTaskProperty, developer.UserPermissions);
        Assert.DoesNotContain(Permission.CanDeleteProject, developer.UserPermissions);
    }

    [Fact]
    public void Developer_Constructor_ValidUserId()
    {
        // Arrange & Act
        var developer = new Developer("DevUser");

        // Assert
        Assert.Equal("DevUser", developer.UserId);
    }
}