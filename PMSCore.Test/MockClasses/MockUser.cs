using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSCore.Test.MockClasses
{
    public class MockUser : IUserInterface
    {
        private string UserID;
        private Permission Permission;


        public Permission GetPermission()
        {
            throw new NotImplementedException();
        }

        public string GetUserID()
        {
            throw new NotImplementedException();
        }

        public bool Login(string userName)
        {
            throw new NotImplementedException();
        }

        public bool Register(string userName)
        {
            throw new NotImplementedException();
        }

        public void SetPermission(Permission permission)
        {
            throw new NotImplementedException();
        }
    }
}
