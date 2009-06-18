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
using DotNetNuke.Services.Exceptions;

namespace ADefWebserver.Modules.ADefHelpDesk
{
    public partial class Comments : DotNetNuke.Entities.Modules.PortalModuleBase
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
            if (!Page.IsPostBack)
            {
                SetView("Default");

                if (ViewOnly)
                {
                    SetViewOnlyMode();                    
                }

                ShowFileUpload();
            }
        }

        #region ShowFileUpload
        private void ShowFileUpload()
        {
            string strAdminRoleID = GetAdminRole();
            if (!(UserInfo.IsInRole(strAdminRoleID) || UserInfo.IsInRole("Administrators") || UserInfo.IsSuperUser))
            {
                string strUploadPermission = GetUploadPermission();

                // Only supress Upload if permission is not set to All              
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
                            lblAttachFile1.Visible = false;
                            TicketFileUpload.Visible = false;
                            lblAttachFile2.Visible = false;
                            fuAttachment.Visible = false;
                        }
                        #endregion
                    }
                    else
                    {
                        // If User is not logged in they cannot see upload
                        lblAttachFile1.Visible = false;
                        TicketFileUpload.Visible = false;
                        lblAttachFile2.Visible = false;
                        fuAttachment.Visible = false;
                    }
                }
            }
        } 
        #endregion

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
            chkCommentVisible.Visible = false;
            chkCommentVisibleEdit.Visible = false;
            lnkDelete.Visible = false;
            Image5.Visible = false;
            lnkUpdate.Visible = false;
            Image4.Visible = false;
            pnlDisplayFile.Visible = false;
            pnlAttachFile.Visible = false;
            imgDelete.Visible = false;
            lnkUpdateRequestor.Visible = false;
            ImgEmailUser.Visible = false;
            btnInsertCommentAndEmail.Visible = false;
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
            NotifyRequestorOfComment(strComment);
        }
        #endregion

        #region InsertComment
        private void InsertComment()
        {
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
                    lblError.Text = "Only .gif, .jpg, .jpeg, .doc, .docx, .xls, .xlsx, .pdf files may be used.";
                    return;
                }
            }

            if (txtComment.Text.Trim().Length > 0)
            {
                ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

                string strComment = txtComment.Text.Trim();

                // Save Task Details
                ADefHelpDesk_TaskDetail objADefHelpDesk_TaskDetail = new ADefHelpDesk_TaskDetail();

                objADefHelpDesk_TaskDetail.TaskID = TaskID;
                objADefHelpDesk_TaskDetail.Description = txtComment.Text.Trim();
                objADefHelpDesk_TaskDetail.InsertDate = DateTime.Now;
                objADefHelpDesk_TaskDetail.UserID = UserId;

                if (chkCommentVisible.Checked)
                {
                    objADefHelpDesk_TaskDetail.DetailType = "Comment-Visible";
                }
                else
                {
                    objADefHelpDesk_TaskDetail.DetailType = "Comment";
                }

                objADefHelpDeskDALDataContext.ADefHelpDesk_TaskDetails.InsertOnSubmit(objADefHelpDesk_TaskDetail);
                objADefHelpDeskDALDataContext.SubmitChanges();
                txtComment.Text = "";

                // Insert Log
                Log.InsertLog(TaskID, UserId, String.Format("{0} inserted comment.", GetUserName()));

                // Upload the File
                if (TicketFileUpload.HasFile)
                {
                    UploadFile(objADefHelpDesk_TaskDetail.DetailID);
                    // Insert Log
                    Log.InsertLog(TaskID, UserId, String.Format("{0} uploaded file '{1}'.", GetUserName(), TicketFileUpload.FileName));
                }

                if (UserIsRequestor())
                {
                    NotifyAssignedGroupOfComment(strComment);
                }

                gvComments.DataBind();
            }
        }
        #endregion

        #region LDSComments_Selecting
        protected void LDSComments_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();
            var result = from ADefHelpDesk_TaskDetails in objADefHelpDeskDALDataContext.ADefHelpDesk_TaskDetails
                         where ADefHelpDesk_TaskDetails.TaskID == TaskID
                         where (ADefHelpDesk_TaskDetails.DetailType == "Comment" || ADefHelpDesk_TaskDetails.DetailType == "Comment-Visible")
                         select ADefHelpDesk_TaskDetails;

            // If View only mode
            if (ViewOnly)
            {
                result = from TaskDetails in result
                         where TaskDetails.DetailType == "Comment-Visible"
                         select TaskDetails;
            }

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
                    string strDisplayName = UserController.GetUser(PortalId, Convert.ToInt32(gvlblUser.Text), false).DisplayName;

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
                    gvlblUser.Text = "<i>Requestor</i>";
                }

                // Comment Visible checkbox
                CheckBox chkDetailType = (CheckBox)objGridViewRow.FindControl("chkDetailType");
                Label lblDetailType = (Label)objGridViewRow.FindControl("lblDetailType");
                // lblDetailType
                chkDetailType.Checked = (lblDetailType.Text == "Comment") ? false : true;
            }
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
                strUserName = UserInfo.DisplayName;
            }

            return strUserName;
        }

        private string GetUserName(int intUserID)
        {
            string strUserName = "Anonymous";

            if (intUserID > -1)
            {
                strUserName = UserController.GetUser(PortalId, intUserID, false).DisplayName;
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
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            var objADefHelpDesk_TaskDetail = (from ADefHelpDesk_TaskDetails in objADefHelpDeskDALDataContext.ADefHelpDesk_TaskDetails
                                              where ADefHelpDesk_TaskDetails.DetailID == Convert.ToInt32(lblDetailID.Text)
                                              select ADefHelpDesk_TaskDetails).FirstOrDefault();

            if (objADefHelpDesk_TaskDetail != null)
            {
                txtDescription.Text = objADefHelpDesk_TaskDetail.Description;
                lblDisplayUser.Text = GetUserName(objADefHelpDesk_TaskDetail.UserID);
                lblInsertDate.Text = String.Format("{0} {1}", objADefHelpDesk_TaskDetail.InsertDate.ToLongDateString(), objADefHelpDesk_TaskDetail.InsertDate.ToLongTimeString());
                chkCommentVisibleEdit.Checked = (objADefHelpDesk_TaskDetail.DetailType == "Comment") ? false : true;

                // Only set the Display of the Email to Requestor link if it is already showing
                if (lnkUpdateRequestor.Visible)
                {
                    // Only Display Email to Requestor link if chkCommentVisibleEdit is checked
                    lnkUpdateRequestor.Visible = chkCommentVisibleEdit.Checked;
                    ImgEmailUser.Visible = chkCommentVisibleEdit.Checked;
                }

                if (objADefHelpDesk_TaskDetail.ADefHelpDesk_Attachments.Count > 0)
                {
                    // There is a atachment
                    pnlAttachFile.Visible = false;
                    pnlDisplayFile.Visible = true;

                    lnkFileAttachment.Text = objADefHelpDesk_TaskDetail.ADefHelpDesk_Attachments.FirstOrDefault().OriginalFileName;
                    lnkFileAttachment.CommandArgument = objADefHelpDesk_TaskDetail.ADefHelpDesk_Attachments.FirstOrDefault().AttachmentID.ToString();
                }
                else
                {
                    // Only do this if not in View Only Mode
                    if (!ViewOnly)
                    {
                        // There is not a file attached
                        pnlAttachFile.Visible = true;
                        pnlDisplayFile.Visible = false;
                    }
                    else
                    {
                        pnlDisplayFile.Visible = false;
                    }
                }
            }
        }
        #endregion

        #region lnkFileAttachment_Click
        protected void lnkFileAttachment_Click(object sender, EventArgs e)
        {
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            var objADefHelpDesk_Attachment = (from ADefHelpDesk_Attachments in objADefHelpDeskDALDataContext.ADefHelpDesk_Attachments
                                              where ADefHelpDesk_Attachments.AttachmentID == Convert.ToInt32(lnkFileAttachment.CommandArgument)
                                              select ADefHelpDesk_Attachments).FirstOrDefault();

            if (objADefHelpDesk_Attachment != null)
            {
                string strPath = objADefHelpDesk_Attachment.AttachmentPath;
                string strOriginalFileName = objADefHelpDesk_Attachment.OriginalFileName;

                try
                {
                    Response.ClearHeaders();
                    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", strOriginalFileName));

                    Response.ClearContent();
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    Response.ContentType = GetContentType(strPath);

                    FileStream sourceFile = new FileStream(strPath, FileMode.Open);
                    long FileSize;
                    FileSize = sourceFile.Length;
                    byte[] getContent = new byte[(int)FileSize];
                    sourceFile.Read(getContent, 0, (int)sourceFile.Length);
                    sourceFile.Close();

                    Response.BinaryWrite(getContent);
                    Response.Flush();
                    Response.Close();

                }
                catch
                {
                }
            }
        }
        #endregion

        #region GetContentType
        public string GetContentType(string strextension)
        {
            string contentType;
            switch (strextension.ToLower())
            {
                case ".gif":
                    contentType = "image/gif";
                    break;
                case ".jpg":
                    contentType = "image/jpeg";
                    break;
                case ".png":
                    contentType = "image/png";
                    break;
                case ".doc":
                    contentType = "application/ms-word";
                    break;
                case ".docx":
                    contentType = "application/vnd.ms-word.document.12";
                    break;
                case ".pdf":
                    contentType = "application/pdf";
                    break;
                case ".xls":
                    contentType = "application/vnd.ms-excel";
                    break;
                case ".ppt":
                    contentType = "application/vnd.ms-powerpoint";
                    break;
                case ".zip":
                    contentType = "application/zip";
                    break;
                case ".txt":
                    contentType = "text/plain";
                    break;
                default:
                    contentType = "application/octet-stream";
                    break;
            }
            return contentType;
        }
        #endregion

        #region lnkDelete_Click
        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            var objADefHelpDesk_TaskDetail = (from ADefHelpDesk_TaskDetails in objADefHelpDeskDALDataContext.ADefHelpDesk_TaskDetails
                                              where ADefHelpDesk_TaskDetails.DetailID == Convert.ToInt32(lblDetailID.Text)
                                              select ADefHelpDesk_TaskDetails).FirstOrDefault();

            // Delete any Attachments
            if (objADefHelpDesk_TaskDetail.ADefHelpDesk_Attachments.Count > 0)
            {
                ADefHelpDesk_Attachment objADefHelpDesk_Attachment = objADefHelpDesk_TaskDetail.ADefHelpDesk_Attachments.FirstOrDefault();
                string strOriginalFileName = objADefHelpDesk_Attachment.OriginalFileName;
                string strFile = objADefHelpDesk_Attachment.AttachmentPath;

                try
                {
                    // Delete file
                    if (strFile != "")
                    {
                        File.Delete(strFile);
                    }
                }
                catch (Exception exc)
                {
                    Exceptions.ProcessModuleLoadException(this, exc);
                }

                objADefHelpDeskDALDataContext.ADefHelpDesk_Attachments.DeleteOnSubmit(objADefHelpDesk_Attachment);
                objADefHelpDeskDALDataContext.SubmitChanges();

                // Insert Log
                Log.InsertLog(TaskID, UserId, String.Format("{0} deleted file '{1}'.", GetUserName(), strOriginalFileName));
            }

            // Delete the Record
            objADefHelpDeskDALDataContext.ADefHelpDesk_TaskDetails.DeleteOnSubmit(objADefHelpDesk_TaskDetail);
            objADefHelpDeskDALDataContext.SubmitChanges();

            // Insert Log
            Log.InsertLog(TaskID, UserId, String.Format("{0} deleted comment: {1}", GetUserName(), txtDescription.Text));

            SetView("Default");
            gvComments.DataBind();
        }
        #endregion

        #region imgDelete_Click
        protected void imgDelete_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            var objADefHelpDesk_TaskDetail = (from ADefHelpDesk_TaskDetails in objADefHelpDeskDALDataContext.ADefHelpDesk_TaskDetails
                                              where ADefHelpDesk_TaskDetails.DetailID == Convert.ToInt32(lblDetailID.Text)
                                              select ADefHelpDesk_TaskDetails).FirstOrDefault();

            // Delete Attachment
            if (objADefHelpDesk_TaskDetail.ADefHelpDesk_Attachments.Count > 0)
            {
                ADefHelpDesk_Attachment objADefHelpDesk_Attachment = objADefHelpDesk_TaskDetail.ADefHelpDesk_Attachments.FirstOrDefault();
                string strOriginalFileName = objADefHelpDesk_Attachment.OriginalFileName;
                string strFile = objADefHelpDesk_Attachment.AttachmentPath;

                try
                {
                    // Delete file
                    if (strFile != "")
                    {
                        File.Delete(strFile);
                    }
                }
                catch (Exception exc)
                {
                    Exceptions.ProcessModuleLoadException(this, exc);
                }

                objADefHelpDeskDALDataContext.ADefHelpDesk_Attachments.DeleteOnSubmit(objADefHelpDesk_Attachment);
                objADefHelpDeskDALDataContext.SubmitChanges();

                // Insert Log
                Log.InsertLog(TaskID, UserId, String.Format("{0} deleted file '{1}'.", GetUserName(), strOriginalFileName));

                pnlAttachFile.Visible = true;
                pnlDisplayFile.Visible = false;
            }
        }
        #endregion

        #region lnkUpdate_Click
        protected void lnkUpdate_Click(object sender, EventArgs e)
        {
            UpdateComment();
        }
        #endregion

        #region lnkUpdateRequestor_Click
        protected void lnkUpdateRequestor_Click(object sender, EventArgs e)
        {
            string strComment = txtDescription.Text;
            UpdateComment();
            NotifyRequestorOfComment(strComment);
        }
        #endregion

        #region UpdateComment
        private void UpdateComment()
        {
            // Validate file upload
            if (fuAttachment.HasFile)
            {
                if (
                    string.Compare(Path.GetExtension(fuAttachment.FileName).ToLower(), ".gif", true) != 0
                    & string.Compare(Path.GetExtension(fuAttachment.FileName).ToLower(), ".jpg", true) != 0
                    & string.Compare(Path.GetExtension(fuAttachment.FileName).ToLower(), ".jpeg", true) != 0
                    & string.Compare(Path.GetExtension(fuAttachment.FileName).ToLower(), ".doc", true) != 0
                    & string.Compare(Path.GetExtension(fuAttachment.FileName).ToLower(), ".docx", true) != 0
                    & string.Compare(Path.GetExtension(fuAttachment.FileName).ToLower(), ".xls", true) != 0
                    & string.Compare(Path.GetExtension(fuAttachment.FileName).ToLower(), ".xlsx", true) != 0
                    & string.Compare(Path.GetExtension(fuAttachment.FileName).ToLower(), ".pdf", true) != 0
                    )
                {
                    lblErrorEditComment.Text = "Only .gif, .jpg, .jpeg, .doc, .docx, .xls, .xlsx, .pdf files may be used.";
                    return;
                }
            }

            if (txtDescription.Text.Trim().Length > 0)
            {
                ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

                string strComment = txtDescription.Text.Trim();

                // Save Task Details
                var objADefHelpDesk_TaskDetail = (from ADefHelpDesk_TaskDetails in objADefHelpDeskDALDataContext.ADefHelpDesk_TaskDetails
                                                  where ADefHelpDesk_TaskDetails.DetailID == Convert.ToInt32(lblDetailID.Text)
                                                  select ADefHelpDesk_TaskDetails).FirstOrDefault();

                if (objADefHelpDesk_TaskDetail != null)
                {

                    objADefHelpDesk_TaskDetail.TaskID = TaskID;
                    objADefHelpDesk_TaskDetail.Description = txtDescription.Text.Trim();
                    objADefHelpDesk_TaskDetail.UserID = UserId;

                    if (chkCommentVisibleEdit.Checked)
                    {
                        objADefHelpDesk_TaskDetail.DetailType = "Comment-Visible";
                    }
                    else
                    {
                        objADefHelpDesk_TaskDetail.DetailType = "Comment";
                    }

                    objADefHelpDeskDALDataContext.SubmitChanges();
                    txtDescription.Text = "";

                    // Insert Log
                    Log.InsertLog(TaskID, UserId, String.Format("{0} updated comment.", GetUserName()));

                    // Upload the File
                    if (fuAttachment.HasFile)
                    {
                        UploadFileCommentEdit(objADefHelpDesk_TaskDetail.DetailID);
                    }

                    SetView("Default");
                    gvComments.DataBind();
                }
            }
        }
        #endregion

        #region UploadFileCommentEdit
        private void UploadFileCommentEdit(int intDetailID)
        {
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            string strUploadefFilesPath = (from ADefHelpDesk_Settings in objADefHelpDeskDALDataContext.ADefHelpDesk_Settings
                                           where ADefHelpDesk_Settings.PortalID == PortalId
                                           where ADefHelpDesk_Settings.SettingName == "UploadefFilesPath"
                                           select ADefHelpDesk_Settings).FirstOrDefault().SettingValue;

            EnsureDirectory(new System.IO.DirectoryInfo(strUploadefFilesPath));
            string strfilename = Convert.ToString(intDetailID) + "_" + GetRandomPassword() + Path.GetExtension(fuAttachment.FileName).ToLower();
            strUploadefFilesPath = strUploadefFilesPath + @"\" + strfilename;
            fuAttachment.SaveAs(strUploadefFilesPath);

            ADefHelpDesk_Attachment objADefHelpDesk_Attachment = new ADefHelpDesk_Attachment();
            objADefHelpDesk_Attachment.DetailID = intDetailID;
            objADefHelpDesk_Attachment.FileName = strfilename;
            objADefHelpDesk_Attachment.OriginalFileName = fuAttachment.FileName;
            objADefHelpDesk_Attachment.AttachmentPath = strUploadefFilesPath;
            objADefHelpDesk_Attachment.UserID = UserId;

            objADefHelpDeskDALDataContext.ADefHelpDesk_Attachments.InsertOnSubmit(objADefHelpDesk_Attachment);
            objADefHelpDeskDALDataContext.SubmitChanges();

            // Insert Log
            Log.InsertLog(TaskID, UserId, String.Format("{0} uploaded file '{1}'.", GetUserName(), fuAttachment.FileName));
        }
        #endregion

        // Emails

        #region NotifyAssignedGroupOfComment
        private void NotifyAssignedGroupOfComment(string strComment)
        {
            RoleController objRoleController = new RoleController();
            string strDescription = GetDescriptionOfTicket();

            // Send to Administrator Role
            string strAssignedRole = "Administrators";
            int intRole = GetAssignedRole();
            if (intRole > -1)
            {
                strAssignedRole = String.Format("{0}", objRoleController.GetRole(intRole, PortalId).RoleName);                
            }
            else
            {
                strAssignedRole = GetAdminRole();
            }

            string strLinkUrl = Utility.FixURLLink(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "EditTask", "mid=" + ModuleID.ToString(), String.Format(@"&TaskID={0}", TaskID)), PortalSettings.PortalAlias.HTTPAlias);

            string strSubject = String.Format("Help Desk Ticket #{0} at http://{1} has been updated", Request.QueryString["TaskID"], PortalSettings.PortalAlias.HTTPAlias);
            string strBody = String.Format(@"Help desk ticket #{0} '{1}' has been updated.", Request.QueryString["TaskID"], strDescription);
            strBody = strBody + Environment.NewLine + Environment.NewLine;
            strBody = strBody + "Comments:" + Environment.NewLine;
            strBody = strBody + strComment;
            strBody = strBody + Environment.NewLine + Environment.NewLine;
            strBody = strBody + String.Format(@"You may see the full status here: {0}", strLinkUrl);

            // Get all users in the AssignedRole Role
            ArrayList colAssignedRoleUsers = objRoleController.GetUsersByRoleName(PortalId, strAssignedRole);

            foreach (UserInfo objUserInfo in colAssignedRoleUsers)
            {
                DotNetNuke.Services.Mail.Mail.SendMail(PortalSettings.Email, objUserInfo.Email, "", strSubject, strBody, "", "HTML", "", "", "", "");
            }

            Log.InsertLog(Convert.ToInt32(Request.QueryString["TaskID"]), UserId, String.Format("{0} assigned ticket to {1}.", UserInfo.DisplayName, strAssignedRole));
        }
        #endregion

        #region NotifyRequestorOfComment
        private void NotifyRequestorOfComment(string strComment)
        {
            string strEmail = GetEmailOfRequestor();

            if (strEmail != "")
            {
                string strDescription = GetDescriptionOfTicket();
                string strSubject = String.Format("Help Desk Ticket #{0} at http://{1} has been updated", Request.QueryString["TaskID"], PortalSettings.PortalAlias.HTTPAlias);
                string strBody = String.Format(@"Help desk ticket #{0} '{1}' has been updated.", Request.QueryString["TaskID"], strDescription);
                strBody = strBody + Environment.NewLine + Environment.NewLine;
                strBody = strBody + "Comments:" + Environment.NewLine;
                strBody = strBody + strComment;

                DotNetNuke.Services.Mail.Mail.SendMail(PortalSettings.Email, strEmail, "", strSubject, strBody, "", "HTML", "", "", "", "");

                Log.InsertLog(Convert.ToInt32(Request.QueryString["TaskID"]), UserId, String.Format("Requestor at '{0}', was emailed comment: {1}.", strEmail, strComment));
            }
        }
        #endregion

        #region GetAssignedRole
        private int GetAssignedRole()
        {
            int intRole = -1;

            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();
            var result = from ADefHelpDesk_TaskDetails in objADefHelpDeskDALDataContext.ADefHelpDesk_Tasks
                         where ADefHelpDesk_TaskDetails.TaskID == Convert.ToInt32(Request.QueryString["TaskID"])
                         select ADefHelpDesk_TaskDetails;

            if (result != null)
            {
                intRole = result.FirstOrDefault().AssignedRoleID;
            }

            return intRole;
        }
        #endregion

        #region UserIsRequestor
        private bool UserIsRequestor()
        {
            bool isRequestor = false;

            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();
            var result = from ADefHelpDesk_TaskDetails in objADefHelpDeskDALDataContext.ADefHelpDesk_Tasks
                         where ADefHelpDesk_TaskDetails.TaskID == Convert.ToInt32(Request.QueryString["TaskID"])
                         select ADefHelpDesk_TaskDetails;

            if (result != null)
            {
                if (UserId == result.FirstOrDefault().RequesterUserID)
                {
                    isRequestor = true;
                }
            }

            return isRequestor;
        }
        #endregion

        // Visible to Requestor CheckBox

        #region chkCommentVisibleEdit_CheckedChanged
        protected void chkCommentVisibleEdit_CheckedChanged(object sender, EventArgs e)
        {
            // Only Display Email to Requestor link if chkCommentVisibleEdit is checked
            lnkUpdateRequestor.Visible = chkCommentVisibleEdit.Checked;
            ImgEmailUser.Visible = chkCommentVisibleEdit.Checked;
        }
        #endregion

        #region chkCommentVisible_CheckedChanged
        protected void chkCommentVisible_CheckedChanged(object sender, EventArgs e)
        {
            // Only Display Email link if chkCommentVisibleEdit is checked
            btnInsertCommentAndEmail.Visible = chkCommentVisible.Checked;
        }
        #endregion

        // Utility

        #region GetEmailOfRequestor
        private string GetEmailOfRequestor()
        {
            string strEmail = "";
            int intTaskId = Convert.ToInt32(Request.QueryString["TaskID"]);

            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();
            var result = (from ADefHelpDesk_TaskDetails in objADefHelpDeskDALDataContext.ADefHelpDesk_Tasks
                          where ADefHelpDesk_TaskDetails.TaskID == Convert.ToInt32(Request.QueryString["TaskID"])
                          select ADefHelpDesk_TaskDetails).FirstOrDefault();

            if (result != null)
            {
                if (result.RequesterUserID == -1)
                {
                    try
                    {
                        strEmail = result.RequesterEmail;
                    }
                    catch (Exception)
                    {
                        // User no longer exists
                        strEmail = "";
                    }
                }
                else
                {
                    strEmail = UserController.GetUser(PortalId, result.RequesterUserID, false).Email;
                }
            }

            return strEmail;
        }
        #endregion

        #region GetDescriptionOfTicket
        private string GetDescriptionOfTicket()
        {
            string strDescription = "";
            int intTaskId = Convert.ToInt32(Request.QueryString["TaskID"]);

            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();
            var result = (from ADefHelpDesk_TaskDetails in objADefHelpDeskDALDataContext.ADefHelpDesk_Tasks
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

        #region GetAdminRole
        private string GetAdminRole()
        {
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            List<ADefHelpDesk_Setting> colADefHelpDesk_Setting = (from ADefHelpDesk_Settings in objADefHelpDeskDALDataContext.ADefHelpDesk_Settings
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
    }
}