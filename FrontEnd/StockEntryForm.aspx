<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeFile="StockEntryForm.aspx.cs" Inherits="FrontEnd_StockEntryForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Center the content vertically and horizontally using flex -->
    <div class="flex items-center justify-center ">
        <div class="container mx-auto p-6 bg-white rounded-lg shadow-lg max-w-2xl shadow-blue-900/50 mt-20">
            <h1 class="text-2xl font-bold text-center text-gray-700 mb-6">Stock Entry Form</h1>

            <!-- Validation Summary -->
            <%--<asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="text-red-500 mb-4" />--%>

            <!-- Grid Layout for Form Fields -->
            <div class="grid grid-cols-2 gap-x-6 gap-y-0">
                <!-- Drug Name Field -->
                <div>
                    <label for="txtDrugName" class="block text-sm font-medium text-gray-900">Drug Name <span class="text-red-500">*</span> </label>
                    <asp:DropDownList ID="txtDrugName" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvDrugName" runat="server" ControlToValidate="txtDrugName" 
                        InitialValue="" ErrorMessage="Please select a drug" CssClass="text-red-500">
                    </asp:RequiredFieldValidator>
                </div>
                      
                    <!-- Category Dropdown -->
                    <div>
                        <label for="ddlCategory" class="block text-sm font-medium text-gray-900">Category <span class="text-red-500">*</span> </label>
                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm">
                            <asp:ListItem Text="Select Category" Value="" />
                            <asp:ListItem Text="Injection" Value="Injection" />
                            <asp:ListItem Text="Capsules/Tablet" Value="Capsules/Tablet" />
                            <asp:ListItem Text="Ointment" Value="Ointment" />
                            <asp:ListItem Text="Syrup" Value="Syrup" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ControlToValidate="ddlCategory" InitialValue="" ErrorMessage="Category is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                    </div>

                <!-- Brand Name -->
                <div>
                    <label for="brandName" class="block text-sm font-medium text-gray-900">Brand Name <span class="text-red-500">*</span> </label>
                    <asp:TextBox ID="brandName" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Enter Brand Name" autocomplete="off"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvBrandName" runat="server" ControlToValidate="brandName" ErrorMessage="Brand Name is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                </div>

                  <!-- Batch Number -->
                  <div>
                      <label for="batchNumber" class="block text-sm font-medium text-gray-900">Batch Number <span class="text-red-500">*</span> </label>
                      <asp:TextBox ID="batchNumber" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Enter Batch Number" autocomplete="off"></asp:TextBox>
                      <asp:RequiredFieldValidator ID="rfvBatchNumber" runat="server" ControlToValidate="batchNumber" ErrorMessage="Batch Number is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                  </div>         
                
                
                <!-- Bitt Date -->
                <div>
                    <label for="BillDate" class="block text-sm font-medium text-gray-900">Bill Date <span class="text-red-500">*</span> </label>
                    <asp:TextBox ID="txtBillDate" runat="server" TextMode="Date" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Enter Bill Date"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="billdate" runat="server" ControlToValidate="txtBillDate" ErrorMessage="Bill Date is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="billdateR" runat="server" ControlToValidate="txtBillDate" ErrorMessage="Invalid date" CssClass="text-red-500" MinimumValue="1900-01-01" MaximumValue="2099-12-31" Type="Date"></asp:RangeValidator>
                </div>

                  <!-- Bill Number -->
                  <div>
                      <label for="BillNumber" class="block text-sm font-medium text-gray-900">Bill Number <span class="text-red-500">*</span> </label>
                      <asp:TextBox ID="txtBillNumber" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Enter Bill Number" autocomplete="off"></asp:TextBox>
                      <asp:RequiredFieldValidator ID="bNumber" runat="server" ControlToValidate="txtBillNumber" ErrorMessage="Bill Number is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                  </div>  

                  <!-- Expiry Date Field -->
                      <div>
                          <label for="txtDate" class="block text-sm font-medium text-gray-900">Expiry Date <span class="text-red-500">*</span> </label>
                          <asp:TextBox ID="txtDate" runat="server" TextMode="Date" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Enter date"></asp:TextBox>
                          <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDate" ErrorMessage="Date is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                          <asp:RangeValidator ID="rvDate" runat="server" ControlToValidate="txtDate" ErrorMessage="Invalid date" CssClass="text-red-500" MinimumValue="1900-01-01" MaximumValue="2099-12-31" Type="Date"></asp:RangeValidator>
                      </div>    
            
                 <!-- Quantity Field -->
                      <div>
                          <label for="txtQuantity" class="block text-sm font-medium text-gray-900">Quantity <span class="text-red-500">*</span> </label>
                          <asp:TextBox ID="txtQuantity" runat="server" type="number" min="1" max="100000" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Enter quantity" autocomplete="off"></asp:TextBox>
                          <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtQuantity" ErrorMessage="Quantity is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                          <asp:RegularExpressionValidator ID="revQuantity" runat="server" ControlToValidate="txtQuantity" ErrorMessage="Invalid quantity (only numbers)" ValidationExpression="^\d+$" CssClass="text-red-500"></asp:RegularExpressionValidator>
                          <span id="TotalQuantityError" class="text-red-500" style="display: none;">Quantity cannot be zero!</span>
                          <span id="MaxQuantityError" class="text-red-500" style="display: none;">Quantity cannot be more than 1000!</span>
                      </div>

                <!-- Purchased From -->
                         <div>
                             <label for="purchasedFrom" class="block text-sm font-medium text-gray-900"> Purchased From <span class="text-red-500">*</span> </label>
                             <asp:TextBox ID="txtpurchasedFrom" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Purchased From"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtpurchasedFrom" ErrorMessage="Field is required" CssClass="text-red-500"></asp:RequiredFieldValidator>                             
                         </div> 
                </div>

            <!-- Buttons -->
            <div class="flex items-center justify-between mt-2">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="bg-green-900 text-white px-4 py-2 rounded-lg shadow hover:bg-green-600" OnClientClick="return validateForm();" OnClick="btnSubmit_Click" />
                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="bg-gray-300 text-gray-700 px-4 py-2 rounded-lg shadow hover:bg-gray-400" OnClientClick="resetForm(); return false;" />
            </div>
        </div>
    </div>

    <script>
        function validateForm() {
            // Ensure ASP.NET validators are checked first
            if (Page_ClientValidate()) {
                var totalQty = parseInt(document.getElementById('<%= txtQuantity.ClientID %>').value) || 0;
                var TotalQuantityError = document.getElementById('TotalQuantityError');
                var MaxQuantityError = document.getElementById('MaxQuantityError');

                if (totalQty === 0) {
                    TotalQuantityError.style.display = 'block';
                    return false;
                }
                else if (totalQty > 1000) {
                    MaxQuantityError.style.display = 'block';
                    return false;
                }
                else {
                    TotalQuantityError.style.display = 'none';
                    MaxQuantityError.style.display = 'none';
                }
                return true; // Allow form submission if all validations pass
            }
            return false; // Prevent form submission if any ASP.NET validation fails
        }
    </script>

</asp:Content>
