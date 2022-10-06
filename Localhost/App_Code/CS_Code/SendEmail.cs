using System;
using System.Text;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Web;
using System.Net.Mail;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Collections;


/// <summary>
/// Summary description for SendEmail
/// </summary>
public class Email
{
    string sConString = "";
    DAL.DAL DlObj;

    public Email(string Constring)
    {
        sConString = Constring;
        DlObj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

    }
    public static string SendEmail(string EmailTo, string EmailFrom, string EmailCC, string sSubject, string sBody)
    {
        MailMessage Msg = new MailMessage(EmailFrom, EmailTo);

        if (!string.IsNullOrEmpty(EmailCC.Trim()))
        {
            Msg.CC.Add(EmailCC);
        }
        Msg.Subject = sSubject;
        Msg.Body = sBody;
        Msg.IsBodyHtml = true;

        SmtpClient smtpClient = new SmtpClient();

        SmtpSection smtpSec = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");


        if (smtpSec.Network.Port == 465 || smtpSec.Network.Port == 587)
        {
            smtpClient.EnableSsl = true;
        }

        smtpClient.Send(Msg);
        return "";
    }

    public int SMS(int iHospitalLocationId, int iFacilityId, string MobileNo, string msg)
    {
        if (MobileNo.Trim() != "")
        {
            WebClient client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            BaseC.SmsClass smsSetup = new BaseC.SmsClass(sConString);
            DataTable dt = smsSetup.GetSMSSetup(Convert.ToInt16(iHospitalLocationId), Convert.ToInt16(iFacilityId));
            if (dt.Rows.Count > 0)
            {
                Stream data = default(Stream);
                //Vendor URl
                //string sms =http://115.112.185.85:6060/urldreamclient/smsapi?userName=f111&password=b111&clientid=dredst22&to=9818666503&text=Hi, thanks for your information. We have updated the same. Wish you all the best. Regards.
                string sms = dt.Rows[0]["PushURL"].ToString() + dt.Rows[0]["UID"].ToString()
                    + "&password=" + dt.Rows[0]["PWD"].ToString() + "&clientid=dredst22&to=" + MobileNo + "&text=" + msg;
                data = client.OpenRead(sms);
                StreamReader reader = new StreamReader(data);
                string s = null;
                s = reader.ReadToEnd();
                data.Close();
                reader.Close();
                return 1;
            }
            else
                return 0;
        }
        else
            return 0;
    }
    public int SMSReferral(int HospId, int ReferToDoctorId, int EncounterId)
    {
        //WebClient client = new WebClient();
        WebClient client = new WebClient();
        client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
        BaseC.SmsClass smsSetup = new BaseC.SmsClass(sConString);
        DataTable dt = smsSetup.GetSMSSetup(Convert.ToInt16(HospId), Convert.ToInt16(System.Web.HttpContext.Current.Session["FacilityId"]));
        DataSet ds = smsSetup.GetSmsDetails(HospId, ReferToDoctorId, EncounterId);
        if (ds.Tables[0].Rows.Count > 0 && dt.Rows.Count > 0)
        {
            Stream data = default(Stream);
            string patientnam = common.myStr(ds.Tables[0].Rows[0]["PatientName"]);
            string doctorname = common.myStr(ds.Tables[0].Rows[0]["DoctorName"]);
            string Regno = common.myStr(ds.Tables[0].Rows[0]["RegNo"]);
            string mobile = common.myStr(ds.Tables[0].Rows[0]["Mobilno"]);
            string IPID = common.myStr(ds.Tables[0].Rows[0]["IPID"]);
            if (mobile != "")
            {
                string sms = dt.Rows[0]["PushURL"].ToString() + dt.Rows[0]["UID"].ToString()
                    + "&password=" + dt.Rows[0]["PWD"].ToString() + "&clientid=dredst22&to=" + mobile + "&text=Dear Sir/Madam, This is an intimation about " + patientnam + " IPID " + IPID + " admitted under Dr. " + doctorname.Trim() + " for a cross referral.";
                data = client.OpenRead(sms);
                StreamReader reader = new StreamReader(data);
                string s = null;
                s = reader.ReadToEnd();
                data.Close();
                reader.Close();
                return 1;
            }
            else
                return 0;
        }
        else
            return 0;
    }
}

