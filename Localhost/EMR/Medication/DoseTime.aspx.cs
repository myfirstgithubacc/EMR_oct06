using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Text;
using System.IO;
using Telerik.Web.UI;

public partial class EMR_Medication_DoseTime : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["LastInitialDate"] = null;
            ViewState["DrugAdministeredDetails"] = null;
            ViewState["DrugExcludedDetails"] = null;
          
            BindFrequencyDoseTimeDetails();
            lblItemName.Text = "Item Name - " + common.myStr(Session["ItemName"]);

        }


        dtExtendDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);

        if (common.myLen(hdnDoseTime.Value) > 0)
        {
            tblDoseExtend.Visible = true;
            dtExtendDate.SelectedDate = null;

            if (common.myInt(hdnColIdx.Value).Equals(1))
            {
                dtExtendDate.MinDate = common.myDate(ViewState["LastInitialDate"]).AddDays(1);
                dtExtendDate.MaxDate = common.myDate(ViewState["LastInitialDate"]).AddDays(1);
                dtExtendDate.SelectedDate = common.myDate(ViewState["LastInitialDate"]).AddDays(1);
            }
            else
            {
                dtExtendDate.MinDate = common.myDate(ViewState["LastInitialDate"]);
                dtExtendDate.MaxDate = common.myDate(ViewState["LastInitialDate"]);
                dtExtendDate.SelectedDate = common.myDate(ViewState["LastInitialDate"]);
            }

            if (Convert.ToDateTime(dtExtendTime.SelectedDate) == Convert.ToDateTime(hdnDoseTime.Value))
            {
                dtExtendTime.SelectedDate = Convert.ToDateTime(hdnDoseTime.Value);
            }
        }
    }

    private void BindFrequencyDoseTimeDetails()
    {
        int totaldays = common.myInt(Request.QueryString["Day"]);
        DataSet ds = new DataSet();
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        BaseC.ICM objIcm = new BaseC.ICM(sConString);

        try
        {
            ds = objIcm.getDrugAdministeredOrNotWithExclude(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                             common.myInt(Request.QueryString["RegId"]), common.myInt(Request.QueryString["EncId"]),
                                             common.myInt(Request.QueryString["ItemId"]), common.myInt(Request.QueryString["FrequencyId"]),
                                             common.myInt(Request.QueryString["IndentId"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["DrugAdministeredDetails"] = ds.Tables[0];
            }

            if (ds.Tables.Count > 1)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ViewState["DrugExcludedDetails"] = ds.Tables[1];
                }
            }

            ds = new DataSet();
            //ds = emr.GetFrequencyDoseTimeDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
            //                                    common.myInt(Request.QueryString["FrequencyId"]), totaldays, common.myStr(Request.QueryString["InitialDate"]));
            ds = emr.GetFrequencyDoseTimeDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Request.QueryString["FrequencyId"]),
                   totaldays);

            ds = mergedExcludedDoseDetails(ds.Copy());

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["FrequencyDoseTime"] = ds.Tables[0];

                ViewState["LastInitialDate"] = common.myDate(ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1]["Date"]);

                gvData.DataSource = ds.Tables[0];
                gvData.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            emr = null;
            objIcm = null;
        }
    }


    //void BindFrequencyDoseTimeDetails()
    //{
    //    int totaldays = common.myInt(Request.QueryString["Day"]);
    //    DataSet ds = new DataSet();
    //    BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
    //    try
    //    {
    //        //if (common.myStr(Request.QueryString["FrequencyData"]) != "")
    //        //{
    //        //    string xmlFrequencySchema = common.myStr(Request.QueryString["FrequencyData"]);
    //        //    StringReader srFrequency = new StringReader(xmlFrequencySchema);
    //        //    ds.ReadXml(srFrequency);
    //        //}
    //        //else
    //        //{
    //            ds = emr.GetFrequencyDoseTimeDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Request.QueryString["FrequencyId"]),
    //                 totaldays);
    //        //}
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            ViewState["FrequencyDoseTime"] = ds.Tables[0];
    //            gvData.DataSource = ds.Tables[0];
    //            gvData.DataBind();
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        clsExceptionLog objException = new clsExceptionLog();
    //        objException.HandleException(Ex);
    //    }
    //    finally
    //    {
    //        ds.Dispose();
    //    }
    //}
    protected void btnCloseW_Click(object sender, EventArgs e)
    {
        DataTable dtgrd = new DataTable();
        try
        {
            dtgrd.Columns.Add("FrequencyId");
            dtgrd.Columns.Add("ItemId");
            dtgrd.Columns.Add("DoseTime");
            dtgrd.Columns.Add("FrequencyDetailId");
            dtgrd.Columns.Add("DoseEnable");
            for (int i = 0; i < gvData.Rows.Count; i++)
            {
                for (int Col = 1; Col < common.myInt(ViewState["TotalCount"]); Col++)
                {
                    DataRow dRow = dtgrd.NewRow();
                    string sDate = gvData.Rows[i].Cells[0].Text;

                    RadTimePicker dtp = (RadTimePicker)gvData.Rows[i].Cells[Col].FindControl("dtp" + Col);
                    CheckBox chk = (CheckBox)gvData.Rows[i].Cells[Col].FindControl("chk" + Col);
                    HiddenField hdn = (HiddenField)gvData.Rows[i].Cells[Col].FindControl("hdn" + Col);

                    if (dtp.SelectedDate.Value.ToString("HH:mm") == "01:01" && !chk.Checked)
                    {
                        Alert.ShowAjaxMsg("Please change dosage time !", Page);
                        dtp.Focus();
                        return;
                    }

                    dRow["FrequencyId"] = common.myStr(Request.QueryString["FrequencyId"]);
                    dRow["ItemId"] = common.myStr(Request.QueryString["ItemId"]);
                    dRow["DoseTime"] = common.myDate(sDate).ToString("yyyy/MM/dd") + " " + dtp.SelectedDate.Value.ToString("HH:mm");
                    dRow["FrequencyDetailId"] = hdn.Value;
                    if (chk.Checked)
                    {
                        dRow["DoseEnable"] = 0;
                    }
                    else
                    {
                        dRow["DoseEnable"] = 1;
                    }
                    dtgrd.Rows.Add(dRow);
                }
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(dtgrd.Copy());
            System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.
            System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
            ds.WriteXml(writer);
            string xmlSchema = writer.ToString();
            hdnXmlString.Value = xmlSchema;
            ds.Dispose();

            ArrayList collFre = new ArrayList();
            StringBuilder strXMLFre = new StringBuilder();
            if (common.myStr(Request.QueryString["From"]) == "Ward")
            {
                for (int i = 0; i < gvData.Rows.Count; i++)
                {
                    for (int Col = 1; Col < common.myInt(ViewState["TotalCount"]); Col++)
                    {
                        DataRow dRow = dtgrd.NewRow();
                        string sDate = gvData.Rows[i].Cells[0].Text;

                        RadTimePicker dtp = (RadTimePicker)gvData.Rows[i].Cells[Col].FindControl("dtp" + Col);
                        CheckBox chk = (CheckBox)gvData.Rows[i].Cells[Col].FindControl("chk" + Col);
                        HiddenField hdn = (HiddenField)gvData.Rows[i].Cells[Col].FindControl("hdn" + Col);

                       
                        collFre.Add(common.myStr(Request.QueryString["ItemId"]));//ItemId INT,
                        collFre.Add(common.myStr(Request.QueryString["FrequencyId"]));//FrequencyId INT, 
                        collFre.Add(common.myStr(common.myDate(sDate).ToString("yyyy/MM/dd") + " " + dtp.SelectedDate.Value.ToString("HH:mm")));//DoseTime String 
                        collFre.Add(common.myStr(hdn.Value));//FrequencyDetailId INT
                        if (chk.Checked)
                        {
                            collFre.Add(0);//FrequencyDetailId INT                          
                        }
                        else
                        {
                            collFre.Add(1);//FrequencyDetailId INT                           
                        }
                       
                        strXMLFre.Append(common.setXmlTable(ref collFre));

                       
                       
                    }
                }
                //for (int fr = 0; fr < dtgrd.Rows.Count; fr++)
                //{
                //    string FrequencyId = common.myStr(dtgrd.Rows[fr]["FrequencyId"]);
                //    int ItemId = common.myInt(dtgrd.Rows[fr]["ItemId"]);
                //    string DoseTime = common.myStr(dtgrd.Rows[fr]["DoseTime"]);
                //    int FrequencyDetailId = common.myInt(dtgrd.Rows[fr]["FrequencyDetailId"]);
                //    bool DoseEnable = common.myBool(dtgrd.Rows[fr]["DoseEnable"]);
                //    collFre.Add(common.myInt(ItemId));//ItemId INT,
                //    collFre.Add(common.myInt(FrequencyId));//FrequencyId INT, 
                //    collFre.Add(common.myStr(DoseTime));//DoseTime String 
                //    collFre.Add(common.myStr(FrequencyDetailId));//FrequencyDetailId INT
                //    collFre.Add(DoseEnable);//FrequencyDetailId INT
                //    strXMLFre.Append(common.setXmlTable(ref collFre));
                //}
                BaseC.ICM icm = new BaseC.ICM(sConString);
                string strMsg = icm.SaveDosageFromWard(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                common.myInt(Request.QueryString["RegId"]), common.myInt(Request.QueryString["EncId"]),
                                common.myInt(Request.QueryString["ItemId"]), common.myInt(Session["UserId"]),
                                common.myInt(Request.QueryString["IndentId"]), strXMLFre.ToString(), common.myInt(Request.QueryString["FrequencyId"]),
                                0, false);
                if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = strMsg;
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = strMsg;
                    return;
                }


            }

        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            dtgrd.Dispose();
        }
        //else
        //{
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
        //}
        
    }
    protected void btnSaveDoseExtend_OnClick(object sender, EventArgs e)
    {
        DataTable dtgrd = new DataTable();
        BaseC.ICM icm = new BaseC.ICM(sConString);
        ArrayList coll = new ArrayList();
        StringBuilder strXML = new StringBuilder();
        try
        {
            if (common.myInt(hdnFrequencyDetailId.Value).Equals(0))
            {
                Alert.ShowAjaxMsg("Please select dose time whitch will be change!", this.Page);
                return;
            }

            if (!common.myBool(hdnDoseEnable.Value))
            {
                Alert.ShowAjaxMsg("Before extend please save data for excluded dose!", this.Page);
                return;
            }

            if (dtExtendTime.SelectedDate == null)
            {
                Alert.ShowAjaxMsg("Please select dose time!", this.Page);
                return;
            }

            coll.Add(common.myStr(Request.QueryString["ItemId"]));//ItemId INT,
            coll.Add(common.myStr(Request.QueryString["FrequencyId"]));//FrequencyId INT, 
            coll.Add(common.myStr(common.myDate(dtExtendDate.SelectedDate).ToString("yyyy/MM/dd") + " " + dtExtendTime.SelectedDate.Value.ToString("HH:mm")));//FrequencyTime SMALLDATETIME 
            coll.Add(common.myInt(hdnFrequencyDetailId.Value));//FrequencyDetailId INT
            coll.Add(1);//DoseEnable BIT

            strXML.Append(common.setXmlTable(ref coll));

            string strMsg = icm.SaveDosageFromWard(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                        common.myInt(Request.QueryString["RegId"]), common.myInt(Request.QueryString["EncId"]),
                                        common.myInt(Request.QueryString["ItemId"]), common.myInt(Session["UserId"]),
                                        common.myInt(Request.QueryString["IndentId"]), strXML.ToString(),
                                        common.myInt(Request.QueryString["FrequencyId"]), 0, true);

            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                hdnFrequencyDetailId.Value = string.Empty;
                hdnDoseTime.Value = string.Empty;
                tblDoseExtend.Visible = false;

                dtExtendTime.SelectedDate = null;

                ViewState["LastInitialDate"] = null;
                ViewState["DrugAdministeredDetails"] = null;
                ViewState["DrugExcludedDetails"] = null;

                BindFrequencyDoseTimeDetails();

                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = strMsg;
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = strMsg;
                return;
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
        }
    }
    protected void gvData_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        int ColumnsCount = e.Row.Cells.Count;
        ViewState["TotalCount"] = e.Row.Cells.Count;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (ViewState["FrequencyDoseTime"] != null)
            {
                RadTimePicker Time = new RadTimePicker();
                CheckBox chk = new CheckBox();
                LinkButton lnkBtn = new LinkButton();
                HiddenField hdn = new HiddenField();

                DataView dvDrugAdministeredDetails = new DataView();
                DataView dvDrugExcludedDetails = new DataView();
                DataTable dt = (DataTable)ViewState["FrequencyDoseTime"];
                DataTable dtDrugAdministeredDetails = new DataTable();
                DataTable dtDrugExcludedDetails = new DataTable();

                try
                {
                    if (ViewState["DrugAdministeredDetails"] != null)
                    {
                        dtDrugAdministeredDetails = ((DataTable)ViewState["DrugAdministeredDetails"]);
                    }

                    if (ViewState["DrugExcludedDetails"] != null)
                    {
                        dtDrugExcludedDetails = (DataTable)ViewState["DrugExcludedDetails"];
                    }

                    for (int colIdx = 1; colIdx < ColumnsCount; colIdx++)
                    {
                        if (!common.myStr(dt.Rows[e.Row.RowIndex]["DoseTime" + colIdx]).Equals(string.Empty))
                        {
                            Time = new RadTimePicker();
                            Time.ID = "dtp" + colIdx;
                            Time.Width = Unit.Pixel(100);
                            e.Row.Cells[colIdx].Controls.AddAt(0, Time);

                            if (common.myStr(dt.Rows[e.Row.RowIndex]["DoseTime" + colIdx]).Contains('/'))
                            {
                                Time.SelectedDate = common.myDate(dt.Rows[e.Row.RowIndex]["DoseTime" + colIdx].ToString().Split('/')[0]);
                            }

                            chk = new CheckBox();
                            chk.ID = "chk" + colIdx;
                            chk.Width = Unit.Pixel(30);
                            e.Row.Cells[colIdx].Controls.AddAt(1, chk);

                            hdn = new HiddenField();
                            hdn.ID = "hdn" + colIdx;
                            e.Row.Cells[colIdx].Controls.AddAt(2, hdn);

                            if (common.myStr(dt.Rows[e.Row.RowIndex]["DoseTime" + colIdx]).Contains('/'))
                            {
                                hdn.Value = common.myStr(dt.Rows[e.Row.RowIndex]["DoseTime" + colIdx].ToString().Split('/')[1]);
                            }

                            lnkBtn = new LinkButton();
                            lnkBtn.Visible = false;

                            if (dtDrugExcludedDetails != null)
                            {
                                if (dtDrugExcludedDetails.Rows.Count > 0)
                                {
                                    dvDrugExcludedDetails = dtDrugExcludedDetails.Copy().DefaultView;

                                    dvDrugExcludedDetails.RowFilter = "DrugScheduleDate='" + common.myStr(dt.Rows[e.Row.RowIndex]["Date"]) + "' AND FrequencyDetailId=" + common.myInt(hdn.Value) + " AND DoseEnable=0";

                                    if (dvDrugExcludedDetails.ToTable().Rows.Count > 0)
                                    {
                                        chk.Checked = true;
                                        lnkBtn.Visible = true;
                                    }

                                    dvDrugExcludedDetails.RowFilter = string.Empty;

                                }
                            }

                            if (dtDrugAdministeredDetails != null)
                            {
                                if (dtDrugAdministeredDetails.Rows.Count > 0)
                                {
                                    dvDrugAdministeredDetails = dtDrugAdministeredDetails.Copy().DefaultView;

                                    dvDrugAdministeredDetails.RowFilter = "DrugScheduleDate='" + common.myStr(dt.Rows[e.Row.RowIndex]["Date"]) + "' AND FrequencyDetailId=" + common.myInt(hdn.Value) + " AND IsMedicationAdminister=1 ";

                                    if (dvDrugAdministeredDetails.ToTable().Rows.Count > 0)
                                    {
                                        Time.Enabled = false;
                                        chk.Enabled = false;
                                        lnkBtn.Visible = false;
                                        e.Row.Cells[colIdx].BackColor = System.Drawing.Color.Gray;
                                    }

                                    dvDrugAdministeredDetails.RowFilter = string.Empty;
                                }
                            }

                            lnkBtn.ID = "lnkBtn" + colIdx;
                            lnkBtn.Text = "D";
                            lnkBtn.ToolTip = "Change dose date and time...";
                            lnkBtn.Attributes.Add("onclick", "return doseExtend('" + hdn.Value + "','" + common.myStr(Time.SelectedDate) + "','" + chk.Checked + "','" + colIdx + "');");
                            lnkBtn.Width = Unit.Pixel(20);
                            e.Row.Cells[colIdx].Controls.AddAt(3, lnkBtn);

                        }
                    }

                    //if (dt.Rows.Count > 0)
                    //{
                    //    if (e.Row.RowIndex.Equals(dt.Rows.Count - 1)) //At last row of the grid
                    //    {
                    //        DateTime dtLastInitialDate = common.myDate(ViewState["LastInitialDate"]);

                    //        hdn = new HiddenField();
                    //        hdn.ID = "hdn0";
                    //        hdn.Value = common.myStr(dt.Rows[e.Row.RowIndex]["Date"].ToString());

                    //        RadDatePicker rdp0 = new RadDatePicker();
                    //        rdp0.DateInput.DateFormat = "dd/MM/yyyy";
                    //        rdp0.MinDate = common.myDate(dtLastInitialDate);
                    //        rdp0.MaxDate = common.myDate(dtLastInitialDate).AddDays(1);
                    //        rdp0.ID = "rdp0";
                    //        rdp0.SelectedDate = common.myDate(hdn.Value);
                    //        rdp0.Width = Unit.Pixel(100);

                    //        e.Row.Cells[0].Controls.AddAt(0, rdp0);
                    //        e.Row.Cells[0].Controls.AddAt(1, hdn);
                    //    }
                    //}
                }
                catch
                {
                }
                finally
                {
                    dt.Dispose();
                    dtDrugAdministeredDetails.Dispose();
                    dtDrugExcludedDetails.Dispose();
                    dvDrugAdministeredDetails.Dispose();
                    dvDrugExcludedDetails.Dispose();
                }
            }
        }
    }
    protected void gvData_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        int ColumnsCount = e.Row.Cells.Count ;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
           RadTimePicker Time;
            CheckBox chk;
            if (ViewState["FrequencyDoseTime"] != null)
            {
                DataTable dt = (DataTable)ViewState["FrequencyDoseTime"];
                for (int i = 1; i < ColumnsCount; i++)
                {
                    if (common.myStr(dt.Rows[e.Row.RowIndex]["DoseTime" + i]) != "")
                    {
                        Time = new RadTimePicker();
                        Time.ID = "dtp" + i;
                        Time.Width = Unit.Pixel(100);
                        e.Row.Cells[i].Controls.AddAt(0, Time);

                        //if (common.myStr(dt.Rows[e.Row.RowIndex]["DoseTime" + i]) != "")
                        //{
                        Time.SelectedDate = common.myDate(dt.Rows[e.Row.RowIndex]["DoseTime" + i].ToString().Split('/')[0]);
                        // }

                        chk = new CheckBox();
                        chk.ID = "chk" + i;
                        chk.Width = Unit.Pixel(30);
                        e.Row.Cells[i].Controls.AddAt(1, chk);


                        //if (common.myDate(e.Row.Cells[0].Text).ToString("dd/MM/yyyy") == System.DateTime.Now.ToString("dd/MM/yyyy"))
                        //{
                        //    chk.Checked = true;
                        //}
                        //else
                        //{
                        //    chk.Checked = false;
                        //    chk.Enabled = false;
                        //}
                        

                        //if (common.myStr(Request.QueryString["FrequencyData"]) != "")
                        //{
                        //    if (common.myBool(dt.Rows[e.Row.RowIndex]["DoseEnable"]) == true)
                        //    {
                         //       chk.Checked = true;
                        //    }
                        //    else
                        //    {
                        //        chk.Checked = false;
                        //    }
                        //}
                        //else
                        //{
                        //    chk.Checked = false;
                        //}

                        HiddenField hdn = new HiddenField();
                        hdn.ID = "hdn" + i;
                        e.Row.Cells[i].Controls.AddAt(2, hdn);
                        hdn.Value = common.myStr(dt.Rows[e.Row.RowIndex]["DoseTime" + i].ToString().Split('/')[1]);

                    }
                }
            }
        }
    }
    protected void btnApplyAll_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            if (ViewState["FrequencyDoseTime"] != null)
            {
                dt = (DataTable)ViewState["FrequencyDoseTime"];
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 1; j < common.myInt(ViewState["TotalCount"]); j++)
                {
                    RadTimePicker dtp = (RadTimePicker)gvData.Rows[i].Cells[j].FindControl("dtp" + j);
                    if (i == 0)
                    {
                        dt.Rows[0]["DoseTime" + j] = dtp.SelectedDate.Value.ToString("HH:mm:ss") + "/" + dt.Rows[i]["DoseTime" + j].ToString().Split('/')[1];
                    }
                    else
                    {
                        dt.Rows[i]["DoseTime" + j] = dt.Rows[0]["DoseTime" + j];
                    }
                    dt.AcceptChanges();
                }
            }
            gvData.DataSource = null;
            gvData.DataBind();

            gvData.DataSource = dt;
            gvData.DataBind();
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
    }
    protected void btnDoseExtend_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (common.myLen(hdnDoseTime.Value) > 0)
            {
                tblDoseExtend.Visible = true;

                dtExtendDate.SelectedDate = null;
                if (common.myInt(hdnColIdx.Value).Equals(1))
                {
                    dtExtendDate.MinDate = common.myDate(ViewState["LastInitialDate"]).AddDays(1);
                    dtExtendDate.MaxDate = common.myDate(ViewState["LastInitialDate"]).AddDays(1);
                    dtExtendDate.SelectedDate = common.myDate(ViewState["LastInitialDate"]).AddDays(1);
                }
                else
                {
                    dtExtendDate.MinDate = common.myDate(ViewState["LastInitialDate"]);
                    dtExtendDate.MaxDate = common.myDate(ViewState["LastInitialDate"]);
                    dtExtendDate.SelectedDate = common.myDate(ViewState["LastInitialDate"]);
                }

                dtExtendTime.SelectedDate = Convert.ToDateTime(hdnDoseTime.Value);
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
        }
    }
    private DataSet mergedExcludedDoseDetails(DataSet dsMain)
    {
        BaseC.ICM objIcm = new BaseC.ICM(sConString);
        DataSet dsExcluded = new DataSet();
        DataTable uniqueDates = new DataTable();
        DataView DV = new DataView();
        DataTable dtDatesData = new DataTable();
        DataTable dtDrugExcludedDetails = new DataTable();
        DataView dvDrugExcludedDetails = new DataView();
        try
        {
            if (ViewState["DrugExcludedDetails"] != null)
            {
                dtDrugExcludedDetails = (DataTable)ViewState["DrugExcludedDetails"];
            }

            if (dtDrugExcludedDetails != null)
            {
                if (dtDrugExcludedDetails.Rows.Count > 0)
                {
                    dvDrugExcludedDetails = dtDrugExcludedDetails.Copy().DefaultView;

                    foreach (DataRow DR in dsMain.Tables[0].Rows)
                    {
                        for (int colIdx = 1; colIdx < dsMain.Tables[0].Columns.Count; colIdx++)
                        {
                            if (common.myStr(DR["DoseTime" + colIdx]).Contains('/'))
                            {
                                int FrequencyDetailId = common.myInt(DR["DoseTime" + colIdx].ToString().Split('/')[1]);
                                if (common.myInt(FrequencyDetailId) > 0)
                                {
                                    dvDrugExcludedDetails.RowFilter = "DrugScheduleDate='" + common.myStr(DR["Date"]) + "' AND FrequencyDetailId=" + FrequencyDetailId.ToString();

                                    if (dvDrugExcludedDetails.ToTable().Rows.Count > 0)
                                    {
                                        DR["DoseTime" + colIdx] = common.myStr(dvDrugExcludedDetails.ToTable().Rows[0]["DoseTime"]) + "/" + FrequencyDetailId.ToString();
                                    }
                                }
                            }
                        }

                        dsMain.Tables[0].AcceptChanges();
                    }

                    dvDrugExcludedDetails.RowFilter = string.Empty;
                }
            }




            dsExcluded = objIcm.getExcludedDoseDetails(common.myInt(Session["FacilityId"]), common.myInt(Request.QueryString["RegId"]),
                                                        common.myInt(Request.QueryString["EncId"]), common.myInt(Request.QueryString["ItemId"]),
                                                        common.myInt(Request.QueryString["FrequencyId"]), common.myInt(Request.QueryString["IndentId"]));

            if (dsExcluded.Tables.Count > 0)
            {
                if (dsExcluded.Tables[0].Rows.Count > 0)
                {
                    uniqueDates = dsExcluded.Tables[0].Copy().DefaultView.ToTable(true, "Date");

                    foreach (DataRow uDate in uniqueDates.Rows)
                    {
                        DataRow DR = dsMain.Tables[0].NewRow();

                        DR["Date"] = uDate["Date"];

                        DV = dsExcluded.Tables[0].Copy().DefaultView;
                        DV.RowFilter = "Date='" + common.myStr(uDate["Date"]) + "'";

                        dtDatesData = new DataTable();
                        dtDatesData = DV.ToTable();

                        foreach (DataRow tDR in dtDatesData.Rows)
                        {
                            if (common.myInt(tDR["FrequencyDetailId"]) > 0)
                            {
                                int foundIdx = 0;

                                if (dsMain.Tables[0].Columns.Count > 1)
                                {
                                    for (int colIdx = 1; colIdx < dsMain.Tables[0].Columns.Count; colIdx++)
                                    {
                                        if (common.myStr(dsMain.Tables[0].Rows[0]["DoseTime" + colIdx]).Contains('/'))
                                        {
                                            if (common.myInt(dsMain.Tables[0].Rows[0]["DoseTime" + colIdx].ToString().Split('/')[1]).Equals(common.myInt(tDR["FrequencyDetailId"])))
                                            {
                                                foundIdx = colIdx;
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (foundIdx > 0)
                                {
                                    DR["DoseTime" + foundIdx] = common.myStr(tDR["DoseTime"]) + "/" + common.myStr(tDR["FrequencyDetailId"]);
                                }
                            }
                        }

                        DV.RowFilter = string.Empty;

                        dsMain.Tables[0].Rows.Add(DR);
                        dsMain.Tables[0].AcceptChanges();
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            objIcm = null;
            dsExcluded.Dispose();
            uniqueDates.Dispose();
            DV.Dispose();
            dtDatesData.Dispose();
            dtDrugExcludedDetails.Dispose();
            dvDrugExcludedDetails.Dispose();
        }

        return dsMain;
    }
}
