using System;
using System.Collections;
using System.Collections.Generic;  
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using Telerik.Web.UI;
using Resources;
using System.Configuration;
using System.Collections.Generic;

public partial class EMRBILLING_Popup_AddServices : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ParseData bc = new BaseC.ParseData();
    BaseC.Hospital baseHc;
    BaseC.EMRMasters bMstr;
    BaseC.EMRBilling.clsOrderNBill BaseBill;
    Hashtable hshInput;
    Hashtable hshOutput;
    DAL.DAL objDl;
    BaseC.EMROrders objEMROrders;
    BaseC.clsLISMaster objLISMaster;
    BaseC.clsLabRequest objLabRequest;
    private const int ItemsPerRequest = 10;
    public string strxmlstring = "";
    public string Getxmlstring
    {
        get { return strxmlstring; }
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["PT"]) == "IPEMR")
        {
            this.MasterPageFile = "~/Include/Master/EMRMaster.master";
        }
    }

    private void BindOtherResource()
    {
        try
        {
            BaseC.Surgery objDoctorClassification = new BaseC.Surgery(sConString);
           DataSet   ds = new DataSet();
            ds = objDoctorClassification.GetDoctorClassification((common.myInt(Session["HospitalLocationID"].ToString())));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataView dv = new DataView(ds.Tables[0]);
                dv.RowFilter = "Type IN ('SR','AN','OD')";
                foreach (DataRow dr in dv.ToTable().Rows)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = (string)dr["Name"];
                    item.Value = dr["ID"].ToString();
                    item.Attributes.Add("Type", dr["Type"].ToString());
                    this.radCmbDoctorClassification.Items.Add(item);
                    item.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
           // lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }



    protected void ddlResourceName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
 
    }


    public void BindSurgeronName()
    {
        ddlResourceName.Visible = true;
        DataSet ds = new DataSet();
        if (ViewState["EmpClassi"] == null)
        {
            BaseC.RestFulAPI objCommon = new BaseC.RestFulAPI(sConString);

            ds = objCommon.GetEmployeeClassification(common.myInt(Session["HospitalLocationId"]), 0, 0, common.myInt(Session["FacilityId"]),0,0);
            ViewState["EmpClassi"] = ds;
        }
        else
            ds = (DataSet)ViewState["EmpClassi"];
        DataView dvF = new DataView(ds.Tables[0]);
        dvF.RowFilter = "Type IN ('D','AN','SR')";
        if (dvF.ToTable().Rows.Count > 0)
        {
            ddlResourceName.Items.Clear();
            ddlResourceName.DataSource = dvF.ToTable();
            ddlResourceName.DataValueField = "EmployeeId";
            ddlResourceName.DataTextField = "EmployeeName";
            ddlResourceName.DataBind();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        baseHc = new BaseC.Hospital(sConString);
        bMstr = new BaseC.EMRMasters(sConString);
        BaseBill = new BaseC.EMRBilling.clsOrderNBill(sConString);
        objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            if (!IsPostBack)
            {
                

                hdnXmlString.Value = "";
                //HiddenField hdnxmlString = (HiddenField)PreviousPage.FindControl("hdnxmlString");
                //Cache.Remove("OPServicesInv_" + common.myStr(Session["UserId"]) + "_" + common.myStr(txtEncId.Text));   

                hdnServiceID.Value = common.myStr(Request.QueryString["ServiceId"]);    
                BindGrid(); 
                BindOtherResource();
                BindSurgeronName(); 
              
            }

            radCmbDoctorClassification.Attributes.Add("onkeypress", "return clickEnterInGrid('" + txtPerc.ClientID + "', event)");
            txtPerc.Attributes.Add("onkeypress", "return clickEnterInGrid('" + cmdAddtoGrid.ClientID + "', event)");
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    protected void radCmbDoctorClassification_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txtPerc.Text = "";  
    }
    
   
 
    
    #region  PageFunctions
 
    

    
   
   
 
    protected void BindGrid()
    {
        try
        {
         
            gvShare.DataSource = CreateShareTable();
            gvShare.DataBind();  
   
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = ex.Message;
            objException.HandleException(ex);
        }
    }



    protected void gvShare_OnRowCommand(object sender, GridViewCommandEventArgs  e)
    {

        if (e.CommandName == "Del")
        {
            GridViewRow dr=(GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            int  rowindex=dr.RowIndex;
            DataTable dt=(DataTable)ViewState["ShareData"];
            dt.Rows.RemoveAt(rowindex);  
            dt.AcceptChanges();  
            gvShare.DataSource=dt;
            gvShare.DataBind(); 
            ViewState["ShareData"]=dt; 
 

 
        }
    }

    protected DataTable CreateShareTable()
    {
       

        DataTable dt = new DataTable();
        dt.Columns.Add("SNo");
        dt.Columns["SNo"].AutoIncrement = true;
        dt.Columns["SNo"].AutoIncrementSeed = 0;
        dt.Columns["SNo"].AutoIncrementStep = 1;
        dt.Columns.Add("ServiceId");
        dt.Columns.Add("SergeonType");
        dt.Columns.Add("Perc");
        dt.Columns.Add("SurgeonID");
        dt.Columns.Add("SurgeonName");
        DataRow dr = dt.NewRow();
        dr["ServiceId"] = 0;
        dr["SergeonType"] = "";
        dr["Perc"] = 0;
        dr["SurgeonID"] = 0;
        dr["SurgeonName"] = "";
       
        dt.Rows.Add(dr);
        ViewState["ShareData"] = dt;  
        
 

        return dt;

    }




 
    #endregion
    protected void cmdAddtoGrid_OnClick(object sender, EventArgs e)
    {
        //UpdateDataTable();

        if (common.myInt(txtPerc.Text) >= 100)
        {
            Alert.ShowAjaxMsg("Percentage cannot be 100%", this.Page);
            return;
        }



        DataTable objdt = UpdateShareDataTable();

        gvShare.DataSource = objdt;
        gvShare.DataBind();

        //gvshare2.DataSource = objdt; 
        //gvshare2.DataBind();

        ViewState["ShareData"] = objdt; 
  
        
        //if (common.myStr(ddlService.SelectedValue) == "")
        //{
        //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //    lblMessage.Text = "Service does not exist, Please select from list !";
        //    Alert.ShowAjaxMsg("Service does not exist, Please select from list !", Page.Page);
        //    return;
        //}

        //string[] stringSeparators_ShowDia = new string[] { "##" };
        //string[] serviceId = common.myStr(ddlService.SelectedValue).Split(stringSeparators_ShowDia, StringSplitOptions.None);

        //if (common.myStr(ViewState["OP_IP"]) == "I")
        //{
        //    if (common.myInt(ViewState["EncId"]) > 0)
        //    {
        //        DataSet datas = new DataSet();
        //        hshInput = new Hashtable();
        //        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //        hshInput.Add("@HospitalLocationId", common.myInt(Session["HospitalLocationId"]));
        //        hshInput.Add("@FacilityId", common.myInt(Session["FacilityId"]));
        //        hshInput.Add("@ServiceId", common.myInt(serviceId[0]));
        //        hshInput.Add("@EncounterId", common.myInt(ViewState["EncId"]));
        //        hshInput.Add("@RegistrationId", common.myInt(ViewState["Regid"]));
        //        datas = dl.FillDataSet(CommandType.Text, "Select SD.ServiceId, I.ServiceName,ISNULL(EM.FirstName,'') + ' '  + ISNULL(EM.MiddleName,'') + ' ' + ISNULL(EM.LASTNAME,'') AS EnteredBy, dbo.GetDateFormatUTC(s.EncodedDate,'DT', F.TimeZoneOffSetMinutes) OrderDate FROM ServiceOrderMain S  INNER JOIN ServiceOrderDetail SD ON S.Id = SD.OrderId  INNER JOIN ItemOfService I ON SD.ServiceId = I.ServiceId  INNER JOIN FacilityMaster F ON S.FacilityId = F.FacilityID  INNER JOIN Users US ON S.EncodedBy=US.ID INNER JOIN Employee EM ON EM.ID=US.EmpID WHERE ISNULL(S.EncounterId,'') =  @EncounterId  AND S.RegistrationId = @RegistrationId  AND CONVERT(VARCHAR,S.OrderDate,111) = CONVERT(VARCHAR,GETUTCDATE(),111)  AND S.HospitalLocationId = @HospitalLocationId  AND S.FacilityId = @FacilityId  AND SD.ServiceId = @ServiceId And S.ACTIVE = 1 AND SD.ACTIVE = 1 ", hshInput);
        //        if (datas.Tables.Count > 0)
        //        {
        //            if (datas.Tables[0].Rows.Count > 0)
        //            {
        //                lblServiceName.Text = common.myStr(datas.Tables[0].Rows[0]["ServiceName"]);
        //                lblEnteredBy.Text = common.myStr(datas.Tables[0].Rows[0]["EnteredBy"]);
        //                lblEnteredOn.Text = common.myStr(datas.Tables[0].Rows[0]["OrderDate"]);
        //                ViewState["DuplicateService"] = "1";
        //                btnYes_OnClick(sender, e);

        //            }
        //            else
        //            {

        //                btnYes_OnClick(sender, e);
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    btnYes_OnClick(sender, e);
        //}

    }
   
    protected void ibtSave_OnClick(object sender, EventArgs e)
    {
        DataTable objdt=(DataTable) ViewState["ShareData"];
        StringBuilder Xmldt = new StringBuilder();
        ArrayList coll = new ArrayList(); 
        foreach (DataRow dr in objdt.Rows )
        {
            coll.Add(dr["ServiceId"]);
            coll.Add(dr["SergeonType"]);
            coll.Add(dr["Perc"]);
            coll.Add(dr["SurgeonID"]); 
          
            Xmldt.Append(common.setXmlTable(ref coll));
        }
        
        hdnXmlString.Value = Xmldt.ToString();


       // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "window.close();", true);
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
    }
    
   
     //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
  
    protected void btnAlredyExist_OnClick(object sender, EventArgs e)
    {
        
    }
   

 
   

 
    private DataTable UpdateShareDataTable()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["ShareData"];
        foreach (GridViewRow gitem in gvShare.Rows)
        {
            Label lblSergeonType = (Label)gitem.FindControl("lblSergeonType");
            Label lblID = (Label)gitem.FindControl("lblID");   
            Label lblPerc = (Label)gitem.FindControl("lblPerc");
            int maxId = 0;
            if (dt.Rows.Count > 0)
            {
                if (common.myStr(dt.Rows[0]["SergeonType"]) == "") //If serviceid = 0 then remove row
                {
                    dt.Rows.Clear();
                }
                else
                {
                    DataView dv = new DataView(dt);
                    dv.Sort = "Sno Desc";
                    maxId = common.myInt(dv[0]["Sno"]);
                }
            }
            DataView objdv = new DataView(dt);
            objdv.RowFilter = "SergeonType=" + "'" + radCmbDoctorClassification.SelectedItem.Attributes["Type"].ToString() + "'";
            if (objdv.ToTable().Rows.Count>0)
            {
               Alert.ShowAjaxMsg("Record Already Added", this.Page);
              
            }
            else
            {
                DataRow dr = dt.NewRow();
                dr["Sno"] = maxId + 1;
                dr["SergeonType"] = radCmbDoctorClassification.SelectedItem.Attributes["Type"].ToString();
                dr["Perc"] =common.myInt(txtPerc.Text);
                dr["ServiceId"] = hdnServiceID.Value;
                dr["SurgeonID"] = ddlResourceName.SelectedValue;
                dr["SurgeonName"] = ddlResourceName.Text;
                dt.Rows.Add(dr);
                dt.AcceptChanges();  

            }
        }
       
        return dt;
    }



}
