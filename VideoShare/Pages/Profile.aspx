<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Master.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="VideoShare.Pages.Profile" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <style>
        td {
            vertical-align: top;
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
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="body" runat="server">
    <div style="width: 70%; margin-left: auto; margin-right: auto;">
        <table style="width: 100%;">
            <tr style="height: 400px">
                <td style="width: 70%">
                    <div class="profilePanel">
                        <div style="width: 800px; margin-left: auto; margin-right: auto; padding-bottom: 10px">
                            <div style="background-color: #323232">
                                <asp:Literal runat="server" ID="LatestVideoBox"></asp:Literal>
                            </div>
                            <asp:Literal runat="server" ID="LatestVideoTitleBox"></asp:Literal>
                        </div>
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
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Literal runat="server" ID="VideoList"></asp:Literal>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
