<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HandleErrorInfo>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Sorry, an error occurred while processing your request.
    </h2>
    <% if (!ViewContext.HttpContext.IsCustomErrorEnabled) { %>
        <h3>
            Exception details:
        </h3>
        <div style="overflow: auto;">
            <%
                Stack<Exception> exceptions = new Stack<Exception>();
                for (Exception ex = ViewData.Model.Exception; ex != null; ex = ex.InnerException) {
                    exceptions.Push(ex);
                }
                foreach (Exception ex in exceptions) {
                    %>
                        <div>
                            <b><%= Html.Encode(ex.GetType().FullName)%></b>: <%= Html.Encode(ex.Message)%>
                        </div>
                        <div>
                            <pre style="font-size: medium;"><%= Html.Encode(ex.StackTrace)%></pre>
                        </div>
                    <%
                }  
            %>
        </div>
    <% } %>
</asp:Content>
