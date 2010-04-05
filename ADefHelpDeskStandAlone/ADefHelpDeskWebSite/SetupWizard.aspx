<%@ Page Language="C#" validateRequest="false" AutoEventWireup="true" CodeBehind="SetupWizard.aspx.cs" Inherits="ADefWebserver.Modules.ADefHelpDesk.SetupWizard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ADefHelpDesk Set-up Wizard</title>
    <style type="text/css">
        .style1
        {
            text-align: right;
            width: 138px;
        }
        .style2
        {
            color: red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>
            <asp:Image ID="imgADefHelpDesk" runat="server" AlternateText="ADefHelpDesk.com" 
                ImageUrl="~/images/ADefHelpDesk_clear.gif" />
        </h1>
    </div>
    <asp:Panel ID="pnlWizard" runat="server" BackColor="#CCCCFF" BorderColor="#0033CC"
        BorderStyle="Outset" BorderWidth="10px" Width="600px">
        <asp:MultiView ID="mvSetupWizard" runat="server" ActiveViewIndex="0">
            <asp:View ID="vwWelcome" runat="server">
                <h2>
                    Welcome to the Set-up Wizard<br />
                    <br />
                    <br />
                    <asp:Button ID="btnWelcomeNext" runat="server" OnClick="btnWelcomeNext_Click" 
                        Text="Start Wizard &gt;" />
                </h2>
            </asp:View>
            <asp:View ID="vwPermissions" runat="server">
                <h2>
                    Permissions Check<br />
                    <br />
                    <asp:Button ID="btnCheckPermissions" runat="server" OnClick="btnCheckPermissions_Click"
                        Text="Click here to check permissions (may need to try more than once)" />
                    <br />
                </h2>
                <asp:BulletedList ID="blPermissions" runat="server">
                </asp:BulletedList>
                <br />
                <asp:Label ID="lblPermissionCheck" runat="server" EnableViewState="False"></asp:Label>
                <br />
                <br />
                <asp:Button ID="btnPermissionsPrevious" runat="server" Text="&lt; Back to Welcome"
                    OnClick="btnPermissionsPrevious_Click" />
                &nbsp;
                <asp:Button ID="btnPermissionsNext" runat="server" Text="Continue To Next Step &gt;"
                    EnableViewState="False" Visible="False" OnClick="btnPermissionsNext_Click" />
            </asp:View>
            <asp:View ID="vwDatabase" runat="server">
                <h2>
                    Database Configuration</h2>
                Enter your Microsoft SQL database connection information and click the&nbsp;<b>Test
                    Connection</b> button.<br />
                <br />
                <table id="tblDatabase" runat="Server" cellpadding="2" cellspacing="2" border="1">
                    <tr>
                        <td height="30" valign="top" class="style1">
                            <asp:Label ID="lblServer" runat="Server">Server:</asp:Label>
                        </td>
                        <td style="width: 150px;" valign="top">
                            <asp:TextBox ID="txtServer" runat="Server" Width="150px">(local)</asp:TextBox>
                        </td>
                        <td valign="middle">
                            &nbsp;
                            <asp:Label ID="lblServerHelp" runat="Server">(name or IP 
                                address of the database server)</asp:Label>
                        </td>
                    </tr>
                    <tr id="trDatabase" runat="server">
                        <td height="30" valign="top" class="style1">
                            <asp:Label ID="lblDataBase" runat="Server">Database Name:</asp:Label>
                        </td>
                        <td style="width: 150px;" valign="top">
                            <asp:TextBox ID="txtDatabase" runat="Server" Width="150px">ADefHelpDeskStandAlone</asp:TextBox>
                        </td>
                        <td valign="middle">
                            &nbsp;
                            <asp:Label ID="lblDatabaseHelp" runat="Server">(name of 
                                database (must already exist))</asp:Label>
                        </td>
                    </tr>
                    <tr id="trIntegrated" runat="server">
                        <td height="30" valign="top" class="style1">
                            <asp:Label ID="lblIntegrated" runat="Server">Integrated Security ?:</asp:Label>
                        </td>
                        <td valign="top">
                            <asp:CheckBox ID="chkIntegrated" runat="Server" AutoPostBack="True" Checked="True"
                                OnCheckedChanged="chkIntegrated_CheckedChanged" />
                        </td>
                        <td valign="middle">
                            &nbsp;
                            <asp:Label ID="lblIntegratedHelp" runat="Server">(check this box if using integrated security)</asp:Label>
                        </td>
                    </tr>
                    <tr id="trUser" runat="Server" visible="false">
                        <td height="30" valign="top" class="style1">
                            <asp:Label ID="lblUserId" runat="Server">User ID:</asp:Label>
                        </td>
                        <td style="width: 150px;" valign="top">
                            <asp:TextBox ID="txtUserId" runat="Server" Width="150px"></asp:TextBox>
                        </td>
                        <td valign="middle">
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="trPassword" runat="Server" visible="false">
                        <td height="30" valign="top" class="style1">
                            <asp:Label ID="lblPassword" runat="Server">Password:</asp:Label>
                        </td>
                        <td style="width: 150px;" valign="top">
                            <asp:TextBox ID="txtPassword" runat="Server" TextMode="Password" Width="150px" />
                        </td>
                        <td valign="middle">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Button ID="btnDatabaseCheck" runat="server" Text="Test Connection" OnClick="btnDatabaseCheck_Click" />
                <br />
                <br />
                <asp:Label ID="lblConnectionResults" runat="server" EnableViewState="False"></asp:Label>
                <br />
                <br />
                <asp:Button ID="btnDatabasePrevious" runat="server" OnClick="btnDatabasePrevious_Click"
                    Text="&lt; Back to Permission Check" />
                &nbsp;<asp:Button ID="btnDatabaseNext" runat="server" EnableViewState="False" OnClick="btnDatabaseNext_Click"
                    Text="Continue To Next Step &gt;" Visible="False" />
            </asp:View>
            <asp:View ID="vwDatabaseSetup" runat="server">
                <h2>
                    Install Database Scripts</h2>
                <br />
                &nbsp;<asp:Label ID="lblDatabaseSetup" runat="server"></asp:Label>
                <br />
                <br />
                <br />
                <asp:Button ID="btnDatabaseSetupPrevious" runat="server" OnClick="btnCompletePrevious_Click"
                    Text="&lt; Back to Database Configuration" />
                &nbsp;
                <asp:Button ID="btnDatabaseSetupNext" runat="server" EnableViewState="False" Text="Continue &gt;"
                    OnClick="btnDatabaseSetupNext_Click" />
            </asp:View>
            <asp:View ID="vwAdminAccount" runat="server">
                <h2>
                    Administrator Account Configuration</h2>
                Enter a user name and password for the administrtor account<table id="tblDatabase0"
                    runat="Server" border="1" cellpadding="2" cellspacing="2">
                    <tr>
                        <td class="style1" height="30" valign="top">
                            <asp:Label ID="lblUserName" runat="Server">User Name:</asp:Label>
                        </td>
                        <td valign="top">
                            <asp:TextBox ID="txtUserName" runat="Server">admin</asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trDatabase1" runat="server">
                        <td class="style1" height="30" valign="top">
                            <asp:Label ID="lblUserPassword" runat="Server">Password:</asp:Label>
                        </td>
                        <td valign="top">
                            <asp:TextBox ID="txtUserPassword" runat="Server" TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trDatabase2" runat="server">
                        <td class="style1" height="30" valign="top">
                            Email:
                        </td>
                        <td valign="top">
                            <asp:TextBox ID="txtUserEmail" runat="Server" Columns="40"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="lblUserAccountResults" runat="server" EnableViewState="False"></asp:Label>
                <br />
                <asp:Button ID="btnDatabaseSetupPrevious0" runat="server" 
                    OnClick="btnCompletePrevious_Click" 
                    Text="&lt; Back to Database Configuration" />
                &nbsp;<asp:Button ID="btnCreateAdmin" runat="server" 
                    onclick="btnCreateAdmin_Click" Text="Continue &gt;" />
            </asp:View>
            <asp:View ID="vwComplete" runat="server">
                <h2>
                    Wizard Complete</h2>
                <br />
                Click the <b>Complete Set-up</b> button to complete the set-up.
                <br />
                <br />
                <span class="style2">Still to do</span> (in Administrator Settings):<br />
                * Set up email (email will not work until this is completed)<br />
                * Set the upload directory (files cannot be uploaded until this this set)<br />
                <br />
                <br />
                <asp:Button ID="btnCompleteNext" runat="server" EnableViewState="False"
                    Text="Complete Set-up" onclick="btnCompleteNext_Click" />
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
    <p>
        <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="~/Default.aspx">[Back]</asp:HyperLink>
    </p>
    </form>
</body>
</html>
