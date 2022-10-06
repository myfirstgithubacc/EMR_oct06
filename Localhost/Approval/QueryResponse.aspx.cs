using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Approval_QueryResponse : System.Web.UI.Page
{
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Request.QueryString["Qid"] != "" && Request.QueryString["Qid"] != null)
            {
                ViewState["Qid"] = Request.QueryString["Qid"].ToString();
                ViewState["UHID"] = Request.QueryString["UHID"].ToString();       
               
            }
            BindgvQuery();
            //dvx
        }


    }
    protected void btnResponse_Click(object sender, EventArgs e)
    {
        try
        {
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //DataSet ds = dl.FillDataSet(CommandType.Text, "exec UspGetGfsQuery @QID=" + (sender as Button).CommandName);
            ViewState["Qid"] = common.myInt((sender as Button).CommandName);
            DataSet ds = new DataSet();
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/Common/GfsQuery";
            APIRootClass.GfsCountManagement objRoot = new global::APIRootClass.GfsCountManagement();
            objRoot.Qid = common.myInt(ViewState["Qid"]);
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);
            GvxConversation.DataSource = ds.Tables[0];
            GvxConversation.DataBind();
            btnSave.CommandName = (sender as Button).CommandName;
            lblQuery.Text = ((sender as Button).NamingContainer as GridViewRow).Cells[0].Text + " </br>" + ((sender as Button).NamingContainer as GridViewRow).Cells[1].Text + "</br>" + ((sender as Button).NamingContainer as GridViewRow).Cells[3].Text;
            lblQueryText.Text = ((sender as Button).NamingContainer as GridViewRow).Cells[2].Text;

            dvx.Visible = true;
            dvgvQuery.Visible = false;

        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtReply.Text.Length > 10)
            {
                int Qid = common.myInt(btnSave.CommandName);
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string ServiceURL = WebAPIAddress.ToString() + "api/Common/SaveGfsQuery";
                APIRootClass.GfsQueryInputs objRoot = new global::APIRootClass.GfsQueryInputs();
                objRoot.Qid = Qid;
                objRoot.formID = Qid.ToString();
                objRoot.Query = txtReply.Text.Replace("'", "''");
                objRoot.QueryBy =common.myInt(Session["UserID"]);
                objRoot.QueryTo = 0;

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);

                //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //    dl.ExecuteNonQuery(System.Data.CommandType.Text, "UspSaveGfsQuery @Isrespone=1, @QID =" + Qid + ",@formID=" + Qid.ToString() + ",@Query='" + txtReply.Text.Replace("'", "''") + "',@QueryBy=" + Session["UserID"] + ",@QueryTo=0");

                Alert.ShowAjaxMsg("Success!", this);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                dvx.Visible = false;
                dvgvQuery.Visible = true;
                BindgvQuery();
            }
            else
            {
                Alert.ShowAjaxMsg("Please Enter Justification", this);
            }
            
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        dvx.Visible = false;
        dvgvQuery.Visible = true;
    }

    protected void BindgvQuery()
    {
        DataSet ds = new DataSet();
        string ServiceURL = WebAPIAddress.ToString() + "api/Common/GetGfsQueryDoc";
        DataView dv = new DataView();
        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;
        APIRootClass.GfsCountManagement objRoot = new global::APIRootClass.GfsCountManagement();
        objRoot.Employeeid = common.myInt(Session["EmployeeId"]);
        string OutResult = string.Empty;
        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        string sValue = client.UploadString(ServiceURL, inputJson);
        sValue = JsonConvert.DeserializeObject<string>(sValue);
        ds = JsonConvert.DeserializeObject<DataSet>(sValue);
        dv= new DataView(ds.Tables[0]);
        if (common.myStr(ViewState["UHID"]) != "")
        {
            dv.RowFilter = "ISNULL(UHID,0)= '" + ViewState["UHID"] + "' and ID=" + ViewState["Qid"];
            gvQuery.DataSource = dv.ToTable();
            gvQuery.DataBind();          

        }
        else
        {
            gvQuery.DataSource = dv.ToTable();
            gvQuery.DataBind();
        }
        
    }
}