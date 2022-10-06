using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;
using System.Text;
using System.IO;
using System.Xml;

public partial class ATD_BedTransfer1 : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DAL.DAL dl = new DAL.DAL();
    SqlDataReader dr;
    DataSet ds;
    BaseC.HospitalSetup baseHs;
    BaseC.ATD objATD;
    BaseC.EMRBilling objBed;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            dvConfirm.Visible = false;
            BaseC.ATD objbc = new BaseC.ATD(sConString);
            ds = new DataSet();
            dtpTransferDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]) + " hh:mm tt";
            dtpTransferDate.SelectedDate = System.DateTime.Now;
            if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                                                                    "isAcceptFutureDateTimeInBedTransfer", sConString).Equals("Y"))
            {
                dtpTransferDate.MaxDate = DateTime.Now;
            }
            BaseC.Security objSecurity = new BaseC.Security(sConString);
            if (common.myBool(objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAllowBedTransferDateInWard").ToString()))
            {
                dtpTransferDate.Enabled = true;
            }
            else
            {
                dtpTransferDate.Enabled = false;
            }
            if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                                                                    "isRequiredAllFacilityBed", sConString).Equals("Y"))
            {
               ViewState["isRequiredAllFacilityBed"] = "Y";
                ddlFacilityName.Enabled = true;
            }
            chkIsBedRetail.Visible = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                "isVisibleOldBedRetain", sConString).Equals("Y") ? false : true;
            tdtransfer.Visible = false;
            if (Request.QueryString["encno"] != "" && Request.QueryString["encno"] != null)
            {
                Session["Encounterno"] = Request.QueryString["encno"].ToString().Trim();
                txtregno.Text = Request.QueryString["Regno"].ToString().Trim();
                txtipno.Text = Session["Encounterno"].ToString();
            }
            if (common.myStr(Request.QueryString["BT"]) == "WM")
            {
                dtpTransferDate.Enabled = false;
            }

            if (common.myStr(Request.QueryString["ER"]).Equals("ER"))
            {
                BaseC.EMR objEMR = new BaseC.EMR(sConString);

                ds = objEMR.GetRegistrationId(common.myLong(txtregno.Text));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    hdnregno.Value = common.myStr(ds.Tables[0].Rows[0]["Id"]).Trim();
                    Session["RegistrationId"] = hdnregno.Value;
                    Session["encounterid"] = common.myStr(Request.QueryString["encid"]);
                }
                else
                {
                    lblmsg.Text = "Patient Can Not Find.";
                }
            }
            else
            {
                ds = objbc.GetRegistrationId(common.myLong(txtregno.Text));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    hdnregno.Value = ds.Tables[0].Rows[0]["id"].ToString().Trim();
                    Session["RegistrationId"] = hdnregno.Value;
                    Session["encounterid"] = ds.Tables[0].Rows[0]["EncounterId"].ToString().Trim();
                }
                else
                {
                    lblmsg.Text = "Patient Can Not Find.";
                }
            }

            GetFacility();
            bindfrombed();
            Bindward();
            fillbedcategory();
            fillData(common.myInt(Session["FacilityId"]));
            BindBillingCategory();



            if (Convert.ToString(txtregno.Text.Trim()) != "")
            {
                BindPatientHiddenDetails(common.myStr(txtregno.Text)); ;
            }
            else
            {


            }
            hdnIsPasswordRequired.Value = common.myStr(Request.QueryString["IsPasswordRequired"]);

            System.Collections.Generic.Dictionary<string, string> collHospitalSetupValues = new System.Collections.Generic.Dictionary<string, string>();

            collHospitalSetupValues = common.GetFlagValueHospitalSetupMultiple(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                    "WardBedTransferReqBedWardNullValue", sConString);

            if (collHospitalSetupValues.ContainsKey("WardBedTransferReqBedWardNullValue"))
                ViewState["WardBedTransferReqBedWardNullValue"] = collHospitalSetupValues["WardBedTransferReqBedWardNullValue"];

            if (common.myStr(ViewState["WardBedTransferReqBedWardNullValue"]).Equals("Y") && common.myStr(Request.QueryString["BT"]) == "WM")
            {
                tdtransfer.Visible = true;
                ltrltoward.Visible = false;
                txttowardname.Visible = false;
                txttoward.Visible = false;
                ltrltobedno.Visible = false;
                txttobedname.Visible = false;
                txttobedno.Visible = false;
                grvBedStatus.Enabled = false;
                ltrltobillingcategory.Visible = false;
                ddltobillingcategoryname.Visible = false;
                txttobedcategoryname.Visible = false;
                txttobedcategory.Visible = false;
                Label8.Visible = false;
            }
        }
    }
    protected void bindfrombed()
    {
        try
        {

            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            if (Request.QueryString["Regno"].ToString() != "" || txtregno.Text.Trim() != "")
            {
                string str = "select wm.WardId,wm.wardName,bcm.BedcategoryId,ios.ServiceName as BedCategoryName,bm.Id,bm.BedNo  from WardMaster wm " +
                       "INNER JOIN BedMaster bm ON wm.WardId=bm.WardId " +
                       "INNER JOIN BedCategoryMaster  bcm ON bm.BedCategoryId =bcm.BedcategoryId " +
                       "INNER JOIN ItemOfService ios ON ios.ServiceId = bcm.BedCategoryId  " +
                       "WHERE bm.BedNo='" + Request.QueryString["bedno"].ToString() + "'AND bm.FacilityId=" + common.myInt(ddlFacilityName.SelectedValue);
                ds = dl.FillDataSet(CommandType.Text, str);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtipno.Enabled = false;
                    txtwardid.Text = ds.Tables[0].Rows[0]["wardId"].ToString().Trim();
                    txtward.Text = ds.Tables[0].Rows[0]["wardName"].ToString().Trim();
                    txtbedcategoryid.Text = ds.Tables[0].Rows[0]["BedcategoryId"].ToString().Trim();
                    txtbedcategory.Text = ds.Tables[0].Rows[0]["BedCategoryName"].ToString().Trim();
                    txtbedid.Text = ds.Tables[0].Rows[0]["Id"].ToString().Trim();
                    txtbedno.Text = ds.Tables[0].Rows[0]["BedNo"].ToString().Trim();
                }
                ds.Clear();
                str = "";
                Hashtable hsIn = new Hashtable();
                hsIn.Add("@iEncounterId", common.myInt(Session["encounterid"]));
                str = "SELECT DISTINCT a.CurrentBillCategory, i.ServiceName, ids.ServiceName AS DesiredBedCategory, a.CurrentBedCategory FROM Admission a with(nolock) INNER JOIN ItemOfService i with(nolock) on a.CurrentBillCategory = i.ServiceId LEFT JOIN ItemOfService ids with(nolock) on a.DesiredBedCategory = ids.ServiceId WHERE a.EncounterId = @iEncounterId";
                ds = dl.FillDataSet(CommandType.Text, str, hsIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblBillingCategory.Text = ds.Tables[0].Rows[0]["ServiceName"].ToString().Trim();
                    lblDesiredBedCategory.Text = common.myStr(ds.Tables[0].Rows[0]["DesiredBedCategory"]).Trim();
                    txtCurrentBedCategoryid.Text = ds.Tables[0].Rows[0]["CurrentBedCategory"].ToString().Trim();
                    txtCurrentBillCategoryid.Text = ds.Tables[0].Rows[0]["CurrentBillCategory"].ToString().Trim();
                }
                ds.Clear();
                ds.Dispose();

            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    void BindPatientHiddenDetails(String RegistrationNo)
    {
        try
        {
            if (Session["PatientDetailString"] != null)
            {
                lblPatientDetail.Text = Session["PatientDetailString"].ToString();
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void grvBedStatus_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int noOfColumn = Convert.ToInt32(ViewState["noOfColumn"]);
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label label1 = (Label)e.Row.FindControl("label1");
                Label label2 = (Label)e.Row.FindControl("label2");
                Label label3 = (Label)e.Row.FindControl("label3");
                Label label4 = (Label)e.Row.FindControl("label4");
                Label label5 = (Label)e.Row.FindControl("label5");
                Label label6 = (Label)e.Row.FindControl("label6");
                Label label7 = (Label)e.Row.FindControl("label7");

                ImageButton imgbutton1 = (ImageButton)e.Row.FindControl("Imagebutton1");
                ImageButton imgbutton2 = (ImageButton)e.Row.FindControl("Imagebutton2");
                ImageButton imgbutton3 = (ImageButton)e.Row.FindControl("Imagebutton3");
                ImageButton imgbutton4 = (ImageButton)e.Row.FindControl("Imagebutton4");
                ImageButton imgbutton5 = (ImageButton)e.Row.FindControl("Imagebutton5");
                ImageButton imgbutton6 = (ImageButton)e.Row.FindControl("Imagebutton6");
                ImageButton imgbutton7 = (ImageButton)e.Row.FindControl("Imagebutton7");

                if (noOfColumn == 1)
                {
                    string[] str = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "0")).Split('@');

                    if (str.Length > 1)
                    {
                        label1.Text = str[0];

                        if (label1.Text != "")
                        {
                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[2] + "','" + str[3] + "','" + str[0] + "');");
                            }
                        }
                    }
                    else
                    {
                        imgbutton1.Visible = false;
                        if (imgbutton1.ImageUrl == "")
                        {
                            label1.Font.Bold = true;
                        }

                        e.Row.BackColor = System.Drawing.Color.FromName("#CFEBFC");
                        e.Row.Cells[0].ColumnSpan = 6;
                        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        e.Row.Height = Unit.Pixel(25);
                        HtmlControl div0 = (HtmlControl)e.Row.FindControl("div1");
                        div0.Attributes.Add("style", "border-width:0px;");
                    }
                    imgbutton2.Visible = false;
                    HtmlControl div1 = (HtmlControl)e.Row.FindControl("div2");
                    div1.Visible = false;
                    imgbutton3.Visible = false;
                    HtmlControl div2 = (HtmlControl)e.Row.FindControl("div3");
                    div2.Visible = false;
                    imgbutton4.Visible = false;
                    HtmlControl div3 = (HtmlControl)e.Row.FindControl("div4");
                    div3.Visible = false;
                    imgbutton5.Visible = false;
                    HtmlControl div4 = (HtmlControl)e.Row.FindControl("div5");
                    div4.Visible = false;
                    HtmlControl div6 = (HtmlControl)e.Row.FindControl("div6");
                    div6.Visible = false;
                    HtmlControl div7 = (HtmlControl)e.Row.FindControl("div7");
                    div7.Visible = false;
                }
                if (noOfColumn == 2)
                {
                    string[] str = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "0")).Split('@');
                    string[] str1 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "1")).Split('@');

                    label1.Text = str[0];
                    label2.Text = str1[0];

                    if (str.Length > 1)
                    {
                        if (label1.Text != "")
                        {
                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[2] + "','" + str[3] + "','" + str[0] + "');");
                            }
                        }
                    }
                    else
                    {
                        imgbutton1.Visible = false;
                        if (imgbutton1.ImageUrl == "")
                        {
                            label1.Font.Bold = true;
                        }

                        e.Row.BackColor = System.Drawing.Color.FromName("#CFEBFC");
                        e.Row.Cells[0].ColumnSpan = 6;
                        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        e.Row.Height = Unit.Pixel(25);
                        HtmlControl div0 = (HtmlControl)e.Row.FindControl("div1");
                        div0.Attributes.Add("style", "border-width:0px;");
                    }
                    if (str1.Length > 1)
                    {
                        if (label2.Text != "")
                        {
                            if (str1[1] == "V")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[2] + "','" + str1[3] + "','" + str1[0] + "');");
                            }
                        }
                    }
                    else
                    {
                        imgbutton2.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                        div.Visible = false;
                    }

                    imgbutton3.Visible = false;
                    HtmlControl div2 = (HtmlControl)e.Row.FindControl("div3");
                    div2.Visible = false;
                    imgbutton4.Visible = false;
                    HtmlControl div3 = (HtmlControl)e.Row.FindControl("div4");
                    div3.Visible = false;
                    imgbutton5.Visible = false;
                    HtmlControl div4 = (HtmlControl)e.Row.FindControl("div5");
                    div4.Visible = false;
                    HtmlControl div6 = (HtmlControl)e.Row.FindControl("div6");
                    div6.Visible = false;

                    HtmlControl div7 = (HtmlControl)e.Row.FindControl("div7");
                    div7.Visible = false;
                }
                if (noOfColumn == 3)
                {
                    string[] str = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "0")).Split('@');
                    string[] str1 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "1")).Split('@');
                    string[] str2 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "2")).Split('@');

                    label1.Text = str[0];
                    label2.Text = str1[0];
                    label3.Text = str2[0];

                    if (str.Length > 1)
                    {
                        if (label1.Text != "")
                        {
                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[2] + "','" + str[3] + "','" + str[0] + "');");
                            }
                        }
                    }
                    else
                    {
                        imgbutton1.Visible = false;
                        if (imgbutton1.ImageUrl == "")
                        {
                            label1.Font.Bold = true;
                        }

                        e.Row.BackColor = System.Drawing.Color.FromName("#CFEBFC");
                        e.Row.Cells[0].ColumnSpan = 6;
                        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        e.Row.Height = Unit.Pixel(25);

                        HtmlControl div0 = (HtmlControl)e.Row.FindControl("div1");
                        div0.Attributes.Add("style", "border-width:0px;");
                    }
                    if (str1.Length > 1)
                    {
                        if (label2.Text != "")
                        {
                            if (str1[1] == "V")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[2] + "','" + str1[3] + "','" + str1[0] + "' );");
                            }
                        }
                    }
                    else
                    {
                        imgbutton2.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                        div.Visible = false;
                    }
                    if (str2.Length > 1)
                    {
                        if (label3.Text != "")
                        {
                            if (str2[1] == "V")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[2] + "','" + str2[3] + "','" + str2[0] + "');");

                            }
                        }
                    }
                    else
                    {
                        imgbutton3.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                        div.Visible = false;
                    }


                    HtmlControl div3 = (HtmlControl)e.Row.FindControl("div4");
                    div3.Visible = false;

                    HtmlControl div4 = (HtmlControl)e.Row.FindControl("div5");
                    div4.Visible = false;

                    HtmlControl div5 = (HtmlControl)e.Row.FindControl("div6");
                    div5.Visible = false;
                    HtmlControl div7 = (HtmlControl)e.Row.FindControl("div7");
                    div7.Visible = false;

                }
                if (noOfColumn == 4)
                {
                    string[] str = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "0")).Split('@');
                    string[] str1 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "1")).Split('@');
                    string[] str2 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "2")).Split('@');
                    string[] str3 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "3")).Split('@');

                    label1.Text = str[0];
                    label2.Text = str1[0];
                    label3.Text = str2[0];
                    label4.Text = str3[0];

                    if (str.Length > 1)
                    {
                        if (label1.Text != "")
                        {
                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[2] + "','" + str[3] + "','" + str[0] + "');");
                            }
                        }
                    }
                    else
                    {
                        imgbutton1.Visible = false;
                        if (imgbutton1.ImageUrl == "")
                        {
                            label1.Font.Bold = true;
                        }

                        e.Row.BackColor = System.Drawing.Color.FromName("#CFEBFC");
                        e.Row.Cells[0].ColumnSpan = 6;
                        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        e.Row.Height = Unit.Pixel(25);

                        HtmlControl div0 = (HtmlControl)e.Row.FindControl("div1");
                        div0.Attributes.Add("style", "border-width:0px;");
                    }
                    if (str1.Length > 1)
                    {
                        if (label2.Text != "")
                        {
                            if (str1[1] == "V")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[2] + "','" + str1[3] + "','" + str1[0] + "');");
                            }

                        }
                    }
                    else
                    {
                        imgbutton2.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                        div.Visible = false;
                    }
                    if (str2.Length > 1)
                    {
                        if (label3.Text != "")
                        {
                            if (str2[1] == "V")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[2] + "','" + str2[3] + "','" + str2[0] + "');");
                            }
                        }
                    }
                    else
                    {
                        imgbutton3.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                        div.Visible = false;
                    }
                    if (str3.Length > 1)
                    {
                        if (label4.Text != "")
                        {
                            if (str3[1] == "V")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[2] + "','" + str3[3] + "','" + str3[0] + "' );");
                            }
                        }
                    }
                    else
                    {
                        imgbutton4.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                        div.Visible = false;
                    }

                    HtmlControl div4 = (HtmlControl)e.Row.FindControl("div5");
                    div4.Visible = false;

                    HtmlControl div5 = (HtmlControl)e.Row.FindControl("div6");
                    div5.Visible = false;

                    HtmlControl div7 = (HtmlControl)e.Row.FindControl("div7");
                    div7.Visible = false;

                }
                if (noOfColumn == 5)
                {
                    string[] str = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "0")).Split('@');
                    string[] str1 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "1")).Split('@');
                    string[] str2 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "2")).Split('@');
                    string[] str3 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "3")).Split('@');
                    string[] str4 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "4")).Split('@');

                    label1.Text = str[0];
                    label2.Text = str1[0];
                    label3.Text = str2[0];
                    label4.Text = str3[0];
                    label5.Text = str4[0];

                    if (str.Length > 1)
                    {
                        if (label1.Text != "")
                        {
                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[2] + "','" + str[3] + "','" + str[0] + "' );");
                            }
                        }
                    }
                    else
                    {
                        imgbutton1.Visible = false;
                        if (imgbutton1.ImageUrl == "")
                            label1.Font.Bold = true;

                        e.Row.BackColor = System.Drawing.Color.FromName("#CFEBFC");
                        e.Row.Cells[0].ColumnSpan = 6;
                        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        e.Row.Height = Unit.Pixel(25);
                    }
                    if (str1.Length > 1)
                    {
                        if (label2.Text != "")
                        {
                            if (str1[1] == "V")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[2] + "','" + str1[3] + "','" + str1[0] + "');");
                            }
                        }
                    }
                    else
                    {
                        imgbutton2.Visible = false;
                        if (imgbutton1.ImageUrl == "")
                        {
                            label1.Font.Bold = true;
                        }

                        HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                        div.Visible = false;
                    }
                    if (str2.Length > 1)
                    {
                        if (label3.Text != "")
                        {
                            if (str2[1] == "V")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[2] + "','" + str2[3] + "','" + str2[0] + "' );");

                            }
                        }
                    }
                    else
                    {
                        imgbutton3.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                        div.Visible = false;
                    }
                    if (str3.Length > 1)
                    {
                        if (label4.Text != "")
                        {
                            if (str3[1] == "V")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[2] + "','" + str3[3] + "','" + str3[0] + "' );");
                            }
                        }
                    }
                    else
                    {
                        imgbutton4.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                        div.Visible = false;
                    }
                    if (str4.Length > 1)
                    {
                        if (label5.Text != "")
                        {
                            if (str4[1] == "V")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[2] + "','" + str4[3] + "','" + str4[0] + "' );");
                            }

                        }
                    }
                    else
                    {
                        imgbutton5.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div5");
                        div.Visible = false;
                    }

                    HtmlControl div6 = (HtmlControl)e.Row.FindControl("div6");
                    div6.Visible = false;
                    HtmlControl div7 = (HtmlControl)e.Row.FindControl("div7");
                    div7.Visible = false;
                }
                if (noOfColumn == 6)
                {
                    string[] str = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "0")).Split('@');
                    string[] str1 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "1")).Split('@');
                    string[] str2 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "2")).Split('@');
                    string[] str3 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "3")).Split('@');
                    string[] str4 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "4")).Split('@');
                    string[] str5 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "5")).Split('@');

                    label1.Text = str[0];
                    label2.Text = str1[0];
                    label3.Text = str2[0];
                    label4.Text = str3[0];
                    label5.Text = str4[0];
                    label6.Text = str5[0];

                    if (str.Length > 1)
                    {
                        if (label1.Text != "")
                        {
                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[2] + "','" + str[3] + "','" + str[0] + "' );");
                            }
                        }
                    }
                    else
                    {
                        imgbutton1.Visible = false;
                        if (imgbutton1.ImageUrl == "")
                        {
                            label1.Font.Bold = true;
                        }

                        e.Row.BackColor = System.Drawing.Color.FromName("#CFEBFC");
                        e.Row.Cells[0].ColumnSpan = 6;
                        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        e.Row.Height = Unit.Pixel(25);

                        HtmlControl div0 = (HtmlControl)e.Row.FindControl("div1");
                        div0.Attributes.Add("style", "border-width:0px;");
                    }
                    if (str1.Length > 1)
                    {
                        if (label2.Text != "")
                        {
                            if (str1[1] == "V")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[2] + "','" + str1[3] + "','" + str1[0] + "');");
                            }
                        }
                    }
                    else
                    {
                        imgbutton2.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                        div.Visible = false;
                    }
                    if (str2.Length > 1)
                    {
                        if (label3.Text != "")
                        {
                            if (str2[1] == "V")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[2] + "','" + str2[3] + "','" + str2[0] + "' );");
                            }
                        }
                    }
                    else
                    {
                        imgbutton3.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                        div.Visible = false;
                    }
                    if (str3.Length > 1)
                    {
                        if (label4.Text != "")
                        {
                            if (str3[1] == "V")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[2] + "','" + str3[3] + "','" + str3[0] + "' );");
                            }
                        }
                    }
                    else
                    {
                        imgbutton4.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                        div.Visible = false;
                    }
                    if (str4.Length > 1)
                    {
                        if (label5.Text != "")
                        {
                            if (str4[1] == "V")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[2] + "','" + str4[3] + "','" + str4[0] + "' );");
                            }
                        }
                    }
                    else
                    {
                        imgbutton5.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div5");
                        div.Visible = false;
                    }
                    if (str5.Length > 1)
                    {
                        if (label6.Text != "")
                        {
                            if (str5[1] == "V")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[2] + "','" + str5[3] + "','" + str5[0] + "' );");
                            }
                        }
                    }
                    else
                    {
                        imgbutton6.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div6");
                        div.Visible = false;
                    }

                    HtmlControl div6 = (HtmlControl)e.Row.FindControl("div7");
                    div6.Visible = false;
                }

                if (noOfColumn == 7)
                {
                    string[] str = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "0")).Split('@');
                    string[] str1 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "1")).Split('@');
                    string[] str2 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "2")).Split('@');
                    string[] str3 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "3")).Split('@');
                    string[] str4 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "4")).Split('@');
                    string[] str5 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "5")).Split('@');
                    string[] str6 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "6")).Split('@');

                    label1.Text = str[0];
                    label2.Text = str1[0];
                    label3.Text = str2[0];
                    label4.Text = str3[0];
                    label5.Text = str4[0];
                    label6.Text = str5[0];
                    label7.Text = str6[0];

                    if (str.Length > 1)
                    {
                        if (label1.Text != "")
                        {
                            if (str[1] == "V")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[2] + "','" + str[3] + "','" + str[0] + "' );");


                            }
                            else if (str[1] == "O")
                            {
                                imgbutton1.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton1.Attributes.Add("OnClick", "showMenu(event,'" + str[2] + "','" + str[3] + "','" + str[0] + "' );");
                            }
                        }
                    }
                    else
                    {
                        imgbutton1.Visible = false;
                        if (imgbutton1.ImageUrl == "")
                            label1.Font.Bold = true;

                        e.Row.BackColor = System.Drawing.Color.FromName("#CFEBFC");
                        e.Row.Cells[0].ColumnSpan = 6;
                        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        e.Row.Height = Unit.Pixel(25);
                        HtmlControl div0 = (HtmlControl)e.Row.FindControl("div1");
                        div0.Attributes.Add("style", "border-width:0px;");
                    }
                    if (str1.Length > 1)
                    {
                        if (label2.Text != "")
                        {
                            if (str1[1] == "V")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[2] + "','" + str1[3] + "','" + str1[0] + "');");
                            }
                            else if (str1[1] == "O")
                            {
                                imgbutton2.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton2.Attributes.Add("OnClick", "showMenu(event,'" + str1[2] + "','" + str1[3] + "','" + str1[0] + "');");
                            }
                        }
                    }
                    else
                    {
                        imgbutton2.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div2");
                        div.Visible = false;
                    }
                    if (str2.Length > 1)
                    {
                        if (label3.Text != "")
                        {
                            if (str2[1] == "V")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[2] + "','" + str2[3] + "','" + str2[0] + "' );");

                            }
                            else if (str2[1] == "O")
                            {
                                imgbutton3.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton3.Attributes.Add("OnClick", "showMenu(event,'" + str2[2] + "','" + str2[3] + "','" + str2[0] + "' );");
                            }
                        }
                    }
                    else
                    {
                        imgbutton3.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div3");
                        div.Visible = false;
                    }
                    if (str3.Length > 1)
                    {
                        if (label4.Text != "")
                        {
                            if (str3[1] == "V")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[2] + "','" + str3[3] + "','" + str3[0] + "' );");
                            }
                            else if (str3[1] == "O")
                            {
                                imgbutton4.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton4.Attributes.Add("OnClick", "showMenu(event,'" + str3[2] + "','" + str3[3] + "','" + str3[0] + "' );");
                            }
                        }
                    }
                    else
                    {
                        imgbutton4.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div4");
                        div.Visible = false;
                    }
                    if (str4.Length > 1)
                    {
                        if (label5.Text != "")
                        {
                            if (str4[1] == "V")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[2] + "','" + str4[3] + "','" + str4[0] + "' );");
                            }
                            else if (str4[1] == "O")
                            {
                                imgbutton5.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton5.Attributes.Add("OnClick", "showMenu(event,'" + str4[2] + "','" + str4[3] + "','" + str4[0] + "' );");
                            }
                        }
                    }
                    else
                    {
                        imgbutton5.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div5");
                        div.Visible = false;
                    }
                    if (str5.Length > 1)
                    {
                        if (label6.Text != "")
                        {
                            if (str5[1] == "V")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[2] + "','" + str5[3] + "','" + str5[0] + "' );");
                            }
                            else if (str5[1] == "O")
                            {
                                imgbutton6.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton6.Attributes.Add("OnClick", "showMenu(event,'" + str5[2] + "','" + str5[3] + "','" + str5[0] + "' );");
                            }
                        }
                    }
                    else
                    {
                        imgbutton6.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div6");
                        div.Visible = false;
                    }
                    if (str6.Length > 1)
                    {
                        if (label7.Text != "")
                        {
                            if (str6[1] == "V")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bed-only.jpg";
                                imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[2] + "','" + str6[3] + "','" + str6[0] + "' );");
                            }
                            else if (str6[1] == "O")
                            {
                                imgbutton7.ImageUrl = "~/Images/Bed/bed3.jpg";
                                imgbutton7.Attributes.Add("OnClick", "showMenu(event,'" + str6[2] + "','" + str6[3] + "','" + str6[0] + "' );");
                            }
                        }
                    }
                    else
                    {
                        imgbutton7.Visible = false;
                        HtmlControl div = (HtmlControl)e.Row.FindControl("div7");
                        div.Visible = false;
                    }
                }

            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    public void fillData( int Facilityid)
    {
        try
        {
            BaseC.ATD Objstatus = new BaseC.ATD(sConString);
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataTable dt = new DataTable();

            DataSet objds = new DataSet();
            int i = 0, noOfColumn = 7;
            DataRow dr1 = null;
            string wardname = "";

            Hashtable hsinput = new Hashtable();
            hsinput.Add("@IntBedCat", ddlbedcategory.SelectedValue);
            hsinput.Add("@IntWardno", ddlward.SelectedValue);
            hsinput.Add("@intFacilityId", Facilityid);


            if (common.myStr(Request.QueryString["ER"]) == "ER")
            {
                objds = dl.FillDataSet(CommandType.StoredProcedure, "USPGetVacantERbedStatus", hsinput);
            }
            else
            {
                if (common.myStr(Request.QueryString["BT"]) == "WM")
                {
                    hsinput.Add("@Source", "BTR");

                }
                else
                { hsinput.Add("@Source", "BT"); }

                objds = dl.FillDataSet(CommandType.StoredProcedure, "USPGetVacantbedStatus", hsinput);
            }
            //  objds = Objstatus.FillBedStatus(common.myInt(ddlbedcategory.SelectedValue), common.myStr(ddlBedStatus.SelectedValue), 0, common.myStr(ddlwardfloor.SelectedValue));

            foreach (DataRow dr in objds.Tables[0].Rows)
            {
                DataColumn dc = new DataColumn();
                dc.ColumnName = Convert.ToString(i);
                dt.Columns.Add(dc);
                i++;
                if (i == noOfColumn)
                {
                    break;
                }
            }
            int l = 0;
            for (int k = 0; objds.Tables[0].Rows.Count != k; k++)
            {
                for (int j = 0; j != noOfColumn; j++)
                {
                    if (objds.Tables[0].Rows.Count == k)
                    {
                        break;
                    }
                    else
                    {
                        if (k == 0)
                        {
                            dr1 = dt.NewRow();
                            wardname = Convert.ToString(objds.Tables[0].Rows[k]["WardName"]);
                            dr1[0] = wardname;
                            dt.Rows.Add(dr1);
                            l = 0;
                            dr1 = dt.NewRow();
                            dr1[l] = Convert.ToString(objds.Tables[0].Rows[k]["BedNo"]) + "@" + Convert.ToString(objds.Tables[0].Rows[k]["BedStatus"]) + "@" + Convert.ToString(objds.Tables[0].Rows[k]["wardNo"]) + "@" + Convert.ToString(objds.Tables[0].Rows[k]["BedCatg"]);
                            l++;
                            if (objds.Tables[0].Rows.Count == k + 1)
                            {
                                dt.Rows.Add(dr1);
                            }
                            break;

                        }
                        else if (wardname == Convert.ToString(objds.Tables[0].Rows[k]["WardName"]))
                        {

                            if (l == noOfColumn)
                            {
                                k--;
                                dt.Rows.Add(dr1);
                                dr1 = dt.NewRow();
                                l = 0;
                                break;
                            }


                            dr1[l] = Convert.ToString(objds.Tables[0].Rows[k]["BedNo"]) + "@" + Convert.ToString(objds.Tables[0].Rows[k]["BedStatus"]) + "@" + Convert.ToString(objds.Tables[0].Rows[k]["wardNo"]) + "@" + Convert.ToString(objds.Tables[0].Rows[k]["BedCatg"]);
                            if (objds.Tables[0].Rows.Count == k + 1)
                            {
                                dt.Rows.Add(dr1);
                            }
                            l++;
                            k++;

                        }
                        else
                        {
                            dt.Rows.Add(dr1);
                            dr1 = dt.NewRow();
                            wardname = Convert.ToString(objds.Tables[0].Rows[k]["WardName"]);
                            dr1[0] = wardname;
                            dt.Rows.Add(dr1);
                            l = 0;
                            dr1 = dt.NewRow();
                            dr1[l] = Convert.ToString(objds.Tables[0].Rows[k]["BedNo"]) + "@" + Convert.ToString(objds.Tables[0].Rows[k]["BedStatus"]) + "@" + Convert.ToString(objds.Tables[0].Rows[k]["wardNo"]) + "@" + Convert.ToString(objds.Tables[0].Rows[k]["BedCatg"]);
                            l++;
                            if (objds.Tables[0].Rows.Count == k + 1)
                            {
                                dt.Rows.Add(dr1);
                            }
                            break;
                        }
                    }
                    if (l == noOfColumn)
                    {
                        if (objds.Tables[0].Rows.Count != k)
                        {
                            k--;
                            dt.Rows.Add(dr1);
                            dr1 = dt.NewRow();
                            l = 0;
                            break;
                        }

                    }
                }

                if (objds.Tables[0].Rows.Count == k)
                {
                    break;
                }
            }
            noOfColumn = dt.Columns.Count;
            ViewState["noOfColumn"] = noOfColumn;
            grvBedStatus.DataSource = dt;
            grvBedStatus.DataBind();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ddlward_SelectedIndexChanged(object sender, EventArgs e)
    {

        BaseC.EMRBilling objBed = new BaseC.EMRBilling(sConString);
        ds = new DataSet();
        DataView dv = new DataView();
        DataTable dtEntry = new DataTable();
        try
        {
            ds = objBed.GetWardBedCategoryWiseBedNo(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt16(Session["FacilityId"]), common.myInt(ddlward.SelectedValue), 0);
            dv = new DataView(ds.Tables[0]);
            dtEntry = dv.ToTable(true, "BedCategoryId", "BedCategoryName");

            ddlbedcategory.DataSource = null;
            ddlbedcategory.DataBind();

            //if (dtEntry.Rows.Count > 0)
            //{
            ddlbedcategory.DataSource = dtEntry;
            ddlbedcategory.DataTextField = "BedCategoryName";
            ddlbedcategory.DataValueField = "BedCategoryId";
            ddlbedcategory.DataBind();

            ddlbedcategory.Items.Insert(0, "All");
            ddlbedcategory.Items[0].Value = "0";

            ddlbedcategory.SelectedIndex = 0;

            ddlbedcategory_SelectedIndexChanged(this, null);
            //}
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            ds.Dispose();
            dv.Dispose();
            dtEntry.Dispose();
        }
        // fillData();

    }
    void fillbedcategory()
    {
        try
        {

            DataView dv = new DataView();
            DataTable dtbedcategory = new DataTable();
            // dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            // Hashtable hshIn = new Hashtable();
            //hshIn.Add("@HospitalLocationID", Convert.ToInt32(Session["HospitalLocationID"]));
            BaseC.ATD objBed = new BaseC.ATD(sConString);
            ds = objBed.GetBedCategory(common.myInt(ddlFacilityName.SelectedValue));
            //dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select b.BedCategoryId,i.ServiceName as BedCategoryName from ItemOfService i inner join BedCategoryMaster b on b.BedCategoryId =i.ServiceId and i.Active=1");
            dv = new DataView(ds.Tables[0]);


            if (common.myStr(Request.QueryString["ER"]) == "ER")
            {
                dv.RowFilter = "bedcategorytype='E'";
            }
            dtbedcategory = dv.ToTable(true, "BedCategoryId", "BedCategoryName");

            ddlbedcategory.DataSource = dtbedcategory;
            ddlbedcategory.DataTextField = "BedCategoryName";
            ddlbedcategory.DataValueField = "BedCategoryId";
            ddlbedcategory.DataBind();
            //dr.Close();
            ddlbedcategory.Items.Insert(0, "All");
            ddlbedcategory.Items[0].Value = "0";

            // ds.Clear();
            //ds.Dispose();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ddlbedcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillData(common.myInt(ddlFacilityName.SelectedValue));
    }
    protected void Bindward()
    {
        try
        {
            BaseC.ATD objbase = new BaseC.ATD(sConString);
            DataSet ds = new DataSet();
            ds = objbase.GetWard(common.myInt(ddlFacilityName.SelectedValue));
            ddlward.DataSource = ds;
            ddlward.DataTextField = "wardName";
            ddlward.DataValueField = "WardId";
            ddlward.DataBind();

            ddlward.Items.Insert(0, "All");
            ddlward.Items[0].Value = "0";

        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }
    protected void RadContextMenu1_ItemClick(object sender, RadMenuEventArgs e)
    {
        try
        {
            if (RadContextMenu1.SelectedValue == "BT")
            {
                if ((Convert.ToString(Session["RegistrationID"]) != "" && Convert.ToString(Session["EncounterId"]) != "") || (txtregno.Text.Trim() != "" && txtipno.Text.Trim() != ""))
                {
                    tdtransfer.Visible = true;
                    Bindtobed();

                    lblmsg.Text = "";


                }
                else
                {
                    Alert.ShowAjaxMsg("Please Select Patient !", Page);

                }
                //  ModalPopupExtender.Show();
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnfind_Click(object sender, EventArgs e)
    {
        fillData(common.myInt(ddlFacilityName.SelectedValue));
    }

    private void CheckUserRights()
    {
        BaseC.Security objSecurity = new BaseC.Security(sConString);
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable hshin = new Hashtable();

            hshin.Add("@intBedCategoryId", common.myInt(lblbedcetogry.Text.Trim()));

            string str = "SELECT ISNULL(IsChangeBillCategoryAuthReq,0) AS IsChangeBillCategoryAuthReq FROM BedCategoryMaster WITH(NOLOCK) WHERE BedCategoryId=@intBedCategoryId";
            ds = dl.FillDataSet(CommandType.Text, str, hshin);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (common.myBool(ds.Tables[0].Rows[0]["IsChangeBillCategoryAuthReq"]))
                {
                    bool IsValidate = objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAuthorizedForChangeIPBillingCategory");
                    if (IsValidate)
                    {
                        ddltobillingcategoryname.Enabled = true;
                    }
                    else
                    {
                        ddltobillingcategoryname.Enabled = false;
                    }
                }
                else
                {
                    ddltobillingcategoryname.Enabled = true;
                }
            }
            else
            {
                ddltobillingcategoryname.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally { objSecurity = null; }
    }
    public void Bindtobed()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable hsinput = new Hashtable();
            hsinput.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            hsinput.Add("@intFacilityId", common.myInt(ddlFacilityName.SelectedValue));
            hsinput.Add("@intBedCategoryId", lblbedcetogry.Text.Trim());
            hsinput.Add("@intWardId", lblwardno.Text.Trim());
            hsinput.Add("@chvBedNo", lblbeno.Text.Trim());
            if (common.myStr(Request.QueryString["ER"]) == "ER")
            {
                ds = dl.FillDataSet(CommandType.StoredProcedure, "ERAtdGetBedStatusBetCategorywise", hsinput);
            }
            else
            {
                ds = dl.FillDataSet(CommandType.StoredProcedure, "IpAtdGetBedStatusBetCategorywise", hsinput);
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                txttowardname.Text = "";
                txttoward.Text = "";
                txttobedcategoryname.Text = "";
                txttobedcategory.Text = "";
                txttobedno.Text = "";
                txttobedname.Text = "";
                // ddltobillingcategoryname.SelectedIndex = 0;

                txttowardname.Text = ds.Tables[0].Rows[0]["WardName"].ToString().Trim();
                txttoward.Text = ds.Tables[0].Rows[0]["wardNo"].ToString().Trim();
                txttobedcategoryname.Text = ds.Tables[0].Rows[0]["BedcategoryName"].ToString().Trim();
                txttobedcategory.Text = ds.Tables[0].Rows[0]["BedCatg"].ToString().Trim();
                txttobedno.Text = ds.Tables[0].Rows[0]["BedId"].ToString().Trim();
                txttobedname.Text = ds.Tables[0].Rows[0]["BedNo"].ToString().Trim();
                //  txttobillingcategoryname.Text = ds.Tables[0].Rows[0]["BedcategoryName"].ToString().Trim();
               if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                                                                   "IsBillingCategorysameforWard", sConString).Equals("Y"))
                {
                    ddltobillingcategoryname.SelectedIndex = ddltobillingcategoryname.Items.IndexOf(ddltobillingcategoryname.Items.FindItemByText(lblBillingCategory.Text));
                    ddltobillingcategoryname.Enabled = false;
                }
                else
                {
                    ddltobillingcategoryname.SelectedValue = ds.Tables[0].Rows[0]["BedCatg"].ToString().Trim();
                    ddltobillingcategoryname.Enabled = true;
                }
            }
            CheckUserRights();
            ds.Clear();
            ds.Dispose();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    public bool IsSave()
    {
        bool issave = true;
        string strmsg = "";

        if (!common.myStr(ViewState["WardBedTransferReqBedWardNullValue"]).Equals("Y") && !common.myStr(Request.QueryString["BT"]).Equals("WM"))
        {
            if (common.myStr(txttobedno.Text) == "")
            {
                issave = false;
                strmsg += "Please select bed for transfer ! ";
            }


        if (common.myInt(ddltobillingcategoryname.SelectedValue).Equals(0))
        {
            issave = false;
            strmsg += "Please select billing category ! ";
        }

            if (common.myInt(ddltobillingcategoryname.SelectedItem.Attributes["AllowedAge"]) > 0
                    && common.myInt(Request.QueryString["AgeYear"]) > common.myInt(ddltobillingcategoryname.SelectedItem.Attributes["AllowedAge"]))
            {
                issave = false;
                strmsg += " Age of this patient is more than " + common.myInt(ddltobillingcategoryname.SelectedItem.Attributes["AllowedAge"]) + " years";
                Alert.ShowAjaxMsg(strmsg, Page);
            }
        }
        lblmsg.Text = "";
        if (strmsg != "")
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = strmsg;
        }

        return issave;
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!IsSave())
            {
                return;
            }
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

            string FlagToCheckPendingBedRequest = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "NeedToIntimateForPendingbedRequest", sConString);
            if (FlagToCheckPendingBedRequest == "Y")
            {
                if (common.myStr(Request.QueryString["BT"]) != "WM" && common.myStr(Request.QueryString["ER"]) != "ER")
                {
                    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    Hashtable hshtableout = new Hashtable();
                    Hashtable hshtablein = new Hashtable();

                    hshtablein.Add("@intEncounterId", Session["EncounterId"]);
                    hshtablein.Add("@FromBedId", txtbedid.Text);
                    hshtableout.Add("@StrStatus", SqlDbType.VarChar);

                    hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "CheckForExistingBedRequest", hshtablein, hshtableout);

                    if (common.myStr(hshtableout["@StrStatus"]).Length > 0)
                    {
                        lblConfirm.Text = common.myStr(hshtableout["@StrStatus"]);
                        dvConfirm.Visible = true;
                        return;
                    }
                }
            }
            if (common.myInt(txttobedcategory.Text) != common.myInt(ddltobillingcategoryname.SelectedValue))
            {
                lblConfirm.Text = "Bed category and billing category are different. Do you want to save ?";
                dvConfirm.Visible = true;
                return;
            }
            else
            {
                if (common.myBool(hdnIsPasswordRequired.Value)) //If transactional username & password required
                {
                    IsValidPassword();
                    return;
                }
                saveRecord();
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnYes_OnClick(object sender, EventArgs e)
    {
        if (common.myBool(hdnIsPasswordRequired.Value)) //If transactional username & password required
        {
            IsValidPassword();
            return;
        }

        ViewState["Comapare"] = "Yes";
        saveRecord();

    }
    protected void btnCancel_OnClick(object sender, EventArgs e)
    {

        dvConfirm.Visible = false;
    }
    protected void saveRecord()
    {
        lblmsg.Text = "";
        objATD = new BaseC.ATD(sConString);
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hshtableout = new Hashtable();
        Hashtable hshtablein = new Hashtable();
        int ToBedId = common.myInt(txttobedno.Text.Trim());
        int ToWardId = common.myInt(txttoward.Text.Trim());
        int ToBillingCategoryId = common.myInt(ddltobillingcategoryname.SelectedValue);
        int ToBedCategoryId = common.myInt(txttobedcategory.Text.Trim());


        if (common.myStr(Request.QueryString["BT"]) == "WM")
        {

            if (common.myStr(ViewState["WardBedTransferReqBedWardNullValue"]).Equals("Y"))
            {
                ToBedId = 0;
                ToWardId = 0;
                ToBillingCategoryId = 0;
                ToBedCategoryId = 0;
            }

            string msg = objATD.SaveBedTansferRequest(
                  common.myInt(Session["HospitalLocationID"]),
                  common.myInt(Session["EncounterId"]),
                  common.myInt(Session["RegistrationId"]),
                  common.myInt(txtCurrentBedCategoryid.Text),
                  common.myInt(txtCurrentBillCategoryid.Text),
                  common.myInt(txtbedid.Text),
                  common.myInt(txtwardid.Text.Trim()),
                 ToBedCategoryId,
                  ToBillingCategoryId,
                  ToBedId, ToWardId,
                   common.myBool(hdnIsPasswordRequired.Value) ? common.myInt(PasswordRequiredHelper.GetTransactionUserId()) : common.myInt(Session["UserId"]), // To swapping between current userid & transaction userid
                   dtpTransferDate.SelectedDate.Value, txtRemarks.Text);
            lblmsg.ForeColor = System.Drawing.Color.Green;
            lblmsg.Text = msg;
            Alert.ShowAjaxMsg(msg, Page);

            string fileLoc = Server.MapPath("~/PatientDocuments/Notofication_SB" + common.myInt(Session["FacilityId"]) + ".txt");
            string read = common.ReadNotificationFile(fileLoc);
            DataSet ds = new DataSet("table1");
            string ShowNotification = "N";
            string Count = "0";
            string StatusType = "";

            if (read != "")
            {
                string xmlSchema = common.myStr(read);
                StringReader sr = new StringReader(xmlSchema);
                ds.ReadXml(sr);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int count = common.myInt(ds.Tables[0].Rows[0]["BTRCount"]);
                    ds.Tables[0].Rows[0]["ShowNotification"] = "Y";
                    ds.Tables[0].Rows[0]["BTRCount"] = count + 1;
                    ds.Tables[0].Rows[0]["StatusType"] = "SB";
                    ds.Tables[0].AcceptChanges();
                    System.Text.StringBuilder builder = new System.Text.StringBuilder();// Create the StringWriter object with the StringBuilder object.
                    System.IO.StringWriter writer = new System.IO.StringWriter(builder);// Write the schema into the StringWriter.
                    ds.Tables[0].WriteXml(writer);
                    //put schema in string
                    string xmlSchemaWrite = writer.ToString();
                    XmlDocument xdoc = new XmlDocument();
                    common.WriteNotificationFile(xmlSchemaWrite, common.myInt(Session["FacilityId"]), fileLoc);
                }
            }
        }
        else
        {
            hshtablein.Add("@HospitalLocationId", Session["HospitalLocationID"]);
            hshtablein.Add("@RegistrationId", Session["RegistrationId"]);
            //hshtablein.Add("@FromBedCategoryId", txtbedcategoryid.Text);
            //hshtablein.Add("@FromBillCategoryId", txtbedcategoryid.Text);

            hshtablein.Add("@FromBedCategoryId", txtCurrentBedCategoryid.Text);
            hshtablein.Add("@FromBillCategoryId", txtCurrentBillCategoryid.Text);


            hshtablein.Add("@FromBedId", txtbedid.Text);
            hshtablein.Add("@FromWardId ", txtwardid.Text);
            hshtablein.Add("@ToBedCategoryId", txttobedcategory.Text.Trim());
            hshtablein.Add("@ToBillCategoryId", ddltobillingcategoryname.SelectedValue);
            hshtablein.Add("@ToBedId", txttobedno.Text.Trim());
            hshtablein.Add("@ToWardId", txttoward.Text.Trim());
            hshtablein.Add("@UserId", common.myBool(hdnIsPasswordRequired.Value) ? common.myInt(PasswordRequiredHelper.GetTransactionUserId()) : common.myInt(Session["UserId"])); // To swapping between current userid & transaction userid);
            hshtablein.Add("@TransferDate", dtpTransferDate.SelectedDate.Value);
            hshtableout.Add("@StrStatus", SqlDbType.VarChar);
            hshtablein.Add("@intfacilityId", common.myStr(Session["FacilityId"]));

            if (common.myStr(Request.QueryString["ER"]) == "ER")
            {
                hshtablein.Add("@encounterid", common.myStr(Request.QueryString["encid"]));
                hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveERBedTransfer", hshtablein, hshtableout);
            }
            else
            {
                hshtablein.Add("@bIsBedRetail", (chkIsBedRetail.Checked == true) ? 1 : 0);
                hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveBedTransfer", hshtablein, hshtableout);
            }
            lblmsg.ForeColor = System.Drawing.Color.Green;
            lblmsg.Text = common.myStr(hshtableout["@StrStatus"]);

            Alert.ShowAjaxMsg(common.myStr(hshtableout["@StrStatus"]), Page);
        }


        txtwardid.Text = "";
        txtward.Text = "";
        txtbedcategoryid.Text = "";
        txtbedcategory.Text = "";
        txtbedid.Text = "";
        txtbedno.Text = "";
        txtCurrentBedCategoryid.Text = "";
        txtCurrentBillCategoryid.Text = "";

        txttobedno.Text = "";
        tdtransfer.Visible = false;
        dvConfirm.Visible = false;
        if (!(lblmsg.Text).Replace(" ", "").ToLower().Contains("alreadyexist"))
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "window.close();", true);
            return;
        }
    }
    protected void btnshowtransfer_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtregno.Text.Trim() != "" && txtipno.Text.Trim() != "")
            {
                RadWindowForNew.NavigateUrl = "/ATD/ShowbedTransfer.aspx?Regno=" + txtregno.Text.Trim() + "&encno=" + txtipno.Text.Trim() + "";
                RadWindowForNew.Height = 500;
                RadWindowForNew.Width = 1000;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                lblmsg.Text = "";
            }
            else
            {
                Alert.ShowAjaxMsg("Please Select Patient !", Page);
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnew_Click(object sender, EventArgs e)
    {
        try
        {
            txtipno.Text = "";
            txtregno.Text = "";
            txtipno.Enabled = true;
            txtward.Text = "";
            txtbedcategory.Text = "";
            txtbedno.Text = "";
            Session["RegistrationID"] = "";
            Session["EncounterId"] = "";

            if (Convert.ToString(txtregno.Text.Trim()) != "")
            {
                //   pnlxmladmissiondetails.Visible = true;
                BindPatientHiddenDetails(common.myStr(txtregno.Text)); ;
            }
            else
            {

            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnfilter_Click(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable hshin = new Hashtable();

            hshin.Add("@intEncounterNo", txtipno.Text.Trim());
            string str = "select RegistrationId,RegistrationNo,EncounterId,EncounterNo from Admission where EncounterNo=@intEncounterNo and PatadType!='C'";
            ds = dl.FillDataSet(CommandType.Text, str, hshin);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtipno.Enabled = false;
                hdnregno.Value = ds.Tables[0].Rows[0]["RegistrationId"].ToString().Trim();
                Session["RegistrationId"] = hdnregno.Value;
                ViewState["IpNo"] = ds.Tables[0].Rows[0]["EncounterNo"].ToString().Trim();
                txtipno.Text = ds.Tables[0].Rows[0]["EncounterNo"].ToString().Trim();
                txtregno.Text = ds.Tables[0].Rows[0]["RegistrationNo"].ToString().Trim();
                Session["encounterid"] = ds.Tables[0].Rows[0]["EncounterId"].ToString().Trim();
            }
            else
            {
                Alert.ShowAjaxMsg("Patient Can Not Find ", Page);
                //   lblmsg.Text = "Patient Can Not Find.";
                hdnregno.Value = "";
            }
            if (txtregno.Text.Trim() != "")
            {
                string sqlstr = "select DISTINCT wm.WardId,wm.wardName,bcm.BedcategoryId,ios.ServiceName as BedCategoryName,bm.Id,bm.BedNo, adm.CurrentBedCategory,adm.CurrentBillCategory  " +
                       "from Admission adm with(nolock) INNER JOIN BedMaster bm with(nolock) ON adm.CurrentBedId=bm.Id   " +
                       "INNER JOIN WardMaster wm with(nolock) ON wm.WardId=bm.WardId " +
                       "INNER JOIN BedCategoryMaster bcm with(nolock) ON bm.BedCategoryId =bcm.BedcategoryId " +
                       "INNER JOIN ItemOfService ios with(nolock) ON ios.ServiceId = bcm.BedCategoryId  " +
                       "WHERE adm.EncounterNo='" + txtipno.Text.Trim() + "' and adm.PatadType !='C' and adm.Active=1";
                ds = dl.FillDataSet(CommandType.Text, sqlstr);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtipno.Enabled = false;
                    txtwardid.Text = ds.Tables[0].Rows[0]["wardId"].ToString().Trim();
                    txtward.Text = ds.Tables[0].Rows[0]["wardName"].ToString().Trim();
                    txtbedcategoryid.Text = ds.Tables[0].Rows[0]["BedcategoryId"].ToString().Trim();
                    txtbedcategory.Text = ds.Tables[0].Rows[0]["BedCategoryName"].ToString().Trim();
                    txtbedid.Text = ds.Tables[0].Rows[0]["Id"].ToString().Trim();
                    txtbedno.Text = ds.Tables[0].Rows[0]["BedNo"].ToString().Trim();
                    txtCurrentBedCategoryid.Text = ds.Tables[0].Rows[0]["CurrentBedCategory"].ToString().Trim();
                    txtCurrentBillCategoryid.Text = ds.Tables[0].Rows[0]["CurrentBillCategory"].ToString().Trim();

                }
            }

            BindPatientHiddenDetails(common.myStr(txtregno.Text)); ;
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void BindBillingCategory()
    {
        BaseC.ATD objBed = new BaseC.ATD(sConString);

        DataSet ds = new DataSet();
        ddltobillingcategoryname.Items.Clear();

        ds = objBed.GetBedCategory("N", common.myInt(ddlFacilityName.SelectedValue));
        DataTable dtcat = new DataTable();
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataView dv = ds.Tables[0].DefaultView;
            //dv.RowFilter = "(BedCategoryType<>'E' And BedCategoryType<>'O') And ISNULL(IsICU,0)=0 AND IsBillingCategory=1";
            //dv.RowFilter = "(BedCategoryType<>'E' And BedCategoryType<>'O')  AND IsBillingCategory=1";
            dtcat = dv.ToTable();

            foreach (DataRowView dr in dtcat.DefaultView)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)dr["BedCatgName"];
                item.Value = dr["BedCategoryId"].ToString();
                item.Attributes.Add("AllowedAge", common.myInt(dr["AllowedAge"]).ToString());
                ddltobillingcategoryname.Items.Add(item);
                item.DataBind();
            }
        }
        ddltobillingcategoryname.Items.Insert(0, new RadComboBoxItem("", "0"));
        ddltobillingcategoryname.SelectedIndex = 0;
    }

    protected void GetFacility()
    {
        try
        {
            DAL.DAL objdl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            ds = objdl.FillDataSet(CommandType.Text, "SELECT FacilityID, facilitycode As Name  FROM FacilityMaster WHERE HospitalLocationID=" + common.myInt(Session["HospitalLocationId"]) + " AND Active =1 ");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlFacilityName.DataSource = ds;
                ddlFacilityName.DataTextField = "Name";
                ddlFacilityName.DataValueField = "FacilityId";
                ddlFacilityName.DataBind();
            }
            ddlFacilityName.Items.Insert(0, "ALL");
            if (common.myStr(ViewState["isRequiredAllFacilityBed"]) == "Y")
            {
                ddlFacilityName.Items[0].Value = "0";
            }
            else
            {
                ddlFacilityName.SelectedIndex = ddlFacilityName.Items.IndexOf(ddlFacilityName.Items.FindByValue(common.myStr(Session["FacilityId"])));
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    #region Transaction password validation
    private void IsValidPassword()
    {
        
        hdnIsValidPassword.Value = "0";
        
        RadWindow1.NavigateUrl = "/Pharmacy/Components/PasswordCheckerAllUser.aspx?UseFor=OPIP";
        RadWindow1.Height = 120;
        RadWindow1.Width = 340;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "OnClientIsValidPasswordClose";
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
    }

    protected void btnIsValidPasswordClose_OnClick(object Sender, EventArgs e)
    {
        try
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            if (common.myInt(hdnIsValidPassword.Value).Equals(0))
            {
                lblmsg.Text = "Invalid Username/Password!";
                return;
            }

            saveRecord();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = Ex.Message;
            objException.HandleException(Ex);
        }
    }
    #endregion


    protected void ddlFacilityMater_SelectedIndexChanged(object sender, EventArgs e)
    {
        Bindward();
        fillbedcategory();
        fillData(common.myInt(ddlFacilityName.SelectedValue));
        
    }


}
