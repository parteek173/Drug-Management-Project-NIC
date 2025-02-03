<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeFile="PatientStockList.aspx.cs" Inherits="FrontEnd_PatientStockList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <div class="container mx-auto p-4 min-h-screen flex flex-col">
        <h1 class="text-3xl font-bold text-center mb-6">Patient Sale Drugs List</h1>

        <!-- Date Filters -->
        <div class="flex justify-center space-x-4 mb-4">
            <input type="date" id="fromDate" class="border p-2 rounded" />
            <input type="date" id="toDate" class="border p-2 rounded" />
            <a href="javascript:void(0)" id="btnFilter" class="bg-blue-500 text-white px-4 py-2 rounded">Filter</a>
            <a href="javascript:void(0)" id="btnReset" class="bg-gray-500 text-white px-4 py-2 rounded">Reset</a>
        </div>

        <!-- Table for displaying patient stock data -->
        <div class="overflow-x-auto flex-grow">
            <table id="patientStockTable" class="display w-full text-sm">
                <thead>
                    <tr class="bg-gray-200 text-gray-700">
                        <th>Patient Name</th>
                        <th>Drug Name</th>
                        <th>Category</th>
                        <th>Quantity Sold</th>
                        <th>Mobile Number</th>
                        <th>Date of Sale</th>
                        <th>Patient ID</th>
                        <th>Prescribed By</th>
                        <th>Hospital Name</th>
                        <th>Doctor Name</th>
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
            // Load all patient stock data by default
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
                    { "data": "PatientName" },
                    { "data": "DrugName" },
                    { "data": "Category" },
                    { "data": "QuantitySold" },
                    { "data": "MobileNumber" },
                    { "data": "DateOFSale" },
                    { "data": "PatientID" },
                    { "data": "PrescribedBy" },
                    { "data": "HospitalName" },
                    { "data": "DoctorName" }
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
                table.ajax.url("PatientStockList.aspx/GetPatientStockData").load(); // Load all data again
            });
        });
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


