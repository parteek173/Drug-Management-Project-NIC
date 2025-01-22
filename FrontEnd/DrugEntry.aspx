<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeFile="DrugEntry.aspx.cs" Inherits="FrontEnd_DrugEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Center the content vertically and horizontally using flex -->
    <div class="flex items-center justify-center min-h-screen">
        <div class="bg-white shadow-lg rounded-lg p-6 w-full max-w-md">
            <h1 class="text-2xl font-bold text-center text-gray-700 mb-6">Patient Entry Form</h1>

            <!-- Validation Summary -->
            <%--<asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="text-red-500 mb-4" />--%>

            <!-- Date Field -->
            <div class="mb-4">
                <label for="txtDate" class="block text-sm font-medium text-gray-600">Date</label>
                <asp:TextBox ID="txtDate" runat="server" TextMode="Date" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Enter date"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDate" ErrorMessage="Date is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rvDate" runat="server" ControlToValidate="txtDate" ErrorMessage="Invalid date" CssClass="text-red-500" MinimumValue="1900-01-01" MaximumValue="2099-12-31" Type="Date"></asp:RangeValidator>
            </div>

            <!-- Patient Name Field -->
            <div class="mb-4">
                <label for="txtPatientName" class="block text-sm font-medium text-gray-600">Patient Name</label>
                <asp:TextBox ID="txtPatientName" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Enter patient name" autocomplete="off"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPatientName" runat="server" ControlToValidate="txtPatientName" ErrorMessage="Patient name is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revPatientName" runat="server" ControlToValidate="txtPatientName" ErrorMessage="Invalid name (letters and spaces only)" ValidationExpression="^[a-zA-Z\s]+$" CssClass="text-red-500"></asp:RegularExpressionValidator>
            </div>

            <!-- Mobile Number Field -->
            <div class="mb-4">
                <label for="txtMobileNumber" class="block text-sm font-medium text-gray-600">Mobile Number</label>
                <asp:TextBox ID="txtMobileNumber" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Enter mobile number" autocomplete="off"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvMobileNumber" runat="server" ControlToValidate="txtMobileNumber" ErrorMessage="Mobile number is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revMobileNumber" runat="server" ControlToValidate="txtMobileNumber" ErrorMessage="Invalid mobile number" ValidationExpression="^\d{10}$" CssClass="text-red-500"></asp:RegularExpressionValidator>
            </div>

            <!-- Patient ID Field -->
            <div class="mb-4">
                <label for="txtPatientID" class="block text-sm font-medium text-gray-600">Patient ID</label>
                <asp:TextBox ID="txtPatientID" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Enter patient ID" autocomplete="off"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPatientID" runat="server" ControlToValidate="txtPatientID" ErrorMessage="Patient ID is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
            </div>

            <!-- Prescribed By Field -->
            <div class="mb-4">
                <label for="txtPrescribedBy" class="block text-sm font-medium text-gray-600">Prescribed By</label>
                <asp:TextBox ID="txtPrescribedBy" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Enter prescriber name" autocomplete="off"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPrescribedBy" runat="server" ControlToValidate="txtPrescribedBy" ErrorMessage="Prescriber name is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
            </div>

            <!-- Hospital Name Field -->
            <div class="mb-4">
                <label for="txtHospitalName" class="block text-sm font-medium text-gray-600">Hospital Name</label>
                <asp:TextBox ID="txtHospitalName" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Enter hospital name" autocomplete="off"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvHospitalName" runat="server" ControlToValidate="txtHospitalName" ErrorMessage="Hospital name is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
            </div>

            <!-- Doctor Name Field -->
            <div class="mb-4">
                <label for="txtDoctorName" class="block text-sm font-medium text-gray-600">Doctor Name</label>
                <asp:TextBox ID="txtDoctorName" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Enter doctor name" autocomplete="off"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDoctorName" runat="server" ControlToValidate="txtDoctorName" ErrorMessage="Doctor name is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
            </div>

            <!-- Buttons -->
            <div class="flex items-center justify-between">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="bg-blue-500 text-white px-4 py-2 rounded-lg shadow hover:bg-blue-600" OnClick="btnSubmit_Click"/>
               <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="bg-gray-300 text-gray-700 px-4 py-2 rounded-lg shadow hover:bg-gray-400" OnClientClick="resetForm(); return false;" />
            </div>
        </div>
    </div>

</asp:Content>


