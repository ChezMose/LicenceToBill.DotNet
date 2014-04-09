<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<ModelLogin>" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="Zone1" runat="server">

    <div class="login">

        <form method="post">
            <fieldset>
        
                
                <div>
                    <%= this.ValidationSummary() %>&nbsp;
                </div>
                <ul>
                    <li><input type="text" name="login" value="<%= this.Model.Login %>" /></li>
                    <li><input type="password" name="password" value="<%= this.Model.Password %>" /></li>
                </ul>
                <div class="spacer"></div>

                <div class="buttons">
                    <button class="button-big">
                        Ok
                    </button>
                </div>

            </fieldset>
        </form>
    </div>

</asp:Content>
