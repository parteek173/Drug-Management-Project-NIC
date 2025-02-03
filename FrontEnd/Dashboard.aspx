﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

     <!-- Main Content -->
  <main class="py-10">
    <div class="container mx-auto px-4">

        <!-- Content Section -->
      <section>
        <h2 class="text-2xl font-bold text-gray-800 mb-6"><asp:Label ID="lblWelcomeUser" runat="server"></asp:Label>
            </h2>
        <p class="text-gray-600">Manage your drug inventory efficiently with Narcotic Drugs Monitoring System.</p>
      </section>


      <!-- Dashboard Section -->
      <section class="mb-10 mt-8">
        <h2 class="text-2xl font-bold text-gray-800 mb-6">Dashboard</h2>
        <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
          <!-- Card 1 -->
          <div class="bg-white p-6 rounded-lg shadow-md">
            <h3 class="text-lg font-bold text-gray-800 mb-4">Total Drugs</h3>
            <p class="text-4xl font-bold text-blue-600">
                <a href="Drugslist.aspx">
                <asp:Label ID="lblTotalDrugs" runat="server" ></asp:Label>
                    </a>
              </p>
            <p class="text-gray-600 mt-2">
                The Drugs liable to be misused (Schedule H & H1)

            </p>
          </div>

          <!-- Card 2 -->
          <div class="bg-white p-6 rounded-lg shadow-md">
            <h3 class="text-lg font-bold text-gray-800 mb-4">Total Stock</h3>
            <p class="text-4xl font-bold text-red-500">
                <a href="stockList.aspx">
                <asp:Label ID="lblTotalStock" runat="server"></asp:Label></a>
            </p>
            <p class="text-gray-600 mt-2">Total stock of the listed chemists</p>
          </div>

          <!-- Card 3 -->
          <div class="bg-white p-6 rounded-lg shadow-md">
            <h3 class="text-lg font-bold text-gray-800 mb-4">Total Chemists</h3>
            <p class="text-4xl font-bold text-yellow-500">
                <a href="ChemistList.aspx">
                <asp:Label ID="lblTotalChemists" runat="server" ></asp:Label>
                    </a>
            </p>
            <p class="text-gray-600 mt-2">Active Chemists </p>
          </div>

          <!-- Card 4 -->
          <%--<div class="bg-white p-6 rounded-lg shadow-md">
            <h3 class="text-lg font-bold text-gray-800 mb-4">Suppliers</h3>
            <p class="text-4xl font-bold text-green-500">56</p>
            <p class="text-gray-600 mt-2">Active suppliers</p>
          </div>--%>


        </div>
      </section>

      
    </div>
  </main>


</asp:Content>

