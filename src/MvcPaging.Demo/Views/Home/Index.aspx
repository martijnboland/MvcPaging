<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="MvcPaging.Demo.Controllers"%>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Html.Encode(ViewData["Message"]) %></h2>
    <dl>
		<dt><%= Html.ActionLink("Simple paging", "Index", "Paging") %></dt>
		<dd>An example of paging a list of products.</dd>
    </dl>
    <dl>
		<dt><%= Html.ActionLink("Paging with filtering", "ViewByCategory", "Paging") %></dt>
		<dd>A list of products that are filtered on the category</dd>
    </dl>
    <dl>
		<dt><%= Html.ActionLink("Simple paging in an area", "Index", "Paging", new { Area = "Area51" }, null ) %></dt>
		<dd>The pager also supports MVC 2 area's</dd>
    </dl>
</asp:Content>
