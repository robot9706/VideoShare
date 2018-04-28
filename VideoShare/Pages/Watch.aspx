<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Master.Master" AutoEventWireup="true" CodeBehind="Watch.aspx.cs" Inherits="VideoShare.Pages.Watch" %>
<asp:Content ID="Head" ContentPlaceHolderID="head" runat="server">
    <style>
        td {
            vertical-align: top;
        }

        .panel {
            background-color: #434343;
            padding: 10px;
            border-radius: 5px;
        }

        a {
            font-size: large;
        }

        p {
            margin-top: 3px;
            margin-bottom: 5px;
            font-size: large;
        }

        .text {
            font-size: large;
            padding-bottom: 15px;
        }
    </style>
    <asp:Literal runat="server" ID="ExtraScript"></asp:Literal>
</asp:Content>
<asp:Content ID="Body" ContentPlaceHolderID="body" runat="server">
    <div style="width: 850px; margin-left: auto; margin-right: auto;">
       <table style="width: 100%">
           <tr style="height: 400px">
               <td>
                   <div class="panel" style="width: 100%;">
                       <asp:Literal runat="server" ID="VideoView"></asp:Literal>
                   </div>
               </td>
           </tr>
           <asp:Literal runat="server" ID="CommentBox"></asp:Literal>
       </table>
    </div>
    <div id="listAdd" style="visibility: hidden;">
        <div style="position: fixed; left: 0; top: 0; background-color: black; opacity: 0.5; width: 100%; height: 100%;"></div>
        <div style="position: fixed; left: 0; top: 0; width: 100%; height: 100%">
            <div style="width: 35%; margin-left: auto; margin-right: auto; background-color: #434343; border-radius: 5px; margin-top: 100px; opacity: 1; padding: 5px; color: white; text-align: center">
                <a>Hozzáadás listához</a>
                <table style="width: 100%">
                    <asp:Literal runat="server" ID="PlaylistBox"></asp:Literal>
                    <tr>
                        <td style="text-align: center; padding-top: 10px;">
                            <div onclick="document.getElementById('listAdd').style.visibility='hidden';" style="padding: 10px; background-color: #131313; cursor:pointer; width: 25%; margin-left: auto; margin-right: auto; border-radius: 5px">
                                <a>Mégse</a>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
