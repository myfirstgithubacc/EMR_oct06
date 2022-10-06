using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Configuration;

/// <summary>
/// Summary description for clsEmail
/// </summary>
public class clsEmail
{
    public clsEmail()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static bool CheckForInternetConnection()
    {
        try
        {
            using (var client = new WebClient())
            using (var stream = client.OpenRead("http://www.google.com"))
            {
                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    public static void SendEmail(string strSubject, Hashtable collToEmailIds, bool IsBodyHtml, string strBody)
    {
        string FromEmailId = ConfigurationManager.ConnectionStrings["FromEmailId"].ConnectionString;
        string FromEmailPWD = ConfigurationManager.ConnectionStrings["FromEmailPWD"].ConnectionString;

        if (collToEmailIds.Count == 0 || common.myLen(FromEmailId) == 0 || common.myLen(FromEmailPWD) == 0)
        {
            return;
        }

        if (CheckForInternetConnection())
        {
            MailMessage mail = new MailMessage();

            foreach (DictionaryEntry item in collToEmailIds)
            {
                if (common.myLen(item.Key) > 0)
                {
                    mail.To.Add(new MailAddress(common.myStr(item.Key), common.myStr(item.Value)));
                }
            }

            //mail.CC.Add(new MailAddress("", ""));
            //mail.Bcc.Add(new MailAddress("", ""));

            mail.From = new MailAddress(FromEmailId, FromEmailId);
            mail.Subject = strSubject;
            mail.Body = strBody;
            mail.IsBodyHtml = IsBodyHtml;
            mail.BodyEncoding = System.Text.Encoding.UTF8;

            SmtpClient client = new SmtpClient("akhilsystems.com", 2525);
            client.Credentials = new NetworkCredential(FromEmailId, FromEmailPWD);
            client.Host = "akhilsystems.com";
            client.Port = 2525;
            client.Send(mail);        
        }
    }

}
