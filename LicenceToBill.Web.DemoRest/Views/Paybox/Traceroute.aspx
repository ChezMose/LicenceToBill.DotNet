<%@ Page Language="C#" MasterPageFile="~/Views/Shared/TestApi.Master" Inherits="System.Web.Mvc.ViewPage<ModelPayboxTraceroute>" %>

<asp:Content ContentPlaceHolderID="Css" runat="server">

</asp:Content>

<asp:Content ContentPlaceHolderID="Zone1" runat="server">

    <div class="demo">

        <form method="post">

            <input type="text" style="width:100%;" name="ipOrHost" value="<%= this.Model.IpOrHost %>" placeholder="IP address or hostname" />
            <br />
            <br />
            <input type="text" style="width:100%;" name="timeout" value="<%= this.Model.Timeout %>" placeholder="Timeout (default is 600 sec)" />
            <br />
            <br />
            <input type="text" style="width:100%;" name="maxHops" value="<%= this.Model.MaxHops %>" placeholder="Max hops (default is 30)" />
            <br />
            <br />
            <button>Submit</button>
            
        </form>

    </div>

</asp:Content>

<asp:Content ContentPlaceHolderID="Zone2" runat="server">

    <% // if we got a result
       if((this.Model.Status != null)
           && (this.Model.Status.Count > 0))
       {
    %>
        <ul>
            <%  // loop
                foreach(var status in this.Model.Status)
                {
            %>
                    <li><%= status %></li>
            <%  } %>
        </ul>
    <% } %>

</asp:Content>
