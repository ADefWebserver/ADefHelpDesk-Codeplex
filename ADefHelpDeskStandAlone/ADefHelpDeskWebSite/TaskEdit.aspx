<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskEdit.aspx.cs" Inherits="ADefHelpDeskWebSite.TaskEdit" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<%@ Register src="Controls/EditTask.ascx" tagname="EditTask" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Edit Task</title>
    <link type="text/css" href="themes/smoothness/jquery-ui-1.8.custom.css" rel="stylesheet" />
	<link type="text/css" href="Module.css" rel="stylesheet" />
	<script type="text/javascript" src="jquery-1.4.2.js"></script>
	<script type="text/javascript" src="ui/jquery.ui.core.js"></script>
	<script type="text/javascript" src="ui/jquery.ui.widget.js"></script>
	<script type="text/javascript" src="ui/jquery.ui.datepicker.js"></script>
</head>
<body>
    <form id="form1" runat="server">
            <table cellpadding="2" cellspacing="2">
                <tr>
                    <td>
            <img alt="ADefHelpDesk" src="images/ADefHelpDesk_clear.gif"/></td>
                    <td align="right" valign="top">
    <asp:LinkButton ID="lnkLogin" runat="server" PostBackUrl="~/Login.aspx" 
        Visible="False" Text="Login" meta:resourcekey="lnkLoginResource1" />
    <asp:LinkButton ID="lnkLogOut" runat="server" OnClick="lnkLogOut_Click" 
        Visible="False" Text="Log Out" meta:resourcekey="lnkLogOutResource1" />
                        &nbsp;<asp:LinkButton ID="lnkRegister" runat="server" 
        PostBackUrl="~/Register.aspx" Text="Register" 
        meta:resourcekey="lnkRegisterResource1" />
                    </td>
                </tr>
            </table>
    <div>
    
        <uc1:EditTask ID="EditTask1" runat="server" />
        <br />
    
    </div>
    </form>
</body>
</html>
