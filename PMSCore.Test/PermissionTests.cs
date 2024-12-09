using System;
using Xunit;
using PMSCore;
using static PMSCore.Permission;

namespace PMSCore.Test
{
    /// <summary>
    /// Unit tests for the <see cref="Permission"/> class.
    /// </summary>
    public class PermissionTests
    {
        /// <summary>
        /// Tests that the default constructor initializes the object with default values.
        /// </summary>
        [Fact]
        public void DefaultConstructorShouldInitializeWithDefaultValues()
        {
            // Arrange & Act: Create an instance using the default constructor.
            var permission = new Permission();

            // Assert: Verify the default level and description values.
            Assert.Equal(Permission.PermissionLevel.DEFAULT, permission.GetPermissionLevel());
            Assert.Equal("Default Permissions. No User Access.", permission.GetPermissionDescription());
        }

        /// <summary>
        /// Tests that the parameterized constructor initializes the object with the provided values.
        /// </summary>
        /// <param name="level">The permission level to initialize.</param>
        /// <param name="expectedDescription">The description expected from the permission level.</param>
        [Theory]
        [InlineData(1, "Basic User Permissions Applied.")]
        [InlineData(2, "Employee Permissions Applied")]
        [InlineData(3, "Managerial Permissions Applied.")]
        [InlineData(4, "Administrative Permissions Applied.")]
        [InlineData(5, "Full Permissions Applied.")]
        public void ParameterizedConstructorShouldInitializeWithProvidedValues(int level, string expectedDescription)
        {
            // Arrange & Act: Create an instance using the parameterized constructor.
            var permission = new Permission(level);

            // Assert: Verify that the level and description match the provided values.
            Assert.Equal(Enum.ToObject(typeof(PermissionLevel), level), permission.GetPermissionLevel());
            Assert.Equal(expectedDescription, permission.GetPermissionDescription());
        }

        /// <summary>
        /// Tests that the ToString method returns the expected string representation.
        /// </summary>
        /// <param name="level">The permission level to test.</param>
        /// <param name="description">The permission description to test.</param>
        /// <param name="expected">The expected string representation.</param>
        [Theory]
        [InlineData(Permission.PermissionLevel.USER, "Level 1: Basic User Permissions Applied.")]
        [InlineData(Permission.PermissionLevel.SUPERUSER, "Level 5: Full Permissions Applied.")]
        public void ToStringShouldReturnCorrectString(Permission.PermissionLevel level, string expected)
        {        
            // Arrange: Create an instance using the parameterized constructor.
            var permission = new Permission((int)level);

            // Act: Call the ToString method.
            var result = permission.ToString();

            // Assert: Verify that the string representation matches the expected output.
            Assert.Equal(expected, result);
        }
    }
}
