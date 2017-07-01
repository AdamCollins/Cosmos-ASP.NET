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
        /*ASP.controls_postcontrol_ascx NewPost = (ASP.controls_postcontrol_ascx)LoadControl("~/Controls/PostControl.ascx");
        NewPost.post_id = "4";
        NewPost.textContent = "Yeee boi more texttiy bois";
        NewPost.dt = "2017/8/1 12:00:00";
        NewPost.comments = getComments(4);
        PostControl.Controls.Add(NewPost);*/
    }


    public void createPost(int post_id, string content, DateTime dt)
    {
        ASP.controls_postcontrol_ascx NewPost = (ASP.controls_postcontrol_ascx)LoadControl("~/Controls/PostControl.ascx");
        NewPost.post_id = post_id + "";
        NewPost.textContent = content;
        NewPost.dt = dt.ToString();
        NewPost.comments = getComments(post_id);
        PostsPanel.Controls.Add(NewPost);

    }

    public string getComments(int post_id)
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
        conn.Close();
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

        try
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT id, textContent, postDateTime FROM [MyDataBase].[dbo].[forum_posts] ORDER BY id DESC;", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                createPost(reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2));
            }
            reader.Close();
        }
        finally
        {
            
            conn.Close();
        }
    }




    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        //Removes Comment Area to allow multiple forms on one page
        CommentArea.Visible = false;

        string text = SubmitText.Text;
        Debug.WriteLine("Posting: "+text);
        if (text.Length > 0)
        {
            postToDatabase(text);
            SubmitText.Text = "";
        }

        CommentArea.Visible = true;
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
            CommentTextBox.Text = null;
        }
    }
}
