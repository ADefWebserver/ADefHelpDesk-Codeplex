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

namespace ADefWebserver.Modules.ADefHelpDesk
{
    public partial class Login : System.Web.UI.Page
    {
        #region ViewState
        public int intUserID
        {
            get
            {
                if (ViewState["UserID"] == null)
                {
                    return 0;
                }
                else
                {
                    return (int)ViewState["UserID"];
                }
            }
            set
            {
                ViewState["UserID"] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region LoginControl_Authenticate
        protected void LoginControl_Authenticate(object sender, AuthenticateEventArgs e)
        {
            // Try to authendicate
            dnnHelpDeskDALDataContext dnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            var result = from AUser in dnnHelpDeskDALDataContext.ADefHelpDesk_Users
                         where AUser.Username == LoginControl.UserName
                         select AUser;

            ADefHelpDesk_User User = (ADefHelpDesk_User)result.FirstOrDefault();

            if (User != null)
            {
                if (User.Password == Utility.HashPassword(User.Username.ToLower() + LoginControl.Password))
                {
                    if (User.VerificationCode == null)
                    {
                        LogUserIntoSite(User);
                    }
                    else
                    {
                        // Verification code is required
                        LoginControl.Visible = false;
                        pnlVerification.Visible = true;

                        // Save the UserID for the round trip
                        intUserID = User.UserID;
                    }
                }
            }
            else
            {
                string strFailureText = GetLocalResourceObject("FailureText.Text").ToString();
                LoginControl.FailureText = strFailureText;
            }
        }        
        #endregion

        #region LogUserIntoSite
        private void LogUserIntoSite(ADefHelpDesk_User User)
        {
            // Log the user into the site
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
              User.UserID.ToString(),
              DateTime.Now,
              DateTime.Now.AddDays(30),
              false,
              User.IsSuperUser.ToString(),
              FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            string encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie.
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

            // Redirect back to original URL.
            Response.Redirect(FormsAuthentication.GetRedirectUrl(User.Username, false));
        }  
        #endregion

        #region btnVerify_Click
        protected void btnVerify_Click(object sender, EventArgs e)
        {
            // Verify Code
            dnnHelpDeskDALDataContext dnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            var result = (from AUser in dnnHelpDeskDALDataContext.ADefHelpDesk_Users
                         where AUser.UserID == intUserID
                         select AUser).FirstOrDefault();

            if (result != null)
            {
                if (result.VerificationCode.ToLower() == txtVerificationCode.Text.ToLower())
                {
                    // Clear the Verification code
                    result.VerificationCode = null;
                    dnnHelpDeskDALDataContext.SubmitChanges();
                    // Log user into site
                    LogUserIntoSite(result);
                }
                else
                {
                    // Bad Verification code
                    lblVerificationNotCorrect.Visible = true;
                    lblVerificationRequired.Visible = false;
                    txtVerificationCode.Text = "";
                }
            }
            else
            {
                // User not found
                // This should never happen
                Response.Redirect(string.Format("{0}", HttpContext.Current.Request.Url.AbsoluteUri));
            }
        } 
        #endregion

    }
}