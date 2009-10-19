//
// http://ADefwebserver.com
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

using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Text;
using Microsoft.VisualBasic;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Personalization;
using DotNetNuke.Security.Membership;

namespace ADefWebserver.Modules.ADefHelpDeskUpgradeAdvisor
{
    public class Authendication
    {
        private int _PortalID;
        private int _UserID;
        private string _Password;
        private int _ModuleId;
        private string _IPAddress;

        public Authendication()
        {
            _PortalID = 0;
            _ModuleId = 0;
            _UserID = -1;
            _Password = "";
            _IPAddress = "127.0.0.1";
        }

        public Authendication(ADefHelpDeskAuthendicationHeader Credentials)
        {
            _PortalID = Credentials.PortalID;
            _ModuleId = Credentials.ModuleId;
            _UserID = Credentials.UserID;
            _Password = Credentials.Password.Trim();
            _IPAddress = Credentials.IPAddress;
        }

        #region SetSilverlightKey
        public static string SetSilverlightKey(UserInfo objUser, int ModuleId, string IPAddress)
        {
            string strRandomPassword = "";
            if (objUser.UserID > -1)
            {
                // Get a random password
                Utility Utility = new Utility();
                strRandomPassword = Utility.GetRandomPassword();

                // Get the users Personalization Info
                PersonalizationController PersonalizationController = new PersonalizationController();
                PersonalizationInfo PersonalizationInfo = new PersonalizationInfo();
                PersonalizationInfo = PersonalizationController.LoadProfile(objUser.UserID, objUser.PortalID);

                //  Store the encryption key in the users profile 
                DotNetNuke.Services.Personalization.Personalization.SetProfile(PersonalizationInfo, ModuleId.ToString(), "SilverlightPassword", strRandomPassword);
                DotNetNuke.Services.Personalization.Personalization.SetProfile(PersonalizationInfo, ModuleId.ToString(), "IPAddress", IPAddress);
                PersonalizationController.SaveProfile(PersonalizationInfo, objUser.UserID, objUser.PortalID);

            }
            return strRandomPassword;
        }

        public string SetSilverlightKey()
        {
            UserInfo UserInfo = GetUserInfo();
            return SetSilverlightKey(UserInfo, _ModuleId, _IPAddress);
        }
        #endregion

        #region IsUserValid
        public bool IsUserValid()
        {
            if (_UserID == -1)
            {
                return false;
            }

            return ValidateUser();
        }
        #endregion

        #region GetUserInfo
        public UserInfo GetUserInfo()
        {
            UserInfo UserInfo = new UserInfo();
            UserInfo = UserController.GetUser(_PortalID, _UserID, false);
            return UserInfo;
        }

        public UserInfo GetUserInfo(int PortalID, int UserID)
        {
            UserInfo UserInfo = new UserInfo();

            if (UserID == -1)
            {
                return UserInfo;
            }

            UserInfo = UserController.GetUser(PortalID, UserID, false);
            return UserInfo;
        }
        #endregion

        #region ValidateUser
        private bool ValidateUser()
        {
            //  Get the  Key from Personalization
            PersonalizationController PersonalizationController = new PersonalizationController();
            PersonalizationInfo PersonalizationInfo = new PersonalizationInfo();
            PersonalizationInfo = PersonalizationController.LoadProfile(_UserID, _PortalID);

            if (!(Personalization.GetProfile(PersonalizationInfo, _ModuleId.ToString(), "SilverlightPassword") == null) & !(Personalization.GetProfile(PersonalizationInfo, _ModuleId.ToString(), "IPAddress") == null))
            {
                string SilverlightPassword = Convert.ToString(Personalization.GetProfile(PersonalizationInfo, _ModuleId.ToString(), "SilverlightPassword"));
                string IPAddress = Convert.ToString(Personalization.GetProfile(PersonalizationInfo, _ModuleId.ToString(), "IPAddress"));

                if ((SilverlightPassword == _Password) & (Strings.Left(IPAddress, 6) == Strings.Left(_IPAddress, 6)))
                {
                    // UserID and password are valid
                    // Is user's account active?

                    UserLoginStatus objloginStatus = new UserLoginStatus();
                    UserInfo SilverlightUser = new UserInfo();

                    SilverlightUser = UserController.GetUser(_PortalID, _UserID, false);
                    string tmpPassword = UserController.GetPassword(ref SilverlightUser, "").ToString();
                    string tmpUsername = SilverlightUser.Username;

                    UserInfo objUser = UserController.ValidateUser(_PortalID, tmpUsername, tmpPassword, "", "", "127.0.0.1", ref objloginStatus);
                    if (objUser == null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    if (Strings.Left(IPAddress, 6) == Strings.Left(_IPAddress, 6))
                    {
                        // The correct IP address was used
                        // To prevent a brute force attack scramble the password
                        // A hacker is now chasing a moving target
                        SetSilverlightKey();
                    }
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}