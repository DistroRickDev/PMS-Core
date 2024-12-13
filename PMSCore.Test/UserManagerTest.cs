using Microsoft.Extensions.Logging;
using PMSCore.Test;
using PMSCore;
using Xunit;

namespace PMSCore.Tests
{
    public class UserManagerTests
    {

        [Fact]
        public void UserManagerTest_ChangeUserPermission_ShouldFailWithInvalidPermission()
        {
            // Arrange
            UserManager.ResetInstance();
            var userManager = UserManager.GetInstance();
            var currentUser = new UserFake("testUser", Permission.DEFAULT);
            var targetUser = new UserFake("testUser2", Permission.DEFAULT);


            // Act
            var result = userManager.ChangeUserPermission(currentUser, targetUser, Permission.ADMIN);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void UserManagerTest_ChangeUserPermission_ShouldSucceed()
        {
            // Arrange
            // Arrange
            UserManager.ResetInstance();
            var userManager = UserManager.GetInstance();
            var adminUser = new UserFake("testUserAdmin", Permission.ADMIN);
            var targetUser = new UserFake("testUser", Permission.DEFAULT);

            // Add admin user as the current user (simulate logged-in admin).
            userManager.AddUser(adminUser);

            // Act
            var result = userManager.ChangeUserPermission(adminUser, targetUser, Permission.ADMIN);

            // Assert
            Assert.True(result);
            Assert.Equal(Permission.ADMIN, targetUser.GetPermission());
        }

        [Fact]
        public void UserManagerTest_AddUser_ShouldSucceed()
        {
            // Arrange
            UserManager.ResetInstance();
            var userManager = UserManager.GetInstance();
            var user = new UserFake("testUser", Permission.DEFAULT);

            // Act
            var result = userManager.AddUser(user);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void UserManagerTest_GetUserID_ShouldReturnCorrectID()
        {
            // Arrange
            UserManager.ResetInstance();
            var userManager = UserManager.GetInstance();
            var user = new UserFake("testUser", Permission.DEFAULT);

            // Act
            var userID = userManager.GetUserID(user);

            // Assert
            Assert.Equal("testUser", userID);
        }

        [Fact]
        public void UserManagerTest_GetUserPermissionLevel_ShouldReturnCorrectPermission()
        {
            // Arrange
            UserManager.ResetInstance();
            var userManager = UserManager.GetInstance();
            var user = new UserFake("testUser", Permission.ADMIN);

            // Act
            var permission = userManager.GetUserPermissionLevel(user);

            // Assert
            Assert.Equal(Permission.ADMIN, permission);
        }
    }
}
