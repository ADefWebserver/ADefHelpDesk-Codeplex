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
    public partial class Register : System.Web.UI.Page
    {
        GeneralSettings objGeneralSettings;

        protected void Page_Load(object sender, EventArgs e)
        {

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
                ValidateUserName() &
                ValidateEmail() &
                ValidatePassword() &
                ValidatePasswordLength() &
                IsValidEmailAddress()
                )
            {
                // Create User
                dnnHelpDeskDALDataContext dnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

                // Check to see if another user has the same User Name
                var objSelectedUserName = (from ADefHelpDesk_Users in dnnHelpDeskDALDataContext.ADefHelpDesk_Users
                                           where ADefHelpDesk_Users.Username == txtUserName.Text.Trim()
                                           select ADefHelpDesk_Users).FirstOrDefault();

                if (objSelectedUserName != null)
                {
                    string strDuplicateUserName = GetLocalResourceObject("DuplicateUserName.Text").ToString();
                    ltError.Text = strDuplicateUserName;
                    return;
                }

                // Check to see if another user has the same email
                var objSelectedEmail = (from ADefHelpDesk_Users in dnnHelpDeskDALDataContext.ADefHelpDesk_Users
                                        where ADefHelpDesk_Users.Email == txtEmail.Text.Trim()
                                        select ADefHelpDesk_Users).FirstOrDefault();

                if (objSelectedEmail != null)
                {
                    string strDuplicateEmail = GetLocalResourceObject("DuplicateEmail.Text").ToString();
                    ltError.Text = strDuplicateEmail;
                    return;
                }

                // Insert User
                ADefHelpDesk_User objADefHelpDesk_User = new ADefHelpDesk_User();

                objADefHelpDesk_User.IsSuperUser = false;
                objADefHelpDesk_User.Username = txtUserName.Text.Trim();
                objADefHelpDesk_User.FirstName = txtFirstName.Text.Trim();
                objADefHelpDesk_User.LastName = txtLastName.Text.Trim();
                objADefHelpDesk_User.Email = txtEmail.Text.Trim();
                objADefHelpDesk_User.Password = Utility.HashPassword(txtUserName.Text.Trim().ToLower() + txtPassword.Text.Trim());

                dnnHelpDeskDALDataContext.ADefHelpDesk_Users.InsertOnSubmit(objADefHelpDesk_User);
                dnnHelpDeskDALDataContext.SubmitChanges();

                // Is Verified registration turned on?
                LoadGeneralSettings();

                if (objGeneralSettings.VerifiedRegistration)
                {
                    // Verified registration - Create a Verification code
                    string strVerificationCode = GetRandomCode();
                    objADefHelpDesk_User.VerificationCode = strVerificationCode;
                    dnnHelpDeskDALDataContext.SubmitChanges();

                    string strEmailResponse = SendVerificationEmail(txtEmail.Text.Trim(), txtUserName.Text.Trim(), strVerificationCode);

                    lnkConfirm.Visible = false;
                    lnkConfirmWithVerification.Visible = true;

                    if (strEmailResponse.Length > 0)
                    {
                        // There was a problem sending the email
                        lnkConfirmWithVerification.Text = strEmailResponse;
                    }
                }
                else
                {
                    // Non verified registration - automatically log in
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                      objADefHelpDesk_User.UserID.ToString(),
                      DateTime.Now,
                      DateTime.Now.AddDays(30),
                      false,
                      objADefHelpDesk_User.IsSuperUser.ToString(),
                      FormsAuthentication.FormsCookiePath);

                    // Encrypt the ticket.
                    string encTicket = FormsAuthentication.Encrypt(ticket);

                    // Create the cookie.
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                    lnkConfirm.Visible = true;
                    lnkConfirmWithVerification.Visible = false;
                }

                // Hide main screen and show confirmation screen
                pnlCreateAccount.Visible = false;
                pnlConfirmation.Visible = true;
            }
        }
        #endregion

        #region ValidateUserName
        private bool ValidateUserName()
        {
            bool boolValidated = false;
            string strDuplicateUserName = GetLocalResourceObject("DuplicateUserName.Text").ToString();

            dnnHelpDeskDALDataContext dnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            var result = (from ADefHelpDesk_Users in dnnHelpDeskDALDataContext.ADefHelpDesk_Users
                          where ADefHelpDesk_Users.Username == txtUserName.Text.Trim()
                          select ADefHelpDesk_Users).FirstOrDefault();

            if (result != null)
            {
                ltError.Text = ltError.Text + "<br>" + strDuplicateUserName;
            }
            else
            {
                boolValidated = true;
            }

            return boolValidated;
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
            string strPasswordMatch = GetLocalResourceObject("PasswordMatch.Text").ToString();

            if (txtPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
            {
                ltError.Text = ltError.Text + "<br>" + strPasswordMatch;
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
            string strPasswordLength = GetLocalResourceObject("PasswordLength.Text").ToString();

            if (txtPassword.Text.Trim().Length < 8)
            {
                ltError.Text = ltError.Text + "<br>" + strPasswordLength;
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

            if (txtEmail.Text.IndexOf("@") < 1)
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

        #region GetRandomCode
        public string GetRandomCode()
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            int intElements = random.Next(4, 6);

            for (int i = 0; i < intElements; i++)
            {
                int intRandomType = random.Next(0, 2);
                if (intRandomType == 1)
                {
                    char ch;
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                    builder.Append(ch);
                }
                else
                {
                    builder.Append(random.Next(0, 9));
                }
            }
            return builder.ToString();
        }
        #endregion

        #region LoadGeneralSettings
        private void LoadGeneralSettings()
        {
            objGeneralSettings = new GeneralSettings();
        }
        #endregion

        #region SendVerificationEmail
        protected string SendVerificationEmail(string ToEmail, string Username, String VerificationCode)
        {
            string[] arrAttachments = new string[0];
            string strEmailResponse =
                Email.SendMail(
                objGeneralSettings.SMTPFromEmail,
                ToEmail,
                "", "",
                objGeneralSettings.SMTPFromEmail,
                MailPriority.Normal,
                String.Format(GetLocalResourceObject("Subject.Text").ToString(), Username),
                Encoding.UTF8,
                String.Format(GetLocalResourceObject("MailBody.Text").ToString(), Username, VerificationCode, HttpContext.Current.Request.Url.AbsoluteUri.Replace("Register.aspx", "Login.aspx")),
                arrAttachments,
                objGeneralSettings.SMTPServer,
                objGeneralSettings.SMTPAuthendication,
                objGeneralSettings.SMTPUserName,
                objGeneralSettings.SMTPPassword,
                objGeneralSettings.SMTPSecure);

            return (strEmailResponse.Trim() == "") ? "" : strEmailResponse;
        }
        #endregion
    }
}
