<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeFile="DrugStockList.aspx.cs" Inherits="DrugStockList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <div class="container mx-auto p-4 min-h-screen flex flex-col">
        <h1 class="text-3xl font-bold text-center mb-6">Drug Stock List</h1>

        <!-- Table for displaying stock data -->
        <div class="overflow-x-auto flex-grow">
            <asp:GridView ID="stockGridView" runat="server" AutoGenerateColumns="false" ShowHeader="false"
                CssClass="display w-full table-auto text-sm" 
                GridLines="None" HeaderStyle-CssClass="bg-gray-200 font-semibold text-gray-700 text-center" 
                RowStyle-CssClass="text-center px-4 py-2 border-b">
                <Columns>
                    <asp:BoundField DataField="DrugName" HeaderText="Drug Name" ItemStyle-CssClass="text-left px-4 py-2 border-b font-semibold text-gray-700" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" ItemStyle-CssClass="text-left px-4 py-2 border-b font-semibold text-gray-700" />
                    <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-CssClass="text-left px-4 py-2 border-b font-semibold text-gray-700" />
                    <asp:BoundField DataField="Category" HeaderText="Category" ItemStyle-CssClass="text-left px-4 py-2 border-b font-semibold text-gray-700" />
                    <asp:BoundField DataField="BatchNumber" HeaderText="Batch Number" ItemStyle-CssClass="text-left px-4 py-2 border-b font-semibold text-gray-700" />
                    <asp:BoundField DataField="SupplierName" HeaderText="Supplier Name" ItemStyle-CssClass="text-left px-4 py-2 border-b font-semibold text-gray-700"/>
                </Columns>
            </asp:GridView>
        </div>
    </div>

     <!-- DataTables Initialization Script -->
 <script>
     $(document).ready(function () {
         $('#<%= stockGridView.ClientID %>').DataTable({
             paging: true,
             searching: true,
             ordering: true,
             info: true,
             lengthMenu: [5, 10, 25, 50],
             pageLength: 10,
             columns: [
                 { title: "Drug Name" },
                 { title: "Quantity" },
                 { title: "Expiry Date" },
                 { title: "Category" },
                 { title: "Batch Number" },
                 { title: "Supplier Name" }
             ]
         });
     });
 </script>

</asp:Content>

