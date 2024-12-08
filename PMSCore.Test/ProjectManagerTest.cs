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
        /// Tests Association of user to project happy path
        /// </summary>
        [Fact]
        public void ProjectManagerTest_AssociateProjectToUserHappyPath()
        {
            var pm = ProjectManager.GetInstance();
            Project testProject = new Project();
            IUser testUser = new UserMock();
            var status = pm.AssociateProjectToUser(testProject, testUser);
            Assert.Equal(AssociationStatus.NoError, status);
        }

        /// <summary>
        /// Tests Association of user to two projects happy path
        /// </summary>
        [Fact]
        public void ProjectManagerTest_AssociateProjectToUserAddTwoProjectsHappyPath()
        {
            var pm = ProjectManager.GetInstance();
            Project testProject = new Project();
            IUser testUser = new UserMock();
            var status = pm.AssociateProjectToUser(testProject, testUser);
            Assert.Equal(AssociationStatus.NoError, status);
            Project testProject2 = new Project();
            status = pm.AssociateProjectToUser(testProject2, testUser);
            Assert.Equal(AssociationStatus.NoError, status);
        }

        /// <summary>
        /// Test association of user to class return InvalidUser error when user is null
        /// </summary>
        [Fact]
        public void ProjectManagerTest_AssociateProjectToUserInvalidUser()
        {
            var pm = ProjectManager.GetInstance();
            Project testProject = new Project();
            var status = pm.AssociateProjectToUser(testProject, null);
            Assert.Equal(AssociationStatus.InvalidUser, status);
        }

        /// <summary>
        /// Test association of user to class return InvalidProject error when project is null
        /// </summary>
        [Fact]
        public void ProjectManagerTest_AssociateProjectToUserInvalidProject()
        {
            var pm = ProjectManager.GetInstance();
            IUser testUser = new UserMock();
            var status = pm.AssociateProjectToUser(null, testUser);
            Assert.Equal(AssociationStatus.InvalidProject, status);
        }

        /// <summary>
        /// Test association of user to class return DuplicatedProjectToUserAssociation when attempting to insert
        /// the project twice 
        /// </summary>
        [Fact]
        public void ProjectManagerTest_AssociateProjectToUserDuplicateAssociation()
        {
            var pm = ProjectManager.GetInstance();
            Project testProject = new Project();
            IUser testUser = new UserMock();
            var status = pm.AssociateProjectToUser(testProject, testUser);
            Assert.Equal(AssociationStatus.NoError, status);
            status = pm.AssociateProjectToUser(testProject, testUser);
            Assert.Equal(AssociationStatus.DuplicatedProjectToUserAssociation, status);
        }

        /// <summary>
        /// Tests remove project from user happy path
        /// </summary>
        [Fact]
        public void ProjectManagerTest_RemoveProjectFromUserHappyPath()
        {
            var pm = ProjectManager.GetInstance();
            Project testProject = new Project();
            IUser testUser = new UserMock();
            var status = pm.AssociateProjectToUser(testProject, testUser);
            Assert.Equal(AssociationStatus.NoError, status);
            status = pm.RemoveProjectFromUser(testProject, testUser);
            Assert.Equal(AssociationStatus.NoError, status);
        }

        /// <summary>
        /// Tests remove project from user but project is null
        /// </summary>
        [Fact]
        public void ProjectManagerTest_RemoveProjectFromUserInvalidProject()
        {
            var pm = ProjectManager.GetInstance();
            IUser testUser = new UserMock();
            var status = pm.RemoveProjectFromUser(null, testUser);
            Assert.Equal(AssociationStatus.InvalidProject, status);
        }

        /// <summary>
        /// Tests remove project from user but user is null
        /// </summary>
        [Fact]
        public void ProjectManagerTest_RemoveProjectFromUserInvalidUser()
        {
            var pm = ProjectManager.GetInstance();
            Project testProject = new Project();
            var status = pm.RemoveProjectFromUser(testProject, null);
            Assert.Equal(AssociationStatus.InvalidUser, status);
        }

        /// <summary>
        /// Tests remove project from user but project does not exist
        /// </summary>
        [Fact]
        public void ProjectManagerTest_RemoveProjectFromUserProjectNotFound()
        {
            var pm = ProjectManager.GetInstance();
            Project testProject = new Project();
            IUser testUser = new UserMock();
            var status = pm.AssociateProjectToUser(testProject, testUser);
            Assert.Equal(AssociationStatus.NoError, status);
            Project invalidProject = new Project();
            status = pm.RemoveProjectFromUser(invalidProject, testUser);
            Assert.Equal(AssociationStatus.ProjectNotFound, status);
        }

        /// <summary>
        /// Tests remove project from user but user does not exist
        /// </summary>
        [Fact]
        public void ProjectManagerTest_RemoveProjectFromUserUserNotFound()
        {
            var pm = ProjectManager.GetInstance();
            Project testProject = new Project();
            IUser testUser = new UserMock();
            var status = pm.AssociateProjectToUser(testProject, testUser);
            Assert.Equal(AssociationStatus.NoError, status);
            IUser invalidUser = new UserMock();
            status = pm.RemoveProjectFromUser(testProject, invalidUser);
            Assert.Equal(AssociationStatus.UserNotFound, status);
        }
    }
}