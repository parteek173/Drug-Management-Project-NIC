<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeFile="StockListEdit.aspx.cs" Inherits="FrontEnd_StockListEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <div class="flex items-center justify-center min-h-screen">
        <div class="bg-white shadow-lg rounded-lg p-6 w-full max-w-4xl">
            <h1 class="text-2xl font-bold text-center text-gray-700 mb-6">Edit Stock Entry</h1>
            
            <asp:HiddenField ID="hiddenStockID" runat="server" />
            
            <div class="grid grid-cols-2 gap-6">
                <div>
                    <label for="txtDrugName" class="block text-sm font-medium text-gray-600">Drug Name <span class="text-red-500">*</span> </label>
                    <asp:DropDownList ID="txtDrugName" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm"></asp:DropDownList>
                </div>
                
                <div>
                    <label for="ddlCategory" class="block text-sm font-medium text-gray-600">Category <span class="text-red-500">*</span> </label>
                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm">
                        <asp:ListItem Text="Injection" Value="Injection" />
                        <asp:ListItem Text="Capsules/Tablet" Value="Capsules/Tablet" />
                        <asp:ListItem Text="Ointment" Value="Ointment" />
                        <asp:ListItem Text="Syrup" Value="Syrup" />
                    </asp:DropDownList>
                </div>
                
                <div>
                    <label for="brandName" class="block text-sm font-medium text-gray-600">Brand Name <span class="text-red-500">*</span> </label>
                    <asp:TextBox ID="brandName" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvBrandName" runat="server" ControlToValidate="brandName" 
                        InitialValue="" ErrorMessage="Please Enter Brand Name" CssClass="text-red-500">
                    </asp:RequiredFieldValidator>
                </div>
                
                <div>
                    <label for="batchNumber" class="block text-sm font-medium text-gray-600">Batch Number <span class="text-red-500">*</span> </label>
                    <asp:TextBox ID="batchNumber" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="batchNumber" 
                        InitialValue="" ErrorMessage="Please Enter Batch Number" CssClass="text-red-500">
                    </asp:RequiredFieldValidator>
                </div>

                  <div>
                      <label for="billDate" class="block text-sm font-medium text-gray-600">Bill Date <span class="text-red-500">*</span> </label>
                      <asp:TextBox ID="txtbillDate" runat="server" TextMode="Date" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm"></asp:TextBox>
                      <asp:RequiredFieldValidator ID="billdate" runat="server" ControlToValidate="txtBillDate" ErrorMessage="Bill Date is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                      <asp:RangeValidator ID="billdateR" runat="server" ControlToValidate="txtBillDate" ErrorMessage="Invalid date" CssClass="text-red-500" MinimumValue="1900-01-01" MaximumValue="2099-12-31" Type="Date"></asp:RangeValidator>

                  </div>
  
                  <div>
                      <label for="billNumber" class="block text-sm font-medium text-gray-600">Bill Number <span class="text-red-500">*</span> </label>
                      <asp:TextBox ID="txtbillNumber" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm"></asp:TextBox>
                      <asp:RequiredFieldValidator ID="bNumber" runat="server" ControlToValidate="txtBillNumber" ErrorMessage="Bill Number is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                  </div>
                
                <div>
                    <label for="txtDate" class="block text-sm font-medium text-gray-600">Expiry Date <span class="text-red-500">*</span> </label>
                    <asp:TextBox ID="txtDate" runat="server" TextMode="Date" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDate" ErrorMessage="Date is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="rvDate" runat="server" ControlToValidate="txtDate" ErrorMessage="Invalid date" CssClass="text-red-500" MinimumValue="1900-01-01" MaximumValue="2099-12-31" Type="Date"></asp:RangeValidator>
                </div>
                
                <div>
                    <label for="txtQuantity" class="block text-sm font-medium text-gray-600">Quantity <span class="text-red-500">*</span> </label>
                    <asp:TextBox ID="txtQuantity" type="number" min="1" max="100000" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtQuantity" ErrorMessage="Quantity is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revQuantity" runat="server" ControlToValidate="txtQuantity" ErrorMessage="Invalid quantity (only numbers)" ValidationExpression="^\d+$" CssClass="text-red-500"></asp:RegularExpressionValidator>
                    <span id="TotalQuantityError" class="text-red-500" style="display: none;">Quantity cannot be zero!</span>
                    <span id="MaxQuantityError" class="text-red-500" style="display: none;">Quantity cannot be more than 1000!</span>
                </div>

                <div>
                    <label for="txtPurchasedFrom" class="block text-sm font-medium text-gray-600">Purchased From <span class="text-red-500">*</span> </label>
                    <asp:TextBox ID="txtPurchasedFrom" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtpurchasedFrom" ErrorMessage="Field is required" CssClass="text-red-500"></asp:RequiredFieldValidator>                             
                </div>
            </div>
            
            <div class="flex items-center justify-between mt-6">
                <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="bg-blue-500 text-white px-4 py-2 rounded-lg shadow" OnClick="btnUpdate_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="bg-gray-300 text-gray-700 px-4 py-2 rounded-lg shadow" OnClientClick="window.history.back(); return false;" />
            </div>
        </div>
    </div>
</asp:Content>

