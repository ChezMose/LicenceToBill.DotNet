<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Demo.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ContentPlaceHolderID="Zone1" runat="server">

    <div class="payment">

        <h2>Cancelled</h2>
        <br />
        <br />

        <p>You cancelled your subcription request</p>
        <p>Too bad for <strong><%= Constants.NameBusiness %></strong> !</p>

    </div>

</asp:Content>
