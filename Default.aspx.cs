using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.UI.HtmlControls;

public partial class Default : System.Web.UI.Page 
{
    private const string Table = "";
    protected void Page_Load(object sender, EventArgs e)
    {
            loadPosts();
    }


    public void createPost(int post_id, string content, DateTime dt)
    {
        System.Web.UI.HtmlControls.HtmlGenericControl NewDiv = new
        System.Web.UI.HtmlControls.HtmlGenericControl();
        NewDiv.TagName = "div";
        NewDiv.Attributes["class"] = "post hidden";
        NewDiv.Attributes["post_id"] = post_id+"";
        string postDate = "<span class='postDate'>" + dt.ToString() + "</span>";
        string postText = "<p>" + content + "</p>";
        string postOptions = "<div class='fixed-action-btn horizontal myButtonGroup'>"
                               +"<a class='btn-floating btn-large'>"
                                    + "<i class='material-icons'>label_outline</i>"
                                + "</a>"
                                +"<ul>"
                                    +"<li><a class='btn-floating'><i class='material-icons'>star</i></a></li>"
                                    + "<li><a class='btn-floating blue darken-1 OpenReplyWindowBtn'><i class='material-icons'>chat_bubble_outline</i></a></li>"
                                    + "<li><a class='btn-floating green'><i class='material-icons'>report_problem</i></a></li>"
                                + "</ul>"
                             + "</div>";



        string comments = getComments(post_id);
        NewDiv.InnerHtml = postDate+postText+postOptions+comments;
        PostsPanel.Controls.Add(NewDiv);
    }

    private string getComments(int post_id)
    {
        SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=MyDataBase; Integrated Security=true");
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT TOP (10) [id], [text_content], [post_datetime] FROM[MyDataBase].[dbo].[forum_comments] WHERE[post_id] = @post_id ORDER BY[post_datetime] DESC; ", conn);
        cmd.Parameters.AddWithValue("@post_id", post_id);
        SqlDataReader reader = cmd.ExecuteReader();
        string comments = "";
        while (reader.Read())
        {
            comments += createComment(reader.GetString(1), reader.GetDateTime(2));
        }
        reader.Close();
        return comments;
    }

    public string createComment(string content, DateTime dt)
    {
        System.Web.UI.HtmlControls.HtmlGenericControl NewComment = new
        System.Web.UI.HtmlControls.HtmlGenericControl();
        NewComment.TagName = "div";
        NewComment.Attributes["class"] = "post comment";
        string postDate = "<span class='postDate' style='margin-left:50px;'>" + dt.ToString() + "</span>";
        string postText = "<p style='border-top:1px solid #52FFB8; margin-left:50px;  margin-bottom:15px;'>" + content + "</p>";
        return postDate + postText;
    }

    public void loadPosts()
    {
        SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=MyDataBase; Integrated Security=true");
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT id, textContent, postDateTime FROM [MyDataBase].[dbo].[forum_posts] ORDER BY id DESC;", conn);
        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            createPost(reader.GetInt32(0),reader.GetString(1), reader.GetDateTime(2));
        }
        reader.Close();
        conn.Close();
    }




    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        string text = SubmitText.Text;
        Debug.WriteLine("Posting: "+text);
        if (text.Length > 0)
        {
            postToDatabase(text);
            SubmitText.Text = "";
        }
    }

    public void postToDatabase(string textContent)
    {
        SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=MyDataBase; Integrated Security=true");
        conn.Open();
        String query = "INSERT INTO [MyDataBase].[dbo].[forum_posts] (textContent, postDateTime) VALUES (@textContent, GETDATE())";

        SqlCommand command = new SqlCommand(query, conn);
        command.Parameters.AddWithValue("@textContent", textContent);
        command.ExecuteNonQuery();
        conn.Close();
        Response.Redirect(Request.RawUrl);
    }

    public void postCommentToDatabase(string textContent, int postId)
    {
        SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=MyDataBase; Integrated Security=true");
        conn.Open();
        String query = "INSERT INTO [MyDataBase].[dbo].[forum_comments] (post_id, text_Content, post_datetime) VALUES (@post_id, @textContent, GETDATE())";
        SqlCommand command = new SqlCommand(query, conn);
        command.Parameters.AddWithValue("@post_id", postId);
        command.Parameters.AddWithValue("@textContent", textContent);
        command.ExecuteNonQuery();
        conn.Close();
        Response.Redirect(Request.RawUrl);
    }

    protected void SubmitCommentButton_Click(object sender, EventArgs e)
    {
        string text = CommentTextBox.Text;
        if (text.Length > 0)
        {
            postCommentToDatabase(text, Convert.ToInt32(Post_id.Value));
            CommentTextBox.Text = "";
        }
    }
}
