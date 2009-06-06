<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Comments.ascx.cs" Inherits="ADefWebserver.Modules.ADefHelpDesk.Comments" %>
<style type="text/css">
    .style1
    {
        font-weight: bold;
    }
</style>
<asp:Panel ID="pnlInsertComment" runat="server" GroupingText="Insert New Comment" Width="540px"
    Font-Size="X-Small" BorderStyle="Outset">
    <table>
        <tr>
            <td valign="top">
                <asp:TextBox ID="txtComment" runat="server" Columns="50" Rows="2" TextMode="MultiLine"></asp:TextBox>
            </td>
            <td valign="top" align="center">
                <asp:CheckBox ID="chkCommentVisible" runat="server" Checked="True" Font-Size="X-Small"
                    Text="Visible to Requestor" AutoPostBack="True" 
                    oncheckedchanged="chkCommentVisible_CheckedChanged" />
            </td>
        </tr>
        <tr>
            <td colspan="2" nowrap="nowrap">
                &nbsp;<span style="font-size: x-small" class="style1">Attach File:</span>
                <asp:FileUpload ID="TicketFileUpload" runat="server" Width="190px" />
                &nbsp;<asp:Button ID="btnInsertComment" runat="server" Font-Bold="True" OnClick="btnInsertComment_Click"
                    Text="Insert" />
                &nbsp;<asp:Button ID="btnInsertCommentAndEmail" runat="server" Font-Bold="True" 
                    OnClick="btnInsertCommentAndEmail_Click" Text="Insert and Email " />
                <br />
                <asp:Label ID="lblError" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="pnlTableHeader" runat="server"><br />
<table cellpadding="0" cellspacing="0" bgcolor="WhiteSmoke" Width="520px">
    <tr>
        <td Width="49px">
            <asp:Image ID="Image6" runat="server" 
                ImageUrl="~/DesktopModules/ADefHelpDesk/images/GridSpacer.gif" 
                Visible="False" />
        </td>
        <td style="border: 1px solid #CCCCCC" align="center" Width="23px">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/World.png"
                AlternateText="Visible to Requestor" />
        </td>
        <td style="border: 1px solid #CCCCCC" align="center">
            <asp:Label ID="lblDescription" runat="server" Width="150px" Font-Bold="True">Comment</asp:Label>
        </td>
        <td style="border: 1px solid #CCCCCC" align="center">
            <asp:Label ID="lblUser" runat="server" Width="152px" Font-Bold="True">User</asp:Label>
        </td>
        <td style="border: 1px solid #CCCCCC" align="center">
            <asp:Label ID="lblTime" runat="server" Width="150px" Font-Bold="True">Time</asp:Label>
        </td>
    </tr>
</table></asp:Panel>

<asp:Panel ID="pnlExistingComments" runat="server" Height="250px" ScrollBars="Vertical"
    Width="520px">
    <asp:GridView ID="gvComments" runat="server" AutoGenerateColumns="False" DataKeyNames="DetailID"
        DataSourceID="LDSComments" ShowHeader="False" 
        OnRowDataBound="gvComments_RowDataBound" 
        onrowcommand="gvComments_RowCommand" Width="100%">
        <Columns>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" Font-Underline="True" 
                        CommandArgument='<%# Bind("DetailID") %>' CommandName="Select" Text="Select"></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle Width="48px" />
            </asp:TemplateField>
            <asp:TemplateField >
                <ItemTemplate>
                    <asp:Label ID="lblDetailType" runat="server" Text='<%# Bind("DetailType") %>' Visible="False" />
                    <asp:CheckBox ID="chkDetailType" runat="server" Checked="true" Enabled="False" ToolTip="Visible to Requestor" />
                </ItemTemplate>
                <ItemStyle Width="20px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Description" SortExpression="Description">
                <ItemTemplate>
                    <asp:Label ID="lblComment" Font-Size="X-Small" runat="server" Text='<%# Bind("Description") %>'
                        Width="148px"></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="213px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="UserID" SortExpression="UserID">
                <ItemTemplate>
                    <asp:Label ID="gvlblUser" Font-Size="X-Small" runat="server" Text='<%# Bind("UserID") %>'
                        Width="150px"></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="110px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="InsertDate" SortExpression="InsertDate">
                <ItemTemplate>
                    <asp:Label ID="lblDate" runat="server" Font-Size="XX-Small" Text='<%# Bind("InsertDate") %>'
                        Width="148px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Panel>
