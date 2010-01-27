<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<IPagedList<Product>>" %>
<%@ Import Namespace="MvcPaging"%>
<%@ Import Namespace="MvcPaging.Demo.Models"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h1>Welcome to area 51</h1>
	<p>Not liking the area? <%= Html.ActionLink("Go back to the root area", "Index", "Home", new { area=String.Empty }, null) %></p>
	<table class="grid">
		<thead>
			<tr>
				<th>Product name</th>
				<th>Category</th>
			</tr>
		</thead>
		<tbody>
			<% foreach (var product in Model) { %>
				<tr>
					<td><%= product.Name %></td>
					<td><%= product.Category %></td>
				</tr>
			<% } %>
		</tbody>
	</table>
	<div class="pager">
		<%= Html.Pager(Model.PageSize, Model.PageNumber, Model.TotalItemCount) %>
	</div>
</asp:Content>
