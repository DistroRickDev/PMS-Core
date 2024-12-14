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

            userManager.Register(currentUser);
            userManager.Login(currentUser);

            userManager.Register(targetUser);

            // Act
            var result = userManager.ChangeUserPermission(targetUser.GetUserID(), Permission.ADMIN);

            // Assert
            Assert.True(result == AssociationStatus.InvalidPermission);
        }

        [Fact]
        public void UserManagerTest_ChangeUserPermission_ShouldFailWithInvalidUser()
        {
            // Arrange
            UserManager.ResetInstance();
            var userManager = UserManager.GetInstance();
            var currentUser = new UserFake("testUser", Permission.ADMIN);

            userManager.Register(currentUser);
            userManager.Login(currentUser);

            // Act
            var result = userManager.ChangeUserPermission(null, Permission.DEFAULT);

            //Assert
            Assert.True(result == AssociationStatus.InvalidUser);
        }

        [Fact]
        public void UserManagerTest_ChangeUserPermission_ShouldSucceed()
        {
            // Arrange
            UserManager.ResetInstance();
            var userManager = UserManager.GetInstance();
            var adminUser = new UserFake("testUserAdmin", Permission.ADMIN);
            var targetUser = new UserFake("testUser", Permission.DEFAULT);

            // Add admin user as the current user (simulate logged-in admin).
            userManager.Register(adminUser);
            userManager.Register(targetUser);
            userManager.Login(adminUser);

            // Act
            var result = userManager.ChangeUserPermission(targetUser.GetUserID(), Permission.ADMIN);

            // Assert
            Assert.True(result == AssociationStatus.NoError);
            Assert.Equal(Permission.ADMIN, targetUser.GetPermission());
        }

        [Fact]
        public void UserManagerTest_Login_ShouldSucceedIfRegistered()
        {
            // Arrange
            UserManager.ResetInstance();
            var userManager = UserManager.GetInstance();
            var user = new UserFake("testUser", Permission.DEFAULT);
            userManager.Register(user);

            // Act
            var result = userManager.Login(user);

            // Assert
            Assert.True(result == UserStatus.OK);
        }


        [Fact]
        public void UserManagerTest_Register_ShouldSucceed()
        {
            // Arrange
            UserManager.ResetInstance();
            var userManager = UserManager.GetInstance();
            var user = new UserFake("testUser", Permission.DEFAULT);

            // Act
            var result = userManager.Register(user);

            // Assert
            Assert.True(result == UserStatus.OK);
        }

        [Fact]
        public void UserManagerTest_Register_ShouldFailIfUserExists()
        {
            // Arrange
            UserManager.ResetInstance();
            var userManager = UserManager.GetInstance();
            var user = new UserFake("testUser", Permission.DEFAULT);
            userManager.Register(user);

            // Act
            var result = userManager.Register(user);

            // Assert
            Assert.True(result == UserStatus.Found);
        }

        [Fact]
        public void UserManagerTest_Login_ShouldFailIfNotRegistered()
        {
            // Arrange
            UserManager.ResetInstance();
            var userManager = UserManager.GetInstance();
            var user = new UserFake("testUser", Permission.DEFAULT);

            // Act
            var result = userManager.Login(user);

            // Assert
            Assert.True(result == UserStatus.NotFound);
        }

        [Fact]
        public void UserManagerTest_Login_ShouldFailIfAlreadyLoggedIn()
        {
            // Arrange
            UserManager.ResetInstance();
            var userManager = UserManager.GetInstance();
            var user1 = new UserFake("testUser1", Permission.DEFAULT);
            var user2 = new UserFake("testUser2", Permission.DEFAULT);
            userManager.Register(user1);
            userManager.Register(user2);
            userManager.Login(user1);

            // Act
            var result = userManager.Login(user2);

            // Assert
            Assert.True(result == UserStatus.Error);

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

        [Fact]
        public void UserManagerTest_DeleteUser_ShouldSucceedIfUserRegistered()
        {
            // Arrange
            UserManager.ResetInstance();
            var userManager = UserManager.GetInstance();
            var user = new UserFake("testUser", Permission.ADMIN);
            userManager.Register(user);

            // Act
            var result = userManager.DeleteUser(user);

            // Assert
            Assert.Equal(UserStatus.OK, result);
        }

        [Fact]
        public void UserManagerTest_DeleteUser_ShouldFailIfUserNotRegistered()
        {
            // Arrange
            UserManager.ResetInstance();
            var userManager = UserManager.GetInstance();
            var user1 = new UserFake("testUser1", Permission.ADMIN);
            var user2 = new UserFake("testUser2", Permission.DEFAULT);
            userManager.Register(user1);

            // Act
            var result = userManager.DeleteUser(user2);

            // Assert
            Assert.Equal(UserStatus.NotFound, result);
            
            // Act

        }
}
}
