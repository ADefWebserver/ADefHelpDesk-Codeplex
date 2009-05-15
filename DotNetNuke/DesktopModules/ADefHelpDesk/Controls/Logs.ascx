<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Logs.ascx.cs" Inherits="ADefWebserver.Modules.ADefHelpDesk.Logs" %>
    <asp:LinqDataSource ID="LDSLogs" runat="server" 
        ContextTypeName="ADefWebserver.Modules.ADefHelpDesk.ADefHelpDeskDALDataContext" 
        OrderBy="DateCreated desc" TableName="ADefHelpDesk_Logs" 
    onselecting="LDSLogs_Selecting">
    </asp:LinqDataSource>
    <asp:GridView ID="gvLogs" runat="server" AllowPaging="True" 
        AutoGenerateColumns="False" DataKeyNames="LogID" DataSourceID="LDSLogs" 
        Width="490px">
        <Columns>
            <asp:BoundField DataField="LogDescription" HeaderText="Description" 
                SortExpression="LogDescription" >
            <ItemStyle Width="100%" />
            </asp:BoundField>
            <asp:BoundField DataField="DateCreated" HeaderText="Date" 
                SortExpression="DateCreated" >
            <ItemStyle Wrap="False" />
            </asp:BoundField>
        </Columns>
    </asp:GridView>
