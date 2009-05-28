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
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using DotNetNuke.Common;
using DotNetNuke.Security.Roles;
using DotNetNuke.Entities.Users;
using System.Collections;
using System.Drawing;
using Microsoft.VisualBasic;
using System.Text;
using System.IO;

namespace ADefWebserver.Modules.ADefHelpDesk
{
    public partial class EditTask : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            cmdtxtDueDateCalendar.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtDueDate);
            cmdtxtStartCalendar.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtStart);
            cmdtxtCompleteCalendar.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtComplete);
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["TaskID"] != null)
                {
                    CommentsControl.ViewOnly = true;
                    if (CheckSecurity())
                    {
                        ShowAdministratorLink();
                        ShowExistingTicketsLink();
                        LoadRolesDropDown();
                        DisplayCategoryTree();
                        DisplayTicketData();
                        CommentsControl.TaskID = Convert.ToInt32(lblTask.Text);
                        LogsControl.TaskID = Convert.ToInt32(lblTask.Text);

                        // If at this point CommentsControl is in View Only mode
                        // Set main form in View Only mode
                        if (CommentsControl.ViewOnly == true)
                        {
                            SetViewOnlyMode();
                        }

                        // Insert Log
                        Log.InsertLog(Convert.ToInt32(lblTask.Text), UserId, String.Format("{0} viewed ticket.", (UserId == -1) ? "Requester" : UserInfo.DisplayName));
                    }
                    else
                    {
                        pnlEditTask.Visible = false;
                        Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
                    }
                }
                else
                {
                    pnlEditTask.Visible = false;
                    Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
                }
            }
        }

        #region SetViewOnlyMode
        private void SetViewOnlyMode()
        {
            btnSave.Visible = false;
            btnComments.Visible = false;
            btnLogs.Visible = false;
            ddlAssigned.Enabled = false;
            ddlStatus.Enabled = false;
            ddlPriority.Enabled = false;
        }
        #endregion

        #region LoadRolesDropDown
        private void LoadRolesDropDown()
        {
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
                    ddlAssigned.Items.Add(RoleListItem);
                }
                catch
                {
                    // Role no longer exists in Portal
                    ListItem RoleListItem = new ListItem();
                    RoleListItem.Text = "[Deleted Role]";
                    RoleListItem.Value = objADefHelpDesk_Role.RoleID.ToString();
                    ddlAssigned.Items.Add(RoleListItem);
                }
            }

            // Add UnAssigned
            ListItem UnassignedRoleListItem = new ListItem();
            UnassignedRoleListItem.Text = "Unassigned";
            UnassignedRoleListItem.Value = "-1";
            ddlAssigned.Items.Add(UnassignedRoleListItem);

        }
        #endregion

        #region CheckSecurity
        private bool CheckSecurity()
        {
            bool boolPassedSecurity = false;
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            ADefHelpDesk_Task objADefHelpDesk_Tasks = (from ADefHelpDesk_Tasks in objADefHelpDeskDALDataContext.ADefHelpDesk_Tasks
                                                       where ADefHelpDesk_Tasks.TaskID == Convert.ToInt32(Request.QueryString["TaskID"])
                                                       select ADefHelpDesk_Tasks).FirstOrDefault();
            if (objADefHelpDesk_Tasks == null)
            {
                pnlEditTask.Visible = false;
                Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
            }

            // User not logged in
            if (UserId == -1)
            {
                // Must have the valid password
                if (Request.QueryString["TP"] != null)
                {
                    // Check the password for this Ticket
                    if (objADefHelpDesk_Tasks.TicketPassword == Convert.ToString(Request.QueryString["TP"]))
                    {
                        boolPassedSecurity = true;
                    }
                    else
                    {
                        boolPassedSecurity = false;
                    }
                }
            }

            // User is logged in
            if (UserId > -1)
            {
                // Is user an Admin?
                string strAdminRoleID = GetAdminRole();
                if (UserInfo.IsInRole(strAdminRoleID) || UserInfo.IsInRole("Administrators") || UserInfo.IsSuperUser)
                {
                    boolPassedSecurity = true;
                    CommentsControl.ViewOnly = false;
                }

                // Is user the Requestor?
                if (UserId == objADefHelpDesk_Tasks.RequesterUserID)
                {
                    boolPassedSecurity = true;
                }

                //Is user in the Assigned Role?
                RoleController objRoleController = new RoleController();
                RoleInfo objRoleInfo = objRoleController.GetRole(objADefHelpDesk_Tasks.AssignedRoleID, PortalId);
                if (objRoleInfo != null)
                {
                    if (UserInfo.IsInRole(objRoleInfo.RoleName))
                    {
                        boolPassedSecurity = true;
                        CommentsControl.ViewOnly = false;
                    }
                }
            }

            return boolPassedSecurity;
        }
        #endregion

        #region lnkAdministratorSettings_Click
        protected void lnkAdministratorSettings_Click(object sender, EventArgs e)
        {
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "AdminSettings", "mid=" + ModuleId.ToString()));
        }
        #endregion

        #region lnkNewTicket_Click
        protected void lnkNewTicket_Click(object sender, EventArgs e)
        {
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(null, "Ticket=new"));
        }
        #endregion

        #region lnkExistingTickets_Click
        protected void lnkExistingTickets_Click(object sender, EventArgs e)
        {
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(), true);
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
            }
            else
            {
                lnkExistingTickets.Visible = false;
                imgExitingTickets.Visible = false;
            }
        }
        #endregion

        #region ShowAdministratorLink
        private void ShowAdministratorLink()
        {
            // Get Admin Role
            string strAdminRoleID = GetAdminRole();
            // Show Admin link if user is an Administrator
            if (UserInfo.IsInRole(strAdminRoleID) || UserInfo.IsInRole("Administrators") || UserInfo.IsSuperUser)
            {
                lnkAdministratorSettings.Visible = true;
                imgAdministrator.Visible = true;
            }
            else
            {
                lnkAdministratorSettings.Visible = false;
                imgAdministrator.Visible = false;
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

            return colADefHelpDesk_Setting;
        }
        #endregion

        // Tags

        #region DisplayCategoryTree
        private void DisplayCategoryTree()
        {
            bool boolUserAssignedToTask = false;
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            ADefHelpDesk_Task objADefHelpDesk_Tasks = (from ADefHelpDesk_Tasks in objADefHelpDeskDALDataContext.ADefHelpDesk_Tasks
                                                       where ADefHelpDesk_Tasks.TaskID == Convert.ToInt32(Request.QueryString["TaskID"])
                                                       select ADefHelpDesk_Tasks).FirstOrDefault();

            //Is user in the Assigned Role?
            RoleController objRoleController = new RoleController();
            RoleInfo objRoleInfo = objRoleController.GetRole(objADefHelpDesk_Tasks.AssignedRoleID, PortalId);

            if (objRoleInfo != null)
            {
                if (UserInfo.IsInRole(objRoleInfo.RoleName))
                {
                    boolUserAssignedToTask = true;
                }
            }

            if (boolUserAssignedToTask || UserInfo.IsInRole(GetAdminRole()) || UserInfo.IsInRole("Administrators") || UserInfo.IsSuperUser)
            {
                // Show all Tags
                TagsTreeExistingTasks.Visible = true;
                TagsTreeExistingTasks.TagID = Convert.ToInt32(Request.QueryString["TaskID"]);
                TagsTreeExistingTasks.DisplayType = "Administrator";
                TagsTreeExistingTasks.Expand = false;
            }
            else
            {
                // Show only Visible Tags
                TagsTreeExistingTasks.Visible = true;
                TagsTreeExistingTasks.TagID = Convert.ToInt32(Request.QueryString["TaskID"]);
                TagsTreeExistingTasks.DisplayType = "Requestor";
                TagsTreeExistingTasks.Expand = false;
            }

            // Select Existing values
            if (objADefHelpDesk_Tasks.ADefHelpDesk_TaskCategories.Select(x => x.CategoryID).ToArray<int>().Count() > 0)
            {
                int[] ArrStrCategories = objADefHelpDesk_Tasks.ADefHelpDesk_TaskCategories.Select(x => x.CategoryID).ToArray<int>();
                int?[] ArrIntCatagories = Array.ConvertAll<int, int?>(ArrStrCategories, new Converter<int, int?>(ConvertToNullableInt));

                TagsTreeExistingTasks.SelectedCategories = ArrIntCatagories;
            }
        }
        #endregion

        #region ConvertToNullableInt
        private int? ConvertToNullableInt(int strParameter)
        {
            return Convert.ToInt32(strParameter);
        }
        #endregion

        // Display Ticket Data

        #region DisplayTicketData
        private void DisplayTicketData()
        {
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            ADefHelpDesk_Task objADefHelpDesk_Tasks = (from ADefHelpDesk_Tasks in objADefHelpDeskDALDataContext.ADefHelpDesk_Tasks
                                                       where ADefHelpDesk_Tasks.TaskID == Convert.ToInt32(Request.QueryString["TaskID"])
                                                       select ADefHelpDesk_Tasks).FirstOrDefault();

            // Name is editable only if user is Anonymous
            if (objADefHelpDesk_Tasks.RequesterUserID == -1)
            {
                txtEmail.Visible = true;
                txtName.Visible = true;
                lblEmail.Visible = false;
                lblName.Visible = false;
                txtEmail.Text = objADefHelpDesk_Tasks.RequesterEmail;
                txtName.Text = objADefHelpDesk_Tasks.RequesterName;
            }
            else
            {
                txtEmail.Visible = false;
                txtName.Visible = false;
                lblEmail.Visible = true;
                lblName.Visible = true;
                lblEmail.Text = UserController.GetUser(PortalId, objADefHelpDesk_Tasks.RequesterUserID, false).Email;
                lblName.Text = UserController.GetUser(PortalId, objADefHelpDesk_Tasks.RequesterUserID, false).DisplayName;
            }

            lblTask.Text = objADefHelpDesk_Tasks.TaskID.ToString();
            lblCreated.Text = String.Format("<b>Created:</b> {0} {1}", objADefHelpDesk_Tasks.CreatedDate.ToShortDateString(), objADefHelpDesk_Tasks.CreatedDate.ToShortTimeString());
            ddlStatus.SelectedValue = objADefHelpDesk_Tasks.Status;
            ddlPriority.SelectedValue = objADefHelpDesk_Tasks.Priority;
            txtDescription.Text = objADefHelpDesk_Tasks.Description;
            txtPhone.Text = objADefHelpDesk_Tasks.RequesterPhone;
            txtDueDate.Text = (objADefHelpDesk_Tasks.DueDate.HasValue) ? objADefHelpDesk_Tasks.DueDate.Value.ToShortDateString() : "";
            txtStart.Text = (objADefHelpDesk_Tasks.EstimatedStart.HasValue) ? objADefHelpDesk_Tasks.EstimatedStart.Value.ToShortDateString() : "";
            txtComplete.Text = (objADefHelpDesk_Tasks.EstimatedCompletion.HasValue) ? objADefHelpDesk_Tasks.EstimatedCompletion.Value.ToShortDateString() : "";
            txtEstimate.Text = (objADefHelpDesk_Tasks.EstimatedHours.HasValue) ? objADefHelpDesk_Tasks.EstimatedHours.Value.ToString() : "";

            ListItem TmpRoleListItem = ddlAssigned.Items.FindByValue(objADefHelpDesk_Tasks.AssignedRoleID.ToString());
            if (TmpRoleListItem == null)
            {
                // Value was not found so add it
                RoleController objRoleController = new RoleController();
                RoleInfo objRoleInfo = objRoleController.GetRole(objADefHelpDesk_Tasks.AssignedRoleID, PortalId);

                if (objRoleInfo != null)
                {
                    ListItem RoleListItem = new ListItem();
                    RoleListItem.Text = objRoleInfo.RoleName;
                    RoleListItem.Value = objADefHelpDesk_Tasks.AssignedRoleID.ToString();
                    ddlAssigned.Items.Add(RoleListItem);

                    ddlAssigned.SelectedValue = objADefHelpDesk_Tasks.AssignedRoleID.ToString();
                }
                else
                {
                    // Role no longer exists in Portal
                    ddlAssigned.SelectedValue = "-1";
                }
            }
            else
            {
                // The Value already exists so set it
                ddlAssigned.SelectedValue = objADefHelpDesk_Tasks.AssignedRoleID.ToString();
            }
        }
        #endregion

        // Save Form Data

        #region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateTicketForm())
                {
                    int intTaskID = SaveTicketForm();
                    SaveTags(intTaskID);
                    ShowUpdated();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
        #endregion

        #region ValidateTicketForm
        private bool ValidateTicketForm()
        {
            List<string> ColErrors = new List<string>();

            // Only validate Name and email if Ticket is not for a DNN user
            // lblName will be hidden if it is not a DNN user
            if (lblName.Visible == false)
            {
                if (txtName.Text.Trim().Length < 1)
                {
                    ColErrors.Add("Name is required");
                }

                if (txtEmail.Text.Trim().Length < 1)
                {
                    ColErrors.Add("Email is required");
                }
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
                    ColErrors.Add("Must use a valid Due date");
                }
            }

            if (txtStart.Text.Trim().Length > 1)
            {
                try
                {
                    DateTime tmpDate = Convert.ToDateTime(txtStart.Text.Trim());
                }
                catch
                {
                    ColErrors.Add("Must use a valid Start date");
                }
            }

            if (txtComplete.Text.Trim().Length > 1)
            {
                try
                {
                    DateTime tmpDate = Convert.ToDateTime(txtComplete.Text.Trim());
                }
                catch
                {
                    ColErrors.Add("Must use a valid Complete date");
                }
            }

            if (txtEstimate.Text.Trim().Length > 0)
            {
                try
                {
                    int tmpInt = Convert.ToInt32(txtEstimate.Text.Trim());
                }
                catch
                {
                    ColErrors.Add("Must use a valid number for Estimate Hours");
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

        #region SaveTicketForm
        private int SaveTicketForm()
        {
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            ADefHelpDesk_Task objADefHelpDesk_Task = (from ADefHelpDesk_Tasks in objADefHelpDeskDALDataContext.ADefHelpDesk_Tasks
                                                      where ADefHelpDesk_Tasks.TaskID == Convert.ToInt32(Request.QueryString["TaskID"])
                                                      select ADefHelpDesk_Tasks).FirstOrDefault();

            // Save original Assigned Group
            int intOriginalAssignedGroup = objADefHelpDesk_Task.AssignedRoleID; 

            // Save Task
            objADefHelpDesk_Task.Status = ddlStatus.SelectedValue;
            objADefHelpDesk_Task.Description = txtDescription.Text;
            objADefHelpDesk_Task.PortalID = PortalId;
            objADefHelpDesk_Task.Priority = ddlPriority.SelectedValue;
            objADefHelpDesk_Task.RequesterPhone = txtPhone.Text;
            objADefHelpDesk_Task.AssignedRoleID = Convert.ToInt32(ddlAssigned.SelectedValue);

            // Only validate Name and email if Ticket is not for a DNN user
            // lblName will be hidden if it is not a DNN user
            if (lblName.Visible == false)
            {
                // not a DNN user
                objADefHelpDesk_Task.RequesterEmail = txtEmail.Text;
                objADefHelpDesk_Task.RequesterName = txtName.Text;
                objADefHelpDesk_Task.RequesterUserID = -1;
            }

            // DueDate
            if (txtDueDate.Text.Trim().Length > 1)
            {
                objADefHelpDesk_Task.DueDate = Convert.ToDateTime(txtDueDate.Text.Trim());
            }
            else
            {
                objADefHelpDesk_Task.DueDate = null;
            }

            // EstimatedStart
            if (txtStart.Text.Trim().Length > 1)
            {
                objADefHelpDesk_Task.EstimatedStart = Convert.ToDateTime(txtStart.Text.Trim());
            }
            else
            {
                objADefHelpDesk_Task.EstimatedStart = null;
            }

            // EstimatedCompletion
            if (txtComplete.Text.Trim().Length > 1)
            {
                objADefHelpDesk_Task.EstimatedCompletion = Convert.ToDateTime(txtComplete.Text.Trim());
            }
            else
            {
                objADefHelpDesk_Task.EstimatedCompletion = null;
            }

            // EstimatedHours
            if (txtEstimate.Text.Trim().Length > 0)
            {
                objADefHelpDesk_Task.EstimatedHours = Convert.ToInt32(txtEstimate.Text.Trim());
            }
            else
            {
                objADefHelpDesk_Task.EstimatedHours = null;
            }

            objADefHelpDeskDALDataContext.SubmitChanges();

            // Notify Assigned Group
            if (Convert.ToInt32(ddlAssigned.SelectedValue) > -1)
            {
                // Only notify if Assigned group has changed
                if (intOriginalAssignedGroup != Convert.ToInt32(ddlAssigned.SelectedValue))
                {
                    NotifyAssignedGroupOfAssignment();                    
                }
            }

            // Insert Log
            Log.InsertLog(objADefHelpDesk_Task.TaskID, UserId, String.Format("{0} updated ticket.", UserInfo.DisplayName));

            return objADefHelpDesk_Task.TaskID;
        }
        #endregion

        #region SaveTags
        private void SaveTags(int intTaskID)
        {
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            var ExistingTaskCategories = from ADefHelpDesk_TaskCategories in objADefHelpDeskDALDataContext.ADefHelpDesk_TaskCategories
                                         where ADefHelpDesk_TaskCategories.TaskID == intTaskID
                                         select ADefHelpDesk_TaskCategories;

            // Delete all existing TaskCategories
            if (ExistingTaskCategories != null)
            {
                objADefHelpDeskDALDataContext.ADefHelpDesk_TaskCategories.DeleteAllOnSubmit(ExistingTaskCategories);
                objADefHelpDeskDALDataContext.SubmitChanges();
            }

            // Add TaskCategories
            TreeView objTreeView = (TreeView)TagsTreeExistingTasks.FindControl("tvCategories");
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

        #region ShowUpdated
        private void ShowUpdated()
        {
            lblError.Text = "** Updated **";

            // Provide a way for the user to see that a record has been updated
            // multiple times by changing the color each time
            lblError.ForeColor = (lblError.ForeColor == Color.Red) ? Color.Blue : Color.Red;

        }
        #endregion

        // Details

        #region DisableAllButtons
        private void DisableAllButtons()
        {
            btnComments.BorderStyle = BorderStyle.Outset;
            btnComments.BackColor = Color.WhiteSmoke;
            btnComments.Font.Bold = false;
            btnComments.ForeColor = Color.Black;
            pnlComments.Visible = false;

            btnWorkItems.BorderStyle = BorderStyle.Outset;
            btnWorkItems.BackColor = Color.WhiteSmoke;
            btnWorkItems.Font.Bold = false;
            btnWorkItems.ForeColor = Color.Black;
            pnlWorkItems.Visible = false;

            btnAssociations.BorderStyle = BorderStyle.Outset;
            btnAssociations.BackColor = Color.WhiteSmoke;
            btnAssociations.Font.Bold = false;
            btnAssociations.ForeColor = Color.Black;
            pnlAssociations.Visible = false;

            btnLogs.BorderStyle = BorderStyle.Outset;
            btnLogs.BackColor = Color.WhiteSmoke;
            btnLogs.Font.Bold = false;
            btnLogs.ForeColor = Color.Black;
            pnlLogs.Visible = false;
        }
        #endregion

        // Comments

        #region btnComments_Click
        protected void btnComments_Click(object sender, EventArgs e)
        {
            // If we are already on the Comments screen then switch Comments to Default mode
            if (pnlComments.Visible == true)
            {
                CommentsControl.SetView("Default");
            }

            DisableAllButtons();
            btnComments.BorderStyle = BorderStyle.Inset;
            btnComments.BackColor = Color.LightGray;
            btnComments.Font.Bold = true;
            btnComments.ForeColor = Color.Red;
            pnlComments.Visible = true;
        }
        #endregion

        // Work Items

        #region btnWorkItems_Click
        protected void btnWorkItems_Click(object sender, EventArgs e)
        {
            DisableAllButtons();
            btnWorkItems.BorderStyle = BorderStyle.Inset;
            btnWorkItems.BackColor = Color.LightGray;
            btnWorkItems.Font.Bold = true;
            btnWorkItems.ForeColor = Color.Red;
            pnlWorkItems.Visible = true;
        }
        #endregion

        // Associations

        #region btnAssociations_Click
        protected void btnAssociations_Click(object sender, EventArgs e)
        {
            DisableAllButtons();
            btnAssociations.BorderStyle = BorderStyle.Inset;
            btnAssociations.BackColor = Color.LightGray;
            btnAssociations.Font.Bold = true;
            btnAssociations.ForeColor = Color.Red;
            pnlAssociations.Visible = true;
        }
        #endregion

        // Emails

        #region NotifyAssignedGroupOfAssignment
        private void NotifyAssignedGroupOfAssignment()
        {
            RoleController objRoleController = new RoleController();
            string strAssignedRole = String.Format("{0}", objRoleController.GetRole(Convert.ToInt32(ddlAssigned.SelectedValue), PortalId).RoleName);

            string strSubject = String.Format("A Help Desk Ticket #{0} at http://{1} hass been assigned to {2}", Request.QueryString["TaskID"], PortalSettings.PortalAlias.HTTPAlias, strAssignedRole);
            string strBody = String.Format(@"A new help desk ticket #{0} has been Assigned '{1}'.", Request.QueryString["TaskID"], txtDescription.Text);
            strBody = strBody + Environment.NewLine;
            strBody = strBody + String.Format(@"You may see the status here: {0}", DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "EditTask", "mid=" + ModuleId.ToString(), String.Format(@"&TaskID={0}", Request.QueryString["TaskID"])));

            // Get all users in the AssignedRole Role
            ArrayList colAssignedRoleUsers = objRoleController.GetUsersByRoleName(PortalId, strAssignedRole);

            foreach (UserInfo objUserInfo in colAssignedRoleUsers)
            {
                DotNetNuke.Services.Mail.Mail.SendMail(PortalSettings.Email, objUserInfo.Email, "", strSubject, strBody, "", "HTML", "", "", "", "");
            }

            Log.InsertLog(Convert.ToInt32(Request.QueryString["TaskID"]), UserId, String.Format("{0} assigned ticket to {1}.", UserInfo.DisplayName, strAssignedRole));
        }
        #endregion

        // Logs

        #region btnLogs_Click
        protected void btnLogs_Click(object sender, EventArgs e)
        {
            DisableAllButtons();
            btnLogs.BorderStyle = BorderStyle.Inset;
            btnLogs.BackColor = Color.LightGray;
            btnLogs.Font.Bold = true;
            btnLogs.ForeColor = Color.Red;
            pnlLogs.Visible = true;

            LogsControl.RefreshLogs();
        }
        #endregion

    }
}