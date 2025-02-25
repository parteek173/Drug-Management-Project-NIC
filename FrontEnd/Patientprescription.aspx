<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Patientprescription.aspx.cs" Inherits="FrontEnd_Patientprescription" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <div class="container mx-auto p-6 min-h-screen flex flex-col">
    <!-- Page Title Section -->
    <h1 class="text-center text-2xl md:text-3xl font-bold text-gray-900 dark:text-white mb-6">
        <span class="text-transparent bg-clip-text bg-gradient-to-r to-emerald-600 from-sky-400">
            Patient Prescription 
        </span> 
        & Drug Records
    </h1>

    <!-- Page Description Section -->
    <p class="text-center text-sm lg:text-base text-gray-500 dark:text-gray-400 mb-6">
        This page provides a detailed record of patient prescriptions, including patient information, contact details, prescribed drugs, and hospital details. 
        <br />
        <b>
        Select the Chemist AND/OR Drug name from the list to view the patient details.
        </b>
    </p>


         <!-- Date & Filter Condition Selection -->
        <!-- Filter Section -->
        <div class="flex flex-wrap gap-4 mb-4">
            <!-- From Date -->
            <div class="flex-1">
                <label for="txtFromDate" class="block text-gray-700 font-semibold mb-2">📅 From Date:</label>
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="w-full border border-gray-300 rounded px-4 py-2 focus:ring-2 focus:ring-blue-500" TextMode="Date"></asp:TextBox>
            </div>

            <!-- To Date -->
            <div class="flex-1">
                <label for="txtToDate" class="block text-gray-700 font-semibold mb-2">📅 To Date:</label>
                <asp:TextBox ID="txtToDate" runat="server" CssClass="w-full border border-gray-300 rounded px-4 py-2 focus:ring-2 focus:ring-blue-500" TextMode="Date"></asp:TextBox>
            </div>

            <!-- Chemist Selection -->
            <div class="flex-1">
                <label for="ddlChemists" class="block text-gray-700 font-semibold mb-2">🔽 Select Chemist:</label>
                <asp:DropDownList ID="ddlChemists" runat="server" CssClass="w-full border border-gray-300 rounded px-4 py-2 focus:ring-2 focus:ring-blue-500">
                    <asp:ListItem Text="Select Chemist" Value=""></asp:ListItem>
           
                </asp:DropDownList>
            </div>

    <!-- Drug Selection -->
    <div class="flex-1">
        <label for="ddlDrugs" class="block text-gray-700 font-semibold mb-2">💊 Select Drug:</label>
        <asp:DropDownList ID="ddlDrugs" runat="server" CssClass="w-full border border-gray-300 rounded px-4 py-2 focus:ring-2 focus:ring-blue-500">
            <asp:ListItem Text="Select Drug" Value=""></asp:ListItem>
           
        </asp:DropDownList>
    </div>

    <!-- Filter Button -->
    <div class="flex items-end">
        <asp:Button ID="btnFilter" runat="server" Text="🔍 Filter" CssClass="bg-blue-600 text-white px-6 py-2 rounded-lg hover:bg-blue-700 transition"
            OnClick="btnFilter_Click" />
    </div>

            <!-- Filter Button -->
    <div class="flex items-end">
        <asp:Button ID="btnReset" runat="server" Text="🔄 Reset" OnClick="btnReset_Click" CssClass="bg-gray-500 text-white px-4 py-2 rounded shadow hover:bg-gray-600 transition-all" />

    </div>
