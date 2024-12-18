namespace PMSCore.Test
{
    /// <summary>
    /// Unit tests for the Admin class.
    /// </summary>
    public class AdminTests
    {
        private readonly Mock<IStateManager> _stateManagerMock;

        public AdminTests()
        {
            _stateManagerMock = new Mock<IStateManager>();
            StateManager.SetInstance(_stateManagerMock.Object);
        }

        /// <summary>
        /// Verifies that a user is created and added to the StateManager.
        /// </summary>
        [Fact]
        public void CreateUser_ShouldAddUserToStateManager_WhenValidInputIsGiven()
        {
            // Arrange
            var admin = new Admin("adminUser");
            var userName = "testerUser";
            var permissions = new HashSet<Permission> { Permission.TESTER };

            // Act
            var user = admin.CreateUser(userName, permissions);

            // Assert
            Assert.NotNull(user);
            if (user != null)
            {
                Assert.Equal(userName, user.UserName);
                Assert.Equal(permissions, user.UserPermissions);
            }
            _stateManagerMock.Verify(sm => sm.AddUser(It.IsAny<UserBase>()), Times.Once);
        }

        /// <summary>
        /// Ensures null is returned and no user is added if permissions are invalid.
        /// </summary>
        [Fact]
        public void CreateUser_ShouldReturnNull_WhenInvalidPermissionsAreProvided()
        {
            // Arrange
            var admin = new Admin("adminUser");
            var userName = "invalidUser";
            var permissions = new HashSet<Permission> { Permission.ADMIN };

            // Act
            var user = admin.CreateUser(userName, permissions);

            // Assert
            Assert.Null(user);
            _stateManagerMock.Verify(sm => sm.AddUser(It.IsAny<UserBase>()), Times.Never);
        }

        /// <summary>
        /// Verifies that a user is removed from the StateManager when deleted.
        /// </summary>
        [Fact]
        public void DeleteUser_ShouldRemoveUserFromStateManager_WhenUserExists()
        {
            // Arrange
            var admin = new Admin("adminUser");
            var user = new Tester("testerUser");
            _stateManagerMock.Setup(sm => sm.RemoveUser(user)).Returns(true);

            // Act
            var result = admin.DeleteUser(user);

            // Assert
            Assert.True(result);
            _stateManagerMock.Verify(sm => sm.RemoveUser(user), Times.Once);
        }

        /// <summary>
        /// Ensures false is returned if the user does not exist in the StateManager.
        /// </summary>
        [Fact]
        public void DeleteUser_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var admin = new Admin("adminUser");
            var user = new Tester("nonExistingUser");
            _stateManagerMock.Setup(sm => sm.RemoveUser(user)).Returns(false);

            // Act
            var result = admin.DeleteUser(user);

            // Assert
            Assert.False(result);
            _stateManagerMock.Verify(sm => sm.RemoveUser(user), Times.Once);
        }

        /// <summary>
        /// Verifies that all users are retrieved from the StateManager.
        /// </summary>
        [Fact]
        public void ListUsers_ShouldReturnUsersFromStateManager()
        {
            // Arrange
            var admin = new Admin("adminUser");
            var users = new List<UserBase>
            {
                new Tester("testerUser"),
                new Developer("devUser")
            };
            _stateManagerMock.Setup(sm => sm.GetUsers()).Returns(users);

            // Act
            var result = admin.ListUsers();

            // Assert
            Assert.Equal(users, result);
        }

        /// <summary>
        /// Verifies that a user's name and permissions are updated via StateManager.
        /// </summary>
        [Fact]
        public void UpdateUser_ShouldUpdateUserDetailsInStateManager_WhenValidInputIsGiven()
        {
            // Arrange
            var admin = new Admin("adminUser");
            var user = new Tester("testerUser");
            var newUserName = "updatedTester";
            var newPermissions = new HashSet<Permission> { Permission.DEVELOPER };
            _stateManagerMock.Setup(sm => sm.UserExists(user)).Returns(true);

            // Act
            var result = admin.UpdateUser(user, newUserName, newPermissions);

            // Assert
            Assert.True(result);
            Assert.Equal(newUserName, user.UserName);
            Assert.Equal(newPermissions, user.UserPermissions);
            _stateManagerMock.Verify(sm => sm.UpdateUser(user), Times.Once);
        }

        /// <summary>
        /// Ensures that updating fails if the user does not exist in the StateManager.
        /// </summary>
        [Fact]
        public void UpdateUser_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var admin = new Admin("adminUser");
            var user = new Tester("testerUser");
            _stateManagerMock.Setup(sm => sm.UserExists(user)).Returns(false);

            // Act
            var result = admin.UpdateUser(user, "newUserName", new HashSet<Permission> { Permission.TESTER });

            // Assert
            Assert.False(result);
            _stateManagerMock.Verify(sm => sm.UpdateUser(It.IsAny<UserBase>()), Times.Never);
        }
    }
}