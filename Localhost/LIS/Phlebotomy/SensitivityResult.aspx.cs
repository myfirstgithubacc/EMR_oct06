using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Text;
using System.Configuration;

public partial class LIS_Phlebotomy_SensitivityResult : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsMicrobiology objval;
    DAL.DAL dl = new DAL.DAL();
    DataTable dt;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["ServiceName"] != null)
            {
                //Added for Decode & as string  on 09-04-2014 Start 
                //this.lblServiceDetails.Text = "Service Name: " + common.myStr(Request.QueryString["ServiceName"]);
                string Servicename = common.myStr(Request.QueryString["ServiceName"]);
                Servicename = Servicename.Replace("/", "&");
                this.lblServiceDetails.Text = "Service Name: " + Servicename;
                //Added for Decode & as string  on 09-04-2014 End


                
            }

            bindSensitivityFormatGrid();

            if (common.myStr(Request.QueryString["RStatus"]) == "RF")
            {
                btnSave.Visible = false;
            }
            
        }
    }

    void bindSensitivityFormatGrid()
    {
        objval = new BaseC.clsMicrobiology(sConString);
        DataSet ds = new DataSet();
        try
        {
            StringBuilder strItemXML = new StringBuilder();
            ArrayList coll = new ArrayList();

            string strSource = common.myStr(Request.QueryString["Source"]);
            string strOrganismIds = common.myStr(Request.QueryString["OrganismIds"]);
            int iDiagSampleId = common.myInt(Request.QueryString["DiagSampleId"]);
            int iResultId = common.myInt(Request.QueryString["ResultId"]);
            string[] Ids = strOrganismIds.Split(',');

            foreach (string Id in Ids)
            {
                if (common.myInt(Id) > 0)
                {
                    coll.Add(common.myInt(Id));
                    strItemXML.Append(common.setXmlTable(ref coll));
                }
            }

            ds = objval.GetLabSensitivityFormat(strSource, iDiagSampleId, strItemXML.ToString(), iResultId, common.myBool(Request.QueryString["IsRelease"]));// (common.myStr(Request.QueryString["xmlOrganismIds"]));
            dt = ds.Tables[0].Copy();

            gvSensitivityResult.DataSource = ds;
            gvSensitivityResult.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }

    protected void gvSensitivityResult_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (dt.Columns.Count == 8)
                {
                    Label lblhdrIRS1 = (Label)e.Row.FindControl("lblhdrIRS1");
                    lblhdrIRS1.Text = common.myStr(dt.Columns[4].ColumnName).Substring(0, dt.Columns[4].ColumnName.Length - 4);
                }
                if (dt.Columns.Count == 14)
                {
                    Label lblhdrIRS1 = (Label)e.Row.FindControl("lblhdrIRS1");
                    lblhdrIRS1.Text = common.myStr(dt.Columns[4].ColumnName).Substring(0, dt.Columns[4].ColumnName.Length - 4);

                    Label lblhdrIRS2 = (Label)e.Row.FindControl("lblhdrIRS2");
                    lblhdrIRS2.Text = common.myStr(dt.Columns[10].ColumnName).Substring(0, dt.Columns[10].ColumnName.Length - 4);
                }
                if (dt.Columns.Count == 20)
                {
                    Label lblhdrIRS1 = (Label)e.Row.FindControl("lblhdrIRS1");
                    lblhdrIRS1.Text = common.myStr(dt.Columns[4].ColumnName).Substring(0, dt.Columns[4].ColumnName.Length - 4);

                    Label lblhdrIRS2 = (Label)e.Row.FindControl("lblhdrIRS2");
                    lblhdrIRS2.Text = common.myStr(dt.Columns[10].ColumnName).Substring(0, dt.Columns[10].ColumnName.Length - 4);

                    Label lblhdrIRS3 = (Label)e.Row.FindControl("lblhdrIRS3");
                    lblhdrIRS3.Text = common.myStr(dt.Columns[16].ColumnName).Substring(0, dt.Columns[16].ColumnName.Length - 4);
                }
                if (dt.Columns.Count == 26)
                {
                    Label lblhdrIRS1 = (Label)e.Row.FindControl("lblhdrIRS1");
                    lblhdrIRS1.Text = common.myStr(dt.Columns[4].ColumnName).Substring(0, dt.Columns[4].ColumnName.Length - 4);

                    Label lblhdrIRS2 = (Label)e.Row.FindControl("lblhdrIRS2");
                    lblhdrIRS2.Text = common.myStr(dt.Columns[10].ColumnName).Substring(0, dt.Columns[10].ColumnName.Length - 4);

                    Label lblhdrIRS3 = (Label)e.Row.FindControl("lblhdrIRS3");
                    lblhdrIRS3.Text = common.myStr(dt.Columns[16].ColumnName).Substring(0, dt.Columns[16].ColumnName.Length - 4);

                    Label lblhdrIRS4 = (Label)e.Row.FindControl("lblhdrIRS4");
                    lblhdrIRS4.Text = common.myStr(dt.Columns[22].ColumnName).Substring(0, dt.Columns[22].ColumnName.Length - 4);
                }
                if (dt.Columns.Count == 32)
                {
                    Label lblhdrIRS1 = (Label)e.Row.FindControl("lblhdrIRS1");
                    lblhdrIRS1.Text = common.myStr(dt.Columns[4].ColumnName).Substring(0, dt.Columns[4].ColumnName.Length - 4);

                    Label lblhdrIRS2 = (Label)e.Row.FindControl("lblhdrIRS2");
                    lblhdrIRS2.Text = common.myStr(dt.Columns[10].ColumnName).Substring(0, dt.Columns[10].ColumnName.Length - 4);

                    Label lblhdrIRS3 = (Label)e.Row.FindControl("lblhdrIRS3");
                    lblhdrIRS3.Text = common.myStr(dt.Columns[16].ColumnName).Substring(0, dt.Columns[16].ColumnName.Length - 4);

                    Label lblhdrIRS4 = (Label)e.Row.FindControl("lblhdrIRS4");
                    lblhdrIRS4.Text = common.myStr(dt.Columns[22].ColumnName).Substring(0, dt.Columns[22].ColumnName.Length - 4);

                    Label lblhdrIRS5 = (Label)e.Row.FindControl("lblhdrIRS5");
                    lblhdrIRS5.Text = common.myStr(dt.Columns[28].ColumnName).Substring(0, dt.Columns[28].ColumnName.Length - 4);
                }
            }
            if (e.Row.RowType != DataControlRowType.Pager)
            {
                if (dt.Columns.Count == 8)
                {
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;
                    e.Row.Cells[9].Visible = false;
                    e.Row.Cells[10].Visible = false;
                    e.Row.Cells[11].Visible = false;
                    e.Row.Cells[12].Visible = false;
                    e.Row.Cells[13].Visible = false;
                    e.Row.Cells[14].Visible = false;
                    e.Row.Cells[15].Visible = false;
                    e.Row.Cells[16].Visible = false;
                }
                if (dt.Columns.Count == 14)
                {
                    e.Row.Cells[10].Visible = false;
                    e.Row.Cells[11].Visible = false;
                    e.Row.Cells[12].Visible = false;
                    e.Row.Cells[13].Visible = false;
                    e.Row.Cells[14].Visible = false;
                    e.Row.Cells[15].Visible = false;
                    e.Row.Cells[16].Visible = false;
                }
                if (dt.Columns.Count == 20)
                {
                    e.Row.Cells[13].Visible = false;
                    e.Row.Cells[14].Visible = false;
                    e.Row.Cells[15].Visible = false;
                    e.Row.Cells[16].Visible = false;
                }
                if (dt.Columns.Count == 26)
                {
                    e.Row.Cells[15].Visible = false;
                    e.Row.Cells[16].Visible = false;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal ltrAntibioticId = (Literal)e.Row.FindControl("ltrAntibioticId");
                DataView dv = new DataView(dt);
                dv.RowFilter = "AntibioticId = " + common.myInt(ltrAntibioticId.Text);

                if (dt.Columns.Count == 8)
                {
                    if (common.myInt(ViewState["i"]) > 0)
                    {
                        Label lblIRS1 = (Label)e.Row.FindControl("lblIRS1");
                        Label lblMIC1 = (Label)e.Row.FindControl("lblMIC1");
                        Label lblBreakPoint1 = (Label)e.Row.FindControl("lblBreakPoint1");
                        Label lblRelease1 = (Label)e.Row.FindControl("lblRelease1");

                        lblIRS1.Visible = false;
                        lblMIC1.Visible = false;
                        lblBreakPoint1.Visible = false;
                        lblRelease1.Visible = false;
                    }
                    Label lblAntibioticName = (Label)e.Row.FindControl("lblAntibioticName");
                    lblAntibioticName.Text = common.myStr(dv[0][1]);

                    Literal ltrOrganismId = (Literal)e.Row.FindControl("ltrOrganismId1");
                    ltrOrganismId.Text = common.myStr(dv[0][2]);

                    Literal ltrOrganismId1_IsResistant = (Literal)e.Row.FindControl("ltrOrganismId1_IsResistant");
                    ltrOrganismId1_IsResistant.Text = common.myStr(dv[0][3]);

                    TextBox txtIRS1 = (TextBox)e.Row.FindControl("txtIRS1");
                    txtIRS1.Text = common.myStr(dv[0][4]);

                    TextBox txtMIC1 = (TextBox)e.Row.FindControl("txtMIC1");
                    txtMIC1.Text = common.myStr(dv[0][5]);

                    TextBox txtBreakPoint1 = (TextBox)e.Row.FindControl("txtBreakPoint1");
                    txtBreakPoint1.Text = common.myStr(dv[0][6]);

                    CheckBox ChkRelease1 = (CheckBox)e.Row.FindControl("ChkRelease1");
                    ChkRelease1.Checked = common.myBool(dv[0][7]);

                    if (common.myStr(txtIRS1.Text) == "X")
                    {
                        txtIRS1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS1.ReadOnly = true;
                        txtMIC1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtMIC1.ReadOnly = true;
                        txtBreakPoint1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtBreakPoint1.ReadOnly = true;
                        ChkRelease1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        ChkRelease1.Enabled = false;
                    }
                    else if (common.myBool(ltrOrganismId1_IsResistant.Text))
                    {
                        txtIRS1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS1.ForeColor = Color.Red;
                        txtIRS1.ReadOnly = true;
                    }
                    else
                    {
                        txtIRS1.Attributes.Add("onkeyup", "javascript:IRSValues('" + txtIRS1.ClientID + "','" + ChkRelease1.ClientID + "');");
                    }
                }
                if (dt.Columns.Count == 14)
                {
                    if (common.myInt(ViewState["i"]) > 0)
                    {
                        Label lblIRS1 = (Label)e.Row.FindControl("lblIRS1");
                        Label lblMIC1 = (Label)e.Row.FindControl("lblMIC1");
                        Label lblBreakPoint1 = (Label)e.Row.FindControl("lblBreakPoint1");
                        Label lblRelease1 = (Label)e.Row.FindControl("lblRelease1");

                        Label lblIRS2 = (Label)e.Row.FindControl("lblIRS2");
                        Label lblMIC2 = (Label)e.Row.FindControl("lblMIC2");
                        Label lblBreakPoint2 = (Label)e.Row.FindControl("lblBreakPoint2");
                        Label lblRelease2 = (Label)e.Row.FindControl("lblRelease2");

                        lblIRS1.Visible = false;
                        lblMIC1.Visible = false;
                        lblBreakPoint1.Visible = false;
                        lblRelease1.Visible = false;

                        lblIRS2.Visible = false;
                        lblMIC2.Visible = false;
                        lblBreakPoint2.Visible = false;
                        lblRelease2.Visible = false;
                    }
                    Label lblAntibioticName = (Label)e.Row.FindControl("lblAntibioticName");
                    lblAntibioticName.Text = common.myStr(dv[0][1]);

                    Literal ltrOrganismId1 = (Literal)e.Row.FindControl("ltrOrganismId1");
                    ltrOrganismId1.Text = common.myStr(dv[0][2]);

                    Literal ltrOrganismId1_IsResistant = (Literal)e.Row.FindControl("ltrOrganismId1_IsResistant");
                    ltrOrganismId1_IsResistant.Text = common.myStr(dv[0][3]);

                    TextBox txtIRS1 = (TextBox)e.Row.FindControl("txtIRS1");
                    txtIRS1.Text = common.myStr(dv[0][4]);

                    TextBox txtMIC1 = (TextBox)e.Row.FindControl("txtMIC1");
                    txtMIC1.Text = common.myStr(dv[0][5]);

                    TextBox txtBreakPoint1 = (TextBox)e.Row.FindControl("txtBreakPoint1");
                    txtBreakPoint1.Text = common.myStr(dv[0][6]);

                    CheckBox ChkRelease1 = (CheckBox)e.Row.FindControl("ChkRelease1");
                    ChkRelease1.Checked = common.myBool(dv[0][7]);

                    if (common.myStr(txtIRS1.Text) == "X")
                    {
                        txtIRS1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS1.ReadOnly = true;
                        txtMIC1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtMIC1.ReadOnly = true;
                        txtBreakPoint1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtBreakPoint1.ReadOnly = true;
                        ChkRelease1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        ChkRelease1.Enabled = false;
                    }
                    else if (common.myBool(ltrOrganismId1_IsResistant.Text))
                    {
                        txtIRS1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS1.ForeColor = Color.Red;
                        txtIRS1.ReadOnly = true;
                    }
                    else
                    {
                        txtIRS1.Attributes.Add("onkeyup", "javascript:IRSValues('" + txtIRS1.ClientID + "','" + ChkRelease1.ClientID + "');");
                    }
                    Literal ltrOrganismId2 = (Literal)e.Row.FindControl("ltrOrganismId2");
                    ltrOrganismId2.Text = common.myStr(dv[0][8]);

                    Literal ltrOrganismId2_IsResistant = (Literal)e.Row.FindControl("ltrOrganismId2_IsResistant");
                    ltrOrganismId2_IsResistant.Text = common.myStr(dv[0][9]);

                    TextBox txtIRS2 = (TextBox)e.Row.FindControl("txtIRS2");
                    txtIRS2.Text = common.myStr(dv[0][10]);

                    TextBox txtMIC2 = (TextBox)e.Row.FindControl("txtMIC2");
                    txtMIC2.Text = common.myStr(dv[0][11]);

                    TextBox txtBreakPoint2 = (TextBox)e.Row.FindControl("txtBreakPoint2");
                    txtBreakPoint2.Text = common.myStr(dv[0][12]);

                    CheckBox ChkRelease2 = (CheckBox)e.Row.FindControl("ChkRelease2");
                    ChkRelease2.Checked = common.myBool(dv[0][13]);

                    if (common.myStr(txtIRS2.Text) == "X")
                    {
                        txtIRS2.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS2.ReadOnly = true;
                        txtMIC2.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtMIC2.ReadOnly = true;
                        txtBreakPoint2.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtBreakPoint2.ReadOnly = true;
                        ChkRelease2.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        ChkRelease2.Enabled = false;
                    }
                    else if (common.myBool(ltrOrganismId2_IsResistant.Text))
                    {
                        txtIRS2.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS2.ForeColor = Color.Red;
                        txtIRS2.ReadOnly = true;
                    }
                    else
                    {
                        txtIRS2.Attributes.Add("onkeyup", "javascript:IRSValues('" + txtIRS2.ClientID + "','" + ChkRelease2.ClientID + "');");
                    }
                }
                if (dt.Columns.Count == 20)
                {
                    if (common.myInt(ViewState["i"]) > 0)
                    {
                        Label lblIRS1 = (Label)e.Row.FindControl("lblIRS1");
                        Label lblMIC1 = (Label)e.Row.FindControl("lblMIC1");
                        Label lblBreakPoint1 = (Label)e.Row.FindControl("lblBreakPoint1");
                        Label lblRelease1 = (Label)e.Row.FindControl("lblRelease1");

                        Label lblIRS2 = (Label)e.Row.FindControl("lblIRS2");
                        Label lblMIC2 = (Label)e.Row.FindControl("lblMIC2");
                        Label lblBreakPoint2 = (Label)e.Row.FindControl("lblBreakPoint2");
                        Label lblRelease2 = (Label)e.Row.FindControl("lblRelease2");

                        Label lblIRS3 = (Label)e.Row.FindControl("lblIRS3");
                        Label lblMIC3 = (Label)e.Row.FindControl("lblMIC3");
                        Label lblBreakPoint3 = (Label)e.Row.FindControl("lblBreakPoint3");
                        Label lblRelease3 = (Label)e.Row.FindControl("lblRelease3");

                        lblIRS1.Visible = false;
                        lblMIC1.Visible = false;
                        lblBreakPoint1.Visible = false;
                        lblRelease1.Visible = false;

                        lblIRS2.Visible = false;
                        lblMIC2.Visible = false;
                        lblBreakPoint2.Visible = false;
                        lblRelease2.Visible = false;

                        lblIRS3.Visible = false;
                        lblMIC3.Visible = false;
                        lblBreakPoint3.Visible = false;
                        lblRelease3.Visible = false;
                    }

                    Label lblAntibioticName = (Label)e.Row.FindControl("lblAntibioticName");
                    lblAntibioticName.Text = common.myStr(dv[0][1]);

                    Literal ltrOrganismId1 = (Literal)e.Row.FindControl("ltrOrganismId1");
                    ltrOrganismId1.Text = common.myStr(dv[0][2]);

                    Literal ltrOrganismId1_IsResistant = (Literal)e.Row.FindControl("ltrOrganismId1_IsResistant");
                    ltrOrganismId1_IsResistant.Text = common.myStr(dv[0][3]);

                    TextBox txtIRS1 = (TextBox)e.Row.FindControl("txtIRS1");
                    txtIRS1.Text = common.myStr(dv[0][4]);

                    TextBox txtMIC1 = (TextBox)e.Row.FindControl("txtMIC1");
                    txtMIC1.Text = common.myStr(dv[0][5]);

                    TextBox txtBreakPoint1 = (TextBox)e.Row.FindControl("txtBreakPoint1");
                    txtBreakPoint1.Text = common.myStr(dv[0][6]);

                    CheckBox ChkRelease1 = (CheckBox)e.Row.FindControl("ChkRelease1");
                    ChkRelease1.Checked = common.myBool(dv[0][7]);

                    if (common.myStr(txtIRS1.Text) == "X")
                    {
                        txtIRS1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS1.ReadOnly = true;
                        txtMIC1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtMIC1.ReadOnly = true;
                        txtBreakPoint1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtBreakPoint1.ReadOnly = true;
                        ChkRelease1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        ChkRelease1.Enabled = false;
                    }
                    else if (common.myStr(ltrOrganismId1_IsResistant.Text) == "True")
                    {
                        txtIRS1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS1.ForeColor = Color.Red;
                        txtIRS1.ReadOnly = true;
                    }
                    else
                    {
                        txtIRS1.Attributes.Add("onkeyup", "javascript:IRSValues('" + txtIRS1.ClientID + "','" + ChkRelease1.ClientID + "');");
                    }

                    Literal ltrOrganismId2 = (Literal)e.Row.FindControl("ltrOrganismId2");
                    ltrOrganismId2.Text = common.myStr(dv[0][8]);

                    Literal ltrOrganismId2_IsResistant = (Literal)e.Row.FindControl("ltrOrganismId2_IsResistant");
                    ltrOrganismId2_IsResistant.Text = common.myStr(dv[0][9]);

                    TextBox txtIRS2 = (TextBox)e.Row.FindControl("txtIRS2");
                    txtIRS2.Text = common.myStr(dv[0][10]);

                    TextBox txtMIC2 = (TextBox)e.Row.FindControl("txtMIC2");
                    txtMIC2.Text = common.myStr(dv[0][11]);

                    TextBox txtBreakPoint2 = (TextBox)e.Row.FindControl("txtBreakPoint2");
                    txtBreakPoint2.Text = common.myStr(dv[0][12]);

                    CheckBox ChkRelease2 = (CheckBox)e.Row.FindControl("ChkRelease2");
                    ChkRelease2.Checked = common.myBool(dv[0][13]);

                    if (common.myStr(txtIRS2.Text) == "X")
                    {
                        txtIRS2.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS2.ReadOnly = true;
                        txtMIC2.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtMIC2.ReadOnly = true;
                        txtBreakPoint2.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtBreakPoint2.ReadOnly = true;
                        ChkRelease2.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        ChkRelease2.Enabled = false;
                    }
                    else if (common.myBool(ltrOrganismId2_IsResistant.Text))
                    {
                        txtIRS2.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS2.ForeColor = Color.Red;
                        txtIRS2.ReadOnly = true;
                    }
                    else
                    {
                        txtIRS2.Attributes.Add("onkeyup", "javascript:IRSValues('" + txtIRS2.ClientID + "','" + ChkRelease2.ClientID + "');");
                    }

                    Literal ltrOrganismId3 = (Literal)e.Row.FindControl("ltrOrganismId3");
                    ltrOrganismId3.Text = common.myStr(dv[0][14]);

                    Literal ltrOrganismId3_IsResistant = (Literal)e.Row.FindControl("ltrOrganismId3_IsResistant");
                    ltrOrganismId3_IsResistant.Text = common.myStr(dv[0][15]);

                    TextBox txtIRS3 = (TextBox)e.Row.FindControl("txtIRS3");
                    txtIRS3.Text = common.myStr(dv[0][16]);

                    TextBox txtMIC3 = (TextBox)e.Row.FindControl("txtMIC3");
                    txtMIC3.Text = common.myStr(dv[0][17]);

                    TextBox txtBreakPoint3 = (TextBox)e.Row.FindControl("txtBreakPoint3");
                    txtBreakPoint3.Text = common.myStr(dv[0][18]);

                    CheckBox ChkRelease3 = (CheckBox)e.Row.FindControl("ChkRelease3");
                    ChkRelease3.Checked = common.myBool(dv[0][19]);

                    if (common.myStr(txtIRS3.Text) == "X")
                    {
                        txtIRS3.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS3.ReadOnly = true;
                        txtMIC3.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtMIC3.ReadOnly = true;
                        txtBreakPoint3.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtBreakPoint3.ReadOnly = true;
                        ChkRelease3.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        ChkRelease3.Enabled = false;
                    }
                    else if (common.myBool(ltrOrganismId3_IsResistant.Text))
                    {
                        txtIRS3.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS3.ForeColor = Color.Red;
                        txtIRS3.ReadOnly = true;
                    }
                    else
                    {
                        txtIRS3.Attributes.Add("onkeyup", "javascript:IRSValues('" + txtIRS3.ClientID + "','" + ChkRelease3.ClientID + "');");
                    }
                }
                if (dt.Columns.Count == 26)
                {
                    if (common.myInt(ViewState["i"]) > 0)
                    {
                        Label lblIRS1 = (Label)e.Row.FindControl("lblIRS1");
                        Label lblMIC1 = (Label)e.Row.FindControl("lblMIC1");
                        Label lblBreakPoint1 = (Label)e.Row.FindControl("lblBreakPoint1");
                        Label lblRelease1 = (Label)e.Row.FindControl("lblRelease1");

                        Label lblIRS2 = (Label)e.Row.FindControl("lblIRS2");
                        Label lblMIC2 = (Label)e.Row.FindControl("lblMIC2");
                        Label lblBreakPoint2 = (Label)e.Row.FindControl("lblBreakPoint2");
                        Label lblRelease2 = (Label)e.Row.FindControl("lblRelease2");

                        Label lblIRS3 = (Label)e.Row.FindControl("lblIRS3");
                        Label lblMIC3 = (Label)e.Row.FindControl("lblMIC3");
                        Label lblBreakPoint3 = (Label)e.Row.FindControl("lblBreakPoint3");
                        Label lblRelease3 = (Label)e.Row.FindControl("lblRelease3");

                        Label lblIRS4 = (Label)e.Row.FindControl("lblIRS4");
                        Label lblMIC4 = (Label)e.Row.FindControl("lblMIC4");
                        Label lblBreakPoint4 = (Label)e.Row.FindControl("lblBreakPoint4");
                        Label lblRelease4 = (Label)e.Row.FindControl("lblRelease4");

                        lblIRS1.Visible = false;
                        lblMIC1.Visible = false;
                        lblBreakPoint1.Visible = false;
                        lblRelease1.Visible = false;

                        lblIRS2.Visible = false;
                        lblMIC2.Visible = false;
                        lblBreakPoint2.Visible = false;
                        lblRelease2.Visible = false;

                        lblIRS3.Visible = false;
                        lblMIC3.Visible = false;
                        lblBreakPoint3.Visible = false;
                        lblRelease3.Visible = false;

                        lblIRS4.Visible = false;
                        lblMIC4.Visible = false;
                        lblBreakPoint4.Visible = false;
                        lblRelease4.Visible = false;
                    }

                    Label lblAntibioticName = (Label)e.Row.FindControl("lblAntibioticName");
                    lblAntibioticName.Text = common.myStr(dv[0][1]);

                    Literal ltrOrganismId1 = (Literal)e.Row.FindControl("ltrOrganismId1");
                    ltrOrganismId1.Text = common.myStr(dv[0][2]);

                    Literal ltrOrganismId1_IsResistant = (Literal)e.Row.FindControl("ltrOrganismId1_IsResistant");
                    ltrOrganismId1_IsResistant.Text = common.myStr(dv[0][3]);

                    TextBox txtIRS1 = (TextBox)e.Row.FindControl("txtIRS1");
                    txtIRS1.Text = common.myStr(dv[0][4]);

                    TextBox txtMIC1 = (TextBox)e.Row.FindControl("txtMIC1");
                    txtMIC1.Text = common.myStr(dv[0][5]);

                    TextBox txtBreakPoint1 = (TextBox)e.Row.FindControl("txtBreakPoint1");
                    txtBreakPoint1.Text = common.myStr(dv[0][6]);

                    CheckBox ChkRelease1 = (CheckBox)e.Row.FindControl("ChkRelease1");
                    ChkRelease1.Checked = common.myBool(dv[0][7]);

                    if (common.myStr(txtIRS1.Text) == "X")
                    {
                        txtIRS1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS1.ReadOnly = true;
                        txtMIC1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtMIC1.ReadOnly = true;
                        txtBreakPoint1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtBreakPoint1.ReadOnly = true;
                        ChkRelease1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        ChkRelease1.Enabled = false;
                    }
                    else if (common.myBool(ltrOrganismId1_IsResistant.Text))
                    {
                        txtIRS1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS1.ForeColor = Color.Red;
                        txtIRS1.ReadOnly = true;
                    }
                    else
                    {
                        txtIRS1.Attributes.Add("onkeyup", "javascript:IRSValues('" + txtIRS1.ClientID + "','" + ChkRelease1.ClientID + "');");
                    }

                    Literal ltrOrganismId2 = (Literal)e.Row.FindControl("ltrOrganismId2");
                    ltrOrganismId2.Text = common.myStr(dv[0][8]);

                    Literal ltrOrganismId2_IsResistant = (Literal)e.Row.FindControl("ltrOrganismId2_IsResistant");
                    ltrOrganismId2_IsResistant.Text = common.myStr(dv[0][9]);

                    TextBox txtIRS2 = (TextBox)e.Row.FindControl("txtIRS2");
                    txtIRS2.Text = common.myStr(dv[0][10]);

                    TextBox txtMIC2 = (TextBox)e.Row.FindControl("txtMIC2");
                    txtMIC2.Text = common.myStr(dv[0][11]);

                    TextBox txtBreakPoint2 = (TextBox)e.Row.FindControl("txtBreakPoint2");
                    txtBreakPoint2.Text = common.myStr(dv[0][12]);

                    CheckBox ChkRelease2 = (CheckBox)e.Row.FindControl("ChkRelease2");
                    ChkRelease2.Checked = common.myBool(dv[0][13]);

                    if (common.myStr(txtIRS2.Text) == "X")
                    {
                        txtIRS2.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS2.ReadOnly = true;
                        txtMIC2.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtMIC2.ReadOnly = true;
                        txtBreakPoint2.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtBreakPoint2.ReadOnly = true;
                        ChkRelease2.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        ChkRelease2.Enabled = false;
                    }
                    else if (common.myBool(ltrOrganismId2_IsResistant.Text))
                    {
                        txtIRS2.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS2.ForeColor = Color.Red;
                        txtIRS2.ReadOnly = true;
                    }
                    else
                    {
                        txtIRS2.Attributes.Add("onkeyup", "javascript:IRSValues('" + txtIRS2.ClientID + "','" + ChkRelease2.ClientID + "');");
                    }

                    Literal ltrOrganismId3 = (Literal)e.Row.FindControl("ltrOrganismId3");
                    ltrOrganismId3.Text = common.myStr(dv[0][14]);

                    Literal ltrOrganismId3_IsResistant = (Literal)e.Row.FindControl("ltrOrganismId3_IsResistant");
                    ltrOrganismId3_IsResistant.Text = common.myStr(dv[0][15]);

                    TextBox txtIRS3 = (TextBox)e.Row.FindControl("txtIRS3");
                    txtIRS3.Text = common.myStr(dv[0][16]);

                    TextBox txtMIC3 = (TextBox)e.Row.FindControl("txtMIC3");
                    txtMIC3.Text = common.myStr(dv[0][17]);

                    TextBox txtBreakPoint3 = (TextBox)e.Row.FindControl("txtBreakPoint3");
                    txtBreakPoint3.Text = common.myStr(dv[0][18]);

                    CheckBox ChkRelease3 = (CheckBox)e.Row.FindControl("ChkRelease3");
                    ChkRelease3.Checked = common.myBool(dv[0][19]);

                    if (common.myStr(txtIRS3.Text) == "X")
                    {
                        txtIRS3.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS3.ReadOnly = true;
                        txtMIC3.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtMIC3.ReadOnly = true;
                        txtBreakPoint3.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtBreakPoint3.ReadOnly = true;
                        ChkRelease3.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        ChkRelease3.Enabled = false;
                    }
                    else if (common.myBool(ltrOrganismId3_IsResistant.Text))
                    {
                        txtIRS3.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS3.ForeColor = Color.Red;
                        txtIRS3.ReadOnly = true;
                    }
                    else
                    {
                        txtIRS3.Attributes.Add("onkeyup", "javascript:IRSValues('" + txtIRS3.ClientID + "','" + ChkRelease3.ClientID + "');");
                    }

                    Literal ltrOrganismId4 = (Literal)e.Row.FindControl("ltrOrganismId4");
                    ltrOrganismId4.Text = common.myStr(dv[0][20]);

                    Literal ltrOrganismId4_IsResistant = (Literal)e.Row.FindControl("ltrOrganismId4_IsResistant");
                    ltrOrganismId4_IsResistant.Text = common.myStr(dv[0][21]);

                    TextBox txtIRS4 = (TextBox)e.Row.FindControl("txtIRS4");
                    txtIRS4.Text = common.myStr(dv[0][22]);

                    TextBox txtMIC4 = (TextBox)e.Row.FindControl("txtMIC4");
                    txtMIC4.Text = common.myStr(dv[0][23]);

                    TextBox txtBreakPoint4 = (TextBox)e.Row.FindControl("txtBreakPoint4");
                    txtBreakPoint4.Text = common.myStr(dv[0][24]);

                    CheckBox ChkRelease4 = (CheckBox)e.Row.FindControl("ChkRelease4");
                    ChkRelease4.Checked = common.myBool(dv[0][25]);

                    if (common.myStr(txtIRS4.Text) == "X")
                    {
                        txtIRS4.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS4.ReadOnly = true;
                        txtMIC4.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtMIC4.ReadOnly = true;
                        txtBreakPoint4.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtBreakPoint4.ReadOnly = true;
                        ChkRelease4.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        ChkRelease4.Enabled = false;
                    }
                    else if (common.myBool(ltrOrganismId4_IsResistant.Text))
                    {
                        txtIRS4.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS4.ForeColor = Color.Red;
                        txtIRS4.ReadOnly = true;
                    }
                    else
                    {
                        txtIRS4.Attributes.Add("onkeyup", "javascript:IRSValues('" + txtIRS4.ClientID + "','" + ChkRelease4.ClientID + "');");
                    }
                }
                if (dt.Columns.Count == 32)
                {
                    if (common.myInt(ViewState["i"]) > 0)
                    {
                        Label lblIRS1 = (Label)e.Row.FindControl("lblIRS1");
                        Label lblMIC1 = (Label)e.Row.FindControl("lblMIC1");
                        Label lblBreakPoint1 = (Label)e.Row.FindControl("lblBreakPoint1");
                        Label lblRelease1 = (Label)e.Row.FindControl("lblRelease1");

                        Label lblIRS2 = (Label)e.Row.FindControl("lblIRS2");
                        Label lblMIC2 = (Label)e.Row.FindControl("lblMIC2");
                        Label lblBreakPoint2 = (Label)e.Row.FindControl("lblBreakPoint2");
                        Label lblRelease2 = (Label)e.Row.FindControl("lblRelease2");

                        Label lblIRS3 = (Label)e.Row.FindControl("lblIRS3");
                        Label lblMIC3 = (Label)e.Row.FindControl("lblMIC3");
                        Label lblBreakPoint3 = (Label)e.Row.FindControl("lblBreakPoint3");
                        Label lblRelease3 = (Label)e.Row.FindControl("lblRelease3");

                        Label lblIRS4 = (Label)e.Row.FindControl("lblIRS4");
                        Label lblMIC4 = (Label)e.Row.FindControl("lblMIC4");
                        Label lblBreakPoint4 = (Label)e.Row.FindControl("lblBreakPoint4");
                        Label lblRelease4 = (Label)e.Row.FindControl("lblRelease4");

                        Label lblIRS5 = (Label)e.Row.FindControl("lblIRS5");
                        Label lblMIC5 = (Label)e.Row.FindControl("lblMIC5");
                        Label lblBreakPoint5 = (Label)e.Row.FindControl("lblBreakPoint5");
                        Label lblRelease5 = (Label)e.Row.FindControl("lblRelease5");

                        lblIRS1.Visible = false;
                        lblMIC1.Visible = false;
                        lblBreakPoint1.Visible = false;
                        lblRelease1.Visible = false;

                        lblIRS2.Visible = false;
                        lblMIC2.Visible = false;
                        lblBreakPoint2.Visible = false;
                        lblRelease2.Visible = false;

                        lblIRS3.Visible = false;
                        lblMIC3.Visible = false;
                        lblBreakPoint3.Visible = false;
                        lblRelease3.Visible = false;

                        lblIRS4.Visible = false;
                        lblMIC4.Visible = false;
                        lblBreakPoint4.Visible = false;
                        lblRelease4.Visible = false;

                        lblIRS5.Visible = false;
                        lblMIC5.Visible = false;
                        lblBreakPoint5.Visible = false;
                        lblRelease5.Visible = false;
                    }

                    Label lblAntibioticName = (Label)e.Row.FindControl("lblAntibioticName");
                    lblAntibioticName.Text = common.myStr(dv[0][1]);

                    Literal ltrOrganismId1 = (Literal)e.Row.FindControl("ltrOrganismId1");
                    ltrOrganismId1.Text = common.myStr(dv[0][2]);

                    Literal ltrOrganismId1_IsResistant = (Literal)e.Row.FindControl("ltrOrganismId1_IsResistant");
                    ltrOrganismId1_IsResistant.Text = common.myStr(dv[0][3]);

                    TextBox txtIRS1 = (TextBox)e.Row.FindControl("txtIRS1");
                    txtIRS1.Text = common.myStr(dv[0][4]);

                    TextBox txtMIC1 = (TextBox)e.Row.FindControl("txtMIC1");
                    txtMIC1.Text = common.myStr(dv[0][5]);

                    TextBox txtBreakPoint1 = (TextBox)e.Row.FindControl("txtBreakPoint1");
                    txtBreakPoint1.Text = common.myStr(dv[0][6]);

                    CheckBox ChkRelease1 = (CheckBox)e.Row.FindControl("ChkRelease1");
                    ChkRelease1.Checked = common.myBool(dv[0][7]);

                    if (common.myStr(txtIRS1.Text) == "X")
                    {
                        txtIRS1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS1.ReadOnly = true;
                        txtMIC1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtMIC1.ReadOnly = true;
                        txtBreakPoint1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtBreakPoint1.ReadOnly = true;
                        ChkRelease1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        ChkRelease1.Enabled = false;
                    }
                    else if (common.myStr(ltrOrganismId1_IsResistant.Text) == "True")
                    {
                        txtIRS1.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS1.ForeColor = Color.Red;
                        txtIRS1.ReadOnly = true;
                    }
                    else
                    {
                        txtIRS1.Attributes.Add("onkeyup", "javascript:IRSValues('" + txtIRS1.ClientID + "','" + ChkRelease1.ClientID + "');");
                    }

                    Literal ltrOrganismId2 = (Literal)e.Row.FindControl("ltrOrganismId2");
                    ltrOrganismId2.Text = common.myStr(dv[0][8]);

                    Literal ltrOrganismId2_IsResistant = (Literal)e.Row.FindControl("ltrOrganismId2_IsResistant");
                    ltrOrganismId2_IsResistant.Text = common.myStr(dv[0][9]);

                    TextBox txtIRS2 = (TextBox)e.Row.FindControl("txtIRS2");
                    txtIRS2.Text = common.myStr(dv[0][10]);

                    TextBox txtMIC2 = (TextBox)e.Row.FindControl("txtMIC2");
                    txtMIC2.Text = common.myStr(dv[0][11]);

                    TextBox txtBreakPoint2 = (TextBox)e.Row.FindControl("txtBreakPoint2");
                    txtBreakPoint2.Text = common.myStr(dv[0][12]);

                    CheckBox ChkRelease2 = (CheckBox)e.Row.FindControl("ChkRelease2");
                    ChkRelease2.Checked = common.myBool(dv[0][13]);

                    if (common.myStr(txtIRS2.Text) == "X")
                    {
                        txtIRS2.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS2.ReadOnly = true;
                        txtMIC2.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtMIC2.ReadOnly = true;
                        txtBreakPoint2.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtBreakPoint2.ReadOnly = true;
                        ChkRelease2.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        ChkRelease2.Enabled = false;
                    }
                    else if (common.myStr(ltrOrganismId2_IsResistant.Text) == "True")
                    {
                        txtIRS2.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS2.ForeColor = Color.Red;
                        txtIRS2.ReadOnly = true;
                    }
                    else
                    {
                        txtIRS2.Attributes.Add("onkeyup", "javascript:IRSValues('" + txtIRS2.ClientID + "','" + ChkRelease2.ClientID + "');");
                    }

                    Literal ltrOrganismId3 = (Literal)e.Row.FindControl("ltrOrganismId3");
                    ltrOrganismId3.Text = common.myStr(dv[0][14]);

                    Literal ltrOrganismId3_IsResistant = (Literal)e.Row.FindControl("ltrOrganismId3_IsResistant");
                    ltrOrganismId3_IsResistant.Text = common.myStr(dv[0][15]);

                    TextBox txtIRS3 = (TextBox)e.Row.FindControl("txtIRS3");
                    txtIRS3.Text = common.myStr(dv[0][16]);

                    TextBox txtMIC3 = (TextBox)e.Row.FindControl("txtMIC3");
                    txtMIC3.Text = common.myStr(dv[0][17]);

                    TextBox txtBreakPoint3 = (TextBox)e.Row.FindControl("txtBreakPoint3");
                    txtBreakPoint3.Text = common.myStr(dv[0][18]);

                    CheckBox ChkRelease3 = (CheckBox)e.Row.FindControl("ChkRelease3");
                    ChkRelease3.Checked = common.myBool(dv[0][19]);

                    if (common.myStr(txtIRS3.Text) == "X")
                    {
                        txtIRS3.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS3.ReadOnly = true;
                        txtMIC3.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtMIC3.ReadOnly = true;
                        txtBreakPoint3.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtBreakPoint3.ReadOnly = true;
                        ChkRelease3.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        ChkRelease3.Enabled = false;
                    }
                    else if (common.myBool(ltrOrganismId3_IsResistant.Text))
                    {
                        txtIRS3.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS3.ForeColor = Color.Red;
                        txtIRS3.ReadOnly = true;
                    }
                    else
                    {
                        txtIRS3.Attributes.Add("onkeyup", "javascript:IRSValues('" + txtIRS3.ClientID + "','" + ChkRelease3.ClientID + "');");
                    }

                    Literal ltrOrganismId4 = (Literal)e.Row.FindControl("ltrOrganismId4");
                    ltrOrganismId4.Text = common.myStr(dv[0][20]);

                    Literal ltrOrganismId4_IsResistant = (Literal)e.Row.FindControl("ltrOrganismId4_IsResistant");
                    ltrOrganismId4_IsResistant.Text = common.myStr(dv[0][21]);

                    TextBox txtIRS4 = (TextBox)e.Row.FindControl("txtIRS4");
                    txtIRS4.Text = common.myStr(dv[0][22]);

                    TextBox txtMIC4 = (TextBox)e.Row.FindControl("txtMIC4");
                    txtMIC4.Text = common.myStr(dv[0][23]);

                    TextBox txtBreakPoint4 = (TextBox)e.Row.FindControl("txtBreakPoint4");
                    txtBreakPoint4.Text = common.myStr(dv[0][24]);

                    CheckBox ChkRelease4 = (CheckBox)e.Row.FindControl("ChkRelease4");
                    ChkRelease4.Checked = common.myBool(dv[0][25]);

                    if (common.myStr(txtIRS4.Text) == "X")
                    {
                        txtIRS4.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS4.ReadOnly = true;
                        txtMIC4.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtMIC4.ReadOnly = true;
                        txtBreakPoint4.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtBreakPoint4.ReadOnly = true;
                        ChkRelease4.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        ChkRelease4.Enabled = false;
                    }
                    else if (common.myBool(ltrOrganismId4_IsResistant.Text))
                    {
                        txtIRS4.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS4.ForeColor = Color.Red;
                        txtIRS4.ReadOnly = true;
                    }
                    else
                    {
                        txtIRS4.Attributes.Add("onkeyup", "javascript:IRSValues('" + txtIRS4.ClientID + "','" + ChkRelease4.ClientID + "');");
                    }

                    Literal ltrOrganismId5 = (Literal)e.Row.FindControl("ltrOrganismId5");
                    ltrOrganismId5.Text = common.myStr(dv[0][26]);

                    Literal ltrOrganismId5_IsResistant = (Literal)e.Row.FindControl("ltrOrganismId5_IsResistant");
                    ltrOrganismId5_IsResistant.Text = common.myStr(dv[0][27]);

                    TextBox txtIRS5 = (TextBox)e.Row.FindControl("txtIRS5");
                    txtIRS5.Text = common.myStr(dv[0][28]);

                    TextBox txtMIC5 = (TextBox)e.Row.FindControl("txtMIC5");
                    txtMIC5.Text = common.myStr(dv[0][29]);

                    TextBox txtBreakPoint5 = (TextBox)e.Row.FindControl("txtBreakPoint5");
                    txtBreakPoint5.Text = common.myStr(dv[0][30]);

                    CheckBox ChkRelease5 = (CheckBox)e.Row.FindControl("ChkRelease5");
                    ChkRelease5.Checked = common.myBool(dv[0][31]);

                    if (common.myStr(txtIRS5.Text) == "X")
                    {
                        txtIRS5.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS5.ReadOnly = true;
                        txtMIC5.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtMIC5.ReadOnly = true;
                        txtBreakPoint5.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtBreakPoint5.ReadOnly = true;
                        ChkRelease5.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        ChkRelease5.Enabled = false;
                    }
                    else if (common.myBool(ltrOrganismId5_IsResistant.Text))
                    {
                        txtIRS5.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                        txtIRS5.ForeColor = Color.Red;
                        txtIRS5.ReadOnly = true;
                    }
                    else
                    {
                        txtIRS5.Attributes.Add("onkeyup", "javascript:IRSValues('" + txtIRS5.ClientID + "','" + ChkRelease5.ClientID + "');");
                    }
                }
                ViewState["i"] = 1;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ArrayList coll = new ArrayList();
            StringBuilder strItemXML = new StringBuilder();
            Hashtable hshIn = new Hashtable();
            int iDiagSampleId = common.myInt(Request.QueryString["DiagSampleId"]);
            int iLabNo = common.myInt(Request.QueryString["LabNo"]);
            int iResultId = common.myInt(Request.QueryString["ResultId"]);
            int iUserId = common.myInt(Session["UserId"]);
            int count = 0;
            for (int i = 0; i < gvSensitivityResult.Rows.Count; i++)
            {
                Literal lblAntibioticId = (Literal)gvSensitivityResult.Rows[i].FindControl("ltrAntibioticId");
                TextBox txtIRS1 = (TextBox)gvSensitivityResult.Rows[i].FindControl("txtIRS1");

                if ((txtIRS1.Text.Trim().Length > 0) && (txtIRS1.Text.Trim() != "X")
                   && (txtIRS1.Text.Trim().ToUpper() == "R" || txtIRS1.Text.Trim().ToUpper() == "MS"
                   || txtIRS1.Text.Trim().ToUpper() == "S" || txtIRS1.Text.Trim().ToUpper() == "I"))
                {
                    count = 1;
                    Literal lblOrganismId1 = (Literal)gvSensitivityResult.Rows[i].FindControl("ltrOrganismId1");
                    TextBox txtMIC1 = (TextBox)gvSensitivityResult.Rows[i].FindControl("txtMIC1");
                    TextBox txtBreakPoint1 = (TextBox)gvSensitivityResult.Rows[i].FindControl("txtBreakPoint1");
                    CheckBox ChkRelease1 = (CheckBox)gvSensitivityResult.Rows[i].FindControl("ChkRelease1");
                    
                    coll.Add(common.myInt(iDiagSampleId));
                    coll.Add(common.myInt(lblAntibioticId.Text));

                    coll.Add(common.myInt(lblOrganismId1.Text));
                    coll.Add(common.myStr(txtIRS1.Text.ToUpper()));
                    coll.Add(common.myStr(txtMIC1.Text));
                    coll.Add(common.myStr(txtBreakPoint1.Text));
                    coll.Add(common.myBool(ChkRelease1.Checked));
                }
                else
                {
                    if (txtIRS1.Text.Trim() != "" && (txtIRS1.Text.Trim() != "X") 
                        && (txtIRS1.Text.Trim().ToUpper() != "R" || txtIRS1.Text.Trim().ToUpper() != "MS"
                        || txtIRS1.Text.Trim().ToUpper() != "S" || txtIRS1.Text.Trim().ToUpper() != "I"))
                    {
                        Alert.ShowAjaxMsg("Please enter only R / S / I / MS", Page);
                        return;
                    }
                    else
                    {
                        coll.Add(common.myInt(iDiagSampleId));
                        coll.Add(common.myInt(lblAntibioticId.Text));

                        coll.Add(0);
                        coll.Add("");
                        coll.Add("");
                        coll.Add("");
                        coll.Add(false);
                    }
                }

                TextBox txtIRS2 = (TextBox)gvSensitivityResult.Rows[i].FindControl("txtIRS2");
                if ((txtIRS2.Text.Trim().Length > 0) && (txtIRS2.Text.Trim() != "X") && (txtIRS2.Text.Trim().ToUpper() == "R" || txtIRS2.Text.Trim().ToUpper() == "MS"
                || txtIRS2.Text.Trim().ToUpper() == "S" || txtIRS2.Text.Trim().ToUpper() == "I"))
                {
                    count = 1;

                    Literal lblOrganismId2 = (Literal)gvSensitivityResult.Rows[i].FindControl("ltrOrganismId2");
                    TextBox txtMIC2 = (TextBox)gvSensitivityResult.Rows[i].FindControl("txtMIC2");
                    TextBox txtBreakPoint2 = (TextBox)gvSensitivityResult.Rows[i].FindControl("txtBreakPoint2");
                    CheckBox ChkRelease2 = (CheckBox)gvSensitivityResult.Rows[i].FindControl("ChkRelease2");
                    
                    coll.Add(common.myInt(lblOrganismId2.Text));
                    coll.Add(common.myStr(txtIRS2.Text.ToUpper()));
                    coll.Add(common.myStr(txtMIC2.Text));
                    coll.Add(common.myStr(txtBreakPoint2.Text));
                    coll.Add(common.myBool(ChkRelease2.Checked));
                }
                else
                {
                    if (txtIRS2.Text.Trim() != "" && (txtIRS2.Text.Trim() != "X") && (txtIRS2.Text.Trim().ToUpper() != "R" || txtIRS2.Text.Trim().ToUpper() != "MS"
                || txtIRS2.Text.Trim().ToUpper() != "S" || txtIRS2.Text.Trim().ToUpper() != "I"))
                    {
                        Alert.ShowAjaxMsg("Please enter only R / S / I/ MS ", Page);
                        return;
                    }
                    else
                    {
                        coll.Add(0);
                        coll.Add("");
                        coll.Add("");
                        coll.Add("");
                        coll.Add(false);
                    }
                }
                TextBox txtIRS3 = (TextBox)gvSensitivityResult.Rows[i].FindControl("txtIRS3");
                if ((txtIRS3.Text.Trim().Length > 0) && (txtIRS3.Text.Trim() != "X") && (txtIRS3.Text.Trim().ToUpper() == "R" || txtIRS3.Text.Trim().ToUpper() == "MS"
                || txtIRS3.Text.Trim().ToUpper() == "S" || txtIRS3.Text.Trim().ToUpper() == "I"))
                {
                    count = 1;
                    Literal lblOrganismId3 = (Literal)gvSensitivityResult.Rows[i].FindControl("ltrOrganismId3"); 
                    TextBox txtMIC3 = (TextBox)gvSensitivityResult.Rows[i].FindControl("txtMIC3");
                    TextBox txtBreakPoint3 = (TextBox)gvSensitivityResult.Rows[i].FindControl("txtBreakPoint3");
                    CheckBox ChkRelease3 = (CheckBox)gvSensitivityResult.Rows[i].FindControl("ChkRelease3");
                    
                    coll.Add(common.myInt(lblOrganismId3.Text));
                    coll.Add(common.myStr(txtIRS3.Text.ToUpper()));
                    coll.Add(common.myStr(txtMIC3.Text));
                    coll.Add(common.myStr(txtBreakPoint3.Text));
                    coll.Add(common.myBool(ChkRelease3.Checked));

                }
                else
                {
                    if (txtIRS3.Text.Trim() != "" && (txtIRS3.Text.Trim() != "X") && (txtIRS3.Text.Trim().ToUpper() != "R" || txtIRS3.Text.Trim().ToUpper() != "MS"
                || txtIRS3.Text.Trim().ToUpper() != "S" || txtIRS3.Text.Trim().ToUpper() != "I"))
                    {
                        Alert.ShowAjaxMsg("Please enter only R / S / I / MS", Page);
                        return;
                    }
                    else
                    {
                        coll.Add(0);
                        coll.Add("");
                        coll.Add("");
                        coll.Add("");
                        coll.Add(false);
                    }
                }
                TextBox txtIRS4 = (TextBox)gvSensitivityResult.Rows[i].FindControl("txtIRS4");
                if ((txtIRS4.Text.Trim().Length > 0) && (txtIRS4.Text.Trim() != "X") && (txtIRS4.Text.Trim().ToUpper() == "R" || txtIRS4.Text.Trim().ToUpper() == "MS"
                || txtIRS4.Text.Trim().ToUpper() == "S" || txtIRS4.Text.Trim().ToUpper() == "I"))
                {
                    count = 1;

                    Literal lblOrganismId4 = (Literal)gvSensitivityResult.Rows[i].FindControl("ltrOrganismId4");
                    TextBox txtMIC4 = (TextBox)gvSensitivityResult.Rows[i].FindControl("txtMIC4");
                    TextBox txtBreakPoint4 = (TextBox)gvSensitivityResult.Rows[i].FindControl("txtBreakPoint4");
                    CheckBox ChkRelease4 = (CheckBox)gvSensitivityResult.Rows[i].FindControl("ChkRelease4");
                    
                    coll.Add(common.myInt(lblOrganismId4.Text));
                    coll.Add(common.myStr(txtIRS4.Text.ToUpper()));
                    coll.Add(common.myStr(txtMIC4.Text));
                    coll.Add(common.myStr(txtBreakPoint4.Text));
                    coll.Add(common.myBool(ChkRelease4.Checked));
                }
                else
                {
                    if (txtIRS4.Text.Trim() != "" && (txtIRS4.Text.Trim() != "X") && (txtIRS4.Text.Trim().ToUpper() != "R" || txtIRS4.Text.Trim().ToUpper() != "MS"
                || txtIRS4.Text.Trim().ToUpper() != "S" || txtIRS4.Text.Trim().ToUpper() != "I"))
                    {
                        Alert.ShowAjaxMsg("Please enter only R / S / I / MS", Page);
                        return;
                    }
                    else
                    {
                        coll.Add(0);
                        coll.Add("");
                        coll.Add("");
                        coll.Add("");
                        coll.Add(false);
                    }
                }
                TextBox txtIRS5 = (TextBox)gvSensitivityResult.Rows[i].FindControl("txtIRS5");
                if ((txtIRS5.Text.Trim().Length > 0) && (txtIRS5.Text.Trim() != "X") && (txtIRS5.Text.Trim().ToUpper() == "R" || txtIRS5.Text.Trim().ToUpper() == "MS"
                || txtIRS5.Text.Trim().ToUpper() == "S" || txtIRS5.Text.Trim().ToUpper() == "I"))
                {
                    count = 1;
                    Literal lblOrganismId5 = (Literal)gvSensitivityResult.Rows[i].FindControl("ltrOrganismId5");
                    TextBox txtMIC5 = (TextBox)gvSensitivityResult.Rows[i].FindControl("txtMIC5");
                    TextBox txtBreakPoint5 = (TextBox)gvSensitivityResult.Rows[i].FindControl("txtBreakPoint5");
                    CheckBox ChkRelease5 = (CheckBox)gvSensitivityResult.Rows[i].FindControl("ChkRelease5");
                    
                    coll.Add(common.myInt(lblOrganismId5.Text));
                    coll.Add(common.myStr(txtIRS5.Text.ToUpper()));
                    coll.Add(common.myStr(txtMIC5.Text));
                    coll.Add(common.myStr(txtBreakPoint5.Text));
                    coll.Add(common.myBool(ChkRelease5.Checked));
                }
                else
                {
                    if (txtIRS5.Text.Trim() != "" && (txtIRS5.Text.Trim() != "X") && (txtIRS5.Text.Trim().ToUpper() != "R" || txtIRS5.Text.Trim().ToUpper() != "MS"
                || txtIRS5.Text.Trim().ToUpper() != "S" || txtIRS5.Text.Trim().ToUpper() != "I"))
                    {
                        Alert.ShowAjaxMsg("Please enter only R / S / I / MS", Page);
                        return;
                    }
                    else
                    {
                        coll.Add(0);
                        coll.Add("");
                        coll.Add("");
                        coll.Add("");
                        coll.Add(false);
                    }
                }
                if (count == 1)
                {
                    strItemXML.Append(common.setXmlTable(ref coll));
                    count = 0;
                }
                else
                {
                    coll = new ArrayList();
                }
            }

            string strSource = common.myStr(Request.QueryString["Source"]);

            objval = new BaseC.clsMicrobiology(sConString);
            objval.source = strSource;
            objval.iLabNo = iLabNo;
            objval.iDiagSampleId = iDiagSampleId;
            objval.iResultId = iResultId;
            objval.xmlSensitivityFormat = strItemXML.ToString();
            objval.iUserId = iUserId;
            objval.iServiceId = common.myInt(Request.QueryString["ServiceId"]);
            objval.iFieldId = common.myInt(Request.QueryString["FieldId"]);
           

            string strMsg = objval.SaveData(objval);
            
            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "window.close();", true);
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = strMsg;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

}
