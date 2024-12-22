using Xunit;
using PMSCore;

namespace PMSCore.Tests;

/// <summary>
/// Tests for Tester class.
/// </summary>
public class TesterTest
{
    [Fact]
    public void Tester_Initialization_HasCorrectPermissions()
    {
        // Arrange
        var tester = new Tester("Tester123");

        // Assert
        Assert.Contains(Permission.CanChangeTaskProperty, tester.UserPermissions);
        Assert.Contains(Permission.CanGenerateEntityReport, tester.UserPermissions);
        Assert.DoesNotContain(Permission.CanCreateProject, tester.UserPermissions);
        Assert.DoesNotContain(Permission.CanDeleteTask, tester.UserPermissions);
    }
}