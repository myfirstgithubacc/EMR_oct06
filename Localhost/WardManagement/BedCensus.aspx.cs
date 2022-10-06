using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using System.Collections;
using Telerik.Web.UI;


public partial class WardManagement_BedCensus : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DataSet ds = new DataSet();
    clsExceptionLog objException = new clsExceptionLog();
    StringBuilder strXML = new StringBuilder();
    ArrayList coll = new ArrayList();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
       
    }
   
    private void ClearForm()
    {

       
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindControl();
            Session["GridCheckedItems"] = null;
            Session["GridCheckedIds"] = null;
        }
    }

    protected void BindControl()
    {
        ds = new DataSet();

        BaseC.ATD objadt = new BaseC.ATD(sConString);
        ds = objadt.GetWard(common.myInt(Session["FacilityId"]));
        gvFavorites.DataSource = ds.Tables[0];
        gvFavorites.DataBind();
        ViewState["Favorites"] = ds.Tables[0];



    }
    public void SaveGridCheckedItems(GridView grv, string chkControlName, string hdnUniqueId)
    {
        ArrayList arr = new ArrayList();
        ArrayList arr1 = new ArrayList();
        int index = -1;
        if (Session["GridCheckedItems"] != null)
            arr = Session["GridCheckedItems"] as ArrayList;
        if (Session["GridCheckedIds"] != null)
            arr1 = Session["GridCheckedIds"] as ArrayList;
        
        foreach (GridViewRow gvrow in grv.Rows)
        {
            index = common.myInt(grv.DataKeys[gvrow.RowIndex].Value);
            bool result = ((CheckBox)gvrow.FindControl(chkControlName)).Checked;
            int hdnId = common.myInt(((HiddenField)gvrow.FindControl(hdnUniqueId)).Value);
            if (result)
            {
                if (!arr.Contains(index))
                {
                    arr.Add(index);
                }
                else
                        if (arr.Contains(index))
                    arr.Remove(index);
                if (!arr1.Contains(hdnId))
                {
                    arr1.Add(hdnId);
                }
                else
                        if (arr1.Contains(hdnId))
                    arr1.Remove(hdnId);
            }
        }
        Session["GridCheckedItems"] = arr;
        Session["GridCheckedIds"] = arr1;
    }

    public void SetGridCheckedItems(GridView grv, string chkControlName)
    {
        if (Session["GridCheckedItems"] == null)
            return;
        ArrayList arr = new ArrayList();
        arr = Session["GridCheckedItems"] as ArrayList;
        foreach (GridViewRow gvrow in grv.Rows)
        {
            int index = common.myInt(grv.DataKeys[gvrow.RowIndex].Value);

            if (arr.Contains(index))
                (((CheckBox)gvrow.FindControl(chkControlName))).Checked = true;
            else
                (((CheckBox)gvrow.FindControl(chkControlName))).Checked = false;
        }
        
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["GridCheckedIds"] == null)
            {
                foreach (GridViewRow row in gvFavorites.Rows)
                {
                    CheckBox check = (CheckBox)row.FindControl("chkRow");
                    if (check.Checked)
                    {
                        HiddenField hdnAccessType = (HiddenField)row.FindControl("hdnId");

                        coll.Add(common.myInt(hdnAccessType.Value));
                        strXML.Append(common.setXmlTable(ref coll));
                    }
                }
            }
            else
            {
                ArrayList arr = Session["GridCheckedIds"] as ArrayList;
                for (int i = 0; i <= arr.Count - 1; i++)
                {
                    coll.Add(common.myInt(arr[i]));
                    strXML.Append(common.setXmlTable(ref coll));
                }
            }
    if(strXML.ToString()!="")
    {
                lblMessage.Text = "";
                Session["WardId"] = strXML.ToString();

        string  RURL = "PrintBedCensus.aspx?WardId=";
        if (RURL != "")
        {
            RadWindowForReport.NavigateUrl = RURL;
            RadWindowForReport.Height = 610;
            RadWindowForReport.Width = 1000;
            RadWindowForReport.Top = 10;
            RadWindowForReport.Left = 10;
            RadWindowForReport.VisibleOnPageLoad = true;
            RadWindowForReport.Modal = true;
            RadWindowForReport.VisibleStatusbar = false;
                
        }
        }
            else
            {
                lblMessage.Text = "Kindly Select ward !";
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void lnkAllergyDetails_OnClick(object o, EventArgs e)
    {
    }
    protected void lnkAlerts_OnClick(object sender, EventArgs e)
    {
    }
    protected void lnkLabHistory_OnClick(object sender, EventArgs e)
    {
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
    }
    protected void txtSearchFavrioute_OnTextChanged(object sender, EventArgs e)
    {
        SearchFavrioute(txtSearchFavrioute.Text);
    }
    protected DataTable CreateFavItemTable()
    {
        DataTable dt = new DataTable();
        try
        {

            dt.Columns.Add("StationName", typeof(string));
            dt.Columns.Add("Id", typeof(int));
          
            return dt;
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
            return dt;
        }
        finally
        {
            dt.Dispose();
        }

    }
    private void BindBlankFavGrid()
    {

        DataTable dt = CreateFavItemTable();
        DataRow dr = dt.NewRow();

        try
        {
            dr["StationName"] = string.Empty;
            dr["ID"] = 0;
         

            dt.Rows.Add(dr);
            dt.AcceptChanges();

            gvFavorites.DataSource = dt;
            gvFavorites.DataBind();


           

        }

        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
        }

    }
    protected void SearchFavrioute(string s)
    {
        DataTable dt = (DataTable)ViewState["Favorites"];

        if (dt.Rows.Count > 0)
        {
            if (s != "")
            {
                DataView dv = new DataView();
                dv = new DataView(dt);
                dv.RowFilter = "WardName like '%" + s + "%'";
                if (dv.Count > 0)
                {
                    gvFavorites.DataSource = dv.ToTable();
                    gvFavorites.DataBind();
                }
                else
                {
                    BindBlankFavGrid();

                }
            }
            else
            {
                gvFavorites.DataSource = dt;
                gvFavorites.DataBind();
            }

        }
        else
        {
            BindBlankFavGrid();

        }

    }

    protected void btnProceedFavourite_OnClick(object sender, EventArgs e)
    {
    }
    protected void gvFavorites_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Find the checkbox control in header and add an attribute
            ((CheckBox)e.Row.FindControl("chkAll")).Attributes.Add("onclick", "javascript:SelectAllFavourite('" +
                ((CheckBox)e.Row.FindControl("chkAll")).ClientID + "')");
        }
    }
    protected void gvFavorites_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
    }
    protected void gvFavorites_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            SaveGridCheckedItems(gvFavorites, "chkRow", "hdnId");
            gvFavorites.PageIndex = e.NewPageIndex;
            BindControl();
            SetGridCheckedItems(gvFavorites, "chkRow");

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnAddRequiredTemplate_Click(object sender, EventArgs e)
    {
    }
    protected void btnYes_OnClick(object sender, EventArgs e)
    {
    }
    protected void btnNo_OnClick(object sender, EventArgs e)
    {
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
    }
    protected void btnAlredyExistProceed_OnClick(object sender, EventArgs e)
    {
    }
    protected void btnAlredyExistCancel_OnClick(object sender, EventArgs e)
    {
    }
    }
