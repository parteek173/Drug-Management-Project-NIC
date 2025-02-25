<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Notifications.aspx.cs" Inherits="FrontEnd_Notifications" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

        <div class="container mx-auto p-6">
        <h2 class="text-2xl font-semibold text-gray-800 mb-4">Add Notification</h2>
        <div class="bg-white p-6 rounded-lg shadow-md">
            <asp:Label ID="lblMessage" runat="server" CssClass="text-red-500"></asp:Label>
            <asp:TextBox ID="txtTitle" runat="server" CssClass="w-full p-2 border rounded mt-2" Placeholder="Title"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle" CssClass="text-red-500" ErrorMessage="Title is required." />
            
            <asp:TextBox ID="txtMessage" runat="server" CssClass="w-full p-2 border rounded mt-2" Placeholder="Message" Height="200" TextMode="MultiLine"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvMessage" runat="server" ControlToValidate="txtMessage" CssClass="text-red-500" ErrorMessage="Message is required." />
            
           
            <asp:TextBox ID="txtCreatedAt" runat="server" CssClass="w-full p-2 border rounded mt-2" Placeholder="Created At" TextMode="DateTimeLocal"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvCreatedAt" runat="server" ControlToValidate="txtCreatedAt" CssClass="text-red-500" ErrorMessage="Created At is required." />
            
            
            <asp:FileUpload ID="filePdf" runat="server" CssClass="w-full p-2 border rounded mt-2" />
           <%-- <asp:RequiredFieldValidator ID="rfvFile" runat="server" ControlToValidate="filePdf" CssClass="text-red-500" ErrorMessage="PDF file is required." />--%>
            
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="bg-blue-500 text-white px-4 py-2 rounded mt-4 hover:bg-blue-600" OnClick="btnSubmit_Click" />
        </div>


            <h2 class="text-2xl font-semibold text-gray-800 mt-8">Notifications List</h2>
            <div class="overflow-x-auto mt-4">
             <asp:GridView ID="gvNotifications" runat="server" CssClass="min-w-full bg-white border border-gray-200 shadow-md rounded-lg text-left"
                AutoGenerateColumns="False" GridLines="None" ShowHeaderWhenEmpty="True" EmptyDataText="No notifications found." OnRowCommand="gvNotifications_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Sr. No.">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                        <ItemStyle CssClass="text-center px-4 py-2 border-b" Width="5%" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Title" HeaderText="Title" ItemStyle-CssClass="px-4 py-2 border-b text-left" ItemStyle-Width="20%" />
                    <asp:BoundField DataField="Message" HeaderText="Message" ItemStyle-CssClass="px-4 py-2 border-b text-left" ItemStyle-Width="20%" />
                    <asp:BoundField DataField="CreatedAt" HeaderText="Created At" DataFormatString="{0:dd-MM-yyyy HH:mm}" ItemStyle-CssClass="px-4 py-2 border-b text-left" ItemStyle-Width="20%" />
                    <asp:TemplateField HeaderText="PDF" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkPdf" runat="server" NavigateUrl='<%# Eval("PdfFilePath") %>' Text="View PDF" CssClass="text-blue-600 hover:underline"></asp:HyperLink>
                        </ItemTemplate>
                        <ItemStyle CssClass="px-4 py-2 border-b text-center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actions" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:Button ValidationGroup="dle" ID="btnDelete" runat="server" CommandName="DeleteNotification" CommandArgument='<%# Eval("NotificationID") %>' Text="Delete" CssClass="bg-red-500 text-white px-3 py-1 rounded hover:bg-red-600" OnClientClick="return confirm('Are you sure you want to delete this notification?');" />
                        </ItemTemplate>
                        <ItemStyle CssClass="px-4 py-2 border-b text-center" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="bg-gray-200 text-gray-700 uppercase text-sm font-bold text-left" />
                <RowStyle CssClass="border-b border-gray-200 text-gray-900" />
                <AlternatingRowStyle CssClass="bg-gray-100" />
            </asp:GridView>
        </div>


    </div>

    


</asp:Content>

