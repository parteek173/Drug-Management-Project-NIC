<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeFile="DrugStockList.aspx.cs" Inherits="DrugStockList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <div class="container mx-auto p-4 min-h-screen flex flex-col">
        <h1 class="text-3xl font-bold text-center mb-6">Drug Stock List</h1>

        <!-- Date Filters -->
        <div class="flex justify-center space-x-4 mb-4">
            <input type="date" id="fromDate" class="border p-2 rounded" />
            <input type="date" id="toDate" class="border p-2 rounded" />

            <a href="javascript:void(0)" id="btnFilter" 
               class="bg-blue-500 text-white px-4 py-2 rounded shadow hover:bg-blue-600 transition-all">
               🔍 Filter
            </a>

            <a href="javascript:void(0)" id="btnReset" 
               class="bg-gray-500 text-white px-4 py-2 rounded shadow hover:bg-gray-600 transition-all">
               🔄 Reset
            </a>

            <a href="javascript:void(0);" onclick="exportData()" 
               class="bg-green-500 text-white px-4 py-2 rounded shadow hover:bg-green-600 transition-all">
               📥 Export Data
            </a>
        </div>

        <!-- Table for displaying stock data -->
        <div class="overflow-x-auto flex-grow">
            <table id="stockTable" class="display w-full text-sm">
                <thead>
                    <tr class="bg-gray-200 text-gray-700">
                        <th>Drug Name</th>
                        <th>Quantity</th>
                        <th>Expiry Date</th>
                        <th>Category</th>
                        <th>Batch Number</th>
                        <th>Brand Name</th>
                        <th>Drug Purchase Date</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody></tbody>
            </table>
        </div>
    </div>

    <!-- jQuery & DataTables -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.datatables.net/1.10.25/js/jquery.dataTables.min.js"></script>
    <link rel="stylesheet" href="https://cdn.datatables.net/1.10.25/css/jquery.dataTables.min.css" />

    <script>
        $(document).ready(function () {
            // Default Data (All Stock Data)
            var table = $('#stockTable').DataTable({
                "ajax": {
                    "url": "DrugStockList.aspx/GetStockData",  // Call to fetch all data
                    "type": "POST",
                    "contentType": "application/json; charset=utf-8",
                    "dataType": "json",
                    "dataSrc": function (json) {
                        return JSON.parse(json.d);
                    }
                },
                "columns": [
                    { "data": "DrugName" },
                    { "data": "Quantity" },
                    { "data": "ExpiryDate" },
                    { "data": "Category" },
                    { "data": "BatchNumber" },
                    { "data": "BrandName" },
                    { "data": "CreatedDate" },
                    {
                        "data": null,
                        "orderable": false, // Disable sorting for Action column
                        "render": function (data, type, row) {
                            if (!row.CreatedDate) return ''; // Handle null or empty dates

                            // Convert CreatedDate from "dd-MM-yyyy" to a JavaScript Date object
                            var parts = row.CreatedDate.split('-'); // Split by "-"
                            if (parts.length !== 3) return ''; // Invalid date format, return empty

                            var createdDate = new Date(parts[2], parts[1] - 1, parts[0]); // Year, Month (0-based), Day

                            // Get today's date in dd-MM-yyyy format
                            var today = new Date();
                            var todayFormatted =
                                String(today.getDate()).padStart(2, '0') + '-' +
                                String(today.getMonth() + 1).padStart(2, '0') + '-' +
                                today.getFullYear();

                            // Compare formatted date with today's date
                            if (row.CreatedDate === todayFormatted) {
                                return `
                            <a href="javascript:void(0);" onclick="editEntry('${row.id}')" 
                                class="text-blue-500 mr-3">
                                ✏️
                            </a>
                            <a href="javascript:void(0);" onclick="deleteEntry('${row.id}')" 
                                class="text-red-500">
                                ❌
                            </a>
                        `;
                            }
                            return ''; // No icons if CreatedDate is not today
                        }
                    }

                ],
                "columnDefs": [
                    { "orderable": false, "targets": -1 } // Make the last column non-sortable
                ]
            });

            window.editEntry = function (id) {
                window.location.href = "StockListEdit.aspx?StockID=" + id;
            };

            // Function to Delete Entry
            window.deleteEntry = function (id) {
                if (confirm("Are you sure you want to delete this record?")) {
                    $.ajax({
                        url: "DrugStockList.aspx/DeleteStockEntry",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({ id: id }),
                        success: function (response) {
                            var result = JSON.parse(response.d);
                            if (result.success) {
                                alert(result.message);
                                $('#stockTable').DataTable().ajax.reload(); // Refresh table after deletion
                            } else {
                                alert("Error: " + result.message);
                            }
                        },
                        error: function () {
                            alert("An error occurred while deleting the record.");
                        }
                    });
                }
            };


            // Filter Button Click Event
            $("#btnFilter").click(function () {
                var fromDate = $("#fromDate").val();
                var toDate = $("#toDate").val();
                var table = $('#stockTable').DataTable();

                if (fromDate && toDate) {
                    // Make AJAX call to fetch filtered data
                    $.ajax({
                        url: "DrugStockList.aspx/GetFilteredStockData",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({
                            fromDate: fromDate,
                            toDate: toDate
                        }),
                        success: function (response) {
                            // Clear previous table data and add new filtered data
                            var data = JSON.parse(response.d);
                            table.clear().rows.add(data).draw();
                        }
                    });
                } else {
                    alert("Please select both From Date and To Date.");
                }
            });

            // Reset Button Click Event
            $("#btnReset").click(function () {
                $("#fromDate").val('');
                $("#toDate").val('');
                table.ajax.url("DrugStockList.aspx/GetStockData").load(); // Load all data
            });
        });

        function exportData() {
            window.location.href = 'DrugStockList.aspx?export=1';
        }

    </script>


         <style>
        /* Adjust the overall width of the DataTables length menu select box */
        .dataTables_length select {
            width: auto; /* Allow the width to adjust based on content */
            min-width: 7rem; /* Set a minimum width for the dropdown */
            padding-right: 2rem; /* Add space for the dropdown arrow */
            appearance: none; /* Remove native dropdown styling */
            border: 1px solid #cbd5e1; /* Tailwind gray-300 */
            border-radius: 0.375rem; /* Tailwind rounded-md */
            background-color: #ffffff; /* White background */
            color: #334155; /* Tailwind slate-700 for text */
            font-size: 0.875rem; /* Tailwind text-sm */
            line-height: 1.25rem; /* Tailwind leading-5 */
            background-image: url('data:image/svg+xml;charset=UTF-8,%3csvg xmlns%3d%22http://www.w3.org/2000/svg%22 width%3d%2224%22 height%3d%2224%22 viewBox%3d%220 0 24 24%22 fill%3d%22none%22 stroke%3d%22%234A5568%22 stroke-width%3d%222%22 stroke-linecap%3d%22round%22 stroke-linejoin%3d%22round%22%3e%3cpolyline points%3d%226 9 12 15 18 9%22/%3e%3c/svg%3e');
            background-repeat: no-repeat;
            background-position: right 0.75rem center; /* Position dropdown arrow */
            background-size: 1rem;
            cursor: pointer; /* Pointer cursor for dropdown */
        }

        .dataTables_length select:focus {
            outline: 2px solid #2563eb; /* Tailwind blue-600 */
            outline-offset: 2px;
            }

            /* Hide extra sorting arrows in DataTables */
    .dataTables_wrapper table.dataTable thead .sorting::before,
    .dataTables_wrapper table.dataTable thead .sorting::after,
    .dataTables_wrapper table.dataTable thead .sorting_asc::before,
    .dataTables_wrapper table.dataTable thead .sorting_asc::after,
    .dataTables_wrapper table.dataTable thead .sorting_desc::before,
    .dataTables_wrapper table.dataTable thead .sorting_desc::after {
        opacity: 0.5; /* Reduce visibility of extra arrows */
    }

    /* Ensure only one set of sorting arrows is displayed */
    table.dataTable thead th {
        position: relative;
    }

    table.dataTable thead th::before,
    table.dataTable thead th::after {
        right: 0.5em !important;
    }

    /* Hide duplicate ASP.NET GridView sorting arrows if they exist */
    table th a {
        text-decoration: none;
        display: inline-block;
        position: relative;
    }

    table th a::after {
        display: none !important;
    }



    </style>


</asp:Content>









