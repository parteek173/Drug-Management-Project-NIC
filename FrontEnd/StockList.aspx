<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StockList.aspx.cs" Inherits="FrontEnd_StockList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <div class="container mx-auto p-4 min-h-screen flex flex-col">
        <h1 class="text-3xl font-bold text-center mb-6">Drug Stock List</h1>

        <!-- Table for displaying stock data -->
        <div class="overflow-x-auto flex-grow">
            <asp:GridView ID="ChemistGridView" runat="server" AutoGenerateColumns="false" ShowHeader="false"
                CssClass="display w-full table-auto text-sm" 
                GridLines="None" HeaderStyle-CssClass="bg-gray-200 font-semibold text-gray-700 text-center" 
                RowStyle-CssClass="text-center px-4 py-2 border-b">
                <Columns>

                    

                    <asp:BoundField DataField="DrugName" HeaderText="Drug Name" ItemStyle-CssClass="text-left px-4 py-2 border-b font-semibold text-gray-700" />
                    <asp:BoundField DataField="Category" HeaderText="Category" ItemStyle-CssClass="text-left px-4 py-2 border-b font-semibold text-gray-700" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" ItemStyle-CssClass="text-left px-4 py-2 border-b font-semibold text-gray-700" />
                    
                </Columns>
            </asp:GridView>
        </div>
    </div>

     <!-- DataTables Initialization Script -->
 <script>
     $(document).ready(function () {
         $('#<%= ChemistGridView.ClientID %>').DataTable({
             paging: true,
             searching: true,
             ordering: true,
             info: true,
             lengthMenu: [5, 10, 25, 50,100, 200],
             pageLength: 50,
             columns: [
                 { title: "DrugName" },
                 { title: "Category" },
                 { title: "Quantity" }
             ]
         });
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

