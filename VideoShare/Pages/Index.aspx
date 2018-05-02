<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Master.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="VideoShare.Pages.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .panel {
            background-color: #434343;
            margin: 10px;
            border-radius: 5px;
        }

        .panelText {
            font-size: large;
            padding-bottom: 15px;
        }
    </style>
    <script>
        function categoryViewReload()
        {
            var catFilter = document.getElementById('cat_catFilter').value;
            var timeFilter = document.getElementById('cat_timeFilter').value;

            var url = "Services.aspx?f=catview&c=" + catFilter + "&t=" + timeFilter;

            url = encodeURI(url);

            var request = new XMLHttpRequest();
            request.onload = function () {
                if (this.readyState == 4) {
                    document.getElementById('categoryVideoView').innerHTML = this.responseText;
                }
            }

            request.open("POST", url, true);
            request.send(null);
        }

        function dateViewReload()
        {
            var timeFilter = document.getElementById('date_filter').value;

            var url = "Services.aspx?f=dateview&t=" + timeFilter;

            url = encodeURI(url);

            var request = new XMLHttpRequest();
            request.onload = function () {
                if (this.readyState == 4) {
                    document.getElementById('dateVideoView').innerHTML = this.responseText;
                }
            }

            request.open("POST", url, true);
            request.send(null);
        }

        var onl = window.onload;
        window.onload=function()
        {
            if (onl != null)
                onl();

            categoryViewReload();
            dateViewReload();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div style="width: 80%; margin-left: auto; margin-right: auto;">
        <asp:Literal runat="server" ID="ContentHTML"></asp:Literal>
        <div class='panel' style='padding: 10px'>
            <table style="width: 100%">
                <tr>
                    <td>
                        <a class='panelText'>Legnézetebb és legújabb videók:</a>
                    </td>
                    <td>
                        <a style="padding-left: 10px">Kategória: </a>
                        <select id="cat_catFilter" onchange="categoryViewReload()">
                            <option value="all">Mind</option>
                            <asp:Literal runat="server" ID="CategorySelect"></asp:Literal>
                        </select>
                    </td>
                    <td>
                        <a style="padding-left: 10px">Szűrés: </a>
                        <select id="cat_timeFilter" onchange="categoryViewReload()">
                            <option value="Time">Feltöltés dátuma</option>
                            <option value="Views">Nézettség</option>
                        </select>
                    </td>
                </tr>
            </table>
            <div id="categoryVideoView" style="width: 100%">
            </div>
        </div>
         <div class='panel' style='padding: 10px'>
            <table style="width: 100%">
                <tr>
                    <td>
                        <a style="padding-left: 10px">Legnépszerűbb videók </a>
                        <select id="date_filter" onchange="dateViewReload()">
                            <option value="Day">napi</option>
                            <option value="Week">heti</option>
                            <option value="Month">havi</option>
                        </select>
                        <a style="padding-left: 10px">bontásban</a>
                    </td>
                </tr>
            </table>
            <div id="dateVideoView" style="width: 100%">
            </div>
        </div>
    </div>
</asp:Content>
