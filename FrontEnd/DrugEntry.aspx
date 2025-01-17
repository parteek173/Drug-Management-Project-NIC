<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeFile="DrugEntry.aspx.cs" Inherits="FrontEnd_DrugEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Center the content vertically and horizontally using flex -->
    <div class="flex items-center justify-center min-h-screen">
        <div class="bg-white shadow-lg rounded-lg p-6 w-full max-w-md">
            <h1 class="text-2xl font-bold text-center text-gray-700 mb-6">Drug Entry Form</h1>
            
            <!-- Name Field -->
            <div class="mb-4">
                <label for="name" class="block text-sm font-medium text-gray-600">Drug Name</label>
                <input type="text" id="name" name="name" runat="server"
                    class="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" 
                    placeholder="Drug name" required />
            </div>

            <!-- Email Field -->
            <div class="mb-4">
                <label for="text" class="block text-sm font-medium text-gray-600">Dosage</label>
                <input type="text" id="quantity" name="quantity" runat="server"
                    class="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                    placeholder="Enter drug dosage" required />
            </div>

            <!-- Message Field -->
            <div class="mb-4">
                <label for="message" class="block text-sm font-medium text-gray-600">Description</label>
                <textarea id="description" name="description" runat="server"
                    class="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                    rows="4" placeholder="Write drug description" required></textarea>
            </div>

            <!-- Buttons -->
            <div class="flex items-center justify-between">
                <button type="submit" runat="server" class="bg-blue-500 text-white px-4 py-2 rounded-lg shadow hover:bg-blue-600">
                    Submit
                </button>
                <button type="reset" class="bg-gray-300 text-gray-700 px-4 py-2 rounded-lg shadow hover:bg-gray-400">
                    Reset
                </button>
            </div>
        </div>
    </div>
</asp:Content>

