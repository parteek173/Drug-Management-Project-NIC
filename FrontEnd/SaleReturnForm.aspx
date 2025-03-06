<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeFile="SaleReturnForm.aspx.cs" Inherits="FrontEnd_SaleReturnForm" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <div class="max-w-6xl mx-auto p-6 bg-white shadow-lg rounded-lg">
        <h2 class="text-2xl font-semibold text-gray-700 mb-4">Sale Return Form</h2>

        <!-- Search Form -->
        <div class="bg-gray-100 p-4 rounded-lg shadow-sm mb-6">
            <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
                <div>
                    <label class="block text-sm font-medium text-gray-600">Bill Number</label>
                    <asp:TextBox ID="txtBillNumber" runat="server" CssClass="w-full p-2 border rounded-lg" autocomplete="off" MaxLength="20"></asp:TextBox>
                </div>
                <div>
                    <label class="block text-sm font-medium text-gray-600">Mobile Number</label>
                    <asp:TextBox ID="txtMobileNumber" runat="server" CssClass="w-full p-2 border rounded-lg" autocomplete="off" MaxLength="10"></asp:TextBox>
                </div>
                <div class="flex items-end">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 transition" OnClick="btnSearch_Click" />
                </div>
            </div>
        </div>

        <!-- GridView for Patient Records -->
        <div class="overflow-x-auto">
            <asp:GridView ID="gvPatientRecords" runat="server" AutoGenerateColumns="False" DataKeyNames="id"
                CssClass="w-full border border-gray-300 rounded-lg shadow-sm"
                OnRowCommand="gvPatientRecords_RowCommand">
                <HeaderStyle CssClass="bg-blue-600 text-white font-semibold p-3 text-left" />
                <RowStyle CssClass="border-b border-gray-200 p-3" />
                <AlternatingRowStyle CssClass="bg-gray-100" />
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="ID" Visible="false" />
                    <asp:BoundField DataField="PatientName" HeaderText="Patient Name" />
                    <asp:BoundField DataField="MobileNumber" HeaderText="Mobile Number" />
                    <asp:BoundField DataField="PatientAddress" HeaderText="Address" />
                    <asp:BoundField DataField="DrugName" HeaderText="Drug Name" />
                    <asp:BoundField DataField="Category" HeaderText="Category" />
                    <asp:BoundField DataField="BatchNumber" HeaderText="Batch Number" />
                    <asp:BoundField DataField="QuantitySold" HeaderText="Quantity Sold" />
                    <asp:BoundField DataField="BillNumber" HeaderText="Bill Number" />
                    <asp:TemplateField HeaderText="Return Quantity">
                        <ItemTemplate>
                                    <asp:TextBox ID="txtReturnQuantity" runat="server" CssClass="w-20 p-1 border rounded-lg text-center"
                                        MaxLength="3" onkeypress="return isNumeric(event)" inputmode="numeric" autocomplete="off"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnReturnStock" runat="server" CommandName="ReturnStock"
                                CommandArgument="<%# Container.DisplayIndex %>"
                                Text="Return Stock"
                                CssClass="bg-green-600 text-white px-4 py-2 rounded-lg hover:bg-green-700 transition" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

         <!-- No Record Found Message -->
        <div id="noRecordsFound" runat="server" visible="false" class="text-center text-red-600 font-semibold mt-4">
            No records found for the given Bill Number or Mobile Number.
        </div>

    </div>

    <script type="text/javascript">
    function isNumeric(event) {
        var keyCode = event.which ? event.which : event.keyCode;
        if (keyCode < 48 || keyCode > 57) {
            event.preventDefault();
            return false;
        }
        return true;
        }

        document.addEventListener("DOMContentLoaded", function () {
            var mobileInput = document.getElementById("<%= txtMobileNumber.ClientID %>");

               mobileInput.addEventListener("keypress", function (e) {
                   if (e.key < "0" || e.key > "9") {
                       e.preventDefault(); // Block non-numeric input
                   }
               });
        });

    </script>


  </asp:Content>


