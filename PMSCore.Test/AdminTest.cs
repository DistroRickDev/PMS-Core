using System.Text.Json;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PMSCore.Test;

/// <summary>
/// Unit tests for the Admin class.
/// </summary>
public class AdminTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public AdminTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

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

    [Fact]
    public void AdminTest_JsonSerializerTest()
    {
        var admin = new Admin("AdminUser");
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var json = JsonSerializer.Serialize<User>(admin, options);
        _testOutputHelper.WriteLine(json);
        var deserialized = JsonSerializer.Deserialize<User>(json, options);
        Assert.NotNull(deserialized);
        Assert.Equal(admin.UserId, deserialized!.UserId);
        Assert.Equal(admin.UserPermissions, deserialized.UserPermissions);
    }
}