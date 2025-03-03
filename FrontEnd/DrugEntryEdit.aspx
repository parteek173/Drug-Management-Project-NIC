<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeFile="DrugEntryEdit.aspx.cs" Inherits="FrontEnd_DrugEntryEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <div class="flex items-center justify-center min-h-screen">
        <div class="bg-white shadow-lg rounded-lg p-6 w-full max-w-4xl">
            <h1 class="text-2xl font-bold text-center text-gray-700 mb-6">Edit Sale Entry</h1>

            <asp:HiddenField ID="hfPatientID" runat="server" />

            <div class="grid grid-cols-2 gap-4">
                <!-- Date Field -->
                <div>
                    <label for="txtDate" class="block text-sm font-medium text-gray-600">Bill Date</label>
                    <asp:TextBox ID="txtDate" runat="server" TextMode="Date" CssClass="form-input" ReadOnly="true"></asp:TextBox>
                </div>

                <div>
                    <label for="txtBillNumber" class="block text-sm font-medium text-gray-600">Bill Number</label>
                    <asp:TextBox ID="txtBillNumber" runat="server" CssClass="form-input"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtbillNumber" ErrorMessage="Bill Number is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                </div>

                <!-- Patient Name -->
                <div>
                    <label for="txtPatientName" class="block text-sm font-medium text-gray-600">Patient Name</label>
                    <asp:TextBox ID="txtPatientName" runat="server" CssClass="form-input"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPatientName" runat="server" ControlToValidate="txtPatientName" ErrorMessage="Patient name is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator ID="revPatientName" runat="server" ControlToValidate="txtPatientName" ErrorMessage="Invalid name (letters and spaces only)" ValidationExpression="^[a-zA-Z\s]+$" CssClass="text-red-500"></asp:RegularExpressionValidator>
                </div>

                <!-- Mobile Number -->
                <div>
                    <label for="txtMobileNumber" class="block text-sm font-medium text-gray-600">Mobile Number</label>
                    <asp:TextBox ID="txtMobileNumber" runat="server" CssClass="form-input" MaxLength="10"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvMobileNumber" runat="server" ControlToValidate="txtMobileNumber" ErrorMessage="Mobile number is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revMobileNumber" runat="server" ControlToValidate="txtMobileNumber" ErrorMessage="Invalid mobile number" ValidationExpression="^\d{10}$" CssClass="text-red-500"></asp:RegularExpressionValidator>
                </div>

                <!-- Patient Address -->
                <div>
                    <label for="txtPatientAddress" class="block text-sm font-medium text-gray-600">Patient Address</label>
                    <asp:TextBox ID="txtPatientAddress" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-input"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPatientAddress" runat="server" ControlToValidate="txtPatientAddress" ErrorMessage="Patient address is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                </div>

                <!-- Drug Name -->
                <div>
                  <label for="ddlDrugName" class="block text-sm font-medium text-gray-600">Drug Name</label>
                  <asp:DropDownList ID="DropDrugName" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" AutoPostBack="true"></asp:DropDownList>

              </div>

              <div>
                  <label for="ddlCategory" class="block text-sm font-medium text-gray-600">Category</label>
                  <asp:DropDownList ID="DropCategory" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" ReadOnly="true"></asp:DropDownList>

              </div>

                <div>
                    <label for="ddlBatchNumber" class="block text-sm font-medium text-gray-900">Batch Number <span class="text-red-500">*</span> </label>
                   <asp:DropDownList ID="DropBatchNumber" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlBatchNumber_SelectedIndexChanged" ReadOnly="true"></asp:DropDownList>

                </div>

                 <!-- Total Quantity-->
                  <div>
                      <label for="txtQuantity" class="block text-sm font-medium text-gray-600">Total Quantity</label>
                      <asp:TextBox ID="txtTotalQuantity" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" ReadOnly="true"></asp:TextBox>
                      <asp:Label id="TotalQuantityError" class="text-red-500" style="display: none;" runat="server">Drug is currently out of stock!</asp:Label>
                  </div>

                <!-- Quantity Sold -->
                <div>
                    <label for="txtQuantitySold" class="block text-sm font-medium text-gray-600">Quantity Sold</label>
                    <asp:TextBox ID="txtQuantitySold" runat="server" CssClass="form-input" MaxLength="3"></asp:TextBox>
                     <asp:RequiredFieldValidator ID="rfvQuantitySold" runat="server" ControlToValidate="txtQuantitySold" ErrorMessage="Quantity sold is required" CssClass="text-red-500" />
                    <asp:RangeValidator ID="rvQuantitySold" runat="server" ControlToValidate="txtQuantitySold" ErrorMessage="Quantity sold must be a positive number" CssClass="text-red-500" 
                     MinimumValue="1" MaximumValue="2147483647" Type="Integer" />

                    <span id="quantityError" class="text-red-500" style="display: none;">Quantity sold should not be more than total quantity</span>
                </div>

                <!-- Prescribed By -->
                <div>
                    <label for="txtPrescribedBy" class="block text-sm font-medium text-gray-600">Prescribed By</label>
                    <asp:TextBox ID="txtPrescribedBy" runat="server" CssClass="form-input"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPrescribedBy" runat="server" ControlToValidate="txtPrescribedBy" ErrorMessage="Prescriber name is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                </div>

                <!-- Hospital Name -->
               <div>
                <label for="txtHospitalName" class="block text-sm font-medium text-gray-600">Hospital Name</label>
                <asp:DropDownList ID="txtHospitalName" runat="server" CssClass="form-input">
                    <asp:ListItem Text="Select Hospital" Value="" />
                    <asp:ListItem Text="Government Medical College and Hospital, Chandigarh Sector 32 CHD" Value="Government Medical College and Hospital, Chandigarh Sector 32 CHD" />
                    <asp:ListItem Text="Government Multi Specialty Hospital Sector 16 , CHD" Value="Government Multi Specialty Hospital Sector 16 , CHD" />
                    <asp:ListItem Text="Postgraduate Institute of Medical Education and Research Sector 12" Value="Postgraduate Institute of Medical Education and Research Sector 12" />
                     <asp:ListItem Text="Civil Hospital, Sector-22" Value="Civil Hospital, Sector-22" />
                     <asp:ListItem Text="Civil Hospital, Sector-45" Value="Civil Hospital, Sector-45" />
                     <asp:ListItem Text="Civil Hospital, Manimajra" Value="Civil Hospital, Manimajra" />
                     <asp:ListItem Text="GMCH, South Campus, Sector 48" Value="GMCH, South Campus, Sector 48" />
                     <asp:ListItem Text="Mental Health Institute (MHI) Sector-32" Value="Mental Health Institute (MHI) Sector-32" />
                    <asp:ListItem Text="Other/Private" Value="Other/Private" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvHospitalName" runat="server" ControlToValidate="txtHospitalName"
                    InitialValue="" ErrorMessage="Hospital name is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
            </div>


                <!-- Hospital Address -->
                <div>
                    <label for="txtHospitalAddress" class="block text-sm font-medium text-gray-600">Hospital Address</label>
                    <asp:TextBox ID="txtHospitalAddress" runat="server" CssClass="form-input"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvHospitalAddress" runat="server" ControlToValidate="txtHospitalAddress"
                            ErrorMessage="Hospital address is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="mt-6 text-center">
                <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="bg-blue-500 text-white px-6 py-2 rounded shadow hover:bg-blue-600" OnClientClick="return validateForm();" OnClick="btnUpdate_Click" />
            </div>
        </div>
    </div>

    <style>
    .form-input {
        display: block;
        width: 100%;
        padding: 0.5rem;
        border: 1px solid #ccc;
        border-radius: 0.25rem;
        outline: none;
    }
