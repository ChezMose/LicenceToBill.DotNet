<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Demo.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ContentPlaceHolderID="Zone1" runat="server">

    <div class="payment">


        <h2>Bank card updated</h2>
        <br />
        <br />

        <p>Your subscription has been updated with success</p>
        <p>thanks for choosing <strong><%= Constants.NameBusiness %></strong></p>

    </div>

</asp:Content>
