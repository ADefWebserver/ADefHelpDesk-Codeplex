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
// Silk icon set 1.3 by
// Mark James
// http://www.famfamfam.com/lab/icons/silk/
// Creative Commons Attribution 2.5 License.
// [ http://creativecommons.org/licenses/by/2.5/ ]

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

namespace ADefWebserver.Modules.ADefHelpDesk
{
    #region ExistingTasks
    [Serializable]
    public class ExistingTasks
    {
        public int TaskID { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Assigned { get; set; }
        public string Description { get; set; }
        public string Requester { get; set; }
        public string RequesterName { get; set; }
        public string Search { get; set; }
        public int?[] Categories { get; set; }
    }
    #endregion

    #region AssignedRoles
    public class AssignedRoles
    {
        public string AssignedRoleID { get; set; }
        public string Key { get; set; }
    }
    #endregion

    public partial class View : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        #region SortExpression
        public string SortExpression
        {
            get
            {
                if (ViewState["SortExpression"] == null)
                {
                    ViewState["SortExpression"] = string.Empty;
                }

                return Convert.ToString(ViewState["SortExpression"]);
            }
            set
            {
                ViewState["SortExpression"] = value;
            }
        }
        #endregion

        #region SortDirection
        public string SortDirection
        {
            get
            {
                if (ViewState["SortDirection"] == null)
                {
                    ViewState["SortDirection"] = string.Empty;
                }

                return Convert.ToString(ViewState["SortDirection"]);
            }
            set
            {
                ViewState["SortDirection"] = value;
            }
        }
        #endregion

