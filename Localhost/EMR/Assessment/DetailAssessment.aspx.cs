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
using System.Data.SqlClient;
using System.Text;

public partial class EMR_Assessment_DetailAssessment : System.Web.UI.Page
{
    #region Page level variable declration section

    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    private Hashtable hshInput;
    BaseC.ParseData Parse = new BaseC.ParseData();
    Hashtable hsNewPage = new Hashtable();
    StringBuilder str = new StringBuilder();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lstaddProblem.Attributes.Add("onblur", "nSat=1;");
            if (Request.QueryString["Mpg"] != null)
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
            if (Session["encounterid"] == null)
            {
                Response.Redirect("/Default.aspx?RegNo=0", false);
            }

            if (Session["registrationId"] != null)
            {
                BaseC.Patient objPatient = new BaseC.Patient(sConString);
                SqlDataReader objDr = (SqlDataReader)objPatient.getPatientDetails(Convert.ToInt32(Session["RegistrationID"]), Convert.ToInt32(Session["HospitalLocationID"]));
                if (objDr.Read())
                    lblPatientInfo.Text = objDr["name"].ToString();
                objDr.Close();
            }
            rbolist_SelectedIndexChanged(sender, e);
            //RetrievePatientDiagnosis();
        }
    }

    #region Get Retrieve data section

    //protected void RetrievePatientDiagnosis()
    //{
    //    try
    //    {
    //        if (Session["encounterid"] != null && Session["registrationno"] != null)
    //        {
    //            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //            hshInput = new Hashtable();

    //            hshInput.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
    //            hshInput.Add("@intRegistrationId", Convert.ToInt32(Session["RegistrationID"]));
    //            hshInput.Add("@intEncounterId", Convert.ToInt32(Session["encounterid"]));
    //            DataSet objDs = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDiagnosis", hshInput);
    //            if (objDs.Tables[0].Rows.Count > 0)
    //            {
    //                ViewState["Record"] = 1;
    //                DataView dvDiagnosisDetail = new DataView(objDs.Tables[0]);
    //                dvDiagnosisDetail.RowFilter = "IsChronic=1";
    //                DataTable dtChronicDiagnosisDetail = dvDiagnosisDetail.ToTable();
    //                if (dtChronicDiagnosisDetail.Rows.Count > 0)
    //                {
    //                    gvComplain.DataSource = dtChronicDiagnosisDetail;
    //                    gvComplain.DataBind();                        
    //                }
    //                else
    //                {
    //                    //BindBlankChronicDiagnosisGrid();
    //                }
    //                dvDiagnosisDetail.RowFilter = "IsChronic=0";
    //                DataTable dtDiagnosisDetail = dvDiagnosisDetail.ToTable();
    //                if (dtDiagnosisDetail.Rows.Count > 0)
    //                {
    //                    gvComplain.DataSource = dtDiagnosisDetail;
    //                    gvComplain.DataBind();                        
    //                }
    //                else
    //                {
    //                    //BindBlankDiagnosisDetailGrid();
    //                }
    //            }
    //            else
    //            {
    //                ViewState["Record"] = 0;
    //                //BindBlankChronicDiagnosisGrid();
    //                //BindBlankDiagnosisDetailGrid();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message;
    //    }
    //}

    #endregion

    protected void rbolist_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            if (rbolist.SelectedIndex == 0) //Today's Diagnosis
            {
                hshInput = new Hashtable();
                hshInput.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
                hshInput.Add("@intRegistrationId", Convert.ToInt32(Session["RegistrationID"]));
                DataSet objDs = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDiagnosis", hshInput);
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    ViewState["Record"] = 1;
                    DataView dvDiagnosisDetail = new DataView(objDs.Tables[0]);
                    dvDiagnosisDetail.RowFilter = "IsChronic=0 AND EncounterId=" + Convert.ToInt32(Session["encounterid"]) + " ";
                    DataTable dtDiagnosisDetail = dvDiagnosisDetail.ToTable();
                    if (dtDiagnosisDetail.Rows.Count > 0)
                    {
                        gvComplain.DataSource = dtDiagnosisDetail;
                        gvComplain.DataBind();
                    }
                    else
                    {
                        BindBlankGrid();
                    }
                }
            }
            else if (rbolist.SelectedIndex == 1) //Chronic Diagnosis
            {
                hshInput = new Hashtable();
                hshInput.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationId"]));
                hshInput.Add("@intRegistrationId", Convert.ToInt32(Session["RegistrationID"]));
                DataSet objDs = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDiagnosis", hshInput);
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    ViewState["Record"] = 1;
                    DataView dvDiagnosisDetail = new DataView(objDs.Tables[0]);
                    dvDiagnosisDetail.RowFilter = "IsChronic=1";
                    DataTable dtChronicDiagnosisDetail = dvDiagnosisDetail.ToTable();

                    if (dtChronicDiagnosisDetail.Rows.Count > 0)
                    {
                        gvComplain.DataSource = dtChronicDiagnosisDetail;
                        gvComplain.DataBind();
                    }
                    else
                    {
                        BindBlankGrid();
                    }
                }
            }
            upnlGrid.Update();
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindBlankGrid()
    {
        try
        {
            DataTable dT = CreateTable();
            for (int i = 0; i < 1; i++)
            {
                DataRow dr = dT.NewRow();
                dr["ICDDescription"] = "";
                dr["Id"] = 0;
                dT.Rows.Add(dr);
            }
            gvComplain.DataSource = dT;
            gvComplain.DataBind();
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected DataTable CreateTable()
    {
        DataTable Dt = new DataTable();
        Dt.Columns.Add("ICDDescription");
        Dt.Columns.Add("Id");
        return Dt;
    }

    protected void gvComplain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[2].Visible = false;
    }

    protected void btnstable_Click(object sender, EventArgs e)
    {
        txtcondition.Text = "Stable";
    }

    protected void btnunstable_Click(object sender, EventArgs e)
    {
        txtcondition.Text = "Unstable";
    }

    protected void btnimproving_Click(object sender, EventArgs e)
    {
        txtcondition.Text = "Improving";
    }

    protected void btndeteriorating_Click(object sender, EventArgs e)
    {
        txtcondition.Text = "Deteriorating";
    }

    protected void btngood_Click(object sender, EventArgs e)
    {
        txtprognosis.Text = "Good";
    }

    protected void btnguarded_Click(object sender, EventArgs e)
    {
        txtprognosis.Text = "Guarded";
    }

    protected void btnpoor_Click(object sender, EventArgs e)
    {
        txtprognosis.Text = "Poor";

    }

    protected void btnfair_Click(object sender, EventArgs e)
    {
        txtprognosis.Text = "Fair";
    }

    protected void btnnew_Click(object sender, EventArgs e)
    {
        txtprognosis.Text = "";
        txtcondition.Text = "";
        txtproblem.Text = "";
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (lstaddProblem.SelectedIndex > -1)
            {
                ListItem item = new ListItem();
                item.Text = lstaddProblem.SelectedItem.Text;
                item.Value = lstaddProblem.SelectedItem.Value;
                lstaddProblem.Items.Remove(item);
            }
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnup_Click(object sender, EventArgs e)
    {
        try
        {
            if (lstaddProblem.SelectedIndex != -1)
            {
                if (lstaddProblem.SelectedIndex > 0)
                {
                    int temp = 0;
                    string tempValue = "0";
                    string tempText = "";
                    for (int i = 0; i < lstaddProblem.Items.Count; i++)
                    {
                        if (lstaddProblem.Items[i].Selected)
                        {
                            tempValue = lstaddProblem.Items[i].Value;
                            tempText = lstaddProblem.Items[i].Text;
                            temp = i;
                            lstaddProblem.Items.Remove(lstaddProblem.Items[i]);
                            i--;
                        }
                    }
                    ListItem templist = new ListItem(tempText, tempValue);

                    lstaddProblem.Items.Insert(temp - 1, templist);
                    lstaddProblem.Items[temp - 1].Selected = true;
                }
            }
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btndown_Click(object sender, EventArgs e)
    {
        try
        {
            if (lstaddProblem.SelectedIndex != -1)
            {
                if (lstaddProblem.SelectedIndex < lstaddProblem.Items.Count - 1)
                {
                    int temp = 0;
                    string tempValue = "0";
                    string tempText = "";
                    for (int i = 0; i < lstaddProblem.Items.Count; i++)
                    {
                        if (lstaddProblem.Items[i].Selected)
                        {
                            tempValue = lstaddProblem.Items[i].Value;
                            tempText = lstaddProblem.Items[i].Text;
                            temp = i;
                            lstaddProblem.Items.Remove(lstaddProblem.Items[i]);
                            i--;
                        }
                    }

                    ListItem templist = new ListItem(tempText, tempValue);

                    lstaddProblem.Items.Insert(temp + 1, templist);
                    lstaddProblem.Items[temp + 1].Selected = true;
                }
            }
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvComplain_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (gvComplain.SelectedRow.Cells[0].Text == "&nbsp;")
            {
                gvComplain.SelectedRow.Cells[0].Text = null;
            }

            if (lstaddProblem.Items.FindByText(gvComplain.SelectedRow.Cells[0].Text) != null)
            {

            }
            else
            {
                ListItem item = new ListItem();
                item.Text = gvComplain.SelectedRow.Cells[0].Text;
                item.Value = gvComplain.SelectedRow.Cells[2].Text + "." + rbolist.SelectedItem.Value;
                lstaddProblem.Items.Add(item);
                lstaddProblem.Focus();
            }
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lstaddProblem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            String sqlstr = "";
            txtproblem.Text = lstaddProblem.SelectedItem.Text;
            String[] str = new String[2];
            char f = '.';
            str = lstaddProblem.SelectedItem.Value.Split(f);
            ViewState["Id"] = str[0];
            ViewState["TableName"] = str[1];

            if (ViewState["TableName"].ToString() == "C")
            {
                sqlstr = "select AssessmentCondition,AssessmentPrognosis,AssessmentProbable,AssessmentDifferential,AssessmentTest,AssessmentPlan,AssessmentComments,Resolved from EMRPatientProblemDetails where Id =" + ViewState["Id"];
            }
            else
            {
                sqlstr = "select AssessmentCondition,AssessmentPrognosis,AssessmentProbable,AssessmentDifferential,AssessmentTest,AssessmentPlan,AssessmentComments,Resolved from ChrAssessmentDetails where Id =" + ViewState["Id"];
            }
            ds = dl.FillDataSet(CommandType.Text, sqlstr);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtcondition.Text = Convert.ToString(ds.Tables[0].Rows[0]["AssessmentCondition"]);
                    txtprognosis.Text = Convert.ToString(ds.Tables[0].Rows[0]["AssessmentPrognosis"]);
                    txtdiagnosis.Text = Convert.ToString(ds.Tables[0].Rows[0]["AssessmentProbable"]);
                    txtdifferential.Text = Convert.ToString(ds.Tables[0].Rows[0]["AssessmentDifferential"]);
                    txttest.Text = Convert.ToString(ds.Tables[0].Rows[0]["AssessmentTest"]);
                    txtplan.Text = Convert.ToString(ds.Tables[0].Rows[0]["AssessmentPlan"]);
                    txtcomment.Text = Convert.ToString(ds.Tables[0].Rows[0]["AssessmentComments"]);
                    //ddlResolved.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["Resolved"]);
                }
            }
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("DetailAssessment.aspx", false);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (lstaddProblem.SelectedIndex > -1)
            {
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                BaseC.ParseData bC = new BaseC.ParseData();

                Hashtable hshtableout = new Hashtable();
                Hashtable hshtablein = new Hashtable();
                Hashtable hshtableinChk = new Hashtable();

                hshtableinChk.Add("@RegistrationId", Convert.ToInt32(Session["RegistrationID"]));//
                hshtableinChk.Add("@EncounterId", Convert.ToInt32(Session["encounterid"]));//

                hshtableinChk.Add("chvAssessmentCondition", bC.ParseQ(txtcondition.Text.Trim()));
                hshtableinChk.Add("chvAssessmentPrognosis", bC.ParseQ(txtprognosis.Text.Trim()));
                hshtableinChk.Add("chvAssessmentProbable", bC.ParseQ(txtdiagnosis.Text.Trim()));
                hshtableinChk.Add("chvAssessmentDifferential", bC.ParseQ(txtdifferential.Text.Trim()));
                hshtableinChk.Add("chvAssessmentTest", bC.ParseQ(txttest.Text.Trim()));

                hshtableinChk.Add("chvAssessmentPlan", bC.ParseQ(txtplan.Text.Trim()));
                hshtableinChk.Add("chvAssessmentComments", bC.ParseQ(txtcomment.Text.Trim()));
                hshtableinChk.Add("Resolved", ddlResolved.SelectedValue);

                hshtableinChk.Add("intId", ViewState["Id"]);

                if (ViewState["TableName"].ToString() == "C")
                {
                    objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRUpdateDiagnosisAssessment", hshtableinChk);
                }

                else
                {
                    objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRUpdateDiagnosisAssessment", hshtableinChk);
                }
                //Alert.ShowAjaxMsg("Data Saved.", Page);
                lblMsg.Text = "Record(s) Saved...";

            }
            else
            {
                Alert.ShowAjaxMsg("Please Select Complain.", Page);
            }
            ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", "nSat=0;", true);
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

}
