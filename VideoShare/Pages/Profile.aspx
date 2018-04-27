<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Master.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="VideoShare.Pages.Profile" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <style>
        td {
            vertical-align: top;
        }

        a {
            font-size: large;
        }

        p {
            margin-top: 3px;
            margin-bottom: 5px;
            font-size: large;
        }

        .profilePanel {
            background-color: #434343;
            margin: 10px;
        }

        .profileText {
            font-size: large;
            padding-bottom: 15px;
        }
    </style>
    <script>
        function upload()
        {
            var title = document.getElementById('vTitle').value;
            if (title == null || title.length == 0)
            {
                return;
            }

            var desc = document.getElementById('vDesc').value;
            if (desc == null || desc.length == 0)
            {
                return;
            }

            var url = "Services.aspx?f=upload&t=" + title + "&d=" + desc + "&c=";

            var cat = false;
            var catDiv = document.getElementById('catList');
            for (var ix = 0; ix < catDiv.children.length; ix++)
            {
                var v = catDiv.children[ix];

                if (v.tagName.toLowerCase() == "input")
                {
                    if (v.checked)
                    {
                        cat = true;
                        url += v.id + ",";
                    }
                }
            }

            if (!cat) {
                return;
            }

            url = encodeURI(url);

            var request = new XMLHttpRequest();
            request.onload = function () {
                if (this.readyState == 4) {
                    if (this.responseText == "ok") {
                        window.location.reload();
                    } else {
                        alert(this.responseText);
                    }
                }
            }

            request.open("POST", url, true);
            request.send(null);
        }
    </script>
    <asp:Literal runat="server" ID="ExtraScript"></asp:Literal>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="body" runat="server">
    <div style="width: 70%; margin-left: auto; margin-right: auto;">
        <table style="width: 100%;">
            <tr style="height: 400px">
                <td style="width: 70%">
                    <div class="profilePanel">
                        <asp:Literal runat="server" ID="LatestVideoBox"></asp:Literal>
                    </div>
                </td>
                <td style="width: 30%">
                    <div class="profilePanel" style="padding: 10px;">
                        <div style="width: 100%; text-align: center; padding-bottom: 10px">
                            <a style="font-size: x-large;"><asp:Literal ID="Profile_UserName" runat="server"></asp:Literal></a><br />
                        </div>
                        <a class="profileText">Csatlakozott: <asp:Literal ID="Profile_JoinDate" runat="server"></asp:Literal></a><br />
                        <a class="profileText">Videók: <asp:Literal ID="Profile_VideoCount" runat="server"></asp:Literal></a><br />
                        <a class="profileText">Nézettség: <asp:Literal ID="Profile_ViewCount" runat="server"></asp:Literal></a><br />
                        <a class="profileText">Leírás: <asp:Literal ID="Profile_Desc" runat="server"></asp:Literal></a><br />
                    </div>
                    <div id="userPanel" class="profilePanel" style="padding-top: 20px; padding-bottom: 20px; visibility: hidden">
                        <div id='commentButton' style='width: 90%; margin-left: auto; margin-right: auto; background-color: #323232; text-align: center; cursor: pointer; border-radius: 5px' onclick="document.getElementById('newVideo').style.removeProperty('visibility');">
                            <a style='padding-top: 5px; padding-bottom: 5px; color: #A0A0A0;'>Új videó feltöltése</a>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Literal runat="server" ID="VideoList"></asp:Literal>
                </td>
            </tr>
        </table>
    </div>
    <div id="newVideo" style="visibility: hidden;">
        <div style="position: absolute; left: 0; top: 0; background-color: black; opacity: 0.5; width: 100%; height: 100%;"></div>
        <div style="position: absolute; left: 0; top: 0; width: 100%; height: 100%">
            <div style="width: 50%; margin-left: auto; margin-right: auto; background-color: #434343; border-radius: 5px; margin-top: 100px; opacity: 1; padding: 5px; color: white; text-align: center">
                <a>Új videó feltöltése</a>
                <table style="width: 100%">
                    <tr>
                        <td>
                            <div style="background-color: #131313; padding: 10px; border-radius: 5px; width: 75%; margin-left: auto; margin-right: auto;">
                                <input id="vTitle" type="text" class="hiddenInput" style="width: 100%" placeholder="Cím" required="required" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="background-color: #131313; padding: 10px; border-radius: 5px; width: 75%; margin-left: auto; margin-right: auto;">
                                <textarea id="vDesc" class="hiddenInput" style="width: 100%; resize: none" rows="5" placeholder="Leírás" required="required"></textarea>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="background-color: #131313; padding: 10px; border-radius: 5px; width: 75%; margin-left: auto; margin-right: auto;">
                                <input type="file" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="padding: 10px; border-radius: 5px; width: 75%; margin-left: auto; margin-right: auto;">
                                <a>Kategória: </a>
                                <asp:Literal runat="server" ID="CategoryList"></asp:Literal>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; padding-top: 10px;">
                            <div onclick="upload()" id="uploadBtn" style="padding: 10px; background-color: #131313; cursor:pointer; width: 50%; margin-left: auto; margin-right: auto; border-radius: 5px">
                                <a>Feltöltés</a>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
