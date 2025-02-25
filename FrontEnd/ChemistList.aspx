<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ChemistList.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="FrontEnd_ChemistList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

     <div class="container mx-auto p-4 min-h-screen flex flex-col">

          <h1 class="mb-2 text-2xl font-bold text-gray-900 dark:text-white md:text-3xl lg:text-2xl text-center">
              <span class="text-transparent bg-clip-text bg-gradient-to-r to-emerald-600 from-sky-400">Chemist & Pharmacy</span> Listings
        </h1>
        <p class="text-sm font-normal text-gray-500 lg:text-base dark:text-gray-400">
            Below is a list of Chemists and Pharmacies available on this portal. To search for a specific Chemist, simply enter a keyword in the search bar.
            
            If you'd like to check the stock for a particular Chemist or Pharmacy, click on the firm name, and it will display the available drug inventory for the selected Chemist.
        </p>

         <!-- Chemist Selection and Drug Stock Section -->
            <div class="flex flex-col md:flex-row justify-center gap-6 mt-8 mb-8">

                <!-- Chemist Selection Dropdown -->
                <div class="flex-1 bg-white p-6 rounded-lg shadow-lg border border-gray-200 flex flex-col justify-between">
                    <label for="ddlChemists" class="block text-gray-700 font-semibold mb-3 text-lg">
                        🔽 Select Location:
                    </label>
                    <p>
                        Select a location to filter the chemist list based on their respective locations.
                    </p>
                    <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="true"
                        CssClass="w-full border border-gray-300 rounded-lg px-4 py-3 bg-white shadow-sm focus:ring-2 focus:ring-blue-500 focus:outline-none"
                        OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
             </div>


        <!-- Table for displaying stock data -->
        <div class="overflow-x-auto flex-grow mt-10" >
           
            <div id="DeleteAlert" runat="server" visible="false" class="flex items-center p-4 mb-4 text-sm text-red-800 border border-red-300 rounded-lg bg-red-50 dark:bg-gray-800 dark:text-red-400 dark:border-red-800" role="alert">
                  <svg class="shrink-0 inline w-4 h-4 me-3" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="currentColor" viewBox="0 0 20 20">
                    <path d="M10 .5a9.5 9.5 0 1 0 9.5 9.5A9.51 9.51 0 0 0 10 .5ZM9.5 4a1.5 1.5 0 1 1 0 3 1.5 1.5 0 0 1 0-3ZM12 15H8a1 1 0 0 1 0-2h1v-3H8a1 1 0 0 1 0-2h2a1 1 0 0 1 1 1v4h1a1 1 0 0 1 0 2Z"/>
                  </svg>
                  <span class="sr-only">Info</span>
                  <div>
                    <span class="font-medium">Alert!</span>  <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Visible="false"></asp:Label>
                  </div>
            </div>

            <asp:GridView ID="ChemistGridView" runat="server" AutoGenerateColumns="false" ShowHeader="false"
                CssClass="display w-full table-auto text-sm" 
                GridLines="None" HeaderStyle-CssClass="bg-gray-200 font-semibold text-gray-700 text-center" 
                 OnRowCommand="ChemistGridView_RowCommand" DataKeyNames="chemist_id"
                OnRowDeleting="ChemistGridView_RowDeleting"
                RowStyle-CssClass="text-center px-4 py-2 border-b">
                        <Columns>

                            <asp:TemplateField HeaderText="Sr. No." >
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                                <ItemStyle CssClass="text-center px-4 py-2 border-b" Width="5%" />
                            </asp:TemplateField>

                        <asp:TemplateField HeaderText="Firm Name">
                            <ItemStyle CssClass="text-left px-4 py-2 border-b" Width="15%" />
                            <ItemTemplate>
                                <a href='StockList.aspx?ChemistID=<%# Eval("chemist_id") %>' class="font-semibold text-gray-700">
                                    <%# Eval("Name_Firm") %>
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                  
                            <asp:BoundField DataField="Address" HeaderText="Address" ItemStyle-CssClass="text-left px-4 py-2 border-b font-semibold text-gray-700" ItemStyle-Width="30%" />
                            
                            <asp:BoundField DataField="Sectors" HeaderText="Sectors" ItemStyle-CssClass="text-left px-4 py-2 border-b font-semibold text-gray-700" ItemStyle-Width="10%" />
                            
                            <asp:BoundField DataField="Mobile" HeaderText="Mobile" ItemStyle-CssClass="text-left px-4 py-2 border-b font-semibold text-gray-700" ItemStyle-Width="15%" />
                            <asp:BoundField DataField="CreatedAt" HeaderText="Created Date"  
                            DataFormatString="{0:dd-MMMM-yyyy}" 
                            ItemStyle-CssClass="text-left px-4 py-2 border-b font-semibold text-gray-700" ItemStyle-Width="15%" />
                  
                             <asp:TemplateField HeaderText="Status">
                                <ItemStyle CssClass="text-left px-4 py-2 border-b" Width="10%" />
                                <ItemTemplate>
                                    <asp:Button ID="btnToggleStatus" runat="server" CssClass="px-3 py-1 rounded text-white font-semibold"
                                        CommandArgument='<%# Eval("chemist_id") %>' CommandName="ToggleStatus"
                                        OnCommand="ToggleStatus_Click"
                                        Text='<%# Convert.ToBoolean(Eval("IsActive")) ? "Active" : "Inactive" %>'
                                        
                                        BackColor='<%# Convert.ToBoolean(Eval("IsActive")) ? System.Drawing.Color.Green : System.Drawing.Color.Red %>'

                                        />
                                </ItemTemplate>
                            </asp:TemplateField>

      

                           <asp:TemplateField HeaderText="Actions">
                                <ItemTemplate>
                                    <header CssClass="text-center"></header>
                                    <ItemStyle CssClass="text-left px-4 py-2 border-b" Width="10%" />

                                    <!-- Edit Button with Hover Effect -->
                                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandArgument='<%# Eval("chemist_id") %>' 
                                        CommandName="Edit" CssClass="px-4 py-2 rounded-md text-white bg-blue-600 font-semibold hover:bg-blue-700 focus:outline-none transition duration-300 ease-in-out transform hover:scale-105" />

                                    <!-- Delete Button with Hover Effect -->
                                    <asp:Button ID="btnDelete" runat="server" Visible="false" Text="Delete" CommandArgument='<%# Eval("chemist_id") %>' 
                                        CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?');" 
                                        CssClass="px-4 py-2 rounded-md text-white bg-red-600 font-semibold hover:bg-red-700 focus:outline-none transition duration-300 ease-in-out transform hover:scale-105 ml-2" />
                                </ItemTemplate>
                            </asp:TemplateField>


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
                 { title: "Sr.No" },
                 { title: "Name Firm" },
                 { title: "Address" },
                 { title: "Sectors" },
                 { title: "Mobile" },
                 { title: "Created Date" },
                 { title: "Is Active" },
                 { title: "Action" }
             ]
         });
     });
 </script>

        <script>
            $(document).ready(function () {
                $('#<%= ddlLocation.ClientID %>').select2({
                width: '100%',  // Adjust width as per your layout
                placeholder: "Search Location...",
                allowClear: true
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

