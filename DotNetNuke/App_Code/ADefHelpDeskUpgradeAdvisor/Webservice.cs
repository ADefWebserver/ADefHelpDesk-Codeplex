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
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Web.Services.Protocols;
using System.Web.Script.Services;

using System.Data;
using System.Configuration;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.IdentityModel;
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Entities.Users;
using System.Reflection;
using System.Web.Compilation;
using DotNetNuke.Entities.Modules;

namespace ADefWebserver.Modules.ADefHelpDeskUpgradeAdvisor
{
    [WebService(Namespace = "http://ADefHelpDesk.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService()]
    public partial class WebService : System.Web.Services.WebService
    {
        #region HelloUser
        [WebMethod(Description = "HelloUser")]
        [ScriptMethod()]
        public string HelloUser(int PortalID, int ModuleId, int UserID, string Password)
        {
            string strIPAddress = this.Context.Request.UserHostAddress;
            ADefHelpDeskAuthendicationHeader SilverlightDesktopAuthendicationHeader = new ADefHelpDeskAuthendicationHeader();
            SilverlightDesktopAuthendicationHeader.PortalID = PortalID;
            SilverlightDesktopAuthendicationHeader.UserID = UserID;
            SilverlightDesktopAuthendicationHeader.Password = Password;
            SilverlightDesktopAuthendicationHeader.ModuleId = ModuleId;
            SilverlightDesktopAuthendicationHeader.IPAddress = strIPAddress;

            string response = "";

            Authendication Authendication = new Authendication(SilverlightDesktopAuthendicationHeader);
            if (Authendication.IsUserValid())
            {
                UserInfo objUser = Authendication.GetUserInfo();
                response = String.Format("Hello {0}!", objUser.DisplayName);
            }

            return response;
        }
        #endregion

        #region CurrentVersionOfADefHelpDeskUpgradeAdvisor
        [WebMethod]
        public string CurrentVersionOfADefHelpDeskUpgradeAdvisor(int PortalID, int ModuleId, int UserID, string Password)
        {
            string strIPAddress = this.Context.Request.UserHostAddress;
            ADefHelpDeskAuthendicationHeader SilverlightDesktopAuthendicationHeader = new ADefHelpDeskAuthendicationHeader();
            SilverlightDesktopAuthendicationHeader.PortalID = PortalID;
            SilverlightDesktopAuthendicationHeader.UserID = UserID;
            SilverlightDesktopAuthendicationHeader.Password = Password;
            SilverlightDesktopAuthendicationHeader.ModuleId = ModuleId;
            SilverlightDesktopAuthendicationHeader.IPAddress = strIPAddress;

            string response = "";

            Authendication Authendication = new Authendication(SilverlightDesktopAuthendicationHeader);
            if (Authendication.IsUserValid())
            {
               ADefHelpDeskWebProxy.ADefHelpDeskProxy.WebService objWebServiceSoapClient = new ADefHelpDeskWebProxy.ADefHelpDeskProxy.WebService();

               response = objWebServiceSoapClient.CurrentUpgradeVersion();
            }

            return response;
        }
        #endregion

        #region GetUpgradeItems
        [WebMethod]
        public List<UpgradeItems> GetUpgradeItems(int PortalID, int ModuleId, int UserID, string Password, string strCurrentVersion)
        {
            string strIPAddress = this.Context.Request.UserHostAddress;
            ADefHelpDeskAuthendicationHeader SilverlightDesktopAuthendicationHeader = new ADefHelpDeskAuthendicationHeader();
            SilverlightDesktopAuthendicationHeader.PortalID = PortalID;
            SilverlightDesktopAuthendicationHeader.UserID = UserID;
            SilverlightDesktopAuthendicationHeader.Password = Password;
            SilverlightDesktopAuthendicationHeader.ModuleId = ModuleId;
            SilverlightDesktopAuthendicationHeader.IPAddress = strIPAddress;

            List<UpgradeItems> ColUpgradeItems = new List<UpgradeItems>();

            Authendication Authendication = new Authendication(SilverlightDesktopAuthendicationHeader);
            if (Authendication.IsUserValid())
            {
                ADefHelpDeskWebProxy.ADefHelpDeskProxy.WebService objWebServiceSoapClient = new ADefHelpDeskWebProxy.ADefHelpDeskProxy.WebService();

                var tmpUpgradeItems = objWebServiceSoapClient.UpgradeCheck(strCurrentVersion);

                foreach (var item in tmpUpgradeItems)
                {
                    ColUpgradeItems.Add
                        (new UpgradeItems
                        {
                            Item = item.Item,
                            ItemDescription = item.ItemDescription,
                            ItemDetails = item.ItemDetails,
                            ItemMethodName = item.ItemMethodName
                        });
                }
            }

            return ColUpgradeItems;
        }
        #endregion

        #region CurrentVersionOfADefHelpDesk
        [WebMethod]
        public string CurrentVersionOfADefHelpDesk(int PortalID, int ModuleId, int UserID, string Password)
        {
            string strIPAddress = this.Context.Request.UserHostAddress;
            ADefHelpDeskAuthendicationHeader SilverlightDesktopAuthendicationHeader = new ADefHelpDeskAuthendicationHeader();
            SilverlightDesktopAuthendicationHeader.PortalID = PortalID;
            SilverlightDesktopAuthendicationHeader.UserID = UserID;
            SilverlightDesktopAuthendicationHeader.Password = Password;
            SilverlightDesktopAuthendicationHeader.ModuleId = ModuleId;
            SilverlightDesktopAuthendicationHeader.IPAddress = strIPAddress;

            string response = "";

            Authendication Authendication = new Authendication(SilverlightDesktopAuthendicationHeader);
            if (Authendication.IsUserValid())
            {
                ModuleController objModuleController = new ModuleController();
                var colModuleInfo = objModuleController.GetAllModules().Cast<ModuleInfo>().ToList();

                var objModule = (from Module in colModuleInfo
                                 where Module.ModuleTitle == "ADefHelpDesk"
                                 select Module).FirstOrDefault();

                if (objModule != null)
                {
                    response = objModule.Version;
                }
            }

            return response;
        }
        #endregion

        #region ADefHelpDeskLastSearchCurrentPagePresent
        [WebMethod]
        public bool ADefHelpDeskLastSearchCurrentPagePresent(int PortalID, int ModuleId, int UserID, string Password)
        {
            string strIPAddress = this.Context.Request.UserHostAddress;
            ADefHelpDeskAuthendicationHeader SilverlightDesktopAuthendicationHeader = new ADefHelpDeskAuthendicationHeader();
            SilverlightDesktopAuthendicationHeader.PortalID = PortalID;
            SilverlightDesktopAuthendicationHeader.UserID = UserID;
            SilverlightDesktopAuthendicationHeader.Password = Password;
            SilverlightDesktopAuthendicationHeader.ModuleId = ModuleId;
            SilverlightDesktopAuthendicationHeader.IPAddress = strIPAddress;

            bool response = false;

            Authendication Authendication = new Authendication(SilverlightDesktopAuthendicationHeader);
            if (Authendication.IsUserValid())
            {
                var TheClass = BuildManager.GetType("ADefWebserver.Modules.ADefHelpDesk.ADefHelpDeskDALDataContext", true, true);
                var TheTable = TheClass.Module.GetTypes().Where(x => x.Name == "ADefHelpDesk_LastSearch");
                var TheField = TheTable.FirstOrDefault().GetProperties().Where(y => y.Name.Contains("CurrentPage"));

                if (TheField.FirstOrDefault() != null)
                {
                    response = true;
                }
            }

            return response;
        }
        #endregion

        #region UpgradeItems
        [Serializable]
        public class UpgradeItems
        {
            public string Item { get; set; }
            public string ItemDescription { get; set; }
            public string ItemDetails { get; set; }
            public string ItemMethodName { get; set; }
        }
        #endregion
    }
}