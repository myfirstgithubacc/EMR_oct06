<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Default.ascx.cs" Inherits="Controls_Default" %>
<div class="sideNav">
    <a href="<%=Page.ResolveUrl(ConfigRoot)%>?Page=Import">Import a new dictionary.</a>
    &nbsp;&nbsp;
     <a href="<%= Page.ResolveUrl(ConfigRoot)%>?Page=EditDictionary">Edit dictionary.</a>
    
</div>
