<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Notifications.aspx.cs" Inherits="FrontEnd_Notifications" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">


    <div class="container mx-auto p-4  flex flex-col">

          <h1 class="mb-2 text-2xl font-bold text-gray-900 dark:text-white md:text-3xl lg:text-2xl text-center">
              <span class="">Add Notification</span> 
        </h1>

        <div class="container mx-auto p-6">
       <%-- <h2 class="text-2xl font-semibold text-gray-800 mb-4">Add Notification</h2>--%>
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


          


    </div>

    </div>


</asp:Content>

