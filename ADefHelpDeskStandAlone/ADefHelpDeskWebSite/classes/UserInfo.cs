//
// ADefwebserver.com
// Copyright (c) 2009
// by Michael Washington
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace ADefWebserver.Modules.ADefHelpDesk
{
    public class UserInfo
    {
        private string _FirstName;

        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }
        private string _LastName;

        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }
        private string _Username;

        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }

        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        private string _DisplayName;

        public string DisplayName
        {
            get { return _DisplayName; }
            set { _DisplayName = value; }
        }

        private int _UserId;

        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        #region IsInRole
        public static bool IsInRole(int UserId, string Role)
        {
            bool boolInRole = false;

            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            var result = (from ADefHelpDesk_UserRoles in objdnnHelpDeskDALDataContext.ADefHelpDesk_UserRoles
                          where ADefHelpDesk_UserRoles.UserID == UserId
                          where ADefHelpDesk_UserRoles.ADefHelpDesk_Role.RoleName == Role
                          select ADefHelpDesk_UserRoles).FirstOrDefault();

            if (result != null)
            {
                boolInRole = true;
            }

            return boolInRole;
        } 
        #endregion

        #region IsSuperUser
        public static bool IsSuperUser(int UserId)
        {
            bool boolSuperUser = false;

            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            var result = (from ADefHelpDesk_Users in objdnnHelpDeskDALDataContext.ADefHelpDesk_Users
                          where ADefHelpDesk_Users.UserID == UserId
                          where ADefHelpDesk_Users.IsSuperUser == true
                          select ADefHelpDesk_Users).FirstOrDefault();

            if (result != null)
            {
                boolSuperUser = true;
            }

            return boolSuperUser;
        }
        #endregion
    }
}
