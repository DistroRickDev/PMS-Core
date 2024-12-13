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
            Assert.True(result == AssociationStatus.InvalidPermission);
        }

        [Fact]
        public void UserManagerTest_ChangeUserPermission_ShouldFailWithInvalidUser()
        {
            // Arrange
            UserManager.ResetInstance();
            var userManager = UserManager.GetInstance();
            var currentUser = new UserFake("testUser", Permission.ADMIN);

            // Act
            var result = userManager.ChangeUserPermission(currentUser, null, Permission.DEFAULT);

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
            userManager.AddUser(adminUser);

            // Act
            var result = userManager.ChangeUserPermission(adminUser, targetUser, Permission.ADMIN);

            // Assert
            Assert.True(result == AssociationStatus.NoError);
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
            Assert.True(result == AssociationStatus.NoError);
        }

        [Fact]
        public void UserManagerTest_AddUser_ShouldFailIfInvalid()
        {
            // Arrange
            UserManager.ResetInstance();
            var userManager = UserManager.GetInstance();
            var user = new UserFake("testUser", Permission.DEFAULT);
            userManager.AddUser(user);

            // Act
            var result = userManager.AddUser(user);

            // Assert
            Assert.True(result == AssociationStatus.InvalidUser);
        }

        [Fact]
        public void UserManagerTest_Login_ShouldSucceedIfRegistered()
        {
            // Arrange
            UserManager.ResetInstance();
            var userManager = UserManager.GetInstance();
            var user = new UserFake("testUser", Permission.DEFAULT);
            userManager.AddUser(user);

            // Act
            var result = userManager.Login(user);

            // Assert
            Assert.True(result == AssociationStatus.NoError);
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
            Assert.True(result == AssociationStatus.InvalidUser);
        }

        [Fact]
        public void UserManagerTest_Login_ShouldFailIfAlreadyLoggedIn()
        {
            // Arrange
            UserManager.ResetInstance();
            var userManager = UserManager.GetInstance();
            var user1 = new UserFake("testUser1", Permission.DEFAULT);
            userManager.Login(user1);
            var user2 = new UserFake("testUser2", Permission.DEFAULT);

            // Act
            var result = userManager.Login(user2);

            // Assert
            Assert.True(result == AssociationStatus.InvalidUser);

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
            Assert.True(result == AssociationStatus.NoError);
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
            Assert.True(result == AssociationStatus.InvalidUser);
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
