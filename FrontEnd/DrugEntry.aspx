<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeFile="DrugEntry.aspx.cs" Inherits="FrontEnd_DrugEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Center the content vertically and horizontally using flex -->
    <div class="flex items-center justify-center min-h-screen">
        <div class="bg-white shadow-lg rounded-lg p-6 w-full max-w-4xl">
            <h1 class="text-2xl font-bold text-center text-gray-700 mb-6">Sale Entry Form</h1>

            <!-- Validation Summary -->
            <%--<asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="text-red-500 mb-4" />--%>

            <!-- Form Fields -->
            <div class="grid grid-cols-2 gap-4">
                <!-- Date Field -->
               <div>
                    <label for="txtDate" class="block text-sm font-medium text-gray-600">Date <span class="text-red-500">*</span> </label>
                    <asp:TextBox ID="txtDate" runat="server" TextMode="Date" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" ReadOnly="true"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDate" ErrorMessage="Date is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="rvDate" runat="server" ControlToValidate="txtDate" ErrorMessage="Invalid date" CssClass="text-red-500" MinimumValue="1900-01-01" MaximumValue="2099-12-31" Type="Date"></asp:RangeValidator>
                </div>


                <!-- Patient Name Field -->
                <div>
                    <label for="txtPatientName" class="block text-sm font-medium text-gray-600">Patient Name <span class="text-red-500">*</span> </label>
                    <asp:TextBox ID="txtPatientName" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Enter patient name" autocomplete="off"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPatientName" runat="server" ControlToValidate="txtPatientName" ErrorMessage="Patient name is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revPatientName" runat="server" ControlToValidate="txtPatientName" ErrorMessage="Invalid name (letters and spaces only)" ValidationExpression="^[a-zA-Z\s]+$" CssClass="text-red-500"></asp:RegularExpressionValidator>
                </div>

                <!-- Mobile Number Field -->
                <div>
                    <label for="txtMobileNumber" class="block text-sm font-medium text-gray-600">Mobile Number <span class="text-red-500">*</span> </label>
                    <asp:TextBox ID="txtMobileNumber" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Enter mobile number" autocomplete="off" MaxLength="10"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvMobileNumber" runat="server" ControlToValidate="txtMobileNumber" ErrorMessage="Mobile number is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revMobileNumber" runat="server" ControlToValidate="txtMobileNumber" ErrorMessage="Invalid mobile number" ValidationExpression="^\d{10}$" CssClass="text-red-500"></asp:RegularExpressionValidator>
                </div>

                <!-- Patient ID Field -->
               <div>
                <label for="txtPatientAddress" class="block text-sm font-medium text-gray-600">Patient Address <span class="text-red-500">*</span> </label>
                <asp:TextBox ID="txtPatientAddress" runat="server" TextMode="MultiLine" Rows="3"
                    CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                    placeholder="Enter patient address" autocomplete="off"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPatientAddress" runat="server" ControlToValidate="txtPatientAddress"
                    ErrorMessage="Patient address is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
            </div>

                <div>
                    <label for="ddlDrugName" class="block text-sm font-medium text-gray-600">Drug Name <span class="text-red-500">*</span> </label>
                    <asp:DropDownList ID="ddlDrugName" runat="server" AutoPostBack="true" 
                        CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                        OnSelectedIndexChanged="ddlDrugName_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>

                <div>
                    <label for="ddlCategory" class="block text-sm font-medium text-gray-600">Category <span class="text-red-500">*</span> </label>
                    <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="true" 
                        OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                        CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm">
                    </asp:DropDownList>
                </div>

                <div>
                    <label for="txtQuantity" class="block text-sm font-medium text-gray-600">Total Quantity <span class="text-red-500">*</span> </label>
                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" ReadOnly="true"></asp:TextBox>
                    <span id="TotalQuantityError" class="text-red-500" style="display: none;">Drug is currently out of stock!</span>
                </div>

                 <div>
                    <label for="txtQuantitySold" class="block text-sm font-medium text-gray-600">Quantity Sold <span class="text-red-500">*</span> </label>
                    <asp:TextBox ID="txtQuantitySold" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" 
                                 Placeholder="Enter quantity sold" MaxLength="3"/>
                    <asp:RequiredFieldValidator ID="rfvQuantitySold" runat="server" ControlToValidate="txtQuantitySold" 
                                                ErrorMessage="Quantity sold is required" CssClass="text-red-500" />
                    <asp:RangeValidator ID="rvQuantitySold" runat="server" ControlToValidate="txtQuantitySold" 
                                        ErrorMessage="Quantity sold must be a positive number" CssClass="text-red-500" 
                                        MinimumValue="1" MaximumValue="2147483647" Type="Integer" />

                         <span id="quantityError" class="text-red-500" style="display: none;">Quantity sold should not be more than total quantity</span>

                </div>


                <!-- Prescribed By Field -->
                <div>
                    <label for="txtPrescribedBy" class="block text-sm font-medium text-gray-600">Prescribed By <span class="text-red-500">*</span> </label>
                    <asp:TextBox ID="txtPrescribedBy" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Enter prescriber name" autocomplete="off"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPrescribedBy" runat="server" ControlToValidate="txtPrescribedBy" ErrorMessage="Prescriber name is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                </div>

              <!-- Hospital Name Field -->
                <div>
                    <label for="txtHospitalName" class="block text-sm font-medium text-gray-600">
                        Hospital/Clinic Name <span class="text-red-500">*</span>
                    </label>
                    <asp:DropDownList ID="txtHospitalName" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm">
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


                <!-- Hospital/Clinic Address Field -->
                <div>
                    <label for="txtHospitalAddress" class="block text-sm font-medium text-gray-600">Hospital/Clinic Address <span class="text-red-500">*</span> </label>
                    <asp:TextBox ID="txtHospitalAddress" runat="server" TextMode="MultiLine" Rows="3"
                                 CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                                 placeholder="Enter hospital address" autocomplete="off"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvHospitalAddress" runat="server" ControlToValidate="txtHospitalAddress"
                                                ErrorMessage="Hospital address is required" CssClass="text-red-500"></asp:RequiredFieldValidator>
                </div>
    </div>


            <!-- Buttons -->
            <div class="flex items-center justify-between mt-6">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="bg-blue-500 text-white px-4 py-2 rounded-lg shadow hover:bg-blue-600" OnClientClick="return validateForm();" OnClick="btnSubmit_Click" />
                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="bg-gray-300 text-gray-700 px-4 py-2 rounded-lg shadow hover:bg-gray-400" OnClientClick="resetForm(); return false;" />
            </div>
        </div>
    </div>

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
            var totalQty = parseInt(document.getElementById('<%= txtQuantity.ClientID %>').value) || 0;
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
    </script>
</asp:Content>



