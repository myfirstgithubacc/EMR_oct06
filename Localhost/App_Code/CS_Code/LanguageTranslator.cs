using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for LanguageTranslator
/// </summary>
public class LanguageTranslator
{
    /// <summary>
    /// Translates the text.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="languagePair">The language pair.</param>
    /// <returns></returns>
    public static void Main(string[] args)
    {
        TranslateText(args[1], args[2]);
    }

    /// <summary>
    /// Translate Text using Google Translate
    /// </summary>
    /// <param name="input">The string you want translated</param>
    /// <param name="toCulture">2 letter Language Pair, delimited by "|". 
    /// e.g. "en|da" language pair means to translate from English to Danish</param>
    /// <param name="encoding">The encoding.</param>
    /// <returns>Translated to String</returns>

    public static string TranslateText(string input, string toCulture)
    {
        string fromCulture = "en";
        toCulture = toCulture.ToLower();

        // normalize the culture in case something like en-us was passed 
        // retrieve only en since Google doesn't support sub-locales
        string[] tokens = fromCulture.Split('-');
        if (tokens.Length > 1)
        {
            fromCulture = tokens[0];
        }

        // normalize ToCulture
        tokens = toCulture.Split('-');
        if (tokens.Length > 1)
        {
            toCulture = tokens[0];
        }

        //string url = string.Format(@"https://translate.google.co.in/?client=j&hl=en&text={0}&sl={1}&tl={2}",
        //                           HttpUtility.UrlEncode(input), fromCulture, toCulture);
        string url = string.Format(@"https://translate.google.co.in/translate_a/t?client=j&text={0}&hl=en&sl={1}&tl={2}",
                                   HttpUtility.UrlEncode(input), fromCulture, toCulture);

        // Retrieve Translation with HTTP GET call
        string html = null;
        try
        {
            WebClient web = new WebClient();

            // MUST add a known browser user agent or else response encoding doen't return UTF-8 (WTF Google?)
            web.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0");
            web.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");

            // Make sure we have response encoding to UTF-8
            web.Encoding = Encoding.UTF8;
            html = web.DownloadString(url);
        }
        catch (Exception ex)
        {
            // this.ErrorMessage = Westwind.Globalization.Resources.Resources.ConnectionFailed + ": " +
            // ex.GetBaseException().Message;
            //   return null;
        }


        string result = Microsoft.TeamFoundation.WorkItemTracking.Controls.HtmlFilter.ConvertToPlainText(html);
        // Extract out trans":"...[Extracted]...","from the JSON string
        // string result = Regex.Match(html, @"content/([A-Za-z0-9\-]+)\.aspx$", RegexOptions.IgnoreCase).Groups[1].Value;
        
        if (string.IsNullOrEmpty(result))
        {
            // this.ErrorMessage = Westwind.Globalization.Resources.Resources.InvalidSearchResult;
            // return null;
        }

        //return WebUtils.DecodeJsString(result);

        // Result is a JavaScript string so we need to deserialize it properly
        JavaScriptSerializer ser = new JavaScriptSerializer();
        return ser.Deserialize(result, typeof(string)) as string;

    }


}