using System;
using System.Data;
using Telerik.Web.UI;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

public partial class _Default : System.Web.UI.Page
{
    //string sConString = System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = System.Configuration.ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
		  //int modid = common.myInt(Session["ModuleId"]);
    //    if (modid.Equals(3) || modid.Equals(0)) // emr Or when no module is selected        
    //        Session["BindPatientLists"] = true;
    //    else if (modid.Equals(6) || modid.Equals(30) || modid.Equals(5) || modid.Equals(34))  // admin, ward, clinical examination , OT 
    //        Session["BindPatientLists"] = false;
		
    //        if (Session["UserID"] != null)
    //        {//gvQuery

    //            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //            //DataSet ds = dl.FillDataSet(CommandType.Text, "exec uspGetInsuranceQuery " + common.myStr(Session["UserID"]));
    //            string ServiceURL = WebAPIAddress.ToString() + "api/Common/GetInsuranceQuery";
    //            APIRootClass.GetInsuranceQuery objRoot = new global::APIRootClass.GetInsuranceQuery();                
    //            objRoot.UserId = common.myInt(Session["UserId"]);
    //            WebClient client = new WebClient();
    //            client.Headers["Content-type"] = "application/json";
    //            client.Encoding = Encoding.UTF8;
    //            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
    //            string sValue = client.UploadString(ServiceURL, inputJson);
    //            sValue = JsonConvert.DeserializeObject<string>(sValue);
    //           DataSet ds = JsonConvert.DeserializeObject<DataSet>(sValue);

    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                gvQuery.DataSource = ds.Tables[0];
    //                gvQuery.DataBind();
    //                dvQuerystatus.Visible = true;
    //            }
    //            else
    //            {
    //                gvQuery.DataSource = null;
    //                gvQuery.DataBind();
    //                dvQuerystatus.Visible = false;
    //            }
    //        }
        }
        //    Response.Redirect("/Login.aspx?Logout=1", false);

        //if (Request.QueryString["RegNo"] != null)
        //{
        //    if (Request.QueryString["RegNo"] == "0")
        //    {
        //        //dvMsg.InnerHtml = "Select Patient First!";
        //        //Find Patient Docked start
        //        RadSlidingZone FPRadSlidingZone;
        //        RadSplitter RadSplitter1 = (RadSplitter)Master.FindControl("RadSplitter1");
        //        FPRadSlidingZone = (RadSlidingZone)RadSplitter1.FindControl("Radslidingzone2");
        //        FPRadSlidingZone.DockedPaneId = "rdpAppList";
        //        RadSlidingPane FPPane = (RadSlidingPane)FPRadSlidingZone.FindControl("rdpAppList");
        //        FPPane.DockOnOpen = true;
        //        //Find Patient Docked End
        //    }
        //}
        //if (Request.QueryString["LRVal"] != null)
        //{
        //    if (Request.QueryString["LRVal"] == "0")
        //    {
        //        dvMsg.InnerHtml = "You are not authorized to view records of this patient.";
        //    }
        //}
        //HttpCookie cookie = Request.Cookies.Get("menudock");
        //Menu Docked start
        //RadSlidingZone MenuRadSlidingZone;
        //RadSplitter RadSplitter1 = (RadSplitter)Master.FindControl("RadSplitter1");
        //MenuRadSlidingZone = (RadSlidingZone)RadSplitter1.FindControl("SlidingZone1");
        //MenuRadSlidingZone.DockedPaneId = "RadSlidingPane1";
        //RadSlidingPane menuPane = (RadSlidingPane)MenuRadSlidingZone.FindControl("RadSlidingPane1");
        //menuPane.DockOnOpen = true;
        //Menu Docked End
        //dvMsg.InnerHtml = cookie.Value.ToString();
    }

    protected void btnAct_Click(object sender, EventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "/QueryManagement/QueryManagement.aspx?Showx=1&IsSaveToDB=1&InvoiceId=" + (sender as Button).CommandName + "&InvoiceNo=''&Regid=0";
            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 750;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.VisibleOnPageLoad = true;
            RadWindowForNew.Modal = true;
            RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
            RadWindowForNew.VisibleStatusbar = false;

        }
        catch
        {

        }
    }
}
