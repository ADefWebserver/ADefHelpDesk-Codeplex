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
using System.Collections;
using System.Drawing;
using Microsoft.VisualBasic;
using System.Text;
using System.IO;
using System.Web.UI;

namespace ADefWebserver.Modules.ADefHelpDesk
{
    public partial class Work : ModuleBase
    {

        #region Properties
        public int TaskID
        {
            get { return Convert.ToInt32(ViewState["TaskID"]); }
            set { ViewState["TaskID"] = value; }
        }

        public int ModuleID
        {
            get { return Convert.ToInt32(ViewState["ModuleID"]); }
            set { ViewState["ModuleID"] = value; }
        }

        public bool ViewOnly
        {
            get { return Convert.ToBoolean(ViewState["ViewOnly"]); }
            set { ViewState["ViewOnly"] = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            // Register JQuery Calendar JavaScript
            JQueryCalendarJavaScript1.Text = Utility.InsertCalendarJavaScript(txtStartDay.ClientID);
            JQueryCalendarJavaScript2.Text = Utility.InsertCalendarJavaScript(txtStopDay.ClientID);
            JQueryCalendarJavaScript3.Text = Utility.InsertCalendarJavaScript(txtStartDayEdit.ClientID);
            JQueryCalendarJavaScript4.Text = Utility.InsertCalendarJavaScript(txtStopDayEdit.ClientID);

                pnlInsertComment.GroupingText = GetLocalResourceObject("pnlInsertComment.Text").ToString();

                if (!Page.IsPostBack)
                {
                    // Insert Default dates and times
                    txtStartDay.Text = DateTime.Now.ToShortDateString();
                    txtStopDay.Text = DateTime.Now.ToShortDateString();
                    txtStartTime.Text = DateTime.Now.AddHours(-1).ToShortTimeString();
                    txtStopTime.Text = DateTime.Now.ToShortTimeString();

                    SetView("Default");

                    if (ViewOnly)
                    {
                        SetViewOnlyMode();
                    }
                }
        }

        #region SetView
        public void SetView(string ViewMode)
        {
            if (ViewMode == "Default")
            {
                pnlInsertComment.Visible = true;
                pnlTableHeader.Visible = true;
                pnlExistingComments.Visible = true;
                pnlEditComment.Visible = false;
            }

            if (ViewMode == "Edit")
            {
                pnlInsertComment.Visible = false;
                pnlTableHeader.Visible = false;
                pnlExistingComments.Visible = false;
                pnlEditComment.Visible = true;
            }
        }
        #endregion

        #region SetViewOnlyMode
        private void SetViewOnlyMode()
        {
            lnkDelete.Visible = false;
            Image5.Visible = false;
            lnkUpdate.Visible = false;
            Image4.Visible = false;
        }
        #endregion

        // Insert Comment

        #region btnInsertComment_Click
        protected void btnInsertComment_Click(object sender, EventArgs e)
        {
            InsertComment();
        }
        #endregion

        #region btnInsertCommentAndEmail_Click
        protected void btnInsertCommentAndEmail_Click(object sender, EventArgs e)
        {
            string strComment = txtComment.Text;
            InsertComment();
        }
        #endregion

        #region InsertComment
        private void InsertComment()
        {
            if (txtComment.Text.Trim().Length > 0)
            {
                try
                {
                    // Try to Make Start and Stop Time
                    DateTime StartTime = Convert.ToDateTime(String.Format("{0} {1}", txtStartDay.Text, txtStartTime.Text));
                    DateTime StopTime = Convert.ToDateTime(String.Format("{0} {1}", txtStopDay.Text, txtStopTime.Text));
                }
                catch
                {
                    lblError.Text = GetLocalResourceObject("MustProvideValidStarAndStopTimes.Text").ToString();
                    return;
                }

                dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

                string strComment = txtComment.Text.Trim();

                // Save Task Details
                ADefHelpDesk_TaskDetail objADefHelpDesk_TaskDetail = new ADefHelpDesk_TaskDetail();

                objADefHelpDesk_TaskDetail.TaskID = TaskID;
                objADefHelpDesk_TaskDetail.Description = txtComment.Text.Trim();
                objADefHelpDesk_TaskDetail.InsertDate = DateTime.Now;
                objADefHelpDesk_TaskDetail.UserID = UserId;
                objADefHelpDesk_TaskDetail.DetailType = "Work";
                objADefHelpDesk_TaskDetail.StartTime = Convert.ToDateTime(String.Format("{0} {1}", txtStartDay.Text, txtStartTime.Text));
                objADefHelpDesk_TaskDetail.StopTime = Convert.ToDateTime(String.Format("{0} {1}", txtStopDay.Text, txtStopTime.Text));

                objdnnHelpDeskDALDataContext.ADefHelpDesk_TaskDetails.InsertOnSubmit(objADefHelpDesk_TaskDetail);
                objdnnHelpDeskDALDataContext.SubmitChanges();
                txtComment.Text = "";

                // Insert Log
                Log.InsertLog(TaskID, UserId, String.Format("{0} inserted Work comment.", GetUserName()));

                gvComments.DataBind();
            }
            else
            {
                lblError.Text = GetLocalResourceObject("MustProvideADescription.Text").ToString();
            }
        }
        #endregion

        #region LDSComments_Selecting
        protected void LDSComments_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();
            var result = from ADefHelpDesk_TaskDetails in objdnnHelpDeskDALDataContext.ADefHelpDesk_TaskDetails
                         where ADefHelpDesk_TaskDetails.TaskID == TaskID
                         where (ADefHelpDesk_TaskDetails.DetailType == "Work")
                         select ADefHelpDesk_TaskDetails;

            e.Result = result;
        }
        #endregion

