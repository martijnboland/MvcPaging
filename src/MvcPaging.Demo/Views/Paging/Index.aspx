<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IPagedList<Product>>" %>
<%@ Import Namespace="MvcPaging"%>
<%@ Import Namespace="MvcPaging.Demo.Models"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

	<table class="grid">
		<thead>
			<tr>
				<th>Product name</th>
				<th>Category</th>
			</tr>
		</thead>
		<tbody>
			<% foreach (var product in ViewData.Model) { %>
				<tr>
					<td><%= product.Name %></td>
					<td><%= product.Category %></td>
				</tr>
			<% } %>
		</tbody>
	</table>
	<div class="pager">
		<%= Html.Pager(ViewData.Model.PageSize, ViewData.Model.PageNumber, ViewData.Model.TotalItemCount) %>
	</div>
</asp:Content>
