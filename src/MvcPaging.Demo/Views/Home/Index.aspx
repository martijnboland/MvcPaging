<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

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
    <dl>
		<dt><%= Html.ActionLink("Paging with AJAX", "IndexAjax", "Paging") %></dt>
		<dd>The pager supports unobtrusive AJAX via jquery</dd>
    </dl>
</asp:Content>
