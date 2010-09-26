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
using System.Linq;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace ADefWebserver.Modules.ADefHelpDesk
{
    public class Utility
    {
        #region DatabaseReady
        public static bool DatabaseReady()
        {
            string strCurrentVersion = "02.10.00";
            // This method returns true if the database exists and the table is created
            bool CanConnect = true;

            try
            {
                dnnHelpDeskDALDataContext dnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();
                var result = from VersionNumber in dnnHelpDeskDALDataContext.ADefHelpDesk_Versions
                             select VersionNumber;
                CanConnect = (result.FirstOrDefault().VersionNumber == strCurrentVersion) ? true : false;
            }
            catch (Exception e)
            {
                string strError = e.Message;
                CanConnect = false;
            }

            return CanConnect;
        }
        #endregion

        #region ConvertToText
        public static string ConvertToText(string sHTML)
        {
            string sContent = sHTML;
            sContent = sContent.Replace("<br />", Environment.NewLine);
            sContent = sContent.Replace("<br>", Environment.NewLine);
            sContent = FormatText(sContent, true);
            return StripTags(sContent, true);
        }
        #endregion

        #region FormatText
        public static string FormatText(string HTML, bool RetainSpace)
        {
            //Match all variants of <br> tag (<br>, <BR>, <br/>, including embedded space
            string brMatch = "\\s*<\\s*[bB][rR]\\s*/\\s*>\\s*";
            //Replace Tags by replacement String and return mofified string
            return System.Text.RegularExpressions.Regex.Replace(HTML, brMatch, Environment.NewLine);
        }
        #endregion

        #region StripTags
        public static string StripTags(string HTML, bool RetainSpace)
        {
            //Set up Replacement String
            string RepString;
            if (RetainSpace)
            {
                RepString = " ";
            }
            else
            {
                RepString = "";
            }

            //Replace Tags by replacement String and return mofified string
            return System.Text.RegularExpressions.Regex.Replace(HTML, "<[^>]*>", RepString);
        }
        #endregion    

        #region NavigateURL
        public static string NavigateURL(HttpContext Page, string PageName)
        {
            string Scheme = Page.Request.Url.Scheme;
            string UrlAuthority = Page.Request.Url.Authority;
            string ApplicationPath = Page.Request.ApplicationPath;

            string AuthorityAndApplicationPath = String.Format(@"{0}{1}", UrlAuthority, ApplicationPath);
            string PathAndPage = String.Format(@"{0}/{1}", AuthorityAndApplicationPath, PageName);

            PathAndPage = PathAndPage.Replace(@"//", @"/");
            
            string response = String.Format(@"{0}://{1}", Scheme, PathAndPage);

            return response;
        }
        #endregion    

        #region HashPassword
        public static string HashPassword(string Password)
        {
            // Note this password is 'salted' with the username when it is passed
            // to this method, so a basic dictionary attack would not work
            string HashedPassword = "";

            HashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(Password, 
                System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());

            return HashedPassword;
        }
        #endregion

        // JavaScript calendar

        #region InsertCalendarJavaScript
        public static string InsertCalendarJavaScript(string ControlID)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<script type='text/javascript'>$(function() { ");
            sb.Append(string.Format("$('#{0}').datepicker(", ControlID));
            sb.Append("{ ");
            sb.Append("showOn: 'button', ");
            sb.Append("buttonImage: 'images/calendar.png', ");
            sb.Append("buttonImageOnly: true ");
            sb.Append("}); ");
            sb.Append("}); ");
            sb.Append("</script> ");

            return sb.ToString();
        }
        #endregion
    }
}
