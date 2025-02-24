<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Drugsentry.aspx.cs" Inherits="FrontEnd_Drugsentry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <div class="max-w-lg mx-auto p-6 bg-white shadow-lg rounded-lg mt-20 shadow-blue-900/50">
      <h1 class="mb-2 text-2xl font-bold text-gray-900 dark:text-white md:text-3xl lg:text-2xl">
       <span class="">Add New  </span> Drug</h1>

        <p class=" text-sm font-normal text-gray-900 lg:text-base dark:text-gray-400">
           Drugs liable to be misused (Schedule H & H1)
        </p>

    <h2 class="text-2xl font-semibold text-center text-gray-800 mb-6"></h2>

        <!-- Message Label -->
    <div class="mt-4 text-center">
        <asp:Label ID="lblMessage" runat="server" CssClass="text-sm font-semibold"></asp:Label>
    </div>
    <!-- Drug Name -->
    <div class="mb-4">
        <label for="txtDrugName" class="block text-gray-700 font-medium">Drug Name:</label>
        <asp:TextBox ID="txtDrugName" runat="server" CssClass="w-full p-2 border border-gray-300 rounded" placeholder="Enter drug name" />
        <asp:RequiredFieldValidator ID="rfvDrugName" runat="server" ControlToValidate="txtDrugName" ErrorMessage="Drug name is required!" Display="Dynamic" CssClass="text-red-500 text-sm" />
    </div>

    <!-- Created Date -->
    <div class="mb-4">
        <label for="txtCreatedDate" class="block text-gray-700 font-medium">Created Date:</label>
        <asp:TextBox ID="txtCreatedDate" runat="server" CssClass="w-full p-2 border border-gray-300 rounded bg-gray-100" ReadOnly="True" />
    </div>

    <!-- Active Status -->
    <div class="flex items-center mb-4">
        <asp:CheckBox ID="chkActive" runat="server" CssClass="mr-2" Checked="true" />
        <label for="chkActive" class="text-gray-700">Is Active?</label>
    </div>

    <!-- Submit Button -->
    <div class="text-center">
        <asp:Button ID="btnSubmit" runat="server" Text="Add Drug" OnClick="btnSubmit_Click" CssClass="w-full py-2 bg-blue-600 text-white rounded-lg hover:bg-green-700 transition" />
    </div>

    
</div>




</asp:Content>

