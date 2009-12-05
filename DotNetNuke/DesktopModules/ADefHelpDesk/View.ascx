<%@ Control Language="C#" AutoEventWireup="true" CodeFile="View.ascx.cs" Inherits="ADefWebserver.Modules.ADefHelpDesk.View" %>
<%@ Register Src="Controls/Tags.ascx" TagName="Tags" TagPrefix="uc1" %>
<style type="text/css">
    .style1
    {
        color: #FF0000;
    }
    .style5
    {
        text-align: center;
    }
</style>
<table cellpadding="2">
    <tr>
        <td>
            <asp:Image ID="imgNewTicket" runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/layout_add.png" />
            <asp:LinkButton ID="lnkNewTicket" runat="server" BackColor="#CCCCCC" Font-Underline="True"
                OnClick="lnkNewTicket_Click" Text="New Ticket" resourcekey="lnkNewTicket" />
        </td>
        <td>
            <asp:Image ID="imgExitingTickets" runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/layout.png" />
            <asp:LinkButton ID="lnkExistingTickets" runat="server" Font-Underline="True" OnClick="lnkExistingTickets_Click"
                Text="Existing Tickets" resourcekey="lnkExistingTickets" />
        </td>
        <td>
            <asp:Image ID="imgMagnifier" runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/magnifier.png" />
            <asp:LinkButton ID="lnkResetSearch" runat="server" Font-Underline="True" OnClick="lnkResetSearch_Click"
                Text="Reset Search" resourcekey="lnkResetSearch" />
        </td>
        <td>
            <asp:Image ID="imgAdministrator" runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/cog.png" />
            <asp:LinkButton ID="lnkAdministratorSettings" runat="server" Font-Underline="True"
                OnClick="lnkAdministratorSettings_Click" Text="Administrator Settings" resourcekey="lnkAdministratorSettings" />
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
</table>
<asp:Panel ID="pnlNewTicket" runat="server">
    <table>
        <tr>
            <td align="left" colspan="2">
                <asp:Panel ID="pnlAdminUserSelection" runat="server" GroupingText="Select User">
                    <table border="0" cellpadding="4">
                        <tr>
                            <td nowrap="nowrap" valign="top">
                                <asp:DropDownList ID="ddlSearchForUserType" runat="server">
                                    <asp:ListItem Selected="True" Text="Email" resourcekey="ddlSearchForUserTypeEmail" />
                                    <asp:ListItem Value="FirstName" Text="First Name" resourcekey="ddlSearchForUserTypeFirstName" />
                                    <asp:ListItem Value="LastName" Text="Last Name" resourcekey="ddlSearchForUserTypeLastName" />
                                </asp:DropDownList>
                                <asp:TextBox ID="txtSearchForUser" runat="server" TabIndex="12"></asp:TextBox>
                                <asp:Button ID="btnSearchUser" runat="server" OnClick="btnSearchUser_Click" Text="Search"
                                    resourcekey="btnSearchUser" />
                            </td>
                            <td nowrap="nowrap" valign="top">
                                <asp:Label ID="lblCurrentProcessorNotFound" runat="server" Text="User is not found"
                                    Visible="False"></asp:Label>
                                <asp:GridView ID="gvCurrentProcessor" runat="server" AutoGenerateColumns="False"
                                    DataKeyNames="UserID" GridLines="None" OnSelectedIndexChanged="gvCurrentProcessor_SelectedIndexChanged"
                                    ShowHeader="False">
                                    <Columns>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandArgument='<%# Bind("UserID") %>'
                                                    CommandName="Select" Text='<%# Bind("DisplayName") %>'></asp:LinkButton>
                                                &nbsp;
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
            <td align="left" valign="top">
                <asp:Panel ID="pnlAdminTicketStatus" runat="server" Visible="False">
                    <asp:Label ID="lblStatus" runat="server" Font-Bold="True" Text="Status:" resourcekey="lblStatus" />
                    <asp:DropDownList ID="ddlStatusAdmin" runat="server" TabIndex="11">
                        <asp:ListItem resourcekey="ddlStatusAdminNew" Value="New" Text="New" />
                        <asp:ListItem resourcekey="ddlStatusAdminActive" Value="Active" Text="Active" />
                        <asp:ListItem resourcekey="ddlStatusAdminOnHold" Value="On Hold" Text="On Hold" />
                        <asp:ListItem resourcekey="ddlStatusAdminResolved" Value="Resolved" Text="Resolved" />
                        <asp:ListItem resourcekey="ddlStatusAdminCancelled" Value="Cancelled" Text="Cancelled" />
                    </asp:DropDownList>
                    &nbsp;<br />
                    <asp:Label ID="lblAssigned" runat="server" Font-Bold="True" Text="Assigned:" resourcekey="lblAssigned" />
                    <asp:DropDownList ID="ddlAssignedAdmin" runat="server" TabIndex="10">
                    </asp:DropDownList>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <table>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblRequiredfield" runat="server" Text="(*) Required Field" ForeColor="Red"
                                Font-Italic="True" Font-Size="XX-Small" resourcekey="lblRequiredfield" />
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b><span class="style1">*</span></b><asp:Label ID="lbltxtName" runat="server" Text="Name:"
                                resourcekey="lbltxtName" />
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server" MaxLength="350" TabIndex="1"></asp:TextBox>
                            <asp:Label ID="lblName" runat="server"></asp:Label>
                            <asp:TextBox ID="txtUserID" runat="server" Columns="1" Visible="False"></asp:TextBox>
                            <asp:Button ID="btnClearUser" runat="server" OnClick="btnClearUser_Click" Text="Clear User"
                                Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b><span class="style1">*</span></b><asp:Label ID="lbltxtEmail" runat="server" Text="Email:"
                                resourcekey="lbltxtEmail" />
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="350" TabIndex="2"></asp:TextBox>
                            <asp:Label ID="lblEmail" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lbltxtPhone" runat="server" Text="Phone:" resourcekey="lbltxtPhone" />
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhone" runat="server" MaxLength="50" TabIndex="3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b><span class="style1">*</span></b><asp:Label ID="lbltxtDescription" runat="server"
                                Text="Description:" resourcekey="lbltxtDescription" />
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescription" runat="server" Columns="50" MaxLength="50" TabIndex="4"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lbltxtDetail" runat="server" Text="Detail:" resourcekey="lbltxtDetail" />
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtDetails" runat="server" Columns="40" MaxLength="500" Rows="5"
                                TabIndex="5" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lbltxtDueDate" runat="server" Text="Date Due:" resourcekey="lbltxtDueDate" />
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtDueDate" runat="server" Columns="10" MaxLength="25" TabIndex="6"></asp:TextBox>
                            <asp:HyperLink ID="cmdStartCalendar" runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/calendar.png"></asp:HyperLink>
                            <asp:Label ID="lbltxtPriority" runat="server" Text="Priority:" resourcekey="lbltxtPriority" />
                            &nbsp;<asp:DropDownList ID="ddlPriority" runat="server" TabIndex="7">
                                <asp:ListItem Selected="True" resourcekey="ddlPriorityNormal" Value="Normal" Text="Normal" />
                                <asp:ListItem resourcekey="ddlPriorityHigh" Value="High" Text="High" />
                                <asp:ListItem resourcekey="ddlPriorityLow" Value="Low" Text="Low" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            &nbsp;<asp:Label ID="lblAttachFile" runat="server" Font-Bold="True" Text="Attach File:"
                                resourcekey="lblAttachFile" />
                        </td>
                        <td>
                            <asp:FileUpload ID="TicketFileUpload" runat="server" TabIndex="8" />
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
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:Button ID="btnSubmit" runat="server" Font-Bold="True" ForeColor="#FF3300" OnClick="btnSubmit_Click"
                                TabIndex="9" Text="Submit Ticket" resourcekey="btnSubmit" />
                            <br />
                            <br />
                            <asp:Label ID="lblError" runat="server" EnableViewState="False" Font-Bold="True"
                                ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td colspan="2" valign="top">
                <table>
                    <tr>
                        <td>
                            &nbsp;<asp:Image ID="Image1" runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/tag_blue.png" />
                            &nbsp;
                            <asp:Label ID="lblCheckTags" runat="server" Font-Bold="True" Text="Check all Tags that apply:"
                                resourcekey="lblCheckTags" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <uc1:Tags ID="TagsTree" runat="server" EnableViewState="True" Visible="False" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td dir="ltr">
                &nbsp;
            </td>
            <td colspan="2" dir="ltr">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td dir="ltr">
                &nbsp;
            </td>
            <td colspan="2" dir="ltr">
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="pnlConfirmAnonymousUserEntry" runat="server" Visible="False">
    <div class="style5">
        <h2>
            <asp:Label ID="lblTicketHasBeenSubmitted" resourcekey="lblTicketHasBeenSubmitted"
                runat="server" Text="Your Ticket has been submitted." /><br />
            <br />
            <asp:Label ID="lblConfirmAnonymousUser" runat="server"></asp:Label>
            <br />
            <br />
            <asp:LinkButton ID="lnlAnonymousContinue" runat="server" Font-Underline="True" OnClick="lnlAnonymousContinue_Click"
                Text="Click here to continue" resourcekey="lnlAnonymousContinue" />
        </h2>
    </div>