        #region SearchCriteria
        public ADefHelpDesk_LastSearch SearchCriteria
        {
            get
            {
                return GetLastSearchCriteria();
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                cmdStartCalendar.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtDueDate);
                if (!Page.IsPostBack)
                {
                    ShowAdministratorLinkAndFileUpload();
                    ShowExistingTicketsLink();
                    txtUserID.Text = UserId.ToString();
                    DisplayCategoryTree();

                    if (Request.QueryString["Ticket"] != null)
                    {
                        if (Convert.ToString(Request.QueryString["Ticket"]) == "new")
                        {
                            SetView("New Ticket");
                            ShowAdministratorLinkAndFileUpload();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        #region GetLastSearchCriteria
        private ADefHelpDesk_LastSearch GetLastSearchCriteria()
        {
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            ADefHelpDesk_LastSearch objADefHelpDesk_LastSearch = (from ADefHelpDesk_LastSearches in objADefHelpDeskDALDataContext.ADefHelpDesk_LastSearches
                                                                  where ADefHelpDesk_LastSearches.PortalID == PortalId
                                                                  where ADefHelpDesk_LastSearches.UserID == UserId
                                                                  select ADefHelpDesk_LastSearches).FirstOrDefault();

            if (objADefHelpDesk_LastSearch == null)
            {
                ADefHelpDesk_LastSearch InsertADefHelpDesk_LastSearch = new ADefHelpDesk_LastSearch();
                InsertADefHelpDesk_LastSearch.UserID = UserId;
                InsertADefHelpDesk_LastSearch.PortalID = PortalId;
                objADefHelpDeskDALDataContext.ADefHelpDesk_LastSearches.InsertOnSubmit(InsertADefHelpDesk_LastSearch);

                // Only save is user is logged in
                if (UserId > -1)
                {
                    objADefHelpDeskDALDataContext.SubmitChanges();
                }

                return InsertADefHelpDesk_LastSearch;
            }
            else
            {
                return objADefHelpDesk_LastSearch;
            }
        }
        #endregion

        #region SaveLastSearchCriteria
        private void SaveLastSearchCriteria(ADefHelpDesk_LastSearch UpdateADefHelpDesk_LastSearch)
        {
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            ADefHelpDesk_LastSearch objADefHelpDesk_LastSearch = (from ADefHelpDesk_LastSearches in objADefHelpDeskDALDataContext.ADefHelpDesk_LastSearches
                                                                  where ADefHelpDesk_LastSearches.PortalID == PortalId
                                                                  where ADefHelpDesk_LastSearches.UserID == UserId
                                                                  select ADefHelpDesk_LastSearches).FirstOrDefault();

            if (objADefHelpDesk_LastSearch == null)
            {
                objADefHelpDesk_LastSearch = new ADefHelpDesk_LastSearch();
                objADefHelpDesk_LastSearch.UserID = UserId;
                objADefHelpDesk_LastSearch.PortalID = PortalId;
                objADefHelpDeskDALDataContext.ADefHelpDesk_LastSearches.InsertOnSubmit(objADefHelpDesk_LastSearch);
                objADefHelpDeskDALDataContext.SubmitChanges();
            }

            objADefHelpDesk_LastSearch.AssignedRoleID = UpdateADefHelpDesk_LastSearch.AssignedRoleID;
            objADefHelpDesk_LastSearch.Categories = UpdateADefHelpDesk_LastSearch.Categories;
            objADefHelpDesk_LastSearch.CreatedDate = UpdateADefHelpDesk_LastSearch.CreatedDate;
            objADefHelpDesk_LastSearch.SearchText = UpdateADefHelpDesk_LastSearch.SearchText;
            objADefHelpDesk_LastSearch.DueDate = UpdateADefHelpDesk_LastSearch.DueDate;
            objADefHelpDesk_LastSearch.Priority = UpdateADefHelpDesk_LastSearch.Priority;
            objADefHelpDesk_LastSearch.Status = UpdateADefHelpDesk_LastSearch.Status;
            objADefHelpDesk_LastSearch.CurrentPage = UpdateADefHelpDesk_LastSearch.CurrentPage;

            objADefHelpDeskDALDataContext.SubmitChanges();
        }
        #endregion

        #region ShowExistingTicketsLink
        private void ShowExistingTicketsLink()
        {
            // Show Existing Tickets link if user is logged in
            if (UserId > -1)
            {
                lnkExistingTickets.Visible = true;
                imgExitingTickets.Visible = true;
                SetView("Existing Tickets");
            }
            else
            {
                lnkExistingTickets.Visible = false;
                imgExitingTickets.Visible = false;
                SetView("New Ticket");
            }
        }
        #endregion

        #region ShowAdministratorLinkAndFileUpload
        private void ShowAdministratorLinkAndFileUpload()
        {
            // Get Admin Role
            string strAdminRoleID = GetAdminRole();
            string strUploadPermission = GetUploadPermission();
            // Show Admin link if user is an Administrator
            if (UserInfo.IsInRole(strAdminRoleID) || UserInfo.IsInRole("Administrators") || UserInfo.IsSuperUser)
            {
                lnkAdministratorSettings.Visible = true;
                imgAdministrator.Visible = true;

                // Show the Administrator user selector and Ticket Status selectors
                pnlAdminUserSelection.Visible = true;
                pnlAdminUserSelection.GroupingText = Localization.GetString("AdminUserSelectionGrouping.Text", LocalResourceFile);
                pnlAdminTicketStatus.Visible = true;

                // Load the Roles dropdown
                LoadRolesDropDown();

                // Display default Admin view
                DisplayAdminView();
            }
            else
            {
                // ** Non Administrators **
                lnkAdministratorSettings.Visible = false;
                imgAdministrator.Visible = false;

                // Do not show the Administrator user selector
                pnlAdminUserSelection.Visible = false;

                // Only supress Upload if permission is not set to All
                #region if (strUploadPermission != "All")
                if (strUploadPermission != "All")
                {
                    // Is user Logged in?
                    if (UserId > -1)
                    {
                        #region if (strUploadPermission != "Administrator/Registered Users")
                        // Only check this if security is set to "Administrator/Registered Users"
                        if (strUploadPermission != "Administrator/Registered Users")
                        {
                            // If User is not an Administrator so they cannot see upload
                            lblAttachFile.Visible = false;
                            TicketFileUpload.Visible = false;
                        }
                        #endregion
                    }
                    else
                    {
                        // If User is not logged in they cannot see upload
                        lblAttachFile.Visible = false;
                        TicketFileUpload.Visible = false;
                    }
                }
                #endregion
            }
        }
        #endregion

        #region LoadRolesDropDown
        private void LoadRolesDropDown()
        {
            ddlAssignedAdmin.Items.Clear();

            RoleController objRoleController = new RoleController();
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            List<ADefHelpDesk_Role> colADefHelpDesk_Roles = (from ADefHelpDesk_Roles in objADefHelpDeskDALDataContext.ADefHelpDesk_Roles
                                                             where ADefHelpDesk_Roles.PortalID == PortalId
                                                             select ADefHelpDesk_Roles).ToList();

            // Create a ListItemCollection to hold the Roles 
            ListItemCollection colListItemCollection = new ListItemCollection();

            // Add the Roles to the List
            foreach (ADefHelpDesk_Role objADefHelpDesk_Role in colADefHelpDesk_Roles)
            {
                try
                {
                    RoleInfo objRoleInfo = objRoleController.GetRole(Convert.ToInt32(objADefHelpDesk_Role.RoleID), PortalId);

                    ListItem RoleListItem = new ListItem();
                    RoleListItem.Text = objRoleInfo.RoleName;
                    RoleListItem.Value = objADefHelpDesk_Role.RoleID.ToString();
                    ddlAssignedAdmin.Items.Add(RoleListItem);
                }
                catch
                {
                    // Role no longer exists in Portal
                    ListItem RoleListItem = new ListItem();
                    RoleListItem.Text = Localization.GetString("DeletedRole.Text", LocalResourceFile);
                    RoleListItem.Value = objADefHelpDesk_Role.RoleID.ToString();
                    ddlAssignedAdmin.Items.Add(RoleListItem);
                }
            }

            // Add UnAssigned
            ListItem UnassignedRoleListItem = new ListItem();
            UnassignedRoleListItem.Text = Localization.GetString("Unassigned.Text", LocalResourceFile);
            UnassignedRoleListItem.Value = "-1";
            ddlAssignedAdmin.Items.Add(UnassignedRoleListItem);
        }
        #endregion

        #region DisplayCategoryTree
        private void DisplayCategoryTree()
        {
            if (UserInfo.IsInRole(GetAdminRole()) || UserInfo.IsInRole("Administrators") || UserInfo.IsSuperUser)
            {
                TagsTree.Visible = true;
                TagsTree.TagID = -1;
                TagsTree.DisplayType = "Administrator";
                TagsTree.Expand = false;

                TagsTreeExistingTasks.Visible = true;
                TagsTreeExistingTasks.TagID = -1;
                TagsTreeExistingTasks.DisplayType = "Administrator";
                TagsTreeExistingTasks.Expand = false;
            }
            else
            {
                TagsTree.Visible = true;
                TagsTree.TagID = -1;
                TagsTree.DisplayType = "Requestor";
                TagsTree.Expand = false;

                TagsTreeExistingTasks.Visible = true;
                TagsTreeExistingTasks.TagID = -1;
                TagsTreeExistingTasks.DisplayType = "Requestor";
                TagsTreeExistingTasks.Expand = false;
            }

            // Only Logged in users can have saved Categiries in the Tag tree
            if ((UserId > -1) & (SearchCriteria.Categories != null))
            {
                if (SearchCriteria.Categories.Trim() != "")
                {
                    char[] delimiterChars = { ',' };
                    string[] ArrStrCategories = SearchCriteria.Categories.Split(delimiterChars);
                    // Convert the Categories selected from the Tags tree to an array of integers
                    int?[] ArrIntCatagories = Array.ConvertAll<string, int?>(ArrStrCategories, new Converter<string, int?>(ConvertStringToNullableInt));

                    TagsTreeExistingTasks.SelectedCategories = ArrIntCatagories;
                }
            }
        }
        #endregion

        #region GetAdminRole
        private string GetAdminRole()
        {
            List<ADefHelpDesk_Setting> objADefHelpDesk_Settings = GetSettings();
            ADefHelpDesk_Setting objADefHelpDesk_Setting = objADefHelpDesk_Settings.Where(x => x.SettingName == "AdminRole").FirstOrDefault();

            string strAdminRoleID = "Administrators";
            if (objADefHelpDesk_Setting != null)
            {
                strAdminRoleID = objADefHelpDesk_Setting.SettingValue;
            }

            return strAdminRoleID;
        }
        #endregion

        #region GetUploadPermission
        private string GetUploadPermission()
        {
            List<ADefHelpDesk_Setting> objADefHelpDesk_Settings = GetSettings();
            ADefHelpDesk_Setting objADefHelpDesk_Setting = objADefHelpDesk_Settings.Where(x => x.SettingName == "UploadPermission").FirstOrDefault();

            string strUploadPermission = "All";
            if (objADefHelpDesk_Setting != null)
            {
                strUploadPermission = objADefHelpDesk_Setting.SettingValue;
            }

            return strUploadPermission;
        }
        #endregion

        #region lnkAdministratorSettings_Click
        protected void lnkAdministratorSettings_Click(object sender, EventArgs e)
        {
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "AdminSettings", "mid=" + ModuleId.ToString()));
        }
        #endregion

        #region GetSettings
        private List<ADefHelpDesk_Setting> GetSettings()
        {
            // Get Settings
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            List<ADefHelpDesk_Setting> colADefHelpDesk_Setting = (from ADefHelpDesk_Settings in objADefHelpDeskDALDataContext.ADefHelpDesk_Settings
                                                                  where ADefHelpDesk_Settings.PortalID == PortalId
                                                                  select ADefHelpDesk_Settings).ToList();

            if (colADefHelpDesk_Setting.Count == 0)
            {
                // Create Default vaules
                ADefHelpDesk_Setting objADefHelpDesk_Setting1 = new ADefHelpDesk_Setting();

                objADefHelpDesk_Setting1.PortalID = PortalId;
                objADefHelpDesk_Setting1.SettingName = "AdminRole";
                objADefHelpDesk_Setting1.SettingValue = "Administrators";

                objADefHelpDeskDALDataContext.ADefHelpDesk_Settings.InsertOnSubmit(objADefHelpDesk_Setting1);
                objADefHelpDeskDALDataContext.SubmitChanges();

                ADefHelpDesk_Setting objADefHelpDesk_Setting2 = new ADefHelpDesk_Setting();

                objADefHelpDesk_Setting2.PortalID = PortalId;
                objADefHelpDesk_Setting2.SettingName = "UploadefFilesPath";
                objADefHelpDesk_Setting2.SettingValue = Server.MapPath("~/DesktopModules/ADefHelpDesk/Upload");

                objADefHelpDeskDALDataContext.ADefHelpDesk_Settings.InsertOnSubmit(objADefHelpDesk_Setting2);
                objADefHelpDeskDALDataContext.SubmitChanges();

                colADefHelpDesk_Setting = (from ADefHelpDesk_Settings in objADefHelpDeskDALDataContext.ADefHelpDesk_Settings
                                           where ADefHelpDesk_Settings.PortalID == PortalId
                                           select ADefHelpDesk_Settings).ToList();
            }

            // Upload Permission
            ADefHelpDesk_Setting UploadPermissionADefHelpDesk_Setting = (from ADefHelpDesk_Settings in objADefHelpDeskDALDataContext.ADefHelpDesk_Settings
                                                                         where ADefHelpDesk_Settings.PortalID == PortalId
                                                                         where ADefHelpDesk_Settings.SettingName == "UploadPermission"
                                                                         select ADefHelpDesk_Settings).FirstOrDefault();

            if (UploadPermissionADefHelpDesk_Setting != null)
            {
                // Add to collection
                colADefHelpDesk_Setting.Add(UploadPermissionADefHelpDesk_Setting);
            }
            else
            {
                // Add Default value
                ADefHelpDesk_Setting objADefHelpDesk_Setting = new ADefHelpDesk_Setting();
                objADefHelpDesk_Setting.SettingName = "UploadPermission";
                objADefHelpDesk_Setting.SettingValue = "All";
                objADefHelpDesk_Setting.PortalID = PortalId;
                objADefHelpDeskDALDataContext.ADefHelpDesk_Settings.InsertOnSubmit(objADefHelpDesk_Setting);
                objADefHelpDeskDALDataContext.SubmitChanges();

                // Add to collection
                colADefHelpDesk_Setting.Add(objADefHelpDesk_Setting);
            }

            return colADefHelpDesk_Setting;
        }
        #endregion

        #region lnkNewTicket_Click
        protected void lnkNewTicket_Click(object sender, EventArgs e)
        {
            // Clear the form
            txtName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtDescription.Text = "";
            txtDetails.Text = "";
            txtDueDate.Text = "";
            ddlPriority.SelectedValue = "Normal";
            txtUserID.Text = UserId.ToString();

            SetView("New Ticket");
            ShowAdministratorLinkAndFileUpload();
        }
        #endregion

        #region lnkExistingTickets_Click
        protected void lnkExistingTickets_Click(object sender, EventArgs e)
        {
            SetView("Existing Tickets");
        }
        #endregion

        #region lnkResetSearch_Click
        protected void lnkResetSearch_Click(object sender, EventArgs e)
        {
            ADefHelpDesk_LastSearch ExistingADefHelpDesk_LastSearch = GetLastSearchCriteria();
            ExistingADefHelpDesk_LastSearch.AssignedRoleID = null;
            ExistingADefHelpDesk_LastSearch.Categories = null;
            ExistingADefHelpDesk_LastSearch.CreatedDate = null;
            ExistingADefHelpDesk_LastSearch.SearchText = null;
            ExistingADefHelpDesk_LastSearch.DueDate = null;
            ExistingADefHelpDesk_LastSearch.Priority = null;
            ExistingADefHelpDesk_LastSearch.Status = null;
            ExistingADefHelpDesk_LastSearch.CurrentPage = 1;
            SaveLastSearchCriteria(ExistingADefHelpDesk_LastSearch);

            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(), true);
        }
        #endregion

        #region lnlAnonymousContinue_Click
        protected void lnlAnonymousContinue_Click(object sender, EventArgs e)
        {
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(), true);
        }
        #endregion

