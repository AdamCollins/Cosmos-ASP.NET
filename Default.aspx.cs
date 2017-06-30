using System;
using System.Data.SqlClient;

public partial class Default : System.Web.UI.Page 
{
    private const string Table = "";
    protected void Page_Load(object sender, EventArgs e)
    {
            loadPosts();
    }
    

    public void createPost(string content, DateTime dt)
    {
        System.Web.UI.HtmlControls.HtmlGenericControl NewDiv = new
        System.Web.UI.HtmlControls.HtmlGenericControl();
        NewDiv.TagName = "div";
        NewDiv.Attributes["class"] = "post hidden";
        string postDate = "<span class='postDate'>" + dt.ToString() + "</span>";
        string postText = "<p>" + content + "</p>";
        string postOptions = "<div class='fixed-action-btn horizontal myButtonGroup'>"
                               +"<a class='btn-floating btn-large'>"
                                    + "<i class='material-icons'>label_outline</i>"
                                + "</a>"
                                +"<ul>"
                                    +"<li><a class='btn-floating'><i class='material-icons'>star</i></a></li>"
                                    + "<li><a class='btn-floating blue darken-1'><i class='material-icons'>chat_bubble_outline</i></a></li>"
                                    + "<li><a class='btn-floating green'><i class='material-icons'>report_problem</i></a></li>"
                                + "</ul>"
                             + "</div>";
        NewDiv.InnerHtml = postDate+postText+postOptions;
        PostsPanel.Controls.Add(NewDiv);
    }
    public void loadPosts()
    {
        SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=MyDataBase; Integrated Security=true");
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT textContent, postDateTime FROM [MyDataBase].[dbo].[forum_posts] ORDER BY id DESC;", conn);
        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            createPost(reader.GetString(0),reader.GetDateTime(1));
        }
        reader.Close();
    }



    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        string text = SubmitText.Text;
        if (text.Length > 0)
        {
            postToServer(text);
            SubmitText.Text = "";
        }
    }

    public void postToServer(string textContent)
    {
        SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=MyDataBase; Integrated Security=true");
        conn.Open();
        String query = "INSERT INTO [MyDataBase].[dbo].[forum_posts] (textContent, postDateTime) VALUES (@textContent, GETDATE())";

        SqlCommand command = new SqlCommand(query, conn);
        command.Parameters.AddWithValue("@textContent", textContent);
        command.ExecuteNonQuery();
        Response.Redirect(Request.RawUrl);
    }
}