</asp:Panel>
<asp:Panel ID="pnlExistingTickets" runat="server">
    <table cellpadding="2">
        <tr>
            <td valign="top">
                <asp:ListView ID="lvTasks" runat="server" OnItemDataBound="lvTasks_ItemDataBound"
                    OnSorting="lvTasks_Sorting" OnItemCommand="lvTasks_ItemCommand" OnDataBound="lvTasks_DataBound">
                    <ItemTemplate>
                        <tr class='<%# Container.DataItemIndex % 2 == 0 ? "row" : "altrow" %>'>
                            <td align="center">
                                <asp:HyperLink ID="lnkTaskID" runat="server" Text='<%# Eval("TaskID") %>' Font-Underline="True" />
                            </td>
                            <td>
                                <asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("Status") %>' />
                            </td>
                            <td>
                                <asp:Label ID="PriorityLabel" runat="server" Text='<%# Eval("Priority") %>' />
                            </td>
                            <td>
                                <asp:Label ID="DueDateLabel" runat="server" Text='<%# Eval("DueDate") %>' />
                            </td>
                            <td nowrap="nowrap">
                                <asp:Label ID="CreatedDateLabel" runat="server" Text='<%# Eval("CreatedDate") %>' />
                            </td>
                            <td>
                                <asp:Label ID="AssignedRoleIDLabel" runat="server" Text='<%# Eval("Assigned") %>' />
                            </td>
                            <td nowrap="nowrap">
                                <asp:Label ID="DescriptionLabel" runat="server" Text='<%# Eval("Description") %>'
                                    ToolTip='<%# Eval("Description") %>' />
                            </td>
                            <td nowrap="nowrap">
                                <asp:Label ID="RequesterUserIDLabel" runat="server" Text='<%# Eval("Requester") %>'
                                    Visible="false" />
                                <asp:Label ID="RequesterNameLabel" runat="server" Text='<%# Eval("RequesterName") %>'
                                    ToolTip='<%# Eval("RequesterName") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <table id="EmptyDataTemplateTable" runat="server" class="yui-grid" cellspacing="0"
                            style="border-style: none">
                            <tr id="Tr2" runat="server">
                                <th id="Th9" runat="server" nowrap="nowrap" colspan="6" style="border-style: none">
                                </th>
                                <th id="Th8" runat="server" nowrap="nowrap" align="right" colspan="2" style="border: 1px solid #989898;">
                                    <asp:Label ID="lblSearchText1" runat="server" resourcekey="lblSearchText1" Text="Ticket # or Search Text:"
                                        ForeColor="#333333" />
                                </th>
                            </tr>
                            <tr id="Tr1" runat="server">
                                <th id="Th1" runat="server" style="border-style: none" nowrap="nowrap">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CommandName="EmptyDataTemplateSearch" />
                                </th>
                                <th id="Th2" runat="server" nowrap="nowrap">
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="60">
                                        <asp:ListItem resourcekey="ddlStatusAdminAll" Value="*All*" Text="*All*" />
                                        <asp:ListItem resourcekey="ddlStatusAdminNew" Value="New" Text="New" />
                                        <asp:ListItem resourcekey="ddlStatusAdminActive" Value="Active" Text="Active" />
                                        <asp:ListItem resourcekey="ddlStatusAdminOnHold" Value="On Hold" Text="On Hold" />
                                        <asp:ListItem resourcekey="ddlStatusAdminResolved" Value="Resolved" Text="Resolved" />
                                        <asp:ListItem resourcekey="ddlStatusAdminCancelled" Value="Cancelled" Text="Cancelled" />
                                    </asp:DropDownList>
                                </th>
                                <th id="Th3" runat="server" nowrap="nowrap">
                                    <asp:DropDownList ID="ddlPriority" runat="server" Width="50">
                                        <asp:ListItem resourcekey="ddlStatusAdminAll" Value="*All*" Text="*All*" />
                                        <asp:ListItem resourcekey="ddlPriorityNormal" Value="Normal" Text="Normal" />
                                        <asp:ListItem resourcekey="ddlPriorityHigh" Value="High" Text="High" />
                                        <asp:ListItem resourcekey="ddlPriorityLow" Value="Low" Text="Low" />
                                    </asp:DropDownList>
                                </th>
                                <th id="Th4" runat="server" nowrap="nowrap">
                                    <asp:TextBox ID="txtDue" runat="server" Width="70" />
                                </th>
                                <th id="Th5" runat="server" nowrap="nowrap">
                                    <asp:TextBox ID="txtCreated" runat="server" Width="70" />
                                </th>
                                <th id="Th6" runat="server" nowrap="nowrap">
                                    <asp:DropDownList ID="ddlAssigned" runat="server" Width="90" DataTextField="AssignedRoleID"
                                        DataValueField="Key" />
                                </th>
                                <th id="Th7" runat="server" nowrap="nowrap" colspan="2">
                                    <asp:TextBox ID="txtSearch" runat="server" Width="200" />
                                </th>
                            </tr>
                            <tr id="Tr3" runat="server">
                                <th id="Th10" runat="server" nowrap="nowrap">
                                    <asp:LinkButton ID="lnkTaskID" runat="server" CommandArgument="TaskID" Text="TaskID"
                                        Font-Underline="True" resourcekey="lnkTaskID" />
                                </th>
                                <th id="Th11" runat="server" nowrap="nowrap">
                                    <asp:LinkButton ID="lnkStatus" runat="server" CommandArgument="Status" Text="Status"
                                        Font-Underline="True" resourcekey="lnkStatus" />
                                </th>
                                <th id="Th12" runat="server" nowrap="nowrap">
                                    <asp:LinkButton ID="lnkPriority" runat="server" CommandArgument="Priority" Text="Priority"
                                        Font-Underline="True" resourcekey="lnkPriority" />
                                </th>
                                <th id="Th13" runat="server" nowrap="nowrap">
                                    <asp:LinkButton ID="lnkDueDate" runat="server" CommandArgument="DueDate" Text="Due"
                                        Font-Underline="True" resourcekey="lnkDueDate" />
                                </th>
                                <th id="Th14" runat="server" nowrap="nowrap">
                                    <asp:LinkButton ID="lnkCreatedDate" runat="server" CommandArgument="CreatedDate"
                                        Text="Created" Font-Underline="True" resourcekey="lnkCreatedDate" />
                                </th>
                                <th id="Th15" runat="server" nowrap="nowrap">
                                    <asp:LinkButton ID="lnkAssigned" runat="server" CommandArgument="Assigned" Text="Assigned"
                                        Font-Underline="True" resourcekey="lnkAssigned" />
                                </th>
                                <th id="Th16" runat="server" nowrap="nowrap">
                                    <asp:LinkButton ID="lnkDescription" runat="server" CommandArgument="Description"
                                        Text="Description" Font-Underline="True" resourcekey="lnkDescription" />
                                </th>
                                <th id="Th17" runat="server" nowrap="nowrap">
                                    <asp:LinkButton ID="lnkRequester" runat="server" CommandArgument="Requester" Text="Requester"
                                        Font-Underline="True" resourcekey="lnkRequester" />
                                </th>
                            </tr>
                            <tr runat="server">
                                <td colspan="8">
                                    No records returned
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table runat="server">
                            <tr runat="server">
                                <td runat="server">
                                    <table id="itemPlaceholderContainer" runat="server" class="yui-grid" cellspacing="0"
                                        style="border-style: none">
                                        <tr id="Tr2" runat="server">
                                            <th id="Th9" runat="server" nowrap="nowrap" colspan="6" style="border-style: none">
                                            </th>
                                            <th id="Th8" runat="server" nowrap="nowrap" align="right" colspan="2" style="border: 1px solid #989898;">
                                                <asp:Label ID="lblSearchText2" runat="server" resourcekey="lblSearchText2" Text="Ticket # or Search Text:"
                                                    ForeColor="#333333" />
                                            </th>
                                        </tr>
                                        <tr id="Tr1" runat="server">
                                            <th id="Th1" runat="server" style="border-style: none" nowrap="nowrap">
                                                <asp:Button ID="btnSearch" runat="server" Text="Search" resourcekey="btnSearch" CommandName="Search" />
                                            </th>
                                            <th id="Th2" runat="server" nowrap="nowrap">
                                                <asp:DropDownList ID="ddlStatus" runat="server" Width="60">
                                                    <asp:ListItem resourcekey="ddlStatusAdminAll" Value="*All*" Text="*All*" />
                                                    <asp:ListItem resourcekey="ddlStatusAdminNew" Value="New" Text="New" />
                                                    <asp:ListItem resourcekey="ddlStatusAdminActive" Value="Active" Text="Active" />
                                                    <asp:ListItem resourcekey="ddlStatusAdminOnHold" Value="On Hold" Text="On Hold" />
                                                    <asp:ListItem resourcekey="ddlStatusAdminResolved" Value="Resolved" Text="Resolved" />
                                                    <asp:ListItem resourcekey="ddlStatusAdminCancelled" Value="Cancelled" Text="Cancelled" />
                                                </asp:DropDownList>
                                            </th>
                                            <th id="Th3" runat="server" nowrap="nowrap">
                                                <asp:DropDownList ID="ddlPriority" runat="server" Width="50">
                                                    <asp:ListItem resourcekey="ddlStatusAdminAll" Value="*All*" Text="*All*" />
                                                    <asp:ListItem resourcekey="ddlPriorityNormal" Value="Normal" Text="Normal" />
                                                    <asp:ListItem resourcekey="ddlPriorityHigh" Value="High" Text="High" />
                                                    <asp:ListItem resourcekey="ddlPriorityLow" Value="Low" Text="Low" />
                                                </asp:DropDownList>
                                            </th>
                                            <th id="Th4" runat="server" nowrap="nowrap">
                                                <asp:TextBox ID="txtDue" runat="server" Width="70" />
                                            </th>
                                            <th id="Th5" runat="server" nowrap="nowrap">
                                                <asp:TextBox ID="txtCreated" runat="server" Width="70" />
                                            </th>
                                            <th id="Th6" runat="server" nowrap="nowrap">
                                                <asp:DropDownList ID="ddlAssigned" runat="server" Width="90" DataTextField="AssignedRoleID"
                                                    DataValueField="Key" />
                                            </th>
                                            <th id="Th7" runat="server" nowrap="nowrap" colspan="2">
                                                <asp:TextBox ID="txtSearch" runat="server" Width="200" />
                                            </th>
                                        </tr>
                                        <tr runat="server">
                                            <th runat="server" nowrap="nowrap">
                                                <asp:LinkButton ID="lnkTaskID" runat="server" CommandName="Sort" CommandArgument="TaskID"
                                                    Text="TaskID" Font-Underline="True" resourcekey="lnkTaskID" />
                                                <asp:ImageButton ID="TaskIDImage" CommandName="Sort" CommandArgument="TaskID" runat="server"
                                                    ImageUrl="~/DesktopModules/ADefHelpDesk/images/dt-arrow-dn.png" Visible="false" />
                                            </th>
                                            <th runat="server" nowrap="nowrap">
                                                <asp:LinkButton ID="lnkStatus" runat="server" CommandName="Sort" CommandArgument="Status"
                                                    Text="Status" Font-Underline="True" resourcekey="lnkStatus" />
                                                <asp:ImageButton ID="StatusImage" CommandName="Sort" CommandArgument="Status" runat="server"
                                                    ImageUrl="~/DesktopModules/ADefHelpDesk/images/dt-arrow-dn.png" Visible="false" />
                                            </th>
                                            <th runat="server" nowrap="nowrap">
                                                <asp:LinkButton ID="lnkPriority" runat="server" CommandName="Sort" CommandArgument="Priority"
                                                    Text="Priority" Font-Underline="True" resourcekey="lnkPriority" /><asp:ImageButton
                                                        ID="PriorityImage" CommandName="Sort" CommandArgument="Priority" runat="server"
                                                        ImageUrl="~/DesktopModules/ADefHelpDesk/images/dt-arrow-dn.png" Visible="false" />
                                            </th>
                                            <th runat="server" nowrap="nowrap">
                                                <asp:LinkButton ID="lnkDueDate" runat="server" CommandName="Sort" CommandArgument="DueDate"
                                                    Text="Due" Font-Underline="True" resourcekey="lnkDueDate" /><asp:ImageButton ID="DueDateImage"
                                                        CommandName="Sort" CommandArgument="DueDate" runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/dt-arrow-dn.png"
                                                        Visible="false" />
                                            </th>
                                            <th runat="server" nowrap="nowrap">
                                                <asp:LinkButton ID="lnkCreatedDate" runat="server" CommandName="Sort" CommandArgument="CreatedDate"
                                                    Text="Created" Font-Underline="True" resourcekey="lnkCreatedDate" />
                                                <asp:ImageButton ID="CreatedDateImage" CommandName="Sort" CommandArgument="CreatedDate"
                                                    runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/dt-arrow-dn.png"
                                                    Visible="false" />
                                            </th>
                                            <th runat="server" nowrap="nowrap">
                                                <asp:LinkButton ID="lnkAssigned" runat="server" CommandName="Sort" CommandArgument="Assigned"
                                                    Text="Assigned" Font-Underline="True" resourcekey="lnkAssigned" />
                                                <asp:ImageButton ID="AssignedImage" CommandName="Sort" CommandArgument="Assigned"
                                                    runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/dt-arrow-dn.png"
                                                    Visible="false" />
                                            </th>
                                            <th runat="server" nowrap="nowrap">
                                                <asp:LinkButton ID="lnkDescription" runat="server" CommandName="Sort" CommandArgument="Description"
                                                    Text="Description" Font-Underline="True" resourcekey="lnkDescription" />
                                                <asp:ImageButton ID="DescriptionImage" CommandName="Sort" CommandArgument="Description"
                                                    runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/dt-arrow-dn.png"
                                                    Visible="false" />
                                            </th>
                                            <th runat="server" nowrap="nowrap">
                                                <asp:LinkButton ID="lnkRequester" runat="server" CommandName="Sort" CommandArgument="Requester"
                                                    Text="Requester" Font-Underline="True" resourcekey="lnkRequester" />
                                                <asp:ImageButton ID="RequesterImage" CommandName="Sort" CommandArgument="Requester"
                                                    runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/dt-arrow-dn.png"
                                                    Visible="false" />
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr runat="server">
                                <td runat="server" style="">
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                </asp:ListView>
                <asp:Panel ID="pnlPaging" runat="server">
                    <asp:LinkButton runat="server" Font-Underline="True" Text="&lt;&lt;" ID="lnkFirst" OnClick="lnkFirst_Click" Visible="False" />
                    &nbsp;<asp:LinkButton ID="lnkPrevious" Font-Underline="True" runat="server" Text="&lt;" OnClick="lnkPrevious_Click" />
                    &nbsp;<asp:Label ID="lblRecords" runat="server"></asp:Label>
                    &nbsp;<asp:LinkButton ID="lnkNext" runat="server" Font-Underline="True" Text="&gt;" OnClick="lnkNext_Click" />
                    &nbsp;<asp:LinkButton ID="lnkLast" runat="server" Font-Underline="True" OnClick="lnkLast_Click" Text="&gt;&gt;" Visible="False" />
                    <asp:TextBox ID="txtCurrentPage" runat="server" Columns="1" Visible="False"></asp:TextBox>
                </asp:Panel>
                <br />
            </td>
            <td valign="top" nowrap="nowrap">
                <asp:Image ID="Image2" runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/tag_blue.png" />
                <b>&nbsp;<asp:Label ID="lblSearchTags" resourcekey="lblSearchTags" runat="server"
                    Text="Search Tags:" /><uc1:Tags ID="TagsTreeExistingTasks" runat="server" Visible="False" />
                </b>
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
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Panel>