        #region SetView
        private void SetView(string ViewName)
        {
            if (ViewName == "New Ticket")
            {
                pnlNewTicket.Visible = true;
                pnlExistingTickets.Visible = false;
                pnlConfirmAnonymousUserEntry.Visible = false;
                imgMagnifier.Visible = false;
                lnkResetSearch.Visible = false;

                lnkNewTicket.Font.Bold = true;
                lnkNewTicket.BackColor = System.Drawing.Color.LightGray;
                lnkExistingTickets.Font.Bold = false;
                lnkExistingTickets.BackColor = System.Drawing.Color.Transparent;

                DisplayNewTicketForm();
            }

            if (ViewName == "Existing Tickets")
            {
                pnlNewTicket.Visible = false;
                pnlExistingTickets.Visible = true;
                pnlConfirmAnonymousUserEntry.Visible = false;
                imgMagnifier.Visible = true;
                lnkResetSearch.Visible = true;

                lnkNewTicket.Font.Bold = false;
                lnkNewTicket.BackColor = System.Drawing.Color.Transparent;
                lnkExistingTickets.Font.Bold = true;
                lnkExistingTickets.BackColor = System.Drawing.Color.LightGray;

                DisplayExistingTickets(SearchCriteria);
            }
        }
        #endregion

        #region DisplayNewTicketForm
        private void DisplayNewTicketForm()
        {
            // Logged in User
            if (UserId > -1)
            {
                txtName.Visible = false;
                txtEmail.Visible = false;
                lblName.Visible = true;
                lblEmail.Visible = true;

                // Load values for user
                lblName.Text = UserInfo.DisplayName;
                lblEmail.Text = UserInfo.Email;
            }
            else
            {
                // Not logged in
                txtName.Visible = true;
                txtEmail.Visible = true;
                lblName.Visible = false;
                lblEmail.Visible = false;
            }

        }
        #endregion

        #region DisplayAdminView
        private void DisplayAdminView()
        {
            btnClearUser.Visible = false;
            txtName.Visible = true;
            txtEmail.Visible = true;
            lblName.Visible = false;
            lblEmail.Visible = false;

            // Admin forms is set for anonymous user be default
            txtUserID.Text = "-1";
        }
        #endregion

        // Submit New Ticket

        #region btnSubmit_Click
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int intTaskID = 0;

