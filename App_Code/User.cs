using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Class1
/// </summary>
public class User
{
    private string username;
    private string password;
    public User(string username, string password)
    {
        Username = username;
        Password = password;
    }
    public string Username { get; set; }
    public string Password { get; set; }
}