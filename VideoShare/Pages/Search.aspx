<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Master.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="VideoShare.Pages.Search" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
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
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="body" runat="server">
     <div style="width: 70%; margin-left: auto; margin-right: auto;">
        <asp:Literal runat="server" ID="ContentHTML"></asp:Literal>
    </div>
</asp:Content>
