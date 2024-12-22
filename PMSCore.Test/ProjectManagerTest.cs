using Xunit;
using PMSCore;

namespace PMSCore.Tests;

/// <summary>
/// Tests for ProjectManager class.
/// </summary>
public class ProjectManagerTest
{
    [Fact]
    public void ProjectManager_Initialization_HasCorrectPermissions()
    {
        // Arrange
        var projectManager = new ProjectManager("PM123");

        // Assert
        Assert.Contains(Permission.CanCreateProject, projectManager.UserPermissions);
        Assert.Contains(Permission.CanChangeProjectProperty, projectManager.UserPermissions);
        Assert.Contains(Permission.CanDeleteProject, projectManager.UserPermissions);
        Assert.DoesNotContain(Permission.CanChangeUser, projectManager.UserPermissions);
        Assert.DoesNotContain(Permission.CanDeleteUser, projectManager.UserPermissions);
    }
}