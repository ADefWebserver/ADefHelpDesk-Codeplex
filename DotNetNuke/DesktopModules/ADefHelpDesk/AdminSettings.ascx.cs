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

namespace ADefWebserver.Modules.ADefHelpDesk
{
    public partial class AdminSettings : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        List<int> colProcessedCategoryIDs;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // Get Admin Role
                string strAdminRoleID = GetAdminRole();
                // Only show if user is an Administrator
                if (!(UserInfo.IsInRole(strAdminRoleID) || UserInfo.IsInRole("Administrators") || UserInfo.IsSuperUser))
                {
                    pnlAdminSettings.Visible = false;
                    Response.Redirect(Globals.NavigateURL());
                }

                SetView("AdministratorRole");
                DisplayAdminRoleDropDown();
            }
        }

        #region SetView
        private void SetView(string ViewName)
        {
            if (ViewName == "AdministratorRole")
            {
                pnlAdministratorRole.Visible = true;
                pnlUploadefFilesPath.Visible = false;
                pnlTagsAdmin.Visible = false;
                pnlRoles.Visible = false;

                lnkAdminRole.Font.Bold = true;
                lnkAdminRole.BackColor = Color.LightGray;
                lnkUploadefFilesPath.Font.Bold = false;
                lnkUploadefFilesPath.BackColor = Color.Transparent;
                lnkTagsAdmin.Font.Bold = false;
                lnkTagsAdmin.BackColor = Color.Transparent;
                lnkRoles.Font.Bold = false;
                lnkRoles.BackColor = Color.Transparent;
            }

            if (ViewName == "UploadedFilesPath")
            {
                pnlAdministratorRole.Visible = false;
                pnlUploadefFilesPath.Visible = true;
                pnlTagsAdmin.Visible = false;
                pnlRoles.Visible = false;

                lnkAdminRole.Font.Bold = false;
                lnkAdminRole.BackColor = Color.Transparent;
                lnkUploadefFilesPath.Font.Bold = true;
                lnkUploadefFilesPath.BackColor = Color.LightGray;
                lnkTagsAdmin.Font.Bold = false;
                lnkTagsAdmin.BackColor = Color.Transparent;
                lnkRoles.Font.Bold = false;
                lnkRoles.BackColor = Color.Transparent;
            }

            if (ViewName == "Roles")
            {
                pnlAdministratorRole.Visible = false;
                pnlUploadefFilesPath.Visible = false;
                pnlTagsAdmin.Visible = false;
                pnlRoles.Visible = true;

                lnkAdminRole.Font.Bold = false;
                lnkAdminRole.BackColor = Color.Transparent;
                lnkUploadefFilesPath.Font.Bold = false;
                lnkUploadefFilesPath.BackColor = Color.Transparent;
                lnkTagsAdmin.Font.Bold = false;
                lnkTagsAdmin.BackColor = Color.Transparent;
                lnkRoles.Font.Bold = true;
                lnkRoles.BackColor = Color.LightGray;
            }

            if (ViewName == "TagsAdministration")
            {
                pnlAdministratorRole.Visible = false;
                pnlUploadefFilesPath.Visible = false;
                pnlTagsAdmin.Visible = true;
                pnlRoles.Visible = false;

                lnkAdminRole.Font.Bold = false;
                lnkAdminRole.BackColor = Color.Transparent;
                lnkUploadefFilesPath.Font.Bold = false;
                lnkUploadefFilesPath.BackColor = Color.Transparent;
                lnkTagsAdmin.Font.Bold = true;
                lnkTagsAdmin.BackColor = Color.LightGray;
                lnkRoles.Font.Bold = false;
                lnkRoles.BackColor = Color.Transparent;
            }
        }
        #endregion

        #region lnkBack_Click
        protected void lnkBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL());
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

        #region lnkAdminRole_Click
        protected void lnkAdminRole_Click(object sender, EventArgs e)
        {
            SetView("AdministratorRole");
            DisplayAdminRoleDropDown();
        }
        #endregion

        #region lnkUploadefFilesPath_Click
        protected void lnkUploadefFilesPath_Click(object sender, EventArgs e)
        {
            SetView("UploadedFilesPath");
            DisplayUploadedFilesPath();
        }
        #endregion

        #region MyRegion
        protected void lnkRoles_Click(object sender, EventArgs e)
        {
            SetView("Roles");
            DisplayRoles();
        }
        #endregion

        #region DisplayAdminRoleDropDown
        private void DisplayAdminRoleDropDown()
        {
            // Get all the Roles
            RoleController RoleController = new RoleController();
            ArrayList colArrayList = RoleController.GetRoles();

            // Create a ListItemCollection to hold the Roles 
            ListItemCollection colListItemCollection = new ListItemCollection();

            // Add the Roles to the List
            foreach (RoleInfo Role in colArrayList)
            {
                if (Role.PortalID == PortalId)
                {
                    ListItem RoleListItem = new ListItem();
                    RoleListItem.Text = Role.RoleName;
                    RoleListItem.Value = Role.RoleID.ToString();
                    colListItemCollection.Add(RoleListItem);
                }
            }

            // Add the Roles to the ListBox
            ddlAdminRole.DataSource = colListItemCollection;
            ddlAdminRole.DataBind();

            // Get Admin Role
            string strAdminRoleID = GetAdminRole();

            try
            {
                // Try to set the role
                ddlAdminRole.SelectedValue = strAdminRoleID;
            }
            catch
            {

            }
        }
        #endregion

        #region DisplayUploadedFilesPath
        private void DisplayUploadedFilesPath()
        {
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            ADefHelpDesk_Setting objADefHelpDesk_Setting = (from ADefHelpDesk_Settings in objADefHelpDeskDALDataContext.ADefHelpDesk_Settings
                                                            where ADefHelpDesk_Settings.PortalID == PortalId
                                                            where ADefHelpDesk_Settings.SettingName == "UploadefFilesPath"
                                                            select ADefHelpDesk_Settings).FirstOrDefault();

            txtUploadedFilesPath.Text = objADefHelpDesk_Setting.SettingValue;
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

        #region btnUpdateAdminRole_Click
        protected void btnUpdateAdminRole_Click(object sender, EventArgs e)
        {
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            ADefHelpDesk_Setting objADefHelpDesk_Setting = (from ADefHelpDesk_Settings in objADefHelpDeskDALDataContext.ADefHelpDesk_Settings
                                                            where ADefHelpDesk_Settings.PortalID == PortalId
                                                            where ADefHelpDesk_Settings.SettingName == "AdminRole"
                                                            select ADefHelpDesk_Settings).FirstOrDefault();


            objADefHelpDesk_Setting.SettingValue = ddlAdminRole.SelectedValue;
            objADefHelpDeskDALDataContext.SubmitChanges();

            lblAdminRole.Text = "Updated";
        }
        #endregion

        #region btnUploadedFiles_Click
        protected void btnUploadedFiles_Click(object sender, EventArgs e)
        {
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            ADefHelpDesk_Setting objADefHelpDesk_Setting = (from ADefHelpDesk_Settings in objADefHelpDeskDALDataContext.ADefHelpDesk_Settings
                                                            where ADefHelpDesk_Settings.PortalID == PortalId
                                                            where ADefHelpDesk_Settings.SettingName == "UploadefFilesPath"
                                                            select ADefHelpDesk_Settings).FirstOrDefault();

            objADefHelpDesk_Setting.SettingValue = txtUploadedFilesPath.Text.Trim();
            objADefHelpDeskDALDataContext.SubmitChanges();

            lblUploadedFilesPath.Text = "Updated";
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
            CategoriesDropDown colCategoriesDropDown = new CategoriesDropDown();
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

            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            var result = (from ADefHelpDesk_Categories in objADefHelpDeskDALDataContext.ADefHelpDesk_Categories
                          where ADefHelpDesk_Categories.CategoryID == intCategoryID
                          select ADefHelpDesk_Categories).FirstOrDefault();

            // Only build a node path if the current level is not the root
            if (result.Level > 1)
            {
                int intCurrentCategoryID = result.CategoryID;

                for (int i = 1; i < result.Level; i++)
                {
                    var CurrentCategory = (from ADefHelpDesk_Categories in objADefHelpDeskDALDataContext.ADefHelpDesk_Categories
                                           where ADefHelpDesk_Categories.CategoryID == intCurrentCategoryID
                                           select ADefHelpDesk_Categories).FirstOrDefault();

                    strNodePath = CurrentCategory.ParentCategoryID.ToString() + @"/" + strNodePath;

                    var ParentCategory = (from ADefHelpDesk_Categories in objADefHelpDeskDALDataContext.ADefHelpDesk_Categories
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
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            if (btnUpdate.Text == "Update")
            {
                var result = (from ADefHelpDesk_Categories in objADefHelpDeskDALDataContext.ADefHelpDesk_Categories
                              where ADefHelpDesk_Categories.CategoryID == Convert.ToInt32(txtCategoryID.Text)
                              select ADefHelpDesk_Categories).FirstOrDefault();

                result.CategoryName = txtCategory.Text.Trim();

                result.ParentCategoryID = (GetParentCategoryID(ddlParentCategory.SelectedValue) == "0") ? (int?)null : Convert.ToInt32(ddlParentCategory.SelectedValue);
                txtParentCategoryID.Text = (ddlParentCategory.SelectedValue == "0") ? "" : ddlParentCategory.SelectedValue;

                result.Level = (ddlParentCategory.SelectedValue == "0") ? 1 : GetLevelOfParent(Convert.ToInt32(ddlParentCategory.SelectedValue)) + 1;
                result.RequestorVisible = chkRequesterVisible.Checked;
                result.Selectable = chkSelectable.Checked;

                objADefHelpDeskDALDataContext.SubmitChanges();

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
                objADefHelpDesk_Category.Level = (ddlParentCategory.SelectedValue == "0") ? 1 : GetLevelOfParent(Convert.ToInt32(ddlParentCategory.SelectedValue)) + 1;
                objADefHelpDesk_Category.RequestorVisible = chkRequesterVisible.Checked;
                objADefHelpDesk_Category.Selectable = chkSelectable.Checked;

                objADefHelpDeskDALDataContext.ADefHelpDesk_Categories.InsertOnSubmit(objADefHelpDesk_Category);
                objADefHelpDeskDALDataContext.SubmitChanges();

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

            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            // Get the children of the current item
            // This method may be called from the top level or recuresively by one of the child items
            var CategoryChildren = from ADefHelpDesk_Categories in objADefHelpDeskDALDataContext.ADefHelpDesk_Categories
                                   where ADefHelpDesk_Categories.ParentCategoryID == result.CategoryID
                                   where !colProcessedCategoryIDs.Contains(result.CategoryID)
                                   select ADefHelpDesk_Categories;

            // Loop thru each item
            foreach (var objCategory in CategoryChildren)
            {
                colProcessedCategoryIDs.Add(objCategory.CategoryID);

                objCategory.Level = ((intStartingLevel) ?? 0) + 1;
                objADefHelpDeskDALDataContext.SubmitChanges();

                //Recursively call the UpdateLevelOfChildren method adding all children
                UpdateLevelOfChildren(objCategory);
            }
        }
        #endregion

        #region GetLevelOfParent
        private int? GetLevelOfParent(int? ParentCategoryID)
        {
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            var result = (from ADefHelpDesk_Categories in objADefHelpDeskDALDataContext.ADefHelpDesk_Categories
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

            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            var result = (from ADefHelpDesk_Categories in objADefHelpDeskDALDataContext.ADefHelpDesk_Categories
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
            if (btnAddNew.Text == "Add New")
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
            btnAddNew.Text = "Cancel";
            btnUpdate.Text = "Save";
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
            btnAddNew.Text = "Add New";
            btnUpdate.Text = "Update";
            btnDelete.Visible = true;
        }
        #endregion

        #region btnDelete_Click
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            // Get the node
            var result = (from ADefHelpDesk_Categories in objADefHelpDeskDALDataContext.ADefHelpDesk_Categories
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
                TmpADefHelpDesk_Category.Level = GetLevelOfParent(result.ParentCategoryID);
            }

            // Get all TaskCategories that use the Node
            var colTaskCategories = from ADefHelpDesk_TaskCategories in objADefHelpDeskDALDataContext.ADefHelpDesk_TaskCategories
                                    where ADefHelpDesk_TaskCategories.CategoryID == Convert.ToInt32(txtCategoryID.Text)
                                    select ADefHelpDesk_TaskCategories;

            // Delete them
            objADefHelpDeskDALDataContext.ADefHelpDesk_TaskCategories.DeleteAllOnSubmit(colTaskCategories);
            objADefHelpDeskDALDataContext.SubmitChanges();

            // Delete the node
            objADefHelpDeskDALDataContext.ADefHelpDesk_Categories.DeleteOnSubmit(result);
            objADefHelpDeskDALDataContext.SubmitChanges();

            // Update levels of all the Children            
            UpdateLevelOfChildren(TmpADefHelpDesk_Category);

            // Update all the children nodes to give them a new parent
            var CategoryChildren = from ADefHelpDesk_Categories in objADefHelpDeskDALDataContext.ADefHelpDesk_Categories
                                   where ADefHelpDesk_Categories.ParentCategoryID == result.CategoryID
                                   select ADefHelpDesk_Categories;

            // Loop thru each item
            foreach (var objCategory in CategoryChildren)
            {
                objCategory.ParentCategoryID = result.ParentCategoryID;
                objADefHelpDeskDALDataContext.SubmitChanges();
            }

            // Delete the Catagory from any Ticket that uses it
            var DeleteHelpDesk_TaskCategories = from ADefHelpDesk_TaskCategories in objADefHelpDeskDALDataContext.ADefHelpDesk_TaskCategories
                                                where ADefHelpDesk_TaskCategories.CategoryID == TmpADefHelpDesk_Category.CategoryID
                                                select ADefHelpDesk_TaskCategories;

            objADefHelpDeskDALDataContext.ADefHelpDesk_TaskCategories.DeleteAllOnSubmit(DeleteHelpDesk_TaskCategories);
            objADefHelpDeskDALDataContext.SubmitChanges();

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
                e.Node.ImageUrl = Convert.ToBoolean(objListItem.Attributes["RequestorVisible"]) ? "images/world.png" : "images/world_delete.png";
                e.Node.ToolTip = Convert.ToBoolean(objListItem.Attributes["RequestorVisible"]) ? "Requestor Visible" : "Requestor Not Visible";
            }
            else
            {
                e.Node.ImageUrl = "images/table.png";
                e.Node.ToolTip = Convert.ToBoolean(objListItem.Attributes["RequestorVisible"]) ? "Requestor Visible" : "Requestor Not Visible";
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

        #region lvRoles_ItemDataBound
        protected void lvRoles_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ListViewDataItem objListViewDataItem = (ListViewDataItem)e.Item;
            Label RoleIDLabel = (Label)e.Item.FindControl("RoleIDLabel");

            try
            {
                RoleController objRoleController = new RoleController();
                RoleIDLabel.Text = String.Format("{0}", objRoleController.GetRole(Convert.ToInt32(RoleIDLabel.Text), PortalId).RoleName);
            }
            catch (Exception)
            {
                RoleIDLabel.Text = "(Role Deleted)";
            }
        }
        #endregion

        #region btnInsertRole_Click
        protected void btnInsertRole_Click(object sender, EventArgs e)
        {
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            // See if Role already exists
            ADefHelpDesk_Role colADefHelpDesk_Roles = (from ADefHelpDesk_Roles in objADefHelpDeskDALDataContext.ADefHelpDesk_Roles
                                                       where ADefHelpDesk_Roles.PortalID == PortalId
                                                       where ADefHelpDesk_Roles.RoleID == Convert.ToInt32(ddlRole.SelectedValue)
                                                       select ADefHelpDesk_Roles).FirstOrDefault();
            if (colADefHelpDesk_Roles != null)
            {
                RoleController objRoleController = new RoleController();
                lblRoleError.Text = String.Format("Role '{0}' is already added.", objRoleController.GetRole(Convert.ToInt32(ddlRole.SelectedValue), PortalId).RoleName);
            }
            else
            {
                ADefHelpDesk_Role objADefHelpDesk_Role = new ADefHelpDesk_Role();
                objADefHelpDesk_Role.PortalID = PortalId;
                objADefHelpDesk_Role.RoleID = Convert.ToInt32(ddlRole.SelectedValue);

                objADefHelpDeskDALDataContext.ADefHelpDesk_Roles.InsertOnSubmit(objADefHelpDesk_Role);
                objADefHelpDeskDALDataContext.SubmitChanges();

                lvRoles.DataBind();
            }
        }
        #endregion

        #region DisplayRoles
        private void DisplayRoles()
        {
            // Get all the Roles
            RoleController RoleController = new RoleController();
            ArrayList colArrayList = RoleController.GetRoles();

            // Create a ListItemCollection to hold the Roles 
            ListItemCollection colListItemCollection = new ListItemCollection();

            // Add the Roles to the List
            foreach (RoleInfo Role in colArrayList)
            {
                if (Role.PortalID == PortalId)
                {
                    ListItem RoleListItem = new ListItem();
                    RoleListItem.Text = Role.RoleName;
                    RoleListItem.Value = Role.RoleID.ToString();
                    colListItemCollection.Add(RoleListItem);
                }
            }

            // Add the Roles to the ListBox
            ddlRole.DataSource = colListItemCollection;
            ddlRole.DataBind();
        }
        #endregion
    }
}