<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Account.aspx.cs" Inherits="ADefHelpDeskWebSite.Account" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Edit Account</title>
</head>
<body>
    <form id="form1" runat="server">
            <img alt="ADefHelpDesk" src="images/ADefHelpDesk_clear.gif"/><br />
            <asp:Panel ID="pnlCreateAccount" runat="server" 
                meta:resourcekey="pnlCreateAccountResource1">
                <table border="0">
                    <tr>
                        <td align="left" colspan="2">
                            <asp:LinkButton ID="lnkHome" runat="server" PostBackUrl="~/Default.aspx" 
                onclick="lnkHome_Click" meta:resourcekey="lnkHomeResource1" 
                Text="[Back to Main]" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Label ID="lblCreateNewAccount" runat="server" Text="Edit Account:" 
                meta:resourcekey="lblCreateNewAccountResource1" Font-Bold="True" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:label runat="server" ID="lblUserName" Font-Bold="True" Text="User Name:" 
                meta:resourcekey="lblUserNameResource1" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtUserName" 
                meta:resourcekey="txtUserNameResource1" MaxLength="100" BackColor="#CCCCCC" 
                                ReadOnly="True" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblFirstName" runat="server" Font-Bold="True" 
                                meta:resourcekey="lblFirstNameResource1" Text="First Name:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtFirstName" runat="server" 
                                meta:resourcekey="txtUserNameResource1" MaxLength="50" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblLastName" runat="server" Font-Bold="True" 
                                meta:resourcekey="lblLastNameResource1" Text="Last Name:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtLastName" runat="server" 
                                meta:resourcekey="txtUserNameResource1" MaxLength="50" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblPassword" runat="server" Font-Bold="True" 
                                meta:resourcekey="lblPasswordResource1" Text="Password:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtPassword" runat="server" 
                                meta:resourcekey="txtPasswordResource1" TextMode="Password" 
                                MaxLength="50" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:label runat="server" ID="lblConfirmPassword" Font-Bold="True" 
                meta:resourcekey="lblConfirmPasswordResource1" Text="Confirm Password:" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtConfirmPassword" 
                meta:resourcekey="TextBoxResource1" TextMode="Password" MaxLength="50" >********</asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblEmail" runat="server" Font-Bold="True" 
                                meta:resourcekey="lblEmailResource1" Text="E-mail:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" 
                                meta:resourcekey="txtEmailResource1" MaxLength="256" Columns="40" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="char" colspan="2" style="color: #FF0000">
                            <asp:Literal ID="ltError" runat="server" meta:resourcekey="ltErrorResource1"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="right">
                            &nbsp;</td>
                        <td align="right">
                            <asp:Button ID="btnSubmit" runat="server" meta:resourcekey="btnSubmitResource1" 
                                onclick="btnSubmit_Click" Text="Submit" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2" style="color:Red;">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
    </form>
</body>
</html>