            try
            {
                if (ValidateNewTicketForm())
                {
                    intTaskID = SaveNewTicketForm();
                    SaveTags(intTaskID);

                    // Display post save view

                    if (txtUserID.Text == "-1")
                    {
                        // User not logged in
                        // Say "Request Submitted"
                        pnlConfirmAnonymousUserEntry.Visible = true;
                        pnlExistingTickets.Visible = false;
                        pnlNewTicket.Visible = false;
                        lnkNewTicket.Font.Bold = false;
                        lnkNewTicket.BackColor = System.Drawing.Color.Transparent;
                        lblConfirmAnonymousUser.Text = String.Format(Localization.GetString("YourTicketNumber.Text", LocalResourceFile), intTaskID.ToString());
                    }
                    else
                    {
                        // User logged in
                        SetView("Existing Tickets");
                    }

                    SendEmail(intTaskID.ToString());
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
        #endregion

        #region ValidateNewTicketForm
        private bool ValidateNewTicketForm()
        {
            List<string> ColErrors = new List<string>();

            // Only validate Name qand emial if user is not logged in
            if (txtUserID.Text == "-1")
            {
                if (txtName.Text.Trim().Length < 1)
                {
                    ColErrors.Add(Localization.GetString("NameIsRequired.Text", LocalResourceFile));
                }

                if (txtEmail.Text.Trim().Length < 1)
                {
                    ColErrors.Add(Localization.GetString("EmailIsRequired.Text", LocalResourceFile));
                }
            }

            if (txtDescription.Text.Trim().Length < 1)
            {
                ColErrors.Add(Localization.GetString("DescriptionIsRequired.Text", LocalResourceFile));
            }

            // Validate the date only if a date was entered
            if (txtDueDate.Text.Trim().Length > 1)
            {
                try
                {
                    DateTime tmpDate = Convert.ToDateTime(txtDueDate.Text.Trim());
                }
                catch
                {
                    ColErrors.Add(Localization.GetString("MustUseAValidDate.Text", LocalResourceFile));
                }
            }

            // Validate file upload
            if (TicketFileUpload.HasFile)
            {
                if (
                    string.Compare(Path.GetExtension(TicketFileUpload.FileName).ToLower(), ".gif", true) != 0
                    & string.Compare(Path.GetExtension(TicketFileUpload.FileName).ToLower(), ".jpg", true) != 0
                    & string.Compare(Path.GetExtension(TicketFileUpload.FileName).ToLower(), ".jpeg", true) != 0
                    & string.Compare(Path.GetExtension(TicketFileUpload.FileName).ToLower(), ".doc", true) != 0
                    & string.Compare(Path.GetExtension(TicketFileUpload.FileName).ToLower(), ".docx", true) != 0
                    & string.Compare(Path.GetExtension(TicketFileUpload.FileName).ToLower(), ".xls", true) != 0
                    & string.Compare(Path.GetExtension(TicketFileUpload.FileName).ToLower(), ".xlsx", true) != 0
                    & string.Compare(Path.GetExtension(TicketFileUpload.FileName).ToLower(), ".pdf", true) != 0
                    )
                {
                    ColErrors.Add("Only .gif, .jpg, .jpeg, .doc, .docx, .xls, .xlsx, .pdf files may be used.");
                }
            }

            // Display Validation Errors
            if (ColErrors.Count > 0)
            {
                foreach (string objError in ColErrors)
                {
                    lblError.Text = lblError.Text + String.Format("* {0}<br />", objError);
                }
            }

            return (ColErrors.Count == 0);
        }
        #endregion

        #region SaveNewTicketForm
        private int SaveNewTicketForm()
        {
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            // Save Task
            ADefHelpDesk_Task objADefHelpDesk_Task = new ADefHelpDesk_Task();

            objADefHelpDesk_Task.Status = "New";
            objADefHelpDesk_Task.CreatedDate = DateTime.Now;
            objADefHelpDesk_Task.Description = txtDescription.Text;
            objADefHelpDesk_Task.PortalID = PortalId;
            objADefHelpDesk_Task.Priority = ddlPriority.SelectedValue;
            objADefHelpDesk_Task.RequesterPhone = txtPhone.Text;
            objADefHelpDesk_Task.AssignedRoleID = -1;
            objADefHelpDesk_Task.TicketPassword = GetRandomPassword();

            if (Convert.ToInt32(txtUserID.Text) == -1)
            {
                // User not logged in
                objADefHelpDesk_Task.RequesterEmail = txtEmail.Text;
                objADefHelpDesk_Task.RequesterName = txtName.Text;
                objADefHelpDesk_Task.RequesterUserID = -1;
            }
            else
            {
                // User logged in
                objADefHelpDesk_Task.RequesterUserID = Convert.ToInt32(txtUserID.Text);
                objADefHelpDesk_Task.RequesterName = UserController.GetUser(PortalId, Convert.ToInt32(txtUserID.Text), false).DisplayName;
            }

            if (txtDueDate.Text.Trim().Length > 1)
            {
                objADefHelpDesk_Task.DueDate = Convert.ToDateTime(txtDueDate.Text.Trim());
            }

            // If Admin panel is visible this is an admin
            // Save the Status and Assignment
            if (pnlAdminTicketStatus.Visible == true)
            {
                objADefHelpDesk_Task.AssignedRoleID = Convert.ToInt32(ddlAssignedAdmin.SelectedValue);
                objADefHelpDesk_Task.Status = ddlStatusAdmin.SelectedValue;
            }

            objADefHelpDeskDALDataContext.ADefHelpDesk_Tasks.InsertOnSubmit(objADefHelpDesk_Task);
            objADefHelpDeskDALDataContext.SubmitChanges();

            // Save Task Details
            ADefHelpDesk_TaskDetail objADefHelpDesk_TaskDetail = new ADefHelpDesk_TaskDetail();

            if ((txtDetails.Text.Trim().Length > 0) || (TicketFileUpload.HasFile))
            {
                objADefHelpDesk_TaskDetail.TaskID = objADefHelpDesk_Task.TaskID;
                objADefHelpDesk_TaskDetail.Description = txtDetails.Text;
                objADefHelpDesk_TaskDetail.DetailType = "Comment-Visible";
                objADefHelpDesk_TaskDetail.InsertDate = DateTime.Now;

                if (Convert.ToInt32(txtUserID.Text) == -1)
                {
                    // User not logged in
                    objADefHelpDesk_TaskDetail.UserID = -1;
                }
                else
                {
                    // User logged in
                    objADefHelpDesk_TaskDetail.UserID = Convert.ToInt32(txtUserID.Text);
                }

                objADefHelpDeskDALDataContext.ADefHelpDesk_TaskDetails.InsertOnSubmit(objADefHelpDesk_TaskDetail);
                objADefHelpDeskDALDataContext.SubmitChanges();

                // Upload the File
                if (TicketFileUpload.HasFile)
                {
                    UploadFile(objADefHelpDesk_TaskDetail.DetailID);
                    // Insert Log
                    Log.InsertLog(objADefHelpDesk_Task.TaskID, UserId, String.Format("{0} uploaded file '{1}'.", GetUserName(), TicketFileUpload.FileName));
                }
            }

            // Insert Log
            Log.InsertLog(objADefHelpDesk_Task.TaskID, UserId, String.Format("{0} created ticket.", GetUserName()));

            return objADefHelpDesk_Task.TaskID;
        }
        #endregion

        #region SaveTags
        private void SaveTags(int intTaskID)
        {
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            TreeView objTreeView = (TreeView)TagsTree.FindControl("tvCategories");
            if (objTreeView.CheckedNodes.Count > 0)
            {
                // Iterate through the CheckedNodes collection 
                foreach (TreeNode node in objTreeView.CheckedNodes)
                {
                    ADefHelpDesk_TaskCategory objADefHelpDesk_TaskCategory = new ADefHelpDesk_TaskCategory();

                    objADefHelpDesk_TaskCategory.TaskID = intTaskID;
                    objADefHelpDesk_TaskCategory.CategoryID = Convert.ToInt32(node.Value);

                    objADefHelpDeskDALDataContext.ADefHelpDesk_TaskCategories.InsertOnSubmit(objADefHelpDesk_TaskCategory);
                    objADefHelpDeskDALDataContext.SubmitChanges();
                }
            }
        }
        #endregion

        #region GetRandomPassword
        public string GetRandomPassword()
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            int intElements = random.Next(10, 26);

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

        #region GetUserName
        private string GetUserName()
        {
            string strUserName = "Anonymous";

            if (UserId > -1)
            {
                strUserName = strUserName = UserInfo.DisplayName;
            }

            return strUserName;
        }
        #endregion

        #region btnClearUser_Click
        protected void btnClearUser_Click(object sender, EventArgs e)
        {
            DisplayAdminView();
        }
        #endregion

        // File upload

        #region UploadFile
        private void UploadFile(int intDetailID)
        {
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            string strUploadefFilesPath = (from ADefHelpDesk_Settings in objADefHelpDeskDALDataContext.ADefHelpDesk_Settings
                                           where ADefHelpDesk_Settings.PortalID == PortalId
                                           where ADefHelpDesk_Settings.SettingName == "UploadefFilesPath"
                                           select ADefHelpDesk_Settings).FirstOrDefault().SettingValue;

            EnsureDirectory(new System.IO.DirectoryInfo(strUploadefFilesPath));
            string strfilename = Convert.ToString(intDetailID) + "_" + GetRandomPassword() + Path.GetExtension(TicketFileUpload.FileName).ToLower();
            strUploadefFilesPath = strUploadefFilesPath + @"\" + strfilename;
            TicketFileUpload.SaveAs(strUploadefFilesPath);

            ADefHelpDesk_Attachment objADefHelpDesk_Attachment = new ADefHelpDesk_Attachment();
            objADefHelpDesk_Attachment.DetailID = intDetailID;
            objADefHelpDesk_Attachment.FileName = strfilename;
            objADefHelpDesk_Attachment.OriginalFileName = TicketFileUpload.FileName;
            objADefHelpDesk_Attachment.AttachmentPath = strUploadefFilesPath;
            objADefHelpDesk_Attachment.UserID = UserId;

            objADefHelpDeskDALDataContext.ADefHelpDesk_Attachments.InsertOnSubmit(objADefHelpDesk_Attachment);
            objADefHelpDeskDALDataContext.SubmitChanges();
        }
        #endregion

        #region EnsureDirectory
        public static void EnsureDirectory(System.IO.DirectoryInfo oDirInfo)
        {
            if (oDirInfo.Parent != null)
                EnsureDirectory(oDirInfo.Parent);
            if (!oDirInfo.Exists)
            {
                oDirInfo.Create();
            }
        }
        #endregion

        // Admin User Search

        #region gvCurrentProcessor_SelectedIndexChanged
        protected void btnSearchUser_Click(object sender, EventArgs e)
        {
            if (txtSearchForUser.Text.Trim().Length != 0)
            {
                ArrayList Users;
                int TotalRecords = 0;
                if (ddlSearchForUserType.SelectedValue == "Email")
                {
                    Users = UserController.GetUsersByEmail(PortalId, false, txtSearchForUser.Text + "%", 0, 5, ref TotalRecords);
                }
                else
                {
                    String propertyName = ddlSearchForUserType.SelectedItem.Value;
                    Users = UserController.GetUsersByProfileProperty(PortalId, false, propertyName, txtSearchForUser.Text + "%", 0, 5, ref TotalRecords);
                }
                if (Users.Count > 0)
                {
                    lblCurrentProcessorNotFound.Visible = false;
                    gvCurrentProcessor.Visible = true;
                    gvCurrentProcessor.DataSource = Users;
                    gvCurrentProcessor.DataBind();
                }
                else
                {
                    lblCurrentProcessorNotFound.Text = Localization.GetString("UserIsNotFound.Text", LocalResourceFile);
                    lblCurrentProcessorNotFound.Visible = true;
                    gvCurrentProcessor.Visible = false;
                }
            }
        }
        #endregion

        #region gvCurrentProcessor_SelectedIndexChanged
        protected void gvCurrentProcessor_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView GridView = (GridView)sender;
            GridViewRow GridViewRowAdd = (GridViewRow)GridView.SelectedRow;
            LinkButton LinkButtonAdd = (LinkButton)GridViewRowAdd.FindControl("linkbutton1");

            UserInfo objUserInfo = new UserInfo();
            objUserInfo = UserController.GetUser(PortalId, Convert.ToInt16(LinkButtonAdd.CommandArgument), false);

            txtName.Visible = false;
            txtEmail.Visible = false;
            lblName.Visible = true;
            lblEmail.Visible = true;

            lblName.Text = objUserInfo.DisplayName;
            lblEmail.Text = objUserInfo.Email;

            txtUserID.Text = LinkButtonAdd.CommandArgument;
            txtSearchForUser.Text = "";
            gvCurrentProcessor.Visible = false;
            btnClearUser.Visible = true;
        }
        #endregion

        // Email

        #region SendEmail
        private void SendEmail(string TaskID)
        {
            try
            {
                ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

                ADefHelpDesk_Task objADefHelpDesk_Tasks = (from ADefHelpDesk_Tasks in objADefHelpDeskDALDataContext.ADefHelpDesk_Tasks
                                                           where ADefHelpDesk_Tasks.TaskID == Convert.ToInt32(TaskID)
                                                           select ADefHelpDesk_Tasks).FirstOrDefault();

                string strPasswordLinkUrl = Utility.FixURLLink(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "EditTask", "mid=" + ModuleId.ToString(), String.Format(@"&TaskID={0}&TP={1}", TaskID, objADefHelpDesk_Tasks.TicketPassword)), PortalSettings.PortalAlias.HTTPAlias);
                string strLinkUrl = Utility.FixURLLink(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "EditTask", "mid=" + ModuleId.ToString(), String.Format(@"&TaskID={0}", TaskID)), PortalSettings.PortalAlias.HTTPAlias);
                string strSubject = String.Format(Localization.GetString("NewHelpDeskTicketCreated.Text", LocalResourceFile), TaskID);
                string strBody = "";

                if (Convert.ToInt32(txtUserID.Text) != UserId || UserId == -1)
                {
                    if (UserId == -1)
                    {
                        // Anonymous user has created a ticket
                        strBody = String.Format(Localization.GetString("ANewHelpDeskTicket.Text", LocalResourceFile), TaskID); ;
                    }
                    else
                    {
                        // Admin has created a Ticket on behalf of another user
                        strBody = String.Format(Localization.GetString("HasCreatedANewHelpDeskTicket.Text", LocalResourceFile), UserInfo.DisplayName, TaskID, PortalSettings.PortalAlias.HTTPAlias); ;
                    }

                    strBody = strBody + Environment.NewLine;
                    strBody = strBody + String.Format(Localization.GetString("YouMaySeeStatusHere.Text", LocalResourceFile), strPasswordLinkUrl);

                    string strEmail = txtEmail.Text;

                    // If userId is not -1 then get the Email
                    if (Convert.ToInt32(txtUserID.Text) > -1)
                    {
                        strEmail = UserController.GetUser(PortalId, Convert.ToInt32(txtUserID.Text), false).Email;
                    }

                    DotNetNuke.Services.Mail.Mail.SendMail(PortalSettings.Email, strEmail, "", strSubject, strBody, "", "HTML", "", "", "", "");

                    // Get Admin Role
                    string strAdminRoleID = GetAdminRole();
                    // User is an Administrator
                    if (UserInfo.IsInRole(strAdminRoleID) || UserInfo.IsInRole("Administrators") || UserInfo.IsSuperUser)
                    {
                        // If Ticket is assigned to any group other than unassigned notify them
                        if (Convert.ToInt32(ddlAssignedAdmin.SelectedValue) > -1)
                        {
                            NotifyAssignedGroup(TaskID);
                        }
                    }
                    else
                    {
                        // This is not an Admin so Notify the Admins
                        strSubject = String.Format(Localization.GetString("NewHelpDeskTicketCreatedAt.Text", LocalResourceFile), TaskID, PortalSettings.PortalAlias.HTTPAlias);
                        strBody = String.Format(Localization.GetString("ANewHelpDeskTicketCreated.Text", LocalResourceFile), TaskID, txtDescription.Text);
                        strBody = strBody + Environment.NewLine;
                        strBody = strBody + String.Format(Localization.GetString("YouMaySeeStatusHere.Text", LocalResourceFile), strLinkUrl);

                        // Get all users in the Admin Role
                        RoleController objRoleController = new RoleController();
                        ArrayList colAdminUsers = objRoleController.GetUsersByRoleName(PortalId, GetAdminRole());

                        foreach (UserInfo objUserInfo in colAdminUsers)
                        {
                            DotNetNuke.Services.Mail.Mail.SendMail(PortalSettings.Email, objUserInfo.Email, "", strSubject, strBody, "", "HTML", "", "", "", "");
                        }
                    }
                }
                else
                {
                    // A normal ticket has been created
                    strSubject = String.Format(Localization.GetString("NewHelpDeskTicketCreatedAt.Text", LocalResourceFile), TaskID, PortalSettings.PortalAlias.HTTPAlias);
                    strBody = String.Format(Localization.GetString("ANewHelpDeskTicketCreated.Text", LocalResourceFile), TaskID, txtDescription.Text);
                    strBody = strBody + Environment.NewLine;
                    strBody = strBody + String.Format(Localization.GetString("YouMaySeeStatusHere.Text", LocalResourceFile), strLinkUrl);

                    // Get all users in the Admin Role
                    RoleController objRoleController = new RoleController();
                    ArrayList colAdminUsers = objRoleController.GetUsersByRoleName(PortalId, GetAdminRole());

                    foreach (UserInfo objUserInfo in colAdminUsers)
                    {
                        DotNetNuke.Services.Mail.Mail.SendMail(PortalSettings.Email, objUserInfo.Email, "", strSubject, strBody, "", "HTML", "", "", "", "");
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }
        #endregion

        #region NotifyAssignedGroup
        private void NotifyAssignedGroup(string TaskID)
        {
            RoleController objRoleController = new RoleController();
            string strAssignedRole = String.Format("{0}", objRoleController.GetRole(Convert.ToInt32(ddlAssignedAdmin.SelectedValue), PortalId).RoleName);

            string strSubject = String.Format(Localization.GetString("HelpDeskTicketAtHasBeenAssigned.Text", LocalResourceFile), TaskID, PortalSettings.PortalAlias.HTTPAlias, strAssignedRole);
            string strBody = String.Format(Localization.GetString("ANewHelpDeskTicketHasBeenAssigned.Text", LocalResourceFile), TaskID, txtDescription.Text);
            strBody = strBody + Environment.NewLine;
            strBody = strBody + String.Format(Localization.GetString("YouMaySeeStatusHere.Text", LocalResourceFile), DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "EditTask", "mid=" + ModuleId.ToString(), String.Format(@"&TaskID={0}", TaskID)));

            // Get all users in the AssignedRole Role
            ArrayList colAssignedRoleUsers = objRoleController.GetUsersByRoleName(PortalId, strAssignedRole);

            foreach (UserInfo objUserInfo in colAssignedRoleUsers)
            {
                DotNetNuke.Services.Mail.Mail.SendMail(PortalSettings.Email, objUserInfo.Email, "", strSubject, strBody, "", "HTML", "", "", "", "");
            }

            Log.InsertLog(Convert.ToInt32(TaskID), UserId, String.Format(Localization.GetString("AssignedTicketTo.Text", LocalResourceFile), UserInfo.DisplayName, strAssignedRole));
        }
        #endregion

        // Existing Tickets

        #region DisplayExistingTickets
        private void DisplayExistingTickets(ADefHelpDesk_LastSearch objLastSearch)
        {
            string[] UsersRoles = UserInfo.Roles;
            List<int> UsersRoleIDs = new List<int>();
            string strSearchText = (objLastSearch.SearchText == null) ? "" : objLastSearch.SearchText;

            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            IQueryable<ExistingTasks> result = from ADefHelpDesk_Tasks in objADefHelpDeskDALDataContext.ADefHelpDesk_Tasks
                                               where ADefHelpDesk_Tasks.PortalID == PortalId
                                               orderby ADefHelpDesk_Tasks.CreatedDate descending
                                               select new ExistingTasks
                                               {
                                                   TaskID = ADefHelpDesk_Tasks.TaskID,
                                                   Status = ADefHelpDesk_Tasks.Status,
                                                   Priority = ADefHelpDesk_Tasks.Priority,
                                                   DueDate = ADefHelpDesk_Tasks.DueDate,
                                                   CreatedDate = ADefHelpDesk_Tasks.CreatedDate,
                                                   Assigned = ADefHelpDesk_Tasks.AssignedRoleID.ToString(),
                                                   Description = ADefHelpDesk_Tasks.Description,
                                                   Requester = ADefHelpDesk_Tasks.RequesterUserID.ToString(),
                                                   RequesterName = ADefHelpDesk_Tasks.RequesterName
                                               };

            #region Only show users the records they should see
            // Only show users the records they should see
            if (!(UserInfo.IsInRole(GetAdminRole()) || UserInfo.IsInRole("Administrators") || UserInfo.IsSuperUser))
            {
                RoleController objRoleController = new RoleController();
                foreach (RoleInfo objRoleInfo in objRoleController.GetUserRoles(PortalId, UserId))
                {
                    UsersRoleIDs.Add(objRoleInfo.RoleID);
                }

                result = from UsersRecords in result
                         where Convert.ToInt32(UsersRecords.Requester) == UserId ||
                         UsersRoleIDs.Contains(Convert.ToInt32(UsersRecords.Assigned))
                         select UsersRecords;
            }
            #endregion

            #region Filter Status
            // Filter Status
            if (objLastSearch.Status != null)
            {
                result = from Status in result
                         where Status.Status == objLastSearch.Status
                         select Status;
            }
            #endregion

            #region Filter Priority
            // Filter Priority
            if (objLastSearch.Priority != null)
            {
                result = from Priority in result
                         where Priority.Priority == objLastSearch.Priority
                         select Priority;
            }
            #endregion

            #region Filter Assigned
            // Filter Assigned
            if (objLastSearch.AssignedRoleID.HasValue)
            {
                if (!(objLastSearch.AssignedRoleID == -2))
                {
                    result = from Assigned in result
                             where Assigned.Assigned == objLastSearch.AssignedRoleID.ToString()
                             select Assigned;
                }
            }
            #endregion

            #region Filter DueDate
            // Filter DueDate
            if (objLastSearch.DueDate.HasValue)
            {
                result = from objDueDate in result
                         where objDueDate.DueDate > objLastSearch.DueDate
                         select objDueDate;
            }
            #endregion

            #region Filter CreatedDate
            // Filter CreatedDate
            if (objLastSearch.CreatedDate.HasValue)
            {
                result = from CreatedDate in result
                         where CreatedDate.CreatedDate > objLastSearch.CreatedDate
                         select CreatedDate;
            }
            #endregion

            #region Filter TextBox (Search)
            // Filter TextBox
            if (strSearchText.Trim().Length > 0)
            {
                result = (from Search in result
                          join details in objADefHelpDeskDALDataContext.ADefHelpDesk_TaskDetails
                          on Search.TaskID equals details.TaskID into joined
                          from leftjoin in joined.DefaultIfEmpty()
                          where Search.Description.Contains(strSearchText) ||
                          Search.RequesterName.Contains(strSearchText) ||
                          Search.TaskID.ToString().Contains(strSearchText) ||
                          leftjoin.Description.Contains(strSearchText)
                          select Search).Distinct();
            }
            #endregion

            // Convert the results to a list because the query to filter the tags 
            // must be made after the preceeding query results have been pulled from the database
            List<ExistingTasks> FinalResult = result.Distinct().ToList();

            #region Filter Tags
            // Filter Tags
            if (objLastSearch.Categories != null)
            {
                char[] delimiterChars = { ',' };
                string[] ArrStrCategories = objLastSearch.Categories.Split(delimiterChars);
                // Convert the Categories selected from the Tags tree to an array of integers
                int[] ArrIntCatagories = Array.ConvertAll<string, int>(ArrStrCategories, new Converter<string, int>(ConvertStringToInt));

                // Perform a query that does in intersect between all the Catagories selected and all the categories that each TaskID has
                // The number of values that match must equal the number of values that were selected in the Tags tree
                FinalResult = (from Categories in FinalResult.AsQueryable()
                               where ((from ADefHelpDesk_TaskCategories in objADefHelpDeskDALDataContext.ADefHelpDesk_TaskCategories
                                       where ADefHelpDesk_TaskCategories.TaskID == Categories.TaskID
                                       select ADefHelpDesk_TaskCategories.CategoryID).ToArray<int>()).Intersect(ArrIntCatagories).Count() == ArrIntCatagories.Length
                               select Categories).ToList();
            }
            #endregion

            #region Sort
            switch (SortExpression)
            {
                case "TaskID":
                case "TaskID ASC":
                    FinalResult = FinalResult.AsEnumerable().OrderBy(p => p.TaskID).ToList();
                    break;
                case "TaskID DESC":
                    FinalResult = FinalResult.AsEnumerable().OrderByDescending(p => p.TaskID).ToList();
                    break;
                case "Status":
                case "Status ASC":
                    FinalResult = FinalResult.AsEnumerable().OrderBy(p => p.Status).ToList();
                    break;
                case "Status DESC":
                    FinalResult = FinalResult.AsEnumerable().OrderByDescending(p => p.Status).ToList();
                    break;
                case "Priority":
                case "Priority ASC":
                    FinalResult = FinalResult.AsEnumerable().OrderBy(p => p.Priority).ToList();
                    break;
                case "Priority DESC":
                    FinalResult = FinalResult.AsEnumerable().OrderByDescending(p => p.Priority).ToList();
                    break;
                case "DueDate":
                case "DueDate ASC":
                    FinalResult = FinalResult.AsEnumerable().OrderBy(p => p.DueDate).ToList();
                    break;
                case "DueDate DESC":
                    FinalResult = FinalResult.AsEnumerable().OrderByDescending(p => p.DueDate).ToList();
                    break;
                case "CreatedDate":
                case "CreatedDate ASC":
                    FinalResult = FinalResult.AsEnumerable().OrderBy(p => p.CreatedDate).ToList();
                    break;
                case "CreatedDate DESC":
                    FinalResult = FinalResult.AsEnumerable().OrderByDescending(p => p.CreatedDate).ToList();
                    break;
                case "Assigned":
                case "Assigned ASC":
                    FinalResult = FinalResult.AsEnumerable().OrderBy(p => p.Assigned).ToList();
                    break;
                case "Assigned DESC":
                    FinalResult = FinalResult.AsEnumerable().OrderByDescending(p => p.Assigned).ToList();
                    break;
                case "Description":
                case "Description ASC":
                    FinalResult = FinalResult.AsEnumerable().OrderBy(p => p.Description).ToList();
                    break;
                case "Description DESC":
                    FinalResult = FinalResult.AsEnumerable().OrderByDescending(p => p.Description).ToList();
                    break;
                case "Requester":
                case "Requester ASC":
                    FinalResult = FinalResult.AsEnumerable().OrderBy(p => p.RequesterName).ToList();
                    break;
                case "Requester DESC":
                    FinalResult = FinalResult.AsEnumerable().OrderByDescending(p => p.RequesterName).ToList();
                    break;
            }
            #endregion

            #region Paging
            int intPageSize = 25;
            int intCurrentPage = (Convert.ToInt32(objLastSearch.CurrentPage) == 0) ? 1 : Convert.ToInt32(objLastSearch.CurrentPage);

            //Paging
            int intPages = 0;
            int intRecords = FinalResult.Count();
            if ((intRecords > 0) & (intRecords > intPageSize))
            {
                intPages = (intRecords / intPageSize);

                // If there are more records add 1 to page count
                if (intRecords % intPageSize > 0)
                {
                    intPages += 1;
                }

                // Show and hide buttons
                lnkPrevious.Visible = (intCurrentPage > 1);
                lnkNext.Visible = (intCurrentPage != intPages);

                lblRecords.Text = String.Format("{0} of {1}", intCurrentPage.ToString(), intPages.ToString());
            }
            #endregion

            // If the current page is greater than the number of pages
            // reset to page one and save 
            if (intCurrentPage > intPages)
            {
                intCurrentPage = 1;
                ADefHelpDesk_LastSearch objADefHelpDesk_LastSearch = GetLastSearchCriteria();
                objADefHelpDesk_LastSearch.CurrentPage = intCurrentPage;
                SaveLastSearchCriteria(objADefHelpDesk_LastSearch);

                lnkPrevious.Visible = true;
            }

            // Display Records
            lvTasks.DataSource = FinalResult.Skip((intCurrentPage - 1) * intPageSize).Take(intPageSize);
            lvTasks.DataBind();

            // Display paging panel
            pnlPaging.Visible = (intPages > 1);
        }
        #endregion

        #region GetCurrentPage
        private int GetCurrentPage()
        {
            int intCurrentPage;
            if (int.TryParse(txtCurrentPage.Text, out intCurrentPage) == false)
            {
                intCurrentPage = 1;
            }

            return intCurrentPage;
        }
        #endregion

        #region ConvertStringToInt
        private int ConvertStringToInt(string strParameter)
        {
            return Convert.ToInt32(strParameter);
        }
        private int? ConvertStringToNullableInt(string strParameter)
        {
            return Convert.ToInt32(strParameter);
        }
        #endregion

        #region lvTasks_ItemDataBound
        protected void lvTasks_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ListView listView = (ListView)sender;
            ListViewDataItem objListViewDataItem = (ListViewDataItem)e.Item;

            // Get instance of fields
            HyperLink lnkTaskID = (HyperLink)e.Item.FindControl("lnkTaskID");
            Label StatusLabel = (Label)e.Item.FindControl("StatusLabel");
            Label PriorityLabel = (Label)e.Item.FindControl("PriorityLabel");
            Label DueDateLabel = (Label)e.Item.FindControl("DueDateLabel");
            Label CreatedDateLabel = (Label)e.Item.FindControl("CreatedDateLabel");
            Label AssignedLabel = (Label)e.Item.FindControl("AssignedRoleIDLabel");
            Label DescriptionLabel = (Label)e.Item.FindControl("DescriptionLabel");
            Label RequesterLabel = (Label)e.Item.FindControl("RequesterUserIDLabel");
            Label RequesterNameLabel = (Label)e.Item.FindControl("RequesterNameLabel");

            // Get the data
            ExistingTasks objExistingTasks = (ExistingTasks)objListViewDataItem.DataItem;

            // Format the TaskID hyperlink
            lnkTaskID.Text = string.Format("{0}", objExistingTasks.TaskID.ToString());
            lnkTaskID.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "EditTask", "mid=" + ModuleId.ToString(), String.Format(@"&TaskID={0}", objExistingTasks.TaskID.ToString()));

