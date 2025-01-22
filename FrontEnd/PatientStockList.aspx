<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeFile="PatientStockList.aspx.cs" Inherits="FrontEnd_PatientStockList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <div class="container mx-auto p-4 min-h-screen flex flex-col">
        <h1 class="text-3xl font-bold text-center mb-6">Patient Sale Drugs List</h1>

        <!-- Table for displaying patient data -->
        <div class="overflow-x-auto flex-grow">
            <asp:GridView ID="patientGridView" runat="server" AutoGenerateColumns="false" 
                CssClass="display w-full table-auto text-sm" 
                ShowHeader="false" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="PatientName" HeaderText="Patient Name" />
                    <asp:BoundField DataField="MobileNumber" HeaderText="Mobile Number" />
                    <asp:BoundField DataField="DateOFSale" HeaderText="Date of Sale" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:BoundField DataField="PatientID" HeaderText="Patient ID" />
                    <asp:BoundField DataField="PrescribedBy" HeaderText="Prescribed By" />
                    <asp:BoundField DataField="HospitalName" HeaderText="Hospital Name" />
                    <asp:BoundField DataField="DoctorName" HeaderText="Doctor Name" />
                </Columns>
            </asp:GridView>
        </div>

        <!-- DataTables Initialization Script -->
        <script>
            $(document).ready(function () {
                $('#<%= patientGridView.ClientID %>').DataTable({
                    paging: true,
                    searching: true,
                    ordering: true,
                    info: true,
                    lengthMenu: [5, 10, 25, 50],
                    pageLength: 10,
                    columns: [
                        { title: "Patient Name" },
                        { title: "Mobile Number" },
                        { title: "Date of Sale" },
                        { title: "Patient ID" },
                        { title: "Prescribed By" },
                        { title: "Hospital Name" },
                        { title: "Doctor Name" }
                    ]
                });
            });
        </script>
    </div>
</asp:Content>

