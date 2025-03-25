<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<main class="py-12  ">
    <div class="container mx-auto px-4 sm:px-6 lg:px-8">

        <!-- Welcome Section -->
        <section class="mb-5">
            <h2 class="text-3xl font-semibold text-gray-800 ">
                <asp:Label ID="lblWelcomeUser" runat="server"></asp:Label>
            </h2>
            <p class="text-gray-600 text-lg leading-relaxed">
                Manage your drug inventory efficiently with the Narcotic Drugs Monitoring System.
            </p>
        </section>

        <!-- Dashboard Section -->
        <section class="bg-white p-4 rounded-lg shadow-lg dashboard-sec shadow-blue-900/50">
            <h2 class="text-2xl font-bold text-gray-800 mb-4">Dashboard</h2>

            <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
                
                 <!-- Card: Quick Links -->
                <div class="bg-green-50 p-6 rounded-lg shadow-md hover:shadow-lg transition-shadow duration-300 flex flex-col flex-grow bg-com-green">
                    <h3 class="mb-4">Quick Links</h3>
                    <div class="grid grid-cols-2 gap-4 ul-li">
                        <a href="#" class="hover:underline hover:text-gray-800 font-medium"><i class="fa fa-angle-right" aria-hidden="true"></i> Dashboard</a>
                        <a href="StockList.aspx" class="hover:underline hover:text-gray-800 font-medium"><i class="fa fa-angle-right" aria-hidden="true"></i> Stock List</a>
                        <a href="InsertChemist.aspx" class="hover:underline hover:text-gray-800 font-medium"><i class="fa fa-angle-right" aria-hidden="true"></i> Chemist Entry</a>
                        <a href="ChemistList.aspx" class="hover:underline hover:text-gray-800 font-medium"><i class="fa fa-angle-right" aria-hidden="true"></i> Chemist List</a>
                        <a href="Drugsentry.aspx" class="hover:underline hover:text-gray-800 font-medium"><i class="fa fa-angle-right" aria-hidden="true"></i> Drug Entry</a>
                        <a href="Drugslist.aspx" class="hover:underline hover:text-gray-800 font-medium"><i class="fa fa-angle-right" aria-hidden="true"></i> Drugs List</a>
                        <a href="ExpiredStockReport.aspx" class="hover:underline hover:text-gray-800 font-medium"><i class="fa fa-angle-right" aria-hidden="true"></i> Expired Stock</a>
                       
                    </div>
                </div>

                <!-- Card: Total Drugs -->
                <div class="bg-blue-50 p-6 rounded-lg shadow-md hover:shadow-lg transition-shadow duration-300 flex flex-col flex-grow bg-com-bl dashb">
                    <h3 class=" mb-4">Total Drugs</h3>
                    <p class="text-4xl font-bold  mb-4">
                        <a href="Drugslist.aspx" class="hover:underline transition-colors duration-300">
                            <asp:Label ID="lblTotalDrugs" runat="server"></asp:Label>
                        </a>
                    </p>
                    <p class=" mt-2 flex-1">
                        The Drugs liable to be misused (Schedule H & H1)
                    </p>
                </div>

                <!-- Card: Total Stock -->
                <div class="bg-red-50 p-6 rounded-lg shadow-md hover:shadow-lg transition-shadow duration-300 flex flex-col flex-grow bg-com-yellow dashb" id="HideTotalStock" runat="server" visible="false">
                    <h3 class=" mb-4">Total Stock</h3>
                    <p class="text-4xl font-bold  mb-4">
                        <a href="stockList.aspx" class="hover:underline hover:text-red-600 transition-colors duration-300">
                            <asp:Label ID="lblTotalStock" runat="server"></asp:Label>
                        </a>
                    </p>
                    <p class="text-gray-600 mt-2 flex-1">
                        Total stock of the listed chemists
                    </p>
                </div>

                <!-- Card: Total Chemists -->
                <div class="bg-yellow-50 p-6 rounded-lg shadow-md hover:shadow-lg transition-shadow duration-300 flex flex-col flex-grow bg-com-yellow dashb">
                    <h3 class=" mb-4">Total Chemists</h3>
                    <p class="text-4xl font-bold  mb-4">
                        <a href="ChemistList.aspx" class="hover:underline hover:text-yellow-600 transition-colors duration-300">
                            <asp:Label ID="lblTotalChemists" runat="server"></asp:Label>
                        </a>
                    </p>
                    <p class=" mt-2 flex-1">
                        Active Chemists
                    </p>
                </div>

            </div>
        </section>


        
       <div class="grid grid-cols-1 md:grid-cols-2 gap-6 mt-6">
            <!-- Drug-wise Stock Chart -->
            <section class="bg-white p-8 rounded-lg shadow-lg">
        <h2 class="text-3xl font-semibold text-gray-800 mb-4">
            <asp:Label ID="Label1" runat="server"></asp:Label>
        </h2>
        <div id="DrugChart" runat="server" class="bg-white p-6 rounded-lg shadow-lg border border-gray-200 flex flex-col justify-between">
            <h2 class="text-xl font-bold text-gray-800 mb-4 flex items-center">💊 Drug-wise Stock</h2>
            <p class="text-gray-600 mb-3">
                This graph displays the <b>Top 5 Drugs Stockist Quantity</b>, highlighting which drug has the highest stock and which chemist holds it.
            </p>
            <canvas id="drugStockChart"></canvas>
        </div>
    </section>
            <!-- Drug-wise Sale Chart -->
            <section class="bg-white p-8 rounded-lg shadow-lg">
        <h2 class="text-3xl font-semibold text-gray-800 mb-4">
            <asp:Label ID="Label3" runat="server"></asp:Label>
        </h2>
        <div id="Div1" runat="server" class="bg-white p-6 rounded-lg shadow-lg border border-gray-200 flex flex-col justify-between">
            <h2 class="text-xl font-bold text-gray-800 mb-4 flex items-center">💊 Drug-wise Sale</h2>
            <p class="text-gray-600 mb-3">
                This graph highlights the <b>Top 5 Sold Drugs </b>, providing insights into the quantity sold, drug name, category, and the associated chemist.
            </p>
            <canvas id="TopSale"></canvas>
        </div>
    </section>
        </div>


     </div>
