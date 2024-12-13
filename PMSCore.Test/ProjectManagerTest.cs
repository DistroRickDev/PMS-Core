using Microsoft.Extensions.Logging;
using Xunit;
using PMSCore;

namespace PMSCore.Test
{
    /// <summary>
    /// Unit tests for the <see cref="ProjectManager"/> class.
    /// </summary>
    public class ProjectManagerTest
    {
        /// <summary>
        /// Test Project used for unit tests
        /// </summary>
        private static Project TestProject { get; set; } = new("TestProject", "TestProject", DateTime.Now);

        /// <summary>
        /// Test User mock used for unit tests
        /// </summary>
        private static IUser TestUser { get; set; } = new UserFake("TestUser");

        /// <summary>
        /// Tests Association of user to project happy path
        /// </summary>
        [Fact]
        public void ProjectManagerTest_AssociateProjectToUserHappyPath()
        {
            ProjectManager.ResetInstance();
            var pm = ProjectManager.GetInstance();
            var status = pm.AssociateProjectToUser(TestProject, TestUser);
            Assert.Equal(AssociationStatus.NoError, status);
        }

        /// <summary>
        /// Tests Association of user to project happy path validate logging message
        /// </summary>
        [Fact]
        public void ProjectManagerTest_AssociateProjectToUserLoggingHappyPath()
        {
            ProjectManager.ResetInstance();
            ILogger logger = new LoggerFake();
            var pm = ProjectManager.GetInstance(logger);
            var status = pm.AssociateProjectToUser(TestProject, TestUser);
            Assert.Equal(AssociationStatus.NoError, status);
            Assert.True(
                (logger as LoggerFake)?.LogStream.Contains(
                    $"Adding new user {TestUser}, appending project {TestProject}"));
        }

        /// <summary>
        /// Tests Association of user to two projects happy path
        /// </summary>
        [Fact]
        public void ProjectManagerTest_AssociateProjectToUserAddTwoProjectsHappyPath()
        {
            ProjectManager.ResetInstance();
            var pm = ProjectManager.GetInstance();
            var status = pm.AssociateProjectToUser(TestProject, TestUser);
            Assert.Equal(AssociationStatus.NoError, status);
            Project testProject2 = new Project("TestProject2", "TestProject2", DateTime.Now);
            status = pm.AssociateProjectToUser(testProject2, TestUser);
            Assert.Equal(AssociationStatus.NoError, status);
        }

        /// <summary>
        /// Test association of user to class return InvalidUser error when user is null
        /// </summary>
        [Fact]
        public void ProjectManagerTest_AssociateProjectToUserInvalidUser()
        {
            ProjectManager.ResetInstance();
            var pm = ProjectManager.GetInstance();
            var status = pm.AssociateProjectToUser(TestProject, null);
            Assert.Equal(AssociationStatus.InvalidUser, status);
        }

        /// <summary>
        /// Test association of user to class return InvalidProject error when project is null
        /// </summary>
        [Fact]
        public void ProjectManagerTest_AssociateProjectToUserInvalidProject()
        {
            ProjectManager.ResetInstance();
            var pm = ProjectManager.GetInstance();
            var status = pm.AssociateProjectToUser(null, TestUser);
            Assert.Equal(AssociationStatus.InvalidProject, status);
        }

        /// <summary>
        /// Test association of user to class return DuplicatedProjectToUserAssociation when attempting to insert
        /// the project twice 
        /// </summary>
        [Fact]
        public void ProjectManagerTest_AssociateProjectToUserDuplicateAssociation()
        {
            ProjectManager.ResetInstance();
            var pm = ProjectManager.GetInstance();
            var status = pm.AssociateProjectToUser(TestProject, TestUser);
            Assert.Equal(AssociationStatus.NoError, status);
            status = pm.AssociateProjectToUser(TestProject, TestUser);
            Assert.Equal(AssociationStatus.DuplicatedProjectToUserAssociation, status);
        }

        /// <summary>
        /// Tests remove project from user happy path
        /// </summary>
        [Fact]
        public void ProjectManagerTest_RemoveProjectFromUserHappyPath()
        {
            ProjectManager.ResetInstance();
            var pm = ProjectManager.GetInstance();
            var status = pm.AssociateProjectToUser(TestProject, TestUser);
            Assert.Equal(AssociationStatus.NoError, status);
            status = pm.RemoveProjectFromUser(TestProject, TestUser);
            Assert.Equal(AssociationStatus.NoError, status);
        }

        /// <summary>
        /// Tests remove project from user but project is null
        /// </summary>
        [Fact]
        public void ProjectManagerTest_RemoveProjectFromUserInvalidProject()
        {
            ProjectManager.ResetInstance();
            var pm = ProjectManager.GetInstance();
            var status = pm.RemoveProjectFromUser(null, TestUser);
            Assert.Equal(AssociationStatus.InvalidProject, status);
        }

        /// <summary>
        /// Tests remove project from user but user is null
        /// </summary>
        [Fact]
        public void ProjectManagerTest_RemoveProjectFromUserInvalidUser()
        {
            ProjectManager.ResetInstance();
            var pm = ProjectManager.GetInstance();
            var status = pm.RemoveProjectFromUser(TestProject, null);
            Assert.Equal(AssociationStatus.InvalidUser, status);
        }

        /// <summary>
        /// Tests remove project from user but project does not exist
        /// </summary>
        [Fact]
        public void ProjectManagerTest_RemoveProjectFromUserProjectNotFound()
        {
            ProjectManager.ResetInstance();
            var pm = ProjectManager.GetInstance();
            var status = pm.AssociateProjectToUser(TestProject, TestUser);
            Assert.Equal(AssociationStatus.NoError, status);
            Project invalidProject = new Project("TestProject2", "TestProject2", DateTime.Now);
            status = pm.RemoveProjectFromUser(invalidProject, TestUser);
            Assert.Equal(AssociationStatus.ProjectNotFound, status);
        }

        /// <summary>
        /// Tests remove project from user but user does not exist
        /// </summary>
        [Fact]
        public void ProjectManagerTest_RemoveProjectFromUserUserNotFound()
        {
            ProjectManager.ResetInstance();
            var pm = ProjectManager.GetInstance();
            var status = pm.AssociateProjectToUser(TestProject, TestUser);
            Assert.Equal(AssociationStatus.NoError, status);
            IUser invalidUser = new UserFake("TestUser");
            status = pm.RemoveProjectFromUser(TestProject, invalidUser);
            Assert.Equal(AssociationStatus.UserNotFound, status);
        }
    }
}