            // Format DueDate
            if (objExistingTasks.DueDate != null)
            {
                DueDateLabel.Text = objExistingTasks.DueDate.Value.ToShortDateString();
                if ((objExistingTasks.DueDate < DateTime.Now) & (StatusLabel.Text == "New" || StatusLabel.Text == "Active" || StatusLabel.Text == "On Hold"))
                {
                    DueDateLabel.BackColor = System.Drawing.Color.Yellow;
                }
            }

            // Format CreatedDate
            if (objExistingTasks.CreatedDate != null)
            {
                DateTime dtCreatedDate = Convert.ToDateTime(objExistingTasks.CreatedDate.Value.ToShortDateString());
                DateTime dtNow = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                if (dtCreatedDate == dtNow)
                {
                    CreatedDateLabel.Text = objExistingTasks.CreatedDate.Value.ToLongTimeString();
                }
                else
                {
                    CreatedDateLabel.Text = objExistingTasks.CreatedDate.Value.ToShortDateString();
                }
            }

            // Format Requestor
            if (RequesterLabel.Text != "-1")
            {
                try
                {
                    RequesterNameLabel.Text = UserController.GetUser(PortalId, Convert.ToInt32(RequesterLabel.Text), false).DisplayName;
                }
                catch 
                {
                    RequesterNameLabel.Text = String.Format("[User Deleted]");
                }
            }
            if (RequesterNameLabel.Text.Length > 10)
            {
                RequesterNameLabel.Text = String.Format("{0} ...", Strings.Left(RequesterNameLabel.Text, 10));
            }

