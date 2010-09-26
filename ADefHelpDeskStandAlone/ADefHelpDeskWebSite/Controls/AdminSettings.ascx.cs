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
using System.Net.Mail;
using System.Text;

namespace ADefWebserver.Modules.ADefHelpDesk
{
    public partial class AdminSettings : ModuleBase
    {
        List<int> colProcessedCategoryIDs;

        #region SelectedUser
        public string SelectedUser
        {
            get
            {
                if (ViewState["SelectedUser"] == null)
                {
                    ViewState["SelectedUser"] = "";
                }

                return Convert.ToString(ViewState["SelectedUser"]);
            }
            set
            {
                ViewState["SelectedUser"] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // Only show if user is an Administrator
                if (!UserInfo.IsSuperUser(UserId))
                {
                    pnlAdminSettings.Visible = false;
                    Response.Redirect("Default.aspx");
                }

                LoadGeneralSettings();
                SetView("UserManager");

                btnAddNew.Text = GetLocalResourceObject("btnAddNew.Text").ToString();
                btnUpdate.Text = GetLocalResourceObject("btnUpdateAdminRole.Text").ToString();
            }
        }

        #region SetView
        private void SetView(string ViewName)
        {
            if (ViewName == "UserRegistration")
            {
                pnlUserAdministration.Visible = false;
                pnlUploadefFilesPath.Visible = false;
                pnlTagsAdmin.Visible = false;
                pnlRoles.Visible = false;
                pnlEmail.Visible = false;
                pnlUserRegistration.Visible = true;

                lnkUserManager.Font.Bold = false;
                lnkUserManager.BackColor = Color.Transparent;
                lnkUploadefFilesPath.Font.Bold = false;
                lnkUploadefFilesPath.BackColor = Color.Transparent;
                lnkTagsAdmin.Font.Bold = false;
                lnkTagsAdmin.BackColor = Color.Transparent;
                lnkRoles.Font.Bold = false;
                lnkRoles.BackColor = Color.Transparent;
                lnkEmailAdministration.Font.Bold = false;
                lnkEmailAdministration.BackColor = Color.Transparent;
                lnkUserRegistration.Font.Bold = true;
                lnkUserRegistration.BackColor = Color.LightGray;
            }

            if (ViewName == "UserManager")
            {
                pnlUserAdministration.Visible = true;
                pnlUploadefFilesPath.Visible = false;
                pnlTagsAdmin.Visible = false;
                pnlRoles.Visible = false;
                pnlEmail.Visible = false;
                pnlUserRegistration.Visible = false;

                lnkUserManager.Font.Bold = true;
                lnkUserManager.BackColor = Color.LightGray;
                lnkUploadefFilesPath.Font.Bold = false;
                lnkUploadefFilesPath.BackColor = Color.Transparent;
                lnkTagsAdmin.Font.Bold = false;
                lnkTagsAdmin.BackColor = Color.Transparent;
                lnkRoles.Font.Bold = false;
                lnkRoles.BackColor = Color.Transparent;
                lnkEmailAdministration.Font.Bold = false;
                lnkEmailAdministration.BackColor = Color.Transparent;
                lnkUserRegistration.Font.Bold = false;
                lnkUserRegistration.BackColor = Color.Transparent;

                UserRolesDropDown();
                SetUserManagerView("Add");
            }

            if (ViewName == "UploadedFilesPath")
            {
                pnlUserAdministration.Visible = false;
                pnlUploadefFilesPath.Visible = true;
                pnlTagsAdmin.Visible = false;
                pnlRoles.Visible = false;
                pnlEmail.Visible = false;
                pnlUserRegistration.Visible = false;

                lnkUserManager.Font.Bold = false;
                lnkUserManager.BackColor = Color.Transparent;
                lnkUploadefFilesPath.Font.Bold = true;
                lnkUploadefFilesPath.BackColor = Color.LightGray;
                lnkTagsAdmin.Font.Bold = false;
                lnkTagsAdmin.BackColor = Color.Transparent;
                lnkRoles.Font.Bold = false;
                lnkRoles.BackColor = Color.Transparent;
                lnkEmailAdministration.Font.Bold = false;
                lnkEmailAdministration.BackColor = Color.Transparent;
                lnkUserRegistration.Font.Bold = false;
                lnkUserRegistration.BackColor = Color.Transparent;
            }

            if (ViewName == "Roles")
            {
                pnlUserAdministration.Visible = false;
                pnlUploadefFilesPath.Visible = false;
                pnlTagsAdmin.Visible = false;
                pnlRoles.Visible = true;
                pnlEmail.Visible = false;
                pnlUserRegistration.Visible = false;

                lnkUserManager.Font.Bold = false;
                lnkUserManager.BackColor = Color.Transparent;
                lnkUploadefFilesPath.Font.Bold = false;
                lnkUploadefFilesPath.BackColor = Color.Transparent;
                lnkTagsAdmin.Font.Bold = false;
                lnkTagsAdmin.BackColor = Color.Transparent;
                lnkRoles.Font.Bold = true;
                lnkRoles.BackColor = Color.LightGray;
                lnkEmailAdministration.Font.Bold = false;
                lnkEmailAdministration.BackColor = Color.Transparent;
                lnkUserRegistration.Font.Bold = false;
                lnkUserRegistration.BackColor = Color.Transparent;
            }

            if (ViewName == "TagsAdministration")
            {
                pnlUserAdministration.Visible = false;
                pnlUploadefFilesPath.Visible = false;
                pnlTagsAdmin.Visible = true;
                pnlRoles.Visible = false;
                pnlEmail.Visible = false;
                pnlUserRegistration.Visible = false;

                lnkUserManager.Font.Bold = false;
                lnkUserManager.BackColor = Color.Transparent;
                lnkUploadefFilesPath.Font.Bold = false;
                lnkUploadefFilesPath.BackColor = Color.Transparent;
                lnkTagsAdmin.Font.Bold = true;
                lnkTagsAdmin.BackColor = Color.LightGray;
                lnkRoles.Font.Bold = false;
                lnkRoles.BackColor = Color.Transparent;
                lnkEmailAdministration.Font.Bold = false;
                lnkEmailAdministration.BackColor = Color.Transparent;
                lnkUserRegistration.Font.Bold = false;
                lnkUserRegistration.BackColor = Color.Transparent;
            }

            if (ViewName == "EmailAdministration")
            {
                pnlUserAdministration.Visible = false;
                pnlUploadefFilesPath.Visible = false;
                pnlTagsAdmin.Visible = false;
                pnlRoles.Visible = false;
                pnlEmail.Visible = true;
                pnlUserRegistration.Visible = false;

                lnkUserManager.Font.Bold = false;
                lnkUserManager.BackColor = Color.Transparent;
                lnkUploadefFilesPath.Font.Bold = false;
                lnkUploadefFilesPath.BackColor = Color.Transparent;
                lnkTagsAdmin.Font.Bold = false;
                lnkTagsAdmin.BackColor = Color.Transparent;
                lnkRoles.Font.Bold = false;
                lnkRoles.BackColor = Color.Transparent;
                lnkEmailAdministration.Font.Bold = true;
                lnkEmailAdministration.BackColor = Color.LightGray;
                lnkUserRegistration.Font.Bold = false;
                lnkUserRegistration.BackColor = Color.Transparent;
            }
        }
        #endregion

