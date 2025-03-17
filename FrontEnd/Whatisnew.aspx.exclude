<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Whatisnew.aspx.cs" Inherits="FrontEnd_Whatisnew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

  <div class="container mx-auto mt-10">
    <!-- Notification Scrolling Container with Increased Width & Height -->
    <div class="relative max-w-5xl h-96 overflow-hidden bg-white shadow-lg rounded-lg p-6 mx-auto border border-gray-200">
        <h2 class="text-2xl font-semibold text-gray-800 mb-4 border-b pb-2">Latest Notifications</h2>

        <div id="notification-list" class="overflow-y-auto h-80 space-y-4 pr-3 scrollbar-thin scrollbar-thumb-gray-300">
            <asp:Repeater ID="rptNotifications" runat="server">
                <ItemTemplate>
                    <div class="p-5 bg-gray-50 rounded-lg shadow-sm hover:bg-gray-100 transition duration-300">
                        <!-- Title Field -->
                        <div class="font-bold text-blue-600 text-lg">
                            <%# GetTitleWithLink(Eval("PdfFilePath"), Eval("Title")) %>
                        </div>

                        <!-- Message Field -->
                        <p class="text-gray-700 mt-2 text-base">
                            <%# Eval("Message") %>
                        </p>

                        <!-- Date Field -->
                        <div class="text-gray-500 text-sm mt-3">
                            <span class="font-medium">Published:</span>
                            <%# Convert.ToDateTime(Eval("CreatedAt")).ToString("dd-MM-yyyy") %>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
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

</asp:Content>