</style>

    <script>

        function validateForm() {
            // Perform ASP.NET validation before allowing submission
            if (typeof (Page_ClientValidate) == 'function') {
                if (!Page_ClientValidate()) {
                    return false; // Stop submission if validation fails
                }
            }
            return validateQuantity(); // Call your quantity check function
        }


        function validateQuantity() {
            var totalQty = parseInt(document.getElementById('<%= txtTotalQuantity.ClientID %>').value) || 0;
            var soldQty = parseInt(document.getElementById('<%= txtQuantitySold.ClientID %>').value) || 0;
            var errorMsg = document.getElementById('quantityError');
            var TotalQuantityError = document.getElementById('TotalQuantityError');



            if (totalQty == 0) {
                TotalQuantityError.style.display = 'block';
                return false;
            }

            if (soldQty > totalQty) {
                errorMsg.style.display = 'block';  // Show error message
                return false;
            } else {
                errorMsg.style.display = 'none';  // Hide error message
            }
            return true;
                }

        document.addEventListener("DOMContentLoaded", function () {
            var mobileInput = document.getElementById("<%= txtMobileNumber.ClientID %>");

             mobileInput.addEventListener("keypress", function (e) {
                 if (e.key < "0" || e.key > "9") {
                     e.preventDefault(); // Block non-numeric input
                 }
             });
        });

        document.addEventListener("DOMContentLoaded", function () {
            var mobileInput = document.getElementById("<%= txtQuantitySold.ClientID %>");

              mobileInput.addEventListener("keypress", function (e) {
                  if (e.key < "0" || e.key > "9") {
                      e.preventDefault(); // Block non-numeric input
                  }
              });
          });
    </script>


</asp:Content>