        #region lnkBack_Click
        protected void lnkBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
        #endregion

        #region lnkUploadefFilesPath_Click
        protected void lnkUploadefFilesPath_Click(object sender, EventArgs e)
        {
            SetView("UploadedFilesPath");
            LoadGeneralSettings();
        }
        #endregion

        // User Manager

        #region lnkUserManager_Click
        protected void lnkUserManager_Click(object sender, EventArgs e)
        {
            SetView("UserManager");
        }
        #endregion

        #region SetUserManagerView
        private void SetUserManagerView(string ViewName)
        {
            if (ViewName == "Add")
            {
                lblSelectedUserID.Text = "";
                chkSuperUser.Checked = false;
                txtUserName.Text = "";
                txtFirstName.Text = "";
                txtLastName.Text = "";
                txtEmail.Text = "";
                txtPasswordInsert.Text = "";
                txtPasswordInsert.Attributes["value"] = "";

                // Set User Name to Read only                
                txtUserName.ReadOnly = true;
                txtUserName.BackColor = Color.LightGray;

                SelectedUser = "";
                ClearAllSelectedUsers();
                gvUserRoles.DataBind();

                btnAddUserRole.Visible = false;
                ddlUserRoles.Visible = false;

                btnAddUser.Visible = true;
                btnSaveUser.Visible = false;
                btnDeleteUser.Visible = false;
                btnCancelUser.Visible = true;

                chkSuperUser.Enabled = false;
                txtFirstName.Enabled = false;
                txtLastName.Enabled = false;
                txtEmail.Enabled = false;

                txtUserName.BackColor = Color.LightGray;
                txtFirstName.BackColor = Color.LightGray;
                txtLastName.BackColor = Color.LightGray;
                txtEmail.BackColor = Color.LightGray;
                txtPasswordInsert.BackColor = Color.LightGray;
            }

            if (ViewName == "Edit")
            {
                // Only show selection to add a user to roles if there are roles
                dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();
                ddlUserRoles.Visible = (objdnnHelpDeskDALDataContext.ADefHelpDesk_Roles.Count() > 0);
                btnAddUserRole.Visible = (objdnnHelpDeskDALDataContext.ADefHelpDesk_Roles.Count() > 0);

                btnAddUser.Visible = true;
                btnSaveUser.Visible = true;
                btnDeleteUser.Visible = true;
                btnCancelUser.Visible = false;

                chkSuperUser.Enabled = true;
                txtFirstName.Enabled = true;
                txtLastName.Enabled = true;
                txtEmail.Enabled = true;

                txtUserName.BackColor = Color.LightGray;
                txtFirstName.BackColor = Color.Transparent;
                txtLastName.BackColor = Color.Transparent;
                txtEmail.BackColor = Color.Transparent;
                txtPasswordInsert.BackColor = Color.Transparent;
            }

            if (ViewName == "Save")
            {
                SelectedUser = "";
                ClearAllSelectedUsers();

                btnAddUser.Visible = false;
                btnSaveUser.Visible = true;
                btnDeleteUser.Visible = false;
                btnCancelUser.Visible = true;

                chkSuperUser.Enabled = true;
                txtFirstName.Enabled = true;
                txtLastName.Enabled = true;
                txtEmail.Enabled = true;

                txtUserName.BackColor = Color.Transparent;
                txtFirstName.BackColor = Color.Transparent;
                txtLastName.BackColor = Color.Transparent;
                txtEmail.BackColor = Color.Transparent;
                txtPasswordInsert.BackColor = Color.Transparent;
            }
        }
        #endregion

        #region btnAddUser_Click
        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            lblSelectedUserID.Text = "";
            chkSuperUser.Checked = false;
            txtUserName.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            txtPasswordInsert.Text = "";
            txtPasswordInsert.Attributes["value"] = "";

            // Enable User Name
            txtUserName.ReadOnly = false;
            txtUserName.BackColor = Color.Transparent;

            // Disable Adding Roles
            btnAddUserRole.Visible = false;
            ddlUserRoles.Visible = false;

            gvUserRoles.DataBind();
            SetUserManagerView("Save");
        }
        #endregion

        #region btnCancelUser_Click
        protected void btnCancelUser_Click(object sender, EventArgs e)
        {
            SetUserManagerView("Add");
        }
        #endregion

