using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SocialsTaker : MonoBehaviour
{
    public void Discord()
    {
        Application.OpenURL("https://discord.gg/yQxvXQuPEr");
    }
    public void Twitter()
    {
        Application.OpenURL("https://twitter.com/Best_Zer0");
    }
    public void Youtube()
    {
        Application.OpenURL("https://www.youtube.com/channel/UCtwDr9OE4vDnqwG_R-2Rs9Q");
        Application.OpenURL("https://www.youtube.com/channel/UCKbCr8vqfXK3tvbE9VOs1xw");
    }
    public void Instagram()
    {
        Application.OpenURL("https://www.instagram.com/dystopian_games/");
    }
    public void Email()
    {
        string email = "tejaaditya.g@gmail.com";
        Application.OpenURL("mailto:" + email);
    }
        
}
