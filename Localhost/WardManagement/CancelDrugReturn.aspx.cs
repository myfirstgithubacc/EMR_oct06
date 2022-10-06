using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;

public partial class WardManagement_CancelDrugReturn : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsPharmacy objPharmacy;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //done by rakesh for user authorisation start
            SetPermission();
            //done by rakesh for user authorisation end
            ViewState["ED"] = common.myStr(Request.QueryString["ED"]);
            ViewState["StoreId"] = common.myStr(Request.QueryString["StoreId"]);
            ViewState["RegId"] = common.myStr(Request.QueryString["RegId"]);
            GetPatientReturn();
        }
    }
    protected void GetPatientReturn()
    {
        objPharmacy = new BaseC.clsPharmacy(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objPharmacy.GetphrReturnItemFroCancellation(common.myInt(Session["HospitalLocationId"]),
                                    common.myInt(ViewState["StoreId"]), common.myInt(Session["FacilityId"]),
                                    common.myInt(ViewState["RegId"]), ((common.myBool(ViewState["ED"])) ? "E" : "")); //common.myStr(ViewState["ED"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                grvReturnData.DataSource = ds.Tables[0];
                grvReturnData.DataBind();
                ViewState["ItemReturn"] = ds.Tables[1];
            }
        }
        catch (Exception Ex)
        {
            Alert.ShowAjaxMsg(Ex.Message, this.Page);
        }
        finally
        {
            ds.Dispose();
        }
    }
    protected void grvReturnData_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (common.myStr(e.CommandName).ToUpper().Equals("SELECT"))
            {
                ViewState["IssueId"] = e.CommandArgument;
                if (ViewState["ItemReturn"] != null)
                {
                    DataTable ds = (DataTable)ViewState["ItemReturn"];
                    if (ds != null)
                    {
                        if (ds.Rows.Count > 0)
                        {
                            ds.DefaultView.RowFilter = "IssueId=" + e.CommandArgument;
                            if (ds.DefaultView.ToTable().Rows.Count > 0)
                            {
                                gvItemReturnDetails.DataSource = ds.DefaultView;
                                gvItemReturnDetails.DataBind();
                                if (common.myStr(ds.DefaultView[0]["returnStatus"]) != "O")
                                {
                                    btnCancel.Visible = false;
                                }
                                else
                                {
                                    //done by rakesh for user authorisation start
                                    //btnCancel.Visible = true;
                                    SetPermission(btnCancel, "C", true);
                                    //done by rakesh for user authorisation end
                                }
                            }
                        }
                    }
                }
            }
            else if (common.myStr(e.CommandName).ToUpper().Equals("PRINT"))
            {
                RadWindowForNew.NavigateUrl = "~/Pharmacy/Reports/IPSaleDocReport.aspx?IssueId=" + common.myInt(e.CommandArgument) + "&SetupId=203&UseFor=DOC&IssueReturn=R&StoreId=" + common.myInt(Request.QueryString["StoreId"]);
                RadWindowForNew.Height = 500;
                RadWindowForNew.Width = 900;
                RadWindowForNew.Top = 10;
                RadWindowForNew.Left = 10;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
            }
        }
        catch (Exception Ex)
        {
            //Alert.ShowAjaxMsg(Ex.Message, this.Page);
        }

    }
    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        objPharmacy = new BaseC.clsPharmacy(sConString);
        //string msg = objPharmacy.CancelItemReturn(common.myInt(Session["HospitalLocationId"]),
        //                         common.myInt(Session["FacilityId"]), common.myInt(ViewState["IssueId"]),
        //                         common.myInt(Session["UserId"]), ((common.myBool(ViewState["ED"])) ? "E" : "")); //common.myStr(ViewState["ED"]));
        string msg = objPharmacy.CancelItemReturn(common.myInt(Session["HospitalLocationId"]),
                               common.myInt(Session["FacilityId"]), common.myInt(ViewState["IssueId"]),
                               common.myInt(Session["UserId"]), ((common.myBool(ViewState["ED"])) ? "E" : common.myStr(Session["OPIP"]))); //common.myStr(ViewState["ED"]));

        GetPatientReturn();
        gvItemReturnDetails.DataSource = null;
        gvItemReturnDetails.DataBind();
    }

    //done by rakesh for user authorisation start
    string masterPageActive = string.Empty;
    private void SetPermission()
    {

        if (common.myStr(Request.QueryString["MASTER"]).Equals("No"))
            masterPageActive = "?Master=No";
        else
            masterPageActive = "?Master=Yes";


        UserAuthorisations ua1 = new UserAuthorisations();
        ua1.DisableEnableControl(btnCancel, false);

        if (ua1.CheckPermissions("C", "/Pharmacy/SaleIssue/IPDItemReturn.aspx?Master=No"))
        {
            ua1.DisableEnableControl(btnCancel, true);
        }
        ua1.Dispose();
    }
    private void SetPermission(Button btnID, string mode, bool action)
    {
        UserAuthorisations ua1 = new UserAuthorisations();
        ua1.DisableEnableControl(btnID, false);

        if (common.myStr(Request.QueryString["MASTER"]).Equals("No"))
            masterPageActive = "?Master=No";
        else
            masterPageActive = "?Master=Yes";

        if (ua1.CheckPermissions(mode, "/Pharmacy/SaleIssue/IPDItemReturn.aspx?Master=No"))
        {
            ua1.DisableEnableControl(btnID, action);
        }
        else
        {
            ua1.DisableEnableControl(btnID, !action);
        }
        ua1.Dispose();
    }



    protected void gvItemReturnDetails_OnRowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblERReturnAckBy = (Label)e.Row.FindControl("lblERReturnAckBy");
                if (common.myLen(lblERReturnAckBy.Text).Equals(0))
                {
                    Label lblAckQty = (Label)e.Row.FindControl("lblAckQty");
                    lblAckQty.Visible = false;
                }
            }
        }
        catch (Exception Ex)
        {   
        }
    }


}