        #region btnSaveUser_Click
        protected void btnSaveUser_Click(object sender, EventArgs e)
        {
            // Must have a User Name, Email, First Name and Last Name
            if (
                txtUserName.Text.Trim() == "" ||
                txtFirstName.Text.Trim() == "" ||
                txtLastName.Text.Trim() == "" ||
                txtEmail.Text.Trim() == "" ||
                txtPasswordInsert.Text.Trim() == ""
                )
            {
                lblUserError.Text = GetLocalResourceObject("MustFillInAllFields.Text").ToString();
                return;
            }

            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            if (lblSelectedUserID.Text.Trim() == "")
            {
                // ** This is an INSERT **

                // Check to see if another user has the same User Name
                var objSelectedUserName = (from ADefHelpDesk_Users in objdnnHelpDeskDALDataContext.ADefHelpDesk_Users
                                           where ADefHelpDesk_Users.Username == txtUserName.Text.Trim()
                                           select ADefHelpDesk_Users).FirstOrDefault();

                if (objSelectedUserName != null)
                {
                    lblUserError.Text = GetLocalResourceObject("AnotherUserHasUserName.Text").ToString();
                    return;
                }

                // Check to see if another user has the same email
                var objSelectedEmail = (from ADefHelpDesk_Users in objdnnHelpDeskDALDataContext.ADefHelpDesk_Users
                                        where ADefHelpDesk_Users.Email == txtEmail.Text.Trim()
                                        select ADefHelpDesk_Users).FirstOrDefault();

                if (objSelectedEmail != null)
                {
                    lblUserError.Text = GetLocalResourceObject("AnotherUserHasEmail.Text").ToString();
                    return;
                }

                // Insert User
                ADefHelpDesk_User objADefHelpDesk_User = new ADefHelpDesk_User();

                objADefHelpDesk_User.IsSuperUser = chkSuperUser.Checked;
                objADefHelpDesk_User.Username = txtUserName.Text.Trim();
                objADefHelpDesk_User.FirstName = txtFirstName.Text.Trim();
                objADefHelpDesk_User.LastName = txtLastName.Text.Trim();
                objADefHelpDesk_User.Email = txtEmail.Text.Trim();
                objADefHelpDesk_User.Password = Utility.HashPassword(txtUserName.Text.Trim().ToLower() + txtPasswordInsert.Text.Trim());

                objdnnHelpDeskDALDataContext.ADefHelpDesk_Users.InsertOnSubmit(objADefHelpDesk_User);
                objdnnHelpDeskDALDataContext.SubmitChanges();

                // Set the UserID
                lblSelectedUserID.Text = objADefHelpDesk_User.UserID.ToString();

                lblUserError.Text = GetLocalResourceObject("Updated.Text").ToString();

                // Show the User Roles
                UserRolesDropDown();

                // Reload the users
                lvUsers.DataBind();

                LoadSeletedUser(objADefHelpDesk_User.UserID);
                return;
            }
            else
            {
                // ** This is an UPDATE **

                // If the user being edited is the user that is logged in, the chkSuperUser must be checked
                if (chkSuperUser.Checked == false & Convert.ToInt32(lblSelectedUserID.Text.Trim()) == UserId)
                {
                    lblUserError.Text = GetLocalResourceObject("CannotUnsetCurrentSuperUser.Text").ToString();
                    return;
                }

                // Check to see if another user has the same email
                var objSelectedUser = (from ADefHelpDesk_Users in objdnnHelpDeskDALDataContext.ADefHelpDesk_Users
                                       where ADefHelpDesk_Users.Email == txtEmail.Text.Trim()
                                       where ADefHelpDesk_Users.UserID != Convert.ToInt32(lblSelectedUserID.Text)
                                       select ADefHelpDesk_Users).FirstOrDefault();

                if (objSelectedUser != null)
                {
                    lblUserError.Text = GetLocalResourceObject("AnotherUserHasEmail.Text").ToString();
                    return;
                }

                var objUpdateUser = (from ADefHelpDesk_Users in objdnnHelpDeskDALDataContext.ADefHelpDesk_Users
                                     where ADefHelpDesk_Users.UserID == Convert.ToInt32(lblSelectedUserID.Text)
                                     select ADefHelpDesk_Users).FirstOrDefault();

                if (objUpdateUser != null)
                {
                    objUpdateUser.IsSuperUser = chkSuperUser.Checked;
                    objUpdateUser.FirstName = txtFirstName.Text.Trim();
                    objUpdateUser.LastName = txtLastName.Text.Trim();
                    objUpdateUser.Email = txtEmail.Text.Trim();

                    // Only update password if it is not 10 astericks
                    if (txtPasswordInsert.Text != "**********")
                    {
                        objUpdateUser.Password = Utility.HashPassword(txtUserName.Text.Trim().ToLower() + txtPasswordInsert.Text.Trim());
                    }

                    objdnnHelpDeskDALDataContext.SubmitChanges();
                    lblUserError.Text = GetLocalResourceObject("Updated.Text").ToString();
                    lvUsers.DataBind();
                    return;
                }
            }
        }
        #endregion

        #region btnUserSearch_Click
        protected void btnUserSearch_Click(object sender, EventArgs e)
        {
            lvUsers.DataBind();
        }
        #endregion

        #region LDSUsers_Selecting
        protected void LDSUsers_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            var result = from ADefHelpDesk_Users in objdnnHelpDeskDALDataContext.ADefHelpDesk_Users
                         where
                         ADefHelpDesk_Users.Username.Contains(txtUserSearch.Text) ||
                         ADefHelpDesk_Users.FirstName.Contains(txtUserSearch.Text) ||
                         ADefHelpDesk_Users.LastName.Contains(txtUserSearch.Text)
                         select ADefHelpDesk_Users;

