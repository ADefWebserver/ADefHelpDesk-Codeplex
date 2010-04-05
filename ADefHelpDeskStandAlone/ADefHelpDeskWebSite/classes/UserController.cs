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
    public class UserController
    {
        #region GetUser
        public static UserInfo GetUser(int PortalId, int UserId, bool IsHydrated)
        {
            // Note: PortalId is not implemented at this time
            // Note: IsHydrated is not implemented at this time

            UserInfo objUserInfo = new UserInfo();

            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            var result = (from ADefHelpDesk_Users in objdnnHelpDeskDALDataContext.ADefHelpDesk_Users
                          where ADefHelpDesk_Users.UserID == UserId
                          select ADefHelpDesk_Users).FirstOrDefault();

            if (result != null)
            {
                objUserInfo.UserId = result.UserID;
                objUserInfo.DisplayName = string.Format("{0} {1}", result.FirstName, result.LastName);
                objUserInfo.Email = result.Email;
                objUserInfo.FirstName = result.FirstName;
                objUserInfo.LastName = result.LastName;
                objUserInfo.Username = result.Username;

                // Fix DisplayName if needed
                if (objUserInfo.DisplayName.Trim().Length == 0)
                {
                    objUserInfo.DisplayName = objUserInfo.Username;
                }
            }

            return objUserInfo;
        }
        #endregion

        #region GetUsersByEmail
        public static List<UserInfo> GetUsersByEmail(string SearchText)
        {
            List<UserInfo> colUserInfo = new List<UserInfo>();

            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            var result = from ADefHelpDesk_Users in objdnnHelpDeskDALDataContext.ADefHelpDesk_Users
                          where ADefHelpDesk_Users.Email.Contains(SearchText)
                          select ADefHelpDesk_Users;

            foreach (var item in result)
            {
                UserInfo objUserInfo = new UserInfo();

                objUserInfo.UserId = item.UserID;
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

        #region GetUsersByProfileProperty
        public static List<UserInfo> GetUsersByProfileProperty(string propertyName, string SearchText)
        {
            List<UserInfo> colUserInfo = new List<UserInfo>();

            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            var result = from ADefHelpDesk_Users in objdnnHelpDeskDALDataContext.ADefHelpDesk_Users
                         select ADefHelpDesk_Users;

            if (propertyName == "FirstName")
            {
                result = from ADefHelpDesk_Users in result
                         where ADefHelpDesk_Users.FirstName.Contains(SearchText)
                         select ADefHelpDesk_Users;
            }

            if (propertyName == "LastName")
            {
                result = from ADefHelpDesk_Users in result
                         where ADefHelpDesk_Users.LastName.Contains(SearchText)
                         select ADefHelpDesk_Users;
            }

            foreach (var item in result)
            {
                UserInfo objUserInfo = new UserInfo();

                objUserInfo.UserId = item.UserID;
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
    }
}
