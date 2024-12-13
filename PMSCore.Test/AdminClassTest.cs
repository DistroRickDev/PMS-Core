namespace PMSCore.Test

{
    public class AdminTests
    {
        [Fact]
        public void CreateUser_ShouldCreateATester_WhenTesterPermissionIsApplied()
        {
            // Arrange - creating an Admin instance
            var admin = new Admin("adminUser");

            // Act - calling the CreateUser method
            var user = admin.CreateUser("testerUser", Permission.TESTER);

            // Assert
            Assert.NotNull(user); // Check for null before accessing properties
            if (user != null)
            {
                Assert.IsType<Tester>(user);
                Assert.Equal(Permission.TESTER, user.UserPermission);
                Assert.Equal("testerUser", user.UserName);
            }
        }

        [Fact]
        public void CreateUser_ShouldCreateADeveloper_WhenDeveloperPermissionIsApplied()
        {
            // Arrange
            var admin = new Admin("adminUser");

            // Act 
            var user = admin.CreateUser("devUser", Permission.DEVELOPER);

            // Assert
            Assert.NotNull(user); // Check for null before accessing properties
            if (user != null)
            {
                Assert.IsType<Developer>(user);
                Assert.Equal(Permission.DEVELOPER, user.UserPermission);
                Assert.Equal("devUser", user.UserName);
            }
        }

        [Fact]
        public void CreateUser_ShouldCreateAProjectOwner_WhenProjectOwnerPermissionIsApplied()
        {
            // Arrange
            var admin = new Admin("adminUser");

            // Act
            var user = admin.CreateUser("projOwnerUser", Permission.PROJECT_OWNER);

            // Assert
            Assert.NotNull(user); // Checking for null before assessing properties
            if (user != null)
            {
                Assert.IsType<ProjectOwner>(user);
                Assert.Equal(Permission.PROJECT_OWNER, user.UserPermission);
                Assert.Equal("projOwnerUser", user.UserName);
            }
        }

        [Fact]
        public void CreateUser_ShouldReturnNull_WhenInvalidPermissionIsPassed()
        {
            // Arrange
            var admin = new Admin("adminUser");

            // Act
            var user = admin.CreateUser("invalidUser", Permission.DEFAULT);

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public void DeleteUser_ShouldReturnTrue_WhenUserExists()
        {
            // Arrange
            var admin = new Admin("adminUser");
            var user = admin.CreateUser("testerUser", Permission.TESTER);

            // Act
            var result = admin.DeleteUser(user);

            // Assert
            Assert.True(result); //Checks for true if user was deleted sucessfully
        }

        [Fact]
        public void DeleteUser_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var admin = new Admin("adminUser");
            var user = new Tester("nonExistingUser");

            // Act
            var result = admin.DeleteUser(user);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ListUsers_ShouldReturnListOfManagedUsers()
        {
            // Arrange
            var admin = new Admin("adminUser");
            admin.CreateUser("testerUser", Permission.TESTER);
            admin.CreateUser("devUser", Permission.DEVELOPER);

            // Act
            var users = admin.ListUsers();

            // Assert
            Assert.Equal(2, users.Count);
            Assert.Contains(users, u => u.UserName == "testerUser");
            Assert.Contains(users, u => u.UserName == "devUser");
        }

        [Fact]
        public void UpdateUser_ShouldUpdateUsername_WhenInputIsValid()
        {
            // Arrange
            var admin = new Admin("adminUser");
            var user = admin.CreateUser("testerUser", Permission.TESTER);

            // Act
            var result = admin.UpdateUser(user, "updatedTester", Permission.TESTER);

            // Assert
            Assert.True(result);
            Assert.Equal("updatedTester", user.UserName);
        }

        [Fact]
        public void UpdateUser_ShouldUpdatePermission_WhenValidPermissionIsSet()
        {
            // Arrange
            var admin = new Admin("adminUser");
            var user = admin.CreateUser("testerUser", Permission.TESTER);

            // Act
            var result = admin.UpdateUser(user, "testerUser", Permission.DEVELOPER);

            // Assert
            Assert.True(result);
            Assert.Equal(Permission.DEVELOPER, user.UserPermission);
        }


        [Fact]
        public void UpdateUser_ShouldReturnFalse_WhenInvalidPermissionIsSet()
        {
            // Arrange
            var admin = new Admin("adminUser");
            var user = admin.CreateUser("testerUser", Permission.TESTER);

            // Act
            var result = admin.UpdateUser(user, "testerUser", Permission.DEFAULT);

            // Assert
            Assert.False(result);
        }
    }
}
