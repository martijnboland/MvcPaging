<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<IPagedList<Product>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<% using (Html.BeginForm("ViewByCategory", "Paging", FormMethod.Get)) { %>
	<p>
		Select a category:
		<%= Html.DropDownList("categoryName") %>
		<input type="submit" value="Browse" />
	</p>
	<% } %>
	<% if (Model.Count > 0) { %>
	<p>
		Found <%= Model.TotalItemCount %> items.
	</p>
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
					<td><%= product.Name%></td>
					<td><%= product.Category%></td>
				</tr>
			<% } %>
		</tbody>
	</table>
	<div class="pager">
		<%= Html.Pager(Model.PageSize, Model.PageNumber, Model.TotalItemCount, new { categoryname = ViewData["CategoryDisplayName"] } )%>
	</div>
	<% } %>
</asp:Content>
