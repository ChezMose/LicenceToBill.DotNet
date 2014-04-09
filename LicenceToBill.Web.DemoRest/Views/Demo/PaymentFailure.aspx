<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Demo.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ContentPlaceHolderID="Zone1" runat="server">

    <div class="payment">

        <h2>Failure</h2>
        <br />
        <br />

        <p>Your subscription could not end with success</p>
        <p>Please try again later</p>
        <p>Thanks for choosing <strong><%= Constants.NameBusiness %></strong></p>

    </div>

</asp:Content>
