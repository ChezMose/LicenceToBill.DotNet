<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ContentPlaceHolderID="Zone1" runat="server">

    <div class="home">
        <h2>This is an innovative service</h2>
        <br />
        <p>This is the home page of an innovative service that decided to use <a href="<%= Constants.UrlLicenceToBill %>"><strong>LicenceToBill</strong></a> to manage its subscriptions.</p>
        <br />
        <p>It's a fake site whose sole purpose is to demonstrate the capacities of <a href="<%= Constants.UrlLicenceToBill %>"><strong>LicenceToBill</strong></a> when it's integrated.</p>
        <br />
        <br />
        <span id="btn-login" class="button-big">
            <a href="/Home/NotImplemented">Try it now</a>
        </span>
        &nbsp;
        &nbsp;
        &nbsp;
        <span id="btn-offer" class="button-big">
            <a href="<%= Constants.UrlLicenceToBill %>">Pricing</a>
        </span>
    </div>

</asp:Content>

<asp:Content ContentPlaceHolderID="Zone3" runat="server">

    <div class="home">
        <div class="third1 left">

            <img id="img-sample2" src="/Content/images/sample2.png"/>

            <div class="service-feature">
               <h3><div class="bullet">1</div>&nbsp;Network #1</h3>
               <br />
               <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum vitae nibh quis augue vulputate tempor eu quis eros.</p>
            </div>
        

        </div>
        <div class="third2 left">

            <img id="img1" src="/Content/images/sample3.png"/>

            <div class="service-feature">
               <h3><div class="bullet">2</div>&nbsp;It's exclusive VIP forum</h3>
               <br />
               <p>Vestibulum vitae nibh quis augue vulputate tempor eu quis eros. Ut quis dolor nulla. Maecenas nec magna elit.</p>
            </div>

        </div>
        <div class="third3 right">

            <img id="img2" src="/Content/images/sample4.png"/>

            <div class="service-feature">
               <h3><div class="bullet">3</div>&nbsp;Up to 309 232 contacts to reach</h3>
               <br />
               <p>Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Maecenas nec magna elit.</p>
            </div>

        </div>
        <div class="spacer"></div>    
    <br />
    <br />
    </div>
</asp:Content>


