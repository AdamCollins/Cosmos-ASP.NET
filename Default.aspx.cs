using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.UI.HtmlControls;

public partial class Default : System.Web.UI.Page
{
    private const string database = "cosmos_db";
    //local database
    private const string connectionString = "Server=.\\SQLEXPRESS;Database=" + database + "; Integrated Security=true";
    protected void Page_Load(object sender, EventArgs e)
    {
        Debug.WriteLine("User {0} connected",Session["username"]);
        loadPosts();
    }


    public void createPost(int post_id, string content, DateTime dt)
    {
        ASP.controls_postcontrol_ascx NewPost = (ASP.controls_postcontrol_ascx)LoadControl("~/Controls/PostControl.ascx");
        NewPost.post_id = post_id + "";
        NewPost.textContent = content;
        NewPost.dt = (48-(int)DateTime.UtcNow.Subtract(dt).TotalHours)+"h remaining";
        NewPost.comments = getComments(post_id);
        PostsPanel.Controls.Add(NewPost);

    }
    public void createPost(int post_id, string content, DateTime dt, bool allowComments)
    {
        ASP.controls_postcontrol_ascx NewPost = (ASP.controls_postcontrol_ascx)LoadControl("~/Controls/PostControl.ascx");
        NewPost.post_id = post_id + "";
        NewPost.textContent = content;
        NewPost.dt = (48 - (int)DateTime.UtcNow.Subtract(dt).TotalHours) + "h remaining";
        NewPost.comments = getComments(post_id);
        PostsPanel.Controls.Add(NewPost);

    }

    public string getComments(int post_id)
    {
        SqlConnection conn = null;
        if (connectToDatabase(ref conn))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT TOP (10) [id], [text_content], [post_datetime] FROM[" + database + "].[dbo].[forum_comments] WHERE[post_id] = @post_id ORDER BY[post_datetime] DESC; ", conn);
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
        return null;
    }

    public string createComment(string content, DateTime dt)
    {
        System.Web.UI.HtmlControls.HtmlGenericControl NewComment = new
        System.Web.UI.HtmlControls.HtmlGenericControl();
        NewComment.TagName = "div";
        int hours = (int)DateTime.UtcNow.Subtract(dt).Hours;
        int minutes = (int)DateTime.UtcNow.Subtract(dt).Minutes;
        NewComment.Attributes["class"] = "post comment";
        string postDate = "<span class='postDate' style='margin-left:50px;'>" + ((minutes>0)?(hours>0?hours+" hours ago ":+minutes+" mins ago"):"now") + "</span>";
        string postText = "<p style='border-top:1px solid #52FFB8; margin-left:50px;  margin-bottom:15px;'>" + content + "</p>";
        return postDate + postText;
    }

    public void loadPosts()
    {
        SqlConnection conn = null;

        if (connectToDatabase(ref conn))
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT id, textContent, postDateTime FROM [" + database + "].[dbo].[forum_posts]  WHERE DATEDIFF(HOUR,postDateTime,GETUTCDATE())<=48 ORDER BY id DESC;", conn);
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
    }

    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        //Removes Comment Area to allow multiple forms on one page

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
        SqlConnection conn = null;
        if (connectToDatabase(ref conn))
        {
            conn.Open();
            String query = "INSERT INTO [" + database + "].[dbo].[forum_posts] (textContent, postDateTime) VALUES (@textContent, GETUTCDATE())";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@textContent", textContent.Replace("\n","</br>"));
            command.ExecuteNonQuery();
            conn.Close();
            Response.Redirect(Request.RawUrl,false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }

    public void postCommentToDatabase(string textContent, int postId)
    {
        SqlConnection conn = null;
        if (connectToDatabase(ref conn))
        {
            conn.Open();
            String query = "INSERT INTO [" + database + "].[dbo].[forum_comments] (post_id, text_content, post_datetime) VALUES (@post_id, @textContent, GETUTCDATE())";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@post_id", postId);
            command.Parameters.AddWithValue("@textContent", textContent);
            command.ExecuteNonQuery();
            conn.Close();
            Response.Redirect(Request.RawUrl);
        }
    }

    protected void SubmitCommentButton_Click(object sender, EventArgs e)
    {
        SubmitPanel.Visible = false;
        string text = CommentTextBox.Text;
        if (text.Length > 0)
        {
            postCommentToDatabase(text, Convert.ToInt32(Post_id.Value));
            CommentTextBox.Text = null;
        }
        SubmitPanel.Visible = true;
    }
    protected void LogoutButton_Click(object sender, EventArgs e)
    {
        Debug.WriteLine("Loggout clicked");
        SignOut();
    }

    //Returns true if connected succesfully
    //Opens connection
    private bool connectToDatabase(ref SqlConnection conn)
    {
        try
        {
            conn = new SqlConnection(connectionString);
            //Tests Connection
            conn.Open();
            conn.Close();
            return true;
        } catch (SqlException e)
        {
            createPost(-1,"ERROR: COULD NOT CONNECT TO DATABASE. " +e.ToString(), DateTime.UtcNow);
            Debug.WriteLine("Cannot connect");
            return false;
        }
    }

    protected void LoginBtnButton_Click(object sender, EventArgs e)
    {
        Debug.WriteLine("Called");
        string username = usernameTF.Text;
        string password = passwordTF.Text;
        SqlConnection conn = null;
        if (connectToDatabase(ref conn))
        {
            Login login = new Login(database);
            User user = new User(username, password);
            if (login.IsLoginSuccesful(user, ref conn))
            {
                Session["loggedIn"] = true;
                Session["username"] = username;
                Debug.WriteLine("username:" + Session["username"]);
            }
        }
        Response.Redirect(Request.RawUrl);
    }


    protected void RegisterButton_Click(object sender, EventArgs e)
    {
        string username = usernameTF.Text;
        string password = passwordTF.Text;
        SqlConnection conn = null;
        if (connectToDatabase(ref conn))
        {
            Login login = new Login(database);
            User user = new User(username, password);
            login.addUser(user, ref conn);
            if (login.IsLoginSuccesful(user, ref conn))
            {
                Session["loggedIn"] = true;
                Session["username"] = username;
                Debug.WriteLine("username:" + Session["username"]);
            }
        }
        Response.Redirect(Request.RawUrl);
    }

    public void SignOut()
    {
        Session["username"] = "";
        Session["loggedIn"] = false;
        Response.Redirect(Request.RawUrl);
    }
}
