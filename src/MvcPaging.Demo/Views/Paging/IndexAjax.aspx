<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<MvcPaging.IPagedList<MvcPaging.Demo.Models.Product>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div id="gridcontainer">
		<%= Html.Partial("ProductGrid", Model) %>
	</div>
</asp:Content>
