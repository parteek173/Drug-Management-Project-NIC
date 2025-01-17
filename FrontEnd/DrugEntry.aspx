<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DrugEntry.aspx.cs" Inherits="FrontEnd_DrugEntry" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Responsive Form</title>
    <!-- Tailwind CSS CDN -->
    <script src="https://cdn.tailwindcss.com"></script>
</head>
<body class="bg-gray-100">
    <form id="form1" runat="server" class="min-h-screen flex items-center justify-center">
        <div class="bg-white shadow-lg rounded-lg p-6 w-full max-w-md">
            <h1 class="text-2xl font-bold text-center text-gray-700 mb-6">Contact Us</h1>
            
            <!-- Name Field -->
            <div class="mb-4">
                <label for="name" class="block text-sm font-medium text-gray-600">Name</label>
                <input type="text" id="name" name="name" runat="server"
                    class="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" 
                    placeholder="Enter your name" required />
            </div>

            <!-- Email Field -->
            <div class="mb-4">
                <label for="email" class="block text-sm font-medium text-gray-600">Email</label>
                <input type="email" id="email" name="email" runat="server"
                    class="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                    placeholder="Enter your email" required />
            </div>

            <!-- Message Field -->
            <div class="mb-4">
                <label for="message" class="block text-sm font-medium text-gray-600">Message</label>
                <textarea id="message" name="message" runat="server"
                    class="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                    rows="4" placeholder="Write your message" required></textarea>
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
    </form>
</body>
</html>
