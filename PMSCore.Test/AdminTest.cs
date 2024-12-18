namespace PMSCore.Test
{
    /// <summary>
    /// Unit tests for the Admin class.
    /// </summary>
    public class AdminTests
    {
        /// <summary>
        /// Verifies that a Tester user is created when the TESTER permission is used.
        /// </summary>
        [Fact]
        public void CreateUser_ShouldCreateATester_WhenTesterPermissionIsApplied()
        {
            // Arrange - create an Admin instance
            var admin = new Admin("adminUser");

            // Act - create a Tester user
            var user = admin.CreateUser("testerUser", Permission.TESTER);

            // Assert
            Assert.NotNull(user); // Check that the user was created
            if (user != null)
            {
                Assert.IsType<Tester>(user);
                Assert.Equal(Permission.TESTER, user.UserPermission);
                Assert.Equal("testerUser", user.UserName);
            }
        }

        /// <summary>
        /// Verifies that a Developer user is created when the DEVELOPER permission is used.
        /// </summary>
        [Fact]
        public void CreateUser_ShouldCreateADeveloper_WhenDeveloperPermissionIsApplied()
        {
            // Arrange
            var admin = new Admin("adminUser");

            // Act - create a Developer user
            var user = admin.CreateUser("devUser", Permission.DEVELOPER);

            // Assert
            Assert.NotNull(user);
            if (user != null)
            {
                Assert.IsType<Developer>(user);
                Assert.Equal(Permission.DEVELOPER, user.UserPermission);
                Assert.Equal("devUser", user.UserName);
            }
        }

        /// <summary>
        /// Verifies that a Project Owner user is created when the PROJECT_OWNER permission is used.
        /// </summary>
        [Fact]
        public void CreateUser_ShouldCreateAProjectOwner_WhenProjectOwnerPermissionIsApplied()
        {
            // Arrange
            var admin = new Admin("adminUser");

            // Act - create a Project Owner user
            var user = admin.CreateUser("projOwnerUser", Permission.PROJECT_OWNER);

            // Assert
            Assert.NotNull(user);
            if (user != null)
            {
                Assert.IsType<ProjectOwner>(user);
                Assert.Equal(Permission.PROJECT_OWNER, user.UserPermission);
                Assert.Equal("projOwnerUser", user.UserName);
            }
        }

        /// <summary>
        /// Ensures null is returned if an invalid permission is passed.
        /// </summary>
        [Fact]
        public void CreateUser_ShouldReturnNull_WhenInvalidPermissionIsPassed()
        {
            // Arrange
            var admin = new Admin("adminUser");

            // Act - try creating a user with an invalid permission
            var user = admin.CreateUser("invalidUser", Permission.DEFAULT);

            // Assert
            Assert.Null(user);
        }

        /// <summary>
        /// Verifies that a user is successfully deleted if they exist.
        /// </summary>
        [Fact]
        public void DeleteUser_ShouldReturnTrue_WhenUserExists()
        {
            // Arrange
            var admin = new Admin("adminUser");
            var user = admin.CreateUser("testerUser", Permission.TESTER);

            // Act - delete the created user
            var result = admin.DeleteUser(user);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Ensures that deleting a non-existing user returns false.
        /// </summary>
        [Fact]
        public void DeleteUser_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var admin = new Admin("adminUser");
            var user = new Tester("nonExistingUser");

            // Act - attempt to delete a user that does not exist
            var result = admin.DeleteUser(user);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Verifies that all managed users are listed correctly.
        /// </summary>
        [Fact]
        public void ListUsers_ShouldReturnListOfManagedUsers()
        {
            // Arrange
            var admin = new Admin("adminUser");
            admin.CreateUser("testerUser", Permission.TESTER);
            admin.CreateUser("devUser", Permission.DEVELOPER);

            // Act - list all users managed by the admin
            var users = admin.ListUsers();

            // Assert
            Assert.Equal(2, users.Count);
            Assert.Contains(users, u => u.UserName == "testerUser");
            Assert.Contains(users, u => u.UserName == "devUser");
        }

        /// <summary>
        /// Verifies that a user's name is updated correctly.
        /// </summary>
        [Fact]
        public void UpdateUser_ShouldUpdateUsername_WhenInputIsValid()
        {
            // Arrange
            var admin = new Admin("adminUser");
            var user = admin.CreateUser("testerUser", Permission.TESTER);

            // Act - update the user's name
            var result = admin.UpdateUser(user, "updatedTester", Permission.TESTER);

            // Assert
            Assert.True(result);
            Assert.Equal("updatedTester", user.UserName);
        }

        /// <summary>
        /// Ensures that a user's permission is updated when a valid permission is provided.
        /// </summary>
        [Fact]
        public void UpdateUser_ShouldUpdatePermission_WhenValidPermissionIsSet()
        {
            // Arrange
            var admin = new Admin("adminUser");
            var user = admin.CreateUser("testerUser", Permission.TESTER);

            // Act - update the user's permission
            var result = admin.UpdateUser(user, "testerUser", Permission.DEVELOPER);

            // Assert
            Assert.True(result);
            Assert.Equal(Permission.DEVELOPER, user.UserPermission);
        }

        /// <summary>
        /// Ensures that updating with an invalid permission fails.
        /// </summary>
        [Fact]
        public void UpdateUser_ShouldReturnFalse_WhenInvalidPermissionIsSet()
        {
            // Arrange
            var admin = new Admin("adminUser");
            var user = admin.CreateUser("testerUser", Permission.TESTER);

            // Act - try to update with an invalid permission
            var result = admin.UpdateUser(user, "testerUser", Permission.DEFAULT);

            // Assert
            Assert.False(result);
        }
    }
}