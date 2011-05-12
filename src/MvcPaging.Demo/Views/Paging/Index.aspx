<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<IPagedList<Product>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

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
