<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Logs.ascx.cs" Inherits="ADefWebserver.Modules.ADefHelpDesk.Logs" %>
    <asp:LinqDataSource ID="LDSLogs" runat="server" 
        ContextTypeName="ADefWebserver.Modules.ADefHelpDesk.dnnHelpDeskDALDataContext" 
        OrderBy="DateCreated desc" TableName="ADefHelpDesk_Logs" 
    onselecting="LDSLogs_Selecting">
    </asp:LinqDataSource>
    <asp:GridView ID="gvLogs" runat="server" AllowPaging="True" 
        AutoGenerateColumns="False" DataKeyNames="LogID" DataSourceID="LDSLogs" 
        Width="490px" BorderStyle="None" PageSize="6">
        <Columns>
            <asp:TemplateField HeaderText="Description" SortExpression="LogDescription">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("LogDescription") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:TextBox ID="TextBox2" runat="server" Rows="2" 
                        Text='<%# Bind("LogDescription") %>' TextMode="MultiLine" Width="350px"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="DateCreated" HeaderText="Date" 
                SortExpression="DateCreated" >
            <ItemStyle Wrap="False" />
            </asp:BoundField>
        </Columns>
    </asp:GridView>
