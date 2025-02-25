<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Whatisnew.aspx.cs" Inherits="FrontEnd_Whatisnew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

   <div class="container mx-auto mt-10">
    <!-- Notification Scrolling Container with Increased Width -->
    <div class="relative w-[900px] h-52 overflow-hidden bg-white shadow-lg rounded-lg p-4 mx-auto">
        <div id="notification-list" class="absolute w-full space-y-2">
            <asp:Repeater ID="rptNotifications" runat="server">
                <ItemTemplate>
                    <div class="views-row border-b border-gray-300 py-2">
                        <!-- Title Field -->
                        <div class="views-field views-field-title">
                            <strong class="field-content">
                                <a href='<%# Eval("PdfFilePath") %>' class="text-blue-600 hover:underline" hreflang="en">
                                    <%# Eval("Title") %>
                                </a>
                            </strong>

                            
                        
                        </div>
                        <div>
                             <%# Eval("Message") %>
                        </div>
                        <!-- Date Field -->
                        <div class="views-field views-field-created text-gray-500 text-sm">
                            <span class="field-content">
                                <%# Convert.ToDateTime(Eval("CreatedAt")).ToString("dd-MM-yyyy") %>
                            </span>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>



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

