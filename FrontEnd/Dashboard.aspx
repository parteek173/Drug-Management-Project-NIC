﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<main class="py-12 bg-gray-100 min-h-screen">
    <div class="container mx-auto px-4 sm:px-6 lg:px-8">

        <!-- Welcome Section -->
        <section class="bg-white p-8 rounded-lg shadow-lg mb-8">
            <h2 class="text-3xl font-semibold text-gray-800 mb-4">
                <asp:Label ID="lblWelcomeUser" runat="server"></asp:Label>
            </h2>
            <p class="text-gray-600 text-lg leading-relaxed">
                Manage your drug inventory efficiently with the Narcotic Drugs Monitoring System.
            </p>
        </section>

        <!-- Dashboard Section -->
        <section class="bg-white p-8 rounded-lg shadow-lg">
            <h2 class="text-2xl font-bold text-gray-800 mb-8">Dashboard</h2>

            <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
                
                 <!-- Card: Quick Links -->
                <div class="bg-green-50 p-6 rounded-lg shadow-md hover:shadow-lg transition-shadow duration-300 flex flex-col flex-grow">
                    <h3 class="text-lg font-semibold text-gray-800 mb-4">Quick Links</h3>
                    <div class="grid grid-cols-2 gap-4 text-blue-600">
                        <a href="#" class="hover:underline hover:text-blue-700">Dashboard</a>
                        <a href="StockList.aspx" class="hover:underline hover:text-blue-700">Stock List</a>
                        <a href="InsertChemist.aspx" class="hover:underline hover:text-blue-700">Chemist Entry</a>
                        <a href="ChemistList.aspx" class="hover:underline hover:text-blue-700">Chemist List</a>
                        <a href="Drugsentry.aspx" class="hover:underline hover:text-blue-700">Drug Entry</a>
                        <a href="Drugslist.aspx" class="hover:underline hover:text-blue-700">Drugs List</a>
                       
                    </div>
                </div>

                <!-- Card: Total Drugs -->
                <div class="bg-blue-50 p-6 rounded-lg shadow-md hover:shadow-lg transition-shadow duration-300 flex flex-col flex-grow">
                    <h3 class="text-lg font-semibold text-gray-800 mb-4">Total Drugs</h3>
                    <p class="text-4xl font-bold text-blue-600 mb-4">
                        <a href="Drugslist.aspx" class="hover:underline hover:text-blue-700 transition-colors duration-300">
                            <asp:Label ID="lblTotalDrugs" runat="server"></asp:Label>
                        </a>
                    </p>
                    <p class="text-gray-600 mt-2 flex-1">
                        The Drugs liable to be misused (Schedule H & H1)
                    </p>
                </div>

                <!-- Card: Total Stock -->
                <div class="bg-red-50 p-6 rounded-lg shadow-md hover:shadow-lg transition-shadow duration-300 flex flex-col flex-grow" id="HideTotalStock" runat="server" visible="false">
                    <h3 class="text-lg font-semibold text-gray-800 mb-4">Total Stock</h3>
                    <p class="text-4xl font-bold text-red-500 mb-4">
                        <a href="stockList.aspx" class="hover:underline hover:text-red-600 transition-colors duration-300">
                            <asp:Label ID="lblTotalStock" runat="server"></asp:Label>
                        </a>
                    </p>
                    <p class="text-gray-600 mt-2 flex-1">
                        Total stock of the listed chemists
                    </p>
                </div>

                <!-- Card: Total Chemists -->
                <div class="bg-yellow-50 p-6 rounded-lg shadow-md hover:shadow-lg transition-shadow duration-300 flex flex-col flex-grow">
                    <h3 class="text-lg font-semibold text-gray-800 mb-4">Total Chemists</h3>
                    <p class="text-4xl font-bold text-yellow-500 mb-4">
                        <a href="ChemistList.aspx" class="hover:underline hover:text-yellow-600 transition-colors duration-300">
                            <asp:Label ID="lblTotalChemists" runat="server"></asp:Label>
                        </a>
                    </p>
                    <p class="text-gray-600 mt-2 flex-1">
                        Active Chemists
                    </p>
                </div>
                
               
               


            </div>
        </section>
    </div>
</main>

   
<!-- Main Content -->
  <%--<main class="py-10">
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
          <div class="bg-white p-6 rounded-lg shadow-md" id="HideTotalStock" runat="server" visible="false">
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
        </div>
      </section>
    </div>
  </main>--%>


</asp:Content>

