<%@ Control Language="C#" AutoEventWireup="true" CodeFile="View.ascx.cs" Inherits="ADefWebserver.Modules.ADefHelpDeskUpgradeAdvisor.View" %>
<script type="text/javascript">
    function onSilverlightError(sender, args) {
        var appSource = "";
        if (sender != null && sender != 0) {
            appSource = sender.getHost().Source;
        }
        var errorType = args.ErrorType;
        var iErrorCode = args.ErrorCode;
        if (errorType == "ImageError" || errorType == "MediaError") {
            return;
        }
        var errMsg = "Unhandled Error in Silverlight Application " + appSource + "\n";
        errMsg += "Code: " + iErrorCode + "    \n";
        errMsg += "Category: " + errorType + "       \n";
        errMsg += "Message: " + args.ErrorMessage + "     \n";
        if (errorType == "ParserError") {
            errMsg += "File: " + args.xamlFile + "     \n";
            errMsg += "Line: " + args.lineNumber + "     \n";
            errMsg += "Position: " + args.charPosition + "     \n";
        }
        else if (errorType == "RuntimeError") {
            if (args.lineNumber != 0) {
                errMsg += "Line: " + args.lineNumber + "     \n";
                errMsg += "Position: " + args.charPosition + "     \n";

            }
            errMsg += "MethodName: " + args.methodName + "     \n";
        }
        throw new Error(errMsg);
    }
</script>

<asp:Panel ID="SilverlightDesktopControl" align="center" runat="server" HorizontalAlign="Left">
    <object data="data:application/x-silverlight-2," type="application/x-silverlight-2"
        style="height:500px; width:500px;">
        <param name="source" value="<%=SilverlightSourceParams %>" />
        <param name="onError" value="onSilverlightError" />
        <param name="minRuntimeVersion" value="3.0.40624.0" />
        <param name="autoUpgrade" value="true" />
        <param name="InitParams" value="<%=SilverlightInitParams %>" /> 
        <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=3.0.40624.0" style="text-decoration: none">
            <img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="Get Microsoft Silverlight"
                style="border-style: none" />
        </a>
    </object>
    <iframe id="_sl_historyFrame" style="visibility: hidden; height: 0px; width: 0px;
        border: 0px"></iframe>
</asp:Panel>
