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
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Web.Configuration;
using System.Web.Security;

namespace ADefWebserver.Modules.ADefHelpDesk
{
    public partial class SetupWizard : System.Web.UI.Page
    {
        string strConnection;
        string strCurrentVersion = "01.00.00";
        string[] arrSQLScriptsToRun = new string[] 
    { 
        "01.00.00.sql"
    };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // If the site is already set-up do not allow the Install Wizard to run
                if (DatabaseReady())
                {
                    Response.Redirect("Default.aspx");
                    Response.End();
                }
            }
        }

        #region Navigation
        protected void btnWelcomeNext_Click(object sender, EventArgs e)
        {
            blPermissions.Items.Clear();

            if (!IsUpgrade())
            {
                mvSetupWizard.SetActiveView(vwPermissions);
            }
            else
            {
                // This is an upgrade so run .sql scripts
                mvSetupWizard.SetActiveView(vwDatabaseSetup);
                SetupDatabase();
            }
        }

        protected void btnPermissionsPrevious_Click(object sender, EventArgs e)
        {
            mvSetupWizard.SetActiveView(vwWelcome);
        }

        protected void btnPermissionsNext_Click(object sender, EventArgs e)
        {
            mvSetupWizard.SetActiveView(vwDatabase);
        }

        protected void btnDatabasePrevious_Click(object sender, EventArgs e)
        {
            blPermissions.Items.Clear();
            mvSetupWizard.SetActiveView(vwPermissions);
        }

        protected void btnDatabaseNext_Click(object sender, EventArgs e)
        {
            mvSetupWizard.SetActiveView(vwDatabaseSetup);
            SetupDatabase();
        }

        protected void btnCompletePrevious_Click(object sender, EventArgs e)
        {
            mvSetupWizard.SetActiveView(vwDatabase);
        }

        protected void btnAdministratorPrevious_Click(object sender, EventArgs e)
        {
            mvSetupWizard.SetActiveView(vwDatabaseSetup);
            SetupDatabase();
        }
        #endregion

        #region PermissionCheck
        protected void btnCheckPermissions_Click(object sender, EventArgs e)
        {
            int intValidItems = 0;
            blPermissions.Items.Clear();

            //FolderCreate
            ListItem permissionItem = new ListItem();
            permissionItem.Text = String.Format("Folder Create - {0}", ((VerifyFolderCreate()) ? "Passed" : "Failed"));
            permissionItem.Enabled = ((permissionItem.Text.Contains("Passed")) ? true : false);
            blPermissions.Items.Add(permissionItem);
            intValidItems = intValidItems + ((permissionItem.Text.Contains("Passed")) ? 1 : 0);

            //FileCreate
            permissionItem = new ListItem();
            permissionItem.Text = String.Format("File Create - {0}", ((VerifyFolderCreate()) ? "Passed" : "Failed"));
            permissionItem.Enabled = ((permissionItem.Text.Contains("Passed")) ? true : false);
            blPermissions.Items.Add(permissionItem);
            intValidItems = intValidItems + ((permissionItem.Text.Contains("Passed")) ? 1 : 0);

            //FileDelete
            permissionItem = new ListItem();
            permissionItem.Text = String.Format("File Delete - {0}", ((VerifyFolderCreate()) ? "Passed" : "Failed"));
            permissionItem.Enabled = ((permissionItem.Text.Contains("Passed")) ? true : false);
            blPermissions.Items.Add(permissionItem);
            intValidItems = intValidItems + ((permissionItem.Text.Contains("Passed")) ? 1 : 0);

            //FolderDelete
            permissionItem = new ListItem();
            permissionItem.Text = String.Format("Folder Delete - {0}", ((VerifyFolderCreate()) ? "Passed" : "Failed"));
            permissionItem.Enabled = ((permissionItem.Text.Contains("Passed")) ? true : false);
            blPermissions.Items.Add(permissionItem);
            intValidItems = intValidItems + ((permissionItem.Text.Contains("Passed")) ? 1 : 0);

            lblPermissionCheck.Text = String.Format("Permissions {0}", ((intValidItems == 4) ? "Passed" : "Failed"));
            lblPermissionCheck.BackColor = ((intValidItems == 4) ? Color.Green : Color.Red);

            // Show the Continue button?
            btnPermissionsNext.Visible = ((intValidItems == 4) ? true : false);
        }

        #region VerifyFolderCreate
        private bool VerifyFolderCreate()
        {
            string verifyPath = Server.MapPath("~/Verify");
            bool verified = true;

            //Attempt to create the Directory
            try
            {
                if (Directory.Exists(verifyPath))
                {
                    Directory.Delete(verifyPath, true);
                }

                Directory.CreateDirectory(verifyPath);
            }
            catch (Exception ex)
            {
                string strTemp = ex.Message;
                verified = false;
            }

            return verified;
        }
        #endregion

        #region VerifyFolderDelete
        private bool VerifyFolderDelete()
        {
            string verifyPath = Server.MapPath("~/Verify");
            bool verified = VerifyFolderCreate();

            if (verified)
            {
                //Attempt to delete the Directory
                try
                {
                    Directory.Delete(verifyPath);
                }
                catch (Exception ex)
                {
                    string strTemp = ex.Message;
                    verified = false;
                }
            }

            return verified;
        }
        #endregion

        #region VerifyFileCreate
        private bool VerifyFileCreate()
        {
            string verifyPath = Server.MapPath("~/Verify/Verify.txt");
            bool verified = VerifyFolderCreate();

            if (verified)
            {
                //Attempt to create the File
                try
                {
                    if (File.Exists(verifyPath))
                    {
                        File.Delete(verifyPath);
                    }

                    Stream fileStream = File.Create(verifyPath);
                    fileStream.Close();
                }

                catch (Exception ex)
                {
                    string strTemp = ex.Message;
                    verified = false;
                }
            }

            return verified;
        }
        #endregion

        #region VerifyFileDelete
        private bool VerifyFileDelete()
        {
            string verifyPath = Server.MapPath("~/Verify/Verify.txt");
            bool verified = VerifyFileCreate();

            if (verified)
            {
                //Attempt to delete the File
                try
                {
                    File.Delete(verifyPath);
                }
                catch (Exception ex)
                {
                    string strTemp = ex.Message;
                    verified = false;
                }
            }

            return verified;
        }
        #endregion

        #endregion

        #region Database Configuration
        protected void chkIntegrated_CheckedChanged(object sender, EventArgs e)
        {
            trUser.Visible = (!chkIntegrated.Checked);
            trPassword.Visible = (!chkIntegrated.Checked);
        }

        protected void btnDatabaseCheck_Click(object sender, EventArgs e)
        {
            bool boolCanConnectToDatabase = CanConnectToDatabase();

            btnDatabaseNext.Visible = boolCanConnectToDatabase;
            this.lblConnectionResults.Text = (boolCanConnectToDatabase) ? "Connection Successful" : "Connection Error";

            // If connection was successful then write the connection string to the web.config to be used on the next wizard step
            if (boolCanConnectToDatabase)
            {
                System.Configuration.Configuration rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
                System.Configuration.ConnectionStringSettings connString;
                connString = rootWebConfig.ConnectionStrings.ConnectionStrings["SiteSqlServer"];
                connString.ConnectionString = strConnection;
                rootWebConfig.Save();
            }
        }

        private bool CanConnectToDatabase()
        {
            string strUserInfo = (!chkIntegrated.Checked) ? String.Format("Persist Security Info=True;User ID={0};Password={1}", this.txtUserId.Text, this.txtPassword.Text) : "Integrated Security=True";
            strConnection = String.Format("Data Source={0};Initial Catalog={1};{2}", this.txtServer.Text, this.txtDatabase.Text, strUserInfo);

            dnnHelpDeskDALDataContext dnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext(strConnection);

            return dnnHelpDeskDALDataContext.DatabaseExists();
        }
        #endregion

        #region Database Set-up
        private void SetupDatabase()
        {
            lblDatabaseSetup.Text = (DatabaseReady()) ? "The Database is already set-up. Click continue." : "Click the Continue button to run the database set-up scripts.";
        }

        private bool DatabaseReady()
        {
            // This method returns true if the databse exists and the table is created
            bool CanConnect = true;

            // Determine if the SiteSqlServer key exists. If it does not create it
            ConnectionStringSettings StringSettings = null;
            Configuration WebConfig = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            StringSettings = WebConfig.ConnectionStrings.ConnectionStrings["SiteSqlServer"];
            if (StringSettings == null)
            {
                // Add the SiteSqlServer key
                StringSettings = new ConnectionStringSettings();
                StringSettings.Name = "SiteSqlServer";
                StringSettings.ConnectionString = "Data Source=(local);Initial Catalog=SilverlightDesktop;Integrated Security=True";
                StringSettings.ProviderName = "System.Data.SqlClient";
                WebConfig.ConnectionStrings.ConnectionStrings.Add(StringSettings);
                WebConfig.Save();
            }

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

        private bool IsUpgrade()
        {
            // This method returns true if the database exists and the table is created
            bool _IsUpgrade = true;
            int intCurrentVersion = Convert.ToInt32(strCurrentVersion.Replace(".", ""));
            try
            {
                dnnHelpDeskDALDataContext dnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();
                var result = from VersionNumber in dnnHelpDeskDALDataContext.ADefHelpDesk_Versions
                             select VersionNumber;
                _IsUpgrade = (Convert.ToInt32(result.FirstOrDefault().VersionNumber.Replace(".", "")) < intCurrentVersion) ? true : false;
            }
            catch (Exception e)
            {
                string strError = e.Message;
                _IsUpgrade = false;
            }

            return _IsUpgrade;
        }

        protected void btnDatabaseSetupNext_Click(object sender, EventArgs e)
        {
            bool InUpgradeMode = IsUpgrade();

            if (!DatabaseReady())
            {
                try
                {
                    // Process all the SQL Scripts   
                    foreach (string strSQLScript in arrSQLScriptsToRun)
                    {
                        string strSqlScript = GetSQLScript(strSQLScript);
                        dnnHelpDeskDALDataContext dnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();
                        dnnHelpDeskDALDataContext.ExecuteCommand(strSqlScript);
                    }
                }
                catch (Exception ex)
                {
                    lblDatabaseSetup.Text = ex.Message;
                    return;
                }
            }

            if (!InUpgradeMode)
            {
                mvSetupWizard.SetActiveView(vwAdminAccount);
            }
            else
            {
                // This is an upgrade so go to end
                mvSetupWizard.SetActiveView(vwComplete);
            }

        }

        private String GetSQLScript(string strSQLScript)
        {
            string strSQL;
            string strFilePath = MapPath(String.Format(@"~/SQLScripts/{0}", strSQLScript));
            StreamReader reader = new StreamReader(strFilePath);
            strSQL = reader.ReadToEnd();
            reader.Close();
            reader = null;
            return strSQL;
        }
        #endregion

        #region Create Admin Account
        protected void btnCreateAdmin_Click(object sender, EventArgs e)
        {
            // Attempt to create the Administrator account
            if (txtUserName.Text.Trim().ToLower() == "" | txtUserPassword.Text.Trim() == "" | txtUserEmail.Text.Trim() == "")
            {
                lblUserAccountResults.Text = "User name, password, and email must be entered to continue";
            }
            else
            {
                // Delete any SuperUser account
                dnnHelpDeskDALDataContext dnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();
                var result = from Superusers in dnnHelpDeskDALDataContext.ADefHelpDesk_Users
                             where Superusers.IsSuperUser == true
                             select Superusers;

                if (result.Count() > 0)
                {
                    dnnHelpDeskDALDataContext.ADefHelpDesk_Users.DeleteOnSubmit(result.FirstOrDefault());
                    dnnHelpDeskDALDataContext.SubmitChanges();
                }

                // Create SuperUser account
                ADefHelpDesk_User User = new ADefHelpDesk_User();

                User.Email = txtUserEmail.Text.Trim();
                User.Username = txtUserName.Text.Trim();
                User.FirstName = "Super";
                User.LastName = "User";
                User.Password = Utility.HashPassword(txtUserName.Text.Trim() + txtUserPassword.Text.Trim());
                User.IsSuperUser = true;                

                dnnHelpDeskDALDataContext.ADefHelpDesk_Users.InsertOnSubmit(User);
                dnnHelpDeskDALDataContext.SubmitChanges();

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

                mvSetupWizard.SetActiveView(vwComplete);
            }
        }
        #endregion

        #region Complete
        protected void btnCompleteNext_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
        #endregion
    }
}