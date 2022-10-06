using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

public partial class Editor_WordProcessorInvestigationResult : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["Inv_Result"] != null)
        {
            if (common.myStr(Session["Inv_Result"]) != string.Empty)
            {
                //RTF1.Content = common.myStr(Session["Inv_Result"]);

                //RTF1.ExportToRtf();

                //string s = System.IO.File.ReadAllText(path, Encoding.UTF8);
                //RichTextBox rtb = new RichTextBox();
                //rtb.Rtf = s;
                //return rtb.Text;

                //FreeTextBox1.Text = common.myStr(Session["Inv_Result"]);

           
    FreeTextBox1.Text = ConvertToText( common.myStr(Session["Inv_Result"]));
}
        }

    }

    public string ConvertToText(string rtf)
    {
        using (RichTextBox rtb = new RichTextBox())
        {
            rtb.Rtf = rtf;
            return rtb.Text;
        }
    }


}
