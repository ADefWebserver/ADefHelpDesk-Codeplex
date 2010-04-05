<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminSettings.ascx.cs"
    Inherits="ADefWebserver.Modules.ADefHelpDesk.AdminSettings" %>
<asp:Panel ID="pnlAdminSettings" runat="server" align="left">
    <asp:Image ID="Image3" runat="server" ImageUrl="~/images/application_side_contract.png" />
    <asp:LinkButton ID="lnkBack" runat="server" meta:resourcekey="lnkBack" Font-Underline="True"
        OnClick="lnkBack_Click" Text="Back" />
    <br />
    <br />
    <table cellpadding="2">
        <tr>
            <td valign="top" align="left" nowrap="nowrap">
                <p>
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/user_suit.png" />
                    <asp:LinkButton ID="lnkUserManager" runat="server" Font-Underline="True" OnClick="lnkUserManager_Click"
                        meta:resourcekey="lnkAdminRole" Text="User Manager" />
                </p>
                <p>
                    <asp:Image ID="Image5" runat="server" ImageUrl="~/images/group.png" Height="16px" />
                    <asp:LinkButton ID="lnkRoles" runat="server" Font-Underline="True" OnClick="lnkRoles_Click"
                        meta:resourcekey="lnkRoles" Text="Roles" />
                </p>
                                <p>
                    <asp:Image ID="Image7" runat="server" ImageUrl="~/images/key_go.png" />
                    <asp:LinkButton ID="lnkUserRegistration" runat="server" Font-Underline="True" meta:resourcekey="lnkUserRegistration"
                        Text="User Registration" OnClick="lnkUserRegistration_Click" />
                </p>
                <p>
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/images/tag_blue.png" />
                    <asp:LinkButton ID="lnkTagsAdmin" runat="server" Font-Underline="True" OnClick="lnkTagsAdmin_Click"
                        meta:resourcekey="lnkTagsAdmin" Text="Tags Administration" />
                </p>
                <p>
                    <asp:Image ID="Image6" runat="server" ImageUrl="~/images/email_link.png" />
                    &nbsp;<asp:LinkButton ID="lnkEmailAdministration" runat="server" Font-Underline="True"
                        meta:resourcekey="lnkEmailAdministration" Text="Email Administration" OnClick="lnkEmailAdministration_Click" />
                </p>
                <p>
                    <asp:Image ID="Image4" runat="server" ImageUrl="~/images/folder.png" Height="16px" />
                    <asp:LinkButton ID="lnkUploadefFilesPath" runat="server" Font-Underline="True" OnClick="lnkUploadefFilesPath_Click"
                        meta:resourcekey="lnkUploadefFilesPath" Text="File Upload Settings" />
                </p>
            </td>
            <td valign="top">
                &nbsp;
            </td>
            <td align="left" valign="top">
                <asp:Panel ID="pnlUserRegistration" runat="server" BorderColor="#CCCCCC" 
                    BorderStyle="Solid" Visible="False">
                    <table cellpadding="2" style="width: 417px">
                        <tr>
                            <td >
                                &nbsp;</td>
                            <td >
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblAllowRegistration" runat="server" 
                                    Text="Allow User Registration:" meta:resourcekey="lblAllowRegistration" />
                            </td>
                            <td >
                                <asp:DropDownList ID="ddlUserRegistration" runat="server" AutoPostBack="True" 
                                    onselectedindexchanged="ddlUserRegistration_SelectedIndexChanged">
                                    <asp:ListItem Text="True" Value="True" meta:resourcekey="ddlRegistrationTrue" />
                                    <asp:ListItem Text="False" Value="False" meta:resourcekey="ddlRegistrationFalse" />
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblVerifiedRegistration" runat="server" 
                                    Text="Verified Registration:" meta:resourcekey="lblVerifiedRegistration"/>
                            </td>
                            <td >
                                <asp:DropDownList ID="ddlVerifiedRegistration" runat="server">
                                    <asp:ListItem Text="True" Value="True" meta:resourcekey="ddlRegistrationTrue" />
                                    <asp:ListItem Text="False" Value="False" meta:resourcekey="ddlRegistrationFalse" />
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td >
                                &nbsp;</td>
                            <td >
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblUserRegistrationStatus" runat="server" 
                                    EnableViewState="False" Font-Italic="True" ForeColor="Red" />
                            </td>
                            <td>
                                <asp:Button ID="btnUpdateUserRegistration" runat="server" 
                                    meta:resourcekey="btnUpdateUserRegistration" 
                                    onclick="btnUpdateUserRegistration_Click" Text="Update" />
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlUserAdministration" runat="server" BorderColor="#CCCCCC" BorderStyle="Solid">
                    <table cellpadding="1">
                        <tr>
                            <td valign="top">
                                <asp:TextBox ID="txtUserSearch" runat="server" Columns="20"></asp:TextBox>
                                <asp:Button ID="btnUserSearch" runat="server" Text="Search" meta:resourcekey="btnUserSearch"
                                    OnClick="btnUserSearch_Click" />
                                <asp:Button ID="btnResetSearch" runat="server" meta:resourcekey="btnResetSearch"
                                    OnClick="btnResetSearch_Click" Text="Reset" />
                                <asp:ListView ID="lvUsers" runat="server" DataKeyNames="UserID" DataSourceID="LDSUsers"
                                    OnItemDataBound="lvUsers_ItemDataBound">
                                    <ItemTemplate>
                                        <tr class='<%# Container.DataItemIndex % 2 == 0 ? "row" : "altrow" %>'>
                                            <td>
                                                <asp:LinkButton ID="lnkUsername" runat="server" CommandArgument='<%# Eval("UserID") %>'
                                                    Font-Underline="True" OnClick="lnkUsername_Click" Text='<%# Eval("Username") %>'
                                                    ToolTip='<%# Eval("Username") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="FirstNameLabel" runat="server" Text='<%# Eval("FirstName") %>' ToolTip='<%# Eval("FirstName") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="LastNameLabel" runat="server" Text='<%# Eval("LastName") %>' ToolTip='<%# Eval("LastName") %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        <table runat="server" class="yui-grid">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblNoRecords" runat="server" ForeColor="#333333" meta:resourcekey="lblNoRecords"
                                                        Text="No Records Returned" />
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table runat="server" width="100%">
                                            <tr runat="server">
                                                <td runat="server">
                                                    <table id="itemPlaceholderContainer" runat="server" border="0" class="yui-grid" width="100%">
                                                        <tr runat="server" style="">
                                                            <th runat="server">
                                                                <asp:Label ID="UsernameHeader" runat="server" meta:resourcekey="UsernameHeader" Text="User Name" />
                                                            </th>
                                                            <th runat="server">
                                                                <asp:Label ID="FirstNameHeader" runat="server" meta:resourcekey="FirstNameHeader"
                                                                    Text="First Name" />
                                                            </th>
                                                            <th runat="server">
                                                                <asp:Label ID="LastNameHeader" runat="server" meta:resourcekey="LastNameHeader" Text="Last Name" />
                                                            </th>
                                                        </tr>
                                                        <tr id="itemPlaceholder" runat="server">
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr runat="server">
                                                <td runat="server" style="">
                                                    <asp:DataPager ID="DataPager1" runat="server" PageSize="20">
                                                        <Fields>
                                                            <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowNextPageButton="False"
                                                                ShowPreviousPageButton="False" />
                                                            <asp:NumericPagerField />
                                                            <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" ShowNextPageButton="False"
                                                                ShowPreviousPageButton="False" />
                                                        </Fields>
                                                    </asp:DataPager>
                                                </td>
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                </asp:ListView>
                                <asp:LinqDataSource ID="LDSUsers" runat="server" ContextTypeName="ADefWebserver.Modules.ADefHelpDesk.dnnHelpDeskDALDataContext"
                                    OnSelecting="LDSUsers_Selecting" OrderBy="LastName" TableName="ADefHelpDesk_Users">
                                </asp:LinqDataSource>
                                <asp:Label ID="lblSelectedUserID" runat="server" meta:resourcekey="lblSelectedUserID"
                                    Visible="False" />
                            </td>
                            <td valign="top">
                                <table cellpadding="2" cellspacing="2" align="center" width="300" style="border: medium solid #C0C0C0">
                                    <tr>
                                        <td align="right" nowrap="nowrap">
                                            &nbsp;
                                        </td>
                                        <td align="right">
                                            <asp:CheckBox ID="chkSuperUser" runat="server" Text="Super User" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" nowrap="nowrap">
                                            <asp:Label ID="lblUserName" runat="server" Text="User Name:" meta:resourcekey="lblUserName" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUserName" runat="server" MaxLength="100" ReadOnly="True"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" nowrap="nowrap">
                                            <asp:Label ID="lblFirstName" runat="server" Text="First Name:" meta:resourcekey="lblFirstName" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" nowrap="nowrap">
                                            <asp:Label ID="lblLastName" runat="server" Text="Last Name:" meta:resourcekey="lblLastName" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLastName" runat="server" MaxLength="50"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" nowrap="nowrap">
                                            <asp:Label ID="lblEmail" runat="server" Text="Email:" meta:resourcekey="lblEmail" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEmail" runat="server" Columns="30" MaxLength="256"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" nowrap="nowrap">
                                            <asp:Label ID="lblPassword" runat="server" Text="Password:" meta:resourcekey="lblPassword" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPasswordInsert" runat="server" MaxLength="50" TextMode="Password">**********</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" nowrap="nowrap">
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" nowrap="nowrap" colspan="2">
                                            <asp:Button ID="btnAddUser" runat="server" Text="Add New" OnClick="btnAddUser_Click"
                                                meta:resourcekey="btnAddUser" />
                                            <asp:Button ID="btnSaveUser" runat="server" Text="Save" OnClick="btnSaveUser_Click"
                                                meta:resourcekey="btnSaveUser" />
                                            <asp:Button ID="btnDeleteUser" runat="server" Text="Delete" OnClientClick='if (!confirm("Are you sure you want to delete?") ){return false;}'
                                                meta:resourcekey="btnDeleteUser" OnClick="btnDeleteUser_Click" />
                                            <asp:Button ID="btnCancelUser" runat="server" Text="Cancel" OnClick="btnCancelUser_Click"
                                                meta:resourcekey="btnCancelUser" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" nowrap="nowrap" colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2" nowrap="nowrap">
                                            <table cellpadding="2">
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="gvUserRoles" runat="server" AutoGenerateColumns="False" CssClass="yui-grid"
                                                            DataKeyNames="ID" DataSourceID="LDSUserRoles" HorizontalAlign="Center" OnRowDeleting="gvUserRoles_RowDeleting"
                                                            ShowHeader="False" Width="90%">
                                                            <RowStyle CssClass="row" />
                                                            <Columns>
                                                                <asp:CommandField ShowDeleteButton="True" />
                                                                <asp:BoundField DataField="RoleName" SortExpression="RoleName">
                                                                    <ItemStyle Width="100%" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                            <AlternatingRowStyle CssClass="altrow" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="ddlUserRoles" runat="server" TabIndex="10">
                                                        </asp:DropDownList>
                                                        <asp:Button ID="btnAddUserRole" runat="server" Text="Add Role" meta:resourcekey="btnAddUserRole"
                                                            OnClick="btnAddUserRole_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:LinqDataSource ID="LDSUserRoles" runat="server" ContextTypeName="ADefWebserver.Modules.ADefHelpDesk.dnnHelpDeskDALDataContext"
                                                EnableDelete="True" OnSelecting="LDSUserRoles_Selecting" OrderBy="RoleName" TableName="ADefHelpDesk_Roles">
                                            </asp:LinqDataSource>
                                            <asp:Label ID="lblUserError" runat="server" EnableViewState="False" Font-Italic="True"
                                                ForeColor="#CC3300"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                </asp:Panel>
                <asp:Panel ID="pnlUploadefFilesPath" runat="server" BorderColor="#CCCCCC" BorderStyle="Solid">
                    <table cellpadding="0">
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap" align="right">
                                <asp:Label ID="lbltxtFileUploadPath" runat="server" meta:resourcekey="lbltxtFileUploadPath"
                                    Text="File Upload Path:" />
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtUploadedFilesPath" runat="server" Columns="50" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="right" nowrap="nowrap">
                                <asp:Label ID="lbltxtFileUploadPermission" runat="server" meta:resourcekey="lbltxtFileUploadPermission"
                                    Text="File Upload Permission:" />
                                &nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlUploadPermission" runat="server">
                                    <asp:ListItem Selected="True" Text="All" Value="All" meta:resourcekey="ddlUploadPermissionAll" />
                                    <asp:ListItem Text="Administrator" Value="Administrator" meta:resourcekey="ddlUploadPermissionAdministrator" />
                                    <asp:ListItem Text="Administrator/Registered Users" Value="Administrator/Registered Users"
                                        meta:resourcekey="ddlUploadPermissionAdminRegUser" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnUploadedFiles" runat="server" Text="Update" meta:resourcekey="btnUpdateAdminRole"
                                    OnClick="btnUploadedFiles_Click" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblUploadedFilesPath" runat="server" EnableViewState="False" Font-Italic="True"
                                    ForeColor="#CC3300"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlRoles" runat="server" BorderColor="#CCCCCC" BorderStyle="Solid">
                    <table cellpadding="0">
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:ListView ID="lvRoles" runat="server" DataKeyNames="ID" DataSourceID="LDSRoles"
                                    OnItemDeleting="lvRoles_ItemDeleting">
                                    <ItemTemplate>
                                        <tr style="">
                                            <td>
                                                <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" OnClientClick='if (!confirm("Are you sure you want to delete?") ){return false;}'
                                                    Text="Delete" />
                                                <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                                            </td>
                                            <td>
                                                <asp:Label ID="RoleNameLabel" runat="server" Text='<%# Eval("RoleName") %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        <table id="Table1" runat="server" style="">
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="Table2" runat="server">
                                            <tr id="Tr1" runat="server">
                                                <td id="Td1" runat="server">
                                                    <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                                                        <tr id="Tr2" runat="server" style="">
                                                            <th id="Th1" runat="server">
                                                            </th>
                                                            <th id="Th2" runat="server">
                                                            </th>
                                                        </tr>
                                                        <tr id="itemPlaceholder" runat="server">
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <EditItemTemplate>
                                        <tr style="">
                                            <td>
                                                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                                                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="RoleNameTextBox" runat="server" Text='<%# Bind("RoleName") %>' />
                                            </td>
                                        </tr>
                                    </EditItemTemplate>
                                </asp:ListView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                &nbsp;<asp:Button ID="btnInsertRole" runat="server" meta:resourcekey="btnInsertRole"
                                    OnClick="btnInsertRole_Click" Text="Insert" />
                                &nbsp;<asp:TextBox ID="txtRoleName" runat="server" Columns="40" MaxLength="50" Rows="1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblRoleError" runat="server" Font-Italic="True" ForeColor="Red" EnableViewState="False"></asp:Label>
                                <asp:LinqDataSource ID="LDSRoles" runat="server" ContextTypeName="ADefWebserver.Modules.ADefHelpDesk.dnnHelpDeskDALDataContext"
                                    EnableDelete="True" EnableUpdate="True" OrderBy="RoleName" TableName="ADefHelpDesk_Roles">
                                </asp:LinqDataSource>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlTagsAdmin" runat="server" BorderColor="#CCCCCC" BorderStyle="Solid">
                    <table cellpadding="8" cellspacing="8">
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblTagError" runat="server" EnableViewState="False" Font-Italic="True"
                                    ForeColor="Red"></asp:Label>
                                <asp:TreeView ID="tvCategories" runat="server" ExpandDepth="0" OnSelectedNodeChanged="tvCategories_SelectedNodeChanged"
                                    BorderColor="#CCCCCC" BorderStyle="Solid" OnTreeNodeDataBound="tvCategories_TreeNodeDataBound">
                                    <SelectedNodeStyle BackColor="#CCCCCC" Font-Bold="False" Font-Underline="False" />
                                    <DataBindings>
                                        <asp:TreeNodeBinding DataMember="ADefWebserver.Modules.ADefHelpDesk.Catagories" Depth="0"
                                            TextField="Value" ValueField="Value" />
                                    </DataBindings>
                                </asp:TreeView>
                            </td>
                            <td valign="top">
                                <table bgcolor="#CCCCCC" cellpadding="2">
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="lblCategory" runat="server" meta:resourcekey="lblCategory" Text="Tag:" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCategory" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="lblParentCategory" runat="server" meta:resourcekey="lblParentCategory"
                                                            Text="Parent Tag:" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlParentCategory" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" colspan="2">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" colspan="2" nowrap="nowrap">
                                                        <asp:CheckBox ID="chkRequesterVisible" runat="server" Checked="True" meta:resourcekey="chkRequesterVisible"
                                                            Text="Requester Visible" ToolTip="This option will be visible to users making a ticket request" />
                                                        &nbsp;
                                                        <asp:CheckBox ID="chkSelectable" runat="server" Checked="True" Text="Selectable"
                                                            meta:resourcekey="chkSelectable" ToolTip="Is a user able to select this option or is it just used for grouping?" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtCategoryID" runat="server" Columns="1" Visible="False"></asp:TextBox>
                                                        <asp:TextBox ID="txtParentCategoryID" runat="server" Columns="1" Visible="False"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <asp:Button ID="btnUpdate" runat="server" ForeColor="Blue" OnClick="btnUpdate_Click"
                                                            Text="Update" CommandName="Update" meta:resourcekey="btnUpdate" />
                                                        &nbsp;
                                                        <asp:Button ID="btnAddNew" runat="server" ForeColor="Green" OnClick="btnAddNew_Click"
                                                            Text="Add New" CommandName="AddNew" meta:resourcekey="btnAddNew" />
                                                        &nbsp;
                                                        <asp:Button ID="btnDelete" runat="server" ForeColor="Red" OnClick="btnDelete_Click"
                                                            OnClientClick='if (!confirm("Are you sure you want to delete?") ){return false;}'
                                                            Text="Delete" meta:resourcekey="btnDelete" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlEmail" runat="server" BorderColor="#CCCCCC" BorderStyle="Solid">
                    <table cellpadding="4">
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;<asp:Label ID="lblSMPTServer" meta:resourcekey="lblSMPTServer" runat="server"
                                    Text="SMTP Server and port:" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtSMTPEmailServer" runat="server" MaxLength="50" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSMTPAuthendication" meta:resourcekey="lblSMTPAuthendication" runat="server"
                                    Text="SMTP Authendication:" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rbAuthendication" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Selected="True" Value="0" Text="Anonymous" meta:resourcekey="rbAuthendicationAnonymous" />
                                    <asp:ListItem Value="1" Text="Basic" meta:resourcekey="rbAuthendicationBasic" />
                                    <asp:ListItem Value="2" Text="NTLM" meta:resourcekey="rbAuthendicationNTLM" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblUsesecureSMTP" meta:resourcekey="lblUsesecureSMTP" runat="server"
                                    Text="Use secure SMTP access:" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="chkSecureAccess" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSMTPUsername" meta:resourcekey="lblSMTPUsername" runat="server"
                                    Text="SMTP Username:" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtSMTPUsername" runat="server" MaxLength="50" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSMTPPassword" meta:resourcekey="lblSMTPPassword" runat="server"
                                    Text="SMTP Password:" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtSMTPPassword" runat="server" MaxLength="50" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSMTPFromAddress" meta:resourcekey="lblSMTPFromAddress" runat="server"
                                    Text="SMTP &quot;From&quot; email address: " />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtSMTPFrom" runat="server" Columns="40" MaxLength="50" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkTestEmail" meta:resourcekey="lnkTestEmail" runat="server"
                                    Text="[Send Test Email]" OnClick="lnkTestEmail_Click" ToolTip="This uses the settings on the form right now, not the last saved settings." />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:ValidationSummary ID="VSummary" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Button ID="btnUpdateSettings" meta:resourcekey="btnUpdateSettings" runat="server"
                                    OnClick="btnUpdateSettings_Click" Text="Update Settings" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblUpdated" runat="server" EnableViewState="False" ForeColor="Blue" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Panel>
