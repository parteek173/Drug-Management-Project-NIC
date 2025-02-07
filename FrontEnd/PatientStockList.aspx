<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeFile="PatientStockList.aspx.cs" Inherits="FrontEnd_PatientStockList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <div class="container mx-auto p-4 min-h-screen flex flex-col">
        <h1 class="text-3xl font-bold text-center mb-6">Sale List</h1>

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
                        <th>Patient Address</th>
                        <th>Prescribed By</th>
                        <th>Hospital Name</th>
                        <th>Hospital Address</th>
                        <th>Action</th> <!-- New Action Column -->
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
                    { "data": "PatientName" },
                    { "data": "DrugName" },
                    { "data": "Category" },
                    { "data": "QuantitySold" },
                    { "data": "MobileNumber" },
                    { "data": "DateOFSale" },
                    { "data": "PatientAddress" },
                    { "data": "PrescribedBy" },
                    { "data": "HospitalName" },
                    { "data": "HospitalAddress" },
                    {
                        "data": null,
                        "render": function (data, type, row) {
                            return `
                                <a href="javascript:void(0);" onclick="editEntry('${row.PatientName}')" class="text-blue-500 mr-3">
                                    ✏️ 
                                </a>
                                <a href="javascript:void(0);" onclick="deleteEntry('${row.PatientName}')" class="text-red-500">
                                    🗑️ 
                                </a>
                            `;
                        }
                    }
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

        function editEntry(patientName) {
            alert("Edit clicked for: " + patientName);
            // Implement edit logic
        }

        function deleteEntry(patientName) {
            if (confirm("Are you sure you want to delete this entry?")) {
                alert("Delete clicked for: " + patientName);
                // Implement delete logic
            }
        }
    </script>
</asp:Content>