            // Format Description
            if (DescriptionLabel.Text.Length > 10)
            {
                DescriptionLabel.Text = String.Format("{0} ...", Strings.Left(DescriptionLabel.Text, 10));

            }

            // Format Assigned
            if (AssignedLabel.Text != "-1")
            {
                RoleController objRoleController = new RoleController();
                try
                {
                    AssignedLabel.Text = String.Format("{0}", objRoleController.GetRole(Convert.ToInt32(AssignedLabel.Text), PortalId).RoleName);
                }
                catch
                {
                    AssignedLabel.Text = Localization.GetString("DeletedRole.Text", LocalResourceFile);
                }
                AssignedLabel.ToolTip = AssignedLabel.Text;

                if (AssignedLabel.Text.Length > 10)
                {
                    AssignedLabel.Text = String.Format("{0} ...", Strings.Left(AssignedLabel.Text, 10));
                }
            }
            else
            {
                AssignedLabel.Text = Localization.GetString("Unassigned.Text", LocalResourceFile);
            }

        }
        #endregion

        #region lvTasks_Sorting
        protected void lvTasks_Sorting(object sender, ListViewSortEventArgs e)
        {
            if (SortDirection == "ASC")
            {
                SortDirection = "DESC";
            }
            else
            {
                SortDirection = "ASC";
            }

            SortExpression = String.Format("{0} {1}", e.SortExpression, SortDirection);

            // Check the sort direction to set the image URL accordingly.
            string imgUrl;
            if (SortDirection == "ASC")
                imgUrl = "~/DesktopModules/ADefHelpDesk/images/dt-arrow-up.png";
            else
                imgUrl = "~/DesktopModules/ADefHelpDesk/images/dt-arrow-dn.png";

            // Check which field is being sorted
            // to set the visibility of the image controls.
            Image TaskIDImage = (Image)lvTasks.FindControl("TaskIDImage");
            Image StatusImage = (Image)lvTasks.FindControl("StatusImage");
            Image PriorityImage = (Image)lvTasks.FindControl("PriorityImage");
            Image DueDateImage = (Image)lvTasks.FindControl("DueDateImage");
            Image CreatedDateImage = (Image)lvTasks.FindControl("CreatedDateImage");
            Image AssignedImage = (Image)lvTasks.FindControl("AssignedImage");
            Image DescriptionImage = (Image)lvTasks.FindControl("DescriptionImage");
            Image RequesterImage = (Image)lvTasks.FindControl("RequesterImage");

            // Set each Image to the proper direction
            TaskIDImage.ImageUrl = imgUrl;
            StatusImage.ImageUrl = imgUrl;
            PriorityImage.ImageUrl = imgUrl;
            DueDateImage.ImageUrl = imgUrl;
            CreatedDateImage.ImageUrl = imgUrl;
            AssignedImage.ImageUrl = imgUrl;
            DescriptionImage.ImageUrl = imgUrl;
            RequesterImage.ImageUrl = imgUrl;

            // Set each Image to false
            TaskIDImage.Visible = false;
            StatusImage.Visible = false;
            PriorityImage.Visible = false;
            DueDateImage.Visible = false;
            CreatedDateImage.Visible = false;
            AssignedImage.Visible = false;
            DescriptionImage.Visible = false;
            RequesterImage.Visible = false;

            switch (e.SortExpression)
            {
                case "TaskID":
                    TaskIDImage.Visible = true;
                    break;
                case "Status":
                    StatusImage.Visible = true;
                    break;
                case "Priority":
                    PriorityImage.Visible = true;
                    break;
                case "DueDate":
                    DueDateImage.Visible = true;
                    break;
                case "CreatedDate":
                    CreatedDateImage.Visible = true;
                    break;
                case "Assigned":
                    AssignedImage.Visible = true;
                    break;
                case "Description":
                    DescriptionImage.Visible = true;
                    break;
                case "Requester":
                    RequesterImage.Visible = true;
                    break;
            }

            ADefHelpDesk_LastSearch objADefHelpDesk_LastSearch = GetValuesFromSearchForm();
            // Save Search Criteria
            SaveLastSearchCriteria(objADefHelpDesk_LastSearch);
            // Execute Search
            DisplayExistingTickets(SearchCriteria);
        }
        #endregion

