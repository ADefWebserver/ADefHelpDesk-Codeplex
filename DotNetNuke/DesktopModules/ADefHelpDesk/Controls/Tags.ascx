<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Tags.ascx.cs" Inherits="ADefWebserver.Modules.ADefHelpDesk.Tags" %>
<asp:TreeView ID="tvCategories" runat="server" ExpandDepth="0" OnTreeNodeDataBound="tvCategories_TreeNodeDataBound">
    <SelectedNodeStyle BackColor="#CCCCCC" Font-Bold="False" Font-Underline="False" />
    <DataBindings>
        <asp:TreeNodeBinding DataMember="ADefWebserver.Modules.ADefHelpDesk.Catagories" Depth="0"
            TextField="Value" ValueField="Value" />
    </DataBindings>
</asp:TreeView>
