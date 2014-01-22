<%@ Page Language="C#" MasterPageFile="~/Views/Shared/TestApi.Master" Inherits="System.Web.Mvc.ViewPage<ModelPayboxTest>" %>

<asp:Content ContentPlaceHolderID="Css" runat="server">

</asp:Content>

<asp:Content ContentPlaceHolderID="Zone1" runat="server">

    <div class="demo">

        <form method="post">

            <%= Html.DropDownList("type", this.Model.Types) %>
            <br />
            <br />
            <textarea name="bodyRequest" style="width:100%;height:250px;"><%= this.Model.BodyRequest %></textarea>
            <br />
            <br />
            <button>Submit</button>
            
        </form>

    </div>

</asp:Content>

<asp:Content ContentPlaceHolderID="Zone2" runat="server">

    <% // if we got a result
       if(!string.IsNullOrEmpty(this.Model.BodyResponse))
       {
    %>
            <h4>
            <% // if we got a status
               if(this.Model.Status.HasValue)
               {
            %>
                    HTTP <%= (int) this.Model.Status.Value %> (<%= this.Model.Status.ToString() %>)
            <% } %>

            in <%= this.Model.ElapsedMilliseconds %>ms</h4>
            via IP <strong><%= Model.IpOutgoing %></strong>
            <br />
            <div style="word-wrap:break-word;background:#ffffff;border:1px solid #d8d8d8; width:100%;min-height:250px;"><%= this.Model.BodyResponse %></div>
            <br />
            <h4>sent to <%= this.Model.Url %></h4>
    <% } %>

</asp:Content>
