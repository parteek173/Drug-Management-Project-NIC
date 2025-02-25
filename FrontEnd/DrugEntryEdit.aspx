<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeFile="DrugEntryEdit.aspx.cs" Inherits="FrontEnd_DrugEntryEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <div class="flex items-center justify-center min-h-screen">
        <div class="bg-white shadow-lg rounded-lg p-6 w-full max-w-4xl">
            <h1 class="text-2xl font-bold text-center text-gray-700 mb-6">Edit Sale Entry</h1>

            <asp:HiddenField ID="hfPatientID" runat="server" />

            <div class="grid grid-cols-2 gap-4">
                <!-- Date Field -->
                <div>
                    <label for="txtDate" class="block text-sm font-medium text-gray-600">Date</label>
                    <asp:TextBox ID="txtDate" runat="server" TextMode="Date" CssClass="form-input" ReadOnly="true"></asp:TextBox>
                </div>

                <!-- Patient Name -->
                <div>
                    <label for="txtPatientName" class="block text-sm font-medium text-gray-600">Patient Name</label>
                    <asp:TextBox ID="txtPatientName" runat="server" CssClass="form-input"></asp:TextBox>
                </div>

                <!-- Mobile Number -->
                <div>
                    <label for="txtMobileNumber" class="block text-sm font-medium text-gray-600">Mobile Number</label>
                    <asp:TextBox ID="txtMobileNumber" runat="server" CssClass="form-input" MaxLength="10"></asp:TextBox>
                </div>

                <!-- Patient Address -->
                <div>
                    <label for="txtPatientAddress" class="block text-sm font-medium text-gray-600">Patient Address</label>
                    <asp:TextBox ID="txtPatientAddress" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-input"></asp:TextBox>
                </div>

                <!-- Drug Name -->
                <div>
                  <label for="ddlDrugName" class="block text-sm font-medium text-gray-600">Drug Name</label>
                  <asp:DropDownList ID="ddlDrugName" runat="server" AutoPostBack="true" 
                      CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                      OnSelectedIndexChanged="ddlDrugName_SelectedIndexChanged">
                  </asp:DropDownList>
              </div>

              <div>
                  <label for="ddlCategory" class="block text-sm font-medium text-gray-600">Category</label>
                  <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="true" 
                      OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                      CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm">
                  </asp:DropDownList>
              </div>

                 <!-- Total Quantity-->
                  <div>
                      <label for="txtQuantity" class="block text-sm font-medium text-gray-600">Total Quantity</label>
                      <asp:TextBox ID="txtQuantity" runat="server" CssClass="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-lg shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm" ReadOnly="true"></asp:TextBox>
                      <span id="TotalQuantityError" class="text-red-500" style="display: none;">Drug is currently out of stock!</span>
                  </div>

                <!-- Quantity Sold -->
                <div>
                    <label for="txtQuantitySold" class="block text-sm font-medium text-gray-600">Quantity Sold</label>
                    <asp:TextBox ID="txtQuantitySold" runat="server" CssClass="form-input" MaxLength="3"></asp:TextBox>
                </div>

                <!-- Prescribed By -->
                <div>
                    <label for="txtPrescribedBy" class="block text-sm font-medium text-gray-600">Prescribed By</label>
                    <asp:TextBox ID="txtPrescribedBy" runat="server" CssClass="form-input"></asp:TextBox>
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
                </div>
            </div>

            <div class="mt-6 text-center">
                <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="bg-blue-500 text-white px-6 py-2 rounded shadow hover:bg-blue-600" OnClick="btnUpdate_Click" />
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


</asp:Content>

