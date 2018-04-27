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
</asp:Content>
