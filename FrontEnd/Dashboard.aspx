<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

     <!-- Main Content -->
  <main class="py-10">
    <div class="container mx-auto px-4">
      <!-- Dashboard Section -->
      <section class="mb-10">
        <h2 class="text-2xl font-bold text-gray-800 mb-6">Dashboard</h2>
        <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
          <!-- Card 1 -->
          <div class="bg-white p-6 rounded-lg shadow-md">
            <h3 class="text-lg font-bold text-gray-800 mb-4">Total Drugs</h3>
            <p class="text-4xl font-bold text-blue-600">1,234</p>
            <p class="text-gray-600 mt-2">Drugs currently in stock</p>
          </div>

          <!-- Card 2 -->
          <div class="bg-white p-6 rounded-lg shadow-md">
            <h3 class="text-lg font-bold text-gray-800 mb-4">Low Stock</h3>
            <p class="text-4xl font-bold text-red-500">24</p>
            <p class="text-gray-600 mt-2">Drugs below threshold</p>
          </div>

          <!-- Card 3 -->
          <div class="bg-white p-6 rounded-lg shadow-md">
            <h3 class="text-lg font-bold text-gray-800 mb-4">Expired Drugs</h3>
            <p class="text-4xl font-bold text-yellow-500">15</p>
            <p class="text-gray-600 mt-2">Drugs past expiry date</p>
          </div>

          <!-- Card 4 -->
          <div class="bg-white p-6 rounded-lg shadow-md">
            <h3 class="text-lg font-bold text-gray-800 mb-4">Suppliers</h3>
            <p class="text-4xl font-bold text-green-500">56</p>
            <p class="text-gray-600 mt-2">Active suppliers</p>
          </div>
        </div>
      </section>

      <!-- Content Section -->
      <section>
        <h2 class="text-2xl font-bold text-gray-800 mb-6">Welcome to PharmaStockPro</h2>
        <p class="text-gray-600">Manage your drug inventory efficiently with this responsive application.</p>
      </section>
    </div>
  </main>


</asp:Content>

