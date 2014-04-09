<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage" %>

<asp:Content ContentPlaceHolderID="Zone1" runat="server">

    <div class="not-implemented">

        <h2>Not implemented yet</h2>
        <br />
        <p>Reminder : you are on a sur un <strong>fake site</strong> whose purpose is to demonstrate the capacities of <a href="<%= Constants.UrlLicenceToBill %>"><strong>LicenceToBill</strong></a>.</p>
        <br />
        <p>If accidentally ended up here, please go to <a href="<%= Constants.UrlLicenceToBill %>">http://licencetobill.com</a> to know more.</p>
        <br />
        <p>If you did not, please contact your favorite administrator.</p>
        <br />
        <span id="btn-login" class="button-big">
            <a href="/">Back to home</a>
        </span>

    </div>

</asp:Content>