        #region lvTasks_ItemCommand
        protected void lvTasks_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            #region Search
            // Search
            if (e.CommandName == "Search")
            {
                ADefHelpDesk_LastSearch objADefHelpDesk_LastSearch = GetValuesFromSearchForm();
                // Save Search Criteria
                SaveLastSearchCriteria(objADefHelpDesk_LastSearch);
                // Execute Search
                DisplayExistingTickets(SearchCriteria);
            }
            #endregion

            #region EmptyDataTemplateSearch
            //EmptyDataTemplateSearch        
            if (e.CommandName == "EmptyDataTemplateSearch")
            {
                TextBox txtSearch = (TextBox)e.Item.FindControl("txtSearch");
                DropDownList ddlStatus = (DropDownList)e.Item.FindControl("ddlStatus");
                DropDownList ddlPriority = (DropDownList)e.Item.FindControl("ddlPriority");
                DropDownList ddlAssigned = (DropDownList)e.Item.FindControl("ddlAssigned");
                TextBox txtDue = (TextBox)e.Item.FindControl("txtDue");
                TextBox txtCreated = (TextBox)e.Item.FindControl("txtCreated");

                // Use an ExistingTasks object to pass the values to the Search method
                ADefHelpDesk_LastSearch objADefHelpDesk_LastSearch = new ADefHelpDesk_LastSearch();
                objADefHelpDesk_LastSearch.SearchText = (txtSearch.Text.Trim().Length == 0) ? null : txtSearch.Text.Trim();
                objADefHelpDesk_LastSearch.Status = (ddlStatus.SelectedValue == "*All*") ? null : ddlStatus.SelectedValue;
                objADefHelpDesk_LastSearch.AssignedRoleID = (ddlAssigned.SelectedValue == "-2") ? null : (int?)Convert.ToInt32(ddlAssigned.SelectedValue);
                objADefHelpDesk_LastSearch.Priority = (ddlPriority.SelectedValue == "*All*") ? null : ddlPriority.SelectedValue;

                // Created Date
                if (txtCreated.Text.Trim().Length > 4)
                {
                    try
                    {
                        DateTime dtCreated = Convert.ToDateTime(txtCreated.Text.Trim());
                        objADefHelpDesk_LastSearch.CreatedDate = dtCreated.AddDays(-1);
                    }
                    catch
                    {
                        txtCreated.Text = "";
                    }
                }
                else
                {
                    txtCreated.Text = "";
                }

                // Due Date
                if (txtDue.Text.Trim().Length > 4)
                {
                    try
                    {
                        DateTime dtDue = Convert.ToDateTime(txtDue.Text.Trim());
                        objADefHelpDesk_LastSearch.DueDate = dtDue.AddDays(-1);
                    }
                    catch
                    {
                        txtDue.Text = "";
                    }
                }
                else
                {
                    txtDue.Text = "";
                }

                // Get Category Tags
                string strCategories = GetTagsTreeExistingTasks();
                if (strCategories.Length > 1)
                {
                    objADefHelpDesk_LastSearch.Categories = strCategories;
                }

                // Current Page
                objADefHelpDesk_LastSearch.CurrentPage = GetCurrentPage();

                // Save Search Criteria
                SaveLastSearchCriteria(objADefHelpDesk_LastSearch);
                // Execute Search
                DisplayExistingTickets(SearchCriteria);
            }
            #endregion

        }
        #endregion

        // Set controls to the Last Search criteria

        #region lvTasks_DataBound
        protected void lvTasks_DataBound(object sender, EventArgs e)
        {
            if (SearchCriteria != null)
            {
                TextBox txtSearch = new TextBox();
                DropDownList ddlStatus = new DropDownList();
                DropDownList ddlPriority = new DropDownList();
                DropDownList ddlAssigned = new DropDownList();
                TextBox txtDue = new TextBox();
                TextBox txtCreated = new TextBox();

                // Get an instance to the Search controls
                if (lvTasks.Items.Count == 0)
                {
                    // Empty Data Template
                    ListViewItem Ctrl0 = (ListViewItem)lvTasks.FindControl("Ctrl0");
                    HtmlTable EmptyDataTemplateTable = (HtmlTable)Ctrl0.FindControl("EmptyDataTemplateTable");

                    txtSearch = (TextBox)EmptyDataTemplateTable.FindControl("txtSearch");
                    ddlStatus = (DropDownList)EmptyDataTemplateTable.FindControl("ddlStatus");
                    ddlPriority = (DropDownList)EmptyDataTemplateTable.FindControl("ddlPriority");
                    txtDue = (TextBox)EmptyDataTemplateTable.FindControl("txtDue");
                    txtCreated = (TextBox)EmptyDataTemplateTable.FindControl("txtCreated");
                    ddlAssigned = (DropDownList)EmptyDataTemplateTable.FindControl("ddlAssigned");
                }
                else
                {
                    // Normal results template
                    txtSearch = (TextBox)lvTasks.FindControl("txtSearch");
                    ddlStatus = (DropDownList)lvTasks.FindControl("ddlStatus");
                    ddlPriority = (DropDownList)lvTasks.FindControl("ddlPriority");
                    txtDue = (TextBox)lvTasks.FindControl("txtDue");
                    txtCreated = (TextBox)lvTasks.FindControl("txtCreated");
                    ddlAssigned = (DropDownList)lvTasks.FindControl("ddlAssigned");
                }

                ADefHelpDesk_LastSearch objADefHelpDesk_LastSearch = (ADefHelpDesk_LastSearch)SearchCriteria;

                if (objADefHelpDesk_LastSearch.SearchText != null)
                {
                    txtSearch.Text = objADefHelpDesk_LastSearch.SearchText;
                }

                if (objADefHelpDesk_LastSearch.Status != null)
                {
                    ddlStatus.SelectedValue = objADefHelpDesk_LastSearch.Status;
                }

                if (objADefHelpDesk_LastSearch.Priority != null)
                {
                    ddlPriority.SelectedValue = objADefHelpDesk_LastSearch.Priority;
                }

                if (objADefHelpDesk_LastSearch.DueDate != null)
                {
                    txtDue.Text = objADefHelpDesk_LastSearch.DueDate.Value.AddDays(1).ToShortDateString();
                }

                if (objADefHelpDesk_LastSearch.CreatedDate != null)
                {
                    txtCreated.Text = objADefHelpDesk_LastSearch.CreatedDate.Value.AddDays(1).ToShortDateString();
                }

                // Load Dropdown
                ddlAssigned.Items.Clear();

                RoleController objRoleController = new RoleController();
                ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

                List<ADefHelpDesk_Role> colADefHelpDesk_Roles = (from ADefHelpDesk_Roles in objADefHelpDeskDALDataContext.ADefHelpDesk_Roles
                                                                 where ADefHelpDesk_Roles.PortalID == PortalId
                                                                 select ADefHelpDesk_Roles).ToList();

                // Create a ListItemCollection to hold the Roles 
                ListItemCollection colListItemCollection = new ListItemCollection();

                // Add All
                ListItem AllRoleListItem = new ListItem();
                AllRoleListItem.Text = Localization.GetString("ddlAssignedAll.Text", LocalResourceFile);
                AllRoleListItem.Value = "-2";
                if (objADefHelpDesk_LastSearch.AssignedRoleID != null)
                {
                    if (objADefHelpDesk_LastSearch.AssignedRoleID == -2)
                    {
                        AllRoleListItem.Selected = true;
                    }
                }
                ddlAssigned.Items.Add(AllRoleListItem);

                // Add the Roles to the List
                foreach (ADefHelpDesk_Role objADefHelpDesk_Role in colADefHelpDesk_Roles)
                {
                    try
                    {
                        RoleInfo objRoleInfo = objRoleController.GetRole(Convert.ToInt32(objADefHelpDesk_Role.RoleID), PortalId);

                        ListItem RoleListItem = new ListItem();
                        RoleListItem.Text = objRoleInfo.RoleName;
                        RoleListItem.Value = objADefHelpDesk_Role.RoleID.ToString();

                        if (objADefHelpDesk_LastSearch.AssignedRoleID != null)
                        {
                            if (objADefHelpDesk_Role.RoleID == objADefHelpDesk_LastSearch.AssignedRoleID)
                            {
                                RoleListItem.Selected = true;
                            }
                        }

                        ddlAssigned.Items.Add(RoleListItem);
                    }
                    catch
                    {
                        // Role no longer exists in Portal
                        ListItem RoleListItem = new ListItem();
                        RoleListItem.Text = Localization.GetString("DeletedRole.Text", LocalResourceFile);
                        RoleListItem.Value = objADefHelpDesk_Role.RoleID.ToString();
                        ddlAssigned.Items.Add(RoleListItem);
                    }
                }

                // Add UnAssigned
                ListItem UnassignedRoleListItem = new ListItem();
                UnassignedRoleListItem.Text = Localization.GetString("Unassigned.Text", LocalResourceFile);
                UnassignedRoleListItem.Value = "-1";
                if (objADefHelpDesk_LastSearch.AssignedRoleID != null)
                {
                    if (objADefHelpDesk_LastSearch.AssignedRoleID == -1)
                    {
                        UnassignedRoleListItem.Selected = true;
                    }
                }
                ddlAssigned.Items.Add(UnassignedRoleListItem);

            }
        }
        #endregion

        #region GetValuesFromSearchForm
        private ADefHelpDesk_LastSearch GetValuesFromSearchForm()
        {
            TextBox txtSearch = (TextBox)lvTasks.FindControl("txtSearch");
            DropDownList ddlStatus = (DropDownList)lvTasks.FindControl("ddlStatus");
            DropDownList ddlPriority = (DropDownList)lvTasks.FindControl("ddlPriority");
            DropDownList ddlAssigned = (DropDownList)lvTasks.FindControl("ddlAssigned");
            TextBox txtDue = (TextBox)lvTasks.FindControl("txtDue");
            TextBox txtCreated = (TextBox)lvTasks.FindControl("txtCreated");

            // Use an ExistingTasks object to pass the values to the Search method
            ADefHelpDesk_LastSearch objADefHelpDesk_LastSearch = new ADefHelpDesk_LastSearch();
            objADefHelpDesk_LastSearch.SearchText = (txtSearch.Text.Trim().Length == 0) ? null : txtSearch.Text.Trim();
            objADefHelpDesk_LastSearch.Status = (ddlStatus.SelectedValue == "*All*") ? null : ddlStatus.SelectedValue;
            objADefHelpDesk_LastSearch.AssignedRoleID = (ddlAssigned.SelectedValue == "-2") ? null : (int?)Convert.ToInt32(ddlAssigned.SelectedValue);
            objADefHelpDesk_LastSearch.Priority = (ddlPriority.SelectedValue == "*All*") ? null : ddlPriority.SelectedValue;

            // Created Date
            if (txtCreated.Text.Trim().Length > 4)
            {
                try
                {
                    DateTime dtCreated = Convert.ToDateTime(txtCreated.Text.Trim());
                    objADefHelpDesk_LastSearch.CreatedDate = dtCreated.AddDays(-1);
                }
                catch
                {
                    txtCreated.Text = "";
                }
            }
            else
            {
                txtCreated.Text = "";
            }

            // Due Date
            if (txtDue.Text.Trim().Length > 4)
            {
                try
                {
                    DateTime dtDue = Convert.ToDateTime(txtDue.Text.Trim());
                    objADefHelpDesk_LastSearch.DueDate = dtDue.AddDays(-1);
                }
                catch
                {
                    txtDue.Text = "";
                }
            }
            else
            {
                txtDue.Text = "";
            }

            // Get Category Tags
            string strCategories = GetTagsTreeExistingTasks();
            if (strCategories.Length > 0)
            {
                objADefHelpDesk_LastSearch.Categories = strCategories;
            }

            // Current Page
            objADefHelpDesk_LastSearch.CurrentPage = GetCurrentPage();

            return objADefHelpDesk_LastSearch;
        }
        #endregion

        #region GetTagsTreeExistingTasks
        private string GetTagsTreeExistingTasks()
        {
            List<string> colSelectedCategories = new List<string>();

            try
            {
                TreeView objTreeView = (TreeView)TagsTreeExistingTasks.FindControl("tvCategories");
                if (objTreeView.CheckedNodes.Count > 0)
                {
                    // Iterate through the CheckedNodes collection 
                    foreach (TreeNode node in objTreeView.CheckedNodes)
                    {
                        colSelectedCategories.Add(node.Value);
                    }
                }

                string[] arrSelectedCategories = colSelectedCategories.ToArray<string>();
                string strSelectedCategories = String.Join(",", arrSelectedCategories);

                return Strings.Left(strSelectedCategories, 2000);
            }
            catch
            {
                return "";
            }
        }
        #endregion

        // Datasource for Assigned Role drop down

        #region LDSAssignedRoleID_Selecting
        protected void LDSAssignedRoleID_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            List<AssignedRoles> resultcolAssignedRoles = new List<AssignedRoles>();
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            List<AssignedRoles> colAssignedRoles = (from ADefHelpDesk_Tasks in objADefHelpDeskDALDataContext.ADefHelpDesk_Tasks
                                                    where ADefHelpDesk_Tasks.AssignedRoleID > -1
                                                    group ADefHelpDesk_Tasks by ADefHelpDesk_Tasks.AssignedRoleID into AssignedRole
                                                    select new AssignedRoles
                                                    {
                                                        AssignedRoleID = GetRolebyID(AssignedRole.Key),
                                                        Key = AssignedRole.Key.ToString()
                                                    }).ToList();

            AssignedRoles objAssignedRolesAll = new AssignedRoles();
            objAssignedRolesAll.AssignedRoleID = "*All*";
            objAssignedRolesAll.Key = "-2";
            resultcolAssignedRoles.Add(objAssignedRolesAll);

            AssignedRoles objAssignedRolesUnassigned = new AssignedRoles();
            objAssignedRolesUnassigned.AssignedRoleID = "Unassigned";
            objAssignedRolesUnassigned.Key = "-1";
            resultcolAssignedRoles.Add(objAssignedRolesUnassigned);
            resultcolAssignedRoles.AddRange(colAssignedRoles);

            e.Result = resultcolAssignedRoles;
        }
        #endregion

        #region GetRolebyID
        private string GetRolebyID(int RoleID)
        {
            string strRoleName = "Unassigned";
            if (RoleID > -1)
            {
                RoleController objRoleController = new RoleController();
                strRoleName = objRoleController.GetRole(RoleID, PortalId).RoleName;
            }

            return strRoleName;
        }
        #endregion

        // Paging

        #region lnkNext_Click
        protected void lnkNext_Click(object sender, EventArgs e)
        {
            ADefHelpDesk_LastSearch objADefHelpDesk_LastSearch = GetLastSearchCriteria();
            int intCurrentPage = Convert.ToInt32(objADefHelpDesk_LastSearch.CurrentPage ?? 1);
            intCurrentPage++;
            objADefHelpDesk_LastSearch.CurrentPage = intCurrentPage;
            SaveLastSearchCriteria(objADefHelpDesk_LastSearch);
            DisplayExistingTickets(SearchCriteria);
        }
        #endregion

        #region lnkPrevious_Click
        protected void lnkPrevious_Click(object sender, EventArgs e)
        {
            ADefHelpDesk_LastSearch objADefHelpDesk_LastSearch = GetLastSearchCriteria();
            int intCurrentPage = Convert.ToInt32(objADefHelpDesk_LastSearch.CurrentPage ?? 2);
            intCurrentPage--;
            objADefHelpDesk_LastSearch.CurrentPage = intCurrentPage;
            SaveLastSearchCriteria(objADefHelpDesk_LastSearch);
            DisplayExistingTickets(SearchCriteria);
        }
        #endregion

    }
}