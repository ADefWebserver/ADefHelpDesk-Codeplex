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
using System.Text.RegularExpressions;
using System.Text;
using System.Net.Mail;

namespace ADefHelpDeskWebSite
{
    public partial class Account : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Don't allow user if not authenticated
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("Default.aspx");
            }

            if (!Page.IsPostBack)
            {
                dnnHelpDeskDALDataContext dnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

                // GetUser
                var objUser = (from ADefHelpDesk_Users in dnnHelpDeskDALDataContext.ADefHelpDesk_Users
                               where ADefHelpDesk_Users.UserID == Convert.ToInt32(HttpContext.Current.User.Identity.Name)
                               select ADefHelpDesk_Users).FirstOrDefault();

                txtUserName.Text = objUser.Username;
                txtFirstName.Text = objUser.FirstName;
                txtLastName.Text = objUser.LastName;
                txtEmail.Text = objUser.Email;
            }
        }

        #region lnkHome_Click
        protected void lnkHome_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("{0}", HttpContext.Current.Request.Url.AbsoluteUri));
        }
        #endregion

        #region btnSubmit_Click
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Clear previous errors
            ltError.Text = "";

            if (
                ValidateEmail() &
                ValidatePassword() &
                ValidatePasswordLength() &
                IsValidEmailAddress()
                )
            {
                dnnHelpDeskDALDataContext dnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

                // GetUser
                var objUser = (from ADefHelpDesk_Users in dnnHelpDeskDALDataContext.ADefHelpDesk_Users
                                           where ADefHelpDesk_Users.UserID == Convert.ToInt32(HttpContext.Current.User.Identity.Name)
                                           select ADefHelpDesk_Users).FirstOrDefault();

                // Only update password if it was entered
                if (txtPassword.Text.Trim().Length > 0)
                {
                    objUser.Password = Utility.HashPassword(txtUserName.Text.Trim().ToLower() + txtPassword.Text.Trim());
                }

                // // Update User
                ADefHelpDesk_User objADefHelpDesk_User = new ADefHelpDesk_User();

                objUser.FirstName = txtFirstName.Text.Trim();
                objUser.LastName = txtLastName.Text.Trim();
                objUser.Email = txtEmail.Text.Trim();

                dnnHelpDeskDALDataContext.SubmitChanges();

                ltError.Text = GetLocalResourceObject("lnkConfirmResource1.Text").ToString();
            }
        }
        #endregion

        #region ValidateEmail
        private bool ValidateEmail()
        {
            bool boolValidated = false;
            string strDuplicateEmail = GetLocalResourceObject("DuplicateEmail.Text").ToString();

            dnnHelpDeskDALDataContext dnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            var result = (from ADefHelpDesk_Users in dnnHelpDeskDALDataContext.ADefHelpDesk_Users
                          where ADefHelpDesk_Users.Email == txtEmail.Text.Trim()
                          where ADefHelpDesk_Users.UserID != Convert.ToInt32(HttpContext.Current.User.Identity.Name)                                           
                          select ADefHelpDesk_Users).FirstOrDefault();

            if (result != null)
            {
                ltError.Text = ltError.Text + "<br>" + strDuplicateEmail;
            }
            else
            {
                boolValidated = true;
            }

            return boolValidated;
        }
        #endregion

        #region ValidatePassword
        private bool ValidatePassword()
        {
            bool boolValidated = false;
            // Only validate password if it was entered
            if (txtPassword.Text.Trim().Length > 0 || txtConfirmPassword.Text.Trim().Length > 0)
            {
                string strPasswordMatch = GetLocalResourceObject("PasswordMatch.Text").ToString();

                if (txtPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
                {
                    ltError.Text = ltError.Text + "<br>" + strPasswordMatch;
                }
                else
                {
                    boolValidated = true;
                }
            }
            else
            {
                boolValidated = true;
            }

            return boolValidated;
        }
        #endregion

        #region ValidatePasswordLength
        private bool ValidatePasswordLength()
        {
            bool boolValidated = false;
            // Only validate password if it was entered
            if (txtPassword.Text.Trim().Length > 0 || txtConfirmPassword.Text.Trim().Length > 0)
            {
                string strPasswordLength = GetLocalResourceObject("PasswordLength.Text").ToString();

                if (txtPassword.Text.Trim().Length < 8)
                {
                    ltError.Text = ltError.Text + "<br>" + strPasswordLength;
                }
                else
                {
                    boolValidated = true;
                }
            }
            else
            {
                boolValidated = true;
            }

            return boolValidated;
        }
        #endregion

        #region IsValidEmailAddress
        // From: http://bytes.com/topic/c-sharp/answers/225262-code-validating-email-address-c-validate-email       
        public bool IsValidEmailAddress()
        {
            bool boolValidated = false;
            string strValidEmailAddress = GetLocalResourceObject("ValidEmailAddress.Text").ToString();

            if (!
                (
                Regex.IsMatch(txtEmail.Text.Trim(),
                @"^[-a-zA-Z0-9][-.a-zA-Z0-9]*@[-.a-zA-Z0-9]+(\.[-.a-zA-Z0-9]+)*\.
                    (com|edu|info|gov|int|mil|net|org|biz|name|museum|coop|aero|pro|
                    [a-zA-Z]{2})$",
                    RegexOptions.IgnorePatternWhitespace)
                    )
                )
            {
                ltError.Text = ltError.Text + "<br>" + strValidEmailAddress; ;
            }
            else
            {
                boolValidated = true;
            }

            return boolValidated;
        }
        #endregion

    }
}
