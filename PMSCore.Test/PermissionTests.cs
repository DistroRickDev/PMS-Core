using System;
using Xunit;
using PMSCore;

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
        public void DefaultConstructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act: Create an instance using the default constructor.
            var permission = new Permission();

            // Assert: Verify the default level and description values.
            Assert.Equal(0, permission.PermissionLevel);
            Assert.Equal("No permissions assigned.", permission.PermissionDescription);
        }

        /// <summary>
        /// Tests that the parameterized constructor initializes the object with the provided values.
        /// </summary>
        /// <param name="level">The permission level to initialize.</param>
        /// <param name="description">The description to initialize.</param>
        [Theory]
        [InlineData(1, "Read-only access")]
        [InlineData(5, "Full admin access")]
        public void ParameterizedConstructor_ShouldInitializeWithProvidedValues(int level, string description)
        {
            // Arrange & Act: Create an instance using the parameterized constructor.
            var permission = new Permission(level, description);

            // Assert: Verify that the level and description match the provided values.
            Assert.Equal(level, permission.PermissionLevel);
            Assert.Equal(description, permission.PermissionDescription);
        }

        /// <summary>
        /// Tests that the parameterized constructor throws an exception for a negative permission level.
        /// </summary>
        [Fact]
        public void ParameterizedConstructor_ShouldThrowArgumentExceptionForNegativeLevel()
        {
            // Arrange: Define invalid input values.
            int invalidLevel = -1;
            string description = "Invalid permission";

            // Act & Assert: Verify that an ArgumentException is thrown with the correct message.
            var exception = Assert.Throws<ArgumentException>(() => new Permission(invalidLevel, description));
            Assert.Equal("Permission level must be a non-negative integer. (Parameter 'level')", exception.Message);
        }

        /// <summary>
        /// Tests that the ToString method returns the expected string representation.
        /// </summary>
        /// <param name="level">The permission level to test.</param>
        /// <param name="description">The permission description to test.</param>
        /// <param name="expected">The expected string representation.</param>
        [Theory]
        [InlineData(1, "Read-only access", "Level 1: Read-only access")]
        [InlineData(10, "Superuser access", "Level 10: Superuser access")]
        public void ToString_ShouldReturnCorrectString(int level, string description, string expected)
        {
            // Arrange: Create an instance using the parameterized constructor.
            var permission = new Permission(level, description);

            // Act: Call the ToString method.
            var result = permission.ToString();

            // Assert: Verify that the string representation matches the expected output.
            Assert.Equal(expected, result);
        }
    }
}
