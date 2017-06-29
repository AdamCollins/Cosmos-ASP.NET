<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default"  EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="https://fonts.googleapis.com/css?family=Lato" rel="stylesheet"/>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <!--Materialize-->
    <!-- Compiled and minified CSS -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/materialize/0.99.0/css/materialize.min.css">
     <!-- Compiled and minified JavaScript -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/materialize/0.99.0/js/materialize.min.js"></script>
    <!--Icons-->
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet">
    <!--MyStyleSheets-->
    <link href="StyleSheet.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
        </Scripts>
    </telerik:RadScriptManager>
    <script type="text/javascript">
        //Put your JavaScript code here.
    </script>
    <div>
        <h1>COSMOS</h1>
        <asp:Panel ID="SubmitPanel" CssClass="submitPanel" runat="server">
             <form class="col s12">
                <div class="row">
                    <div class="input-field col s12">
                        <asp:TextBox ID="SubmitText" CssClass="materialize-textarea" TextMode="MultiLine" runat="server"></asp:TextBox>
                        <label for="SubmitText">What's on your mind?</label>
                    </div>
                    <asp:LinkButton ID="submitButton" CssClass="btn waves-effect waves-light" type="submit" OnClick="SubmitButton_Click" runat="server">Submit
                    <i class="material-icons right">send</i></asp:LinkButton>
                 </div>
            </form>
        </asp:Panel>
        <asp:Panel ID="PostsPanel" runat="server"></asp:Panel>
    </div>
    </form>
</body>
</html>
