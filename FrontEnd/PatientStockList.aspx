<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeFile="PatientStockList.aspx.cs" Inherits="FrontEnd_PatientStockList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <div class="container mx-auto p-4  flex flex-col">
        <h1 class="text-3xl font-bold text-center mb-6">Sale List</h1>

        <!-- Date Filters -->
        <div class="flex justify-center space-x-4 mb-4">
            <input type="date" id="fromDate" class="border p-2 rounded" />
            <input type="date" id="toDate" class="border p-2 rounded" />

            <a href="javascript:void(0)" id="btnFilter" 
               class="bg-blue-900 text-white px-4 py-2 rounded shadow hover:bg-blue-600 transition-all">
               <i class="fa fa-filter" aria-hidden="true"></i>  Filter
            </a>

            <a href="javascript:void(0)" id="btnReset" 
               class="bg-gray-900 text-white px-4 py-2 rounded shadow hover:bg-gray-600 transition-all">
                <i class="fa fa-refresh" aria-hidden="true"></i> Reset
            </a>

            <a href="javascript:void(0);" onclick="exportData()" 
               class="bg-green-900 text-white px-4 py-2 rounded shadow hover:bg-green-600 transition-all">
                <i class="fa fa-download" aria-hidden="true"></i> Export Data
            </a>
        </div>

        <!-- Table for displaying patient stock data -->
        <div class="overflow-x-auto flex-grow">
            <table id="patientStockTable" class="display w-full text-sm">
                <thead>
                     <tr class="text-gray-700">
                <th style="width:80px;">Bill Date</th>
                <th>Bill Number</th>
                <th>Patient Name</th>
                <th>Drug Name</th>
                <th>Category</th>
                <th>Batch Number</th>
                <th>Quantity Sold</th>
                <th>Quantity Return</th>
                <th>Mobile Number</th>                
                <th>Patient Address</th>
                <th>Prescribed By</th>
                <th>Hospital Name</th>
                <th>Hospital Address</th>
                <th style="display: none;">Created Date</th> 
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
            var table = $('#patientStockTable').DataTable({
                "ajax": {
                    "url": "PatientStockList.aspx/GetPatientStockData",
                    "type": "POST",
                    "contentType": "application/json; charset=utf-8",
                    "dataType": "json",
                    "dataSrc": function (json) {
                        return JSON.parse(json.d);
                    }
                },
                "columns": [
                    { "data": "DateOFSale" },
                    { "data": "BillNumber" },
                    { "data": "PatientName" },
                    { "data": "DrugName" },
                    { "data": "Category" },
                    { "data": "BatchNumber" },
                    { "data": "QuantitySold" },
                    {
                        "data": "ReturnQuantity",
                        "render": function (data, type, row) {
                            return data == null ? 0 : data; // Show 0 if null
                        }
                    },                   
                    { "data": "MobileNumber" },                   
                    { "data": "PatientAddress" },
                    { "data": "PrescribedBy" },
                    { "data": "HospitalName" },
                    { "data": "HospitalAddress" },
                    {
                        "data": "CreatedDate",
                        "render": function (data, type, row) {
                            if (!data) return ''; // Handle null values

                            // Convert "dd-MM-yyyy HH:mm:ss" to "YYYY-MM-DD" format
                            var dateParts = data.split(" ")[0].split("-"); // Extract "dd-MM-yyyy"
                            if (dateParts.length !== 3) return ''; // Invalid format check

                            var formattedDate = new Date(dateParts[2], dateParts[1] - 1, dateParts[0]); // Year, Month (0-based), Day

                            // Get today's date in "dd-MM-yyyy" format
                            var today = new Date();
                            var todayFormatted =
                                String(today.getDate()).padStart(2, '0') + '-' +
                                String(today.getMonth() + 1).padStart(2, '0') + '-' +
                                today.getFullYear();

                            // Convert isReturned to a number explicitly
                            var isReturnedValue = Number(row.isReturned) || 0; // Default to 0 if null/undefined

                            // Check if CreatedDate matches today's date AND isReturned is NOT 1
                            if (data.startsWith(todayFormatted) && isReturnedValue !== 1) {
                                return `
                        <a href="javascript:void(0);" onclick="editEntry('${row.id}')" class="text-blue-500 mr-3">
                            ✏️
                        </a>
                    `;
                            }
                            return ''; // Hide icon if CreatedDate is not today or isReturned is 1
                        }
                    }
                ],
                "order": [[10, "desc"]], // Sort by CreatedDate (column index 10) in descending order
                "columnDefs": [
                    { "targets": 10, "visible": false }, // Ensure CreatedDate column stays hidden
                    { "orderable": false, "targets": -1 } // Disable sorting for last column (Action)
                ]
            });





            // Filter Button Click Event
            $("#btnFilter").click(function () {
                var fromDate = $("#fromDate").val();
                var toDate = $("#toDate").val();

                if (fromDate && toDate) {
                    $.ajax({
                        url: "PatientStockList.aspx/GetFilteredPatientStockData",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({ fromDate: fromDate, toDate: toDate }),
                        success: function (response) {
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
                table.ajax.url("PatientStockList.aspx/GetPatientStockData").load();
            });
        });

        function exportData() {
            window.location.href = 'PatientStockList.aspx?export=1';
        }

        function editEntry(patientID) {
            var encryptedID = btoa(patientID);
            window.location.href = "DrugEntryEdit.aspx?PatientID=" + encodeURIComponent(encryptedID);
        }

        function deleteEntry(patientName) {
            if (confirm("Are you sure you want to delete this entry?")) {
                alert("Delete clicked for: " + patientName);
                // Implement delete logic
            }
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