            e.Result = result;
        }
        #endregion

        #region btnResetSearch_Click
        protected void btnResetSearch_Click(object sender, EventArgs e)
        {
            txtUserSearch.Text = "";
            lvUsers.DataBind();
        }
        #endregion

        #region lvUsers_ItemDataBound
        protected void lvUsers_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ListView listView = (ListView)sender;
            ListViewDataItem objListViewDataItem = (ListViewDataItem)e.Item;

            LinkButton lnkUsername = (LinkButton)e.Item.FindControl("lnkUsername");
            Label FirstNameLabel = (Label)e.Item.FindControl("FirstNameLabel");
            Label LastNameLabel = (Label)e.Item.FindControl("LastNameLabel");

            if (lnkUsername.Text.Length > 15)
            {
                lnkUsername.Text = String.Format("{0} ...", Strings.Left(lnkUsername.Text, 15));
            }

            if (FirstNameLabel.Text.Length > 15)
            {
                FirstNameLabel.Text = String.Format("{0} ...", Strings.Left(FirstNameLabel.Text, 15));
            }

            if (LastNameLabel.Text.Length > 15)
            {
                LastNameLabel.Text = String.Format("{0} ...", Strings.Left(LastNameLabel.Text, 15));
            }

            // Set the selected user in the ListView
            if (lnkUsername.CommandArgument == SelectedUser)
            {
                lnkUsername.Font.Size = FontUnit.Larger;
                lnkUsername.Font.Bold = true;
                lnkUsername.BackColor = Color.LightGray;
            }
            else
            {
                lnkUsername.Font.Size = FontUnit.Empty;
                lnkUsername.Font.Bold = false;
                lnkUsername.BackColor = Color.Transparent;
            }
        }
        #endregion

        #region lnkUsername_Click
        protected void lnkUsername_Click(object sender, EventArgs e)
        {
            // Set the selected user
            LinkButton lnkSelectedUsername = (LinkButton)sender;
            SelectedUser = lnkSelectedUsername.CommandArgument;

            lnkSelectedUsername.Font.Size = FontUnit.Larger;
            lnkSelectedUsername.Font.Bold = true;
            lnkSelectedUsername.BackColor = Color.LightGray;

            LoadSeletedUser(Convert.ToInt32(lnkSelectedUsername.CommandArgument));
        }        
        #endregion

        #region btnDeleteUser_Click
        protected void btnDeleteUser_Click(object sender, EventArgs e)
        {
            // The user being deleted cannot be the user that is logged in
            if (Convert.ToInt32(lblSelectedUserID.Text.Trim()) == UserId)
            {
                lblUserError.Text = GetLocalResourceObject("CannotDeletecurrentUser.Text").ToString();
                return;
            }

            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            // Delte any UserRoles
            var colUserRoles = from ADefHelpDesk_UserRoles in objdnnHelpDeskDALDataContext.ADefHelpDesk_UserRoles
                               where ADefHelpDesk_UserRoles.UserID == Convert.ToInt32(lblSelectedUserID.Text)
                               select ADefHelpDesk_UserRoles;

            objdnnHelpDeskDALDataContext.ADefHelpDesk_UserRoles.DeleteAllOnSubmit(colUserRoles);
            objdnnHelpDeskDALDataContext.SubmitChanges();

            // Delte the user
            var objSelectedUser = (from ADefHelpDesk_Users in objdnnHelpDeskDALDataContext.ADefHelpDesk_Users
                                   where ADefHelpDesk_Users.UserID == Convert.ToInt32(lblSelectedUserID.Text)
                                   select ADefHelpDesk_Users).FirstOrDefault();

            if (objSelectedUser != null)
            {
                objdnnHelpDeskDALDataContext.ADefHelpDesk_Users.DeleteOnSubmit(objSelectedUser);
                objdnnHelpDeskDALDataContext.SubmitChanges();

                // Clear all Users
                SelectedUser = "";
                ClearAllSelectedUsers();
                lvUsers.DataBind();
                SetUserManagerView("Add");
            }
        }
        #endregion

        #region ClearAllSelectedUsers
        private void ClearAllSelectedUsers()
        {
            // Clear all LinkButtons
            foreach (var item in lvUsers.Items)
            {
                LinkButton lnkUsername = (LinkButton)item.FindControl("lnkUsername");

                if (lnkUsername.CommandArgument != SelectedUser)
                {
                    lnkUsername.Font.Size = FontUnit.Empty;
                    lnkUsername.Font.Bold = false;
                    lnkUsername.BackColor = Color.Transparent;
                }
            }
        }
        #endregion

        #region LoadSeletedUser
        private void LoadSeletedUser(int intSelectedUser)
        {
            // Clear all other Users
            ClearAllSelectedUsers();

            // Load selected user
            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            var objSelectedUser = (from ADefHelpDesk_Users in objdnnHelpDeskDALDataContext.ADefHelpDesk_Users
                                   where ADefHelpDesk_Users.UserID == intSelectedUser
                                   select ADefHelpDesk_Users).FirstOrDefault();

            if (objSelectedUser != null)
            {
                lblSelectedUserID.Text = objSelectedUser.UserID.ToString();
                chkSuperUser.Checked = objSelectedUser.IsSuperUser;
                txtUserName.Text = objSelectedUser.Username;
                txtFirstName.Text = objSelectedUser.FirstName;
                txtLastName.Text = objSelectedUser.LastName;
                txtEmail.Text = objSelectedUser.Email;
                txtPasswordInsert.Attributes["value"] = "**********";
            }

            // Set User Name to Read only
            txtUserName.ReadOnly = true;
            txtUserName.BackColor = Color.LightGray;

            // Load the User Roles
            gvUserRoles.DataBind();

            SetUserManagerView("Edit");
        } 
        #endregion

        // Utility

        #region MyRegion
        protected void lnkRoles_Click(object sender, EventArgs e)
        {
            SetView("Roles");
            lvRoles.DataBind();
        }
        #endregion

        #region lnkTagsAdmin_Click
        protected void lnkTagsAdmin_Click(object sender, EventArgs e)
        {
            SetView("TagsAdministration");
            DisplayCatagories();
            tvCategories.CollapseAll();
        }
        #endregion

        #region btnUploadedFiles_Click
        protected void btnUploadedFiles_Click(object sender, EventArgs e)
        {
            GeneralSettings.UpdateFileUploadPath(txtUploadedFilesPath.Text.Trim());
            GeneralSettings.UpdateUploadPermission(ddlUploadPermission.SelectedValue);

            lblUploadedFilesPath.Text = GetLocalResourceObject("Updated.Text").ToString();
        }
        #endregion

        // Tags

        #region DisplayCatagories
        private void DisplayCatagories()
        {
            CatagoriesTree colCatagories = new CatagoriesTree(PortalId, false);
            tvCategories.DataSource = colCatagories;

            TreeNodeBinding RootBinding = new TreeNodeBinding();
            RootBinding.DataMember = "ListItem";
            RootBinding.TextField = "Text";
            RootBinding.ValueField = "Value";

            tvCategories.DataBindings.Add(RootBinding);

            tvCategories.DataBind();
            tvCategories.CollapseAll();

            // If a node was selected previously select it again
            if (txtCategoryID.Text != "")
            {
                int intCategoryID = Convert.ToInt32(txtCategoryID.Text);
                TreeNode objTreeNode = (TreeNode)tvCategories.FindNode(GetNodePath(intCategoryID));
                objTreeNode.Select();
                objTreeNode.Expand();

                // Expand it's parent nodes
                // Get the value of each parent node
                string[] strParentNodes = objTreeNode.ValuePath.Split(Convert.ToChar("/"));
                // Loop through each parent node
                for (int i = 0; i < objTreeNode.Depth; i++)
                {
                    // Get the parent node
                    TreeNode objParentTreeNode = (TreeNode)tvCategories.FindNode(GetNodePath(Convert.ToInt32(strParentNodes[i])));
                    // Expand the parent node
                    objParentTreeNode.Expand();
                }
            }
            else
            {
                //If there is at least one existing category, select it
                if (tvCategories.Nodes.Count > 0)
                {
                    tvCategories.Nodes[0].Select();
                    txtCategoryID.Text = "0";
                    SelectTreeNode();
                }
                else
                {
                    // There is no data so set form to Add New
                    SetFormToAddNew();
                }
            }

            // If a node is selected, remove it from the BindDropDown drop-down
            int intCategoryNotToShow = -1;
            TreeNode objSelectedTreeNode = (TreeNode)tvCategories.SelectedNode;
            if (objSelectedTreeNode != null)
            {
                intCategoryNotToShow = Convert.ToInt32(tvCategories.SelectedNode.Value);
            }

            BindDropDown(intCategoryNotToShow);
        }
        #endregion

        #region BindDropDown
        private void BindDropDown(int intCategoryNotToShow)
        {
            // Bind drop-down
            CategoriesDropDown colCategoriesDropDown = new CategoriesDropDown(PortalId);
            ListItemCollection objListItemCollection = colCategoriesDropDown.Categories(intCategoryNotToShow);

            // Don't show the currently selected node
            foreach (ListItem objListItem in objListItemCollection)
            {
                if (objListItem.Value == intCategoryNotToShow.ToString())
                {
                    objListItemCollection.Remove(objListItem);
                    break;
                }
            }

            ddlParentCategory.DataSource = objListItemCollection;
            ddlParentCategory.DataTextField = "Text";
            ddlParentCategory.DataValueField = "Value";
            ddlParentCategory.DataBind();
        }
        #endregion

        #region GetNodePath
        private string GetNodePath(int intCategoryID)
        {
            string strNodePath = intCategoryID.ToString();

            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            var result = (from ADefHelpDesk_Categories in objdnnHelpDeskDALDataContext.ADefHelpDesk_Categories
                          where ADefHelpDesk_Categories.CategoryID == intCategoryID
                          select ADefHelpDesk_Categories).FirstOrDefault();

            // Only build a node path if the current level is not the root
            if (result.Level > 1)
            {
                int intCurrentCategoryID = result.CategoryID;

                for (int i = 1; i < result.Level; i++)
                {
                    var CurrentCategory = (from ADefHelpDesk_Categories in objdnnHelpDeskDALDataContext.ADefHelpDesk_Categories
                                           where ADefHelpDesk_Categories.CategoryID == intCurrentCategoryID
                                           select ADefHelpDesk_Categories).FirstOrDefault();

                    strNodePath = CurrentCategory.ParentCategoryID.ToString() + @"/" + strNodePath;

                    var ParentCategory = (from ADefHelpDesk_Categories in objdnnHelpDeskDALDataContext.ADefHelpDesk_Categories
                                          where ADefHelpDesk_Categories.CategoryID == CurrentCategory.ParentCategoryID
                                          select ADefHelpDesk_Categories).FirstOrDefault();

                    intCurrentCategoryID = ParentCategory.CategoryID;
                }
            }

            return strNodePath;
        }
        #endregion

        #region tvCategories_SelectedNodeChanged
        protected void tvCategories_SelectedNodeChanged(object sender, EventArgs e)
        {
            SelectTreeNode();
            ResetForm();
        }
        #endregion

        #region SelectTreeNode
        private void SelectTreeNode()
        {
            if (tvCategories.SelectedNode != null)
            {
                if (tvCategories.SelectedNode.Value != "")
                {
                    var result = (from ADefHelpDesk_Categories in CategoriesTable.GetCategoriesTable(PortalId, false)
                                  where ADefHelpDesk_Categories.CategoryID == Convert.ToInt32(tvCategories.SelectedNode.Value)
                                  select ADefHelpDesk_Categories).FirstOrDefault();

                    txtCategory.Text = result.CategoryName;
                    txtCategoryID.Text = result.CategoryID.ToString();
                    chkRequesterVisible.Checked = result.RequestorVisible;
                    chkSelectable.Checked = result.Selectable;

                    // Remove Node from the Bind DropDown drop-down
                    BindDropDown(result.CategoryID);

                    // Set the Parent drop-down
                    ddlParentCategory.SelectedValue = (result.ParentCategoryID == null) ? "0" : result.ParentCategoryID.ToString();
                    txtParentCategoryID.Text = (result.ParentCategoryID == null) ? "" : result.ParentCategoryID.ToString();
                }
            }
        }
        #endregion

        #region btnUpdate_Click
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            if (btnUpdate.CommandName == "Update")
            {
                var result = (from ADefHelpDesk_Categories in objdnnHelpDeskDALDataContext.ADefHelpDesk_Categories
                              where ADefHelpDesk_Categories.CategoryID == Convert.ToInt32(txtCategoryID.Text)
                              select ADefHelpDesk_Categories).FirstOrDefault();

                result.CategoryName = txtCategory.Text.Trim();

                result.ParentCategoryID = (GetParentCategoryID(ddlParentCategory.SelectedValue) == "0") ? (int?)null : Convert.ToInt32(ddlParentCategory.SelectedValue);
                txtParentCategoryID.Text = (ddlParentCategory.SelectedValue == "0") ? "" : ddlParentCategory.SelectedValue;

                result.Level = Convert.ToInt32((ddlParentCategory.SelectedValue == "0") ? 1 : GetLevelOfParent(Convert.ToInt32(ddlParentCategory.SelectedValue)) + 1);
                result.RequestorVisible = chkRequesterVisible.Checked;
                result.Selectable = chkSelectable.Checked;

                objdnnHelpDeskDALDataContext.SubmitChanges();

                // Update levels off all the Children
                colProcessedCategoryIDs = new List<int>();
                UpdateLevelOfChildren(result);
            }
            else
            {
                // This is a Save for a new Node                

                ADefHelpDesk_Category objADefHelpDesk_Category = new ADefHelpDesk_Category();
                objADefHelpDesk_Category.PortalID = PortalId;
                objADefHelpDesk_Category.CategoryName = txtCategory.Text.Trim();
                objADefHelpDesk_Category.ParentCategoryID = (GetParentCategoryID(ddlParentCategory.SelectedValue) == "0") ? (int?)null : Convert.ToInt32(ddlParentCategory.SelectedValue);
                objADefHelpDesk_Category.Level = Convert.ToInt32((ddlParentCategory.SelectedValue == "0") ? 1 : GetLevelOfParent(Convert.ToInt32(ddlParentCategory.SelectedValue)) + 1);
                objADefHelpDesk_Category.RequestorVisible = chkRequesterVisible.Checked;
                objADefHelpDesk_Category.Selectable = chkSelectable.Checked;

                objdnnHelpDeskDALDataContext.ADefHelpDesk_Categories.InsertOnSubmit(objADefHelpDesk_Category);
                objdnnHelpDeskDALDataContext.SubmitChanges();

                // Set the Hidden CategoryID
                txtParentCategoryID.Text = (objADefHelpDesk_Category.ParentCategoryID == null) ? "" : ddlParentCategory.SelectedValue;
                txtCategoryID.Text = objADefHelpDesk_Category.CategoryID.ToString();
                ResetForm();
            }

            RefreshCache();
            DisplayCatagories();

            // Set the Parent drop-down
            if (txtParentCategoryID.Text != "")
            {
                ddlParentCategory.SelectedValue = txtParentCategoryID.Text;
            }
        }
        #endregion

        #region UpdateLevelOfChildren
        private void UpdateLevelOfChildren(ADefHelpDesk_Category result)
        {
            int? intStartingLevel = result.Level;

            if (colProcessedCategoryIDs == null)
            {
                colProcessedCategoryIDs = new List<int>();
            }

            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            // Get the children of the current item
            // This method may be called from the top level or recuresively by one of the child items
            var CategoryChildren = from ADefHelpDesk_Categories in objdnnHelpDeskDALDataContext.ADefHelpDesk_Categories
                                   where ADefHelpDesk_Categories.ParentCategoryID == result.CategoryID
                                   where !colProcessedCategoryIDs.Contains(result.CategoryID)
                                   select ADefHelpDesk_Categories;

            // Loop thru each item
            foreach (var objCategory in CategoryChildren)
            {
                colProcessedCategoryIDs.Add(objCategory.CategoryID);

                objCategory.Level = ((intStartingLevel) ?? 0) + 1;
                objdnnHelpDeskDALDataContext.SubmitChanges();

                //Recursively call the UpdateLevelOfChildren method adding all children
                UpdateLevelOfChildren(objCategory);
            }
        }
        #endregion

        #region GetLevelOfParent
        private int? GetLevelOfParent(int? ParentCategoryID)
        {
            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            var result = (from ADefHelpDesk_Categories in objdnnHelpDeskDALDataContext.ADefHelpDesk_Categories
                          where ADefHelpDesk_Categories.CategoryID == ParentCategoryID
                          select ADefHelpDesk_Categories).FirstOrDefault();

            return (result == null) ? 0 : result.Level;
        }
        #endregion

        #region GetParentCategoryID
        private string GetParentCategoryID(string strParentCategoryID)
        {
            // This is to ensure that the ParentCategoryID does exist and has not been deleted since the last time the form was loaded
            int ParentCategoryID = 0;
            if (strParentCategoryID != "0")
            {
                ParentCategoryID = Convert.ToInt32(strParentCategoryID);
            }

            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            var result = (from ADefHelpDesk_Categories in objdnnHelpDeskDALDataContext.ADefHelpDesk_Categories
                          where ADefHelpDesk_Categories.CategoryID == ParentCategoryID
                          select ADefHelpDesk_Categories).FirstOrDefault();

            string strResultParentCategoryID = "0";
            if (result != null)
            {
                strResultParentCategoryID = result.CategoryID.ToString();
            }

            return strResultParentCategoryID;
        }
        #endregion

        #region btnAddNew_Click
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            if (btnAddNew.CommandName == "AddNew")
            {
                SetFormToAddNew();
            }
            else
            {
                // This is a Cancel
                ResetForm();
                DisplayCatagories();
                SelectTreeNode();
            }
        }
        #endregion

        #region SetFormToAddNew
        private void SetFormToAddNew()
        {
            txtCategory.Text = "";
            chkRequesterVisible.Checked = true;
            chkSelectable.Checked = true;
            btnAddNew.CommandName = "Cancel";
            btnUpdate.CommandName = "Save";
            btnAddNew.Text = GetLocalResourceObject("Cancel.Text").ToString();
            btnUpdate.Text = GetLocalResourceObject("Save.Text").ToString();
            btnDelete.Visible = false;
            BindDropDown(-1);

            if (tvCategories.SelectedNode == null)
            {
                ddlParentCategory.SelectedValue = "0";
            }
            else
            {
                try
                {
                    ddlParentCategory.SelectedValue = tvCategories.SelectedNode.Value;
                }
                catch (Exception ex)
                {
                    lblTagError.Text = ex.Message;
                }
            }
        }
        #endregion

        #region ResetForm
        private void ResetForm()
        {
            btnUpdate.CommandName = "Update";
            btnAddNew.CommandName = "AddNew";
            btnAddNew.Text = GetLocalResourceObject("btnAddNew.Text").ToString();
            btnUpdate.Text = GetLocalResourceObject("btnUpdateAdminRole.Text").ToString();
            btnDelete.Visible = true;
        }
        #endregion

        #region btnDelete_Click
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            // Get the node
            var result = (from ADefHelpDesk_Categories in objdnnHelpDeskDALDataContext.ADefHelpDesk_Categories
                          where ADefHelpDesk_Categories.CategoryID == Convert.ToInt32(txtCategoryID.Text)
                          select ADefHelpDesk_Categories).FirstOrDefault();

            // Make a Temp object to use to update the child nodes
            ADefHelpDesk_Category TmpADefHelpDesk_Category = new ADefHelpDesk_Category();
            TmpADefHelpDesk_Category.CategoryID = result.CategoryID;
            if (result.ParentCategoryID == null)
            {
                TmpADefHelpDesk_Category.Level = 0;
            }
            else
            {
                TmpADefHelpDesk_Category.Level = Convert.ToInt32(GetLevelOfParent(result.ParentCategoryID));
            }

            // Get all TaskCategories that use the Node
            var colTaskCategories = from ADefHelpDesk_TaskCategories in objdnnHelpDeskDALDataContext.ADefHelpDesk_TaskCategories
                                    where ADefHelpDesk_TaskCategories.CategoryID == Convert.ToInt32(txtCategoryID.Text)
                                    select ADefHelpDesk_TaskCategories;

            // Delete them
            objdnnHelpDeskDALDataContext.ADefHelpDesk_TaskCategories.DeleteAllOnSubmit(colTaskCategories);
            objdnnHelpDeskDALDataContext.SubmitChanges();

            // Delete the node
            objdnnHelpDeskDALDataContext.ADefHelpDesk_Categories.DeleteOnSubmit(result);
            objdnnHelpDeskDALDataContext.SubmitChanges();

            // Update levels of all the Children            
            UpdateLevelOfChildren(TmpADefHelpDesk_Category);

            // Update all the children nodes to give them a new parent
            var CategoryChildren = from ADefHelpDesk_Categories in objdnnHelpDeskDALDataContext.ADefHelpDesk_Categories
                                   where ADefHelpDesk_Categories.ParentCategoryID == result.CategoryID
                                   select ADefHelpDesk_Categories;

            // Loop thru each item
            foreach (var objCategory in CategoryChildren)
            {
                objCategory.ParentCategoryID = result.ParentCategoryID;
                objdnnHelpDeskDALDataContext.SubmitChanges();
            }

            // Delete the Catagory from any Ticket that uses it
            var DeleteHelpDesk_TaskCategories = from ADefHelpDesk_TaskCategories in objdnnHelpDeskDALDataContext.ADefHelpDesk_TaskCategories
                                                where ADefHelpDesk_TaskCategories.CategoryID == TmpADefHelpDesk_Category.CategoryID
                                                select ADefHelpDesk_TaskCategories;

            objdnnHelpDeskDALDataContext.ADefHelpDesk_TaskCategories.DeleteAllOnSubmit(DeleteHelpDesk_TaskCategories);
            objdnnHelpDeskDALDataContext.SubmitChanges();

            RefreshCache();

            // Set the CategoryID
            txtCategoryID.Text = (result.ParentCategoryID == null) ? "" : result.ParentCategoryID.ToString();

            DisplayCatagories();
            SelectTreeNode();
        }
        #endregion

        #region RefreshCache
        private void RefreshCache()
        {
            // Get Table out of Cache
            object objCategoriesTable = HttpContext.Current.Cache.Get(String.Format("CategoriesTable_{0}", PortalId.ToString()));

            // Is the table in the cache?
            if (objCategoriesTable != null)
            {
                // Remove table from cache
                HttpContext.Current.Cache.Remove(String.Format("CategoriesTable_{0}", PortalId.ToString()));
            }

            // Get Table out of Cache
            object objRequestorCategoriesTable_ = HttpContext.Current.Cache.Get(String.Format("RequestorCategoriesTable_{0}", PortalId.ToString()));

            // Is the table in the cache?
            if (objRequestorCategoriesTable_ != null)
            {
                // Remove table from cache
                HttpContext.Current.Cache.Remove(String.Format("RequestorCategoriesTable_{0}", PortalId.ToString()));
            }
        }
        #endregion

        #region tvCategories_TreeNodeDataBound
        protected void tvCategories_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
        {
            ListItem objListItem = (ListItem)e.Node.DataItem;
            e.Node.ShowCheckBox = Convert.ToBoolean(objListItem.Attributes["Selectable"]);
            if (Convert.ToBoolean(objListItem.Attributes["Selectable"]))
            {
                e.Node.ImageUrl = Convert.ToBoolean(objListItem.Attributes["RequestorVisible"]) ? "../images/world.png" : "../images/world_delete.png";
                e.Node.ToolTip = Convert.ToBoolean(objListItem.Attributes["RequestorVisible"]) ? "Requestor Visible" : "Requestor Not Visible";
            }
            else
            {
                e.Node.ImageUrl = "../images/table.png";
                e.Node.ToolTip = Convert.ToBoolean(objListItem.Attributes["RequestorVisible"]) ? "Requestor Visible" : "Requestor Not Visible";
            }
        }
        #endregion

        // User Roles

        #region LDSUserRoles_Selecting
        protected void LDSUserRoles_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            int intSelectedUserID = -1;
            if (lblSelectedUserID.Text.Trim() != "")
            {
                intSelectedUserID = Convert.ToInt32(lblSelectedUserID.Text.Trim());
            }

            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            var result = from ADefHelpDesk_UserRoles in objdnnHelpDeskDALDataContext.ADefHelpDesk_UserRoles
                         where ADefHelpDesk_UserRoles.UserID == intSelectedUserID
                         select ADefHelpDesk_UserRoles.ADefHelpDesk_Role;

            e.Result = result;
            return;
        }
        #endregion

        #region gvUserRoles_RowDeleting
        protected void gvUserRoles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Handle the deletion and do not actually pass it to the Linq Data Source
            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            var objADefHelpDesk_UserRoles = (from ADefHelpDesk_UserRoles in objdnnHelpDeskDALDataContext.ADefHelpDesk_UserRoles
                                             where ADefHelpDesk_UserRoles.RoleID == Convert.ToInt32(e.Keys["ID"])
                                             where ADefHelpDesk_UserRoles.UserID == Convert.ToInt32(lblSelectedUserID.Text.Trim())
                                             select ADefHelpDesk_UserRoles).FirstOrDefault();

            objdnnHelpDeskDALDataContext.ADefHelpDesk_UserRoles.DeleteOnSubmit(objADefHelpDesk_UserRoles);
            objdnnHelpDeskDALDataContext.SubmitChanges();

            gvUserRoles.DataBind();
            e.Cancel = true;
        }
        #endregion

        #region UserRolesDropDown
        private void UserRolesDropDown()
        {
            ddlUserRoles.Items.Clear();

            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            List<ADefHelpDesk_Role> colADefHelpDesk_Roles = (from ADefHelpDesk_Roles in objdnnHelpDeskDALDataContext.ADefHelpDesk_Roles
                                                             where ADefHelpDesk_Roles.PortalID == PortalId
                                                             select ADefHelpDesk_Roles).ToList();

            // Add the Roles to the List
            foreach (ADefHelpDesk_Role objADefHelpDesk_Role in colADefHelpDesk_Roles)
            {
                ListItem RoleListItem = new ListItem();
                RoleListItem.Text = objADefHelpDesk_Role.RoleName;
                RoleListItem.Value = objADefHelpDesk_Role.ID.ToString();
                ddlUserRoles.Items.Add(RoleListItem);
            }

            // do not show the Roles dropdown if there is no user selected
            if (lblSelectedUserID.Text.Trim() == "")
            {
                btnAddUserRole.Visible = false;
                ddlUserRoles.Visible = false;
            }
            else
            {
                // Hide User roles drop down and Add Roles button if there are no roles
                btnAddUserRole.Visible = (ddlUserRoles.Items.Count != 0);
                ddlUserRoles.Visible = (ddlUserRoles.Items.Count != 0);
            }
        }
        #endregion

        #region btnAddUserRole_Click
        protected void btnAddUserRole_Click(object sender, EventArgs e)
        {
            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            int intSelectedUserID = -1;
            if (lblSelectedUserID.Text.Trim() != "")
            {
                intSelectedUserID = Convert.ToInt32(lblSelectedUserID.Text.Trim());
            }

            // See if Role already exists
            var objUserRole = (from ADefHelpDesk_UserRoles in objdnnHelpDeskDALDataContext.ADefHelpDesk_UserRoles
                               where ADefHelpDesk_UserRoles.UserID == intSelectedUserID
                               where ADefHelpDesk_UserRoles.RoleID == Convert.ToInt32(ddlUserRoles.SelectedValue)
                               select ADefHelpDesk_UserRoles).FirstOrDefault();

            if (objUserRole != null)
            {
                RoleController objRoleController = new RoleController();
                lblUserError.Text = String.Format(GetLocalResourceObject("RoleAlreadyAdded.Text").ToString(),
                    objRoleController.GetRole(PortalId,Convert.ToInt32(ddlUserRoles.SelectedValue)).RoleName);
                return;
            }
            else
            {
                ADefHelpDesk_UserRole objADefHelpDesk_UserRole = new ADefHelpDesk_UserRole();
                objADefHelpDesk_UserRole.UserID = intSelectedUserID;
                objADefHelpDesk_UserRole.RoleID = Convert.ToInt32(ddlUserRoles.SelectedValue);

                objdnnHelpDeskDALDataContext.ADefHelpDesk_UserRoles.InsertOnSubmit(objADefHelpDesk_UserRole);
                objdnnHelpDeskDALDataContext.SubmitChanges();
                gvUserRoles.DataBind();
            }
        }
        #endregion

        // Roles

        #region ldsRoles_Selecting
        protected void ldsRoles_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            e.WhereParameters["PortalID"] = PortalId;
        }
        #endregion

        #region btnInsertRole_Click
        protected void btnInsertRole_Click(object sender, EventArgs e)
        {
            txtRoleName.Text = txtRoleName.Text.Trim();

            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            // See if Role already exists
            ADefHelpDesk_Role colADefHelpDesk_Roles = (from ADefHelpDesk_Roles in objdnnHelpDeskDALDataContext.ADefHelpDesk_Roles
                                                       where ADefHelpDesk_Roles.PortalID == PortalId
                                                       where ADefHelpDesk_Roles.RoleName == txtRoleName.Text
                                                       select ADefHelpDesk_Roles).FirstOrDefault();
            if (colADefHelpDesk_Roles != null)
            {
                RoleController objRoleController = new RoleController();
                lblRoleError.Text = String.Format(GetLocalResourceObject("RoleAlreadyAdded.Text").ToString(), txtRoleName.Text);
            }
            else
            {
                ADefHelpDesk_Role objADefHelpDesk_Role = new ADefHelpDesk_Role();
                objADefHelpDesk_Role.PortalID = PortalId;
                objADefHelpDesk_Role.RoleName = txtRoleName.Text;

                objdnnHelpDeskDALDataContext.ADefHelpDesk_Roles.InsertOnSubmit(objADefHelpDesk_Role);
                objdnnHelpDeskDALDataContext.SubmitChanges();
                lvRoles.DataBind();

                txtRoleName.Text = "";
                lblRoleError.Text = "";
            }
        }
        #endregion

        #region lvRoles_ItemDeleting
        protected void lvRoles_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            dnnHelpDeskDALDataContext objdnnHelpDeskDALDataContext = new dnnHelpDeskDALDataContext();

            // Set Assigned role to -1 on any Tasks
            var ExistingTasks = from ADefHelpDesk_Tasks in objdnnHelpDeskDALDataContext.ADefHelpDesk_Tasks
                                where ADefHelpDesk_Tasks.AssignedRoleID == Convert.ToInt32(e.Keys["ID"])
                                select ADefHelpDesk_Tasks;

            foreach (var Task in ExistingTasks)
            {
                Task.AssignedRoleID = -1;
            }

            objdnnHelpDeskDALDataContext.SubmitChanges();

            // Delete entries in UserRole table
            var DeleteUserRoles = from ADefHelpDesk_UserRoles in objdnnHelpDeskDALDataContext.ADefHelpDesk_UserRoles
                                  where ADefHelpDesk_UserRoles.RoleID == Convert.ToInt32(e.Keys["ID"])
                                  select ADefHelpDesk_UserRoles;

            objdnnHelpDeskDALDataContext.ADefHelpDesk_UserRoles.DeleteAllOnSubmit(DeleteUserRoles);
            objdnnHelpDeskDALDataContext.SubmitChanges();
        }
        #endregion

        // Email Administration

        #region lnkEmailAdministration_Click
        protected void lnkEmailAdministration_Click(object sender, EventArgs e)
        {
            SetView("EmailAdministration");
            LoadGeneralSettings();
        } 
        #endregion

        #region LoadGeneralSettings
        private void LoadGeneralSettings()
        {
            GeneralSettings GeneralSettings = new GeneralSettings();
            txtSMTPEmailServer.Text = GeneralSettings.SMTPServer;
            rbAuthendication.SelectedValue = GeneralSettings.SMTPAuthendication;
            chkSecureAccess.Checked = GeneralSettings.SMTPSecure;
            txtSMTPUsername.Text = GeneralSettings.SMTPUserName;
            txtSMTPPassword.Text = GeneralSettings.SMTPPassword;
            txtSMTPFrom.Text = GeneralSettings.SMTPFromEmail;
            ddlUploadPermission.SelectedValue = GeneralSettings.UploadPermission;
            txtUploadedFilesPath.Text = GeneralSettings.FileUploadPath;
            ddlUserRegistration.SelectedValue = GeneralSettings.AllowRegistration.ToString();
            ddlVerifiedRegistration.SelectedValue = GeneralSettings.VerifiedRegistration.ToString();
            lblVersion.Text = String.Format("ADefHelpDesk Version: {0}", GeneralSettings.Version);
            // enable Verified Registration only if User Registration is true
            ddlVerifiedRegistration.Enabled = (ddlUserRegistration.SelectedValue == "True");
        }
        #endregion

        #region Update Settings
        protected void btnUpdateSettings_Click(object sender, EventArgs e)
        {
            try
            {
                GeneralSettings.UpdateSMTPServer(txtSMTPEmailServer.Text.Trim());
                GeneralSettings.UpdateSMTPAuthendication(rbAuthendication.SelectedValue);
                GeneralSettings.UpdateSMTPSecure(chkSecureAccess.Checked);
                GeneralSettings.UpdateSMTPUserName(txtSMTPUsername.Text.Trim());
                GeneralSettings.UpdateSMTPPassword(txtSMTPPassword.Text.Trim());
                GeneralSettings.UpdateSMTPFromEmail(txtSMTPFrom.Text.Trim());
            }
            catch (Exception ex)
            {
                lblUpdated.Text = ex.Message;
                return;
            }

            lblUpdated.Text = GetLocalResourceObject("Updated.Text").ToString();
        }
        #endregion

        #region Send Test Email
        protected void lnkTestEmail_Click(object sender, EventArgs e)
        {
            // Send Test Email
            // For normal emails use this constructor:
            // SendMail(string MailTo, string Cc, string Bcc, string ReplyTo, string Subject, string Body, string strAttachment)

            string[] arrAttachments = new string[0];
            string strEmailResponse = Email.SendMail(txtSMTPFrom.Text.Trim(), txtSMTPFrom.Text.Trim(), "", "", txtSMTPFrom.Text.Trim(), MailPriority.Normal, GetLocalResourceObject("ADefHelpDeskEmail.Text").ToString(), Encoding.UTF8, GetLocalResourceObject("ADefHelpDeskTestEmail.Text").ToString(),
            arrAttachments, txtSMTPEmailServer.Text.Trim(), rbAuthendication.SelectedValue, txtSMTPUsername.Text.Trim(), txtSMTPPassword.Text.Trim(), chkSecureAccess.Checked);

            lblUpdated.Text = (strEmailResponse.Trim() == "") ? GetLocalResourceObject("EmailSent.Text").ToString() : strEmailResponse;
        }
        #endregion

        // User Registration

        #region lnkUserRegistration_Click
        protected void lnkUserRegistration_Click(object sender, EventArgs e)
        {
            SetView("UserRegistration");
            LoadGeneralSettings();
        } 
        #endregion

        #region ddlUserRegistration_SelectedIndexChanged
        protected void ddlUserRegistration_SelectedIndexChanged(object sender, EventArgs e)
        {
            // enable Verified Registration only if User Registration is true
            ddlVerifiedRegistration.Enabled = (ddlUserRegistration.SelectedValue == "True");
            if (ddlUserRegistration.SelectedValue == "False")
            {
                ddlVerifiedRegistration.SelectedValue = "False";
            }
        } 
        #endregion

        #region btnUpdateUserRegistration_Click
        protected void btnUpdateUserRegistration_Click(object sender, EventArgs e)
        {
            try
            {
                GeneralSettings.UpdateAllowRegistration(Convert.ToBoolean(ddlUserRegistration.SelectedValue));
                GeneralSettings.UpdateVerifiedRegistration(Convert.ToBoolean(ddlVerifiedRegistration.SelectedValue));
            }
            catch (Exception ex)
            {
                lblUserRegistrationStatus.Text = ex.Message;
                return;
            }

            lblUserRegistrationStatus.Text = GetLocalResourceObject("lblUserRegistrationStatus.Text").ToString();
        } 
        #endregion

    }
}