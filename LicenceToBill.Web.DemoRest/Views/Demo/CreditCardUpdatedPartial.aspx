<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Demo.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ContentPlaceHolderID="Zone1" runat="server">

    <div class="payment">


        <h2>Bank card updated partial success</h2>
        <br />
        <br />

        <p>Your bank card has been updated</p>
        <p>However, the system encountered a problem while restarting one of your subscription </p>
        <p>Please contact an administrator to know more.</p>
        <p>thanks for choosing <strong><%= Constants.NameBusiness %></strong></p>

    </div>

</asp:Content>
