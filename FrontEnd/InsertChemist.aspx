<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="InsertChemist.aspx.cs" Inherits="FrontEnd_InsertChemist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">


   <div class="container mx-auto p-6 bg-white rounded-lg shadow-md max-w-2xl">

      <h1 class="mb-2 text-2xl font-bold text-gray-900 dark:text-white md:text-3xl lg:text-2xl">
        <span class="text-transparent bg-clip-text bg-gradient-to-r to-emerald-600 from-sky-400">Insert Chemist </span> Details </h1>
        <p class="text-sm font-normal text-gray-500 lg:text-base dark:text-gray-400">
            Please enter the details related to the chemist as listed below.
        </p>
   

    <div class="grid md:grid-cols-2 sm:grid-cols-1 gap-4 mt-6">
        <!-- Firm Name -->
        <div>
            <label for="txtFirmName" class="text-sm font-semibold text-gray-700">Firm Name:</label>
            <asp:TextBox ID="txtFirmName" runat="server" CssClass="mt-2 w-full p-2 border border-gray-300 rounded" placeholder="Enter firm name" />
            <asp:RequiredFieldValidator ID="rfvFirmName" runat="server" ControlToValidate="txtFirmName" ErrorMessage="Firm Name is required." Display="Dynamic" CssClass="text-red-500 text-sm" />
        </div>

        <!-- Address -->
        <div>
            <label for="txtAddress" class="text-sm font-semibold text-gray-700">Address Line 1:</label>
            <asp:TextBox ID="txtAddress" runat="server" CssClass="mt-2 w-full p-2 border border-gray-300 rounded" placeholder="Enter address" />
            <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddress" ErrorMessage="Address is required." Display="Dynamic" CssClass="text-red-500 text-sm" />
        </div>

        <!-- Phone Number -->
        <div>
            <label for="txtPhoneNumber" class="text-sm font-semibold text-gray-700">Phone Number:</label>
            <asp:TextBox ID="txtPhoneNumber" MaxLength="10" runat="server" CssClass="mt-2 w-full p-2 border border-gray-300 rounded" placeholder="Enter phone number" />
            <asp:RequiredFieldValidator ID="rfvPhoneNumber" runat="server" ControlToValidate="txtPhoneNumber" ErrorMessage="Phone Number is required." Display="Dynamic" CssClass="text-red-500 text-sm" />
            <asp:RegularExpressionValidator ID="revPhoneNumber" runat="server" ControlToValidate="txtPhoneNumber" ValidationExpression="^\d{10}$" ErrorMessage="Phone Number must be 10 digits." Display="Dynamic" CssClass="text-red-500 text-sm" />
        </div>

        <!-- Location Dropdown (Sectors & Villages) -->
        <div>
            <label for="ddlLocation" class="text-sm font-semibold text-gray-700">Select Location:</label>
            <asp:DropDownList ID="ddlLocation" runat="server" CssClass="mt-2 w-full p-2 border border-gray-300 rounded"></asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ControlToValidate="ddlLocation" InitialValue="Select Location" ErrorMessage="Please select a location." Display="Dynamic" CssClass="text-red-500 text-sm" />
        </div>

        <!-- Is Active -->
        <div class="flex items-center gap-2">
            <asp:CheckBox ID="chkIsActive" runat="server" CssClass="mt-1" />
            <label for="chkIsActive" class="text-sm font-semibold text-gray-700">Is Active</label>
        </div>

        <!-- Created At -->
        <div class="col-span-2">
            <label for="txtCreatedAt" class="text-sm font-semibold text-gray-700">Created At:</label>
            <asp:TextBox ID="txtCreatedAt" runat="server" CssClass="mt-2 w-full p-2 border border-gray-300 rounded bg-gray-100" ReadOnly="True" Text='<%# DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") %>' />
        </div>

        <!-- Submit Button -->
        <div class="col-span-2 text-center">
            <asp:Button ID="btnSubmit" runat="server" Text="Insert Details" OnClick="btnSubmit_Click" 
                CssClass="mt-4 py-2 px-6 bg-blue-600 text-white rounded-lg hover:bg-green-700 w-full" />
        </div>
    </div>
</div>




</asp:Content>

