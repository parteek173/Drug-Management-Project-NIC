<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="FrontEnd_Default2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title><%= System.Configuration.ConfigurationManager.AppSettings["ProjectName"] %></title>
    <link rel="stylesheet" href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css" />
    <link href="https://cdn.jsdelivr.net/npm/flowbite@2.5.2/dist/flowbite.min.css" rel="stylesheet" />
    <script src="https://cdn.tailwindcss.com"></script>
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/flowbite@2.5.2/dist/flowbite.min.js"></script>
    <link href="css/CustomDesign.css" rel="stylesheet" />
    <link rel="apple-touch-icon" sizes="180x180" href="../Assets/apple-touch-icon.png" />
    <link rel="icon" type="image/png" sizes="32x32" href="../Assets/favicon-32x32.png" />
    <link rel="icon" type="image/png" sizes="16x16" href="../Assets/favicon-16x16.png"/>
    <link rel="manifest" href="../Assets/site.webmanifest" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/js/select2.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/select2@4.0.13/dist/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/select2@4.0.13/dist/js/select2.min.js"></script>
    <link href="fonts/font-awesome/css/font-awesome.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class=" bg-white-100 py-4 shadow-lg border-b border-blue-300">
            <div class="container mx-auto flex items-center justify-between px-6">
                <!-- Left Section (Logo & Title) -->
                    <div class="flex items-center space-x-4 w-full">
                        <!-- Logo -->
                        <img src="/Assets/chd-logo.png" class="h-20" alt="Chandigarh Administration Logo" />
                        <!-- Title Section -->
                        <div class="flex flex-col inr-dash">
                            <h1 class="text-2xl font-bold text-blue-800 tracking-wide com-blue"><%=System.Configuration.ConfigurationManager.AppSettings["ProjectTitle"] %></h1>
                            <p class="text-gray-700 text-sm font-medium">Health Department, Chandigarh Administration</p>
                        </div>
                    </div>
                 <img src="/Assets/emblem.png"  width="80" alt="Emblem" />
            </div>
        </div>


        <div class="container mx-auto mt-10 px-6 max-w-5xl">
    <div class="grid grid-cols-1 md:grid-cols-[1.5fr_2fr] gap-10 items-stretch justify-center">
        <!-- Left Column: Login Section -->
        <div class="bg-white shadow-lg rounded-lg p-6 border border-gray-200 w-full min-w-[420px] h-full flex flex-col">
            <h2 class="text-2xl font-semibold text-gray-800 mb-4 border-b pb-2">What's New / Latest Notifications</h2>
            <div id="notification-list" class="overflow-y-auto h-80 space-y-4 pr-3 scrollbar-thin scrollbar-thumb-gray-300 flex-grow">
                <asp:Repeater ID="rptNotifications" runat="server">
                    <ItemTemplate>
                        <div class="p-5 bg-gray-50 rounded-lg shadow-sm hover:bg-gray-100 transition duration-300">
                            <div class="font-bold text-blue-600 text-lg">
                                <%# GetTitleWithLink(Eval("PdfFilePath"), Eval("Title")) %>
                            </div>
                            <p class="text-gray-700 mt-2 text-base">
                                <%# Eval("Message") %>
                            </p>
                            <div class="text-gray-500 text-sm mt-3">
                                <span class="font-medium">Published:</span>
                                <%# Convert.ToDateTime(Eval("CreatedAt")).ToString("dd-MM-yyyy") %>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>

        <!-- Right Column: Notification Section -->
        <div class="bg-white px-8 py-10 rounded-lg shadow-lg min-w-[380px] w-full h-full flex flex-col">
            <!-- Page Header -->
            <div class="flex items-center space-x-4 mb-6">
                <img src="/Assets/chd-logo.png" width="80" alt="Chandigarh Administration Logo" />
                <h1 class="text-2xl font-bold text-gray-800">
                    <%=System.Configuration.ConfigurationManager.AppSettings["ProjectName"] %>
                </h1>
            </div>

            <asp:Panel ID="firstpanel" runat="server">
                <h2 class="text-xl font-semibold text-gray-700 text-center">Login</h2>
                <p class="text-center text-gray-600 mt-2">
                    Enter your mobile number to receive an OTP and log in to your account.
                </p>
            </asp:Panel>

            <asp:Label ID="lblMessage" runat="server" CssClass="block text-center text-red-500 font-semibold mb-4"></asp:Label>

            <asp:Panel ID="MobilePanel" runat="server" class="flex-grow">
                <div class="mb-4">
                    <label class="block text-gray-700 font-medium mb-2">Mobile Number</label>
                    <asp:TextBox ID="txtMobileNumber" MaxLength="10" runat="server"
                        CssClass="w-full px-4 py-3 border border-gray-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                        Placeholder="Enter your mobile number">
                    </asp:TextBox>
                </div>
                <div class="text-center">
                    <asp:Button ID="btnSendOTP" runat="server" Text="Send OTP"
                        CssClass="bg-blue-700 text-white font-medium px-6 py-2 rounded-lg hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-400 transition-transform transform hover:scale-105"
                        OnClick="btnSendOTP_Click" />
                </div>
            </asp:Panel>

            <asp:Panel ID="OTPPanel" runat="server" Visible="false" class="flex-grow">
                <div class="mb-4">
                    <label class="block text-gray-700 font-medium mb-2">Enter OTP</label>
                    <asp:TextBox ID="txtOTP" runat="server"
                        CssClass="w-full px-4 py-3 border border-gray-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                        Placeholder="Enter OTP">
                    </asp:TextBox>
                </div>
                <div class="text-center">
                    <asp:Button ID="btnLogin" runat="server" Text="Login"
                        CssClass="bg-green-700 text-white font-medium px-6 py-2 rounded-lg hover:bg-green-600 focus:outline-none focus:ring-2 focus:ring-green-400 transition-transform transform hover:scale-105"
                        OnClick="btnLogin_Click" />
                </div>
            </asp:Panel>

            <!-- Already Logged In Message -->
            <asp:Panel ID="AlreadyLogin" runat="server" Visible="false">
                <div class="text-center bg-gray-50 p-6 rounded-lg shadow-lg mt-6">
                    <label class="block text-gray-800 font-medium mb-4 text-lg">
                        You Are Already Logged In
                    </label> 
                    <asp:LinkButton ID="LinkButton1" ValidationGroup="logout" runat="server" 
                        CssClass="bg-red-600 text-white font-medium px-6 py-2 rounded-lg hover:bg-red-500 focus:outline-none focus:ring-2 focus:ring-red-400 transition-transform transform hover:scale-105" 
                        OnClick="LinkButton1_Click">Logout</asp:LinkButton>


                    <a href="Dashboard.aspx"  class="bg-blue-600 text-white font-medium px-6 py-2 rounded-lg hover:bg-red-500 focus:outline-none focus:ring-2 focus:ring-red-400 transition-transform transform hover:scale-105"
                        >Dashboard</a>

                </div>
            </asp:Panel>
        </div>

        
        
    </div>
</div>


            
            <br /><br />

            <!-- jQuery for Scrolling Effect -->
            <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
            <script>
                $(document).ready(function () {
                    function slideNotifications() {
                        var list = $("#notification-list");
                        var first = list.children().first();

                        first.animate({ marginTop: "-3rem" }, 1000, function () {
                            $(this).appendTo(list).css("margin-top", "0");
                        });
                    }

                    setInterval(slideNotifications, 3000);
                });
            </script>



        <footer class="bg-blue-600 text-white py-2 bg-com-blue">
            <div class="container mx-auto text-center">
                <p>&copy; 2025 <%= System.Configuration.ConfigurationManager.AppSettings["ProjectName"] %>. All rights reserved.</p>
                <!--<nav class="flex justify-center space-x-6 ">
                    <a href="#" class="hover:underline">Privacy Policy</a>
                    <a href="#" class="hover:underline">Terms of Service</a>
                    <a href="#" class="hover:underline">Contact Us</a>
                </nav>-->
            </div>
        </footer>

    </form>
</body>
</html>
