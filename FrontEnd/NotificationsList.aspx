<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="NotificationsList.aspx.cs" Inherits="FrontEnd_NotificationsList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <div class="container mx-auto p-4  flex flex-col">
    <!-- Table for displaying stock data -->
    <div class="overflow-x-auto flex-grow mt-6">
        <div id="DeleteAlert" runat="server" visible="false" class="flex items-center p-4 mb-4 text-sm text-red-800 border border-red-300 rounded-lg bg-red-50 dark:bg-gray-800 dark:text-red-400 dark:border-red-800" role="alert">
                  <svg class="shrink-0 inline w-4 h-4 me-3" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="currentColor" viewBox="0 0 20 20">
                    <path d="M10 .5a9.5 9.5 0 1 0 9.5 9.5A9.51 9.51 0 0 0 10 .5ZM9.5 4a1.5 1.5 0 1 1 0 3 1.5 1.5 0 0 1 0-3ZM12 15H8a1 1 0 0 1 0-2h1v-3H8a1 1 0 0 1 0-2h2a1 1 0 0 1 1 1v4h1a1 1 0 0 1 0 2Z"/>
                  </svg>
                  <span class="sr-only">Info</span>
                  <div>
                    <span class="font-medium">Alert!</span>  <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Visible="false"></asp:Label>
                  </div>
        </div>


        </div>
    <div class="container mx-auto p-4  flex flex-col">

          <h1 class="mb-2 text-2xl font-bold text-gray-900 dark:text-white md:text-3xl lg:text-2xl text-center">
              <span class="">Notifications List</span> 
        </h1>

            <div class="overflow-x-auto mt-4">
                   <asp:GridView ID="gvNotifications" runat="server" AutoGenerateColumns="false" ShowHeader="false"
            CssClass="display w-full table-auto text-sm"  
            OnRowDeleting="DrugsGridView_RowDeleting"
            DataKeyNames="NotificationID" 
            GridLines="None" HeaderStyle-CssClass="bg-gray-200 font-semibold text-gray-700 text-center"
            RowStyle-CssClass="text-center px-4 py-2 border-b" EmptyDataText="No notifications found." 
                OnRowCommand="gvNotifications_RowCommand"
                       >
                <Columns>
                    <asp:TemplateField HeaderText="Sr. No.">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                         <ItemStyle CssClass="text-left px-4 py-2 border-b" Width="5%" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Title" HeaderText="Title" ItemStyle-CssClass="text-left px-4 py-2 border-b font-semibold text-gray-700" ItemStyle-Width="20%" />
                    <asp:BoundField DataField="Message" HeaderText="Message" ItemStyle-CssClass="px-4 py-2 border-b text-left" ItemStyle-Width="20%" />
                    <asp:BoundField DataField="CreatedAt" HeaderText="Created At" DataFormatString="{0:dd-MM-yyyy HH:mm}" ItemStyle-CssClass="px-4 py-2 border-b text-left" ItemStyle-Width="20%" />
                    
                    <asp:TemplateField HeaderText="PDF" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink1" runat="server" 
                                NavigateUrl='<%# string.IsNullOrEmpty(Eval("PdfFilePath") as string) ? "#" : Eval("PdfFilePath") %>' 
                                Text='<%# string.IsNullOrEmpty(Eval("PdfFilePath") as string) ? "N/A" : "View PDF" %>' 
                                CssClass="text-blue-600 hover:underline">
                            </asp:HyperLink>
                        </ItemTemplate>
                        <ItemStyle CssClass="px-4 py-2 border-b text-center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Actions" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:Button ID="btnDelete" runat="server" CommandName="DeleteNotification" CommandArgument='<%# Eval("NotificationID") %>'
                            CssClass="text-red-600 hover:text-red-800 text-sm font-semibold" Text="🗑️ Delete"
                            OnClientClick="return confirm('Are you sure you want to delete this notification?');" />

                        </ItemTemplate>
                        <ItemStyle CssClass="px-4 py-2 border-b text-center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

    </div>

    </div>
        <!-- DataTables Initialization Script -->
 <script>
     $(document).ready(function () {
         $('#<%= gvNotifications.ClientID %>').DataTable({
             paging: true,
             searching: true,
             ordering: true,
             info: true,
             lengthMenu: [5, 10, 25, 50, 100, 200],
             pageLength: 50,
             columns: [
                 { title: "Sr. No." },
                 { title: "Title" },
                 { title: "Message" },
                 { title: "Created At" },
                 { title: "PDF" },
                 { title: "Actions" }  // Add this to match your GridView
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