</div>

    <!-- No Stock Alert Message -->
    <div id="MsgAlert" runat="server" visible="false" class="border-l-4 border-orange-500 text-orange-700 p-4 mb-6">
        <p class="font-semibold">⚠ No data Found</p>
        <p><asp:Label ID="lblMessage" runat="server"></asp:Label></p>
    </div>

        <!-- Patient Data Table -->
    <div class="overflow-x-auto">
        <asp:GridView ID="PatientGridView" runat="server" AutoGenerateColumns="false" ShowHeader="false"
            CssClass="w-full table-auto text-sm border border-gray-300 shadow-md rounded-lg"
            GridLines="None" HeaderStyle-CssClass="bg-gray-100 font-semibold text-gray-700 text-center"
            RowStyle-CssClass="text-center px-4 py-2 border-b">
            <Columns>
                <asp:TemplateField HeaderText="Sr. No.">
                    <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                    <ItemStyle CssClass="text-left px-4 py-2 border-b" Width="10"/>
                </asp:TemplateField>
                <asp:BoundField DataField="PatientName" HeaderText="Patient Name" ItemStyle-CssClass="text-left px-4 py-2 border-b font-semibold text-gray-700"/>
                <asp:BoundField DataField="MobileNumber" HeaderText="Mobile Number" ItemStyle-CssClass="text-left px-4 py-2 border-b font-semibold text-gray-700"/>
                <asp:BoundField DataField="PatientAddress" HeaderText="Patient Address" ItemStyle-CssClass="text-left px-4 py-2 border-b font-semibold text-gray-700"/>
                <asp:BoundField DataField="ChemistName" HeaderText="Chemist Name" SortExpression="ChemistName" ItemStyle-CssClass="text-left px-4 py-2 border-b font-semibold text-gray-700"/>
                <asp:BoundField DataField="DrugName" HeaderText="Drug Name" ItemStyle-CssClass="text-left px-4 py-2 border-b font-semibold text-gray-700"/>
                <asp:BoundField DataField="QuantitySold" HeaderText="Quantity Sold" ItemStyle-CssClass="text-left px-4 py-2 border-b font-semibold text-gray-700"/>
                <asp:BoundField DataField="HospitalName" HeaderText="Hospital Name" ItemStyle-CssClass="text-left px-4 py-2 border-b font-semibold text-gray-700"/>
            </Columns>
        </asp:GridView>
    </div>
</div>


     <!-- DataTables Initialization Script -->
 <script>
     $(document).ready(function () {
         $('#<%= PatientGridView.ClientID %>').DataTable({
             paging: true,
             searching: true,
             ordering: true,
             info: true,
             lengthMenu: [5, 10, 25, 50, 100, 200],
             pageLength: 50,
             columns: [
                 { title: "Sr.No" },
                 { title: "Patient Name" },
                 { title: "Mobile Number" },
                 { title: "Patient Address" },
                 { title: "Chemist Name" },
                 { title: "Drug Name" },
                 { title: "Quantity Sold" },
                 { title: "Hospital Name" },
             ]
         });
     });
 </script>

<script>
    $(document).ready(function () {
        // Apply the CSS class to the chemist select element
        $('#<%= ddlChemists.ClientID %>').select2({
            width: '100%',  // Adjust width as per your layout
            placeholder: "Search Chemist...",
            allowClear: true
        }).addClass('w-full border border-gray-300 rounded px-4 py-2 focus:ring-2 focus:ring-blue-500');

        // Apply the CSS class to the drugs select element
        $('#<%= ddlDrugs.ClientID %>').select2({
            width: '100%',  // Adjust width as per your layout
            placeholder: "Search Chemist...",
            allowClear: true
        }).addClass('w-full border border-gray-300 rounded px-4 py-2 focus:ring-2 focus:ring-blue-500');
    });
</script>


     <style>
    /* Adjust the overall width of the DataTables length menu select box */
    .dataTables_length select {
        width: auto; /* Allow the width to adjust based on content */
        min-width: 7rem; /* Set a minimum width for the dropdown */
        padding-right: 2rem; /* Add space for the dropdown arrow */
        appearance: none; /* Remove native dropdown styling */
        border: 1px solid #cbd5e1; /* Tailwind gray-300 */
        border-radius: 0.375rem; /* Tailwind rounded-md */
        background-color: #ffffff; /* White background */
        color: #334155; /* Tailwind slate-700 for text */
        font-size: 0.875rem; /* Tailwind text-sm */
        line-height: 1.25rem; /* Tailwind leading-5 */
        background-image: url('data:image/svg+xml;charset=UTF-8,%3csvg xmlns%3d%22http://www.w3.org/2000/svg%22 width%3d%2224%22 height%3d%2224%22 viewBox%3d%220 0 24 24%22 fill%3d%22none%22 stroke%3d%22%234A5568%22 stroke-width%3d%222%22 stroke-linecap%3d%22round%22 stroke-linejoin%3d%22round%22%3e%3cpolyline points%3d%226 9 12 15 18 9%22/%3e%3c/svg%3e');
        background-repeat: no-repeat;
        background-position: right 0.75rem center; /* Position dropdown arrow */
        background-size: 1rem;
        cursor: pointer; /* Pointer cursor for dropdown */
    }

    .dataTables_length select:focus {
        outline: 2px solid #2563eb; /* Tailwind blue-600 */
        outline-offset: 2px;
    }
</style>


</asp:Content>

