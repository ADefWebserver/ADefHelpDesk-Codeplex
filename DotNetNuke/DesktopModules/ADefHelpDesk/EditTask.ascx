<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditTask.ascx.cs" Inherits="ADefWebserver.Modules.ADefHelpDesk.EditTask" %>
<%@ Register Src="Controls/Tags.ascx" TagName="Tags" TagPrefix="uc1" %>
<%@ Register src="Controls/Comments.ascx" tagname="Comments" tagprefix="uc2" %>
<%@ Register src="Controls/Logs.ascx" tagname="Logs" tagprefix="uc3" %>
<body>
<asp:Panel ID="pnlEditTask" runat="server" HorizontalAlign="Left">
    <table cellpadding="2">
        <tr>
            <td>
                <asp:Image ID="imgNewTicket" runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/layout_add.png" />
                <asp:LinkButton ID="lnkNewTicket" runat="server" Font-Underline="True" OnClick="lnkNewTicket_Click">New Ticket</asp:LinkButton>
            </td>
            <td>
                <asp:Image ID="imgExitingTickets" runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/layout.png" />
                <asp:LinkButton ID="lnkExistingTickets" runat="server" Font-Underline="True" OnClick="lnkExistingTickets_Click">Existing Tickets</asp:LinkButton>
            </td>
            <td>
                <asp:Image ID="imgAdministrator" runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/cog.png" />
                <asp:LinkButton ID="lnkAdministratorSettings" runat="server" Font-Underline="True"
                    OnClick="lnkAdministratorSettings_Click">Administrator Settings</asp:LinkButton>
            </td>
            <td>
                &nbsp;&nbsp;</td>
        </tr>
    </table>
    <table>
        <tr>
            <td valign="top">
                <table>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnSave" runat="server" ForeColor="Red" OnClick="btnSave_Click" 
                                Text="Save" Width="63px" />
                        </td>
                        <td nowrap="nowrap">
                            <b>Ticket:</b>&nbsp;<asp:Label ID="lblTask" runat="server"></asp:Label>
                            &nbsp;<asp:Label ID="lblCreated" runat="server"></asp:Label>
                            &nbsp;<asp:Label ID="lblError" runat="server" Font-Bold="True" 
                                Font-Size="X-Small" ForeColor="Red" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Status: </b>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server">
                                <asp:ListItem>New</asp:ListItem>
                                <asp:ListItem>Active</asp:ListItem>
                                <asp:ListItem>On Hold</asp:ListItem>
                                <asp:ListItem>Resolved</asp:ListItem>
                                <asp:ListItem>Cancelled</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;<b>Assigned:</b>
                            <asp:DropDownList ID="ddlAssigned" runat="server">
                            </asp:DropDownList>
                            &nbsp; &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Priority: </b>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPriority" runat="server">
                                <asp:ListItem>Normal</asp:ListItem>
                                <asp:ListItem>High</asp:ListItem>
                                <asp:ListItem>Low</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;&nbsp;&nbsp; <b>Due Date:</b> <b>
                                <asp:TextBox ID="txtDueDate" runat="server" Columns="8"></asp:TextBox>
                            </b>
                            <asp:HyperLink ID="cmdtxtDueDateCalendar" runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/calendar.png"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Name:</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server" Columns="50" MaxLength="350"></asp:TextBox>
                            <asp:Label ID="lblName" runat="server" Visible="False"></asp:Label>
                            &nbsp; &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Email:</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" Columns="50" MaxLength="350"></asp:TextBox>
                            <asp:Label ID="lblEmail" runat="server" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Phone:</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhone" runat="server" Columns="20" MaxLength="50"></asp:TextBox>
                            <b>&nbsp;Estimate Hours:
                                <asp:TextBox ID="txtEstimate" runat="server" Columns="2"></asp:TextBox>
                            </b>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Description:</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescription" runat="server" Columns="50" MaxLength="50"></asp:TextBox>
                            &nbsp; &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <b>Start:
                                <asp:TextBox ID="txtStart" runat="server" Columns="8"></asp:TextBox>
                                <asp:HyperLink ID="cmdtxtStartCalendar" runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/calendar.png"></asp:HyperLink>
                                Complete:
                                <asp:TextBox ID="txtComplete" runat="server" Columns="8"></asp:TextBox>
                            </b>
                            <asp:HyperLink ID="cmdtxtCompleteCalendar" runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/calendar.png"></asp:HyperLink>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top" rowspan="4">
                <b>
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/tag_blue.png" />
                    Tags:&nbsp;&nbsp;<uc1:Tags ID="TagsTreeExistingTasks" runat="server" Visible="True" />
                </b>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Panel ID="pnlDetails" runat="server">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnComments" runat="server" OnClick="btnComments_Click" Text="Comments"
                                    BackColor="#CCCCCC" BorderColor="#CCCCCC" BorderStyle="Inset" Font-Bold="True"
                                    ForeColor="Red" Width="100px" />
                            </td>
                            <td>
                                <asp:Button ID="btnWorkItems" runat="server" OnClick="btnWorkItems_Click" Text="Work Items"
                                    BackColor="#F0F0F0" BorderColor="#CCCCCC" BorderStyle="Outset" 
                                    Width="100px" Visible="False" />
                            </td>
                            <td>
                                <asp:Button ID="btnAssociations" runat="server" BackColor="#F0F0F0" BorderColor="#CCCCCC"
                                    BorderStyle="Outset" OnClick="btnAssociations_Click" Text="Associations" 
                                    Width="100px" Visible="False" />
                            </td>
                            <td>
                                <asp:Button ID="btnLogs" runat="server" BackColor="#F0F0F0" BorderColor="#CCCCCC"
                                    BorderStyle="Outset" OnClick="btnLogs_Click" Text="Logs" Width="100px" />
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="pnlComments" runat="server">
                        <uc2:Comments ID="CommentsControl" runat="server" />
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="pnlWorkItems" runat="server" BorderColor="#CCCCCC" BorderStyle="Solid"
                    BorderWidth="1px" Height="250px" ScrollBars="Vertical" Width="500px" Visible="False">
                    WorkItems</asp:Panel>
                <asp:Panel ID="pnlAssociations" runat="server" BorderColor="#CCCCCC" BorderStyle="Solid"
                    BorderWidth="1px" Height="250px" ScrollBars="Vertical" Width="500px" Visible="False">
                    Associated Tickets</asp:Panel>
                <asp:Panel ID="pnlLogs" runat="server" BorderColor="#CCCCCC" BorderStyle="Solid"
                    BorderWidth="1px" Height="300px" ScrollBars="Auto" Width="510px" 
                    Visible="False" Wrap="False">
                    <uc3:Logs ID="LogsControl" runat="server" />
                </asp:Panel>
    </asp:Panel>
</td> </tr>
<tr>
    <td valign="top">
        <asp:Label ID="lblDetailsError" runat="server" EnableViewState="False" ForeColor="Red" />
        
    </td>
</tr>
</table> </asp:Panel> 