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
using System.Linq;
using System.Collections.Generic;
using DotNetNuke.Common;
using DotNetNuke.Entities.Users;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Collections;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Security.Roles;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using Microsoft.VisualBasic;
using DotNetNuke.Services.Localization;

namespace ADefWebserver.Modules.ADefHelpDeskUpgradeAdvisor
{
    public partial class View : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        public string SilverlightInitParams { get; set; }
        public string SilverlightSourceParams { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Register Silverlight.js file
            Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "SilverlightJS",
                (this.TemplateSourceDirectory + "/Silverlight.js"));

            DotNetNuke.Entities.Users.UserInfo objUser;
            objUser = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo();
            int intPortalID = objUser.PortalID;
            int intUserID = objUser.UserID;

            string strIPAddress = this.Context.Request.UserHostAddress;
            string strSilverlightPassword = Authendication.SetSilverlightKey(objUser, ModuleId, strIPAddress);
            string strWebServiceBase = GetWebServiceBase();

            SilverlightSourceParams = this.TemplateSourceDirectory + "/ClientBin/UpgradeAdvisorClient.xap";

            SilverlightInitParams =
                String.Format("PortalID={0},ModuleId={1},UserID={2},Password={3},WebServiceBase={4}",
                "0", ModuleId.ToString(), intUserID.ToString(), strSilverlightPassword, strWebServiceBase);
        }

        #region GetWebServiceBase
        private string GetWebServiceBase()
        {
            string strWebServiceBase = String.Format(@"{0}://", this.Request.Url.Scheme);
            strWebServiceBase = String.Format(@"{0}{1}", strWebServiceBase, this.Request.Url.Host.Replace(@"\", @"/"));
            strWebServiceBase = String.Format(@"{0}{1}", strWebServiceBase, ((this.Request.Url.Port == 80) ? "" : ":" + this.Request.Url.Port.ToString()));
            strWebServiceBase = String.Format(@"{0}{1}", strWebServiceBase, this.Request.ApplicationPath.Replace(@"\", ""));
            strWebServiceBase = String.Format(@"{0}/DesktopModules/ADefHelpDeskUpgradeAdvisor/", strWebServiceBase);
            return strWebServiceBase;
        }
        #endregion
    }
}
