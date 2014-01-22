<%@ Page Language="C#" MasterPageFile="~/Views/Shared/TestApi.Master" Inherits="System.Web.Mvc.ViewPage<ModelTestApiOverload>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Zone1" runat="server">

    <%  // get the data
        var model = this.Model;
    %>

    <form method="post">
        <input type="text" name="urlHost" value="<%= model.UrlHost %>" style="width:100%;" />
        <br />
        <br />
        <%= Html.DropDownList("typeTest", new SelectList(Enum.GetValues(typeof(ModelTestApiOverload.TypeTest)))) %>
        <br />
        <br />
        <button>Go</button>
    </form>

    <%  if(!string.IsNullOrEmpty(model.Message))
        {
    %>
        <br />
            <div style="width:100%;background:#efefef;border:1px #d8d8d8 solid;" >
                <%= model.Message %>
            </div>
    <%  }
    %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Zone2" runat="server">

    <%  // get the data
        var model = this.Model;
    %>
    
    <%  if(model.Calls != null)
        {
    %>
            <%= model.Calls.Count %> calls in <%= model.TimeTotal %> seconds

            <br />
            <br />
            <a href="/TestApi/OverloadCsv">Télecharger le CSV</a>
    <%  }
    %>


</asp:Content>
