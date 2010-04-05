<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditTask.ascx.cs" Inherits="ADefWebserver.Modules.ADefHelpDesk.EditTask" %>
<%@ Register Src="~/Controls/Tags.ascx" TagName="Tags" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Comments.ascx" TagName="Comments" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Logs.ascx" TagName="Logs" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/Work.ascx" TagName="Work" TagPrefix="uc4" %>
<asp:Literal ID="JQueryCalendarJavaScript1" runat="server" 
    meta:resourcekey="JQueryCalendarJavaScript1Resource1" />
<asp:Literal ID="JQueryCalendarJavaScript2" runat="server" 
    meta:resourcekey="JQueryCalendarJavaScript2Resource1" />
<asp:Literal ID="JQueryCalendarJavaScript3" runat="server" 
    meta:resourcekey="JQueryCalendarJavaScript3Resource1" />
    <link type="text/css" href="themes/smoothness/jquery-ui-1.8.custom.css" rel="stylesheet" />
	<link type="text/css" href="Module.css" rel="stylesheet" />
	<script type="text/javascript" src="jquery-1.4.2.js"></script>
	<script type="text/javascript" src="ui/jquery.ui.core.js"></script>
	<script type="text/javascript" src="ui/jquery.ui.widget.js"></script>
	<script type="text/javascript" src="ui/jquery.ui.datepicker.js"></script>
<asp:Panel ID="pnlEditTask" runat="server" HorizontalAlign="Left" 
    meta:resourcekey="pnlEditTaskResource1">
<table>
<tr>
            <td valign="top"><table cellpadding="2">
        <tr>
            <td>
                <asp:Image ID="imgNewTicket" runat="server" ImageUrl="~/images/layout_add.png" 
                    meta:resourceKey="imgNewTicketResource1" />
                <asp:LinkButton ID="lnkNewTicket" meta:resourcekey="lnkNewTicket" 
                    runat="server" Font-Underline="True" OnClick="lnkNewTicket_Click" 
                    Text="New Ticket"></asp:LinkButton>
            </td>
            <td>
                <asp:Image ID="imgExitingTickets" runat="server" ImageUrl="~/images/layout.png" 
                    meta:resourceKey="imgExitingTicketsResource1" />
                <asp:LinkButton ID="lnkExistingTickets" meta:resourcekey="lnkExistingTickets" 
                    runat="server" Font-Underline="True" OnClick="lnkExistingTickets_Click" 
                    Text="Existing Tickets"></asp:LinkButton>
            </td>
            <td>
                <asp:Image ID="imgAdministrator" runat="server" ImageUrl="~/images/cog.png" 
                    meta:resourceKey="imgAdministratorResource1" />
                <asp:LinkButton ID="lnkAdministratorSettings" 
                    meta:resourcekey="lnkAdministratorSettings" runat="server" Font-Underline="True"
                    OnClick="lnkAdministratorSettings_Click" Text="Administrator Settings"></asp:LinkButton>
            </td>
            <td>
                &nbsp;&nbsp;
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td valign="top" title="Ticket">
                <table>
                    <tr>
                        <td align="center" >
                            <asp:Button ID="btnSave" meta:resourcekey="btnSave" runat="server" ForeColor="Red" OnClick="btnSave_Click" Text="Save"
                                Width="63px" />
                        </td>
                        <td nowrap="nowrap" colspan="1">
                            <b>
                            <asp:Label ID="lblTicket" runat="server" Font-Bold="True" 
                                meta:resourcekey="lblTicket" Text="Ticket:" />
                            </b>&nbsp;<asp:Label ID="lblTask" runat="server" 
                                meta:resourceKey="lblTaskResource1"></asp:Label>
                            &nbsp;<asp:Label ID="lblCreated" runat="server" 
                                meta:resourceKey="lblCreatedResource1"></asp:Label>
                            &nbsp;<asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="X-Small"
                                ForeColor="Red" meta:resourceKey="lblErrorResource1" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>
                            <asp:Label ID="lblStatus" runat="server" Font-Bold="True" 
                                meta:resourcekey="lblStatus" Text="Status:" />
                            &nbsp;</b></td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server" 
                                meta:resourceKey="ddlStatusResource1">
                        <asp:ListItem meta:resourcekey="ddlStatusAdminNew" Value="New" Text="New" />
                        <asp:ListItem meta:resourcekey="ddlStatusAdminActive" Value="Active" Text="Active" />
                        <asp:ListItem meta:resourcekey="ddlStatusAdminOnHold" Value="On Hold" Text="On Hold" />
                        <asp:ListItem meta:resourcekey="ddlStatusAdminResolved" Value="Resolved" Text="Resolved" />
                        <asp:ListItem meta:resourcekey="ddlStatusAdminCancelled" Value="Cancelled" Text="Cancelled" />
                            </asp:DropDownList>
                            &nbsp;<asp:Label ID="lblAssigned" runat="server" Font-Bold="True" 
                                meta:resourcekey="lblAssigned" Text="Assigned:" />
                                &nbsp;<asp:DropDownList ID="ddlAssigned" runat="server" 
                                meta:resourceKey="ddlAssignedResource1">
                            </asp:DropDownList>
                            &nbsp; &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>
                            <asp:Label ID="lbltxtPriority" runat="server" meta:resourcekey="lbltxtPriority" 
                                Text="Priority:" />
                            &nbsp;</b></td>
                        <td>
                            <asp:DropDownList ID="ddlPriority" runat="server" 
                                meta:resourceKey="ddlPriorityResource1">
                                <asp:ListItem meta:resourcekey="ddlPriorityNormal" Value="Normal" Text="Normal" />
                                <asp:ListItem meta:resourcekey="ddlPriorityHigh" Value="High" Text="High" />
                                <asp:ListItem meta:resourcekey="ddlPriorityLow" Value="Low" Text="Low" />
                            </asp:DropDownList>
                            &nbsp;&nbsp;&nbsp;<b>
                            <asp:Label ID="lbltxtDueDate" runat="server" meta:resourcekey="lbltxtDueDate" 
                                Text="Date Due:" /></b>
                            &nbsp;<b><asp:TextBox ID="txtDueDate" runat="server" Columns="8" 
                                meta:resourceKey="txtDueDateResource1"></asp:TextBox>
                            </b>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b><asp:Label ID="lbltxtName" runat="server" meta:resourcekey="lbltxtName" 
                                Text="Name:" /></b>
&nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server" Columns="50" MaxLength="350" 
                                meta:resourceKey="txtNameResource1"></asp:TextBox>
                            <asp:Label ID="lblName" runat="server" Visible="False" 
                                meta:resourceKey="lblNameResource1"></asp:Label>
                            &nbsp; &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b><asp:Label ID="lbltxtEmail" runat="server" meta:resourcekey="lbltxtEmail" 
                                Text="Email:" /></b>
&nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" Columns="50" MaxLength="350" 
                                meta:resourceKey="txtEmailResource1"></asp:TextBox>
                            <asp:Label ID="lblEmail" runat="server" Visible="False" 
                                meta:resourceKey="lblEmailResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b><asp:Label ID="lbltxtPhone" runat="server" meta:resourcekey="lbltxtPhone" 
                                Text="Phone:" /></b>
&nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtPhone" runat="server" Columns="20" MaxLength="50" 
                                meta:resourceKey="txtPhoneResource1"></asp:TextBox>
                            <b>&nbsp;<asp:Label ID="lbltxtEstimateHours" runat="server" 
                                meta:resourcekey="lbltxtEstimateHours" Text="Estimate Hours:" />
&nbsp;<asp:TextBox ID="txtEstimate" runat="server" Columns="2" meta:resourceKey="txtEstimateResource1"></asp:TextBox>
                            </b>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b><asp:Label ID="lbltxtDescription" runat="server" 
                                meta:resourcekey="lbltxtDescription" Text="Description:" /></b>
&nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtDescription" runat="server" Columns="50" MaxLength="50" 
                                meta:resourceKey="txtDescriptionResource1"></asp:TextBox>
                            &nbsp; &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <b>
                            <asp:Label ID="lbltxtStart" runat="server" meta:resourcekey="lbltxtStart" 
                                Text="Start:" />
&nbsp;<asp:TextBox ID="txtStart" runat="server" Columns="8" meta:resourceKey="txtStartResource1"></asp:TextBox>
                                &nbsp;<asp:Label ID="lbltxtComplete" runat="server" 
                                meta:resourcekey="lbltxtComplete" Text="Complete:" />
                            &nbsp;<asp:TextBox ID="txtComplete" runat="server" Columns="8" 
                                meta:resourceKey="txtCompleteResource1"></asp:TextBox>
                            </b>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Panel ID="pnlDetails" runat="server" 
                    meta:resourceKey="pnlDetailsResource1">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnComments" runat="server" OnClick="btnComments_Click" Text="Comments"
                                    BackColor="#CCCCCC" BorderColor="#CCCCCC" BorderStyle="Inset" Font-Bold="True"
                                    ForeColor="Red" Width="100px" meta:resourcekey="btnComments" />
                                <asp:Button ID="btnWorkItems" runat="server" BackColor="#F0F0F0" BorderColor="#CCCCCC"
                                    BorderStyle="Outset" OnClick="btnWorkItems_Click" Text="Work" Width="100px" meta:resourcekey="btnWorkItems" />
                                <asp:Button ID="btnLogs" runat="server" BackColor="#F0F0F0" BorderColor="#CCCCCC"
                                    BorderStyle="Outset" OnClick="btnLogs_Click" Text="Logs" Width="100px" meta:resourcekey="btnLogs" />
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="pnlComments" runat="server" 
                        meta:resourceKey="pnlCommentsResource1">
                        <uc2:Comments ID="CommentsControl" runat="server" />
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="pnlWorkItems" runat="server" Visible="False" 
                    meta:resourceKey="pnlWorkItemsResource1">
                    <uc4:Work ID="WorkControl" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlLogs" runat="server" BorderColor="#CCCCCC" BorderStyle="Solid"
                    BorderWidth="1px" Height="300px" ScrollBars="Auto" Width="510px" Visible="False"
                    Wrap="False" meta:resourceKey="pnlLogsResource1">
                    <uc3:Logs ID="LogsControl" runat="server" />
                </asp:Panel>
    </table></td> <td valign="top"><p>
    <asp:Image ID="imgTags" runat="server" 
                    ImageUrl="~/images/tag_blue.png" meta:resourceKey="imgTagsResource1" />
    <b>
                <asp:Label ID="lbltxtTags" runat="server" meta:resourcekey="lbltxtTags" 
                    Text="Tags:" />
                </b></p>
<uc1:Tags ID="TagsTreeExistingTasks" runat="server" />
                <br />
            </td>
            </tr>
</table>
   
</asp:Panel>
<asp:Label ID="lblDetailsError" runat="server" EnableViewState="False" 
    ForeColor="Red" meta:resourcekey="lblDetailsErrorResource1" />