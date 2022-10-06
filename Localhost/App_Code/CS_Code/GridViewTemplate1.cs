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

public class GridViewTemplate1 : ITemplate
{
    private DataControlRowType templateType;
    private string columnName;
    private int columnWidth;
    private string columnType;

    public GridViewTemplate1(DataControlRowType type, string colname, int colWidth, string colType)
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
                else
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
                break;
            default:
                break;
        }
    }

    private void Label_DataBinding(Object sender, EventArgs e)
    {
        Label l = (Label)sender;
        GridViewRow row = (GridViewRow)l.NamingContainer;
        l.Text = DataBinder.Eval(row.DataItem, columnName).ToString();
    }

    private void Link_DataBinding(Object sender, EventArgs e)
    {
        int intColNo;
        string strtmp;

        HyperLink l = (HyperLink)sender;
        GridViewRow row = (GridViewRow)l.NamingContainer;

        intColNo = Convert.ToInt16(columnName.Substring(5, 1));
        l.Text = DataBinder.Eval(row.DataItem, columnName).ToString();


        if (Convert.ToString(DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "3")).Length > 0)
        {
            if (Convert.ToString(l.Text).Length == 0)
            {
                l.Text = "";
            }
            strtmp = DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "1").ToString();
            strtmp = strtmp + "&t=" + DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "3").ToString();
            strtmp = strtmp + "&dt=" + Convert.ToDateTime(DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "5"));
            strtmp = strtmp + "&appid=" + DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "8").ToString();    
            strtmp = strtmp + "&dn=" + DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "2").ToString();
            if (!DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "3").ToString().Contains("Next"))
            l.ToolTip = "Appointment Date & Time: " + Convert.ToDateTime(DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "5")).ToString("dd/MM/yyyy") + " " + Convert.ToDateTime(DataBinder.Eval(row.DataItem, "Field" + Convert.ToString(intColNo) + "3")).ToString("hh:mm tt");
            //l.Attributes.Add("onclick", "window.showModalDialog('AppointmentOptions.aspx?d=" + strtmp + "', null,'status:no;dialogWidth:500px;dialogHeight:75px;dialogHide:true;help:no;scroll:no');");
            l.Attributes.Add("onclick", "options=window.open('AppointmentOptions.aspx?d=" + strtmp + "', null,'resizable=0,left=300,top=300,Width=250,Height=140,status=0,toolbars=0,menubar=0,location=no,directories=0');options.focus();");
            l.Attributes.Add("onmouseover", "javascript:fnOnMouseOver('" + l.ClientID + "');");
            l.Attributes.Add("onmouseout", "javascript:fnOnMouseOut('" + l.ClientID + "');");
        }
    }
}
