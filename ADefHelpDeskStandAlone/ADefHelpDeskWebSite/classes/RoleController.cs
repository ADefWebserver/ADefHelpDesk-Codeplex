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
    #region RoleInfo
    public class RoleInfo
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
    } 
    #endregion

    public class RoleController
    {
        #region GetRoles
        public List<RoleInfo> GetRoles()
        {
            List<RoleInfo> colRoles = new List<RoleInfo>();

            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            var result = from ADefHelpDesk_Roles in objdnnHelpDeskDALDataContext.ADefHelpDesk_Roles
                         select ADefHelpDesk_Roles;

            foreach (var item in result)
            {
                RoleInfo objRoleInfo = new RoleInfo();

                objRoleInfo.RoleID = item.ID;
                objRoleInfo.RoleName = item.RoleName;

                colRoles.Add(objRoleInfo);
            }

            return colRoles;
        }
        #endregion

        #region GetRole
        public RoleInfo GetRole(int PortalId, int RoleID)
        {
            // Note: PortalId is not implemented at this time
            RoleInfo objRoleInfo = new RoleInfo();

            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            var result = (from ADefHelpDesk_Roles in objdnnHelpDeskDALDataContext.ADefHelpDesk_Roles
                          where ADefHelpDesk_Roles.ID == RoleID
                          select ADefHelpDesk_Roles).FirstOrDefault();

            if (result != null)
            {
                objRoleInfo.RoleID = result.ID;
                objRoleInfo.RoleName = result.RoleName;
            }

            return objRoleInfo;
        }
        #endregion

        #region GetUsersByRoleName
        public List<UserInfo> GetUsersByRoleName(int PortalId, string RoleName)
        {
            // Note: PortalId is not implemented at this time
            List<UserInfo> colUserInfo = new List<UserInfo>();

            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            var result = from ADefHelpDesk_Roles in objdnnHelpDeskDALDataContext.ADefHelpDesk_Roles
                         from ADefHelpDesk_UserRoles in objdnnHelpDeskDALDataContext.ADefHelpDesk_UserRoles
                         where ADefHelpDesk_Roles.ID == ADefHelpDesk_UserRoles.RoleID
                         where ADefHelpDesk_Roles.RoleName == RoleName
                         select ADefHelpDesk_UserRoles.ADefHelpDesk_User;

            foreach (var item in result)
            {
                UserInfo objUserInfo = new UserInfo();

                objUserInfo.DisplayName = string.Format("{0} {1}", item.FirstName, item.LastName);
                objUserInfo.Email = item.Email;
                objUserInfo.FirstName = item.FirstName;
                objUserInfo.LastName = item.LastName;
                objUserInfo.Username = item.Username;

                colUserInfo.Add(objUserInfo);
            }

            return colUserInfo;
        }
        #endregion

        #region GetSuperUsers
        public List<UserInfo> GetSuperUsers()
        {
            // Note: PortalId is not implemented at this time
            List<UserInfo> colUserInfo = new List<UserInfo>();

            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            var result = from ADefHelpDesk_Users in objdnnHelpDeskDALDataContext.ADefHelpDesk_Users
                         where ADefHelpDesk_Users.IsSuperUser == true
                         select ADefHelpDesk_Users;

            foreach (var item in result)
            {
                UserInfo objUserInfo = new UserInfo();

                objUserInfo.DisplayName = string.Format("{0} {1}", item.FirstName, item.LastName);
                objUserInfo.Email = item.Email;
                objUserInfo.FirstName = item.FirstName;
                objUserInfo.LastName = item.LastName;
                objUserInfo.Username = item.Username;

                colUserInfo.Add(objUserInfo);
            }

            return colUserInfo;
        }
        #endregion

        #region GetUserRoles
        public List<RoleInfo> GetUserRoles(int PortalId, int UserID)
        {
            List<RoleInfo> colRoles = new List<RoleInfo>();

            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            var result = from ADefHelpDesk_UserRoles in objdnnHelpDeskDALDataContext.ADefHelpDesk_UserRoles
                         where ADefHelpDesk_UserRoles.UserID == UserID
                         select ADefHelpDesk_UserRoles.ADefHelpDesk_Role;

            foreach (var item in result)
            {
                RoleInfo objRoleInfo = new RoleInfo();

                objRoleInfo.RoleID = item.ID;
                objRoleInfo.RoleName = item.RoleName;

                colRoles.Add(objRoleInfo);
            }

            return colRoles;
        }
        #endregion
    }
}
