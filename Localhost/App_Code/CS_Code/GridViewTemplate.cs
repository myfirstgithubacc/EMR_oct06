using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;


public class GridViewTemplate : ITemplate
{
    private DataControlRowType templateType;
    private string columnName;
    private int columnWidth;
    private string columnType;
    public string GetContentFillerText()
    {
        return
            "ASP.NET AJAX is a free framework for building a new generation of richer, more interactive, highly personalized cross-browser web applications.  " +
            "This new web development technology from Microsoft integrates cross-browser client script libraries with the ASP.NET 2.0 server-based development framework.  " +
            "In addition, ASP.NET AJAX offers you the same type of development platform for client-based web pages that ASP.NET offers for server-based pages.  " +
            "And because ASP.NET AJAX is an extension of ASP.NET, it is fully integrated with server-based services. ASP.NET AJAX makes it possible to easily take advantage of AJAX techniques on the web and enables you to create ASP.NET pages with a rich, responsive UI and server communication.  " +
            "However, AJAX isn't just for ASP.NET.  " +
            "You can take advantage of the rich client framework to easily build client-centric web applications that integrate with any backend data provider and run on most modern browsers.  ";
    }
    public GridViewTemplate(DataControlRowType type, string colname, int colWidth, string colType)
    {
        templateType = type;
        columnName = colname;
        columnWidth = colWidth;
        columnType = colType;
    }

    public void InstantiateIn(System.Web.UI.Control container)
    {
        switch (templateType)
        {
            case DataControlRowType.Header:
                Literal lc = new Literal();
                lc.Text = HttpUtility.HtmlDecode("<span style=\"font-family:Verdana;font-size:11px;font-weight:bold;color:black;height:22px\">" + columnName + "</span>");
                container.Controls.Add(lc);
                break;

            case DataControlRowType.Footer:
                Label fc = new Label();
                fc.Text = "Next";//HttpUtility.HtmlDecode("<span style=\"font-family:Verdana;font-size:11px;font-weight:bold;color:black;height:22px\">" + columnName + "</span>");
                //fc.DataBinding += new EventHandler(this.Label_DataBinding);
                container.Controls.Add(fc);
                fc.ID = columnName;
                if (fc.ID.Substring(0, 1).ToString() == "S")
                {
                    //fc.Attributes.Add("onclick", "javascript:alert('sdas');");
                    fc.Attributes.Add("style", "cursor: pointer;");
                }
                else
                {
                    fc.Visible = false;
                }
                break;

            case DataControlRowType.DataRow:
                if (columnType == "Link")
                {
                    HyperLink lb = new HyperLink();
                    lb.ID = columnName;
                    lb.Font.Name = "Verdana";
                    lb.Font.Size = 8;
                    lb.Height = 14;
                    lb.Width = columnWidth;
                    lb.DataBinding += new EventHandler(this.Link_DataBinding);
                    container.Controls.Add(lb);


                }
                else if (columnType == "Label")
                {
                    Label lb = new Label();
                    lb.ID = columnName;
                    lb.Font.Name = "Verdana";
                    lb.Font.Size = 8;
                    lb.Height = 14;
                    lb.Width = columnWidth;
                    lb.DataBinding += new EventHandler(this.Label_DataBinding);
                    container.Controls.Add(lb);

                }
                else
                {
                    TextBox lb = new TextBox();
                    lb.ID = columnName;
                    lb.Font.Name = "Verdana";
                    lb.Font.Size = 8;
                    lb.Height = 14;
                    lb.Width = columnWidth;
                    lb.DataBinding += new EventHandler(this.TextBox_DataBinding);
                    container.Controls.Add(lb);
                    lb.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                    lb.ReadOnly = true;
                }
                break;
            default:
                break;
        }
    }

    private void TextBox_DataBinding(Object sender, EventArgs e)
    {
        TextBox l = (TextBox)sender;
        GridViewRow row = (GridViewRow)l.NamingContainer;
        l.Text = DataBinder.Eval(row.DataItem, columnName).ToString();

    }

    private void Label_DataBinding(Object sender, EventArgs e)
    {
        Label l = (Label)sender;
        GridViewRow row = (GridViewRow)l.NamingContainer;

        l.Text = DataBinder.Eval(row.DataItem, columnName).ToString();
        l.Attributes.Add("cursor", "pointer");
    }

