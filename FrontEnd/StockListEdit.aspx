<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeFile="StockListEdit.aspx.cs" Inherits="FrontEnd_StockListEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <div class="flex items-center justify-center min-h-screen">
        <div class="bg-white shadow-lg rounded-lg p-6 w-full max-w-4xl">
            <h1 class="text-2xl font-bold text-center text-gray-700 mb-6">Edit Stock Entry</h1>
            
            <asp:HiddenField ID="hiddenStockID" runat="server" />
            
            <div class="grid grid-cols-2 gap-6">
                <div>
                    <label for="txtDrugName" class="block text-sm font-medium text-gray-600">Drug Name</label>
                    <asp:DropDownList ID="txtDrugName" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm"></asp:DropDownList>
                </div>
                
                <div>
                    <label for="ddlCategory" class="block text-sm font-medium text-gray-600">Category</label>
                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm">
                        <asp:ListItem Text="Injection" Value="Injection" />
                        <asp:ListItem Text="Capsules/Tablet" Value="Capsules/Tablet" />
                        <asp:ListItem Text="Ointment" Value="Ointment" />
                        <asp:ListItem Text="Syrup" Value="Syrup" />
                    </asp:DropDownList>
                </div>
                
                <div>
                    <label for="brandName" class="block text-sm font-medium text-gray-600">Brand Name</label>
                    <asp:TextBox ID="brandName" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm"></asp:TextBox>
                </div>
                
                <div>
                    <label for="batchNumber" class="block text-sm font-medium text-gray-600">Batch Number</label>
                    <asp:TextBox ID="batchNumber" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm"></asp:TextBox>
                </div>
                
                <div>
                    <label for="txtDate" class="block text-sm font-medium text-gray-600">Expiry Date</label>
                    <asp:TextBox ID="txtDate" runat="server" TextMode="Date" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm"></asp:TextBox>
                </div>
                
                <div>
                    <label for="txtQuantity" class="block text-sm font-medium text-gray-600">Quantity</label>
                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm"></asp:TextBox>
                </div>
            </div>
            
            <div class="flex items-center justify-between mt-6">
                <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="bg-blue-500 text-white px-4 py-2 rounded-lg shadow" OnClick="btnUpdate_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="bg-gray-300 text-gray-700 px-4 py-2 rounded-lg shadow" OnClientClick="window.history.back(); return false;" />
            </div>
        </div>
    </div>
</asp:Content>

