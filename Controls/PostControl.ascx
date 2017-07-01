<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PostControl.ascx.cs" Inherits="WebUserControl" %>
<script runat="server">
    public string dt;
    public string textContent;
    public string comments;
    public string post_id;
</script>
<div class="post hidden col s12 m6" runat="server">
    <span class='postDate'><%= dt %></span>
    <span class="post_id hidden"><%= post_id %></span>
    <p><%= textContent %></p>
    <div class='fixed-action-btn horizontal myButtonGroup'>
        <a class='btn-floating btn-large'><i class='material-icons'>label_outline</i></a>
        <ul>
            <li><a class='btn-floating'><i class='material-icons'>star</i></a></li>
            <li><a class='btn-floating blue darken-1 OpenReplyWindowBtn'><i class='material-icons'>chat_bubble_outline</i></a></li>
            <li><a class='btn-floating green'><i class='material-icons'>report_problem</i></a></li>
        </ul>
    </div>
    <%= comments %>
</div>