    private void Link_DataBinding(Object sender, EventArgs e)
    {
        int intColNo;
        string strtmp;
        string str;
        HyperLink l = (HyperLink)sender;
        GridViewRow row = (GridViewRow)l.NamingContainer;

        intColNo = Convert.ToInt16(columnName.Substring(5, 1));
        l.Text = DataBinder.Eval(row.DataItem, columnName).ToString();

        if (Convert.ToString(DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "3")).Length > 0)
        {
            String sAcc = "";
            if (Convert.ToString(l.Text).Length == 0)
            {
                l.Text = "";
            }
            else
            {
                if ((l.Text.IndexOf("-") > 1) && (l.Text.IndexOf(",") > 1))
                {
                    sAcc = l.Text.Substring(l.Text.IndexOf("-") + 1, (l.Text.IndexOf(",") - l.Text.IndexOf("-")) - 1);
                }
            }
           
            strtmp = DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "1").ToString();
            strtmp = strtmp + "&t=" + DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "3").ToString();
            strtmp = strtmp + "&dt=" + Convert.ToDateTime(DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "5"));
            strtmp = strtmp + "&appid=" + DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "8").ToString();
            strtmp = strtmp + "&dn=" + DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "2").ToString();
            strtmp = strtmp + "&Ac=" + sAcc.ToString();
            if (!DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "3").ToString().Contains("Next"))
            {
                System.Text.StringBuilder strToolTip = new System.Text.StringBuilder();
                strToolTip.Remove(0, strToolTip.Length);
                if (Convert.ToString(l.Text).Length > 0)
                {
                    String StrText = l.Text.ToString().Trim();
                    String strUHID = "0";
                    String strName = "";
                    String strApptType = "";
                    String strRemarks = "";
                    strUHID = StrText.Substring(l.Text.IndexOf("-") + 1, (l.Text.IndexOf(",") - l.Text.IndexOf("-")) - 1); ;
                    StrText = StrText.Substring(StrText.IndexOf(",") + 1).Trim();
                    strName = StrText.Substring(0, StrText.IndexOf(",")).Trim();
                    if (StrText.IndexOf(",") > 0)
                    {
                        StrText = StrText.Substring(StrText.IndexOf(",") + 1).Trim();
                    }
                    if (StrText.ToString().Trim().Length > 0)
                    {
                        if (StrText.IndexOf(",") > 0)
                        {
                            strApptType = StrText.Substring(1, StrText.IndexOf(",") - 1);
                        }
                        else
                        {
                            strApptType = StrText.ToString().Trim();
                        }
                    }
                    if (StrText.ToString().Trim().Length > 0)
                    {
                        if (StrText.IndexOf(",") > 0)
                        {
                            StrText = StrText.Substring(StrText.IndexOf(",") + 1);
                        }
                        else
                        {
                            StrText = "";
                        }
                    }
                    if (StrText.ToString().Trim().Length > 0)
                    {
                        if (StrText.IndexOf(",") > 0)
                        {
                            strRemarks = StrText.Substring(1, StrText.Length - 1);
                        }
                        else
                        {
                            strRemarks = StrText.ToString().Trim();
                        }
                    }


                    if (strUHID != "0")
                    {
                        strToolTip.Append("{0}UHID : " + strUHID);
                    }
                    else
                    {
                        strToolTip.Append("{0}UHID : Not Available");
                    }
                    strToolTip.Append("{0}Name : " + strName);
                    strToolTip.Append("{0}Appointment Type : " + strApptType);
                    if (strRemarks != "")
                    {
                        strToolTip.Append("{0}Remarks : " + strRemarks);
                    }
                    else
                    {
                        strToolTip.Append("{0}Remarks : Not Available");
                    }
                }
                l.ToolTip = string.Format("Appointment Date & Time:" + Convert.ToDateTime(DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "5")).ToString("dd/MM/yyyy") + " " + Convert.ToDateTime(DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "3")).ToString("hh:mm tt") + strToolTip.ToString(), Environment.NewLine);
            }
            //if (!DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "3").ToString().Contains("Next"))
            //    l.ToolTip = "Appointment Date & Time: " + Convert.ToDateTime(DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "5")).ToString("dd/MM/yyyy") + " " + Convert.ToDateTime(DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "3")).ToString("hh:mm tt");
            
            
            //l.Attributes.Add("onclick", "window.showModalDialog('AppointmentOptions.aspx?d=" + strtmp + "', null,'status:no;dialogWidth:500px;dialogHeight:75px;dialogHide:true;help:no;scroll:no');");


            //str = "d=" + Request.QueryString["d"].ToString();
            //str = str + "&t=" + Request.QueryString["t"].ToString();
            //str = str + "&dt=" + Request.QueryString["dt"].ToString();
            //str = str + "&appid=" + Request.QueryString["appid"].ToString();

            str = "d=";
            str = str + DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "1").ToString();
            str = str + "&t=" + DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "3").ToString();
            str = str + "&dt=" + Convert.ToDateTime(DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "5"));
            str = str + "&appid=" + DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "8").ToString();
            str = str + "&Ac=" + sAcc.ToString();
            string strTitlePart = DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "3").ToString();
            if (DateTime.Now.Date > Convert.ToDateTime(DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "5")))
            {
                //l.Attributes.Add("onclick", "options=window.open('AppointmentOptions.aspx?d=" + strtmp + "', null,'resizable=0,left=300,top=300,Width=250,Height=162,status=0,toolbars=0,menubar=0,location=no,directories=0');options.focus();");
                l.Attributes.Add("onclick", "__Open_Help('" + l.ClientID + "', 'Appointment Options - " + strTitlePart + "', 150, 270, 50, 'AppointmentOptions.aspx')");
            }

            else
            {
                if (DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "3").ToString().Trim() != "Next")
                {
                    //if (DateTime.Now.Date == Convert.ToDateTime(DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "5")) && Convert.ToDateTime("01/01/1900" + ' ' + DateTime.Now.ToString("hh:mm tt")) > Convert.ToDateTime("01/01/1900 " + DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "3").ToString()) && Convert.ToInt64(DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "8").ToString()) == 0)
                    //{
                    //    l.Attributes.Add("onclick", "options=window.open('AppointmentOptions.aspx?d=" + strtmp + "', null,'resizable=0,left=300,top=300,Width=250,Height=162,status=0,toolbars=0,menubar=0,location=no,directories=0');options.focus();");
                    //}
                    //else
                    //{
                        //HttpContext.Current.Request.Cookies copyappointmentId = HttpContext.Current.Request.Cookies.Get("copyappointmentId");


                        if (HttpContext.Current.Request.Cookies.AllKeys.Contains("copyappointmentId") == true)
                        {

                            //if (Convert.ToString(HttpContext.Current.Request.Cookies.Get("copyappointmentId").Value) != "")
                            //{

                            //}
                            //else
                            //{
                            //    
                            //}

                            if (Convert.ToString(HttpContext.Current.Request.Cookies["copyappointmentId"].Value) != "")
                            {
                                //l.Attributes.Add("onclick", "options=window.open('AppointmentOptions.aspx?d=" + strtmp + "', null,'resizable=0,left=300,top=300,Width=250,Height=162,status=0,toolbars=0,menubar=0,location=no,directories=0');options.focus();");
                                l.Attributes.Add("onclick", "__Open_Help('" + l.ClientID + "', 'Appointment Options - " + strTitlePart + "', 150, 270, 50, 'AppointmentOptions.aspx?d=" + strtmp + "')");
                            }
                            else
                            {

                                l.Attributes.Add("onclick", "pos=window.showModalDialog('AppointmentDetails.aspx?" + str + "','mywindow','status:no;dialogWidth:800px;dialogHeight:650px;dialogHide:true;help:no;scroll:no'); document.all(\"btnRefresh\").click();");

                            }



                        }

                        else
                        {

                            if (Convert.ToInt64(DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "8").ToString()) > 0)
                            {
                                //l.Attributes.Add("onclick", "options=window.open('AppointmentOptions.aspx?d=" + strtmp + "', null,'resizable=0,left=300,top=300,Width=250,Height=162,status=0,toolbars=0,menubar=0,location=no,directories=0');options.focus();");
                                l.Attributes.Add("onclick", "__Open_Help('" + l.ClientID + "', 'Appointment Options - " + strTitlePart + "', 150, 270, 50, 'AppointmentOptions.aspx?d=" + strtmp + "')");

                            }

                            else
                            {

                                l.Attributes.Add("onclick", "pos=window.showModalDialog('AppointmentDetails.aspx?" + str + "',\"mywindow\", \"status:no;dialogWidth:800px;dialogHeight:650px;dialogHide:true;help:no;scroll:no\"); document.all(\"btnRefresh\").click();");

                            }


                        }



                    //}
                }
                else
                {
                    //l.Attributes.Add("onclick", "options=window.open('AppointmentOptions.aspx?d=" + strtmp + "', null,'resizable=0,left=300,top=300,Width=250,Height=162,status=0,toolbars=0,menubar=0,location=no,directories=0');options.focus();");
                    l.Attributes.Add("onclick", "__Open_Help('" + l.ClientID + "', 'Appointment Options - " + strTitlePart + "', 150, 270, 50, 'AppointmentOptions.aspx?d=" + strtmp + "')");
                }
            }



            //if (HttpContext.Current.Cache["copyappointmentId"] != null)
            //{
            //    l.Attributes.Add("onclick", "options=window.open('AppointmentOptions.aspx?d=" + strtmp + "', null,'resizable=0,left=300,top=300,Width=250,Height=140,status=0,toolbars=0,menubar=0,location=no,directories=0');options.focus();");
            //}
            //else
            //{
            //    l.Attributes.Add("onclick", "pos=window.open('AppointmentDetails.aspx?" + str + "',\"mywindow\", \"menubar=0,resizable=0,width=800,height=650,status=0,toolbars=0\");");

            //}


            l.Attributes.Add("onmouseover", "javascript:fnOnMouseOver('" + l.ClientID + "');");
            l.Attributes.Add("onmouseout", "javascript:fnOnMouseOut('" + l.ClientID + "');");
        }
    }

}