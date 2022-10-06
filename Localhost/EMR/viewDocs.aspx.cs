using System;
using System.Web.UI;

public partial class EMR_viewDocs : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["FileName"] != null)
        {
            if (Session["FileName"].ToString().Contains("pdf"))
            {
                Response.ContentType = "Application/pdf";
            }
            else if (Session["FileName"].ToString().Contains("jpg") || Session["FileName"].ToString().Contains("png") || Session["FileName"].ToString().Contains("bmp"))
            {
                Response.ContentType = "image/png";
            }
             else
            {
                return;//Response.ContentType = "application/msword";
            }
            

            string filepath = Session["FileName"].ToString();
            //Response.ContentType = "Application/pdf";
            Response.WriteFile(filepath);
            Response.End();
            return;
        }
        if (Session["LocFile"] != null)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "UpdateMsg", Session["LocFile"].ToString() + "&PID=" + Request.QueryString["RegNo"].ToString()+"'", true);
        }
    }
}
