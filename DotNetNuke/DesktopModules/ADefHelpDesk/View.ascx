<%@ Control Language="C#" AutoEventWireup="true" CodeFile="View.ascx.cs" Inherits="ADefWebserver.Modules.ADefHelpDesk.View" %>
<%@ Register Src="Controls/Tags.ascx" TagName="Tags" TagPrefix="uc1" %>
<style type="text/css">
    .style3
    {
        color: #FF0000;
        font-style: italic;
        font-size: xx-small;
    }
    .style4
    {
        font-style: italic;
        font-size: xx-small;
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
                OnClick="lnkNewTicket_Click">New Ticket</asp:LinkButton>
        </td>
        <td>
            <asp:Image ID="imgExitingTickets" runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/layout.png" />
            <asp:LinkButton ID="lnkExistingTickets" runat="server" Font-Underline="True" OnClick="lnkExistingTickets_Click">Existing Tickets</asp:LinkButton>
        </td>
        <td>
            <asp:Image ID="imgMagnifier" runat="server" 
                ImageUrl="~/DesktopModules/ADefHelpDesk/images/magnifier.png" />
            <asp:LinkButton ID="lnkResetSearch" runat="server" Font-Underline="True"
                OnClick="lnkResetSearch_Click">Reset Search</asp:LinkButton>
        </td>
        <td>
            <asp:Image ID="imgAdministrator" runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/cog.png" />
            <asp:LinkButton ID="lnkAdministratorSettings" runat="server" Font-Underline="True"
                OnClick="lnkAdministratorSettings_Click">Administrator Settings</asp:LinkButton>
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
                                    <asp:ListItem Selected="True">Email</asp:ListItem>
                                    <asp:ListItem Value="FirstName">First Name</asp:ListItem>
                                    <asp:ListItem Value="LastName">Last Name</asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtSearchForUser" runat="server" TabIndex="12"></asp:TextBox>
                                <asp:Button ID="btnSearchUser" runat="server" OnClick="btnSearchUser_Click" 
                                    Text="Search" />
                            </td>
                            <td nowrap="nowrap" valign="top">
                                        <asp:Label ID="lblCurrentProcessorNotFound" runat="server" 
                                            Text="User is not found" Visible="False"></asp:Label>
                                        <asp:GridView ID="gvCurrentProcessor" runat="server" 
                                            AutoGenerateColumns="False" DataKeyNames="UserID" GridLines="None" 
                                            OnSelectedIndexChanged="gvCurrentProcessor_SelectedIndexChanged" 
                                            ShowHeader="False">
                                            <Columns>
                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                                                            CommandArgument='<%# Bind("UserID") %>' CommandName="Select" 
                                                            Text='<%# Bind("DisplayName") %>'></asp:LinkButton>
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
                    <b>Status:</b>&nbsp;<asp:DropDownList ID="ddlStatusAdmin" runat="server" 
                        TabIndex="11">
                        <asp:ListItem>New</asp:ListItem>
                        <asp:ListItem>Active</asp:ListItem>
                        <asp:ListItem>On Hold</asp:ListItem>
                        <asp:ListItem>Resolved</asp:ListItem>
                        <asp:ListItem>Cancelled</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;<br />
                    <b>Assigned:</b>
                    <asp:DropDownList ID="ddlAssignedAdmin" runat="server" TabIndex="10">
                    </asp:DropDownList>
                </asp:Panel></td>
        </tr>
        <tr>
            <td valign="top">
                <table>
                    <tr>
                        <td align="right">
                            <span class="style3">(*)</span><span class="style4"> Required field</span>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b><span class="style1">*</span> Name: </b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server" MaxLength="350" TabIndex="1"></asp:TextBox>
                            <asp:Label ID="lblName" runat="server"></asp:Label>
                            <asp:TextBox ID="txtUserID" runat="server" Columns="1" Visible="False"></asp:TextBox>
                            <asp:Button ID="btnClearUser" runat="server" OnClick="btnClearUser_Click" 
                                Text="Clear User" Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b><span class="style1">*</span> Email:</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="350" TabIndex="2"></asp:TextBox>
                            <asp:Label ID="lblEmail" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Phone:</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhone" runat="server" MaxLength="50" TabIndex="3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b><span class="style1">*</span> Description:</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescription" runat="server" Columns="50" MaxLength="50" 
                                TabIndex="4"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Detail:</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDetails" runat="server" Columns="40" MaxLength="500" 
                                Rows="5" TabIndex="5" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Date Due:</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDueDate" runat="server" Columns="10" MaxLength="25" 
                                TabIndex="6"></asp:TextBox>
                            <asp:HyperLink ID="cmdStartCalendar" runat="server" 
                                ImageUrl="~/DesktopModules/ADefHelpDesk/images/calendar.png"></asp:HyperLink>
                            <b>Priority:</b>
                            <asp:DropDownList ID="ddlPriority" runat="server" TabIndex="7">
                                <asp:ListItem Selected="True">Normal</asp:ListItem>
                                <asp:ListItem>High</asp:ListItem>
                                <asp:ListItem>Low</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            &nbsp;<asp:Label ID="lblAttachFile" runat="server" Font-Bold="True" 
                                Text="Attach File:"></asp:Label>
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
                            <asp:Button ID="btnSubmit" runat="server" Font-Bold="True" ForeColor="#FF3300" 
                                OnClick="btnSubmit_Click" TabIndex="9" Text="Submit Ticket" />
                            <br />
                            <br />
                            <asp:Label ID="lblError" runat="server" EnableViewState="False" 
                                Font-Bold="True" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td colspan="2" valign="top">
                <table>
                    <tr>
                        <td colspan="2">
                            &nbsp;<asp:Image ID="Image1" runat="server" 
                                ImageUrl="~/DesktopModules/ADefHelpDesk/images/tag_blue.png" />
                            <b>&nbsp;Check all Tags that apply:</b>&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <uc1:Tags ID="TagsTree" runat="server" EnableViewState="True" Visible="False" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
       
        <tr>
            <td dir="ltr">
                &nbsp;</td>
            <td colspan="2" dir="ltr">
                &nbsp;</td>
        </tr>
        <tr>
            <td dir="ltr">
                &nbsp;</td>
            <td colspan="2" dir="ltr">
                &nbsp;</td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="pnlConfirmAnonymousUserEntry" runat="server" Visible="False">
    <div class="style5">
        <h2>
            Your Ticket has been submitted.<br />
            <br />
            <asp:Label ID="lblConfirmAnonymousUser" runat="server"></asp:Label>
            <br />
            <br />
            <asp:LinkButton ID="lnlAnonymousContinue" runat="server" Font-Underline="True" OnClick="lnlAnonymousContinue_Click">Click here to continue</asp:LinkButton>
        </h2>
    </div>
</asp:Panel>
<asp:Panel ID="pnlExistingTickets" runat="server">
    <table cellpadding="2">
        <tr>
            <td valign="top">
                <asp:ListView ID="lvTasks" runat="server" OnItemDataBound="lvTasks_ItemDataBound"
                    OnSorting="lvTasks_Sorting" OnItemCommand="lvTasks_ItemCommand" 
                    ondatabound="lvTasks_DataBound">
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
                            <td nowrap="nowrap"  >
                                <asp:Label ID="CreatedDateLabel" runat="server" Text='<%# Eval("CreatedDate") %>' />
                            </td>
                            <td>
                                <asp:Label ID="AssignedRoleIDLabel" runat="server" Text='<%# Eval("Assigned") %>' />
                            </td>
                            <td nowrap="nowrap">
                                <asp:Label ID="DescriptionLabel" runat="server" Text='<%# Eval("Description") %>' ToolTip='<%# Eval("Description") %>' />
                            </td>
                            <td nowrap="nowrap">
                                <asp:Label ID="RequesterUserIDLabel" runat="server" Text='<%# Eval("Requester") %>'
                                    Visible="false" />
                                <asp:Label ID="RequesterNameLabel" runat="server" Text='<%# Eval("RequesterName") %>' ToolTip='<%# Eval("RequesterName") %>' />
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
                                    <asp:Label ID="Label1" runat="server" Text="Ticket # or Search Text:" ForeColor="#333333" />
                                </th>
                            </tr>
                            <tr id="Tr1" runat="server">
                                <th id="Th1" runat="server" style="border-style: none" nowrap="nowrap">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CommandName="EmptyDataTemplateSearch" />
                                </th>
                                <th id="Th2" runat="server" nowrap="nowrap">
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="60">
                                        <asp:ListItem>*All*</asp:ListItem>
                                        <asp:ListItem>New</asp:ListItem>
                                        <asp:ListItem>Active</asp:ListItem>
                                        <asp:ListItem>On Hold</asp:ListItem>
                                        <asp:ListItem>Resolved</asp:ListItem>
                                        <asp:ListItem>Cancelled</asp:ListItem>
                                    </asp:DropDownList>
                                </th>
                                <th id="Th3" runat="server" nowrap="nowrap">
                                    <asp:DropDownList ID="ddlPriority" runat="server" Width="50">
                                        <asp:ListItem>*All*</asp:ListItem>
                                        <asp:ListItem>Normal</asp:ListItem>
                                        <asp:ListItem>Low</asp:ListItem>
                                        <asp:ListItem>High</asp:ListItem>
                                    </asp:DropDownList>
                                </th>
                                <th id="Th4" runat="server" nowrap="nowrap">
                                    <asp:TextBox ID="txtDue" runat="server" Width="70" />
                                </th>
                                <th id="Th5" runat="server" nowrap="nowrap">
                                    <asp:TextBox ID="txtCreated" runat="server" Width="70" />
                                </th>
                                <th id="Th6" runat="server" nowrap="nowrap">
                                    <asp:DropDownList ID="ddlAssigned" runat="server" Width="90" DataTextField="AssignedRoleID" DataValueField="Key" />
                                </th>
                                <th id="Th7" runat="server" nowrap="nowrap" colspan="2">
                                    <asp:TextBox ID="txtSearch" runat="server" Width="200" />
                                </th>
                            </tr>
                            <tr id="Tr3" runat="server">
                                <th id="Th10" runat="server" nowrap="nowrap">
                                    <asp:LinkButton ID="lnkTaskID" runat="server" CommandArgument="TaskID"
                                        Text="TaskID" Font-Underline="True" />
                                </th>
                                <th id="Th11" runat="server" nowrap="nowrap">
                                    <asp:LinkButton ID="lnkStatus" runat="server" CommandArgument="Status"
                                        Text="Status" Font-Underline="True" />
                                </th>
                                <th id="Th12" runat="server" nowrap="nowrap">
                                    <asp:LinkButton ID="lnkPriority" runat="server" CommandArgument="Priority"
                                        Text="Priority" Font-Underline="True" />
                                </th>
                                <th id="Th13" runat="server" nowrap="nowrap">
                                    <asp:LinkButton ID="lnkDueDate" runat="server" CommandArgument="DueDate"
                                        Text="Due" Font-Underline="True" />
                                </th>
                                <th id="Th14" runat="server" nowrap="nowrap">
                                    <asp:LinkButton ID="lnkCreatedDate" runat="server" CommandArgument="CreatedDate"
                                        Text="Created" Font-Underline="True" />
                                </th>
                                <th id="Th15" runat="server" nowrap="nowrap">
                                    <asp:LinkButton ID="lnkAssigned" runat="server" CommandArgument="Assigned"
                                        Text="Assigned" Font-Underline="True" />
                                </th>
                                <th id="Th16" runat="server" nowrap="nowrap">
                                    <asp:LinkButton ID="lnkDescription" runat="server" CommandArgument="Description"
                                        Text="Description" Font-Underline="True" />
                                </th>
                                <th id="Th17" runat="server" nowrap="nowrap">
                                    <asp:LinkButton ID="lnkRequester" runat="server" CommandArgument="Requester"
                                        Text="Requester" Font-Underline="True" />
                                </th>
                            </tr>
                            <tr runat="server">
                            <td colspan="8"> No records returned</td>
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
                                                <asp:Label ID="Label1" runat="server" Text="Ticket # or Search Text:" ForeColor="#333333" />
                                            </th>
                                        </tr>
                                        <tr id="Tr1" runat="server">
                                            <th id="Th1" runat="server" style="border-style: none" nowrap="nowrap">
                                                <asp:Button ID="btnSearch" runat="server" Text="Search" CommandName="Search" />
                                            </th>
                                            <th id="Th2" runat="server" nowrap="nowrap">
                                                <asp:DropDownList ID="ddlStatus" runat="server" Width="60">
                                                    <asp:ListItem>*All*</asp:ListItem>
                                                    <asp:ListItem>New</asp:ListItem>
                                                    <asp:ListItem>Active</asp:ListItem>
                                                    <asp:ListItem>On Hold</asp:ListItem>
                                                    <asp:ListItem>Resolved</asp:ListItem>
                                                    <asp:ListItem>Cancelled</asp:ListItem>
                                                </asp:DropDownList>
                                            </th>
                                            <th id="Th3" runat="server" nowrap="nowrap">
                                                <asp:DropDownList ID="ddlPriority" runat="server" Width="50">
                                                    <asp:ListItem>*All*</asp:ListItem>
                                                    <asp:ListItem>Normal</asp:ListItem>
                                                    <asp:ListItem>Low</asp:ListItem>
                                                    <asp:ListItem>High</asp:ListItem>
                                                </asp:DropDownList>
                                            </th>
                                            <th id="Th4" runat="server" nowrap="nowrap">
                                                <asp:TextBox ID="txtDue" runat="server" Width="70" />
                                            </th>
                                            <th id="Th5" runat="server" nowrap="nowrap">
                                                <asp:TextBox ID="txtCreated" runat="server" Width="70" />
                                            </th>
                                            <th id="Th6" runat="server" nowrap="nowrap">
                                                <asp:DropDownList ID="ddlAssigned" runat="server" Width="90" DataTextField="AssignedRoleID" DataValueField="Key" />
                                            </th>
                                            <th id="Th7" runat="server" nowrap="nowrap" colspan="2">
                                                <asp:TextBox ID="txtSearch" runat="server" Width="200" />
                                            </th>
                                        </tr>
                                        <tr runat="server">
                                            <th runat="server" nowrap="nowrap">
                                                <asp:LinkButton ID="lnkTaskID" runat="server" CommandName="Sort" CommandArgument="TaskID"
                                                    Text="TaskID" Font-Underline="True" />
                                                <asp:ImageButton ID="TaskIDImage" CommandName="Sort" CommandArgument="TaskID" runat="server"
                                                    ImageUrl="~/DesktopModules/ADefHelpDesk/images/dt-arrow-dn.png" Visible="false" />
                                            </th>
                                            <th runat="server" nowrap="nowrap">
                                                <asp:LinkButton ID="lnkStatus" runat="server" CommandName="Sort" CommandArgument="Status"
                                                    Text="Status" Font-Underline="True" />
                                                <asp:ImageButton ID="StatusImage" CommandName="Sort" CommandArgument="Status" runat="server"
                                                    ImageUrl="~/DesktopModules/ADefHelpDesk/images/dt-arrow-dn.png" Visible="false" />
                                            </th>
                                            <th runat="server" nowrap="nowrap">
                                                <asp:LinkButton ID="lnkPriority" runat="server" CommandName="Sort" CommandArgument="Priority"
                                                    Text="Priority" Font-Underline="True" /><asp:ImageButton ID="PriorityImage" CommandName="Sort"
                                                        CommandArgument="Priority" runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/dt-arrow-dn.png"
                                                        Visible="false" />
                                            </th>
                                            <th runat="server" nowrap="nowrap">
                                                <asp:LinkButton ID="lnkDueDate" runat="server" CommandName="Sort" CommandArgument="DueDate"
                                                    Text="Due" Font-Underline="True" /><asp:ImageButton ID="DueDateImage" CommandName="Sort"
                                                        CommandArgument="DueDate" runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/dt-arrow-dn.png"
                                                        Visible="false" />
                                            </th>
                                            <th runat="server" nowrap="nowrap">
                                                <asp:LinkButton ID="lnkCreatedDate" runat="server" CommandName="Sort" CommandArgument="CreatedDate"
                                                    Text="Created" Font-Underline="True" />
                                                <asp:ImageButton ID="CreatedDateImage" CommandName="Sort" CommandArgument="CreatedDate"
                                                    runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/dt-arrow-dn.png"
                                                    Visible="false" />
                                            </th>
                                            <th runat="server" nowrap="nowrap">
                                                <asp:LinkButton ID="lnkAssigned" runat="server" CommandName="Sort" CommandArgument="Assigned"
                                                    Text="Assigned" Font-Underline="True" />
                                                <asp:ImageButton ID="AssignedImage" CommandName="Sort" CommandArgument="Assigned"
                                                    runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/dt-arrow-dn.png"
                                                    Visible="false" />
                                            </th>
                                            <th runat="server" nowrap="nowrap">
                                                <asp:LinkButton ID="lnkDescription" runat="server" CommandName="Sort" CommandArgument="Description"
                                                    Text="Description" Font-Underline="True" />
                                                <asp:ImageButton ID="DescriptionImage" CommandName="Sort" CommandArgument="Description"
                                                    runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/dt-arrow-dn.png"
                                                    Visible="false" />
                                            </th>
                                            <th runat="server" nowrap="nowrap">
                                                <asp:LinkButton ID="lnkRequester" runat="server" CommandName="Sort" CommandArgument="Requester"
                                                    Text="Requester" Font-Underline="True" />
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
                    <asp:LinkButton ID="lnkPrevious" runat="server" onclick="lnkPrevious_Click">&lt;</asp:LinkButton>
                    &nbsp;<asp:Label ID="lblRecords" runat="server"></asp:Label>
                    &nbsp;<asp:LinkButton ID="lnkNext" runat="server" onclick="lnkNext_Click">&gt;</asp:LinkButton>
                    <asp:TextBox ID="txtCurrentPage" runat="server" Columns="1" Visible="False"></asp:TextBox>
                </asp:Panel>
                <br />
            </td>
            <td valign="top" nowrap="nowrap">
                <asp:Image ID="Image2" runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/tag_blue.png" />
                <b>&nbsp;Search Tags:<uc1:Tags ID="TagsTreeExistingTasks" runat="server" Visible="False" />
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