</main>

   


        <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>


        <script>
        document.addEventListener("DOMContentLoaded", function () {
            if (typeof drugStockData !== "undefined" && drugStockData.length > 0) {
                // Combine Drug Name and Chemist Name for better clarity
                let drugLabels = drugStockData.map(item => `${item.DrugName} - ${item.Category} `);
                let drugStock = drugStockData.map(item => item.Quantity);

                var ctx2 = document.getElementById("drugStockChart").getContext("2d");
                new Chart(ctx2, {
                    type: "bar",  // bar , line , radar , doughnut, polarArea , bubble , scatter
                    data: {
                        labels: drugLabels,
                        datasets: [{
                            label: "Stock Quantity",
                            data: drugStock,
                            backgroundColor: ["#EF4444", "#3B82F6", "#10B981", "#FBBF24", "#A855F7"]
                        }]
                    },
                    options: {
                        responsive: true,
                        plugins: {
                            legend: { display: false },
                            tooltip: {
                                callbacks: {
                                    label: function (tooltipItem) {
                                        let dataIndex = tooltipItem.dataIndex;
                                        let chemistName = drugStockData[dataIndex].ChemistName;
                                        let Category = drugStockData[dataIndex].Category;
                                        return `Stock: ${tooltipItem.raw} (Chemist: ${chemistName})`;
                                    }
                                }
                            }
                        },
                        scales: {
                            y: { beginAtZero: true }
                        }
                    }
                });
            }
        });
    </script>

        <script>
            document.addEventListener("DOMContentLoaded", function () {
                // Ensure topSaleData is defined before proceeding
                if (typeof topSaleData !== "undefined" && Array.isArray(topSaleData) && topSaleData.length > 0) {

                    // Extract relevant data
                    let drugLabels = topSaleData.map(item => `${item.DrugName} - ${item.Category}`);
                    let drugStock = topSaleData.map(item => item.QuantitySold);

                    // Get canvas context
                    var ctx2 = document.getElementById("TopSale").getContext("2d");

                    // Create Chart
                    new Chart(ctx2, {
                        type: "bar",
                        data: {
                            labels: drugLabels,
                            datasets: [{
                                label: "Stock Quantity",
                                data: drugStock,
                                backgroundColor: ["#EF4444", "#3B82F6", "#10B981", "#FBBF24", "#A855F7"]
                            }]
                        },
                        options: {
                            responsive: true,
                            plugins: {
                                legend: { display: false },
                                tooltip: {
                                    callbacks: {
                                        label: function (tooltipItem) {
                                            let dataIndex = tooltipItem.dataIndex;
                                            let chemistName = topSaleData[dataIndex]?.ChemistName || "Unknown";
                                            return `Quantity: ${tooltipItem.raw} (Chemist: ${chemistName})`;
                                        }
                                    }
                                }
                            },
                            scales: {
                                y: { beginAtZero: true }
                            }
                        }
                    });
                } else {
                    console.warn("topSaleData is undefined or empty.");
                }
            });
        </script>

    
</asp:Content>

