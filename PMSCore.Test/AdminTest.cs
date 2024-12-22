using Xunit;

namespace PMSCore.Test;

/// <summary>
/// Unit tests for the Admin class.
/// </summary>
public class AdminTest
{
    [Fact]
    public void Admin_Constructor_ValidPermissions()
    {
        // Arrange & Act
        var admin = new Admin("AdminUser");

        // Assert
        Assert.Contains(Permission.CanCreateProject, admin.UserPermissions);
        Assert.Contains(Permission.CanDeleteUser, admin.UserPermissions);
        Assert.Contains(Permission.CanChangeUser, admin.UserPermissions);
    }

    [Fact]
    public void Admin_Constructor_ValidUserId()
    {
        // Arrange & Act
        var admin = new Admin("AdminUser");

        // Assert
        Assert.Equal("AdminUser", admin.UserId);
    }
}
