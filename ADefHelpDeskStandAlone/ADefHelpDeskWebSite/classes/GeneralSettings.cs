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
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace ADefWebserver.Modules.ADefHelpDesk
{
    public class GeneralSettings
    {
        string _Version;
        string _SMTPServer;
        string _SMTPAuthendication;
        bool _SMTPSecure;
        string _SMTPUserName;
        string _SMTPPassword;
        string _SMTPFromEmail;
        string _FileUploadPath;
        string _UploadPermission;
        bool _AllowRegistration;
        bool _VerifiedRegistration;

        #region Public Properties
        public string Version
        {
            get { return _Version; }
        }

        public string SMTPServer
        {
            get { return _SMTPServer; }
        }

        public string SMTPAuthendication
        {
            get { return _SMTPAuthendication; }
        }

        public bool SMTPSecure
        {
            get { return _SMTPSecure; }
        }

        public string SMTPUserName
        {
            get { return _SMTPUserName; }
        }

        public string SMTPPassword
        {
            get { return _SMTPPassword; }
        }

        public string SMTPFromEmail
        {
            get { return _SMTPFromEmail; }
        }

        public string FileUploadPath
        {
            get { return _FileUploadPath; }
        }

        public string UploadPermission
        {
            get { return _UploadPermission; }
        }

        public bool AllowRegistration
        {
            get { return _AllowRegistration; }
        }

        public bool VerifiedRegistration
        {
            get { return _VerifiedRegistration; }
        }
        #endregion

        public GeneralSettings()
        {
            dnnHelpDeskDALDataContext dnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();
            var resuts = from Settings in dnnHelpDeskDALDataContext.ADefHelpDesk_Settings
                         select Settings;

            _SMTPServer = Convert.ToString(resuts.FirstOrDefault(x => x.SettingName == "SMTPServer").SettingValue);
            _SMTPAuthendication = Convert.ToString(resuts.FirstOrDefault(x => x.SettingName == "SMTPAuthendication").SettingValue);
            _SMTPSecure = Convert.ToBoolean(resuts.FirstOrDefault(x => x.SettingName == "SMTPSecure").SettingValue);
            _SMTPUserName = Convert.ToString(resuts.FirstOrDefault(x => x.SettingName == "SMTPUserName").SettingValue);
            _SMTPPassword = Convert.ToString(resuts.FirstOrDefault(x => x.SettingName == "SMTPPassword").SettingValue);
            _SMTPFromEmail = Convert.ToString(resuts.FirstOrDefault(x => x.SettingName == "SMTPFromEmail").SettingValue);

            _FileUploadPath = Convert.ToString(resuts.FirstOrDefault(x => x.SettingName == "FileUploadPath").SettingValue);
            _UploadPermission = Convert.ToString(resuts.FirstOrDefault(x => x.SettingName == "UploadPermission").SettingValue);
           
            _AllowRegistration = Convert.ToBoolean(resuts.FirstOrDefault(x => x.SettingName == "AllowRegistration").SettingValue);
            _VerifiedRegistration = Convert.ToBoolean(resuts.FirstOrDefault(x => x.SettingName == "VerifiedRegistration").SettingValue);

            var versions = from Version in dnnHelpDeskDALDataContext.ADefHelpDesk_Versions
                           select Version;

            _Version = Convert.ToString(versions.FirstOrDefault().VersionNumber.ToString());
        }

        #region UpdateSMTPServer
        public static void UpdateSMTPServer(string SMTPServer)
        {
            dnnHelpDeskDALDataContext dnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();
            var resuts = from Settings in dnnHelpDeskDALDataContext.ADefHelpDesk_Settings
                         where Settings.SettingName == "SMTPServer"
                         select Settings;

            resuts.FirstOrDefault().SettingValue = Convert.ToString(SMTPServer);
            dnnHelpDeskDALDataContext.SubmitChanges();
        }
        #endregion

        #region UpdateSMTPAuthendication
        public static void UpdateSMTPAuthendication(string SMTPAuthendication)
        {
            dnnHelpDeskDALDataContext dnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();
            var resuts = from Settings in dnnHelpDeskDALDataContext.ADefHelpDesk_Settings
                         where Settings.SettingName == "SMTPAuthendication"
                         select Settings;

            resuts.FirstOrDefault().SettingValue = Convert.ToString(SMTPAuthendication);
            dnnHelpDeskDALDataContext.SubmitChanges();
        }
        #endregion

        #region UpdateSMTPSecure
        public static void UpdateSMTPSecure(bool SMTPSecure)
        {
            dnnHelpDeskDALDataContext dnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();
            var resuts = from Settings in dnnHelpDeskDALDataContext.ADefHelpDesk_Settings
                         where Settings.SettingName == "SMTPSecure"
                         select Settings;

            resuts.FirstOrDefault().SettingValue = Convert.ToString(SMTPSecure);
            dnnHelpDeskDALDataContext.SubmitChanges();
        }
        #endregion

        #region UpdateSMTPUserName
        public static void UpdateSMTPUserName(string SMTPUserName)
        {
            dnnHelpDeskDALDataContext dnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();
            var resuts = from Settings in dnnHelpDeskDALDataContext.ADefHelpDesk_Settings
                         where Settings.SettingName == "SMTPUserName"
                         select Settings;

            resuts.FirstOrDefault().SettingValue = Convert.ToString(SMTPUserName);
            dnnHelpDeskDALDataContext.SubmitChanges();
        }
        #endregion

        #region UpdateSMTPPassword
        public static void UpdateSMTPPassword(string SMTPPassword)
        {
            dnnHelpDeskDALDataContext dnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();
            var resuts = from Settings in dnnHelpDeskDALDataContext.ADefHelpDesk_Settings
                         where Settings.SettingName == "SMTPPassword"
                         select Settings;

            resuts.FirstOrDefault().SettingValue = Convert.ToString(SMTPPassword);
            dnnHelpDeskDALDataContext.SubmitChanges();
        }
        #endregion

        #region UpdateSMTPFromEmail
        public static void UpdateSMTPFromEmail(string SMTPFromEmail)
        {
            dnnHelpDeskDALDataContext dnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();
            var resuts = from Settings in dnnHelpDeskDALDataContext.ADefHelpDesk_Settings
                         where Settings.SettingName == "SMTPFromEmail"
                         select Settings;

            resuts.FirstOrDefault().SettingValue = Convert.ToString(SMTPFromEmail);
            dnnHelpDeskDALDataContext.SubmitChanges();
        }
        #endregion

        #region UpdateFileUploadPath
        public static void UpdateFileUploadPath(string FileUploadPath)
        {
            dnnHelpDeskDALDataContext dnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();
            var resuts = from Settings in dnnHelpDeskDALDataContext.ADefHelpDesk_Settings
                         where Settings.SettingName == "FileUploadPath"
                         select Settings;

            resuts.FirstOrDefault().SettingValue = Convert.ToString(FileUploadPath);
            dnnHelpDeskDALDataContext.SubmitChanges();
        }
        #endregion

        #region UpdateUploadPermission
        public static void UpdateUploadPermission(string UploadPermission)
        {
            dnnHelpDeskDALDataContext dnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();
            var resuts = from Settings in dnnHelpDeskDALDataContext.ADefHelpDesk_Settings
                         where Settings.SettingName == "UploadPermission"
                         select Settings;

            resuts.FirstOrDefault().SettingValue = Convert.ToString(UploadPermission);
            dnnHelpDeskDALDataContext.SubmitChanges();
        }
        #endregion

        #region UpdateAllowRegistration
        public static void UpdateAllowRegistration(bool AllowRegistration)
        {
            dnnHelpDeskDALDataContext dnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();
            var resuts = from Settings in dnnHelpDeskDALDataContext.ADefHelpDesk_Settings
                         where Settings.SettingName == "AllowRegistration"
                         select Settings;

            resuts.FirstOrDefault().SettingValue = Convert.ToString(AllowRegistration);
            dnnHelpDeskDALDataContext.SubmitChanges();
        }
        #endregion

        #region UpdateVerifiedRegistration
        public static void UpdateVerifiedRegistration(bool VerifiedRegistration)
        {
            dnnHelpDeskDALDataContext dnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();
            var resuts = from Settings in dnnHelpDeskDALDataContext.ADefHelpDesk_Settings
                         where Settings.SettingName == "VerifiedRegistration"
                         select Settings;

            resuts.FirstOrDefault().SettingValue = Convert.ToString(VerifiedRegistration);
            dnnHelpDeskDALDataContext.SubmitChanges();
        }
        #endregion
    }
}