        #region gvComments_RowDataBound
        protected void gvComments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridViewRow objGridViewRow = (GridViewRow)e.Row;

                // Comment
                Label lblComment = (Label)objGridViewRow.FindControl("lblComment");
                if (lblComment.Text.Trim().Length > 100)
                {
                    lblComment.Text = String.Format("{0}...", Strings.Left(lblComment.Text, 100));
                }

                // User
                Label gvlblUser = (Label)objGridViewRow.FindControl("gvlblUser");
                if (gvlblUser.Text != "-1")
                {
                    UserInfo objUser = UserController.GetUser(PortalId, Convert.ToInt32(gvlblUser.Text), false);

                    if (objUser != null)
                    {
                        string strDisplayName = objUser.DisplayName;

                        if (strDisplayName.Length > 25)
                        {
                            gvlblUser.Text = String.Format("{0}...", Strings.Left(strDisplayName, 25));
                        }
                        else
                        {
                            gvlblUser.Text = strDisplayName;
                        }
                    }
                    else
                    {
                        gvlblUser.Text = "[User Deleted]";
                    }
                }
                else
                {
                    gvlblUser.Text = GetLocalResourceObject("Requestor.Text").ToString();
                }


                // Time
                Label lblTimeSpan = (Label)objGridViewRow.FindControl("lblTimeSpan");
                try
                {

                    Label lblStartTime = (Label)objGridViewRow.FindControl("lblStartTime");
                    Label lblStopTime = (Label)objGridViewRow.FindControl("lblStopTime");

                    DateTime StartDate = Convert.ToDateTime(lblStartTime.Text);
                    DateTime StopDate = Convert.ToDateTime(lblStopTime.Text);
                    TimeSpan TimeDifference = StopDate.Subtract(StartDate);

                    // if no Days
                    if (TimeDifference.Days == 0)
                    {
                        if (TimeDifference.Hours == 0)
                        {
                            lblTimeSpan.Text = String.Format(GetLocalResourceObject("Minute.Text").ToString(), TimeDifference.Minutes.ToString(), ((TimeDifference.Minutes > 1) ? "s" : ""));
                        }
                        else
                        {
                            lblTimeSpan.Text = String.Format(GetLocalResourceObject("HoursandMinute.Text").ToString(), TimeDifference.Hours.ToString(), TimeDifference.Minutes.ToString(), ((TimeDifference.Minutes > 1) ? "s" : ""));
                        }
                    }
                    else
                    {
                        lblTimeSpan.Text = String.Format(GetLocalResourceObject("DaysHoursMinutes.Text").ToString(), TimeDifference.Days.ToString(), ((TimeDifference.Days > 1) ? "s" : ""), TimeDifference.Hours.ToString(), TimeDifference.Minutes.ToString(), ((TimeDifference.Minutes > 1) ? "s" : ""));
                    }
                }
                catch (Exception ex)
                {
                    lblTimeSpan.Text = ex.Message;
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
            string strUserName = GetLocalResourceObject("Anonymous.Text").ToString();

            if (UserId > -1)
            {
                UserInfo objUserInfo = UserController.GetUser(PortalId, UserId, false);
                strUserName = objUserInfo.DisplayName;
            }

            return strUserName;
        }

