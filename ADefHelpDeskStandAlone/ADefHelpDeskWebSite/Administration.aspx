<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Administration.aspx.cs" Inherits="ADefWebserver.Modules.ADefHelpDesk.Administration" %>
<%@ Register src="Controls/AdminSettings.ascx" tagname="AdminSettings" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Administration</title>
    <link href="Module.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server"> 
        <div>
        <h1>
            <img alt="ADefHelpDesk" src="images/ADefHelpDesk_clear.gif"/></h1>
    </div>
    <uc1:AdminSettings ID="AdministrationView" runat="server" /> 
    </form>
    </body>
</html>