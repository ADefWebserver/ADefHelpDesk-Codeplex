<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ADefWebserver.Modules.ADefHelpDesk.Login" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ADefHelpDesk - Login</title>
</head>
<body>
    <p>
            <img alt="ADefHelpDesk" src="images/ADefHelpDesk_clear.gif"/></p>
    <form id="form1" runat="server">
    <div>
    
        <asp:LinkButton ID="lnkHome" runat="server" PostBackUrl="~/Default.aspx" 
            meta:resourcekey="lnkHomeResource1">[Back 
        to Main]</asp:LinkButton>
        <br />
        <br />
        <asp:Login ID="LoginControl" runat="server" 
            onauthenticate="LoginControl_Authenticate" DisplayRememberMe="False" 
            BackColor="#F7F6F3" BorderColor="#E6E2D8" BorderPadding="4" BorderStyle="Solid" 
            BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" 
            ForeColor="#333333" meta:resourcekey="LoginControlResource1">
            <TextBoxStyle Font-Size="0.8em" />
            <LoginButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid" 
                BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284775" />
            <LayoutTemplate>
                <table border="0" cellpadding="4" cellspacing="0" 
                    style="border-collapse:collapse;">
                    <tr>
                        <td>
                            <table border="0" cellpadding="0">
                                <tr>
                                    <td align="center" colspan="2" 
                                        style="color:White;background-color:#5D7B9D;font-size:0.9em;font-weight:bold;">
                                        Log In</td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" 
                                            meta:resourcekey="UserNameLabelResource1" Text="User Name:" /> 
                                    </td>
                                    <td>
                                        <asp:TextBox ID="UserName" runat="server" Font-Size="0.8em" 
                                            meta:resourcekey="UserNameResource1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                            ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                            meta:resourcekey="UserNameRequiredResource1"  
                                            ValidationGroup="LoginControl">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" 
                                            meta:resourcekey="PasswordLabelResource1" Text="Password:" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Password" runat="server" Font-Size="0.8em" 
                                            meta:resourcekey="PasswordResource1" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                            ControlToValidate="Password" ErrorMessage="Password is required." 
                                            meta:resourcekey="PasswordRequiredResource1"  
                                            ValidationGroup="LoginControl">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2" style="color:Red;">
                                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False" 
                                            meta:resourcekey="FailureTextResource1" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        <asp:Button ID="LoginButton" runat="server" BackColor="#FFFBFF" 
                                            BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CommandName="Login" 
                                            Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284775" 
                                            meta:resourcekey="LoginButtonResource1" Text="Log In" 
                                            ValidationGroup="LoginControl" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
            <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
            <TitleTextStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="0.9em" 
                ForeColor="White" />
        </asp:Login>
    
        <br />
        <asp:Panel ID="pnlVerification" runat="server" 
            meta:resourcekey="pnlVerificationResource1" Visible="False">
            <asp:Label ID="lblVerificationRequired" runat="server" 
    Text="Verification is required - Enter te verification code that was emailed on account creation." meta:resourcekey="lblVerificationRequiredResource1" />
            <asp:Label ID="lblVerificationNotCorrect" runat="server" 
                meta:resourcekey="lblVerificationNotCorrectResource1" 
                Text="Verification is required - Code entered was not correct. Try again." 
                Visible="False" />
            <br />
            <br />
            <asp:TextBox ID="txtVerificationCode" runat="server" 
                meta:resourcekey="txtVerificationCodeResource1"></asp:TextBox>
            <br />
            <asp:Button ID="btnVerify" runat="server" meta:resourcekey="btnVerifyResource1" 
                Text="Submit" onclick="btnVerify_Click" />
        </asp:Panel>
        <br />
    
    </div>
    </form>
</body>
</html>

