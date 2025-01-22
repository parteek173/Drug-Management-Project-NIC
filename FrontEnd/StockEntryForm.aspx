<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeFile="StockEntryForm.aspx.cs" Inherits="FrontEnd_StockEntryForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Center the content vertically and horizontally using flex -->
    <div class="flex items-center justify-center min-h-screen">
        <div class="bg-white shadow-lg rounded-lg p-6 w-full max-w-md">
            <h1 class="text-2xl font-bold text-center text-gray-700 mb-6">Stock Entry Form</h1>

            <!-- Validation Summary -->
            <%--<asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="text-red-500 mb-4" />--%>

            <!-- Drug Name Field -->
            <div class="mb-4">
                <label for="txtDrugName" class="block text-sm font-medium text-gray-600">Drug Name</label>
                <asp:TextBox ID="txtDrugName" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Enter drug name" autocomplete="off"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDrugName" runat="server" ControlToValidate="txtDrugName" ErrorMessage="Drug name is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
            </div>

            <!-- Quantity Field -->
            <div class="mb-4">
                <label for="txtQuantity" class="block text-sm font-medium text-gray-600">Quantity</label>
                <asp:TextBox ID="txtQuantity" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Enter quantity" autocomplete="off"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtQuantity" ErrorMessage="Quantity is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revQuantity" runat="server" ControlToValidate="txtQuantity" ErrorMessage="Invalid quantity (only numbers)" ValidationExpression="^\d+$" CssClass="text-red-500"></asp:RegularExpressionValidator>
            </div>

            <!-- Date Field -->
            <div class="mb-4">
                <label for="txtDate" class="block text-sm font-medium text-gray-600">Expiry Date</label>
                <asp:TextBox ID="txtDate" runat="server" TextMode="Date" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Enter date"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDate" ErrorMessage="Date is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rvDate" runat="server" ControlToValidate="txtDate" ErrorMessage="Invalid date" CssClass="text-red-500" MinimumValue="1900-01-01" MaximumValue="2099-12-31" Type="Date"></asp:RangeValidator>
            </div>

              <!-- Batch Number -->
             <div class="mb-4">
                 <label for="batchNumber" class="block text-sm font-medium text-gray-600">Batch Number</label>
                 <asp:TextBox ID="batchNumber" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Enter Bacth Number"></asp:TextBox>
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="batchNumber" ErrorMessage="Batch Number is required" CssClass="text-red-500"></asp:RequiredFieldValidator>                 
             </div>

             <!-- Supplier Name -->
            <div class="mb-4">
                <label for="supplierName" class="block text-sm font-medium text-gray-600">Supplier Name</label>
                <asp:TextBox ID="supplierName" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Enter Supplier Name"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="supplierName" ErrorMessage="Supplier Name is required" CssClass="text-red-500"></asp:RequiredFieldValidator>               
            </div>

            <!-- Category Dropdown -->
            <div class="mb-4">
                <label for="ddlCategory" class="block text-sm font-medium text-gray-600">Category</label>
                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm">
                    <asp:ListItem Text="Select Category" Value="" />
                    <asp:ListItem Text="Injection" Value="Injection" />
                    <asp:ListItem Text="Capsules/Tablet" Value="Capsules/Tablet" />
                    <asp:ListItem Text="Ointment" Value="Ointment" />
                    <asp:ListItem Text="Cyrup" Value="Cyrup" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ControlToValidate="ddlCategory" InitialValue="" ErrorMessage="Category is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
            </div>

            <!-- Buttons -->
            <div class="flex items-center justify-between">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="bg-blue-500 text-white px-4 py-2 rounded-lg shadow hover:bg-blue-600" OnClick="btnSubmit_Click" />
                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="bg-gray-300 text-gray-700 px-4 py-2 rounded-lg shadow hover:bg-gray-400" OnClientClick="resetForm(); return false;" />
            </div>
        </div>
    </div>

</asp:Content>