        private string GetUserName(int intUserID)
        {
            string strUserName = GetLocalResourceObject("Anonymous.Text").ToString();

            if (intUserID > -1)
            {
                UserInfo objUser = UserController.GetUser(PortalId, intUserID, false);

                if (objUser != null)
                {
                    strUserName = objUser.DisplayName;
                }
                else
                {
                    strUserName = GetLocalResourceObject("Anonymous.Text").ToString();
                }
            }

            return strUserName;
        }
        #endregion

        // GridView

        #region gvComments_RowCommand
        protected void gvComments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                SetView("Edit");
                lblDetailID.Text = Convert.ToString(e.CommandArgument);
                DisplayComment();
            }
        }
        #endregion

        // Comment Edit

        #region lnkBack_Click
        protected void lnkBack_Click(object sender, EventArgs e)
        {
            SetView("Default");
        }
        #endregion

        #region DisplayComment
        private void DisplayComment()
        {
            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            var objADefHelpDesk_TaskDetail = (from ADefHelpDesk_TaskDetails in objdnnHelpDeskDALDataContext.ADefHelpDesk_TaskDetails
                                              where ADefHelpDesk_TaskDetails.DetailID == Convert.ToInt32(lblDetailID.Text)
                                              select ADefHelpDesk_TaskDetails).FirstOrDefault();

            if (objADefHelpDesk_TaskDetail != null)
            {
                txtDescription.Text = objADefHelpDesk_TaskDetail.Description;
                lblDisplayUser.Text = GetUserName(objADefHelpDesk_TaskDetail.UserID);
                txtStartDayEdit.Text = objADefHelpDesk_TaskDetail.StartTime.Value.ToShortDateString();
                txtStopDayEdit.Text = objADefHelpDesk_TaskDetail.StopTime.Value.ToShortDateString();
                txtStartTimeEdit.Text = objADefHelpDesk_TaskDetail.StartTime.Value.ToShortTimeString();
                txtStopTimeEdit.Text = objADefHelpDesk_TaskDetail.StopTime.Value.ToShortTimeString();
                lblInsertDate.Text = String.Format("{0} {1}", objADefHelpDesk_TaskDetail.InsertDate.ToLongDateString(), objADefHelpDesk_TaskDetail.InsertDate.ToLongTimeString());
            }
        }
        #endregion

        #region lnkDelete_Click
        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            var objADefHelpDesk_TaskDetail = (from ADefHelpDesk_TaskDetails in objdnnHelpDeskDALDataContext.ADefHelpDesk_TaskDetails
                                              where ADefHelpDesk_TaskDetails.DetailID == Convert.ToInt32(lblDetailID.Text)
                                              select ADefHelpDesk_TaskDetails).FirstOrDefault();

            // Delete the Record
            objdnnHelpDeskDALDataContext.ADefHelpDesk_TaskDetails.DeleteOnSubmit(objADefHelpDesk_TaskDetail);
            objdnnHelpDeskDALDataContext.SubmitChanges();

            // Insert Log
            Log.InsertLog(TaskID, UserId, String.Format("{0} deleted Work comment: {1}", GetUserName(), txtDescription.Text));

            SetView("Default");
            gvComments.DataBind();
        }
        #endregion

        #region lnkUpdate_Click
        protected void lnkUpdate_Click(object sender, EventArgs e)
        {
            UpdateComment();
        }
        #endregion

        #region UpdateComment
        private void UpdateComment()
        {
            try
            {
                // Try to Make Start and Stop Time
                DateTime StartTime = Convert.ToDateTime(String.Format("{0} {1}", txtStartDayEdit.Text, txtStartTimeEdit.Text));
                DateTime StopTime = Convert.ToDateTime(String.Format("{0} {1}", txtStopDayEdit.Text, txtStopTimeEdit.Text));
            }
            catch
            {
                lblErrorEditComment.Text = GetLocalResourceObject("MustProvideValidStarAndStopTimes.Text").ToString();
                return;
            }

            if (txtDescription.Text.Trim().Length > 0)
            {
                dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

                string strComment = txtDescription.Text.Trim();

                // Save Task Details
                var objADefHelpDesk_TaskDetail = (from ADefHelpDesk_TaskDetails in objdnnHelpDeskDALDataContext.ADefHelpDesk_TaskDetails
                                                  where ADefHelpDesk_TaskDetails.DetailID == Convert.ToInt32(lblDetailID.Text)
                                                  select ADefHelpDesk_TaskDetails).FirstOrDefault();

                if (objADefHelpDesk_TaskDetail != null)
                {

                    objADefHelpDesk_TaskDetail.TaskID = TaskID;
                    objADefHelpDesk_TaskDetail.Description = txtDescription.Text.Trim();
                    objADefHelpDesk_TaskDetail.UserID = UserId;
                    objADefHelpDesk_TaskDetail.DetailType = "Work";
                    objADefHelpDesk_TaskDetail.StartTime = Convert.ToDateTime(String.Format("{0} {1}", txtStartDayEdit.Text, txtStartTimeEdit.Text));
                    objADefHelpDesk_TaskDetail.StopTime = Convert.ToDateTime(String.Format("{0} {1}", txtStopDayEdit.Text, txtStopTimeEdit.Text));

                    objdnnHelpDeskDALDataContext.SubmitChanges();
                    txtDescription.Text = "";

                    // Insert Log
                    Log.InsertLog(TaskID, UserId, String.Format("{0} updated Work comment.", GetUserName()));

                    SetView("Default");
                    gvComments.DataBind();
                }
            }
            else
            {
                lblErrorEditComment.Text = GetLocalResourceObject("MustProvideADescription.Text").ToString();
            }
        }
        #endregion

        // Utility

        #region GetAssignedRole
        private int GetAssignedRole()
        {
            int intRole = -1;

            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();
            var result = from ADefHelpDesk_TaskDetails in objdnnHelpDeskDALDataContext.ADefHelpDesk_Tasks
                         where ADefHelpDesk_TaskDetails.TaskID == Convert.ToInt32(Request.QueryString["TaskID"])
                         select ADefHelpDesk_TaskDetails;

            if (result != null)
            {
                intRole = result.FirstOrDefault().AssignedRoleID;
            }

            return intRole;
        }
        #endregion

        #region GetDescriptionOfTicket
        private string GetDescriptionOfTicket()
        {
            string strDescription = "";
            int intTaskId = Convert.ToInt32(Request.QueryString["TaskID"]);

            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();
            var result = (from ADefHelpDesk_TaskDetails in objdnnHelpDeskDALDataContext.ADefHelpDesk_Tasks
                          where ADefHelpDesk_TaskDetails.TaskID == Convert.ToInt32(Request.QueryString["TaskID"])
                          select ADefHelpDesk_TaskDetails).FirstOrDefault();

            if (result != null)
            {
                strDescription = result.Description;
            }

            return strDescription;
        }
        #endregion

        #region GetSettings
        private List<ADefHelpDesk_Setting> GetSettings()
        {
            // Get Settings
            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            List<ADefHelpDesk_Setting> colADefHelpDesk_Setting = (from ADefHelpDesk_Settings in objdnnHelpDeskDALDataContext.ADefHelpDesk_Settings
                                                                  where ADefHelpDesk_Settings.PortalID == PortalId
                                                                  select ADefHelpDesk_Settings).ToList();

            if (colADefHelpDesk_Setting.Count == 0)
            {
                // Create Default vaules
                ADefHelpDesk_Setting objADefHelpDesk_Setting1 = new ADefHelpDesk_Setting();

                objADefHelpDesk_Setting1.PortalID = PortalId;
                objADefHelpDesk_Setting1.SettingName = "AdminRole";
                objADefHelpDesk_Setting1.SettingValue = "Administrators";

                objdnnHelpDeskDALDataContext.ADefHelpDesk_Settings.InsertOnSubmit(objADefHelpDesk_Setting1);
                objdnnHelpDeskDALDataContext.SubmitChanges();

                ADefHelpDesk_Setting objADefHelpDesk_Setting2 = new ADefHelpDesk_Setting();

                objADefHelpDesk_Setting2.PortalID = PortalId;
                objADefHelpDesk_Setting2.SettingName = "FileUploadPath";
                objADefHelpDesk_Setting2.SettingValue = Server.MapPath("~/DesktopModules/ADefHelpDesk/Upload");

                objdnnHelpDeskDALDataContext.ADefHelpDesk_Settings.InsertOnSubmit(objADefHelpDesk_Setting2);
                objdnnHelpDeskDALDataContext.SubmitChanges();

                colADefHelpDesk_Setting = (from ADefHelpDesk_Settings in objdnnHelpDeskDALDataContext.ADefHelpDesk_Settings
                                           where ADefHelpDesk_Settings.PortalID == PortalId
                                           select ADefHelpDesk_Settings).ToList();
            }

            // Upload Permission
            ADefHelpDesk_Setting UploadPermissionADefHelpDesk_Setting = (from ADefHelpDesk_Settings in objdnnHelpDeskDALDataContext.ADefHelpDesk_Settings
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
                objdnnHelpDeskDALDataContext.ADefHelpDesk_Settings.InsertOnSubmit(objADefHelpDesk_Setting);
                objdnnHelpDeskDALDataContext.SubmitChanges();

                // Add to collection
                colADefHelpDesk_Setting.Add(objADefHelpDesk_Setting);
            }

            return colADefHelpDesk_Setting;
        }
        #endregion

        #region GetAdminRole
        private string GetAdminRole()
        {
            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            List<ADefHelpDesk_Setting> colADefHelpDesk_Setting = (from ADefHelpDesk_Settings in objdnnHelpDeskDALDataContext.ADefHelpDesk_Settings
                                                                  where ADefHelpDesk_Settings.PortalID == PortalId
                                                                  select ADefHelpDesk_Settings).ToList();

            ADefHelpDesk_Setting objADefHelpDesk_Setting = colADefHelpDesk_Setting.Where(x => x.SettingName == "AdminRole").FirstOrDefault();

            string strAdminRoleID = "Administrators";
            if (objADefHelpDesk_Setting != null)
            {
                strAdminRoleID = objADefHelpDesk_Setting.SettingValue;
            }

            return strAdminRoleID;
        }
        #endregion
    }
}