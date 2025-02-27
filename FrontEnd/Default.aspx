<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="FrontEnd_Default" %>



<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

  <meta charset="UTF-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0" />
  <title><%= System.Configuration.ConfigurationManager.AppSettings["ProjectName"] %> - Login with OTP</title>
  <script src="https://cdn.tailwindcss.com"></script>
    <link href="../css/CustomDesign.css" rel="stylesheet" />

</head>
<body class="bg-gradient-to-r from-blue-500 via-indigo-500 to-purple-500 min-h-screen flex items-center justify-center login-bg-color">
  <form id="form1" runat="server" class="w-full max-w-md">
    <div class="bg-white px-4 py-6 rounded-lg shadow-lg">
      <!-- Page Header -->
        <div class="logo-bx ">
             <img src="/Assets/chd-logo.png" width="100" alt="Chandigarh Administration Logo" class="mr-3" />
      <h1 class="text-3xl font-bold text-gray-800"><%=System.Configuration.ConfigurationManager.AppSettings["ProjectName"] %></h1>
                      </h1>
 </div>
        <h2>Login</h2>
            <p class="text-center  ">
         Enter your mobile number to receive an OTP and log in to your account.
      </p>

      <!-- Error Message -->
      <asp:Label ID="lblMessage" runat="server" CssClass="block text-center text-red-500 font-semibold mb-4"></asp:Label>

      <!-- Mobile Number Input -->
      <asp:Panel ID="MobilePanel" runat="server">
        <div class="mb-1">
          <label for="mobileNumber" class="block text-gray-800 font-medium mb-2">Mobile Number</label>
          <asp:TextBox
            ID="txtMobileNumber"
            MaxLength="10"
            
            runat="server"
            CssClass="w-full px-4 py-3 border border-gray-500 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            Placeholder="Enter your mobile number">
          </asp:TextBox>
          <asp:RequiredFieldValidator
            ID="rfvMobileNumber"
            runat="server"
            ControlToValidate="txtMobileNumber"
            ErrorMessage="Mobile number is required."
            CssClass="text-red-800 text-sm">
          </asp:RequiredFieldValidator>
            <br />
            <asp:RegularExpressionValidator
                ID="revMobileNumber"
                runat="server"
                ControlToValidate="txtMobileNumber"
                ValidationExpression="^[0-9]{10}$"
                ErrorMessage="Please enter a valid 10-digit mobile number."
                CssClass="text-red-800 text-sm">
            </asp:RegularExpressionValidator>
        </div>

        <div class="text-center">
          <asp:Button
            ID="btnSendOTP"
            runat="server"
            Text="Send OTP"
            CssClass="bg-blue-800 text-white font-medium px-6 py-2 rounded-lg hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-400 shadow-md transition-transform transform hover:scale-105"
            OnClick="btnSendOTP_Click" />
        </div>
      </asp:Panel>

      <!-- OTP Verification Input -->
      <asp:Panel ID="OTPPanel" runat="server" Visible="false">
        <div class="mb-6">
          <label for="otp" class="block text-gray-800 font-medium mb-2">Enter OTP</label>
          <asp:TextBox
            ID="txtOTP"
            runat="server"
            CssClass="w-full px-4 py-3 border border-gray-500 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            Placeholder="Enter OTP">
          </asp:TextBox>
        </div>

        <div class="text-center">
          <asp:Button
            ID="btnLogin"
            runat="server"
            Text="Login"
            CssClass="bg-green-800 text-white font-medium px-6 py-2 rounded-lg hover:bg-green-600 focus:outline-none focus:ring-2 focus:ring-green-400 shadow-md transition-transform transform hover:scale-105"
            OnClick="btnLogin_Click" />
        </div>
      </asp:Panel>
    </div>

  </form>
</body>
</html>
