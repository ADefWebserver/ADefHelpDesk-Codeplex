//
// http://ADefwebserver.com
// Copyright (c) 2010
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
// Silk icon set 1.3 by
// Mark James
// http://www.famfamfam.com/lab/icons/silk/
// Creative Commons Attribution 2.5 License.
// [ http://creativecommons.org/licenses/by/2.5/ ]

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ADefWebserver.Modules.ADefHelpDesk;

namespace ADefHelpDeskWebSite
{
    public partial class TaskEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Show Registration link
            GeneralSettings objGeneralSettings = new GeneralSettings();
            lnkRegister.Visible = objGeneralSettings.AllowRegistration;

            if (User.Identity.IsAuthenticated)
            {
                lnkLogin.Visible = false;
                lnkLogOut.Visible = true;

                // Don't show Register link if not logged in:
                lnkRegister.Visible = false;
            }
            else
            {
                lnkLogin.Visible = true;
                lnkLogOut.Visible = false;
            }
        }

        #region LogOut
        protected void lnkLogOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("Default.aspx");
        }
        #endregion
    }
}