<asp:Panel ID="pnlEditComment" runat="server" Visible="False">
        <table>
        <tr>
            <td valign="top" nowrap="nowrap">
                <asp:Image ID="Image3" runat="server" 
                    ImageUrl="~/DesktopModules/ADefHelpDesk/images/application_side_contract.png" />
                <asp:LinkButton ID="lnkBack" runat="server" Font-Underline="True" 
                    onclick="lnkBack_Click">Back</asp:LinkButton>&nbsp;<asp:Image ID="Image4" 
                    runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/page_add.png" />
                <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update" 
                    Font-Underline="True" onclick="lnkUpdate_Click" />
                <asp:Image ID="ImgEmailUser" runat="server" ImageUrl="~/DesktopModules/ADefHelpDesk/images/user_suit.png" /><asp:LinkButton 
                    ID="lnkUpdateRequestor" runat="server" Font-Underline="True" 
                    onclick="lnkUpdateRequestor_Click" Text="Update and Email Requestor" />
&nbsp;<asp:Image ID="Image5" runat="server" 
                    ImageUrl="~/DesktopModules/ADefHelpDesk/images/page_delete.png" />
                <asp:LinkButton ID="lnkDelete" runat="server" 
                    OnClientClick='if (!confirm("Are you sure you want to delete?") ){return false;}' 
                    Text="Delete" Font-Underline="True" onclick="lnkDelete_Click" />
            </td>
            <td valign="top" align="right">
                <asp:CheckBox ID="chkCommentVisibleEdit" runat="server" Font-Size="X-Small" 
                    Text="Visible to Requestor" AutoPostBack="True" 
                    oncheckedchanged="chkCommentVisibleEdit_CheckedChanged" />
            </td>
        </tr>
            <tr>
                <td valign="top" colspan="2">
                    <asp:TextBox ID="txtDescription" runat="server" Columns="60" Rows="10" 
                        TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
        <tr>
            <td colspan="2">
                <b>User:</b>
                <asp:Label ID="lblDisplayUser" runat="server"></asp:Label>
                &nbsp;<b>Insert Date:</b>&nbsp;<asp:Label ID="lblInsertDate" runat="server"></asp:Label>
                &nbsp;
                  <asp:Panel ID="pnlDisplayFile" runat="server" Visible="False">
                    <span style="font-size: x-small; font-weight: bold;" >
                      <br />
                      Attachment: 
                      <asp:ImageButton ID="imgDelete" runat="server" 
                          ImageUrl="~/DesktopModules/ADefHelpDesk/images/cancel.png" 
                          ToolTip="Delete Attachment" 
                          OnClientClick='if (!confirm("Are you sure you want to delete?") ){return false;}' 
                          onclick="imgDelete_Click" />
                      &nbsp;</span><asp:LinkButton ID="lnkFileAttachment" runat="server" Font-Underline="True" 
                          onclick="lnkFileAttachment_Click" 
                          ToolTip="Click here to download this file"></asp:LinkButton>
                      &nbsp;</asp:Panel>
                <asp:Panel ID="pnlAttachFile" runat="server" Visible="False">
                    <span style="font-size: x-small; font-weight: bold;">
                    <br />
                    Attach File:</span>
                    <asp:FileUpload ID="fuAttachment" runat="server" />
                    &nbsp;&nbsp;&nbsp;&nbsp;</asp:Panel>
                <br />
                <asp:Label ID="lblErrorEditComment" runat="server" EnableViewState="False" 
                    ForeColor="Red"></asp:Label>
                <asp:Label ID="lblDetailID" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:LinqDataSource ID="LDSComments" runat="server" ContextTypeName="ADefWebserver.Modules.ADefHelpDesk.ADefHelpDeskDALDataContext"
    OrderBy="InsertDate desc" TableName="ADefHelpDesk_TaskDetails" OnSelecting="LDSComments_Selecting">
</asp:LinqDataSource>
