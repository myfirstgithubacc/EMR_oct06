using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Configuration;
using System.Globalization;


    //----------------------------------------------------------------------------
    //   Purpose           :To write methods of parsing text
    //  --------------------------------------------------------------------------
    public class ParseData
     {
         //----------------------------------------------------------------------------
         //   Purpose           :To parse user entered data
         //   Input parameters  :string
         //   Output Parameters :string
         //   Author            :Madhukar Jha
         //  --------------------------------------------------------------------------
        public string ParseQ(string str)
        {

            if (str == null) return null;
            if (str.Length > 0)
            {
                str = str.Replace("&nbsp;", "");
                str = str.Replace("&apos;", "'");
                str = str.Replace("&#39;", "'");
                //str = str.Replace("'", "''");
                str = str.Replace("\r\n", " ");
                str = str.Replace(";", "");

                str = str.Trim();
            }
            return str;
        }

        //----------------------------------------------------------------------------
        //   Purpose           :To parse xml data
        //   Input parameters  :string
        //   Output Parameters :string
        //   Author            :Madhukar Jha
        //  --------------------------------------------------------------------------
         public string ParseXML(string str)
        {
            if (str == "") return null;
            if (str.Length > 0)
            {
                str = str.Replace("&", "&amp;");
                str = str.Replace("<", "&lt;");
                str = str.Replace(">", "&gt;");
                str = str.Replace("\"", "&quot;");
                str = str.Replace("'", "&apos;");

                str = str.Trim();
            }
            return str;
        }


        //----------------------------------------------------------------------------
        //   Purpose           :To parse user entered data and add proper operator for full text search
        //   Input parameters  :string
        //   Output Parameters :string
        //   Author            :Madhukar Jha
        //  --------------------------------------------------------------------------
        public string ParseSearchData(string str, int Criteria)
        {
            //0= Search all of the words
            //1= Search any of the words
            //2= Exact Phrase
            //3= Boolean Search

            str = ParseQ(str);

            bool sFlag = false;
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            str = str.Trim();
            while ((Regex.IsMatch(str, "\\s{2}")))
            {
                str = str.Replace("  ", " ");
            }

            if (str.IndexOf(" OR ", 0) > 1)
            {
                str = str.Replace(" OR ", " | ");
                sFlag = true;
            }
            if (str.IndexOf(" AND ", 0) > 1)
            {
                str = str.Replace(" AND ", " & ");
                sFlag = true;
            }
            if (str.IndexOf(" NOT ",0) > 1)
            {
                str = str.Replace(" NOT ", " &! ");
                sFlag = true;
            }
            if (str.IndexOf(" AND NOT ", 0) > 1)
            {
                str = str.Replace(" AND NOT ", " &! ");
                sFlag = true;
            }
            if (str.IndexOf(" NEAR ", 0) > 1)
            {
                str = str.Replace(" NEAR ", " ~ ");
                sFlag = true;
            }

            if (str.Length < 1)
            {
                return str;
            }

            if (Criteria == 0 & !sFlag)
            {
                str = str.Replace(" ", "\" ~ \"");
            }
            else if (Criteria == 0 & Regex.IsMatch(str, "\"\\s*\""))
            {
                str = Regex.Replace(str, "\"\\s*\"", "\" ~ \"");
            }
            if (Criteria == 1 & !sFlag)
            {
                str = str.Replace(" ", "\" | \"");
            }
            if (!str.StartsWith("\"")) str = "\"" + str;
            if (!str.EndsWith("\"")) str += "\"";
            str = str.Replace("\"\"", "\"");


            //Exact Phrase
            if (Criteria == 2)
            {
                str = " " + str + " ";

                return str;

            }
            return str;

        }
        public string ParseQ1(string str)
        {
            if (str == "") return null;
            if (str.Length > 0)
            {
                str = str.Replace("&apos;", "'");
                str = str.Replace("&#39;", "'");
                str = str.Replace("'", "''");
                str = str.Replace("\r\n", " ");
                str = str.Replace(";", "");
                str = str.Trim();
            }
            return str;
        }

        public string FormatDate(string Date)
        {
            char[] chArr = {'/'};
            string[] strDate = Date.Split(chArr);
            return strDate[2] + "/" + strDate[1] + "/" + strDate[0];

        }

        public string MakeXMLString(DataTable dt)
        {
            string str = null;
            StringBuilder strServices = new StringBuilder(2000);
            short intRow = 0;

            strServices.Remove(0, strServices.Length);

            foreach (DataRow dr in dt.Rows)
            {
                intRow = 1;
                strServices.Append("<Table1>");
                foreach (DataColumn dc in dt.Columns)
                {
                    strServices.Append("<c" + intRow + ">" + dr[dc] + "" + "</c" + intRow + ">");
                    intRow += 1;
                }
                strServices.Append("</Table1>");
            }
            str = strServices.ToString();
            return str;
        }

    }
