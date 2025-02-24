<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ChemistDashboard.aspx.cs" Inherits="FrontEnd_ChemistDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">



     <!-- Main Content -->
  <main class="py-10">
    <div class="container mx-auto px-4">

        <!-- Content Section -->
      <section class="mb-5">
        <h2 class="text-2xl font-bold text-gray-800"><asp:Label ID="lblWelcomeUser" runat="server"></asp:Label>
            </h2>
        <p class="text-gray-900">Manage your drug inventory efficiently with Narcotic Drugs Monitoring System.</p>
      </section>


      <!-- Dashboard Section -->
      <section class="bg-white p-4 rounded-lg shadow-lg dashboard-sec shadow-blue-900/50">
        <h2 class="text-2xl font-bold text-gray-800 mb-4">Dashboard</h2>
        <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
          <!-- Card 1 -->
          <div class="bg-green-50 p-6 rounded-lg shadow-md hover:shadow-lg transition-shadow duration-300 flex flex-col flex-grow bg-com-green dashb">
            <h3 class=" mb-4">Total Drug Catageories</h3>
            <p class="text-4xl font-bold "> <a href="DrugStockList.aspx"> <asp:Label ID="lblTotalCount" runat="server" ></asp:Label> </a> </p>
            <p class=" mt-2">
                <%--The Drugs liable to be misused (Schedule H & H1)--%>

            </p>
          </div>

          <!-- Card 2 -->
          <div class="bg-blue-50 p-6 rounded-lg shadow-md hover:shadow-lg transition-shadow duration-300 flex flex-col flex-grow bg-com-bl dashb">
            <h3 class="mb-4">Total Sale </h3>
            <p class="text-4xl font-bold ">
               <a href="PatientStockList.aspx"> <asp:Label ID="lblTotalPatients" runat="server"> </asp:Label> </a>
            </p>
            <p class="text-gray-600 mt-2">Total Patient listed</p>
          </div>

          <!-- Card 3 -->
        <%--  <div class="bg-white p-6 rounded-lg shadow-md">
            <h3 class="text-lg font-bold text-gray-800 mb-4">Total Chemists</h3>
            <p class="text-4xl font-bold text-yellow-500">
                <asp:Label ID="lblTotalChemists" runat="server" ></asp:Label>
            </p>
            <p class="text-gray-600 mt-2">Active Chemists </p>
          </div>--%>

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

