using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
/// <summary>
/// Summary description for Class1
/// </summary>
public class Login
{
    private string database;
    private SqlConnection conn;
    public Login(string database)
    {
        this.database = database;

    }

    public bool IsLoginSuccesful(User user, ref SqlConnection conn)
    {
        string query = "SELECT [hashed_password], [salt] FROM [cosmos_db].[dbo].[forum_users] WHERE [username] = '"+user.Username + "';";
        conn.Open();
        SqlCommand cmd = new SqlCommand(query, conn);
        SqlDataReader reader = null;
        try
        {
            reader = cmd.ExecuteReader();
            reader.Read();
            string strHashedPassword = reader.GetString(0);

            byte[] salt = reader.GetSqlBinary(1).Value;
            reader.Close();
            conn.Close();
            //Returns true if stored hash equals hash created
            return strHashedPassword == getPasswordHash(user.Password, salt);
        }
        catch (System.InvalidOperationException)
        {
            Debug.WriteLine("Login Failed: User Not found");
            return false;
        }  
        
    }

    private static string getPasswordHash(string password, byte[] salt)
    {
        //hashes with 10 itterations
        var hasher = new Rfc2898DeriveBytes(password, salt, 10);
        var hash = hasher.GetBytes(40);
        string hashedPassword = Convert.ToBase64String(hash);
        return hashedPassword;
    }

    private static byte[] generateSalt()
    {
        byte[] salt = new byte[64];
        RandomNumberGenerator rgn = new RNGCryptoServiceProvider();
        rgn.GetNonZeroBytes(salt);

        return salt;
    }

    public void addUser(User user, ref SqlConnection conn)
    {
        byte[] newSalt = generateSalt();
        
        String query = "INSERT INTO [" + database + "].[dbo].[forum_users] (username, hashed_password, salt, create_date) VALUES (@username, @hashed_password, @salt, GETUTCDATE())";
        conn.Open();
        SqlCommand command = new SqlCommand(query, conn);
        command.Parameters.AddWithValue("@username", user.Username);
        command.Parameters.AddWithValue("@hashed_password", getPasswordHash(user.Password,newSalt));
        command.Parameters.AddWithValue("@salt", newSalt);
        command.ExecuteNonQuery();
        conn.Close();
        user = null;
    }

}