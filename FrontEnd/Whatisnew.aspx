<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Whatisnew.aspx.cs" Inherits="FrontEnd_Whatisnew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <div class="container mx-auto mt-10">
            <div class="relative w-80 h-48 overflow-hidden bg-white shadow-lg rounded-lg p-4">
                <div id="notification-list" class="absolute w-full space-y-2">
                    <asp:Repeater ID="rptNotifications" runat="server">
                        <ItemTemplate>
                            <div class="bg-blue-500 text-white p-3 rounded shadow-md">
                                <strong><%# Eval("Title") %></strong>
                                <p class="text-sm"><%# Eval("Message") %></p>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>


    <!-- jQuery for Sliding Effect -->
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

