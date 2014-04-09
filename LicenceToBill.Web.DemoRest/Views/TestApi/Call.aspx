<%@ Page Language="C#" MasterPageFile="~/Views/Shared/TestApi.Master" Inherits="System.Web.Mvc.ViewPage<ModelTestApiCall>" %>

<asp:Content ContentPlaceHolderID="Css" runat="server">
    <style>
        #main #zone1
        {
        	width:340px;
        }
        #main #zone2
        {
        	width:600px;
        }
        #main #zone2 img
        {
        	float:right;
        }
    </style>

</asp:Content>

<asp:Content ContentPlaceHolderID="Zone1" runat="server">

    <div class="demo">

        <%= Html.Partial("Ressources") %>

    </div>

</asp:Content>

<asp:Content ContentPlaceHolderID="Zone2" runat="server">

    <%  // get the data
        var model = this.Model;
    %>

    <h3><a target="_blank" href="<%= model.UrlRessource %>"><%= model.UrlRessource %></a></h3>
    <hr />
    <%  // loop on the methods
        foreach(var method in model.Methods)
        {
    %>
            <button class="button button-call call-filter" method="<%= method %>"><%= method %></button>
    <%  }
    %>

    <div class="call-filter">
        <%  // if we have users
            if(model.KeyUsers != null)
            {
        %>
                <%= Html.DropDownList("keyUser", new SelectList(model.KeyUsers, "Item2", "Item1")) %>
        <%  }
        %>
        <%  // if we have features
            if(model.KeyFeatures != null)
            {
        %>
                <%= Html.DropDownList("keyFeature", new SelectList(model.KeyFeatures, "Item2", "Item1")) %>
        <%  }
        %>
        <%  // if we have offers
            if(model.KeyOffers != null)
            {
        %>
                <%= Html.DropDownList("keyOffer", new SelectList(model.KeyOffers, "Item2", "Item1")) %>
        <%  }
        %>
        <%  // if we have languages
            if(model.Lcids != null)
            {
        %>
                <%= Html.DropDownList("lcid", new SelectList(model.Lcids, "Item2", "Item1")) %>
        <%  }
        %>
    </div>

    <div id="call_result">
    </div>

    <script type="text/javascript">
        $(function () {
            $('.button-call').click(function () {
                // get the button
                var button = $(this);
                // disable it to prevent flooding
                button.attr('disabled', 'disabled');
                // perform the ajax call
                $.ajax
                ({  type: 'post',
                    data:
                    {
                        method: button.attr('method'),
                        keyUser: $('#keyUser').val(),
                        keyFeature: $('#keyFeature').val(),
                        keyOffer: $('#keyOffer').val(),
                        lcid: $('#lcid').val()
                    },
                    success: function (data) {
                        // set result
                        var container = $('#call_result').replaceWith(data);
                        // enable the button
                        button.removeAttr('disabled');
                    }
                });
            });
        });
    </script>

</asp:Content>

<asp:Content ContentPlaceHolderID="Zone3" runat="server">

    <div id="code_result" class="api-result" style="display:none;">

        <h4>Returned response body</h4>
        <div class="result-code">
            <pre id="code_result_string"></pre>
        </div>

    </div>

</asp:Content>
