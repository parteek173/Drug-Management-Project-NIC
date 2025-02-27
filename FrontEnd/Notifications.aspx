<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Notifications.aspx.cs" Inherits="FrontEnd_Notifications" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">


        <div class="container mx-auto p-6 flex flex-col items-center">
    
    <!-- Title Section -->
    <h1 class="mb-4 text-2xl font-bold text-gray-900 dark:text-white md:text-3xl lg:text-2xl text-center">
        Add Notification
    </h1>

    <!-- Notification Form -->
    <div class="bg-white p-6 rounded-lg shadow-md w-full max-w-lg">
        
        <!-- Message Label -->
        <asp:Label ID="lblMessage" runat="server" CssClass="text-red-500 block mb-2"></asp:Label>

        <!-- Title Input -->
        <asp:TextBox ID="txtTitle" runat="server" CssClass="w-full p-3 border border-gray-300 rounded mt-2 focus:ring-2 focus:ring-blue-500 focus:outline-none" Placeholder="Title" required></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle" CssClass="text-red-500 text-sm mt-1 block" ErrorMessage="Title is required." />

        <!-- Message Input -->
        <asp:TextBox ID="txtMessage" runat="server" CssClass="w-full p-3 border border-gray-300 rounded mt-3 focus:ring-2 focus:ring-blue-500 focus:outline-none" Placeholder="Message" Height="150" TextMode="MultiLine" required></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvMessage" runat="server" ControlToValidate="txtMessage" CssClass="text-red-500 text-sm mt-1 block" ErrorMessage="Message is required." />

        <!-- Created At Input -->
        <asp:TextBox ID="txtCreatedAt" runat="server" CssClass="w-full p-3 border border-gray-300 rounded mt-3 focus:ring-2 focus:ring-blue-500 focus:outline-none" Placeholder="Created At" TextMode="DateTimeLocal" required></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvCreatedAt" runat="server" ControlToValidate="txtCreatedAt" CssClass="text-red-500 text-sm mt-1 block" ErrorMessage="Created At is required." />

        <!-- File Upload -->
        <asp:FileUpload ID="filePdf" runat="server" CssClass="w-full p-3 border border-gray-300 rounded mt-3 bg-gray-50" />

        <!-- Submit Button -->
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="w-full bg-blue-500 text-white p-3 rounded mt-4 hover:bg-blue-600 transition duration-300" OnClick="btnSubmit_Click" />

    </div>

</div>

</asp:Content>

