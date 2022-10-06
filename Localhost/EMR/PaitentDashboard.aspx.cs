using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class EMR_PaitentDashboard : System.Web.UI.Page
{
    // new files
    DataSet ds;

    BaseC.ICM ObjIcm;

    int it = 0;
    string sMutipleRecordsDate = string.Empty;
    StringBuilder sb = new StringBuilder();
    DL_Funs ff = new DL_Funs();
    private int iPrevId = 0;
    StringBuilder sbSign = new StringBuilder();
    bool bTRecords = false;
    bool bTMRecords = false;

    string FlagIsRequiredDischargeSummaryDate = string.Empty;
    bool flagOTHeadingName = true;
    // new files ending here

    BaseC.Patient objbc1;
    DAL.DAL dl;
    private static string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    string path = string.Empty;
    public string RTF1Content = string.Empty;
    string FromDate = string.Empty;
    string ToDate = string.Empty;
    string sFontSize = string.Empty;
    bool flag = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            hdnRegistrationID.Value = common.myStr(Session["RegistrationID"]);
            hdnRegistrationNo.Value = common.myStr(Session["RegistrationNo"]);

            //////   SetSectionVisibility();
            BindGrid();
            BindPatientData();
            LabVisitHistory();
            HideFields();
            //  PrescribedMedicineHistory();
            bindGraph();
            bindGraphView();
        }
    }
    private void HideFields()
    {

        dvVitals.Columns[0].Visible = false;
        dvVitals.Columns[1].Visible = false;
        dvVitals.Columns[2].Visible = false;
        dvVitals.Columns[4].Visible = false;

        rpfCategory.Columns[6].Visible = true;
        rpfCategory.Columns[7].Visible = true;
        rpfCategory.Columns[8].Visible = true;

        //rpfCategory.Columns[9].Visible = false;
        //rpfCategory.Columns[10].Visible = false;
        //rpfCategory.Columns[11].Visible = false;
        //rpfCategory.Columns[12].Visible = false;
        //rpfCategory.Columns[13].Visible = false;
        //rpfCategory.Columns[14].Visible = false;
    }



    void SetSectionVisibility()
    {
        DataSet ds = new DataSet();
        DataView DV = new DataView();
        objbc1 = new BaseC.Patient(sConString);

        if (common.myInt(Session["FacilityId"]) > 0)
        {
            ds = objbc1.GetPatientDashBordSection(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DV = ds.Tables[0].DefaultView;



                    DV.RowFilter = "SectionCode='DLV'";
                    if (DV.ToTable().Rows.Count > 0)
                    {
                        divLastPatientVisit.Visible = true;
                    }
                    DV.RowFilter = string.Empty;


                    DV.RowFilter = "SectionCode='VTL'";
                    if (DV.ToTable().Rows.Count > 0)
                    {
                        divViatalsGraph.Visible = true;
                    }
                    DV.RowFilter = string.Empty;

                    DV.RowFilter = "SectionCode='DNS'";
                    if (DV.ToTable().Rows.Count > 0)
                    {
                        //divDoctorsNotification.Visible = true;
                    }
                    DV.RowFilter = string.Empty;

                    DV.RowFilter = "SectionCode='COM'";
                    if (DV.ToTable().Rows.Count > 0)
                    {
                        //divProblemList.Visible = true;
                    }
                    DV.RowFilter = string.Empty;

                    DV.RowFilter = "SectionCode='PRS'";
                    if (DV.ToTable().Rows.Count > 0)
                    {
                        divMedicinesGiven.Visible = true;
                    }
                    DV.RowFilter = string.Empty;
                    DV.RowFilter = "SectionCode='LBT'";
                    if (DV.ToTable().Rows.Count > 0)
                    {
                        divLabTest.Visible = true;
                    }
                    DV.RowFilter = "SectionCode='BSG'";
                    if (DV.ToTable().Rows.Count > 0)
                    {
                        //divSugarGraph.Visible = true;
                    }
                    DV.RowFilter = string.Empty;

                    DV.RowFilter = "SectionCode='RDT'";
                    if (DV.ToTable().Rows.Count > 0)
                    {
                        divRadiologyTest.Visible = true;
                    }
                    DV.RowFilter = string.Empty;

                }
                else
                {
                    divLabTest.Visible = true;
                    divLastPatientVisit.Visible = true;
                    //divSugarGraph.Visible = true;
                    divViatalsGraph.Visible = true;
                    divRadiologyTest.Visible = true;
                    divMedicinesGiven.Visible = true;
                    //divProblemList.Visible = true;
                    //divDoctorsNotification.Visible = true;



                }
            }
            else
            {
                divLabTest.Visible = true;
                divLastPatientVisit.Visible = true;
                //divSugarGraph.Visible = true;
                divViatalsGraph.Visible = true;
                divRadiologyTest.Visible = true;
                divMedicinesGiven.Visible = true;
                //divProblemList.Visible = true;
                //divDoctorsNotification.Visible = true;



            }

        }
    }


    protected void rptPaging_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
    {
        PageNumber = Convert.ToInt32(e.CommandArgument) - 1;
        BindPatientData();
    }

    protected void BindPatientData()
    {
        objbc1 = new BaseC.Patient(sConString);
        DataSet dsOtherPatientDetail = new DataSet();

        try
        {

            // dsOtherPatientDetail = objbc1.GetPrescribedMedicineHistory(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), Session["RegistrationNo"].ToString());
            dsOtherPatientDetail = (DataSet)ViewState["OtherPatientDetail"];
            if (dsOtherPatientDetail != null)
            {
                if (dsOtherPatientDetail.Tables.Count > 0)
                {
                    #region Chief Complaints 
                    if (dsOtherPatientDetail.Tables[1].Rows.Count > 0)
                    {

                        //rptProblemList.DataSource = dsOtherPatientDetail.Tables[1];
                        //rptProblemList.DataBind();

                        PagedDataSource pgitems = new PagedDataSource();
                        DataView dv = new DataView(dsOtherPatientDetail.Tables[1]);
                        pgitems.DataSource = dv;
                        pgitems.AllowPaging = true;
                        pgitems.PageSize = 2;
                        pgitems.CurrentPageIndex = LabPageNumber;
                        if (pgitems.PageCount > 1)
                        {
                            //rptPaging.Visible = true;
                            //ArrayList pages = new ArrayList();
                            //for (int i = 0; i < pgitems.PageCount; i++)
                            //    pages.Add((i + 1).ToString());
                            //rptPaging.DataSource = pages;
                            //rptPaging.DataBind();
                        }
                        else
                        {
                            //rptPaging.Visible = false;

                            //rptPaging.DataSource = pgitems;// dsOtherPatientDetail.Tables[1];
                            //rptPaging.DataBind();

                        }


                    }
                    #endregion

                    #region Allergy
                    if (dsOtherPatientDetail.Tables[4].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsOtherPatientDetail.Tables[4].Rows.Count; i++)
                        {
                            //if (i <= (common.myInt(dsOtherPatientDetail.Tables[4].Rows.Count) - 2))
                            //{
                            //    lblAllergy.Text = common.myStr(lblAllergy.Text) + common.myStr(dsOtherPatientDetail.Tables[4].Rows[i]["AllergyName"]) + " , ";
                            //}
                            //else if (i.Equals((common.myInt(dsOtherPatientDetail.Tables[4].Rows.Count) - 1)))
                            //{
                            //    lblAllergy.Text = common.myStr(lblAllergy.Text) + common.myStr(dsOtherPatientDetail.Tables[4].Rows[i]["AllergyName"]);
                            //}

                            gvAllergy.DataSource = dsOtherPatientDetail.Tables[4];
                            gvAllergy.DataBind();
                            PagedDataSource pgitems = new PagedDataSource();
                            DataView dv = new DataView(dsOtherPatientDetail.Tables[4]);
                            pgitems.DataSource = dv;
                            pgitems.AllowPaging = true;
                            pgitems.PageSize = 2;
                            pgitems.CurrentPageIndex = LabPageNumber;
                            if (pgitems.PageCount > 1)
                            {
                                //////rptLabvisitpaging.Visible = true;
                                //////ArrayList pages = new ArrayList();
                                //////for (int i = 0; i < pgitems.PageCount; i++)
                                //////    pages.Add((i + 1).ToString());
                                //////rptLabvisitpaging.DataSource = pages;
                                //////rptLabvisitpaging.DataBind();
                            }
                            else
                            {
                                //////    rptLabvisitpaging.Visible = false;

                                //////    rptLabVisitHistory.DataSource = pgitems;// dsOtherPatientDetail.Tables[1];
                                //////    rptLabVisitHistory.DataBind();
                                //////}
                            }

                        }

                    }
                    else if (dsOtherPatientDetail.Tables[5].Rows.Count > 0)
                    {
                        if (!common.myStr(dsOtherPatientDetail.Tables[5].Rows[0]["AllergyName"]).Equals(string.Empty))
                        {
                            //lblAllergy.Text = "No Allergy";
                            gvAllergy.DataSource = dsOtherPatientDetail.Tables[5];
                            gvAllergy.DataBind();
                        }
                    }
                    #endregion

                    #region PrescribedMedicine
                    if (dsOtherPatientDetail.Tables[2].Rows.Count > 0)
                    {
                        gvPrescribedMedicine.DataSource = dsOtherPatientDetail.Tables[2];
                        gvPrescribedMedicine.DataBind();

                        PagedDataSource pgitems = new PagedDataSource();
                        DataView dv = new DataView(dsOtherPatientDetail.Tables[2]);
                        pgitems.DataSource = dv;
                        pgitems.AllowPaging = true;
                        pgitems.PageSize = 2;
                        pgitems.CurrentPageIndex = LabPageNumber;
                        if (pgitems.PageCount > 1)
                        {
                            //////rptLabvisitpaging.Visible = true;
                            //////ArrayList pages = new ArrayList();
                            //////for (int i = 0; i < pgitems.PageCount; i++)
                            //////    pages.Add((i + 1).ToString());
                            //////rptLabvisitpaging.DataSource = pages;
                            //////rptLabvisitpaging.DataBind();
                        }
                        else
                        {
                            //////    rptLabvisitpaging.Visible = false;

                            //////    rptLabVisitHistory.DataSource = pgitems;// dsOtherPatientDetail.Tables[1];
                            //////    rptLabVisitHistory.DataBind();
                            //////}
                        }


                    }
                    #endregion

                    #region vitals
                    if (dsOtherPatientDetail.Tables[3].Rows.Count > 0)
                    {
                        dvVitals.DataSource = dsOtherPatientDetail.Tables[3];
                        dvVitals.DataBind();

                        ViewState["Vitals"] = dsOtherPatientDetail.Tables[3];

                        PagedDataSource pgitems = new PagedDataSource();
                        DataView dv = new DataView(dsOtherPatientDetail.Tables[2]);
                        pgitems.DataSource = dv;
                        pgitems.AllowPaging = true;
                        pgitems.PageSize = 2;
                        pgitems.CurrentPageIndex = LabPageNumber;
                        if (pgitems.PageCount > 1)
                        {
                            //////rptLabvisitpaging.Visible = true;
                            //////ArrayList pages = new ArrayList();
                            //////for (int i = 0; i < pgitems.PageCount; i++)
                            //////    pages.Add((i + 1).ToString());
                            //////rptLabvisitpaging.DataSource = pages;
                            //////rptLabvisitpaging.DataBind();
                        }
                        else
                        {
                            //////    rptLabvisitpaging.Visible = false;

                            //////    rptLabVisitHistory.DataSource = pgitems;// dsOtherPatientDetail.Tables[1];
                            //////    rptLabVisitHistory.DataBind();
                            //////}
                        }


                    }
                    #endregion
                    #region RadiologyLabTest
                    if (dsOtherPatientDetail.Tables[6].Rows.Count > 0)
                    {
                        gvLabTest.DataSource = dsOtherPatientDetail.Tables[6];
                        gvLabTest.DataBind();
                    }
                    if (dsOtherPatientDetail.Tables[7].Rows.Count > 0)
                    {
                        gvRadiologyTest.DataSource = dsOtherPatientDetail.Tables[7];
                        gvRadiologyTest.DataBind();
                    }


                    #endregion


                }
            }
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            if (dsOtherPatientDetail != null)
            {
                dsOtherPatientDetail.Dispose();
            }

        }
    }
    protected void PrescribedMedicineHistory()
    {
        objbc1 = new BaseC.Patient(sConString);
        DataSet dsPrescribedMedicine = new DataSet();

        try
        {

            // dsPrescribedMedicine = objbc1.GetPrescribedMedicineHistory(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), Session["RegistrationNo"].ToString());
            dsPrescribedMedicine = (DataSet)ViewState["OtherPatientDetail"];
            if (dsPrescribedMedicine != null)
            {
                if (dsPrescribedMedicine.Tables.Count > 0)
                {
                    if (dsPrescribedMedicine.Tables[2].Rows.Count > 0)
                    {
                        gvPrescribedMedicine.DataSource = dsPrescribedMedicine;
                        gvPrescribedMedicine.DataBind();

                        PagedDataSource pgitems = new PagedDataSource();
                        DataView dv = new DataView(dsPrescribedMedicine.Tables[2]);
                        pgitems.DataSource = dv;
                        pgitems.AllowPaging = true;
                        pgitems.PageSize = 2;
                        pgitems.CurrentPageIndex = LabPageNumber;
                        if (pgitems.PageCount > 1)
                        {
                            //////rptLabvisitpaging.Visible = true;
                            //////ArrayList pages = new ArrayList();
                            //////for (int i = 0; i < pgitems.PageCount; i++)
                            //////    pages.Add((i + 1).ToString());
                            //////rptLabvisitpaging.DataSource = pages;
                            //////rptLabvisitpaging.DataBind();
                        }
                        else
                        {
                            //////    rptLabvisitpaging.Visible = false;

                            //////    rptLabVisitHistory.DataSource = pgitems;// dsPrescribedMedicine.Tables[2];
                            //////    rptLabVisitHistory.DataBind();
                            //////}
                        }


                    }
                }
            }
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            if (dsPrescribedMedicine != null)
            {
                dsPrescribedMedicine.Dispose();
            }
        }
    }

    void LabVisitHistory()
    {
        objbc1 = new BaseC.Patient(sConString);
        DataSet dsTemp = new DataSet();

        try
        {

            dsTemp = objbc1.GetLabVisitHistory(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                    Session["RegistrationNo"].ToString());

            if (dsTemp.Tables[0].Rows.Count > 0)
            {

                PagedDataSource pgitems = new PagedDataSource();
                DataView dv = new DataView(dsTemp.Tables[0]);
                pgitems.DataSource = dv;
                pgitems.AllowPaging = true;
                pgitems.PageSize = 2;
                pgitems.CurrentPageIndex = LabPageNumber;
                if (pgitems.PageCount > 1)
                {
                    //////rptLabvisitpaging.Visible = true;
                    //////ArrayList pages = new ArrayList();
                    //////for (int i = 0; i < pgitems.PageCount; i++)
                    //////    pages.Add((i + 1).ToString());
                    //////rptLabvisitpaging.DataSource = pages;
                    //////rptLabvisitpaging.DataBind();
                }
                else
                {
                    //////    rptLabvisitpaging.Visible = false;

                    //////    rptLabVisitHistory.DataSource = pgitems;// dsTemp.Tables[0];
                    //////    rptLabVisitHistory.DataBind();
                    //////}
                }


            }
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            dsTemp.Dispose();
        }
    }
    public int LabPageNumber
    {
        get
        {
            if (ViewState["LabPageNumber"] != null)
                return Convert.ToInt32(ViewState["LabPageNumber"]);
            else
                return 0;
        }
        set
        {
            ViewState["LabPageNumber"] = value;
        }
    }

    protected void rptLabvisitpaging_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        //RepeaterItem ri = e.Item;
        //LinkButton lbtn = (LinkButton)ri.FindControl("btnPage");
        //lbtn.BackColor = System.Drawing.Color.SkyBlue;

        LabPageNumber = Convert.ToInt32(e.CommandArgument) - 1;
        LabVisitHistory();
    }

    protected void rptLabvisitpaging_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.Item)
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            LinkButton lnk = (LinkButton)e.Item.FindControl("lbtnPage");

            if (lnk.CommandArgument.ToString() == (LabPageNumber + 1).ToString())
            {

                lnk.CssClass = "page_disabled";
                //lnk.ForeColor = System.Drawing.Color.Black;

            }

            else
            {
                if (lnk.CommandArgument.ToString() == "View all")
                {
                    lnk.CssClass = "page_enabled";
                    lnk.ForeColor = System.Drawing.Color.Black;
                }
                else
                {
                    lnk.CssClass = "page_enabled";
                    lnk.ForeColor = System.Drawing.Color.Black;
                }
            }
        }
    }
    public int PageNumber
    {
        get
        {
            if (ViewState["PageNumber"] != null)
                return Convert.ToInt32(ViewState["PageNumber"]);
            else
                return 0;
        }
        set
        {
            ViewState["PageNumber"] = value;
        }
    }

    protected void rptPages_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        //RepeaterItem item = (RepeaterItem)source;
        //LinkButton Link1 = (LinkButton)e.Item.FindControl("lbtnPage");


        LinkButton lbtn = e.Item.FindControl("lbtnPage") as LinkButton;//(LinkButton)e.Item.FindControl("lbtnPage");
        lbtn.CssClass = "page_enabled";

        //lbtn.BackColor = System.Drawing.Color.SkyBlue;
        PageNumber = Convert.ToInt32(e.CommandArgument) - 1;

        BindGrid();
    }

    protected void rptPages_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.Item)
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            LinkButton lnk = (LinkButton)e.Item.FindControl("lbtnPage");

            if (lnk.CommandArgument.ToString() == (PageNumber + 1).ToString())
            {

                lnk.CssClass = "page_disabled";
            }

            else
            {
                if (lnk.CommandArgument.ToString() == "View all")
                {
                    lnk.CssClass = "page_enabled";
                    lnk.ForeColor = System.Drawing.Color.Black;
                }
                else
                {
                    lnk.CssClass = "page_enabled";
                    lnk.ForeColor = System.Drawing.Color.Black;
                }
            }
        }
    }


    protected void BindGrid()
    {

        objbc1 = new BaseC.Patient(sConString);
        DataSet dsPatientDetail = new DataSet();
        DataSet dsOtherPatientDetail = new DataSet();
        string strDateRange = string.Empty;
        try
        {
            dsPatientDetail = (DataSet)Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])];
            dsOtherPatientDetail = objbc1.GetPatientDashboard(common.myInt(Session["RegistrationId"]));
            ViewState["OtherPatientDetail"] = dsOtherPatientDetail;
            if (dsPatientDetail != null)
            {
                if (dsPatientDetail.Tables.Count > 0)
                {
                    if (dsPatientDetail.Tables[0].Rows.Count > 0)
                    {


                        DataRow dr1 = dsPatientDetail.Tables[0].Rows[0];


                        ////Label lblAge = (Label)Master.FindControl("lblAge");
                        ////Label lblName = (Label)Master.FindControl("lblName");
                        ////Label lblUHID = (Label)Master.FindControl("lblUHID");
                        ////Label lblAddress = (Label)Master.FindControl("lblAddress");
                        ////Label lblFacility = (Label)Master.FindControl("lblFacility");
                        ////Label lblHeader = (Label)Master.FindControl("lblHeader");

                        ////Session["FacilityName"] = lblFacility.Text = dr1["FacilityName"].ToString();
                        ////Session["Name"] = lblName.Text = dr1["Name"].ToString() + ", ";
                        ////lblHeader.Text = dr1["Name"].ToString() + "  My Medical records";
                        ////Session["GenderAge"] = lblAge.Text = dr1["GenderAge"].ToString() + ", ";
                        ////Image PatientImageobj = (Image)Master.FindControl("PatientImage");
                        ////PatientImageobj.ImageUrl = "Images/Raju/photo.jpg";

                        //Session[""] = lblAddress.Text = dr1["Address"].ToString() + ", ";

                        //lblUHID.Text = dr1["RegistrationNo"].ToString();
                        //lblName.Text = dr1["PatientName"].ToString();
                        //lblAge.Text = dr1["AgeGender"].ToString();
                        //lblContact.Text = dr1["MobileNo"].ToString();



                        if (dr1["ImageType"].ToString() != "")
                        {
                            try
                            {
                                Stream strm;
                                Object img = dr1["PatientImage"];
                                String FileName = dr1["ImageType"].ToString();
                                strm = new MemoryStream((byte[])img);
                                byte[] buffer = new byte[strm.Length];
                                string targetFile = Server.MapPath("~/PatientDocuments/PatientImages/" + FileName);
                                int byteSeq = strm.Read(buffer, 0, System.Convert.ToInt32(strm.Length));
                                FileStream fs = new FileStream(Server.MapPath("~/PatientDocuments/PatientImages/" + FileName), FileMode.Create, FileAccess.Write);
                                fs.Write(buffer, 0, byteSeq);
                                fs.Dispose();
                                GenerateThumbnails(0.0375, strm, targetFile);
                                //PatientImage.ImageUrl = "~/PatientDocuments/PatientImages/" + FileName;

                                ViewState["PatientImages"] = "/PatientDocuments/PatientImages/" + FileName;
                                ViewState["strm"] = strm;
                                ViewState["targetFile"] = targetFile;

                                // imgPatient.ImageUrl = "/PatientDocuments/PatientImages/" + dr1["ImageType"].ToString() + "";
                                //imgPatient.ImageUrl = "/PatientDocuments/PatientImages/" + UnquieID + ".jpg"";
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            //   imgPatient.ImageUrl = "~/imagesHTML/blank1.png";
                            //////PatientImage.ImageUrl = "~/Images/no_photo.jpg";
                        }

                        if (dsOtherPatientDetail != null)
                        {
                            if (dsOtherPatientDetail.Tables.Count > 0)
                            {
                                PagedDataSource pgitems = new PagedDataSource();
                                DataView dv = new DataView(dsOtherPatientDetail.Tables[0]);
                                pgitems.DataSource = dv;
                                pgitems.AllowPaging = true;
                                pgitems.PageSize = 10;
                                pgitems.CurrentPageIndex = PageNumber;
                                if (pgitems.PageCount > 1)
                                {
                                    rptPages.Visible = true;
                                    ArrayList pages = new ArrayList();
                                    for (int i = 0; i < pgitems.PageCount; i++)
                                        pages.Add((i + 1).ToString());
                                    rptPages.DataSource = pages;
                                    rptPages.DataBind();
                                }
                                else
                                    rptPages.Visible = false;

                                rpfCategory.DataSource = pgitems;// dsTemp.Tables[0];
                                rpfCategory.DataBind();
                            }
                        }
                    }
                }
            }
            else
            {
                rpfCategory.DataSource = null;
                rpfCategory.DataBind();
                rptPages.Visible = false;
            }
        }
        catch (Exception Ex)
        {

            objException.HandleException(Ex);
        }
        finally
        {
            if (dsPatientDetail != null)
            {
                dsPatientDetail.Dispose();
            }
        }
    }


    private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
    {
        using (var image = System.Drawing.Image.FromStream(sourcePath))
        {
            var newWidth = (int)(image.Width * scaleFactor);
            var newHeight = (int)(image.Height * scaleFactor);
            var thumbnailImg = new Bitmap(newWidth, newHeight);
            var thumbGraph = Graphics.FromImage(thumbnailImg);
            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbGraph.DrawImage(image, imageRectangle);
            thumbnailImg.Save(targetPath, image.RawFormat);
        }
    }

    //-----------
    //public DataSet GetPatientHistoryDetails(int iHospitalLocationId, int IFacilityId, int iRegId, string Daterange, string iFromdate, string iTodate, string Source)
    //{
    //    DataSet ds = new DataSet();
    //    //objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    Hashtable hshInput = new Hashtable();
    //    hshInput.Add("@inyHospitalLocationId", iHospitalLocationId);
    //    hshInput.Add("@intFacilityId", IFacilityId);
    //    //hshInput.Add("@intProviderId", iDoctorId);
    //    hshInput.Add("@intRegistrationId", iRegId);
    //    hshInput.Add("@chvDateRange", Daterange);
    //    hshInput.Add("@chrFromDate", iFromdate);
    //    hshInput.Add("@chrToDate", iTodate);
    //    hshInput.Add("@Source", Source);
    //    //ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspSearchPatientHistry", hshInput);  PPSpVisitHistory

    //    ds = objDl.FillDataSet(CommandType.StoredProcedure, "PPSpVisitHistory", hshInput);
    //    return ds;
    //}

    //-----------
    void GetDocumentList()
    {
        objbc1 = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objbc1.GetDocumentList(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                    common.myLong(Session["RegistrationNo"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                //rptDocumentList.DataSource = ds.Tables[0];
                //rptDocumentList.DataBind();
            }
            else
            {
                //rptDocumentList.DataSource = null;
                //rptDocumentList.DataBind();
            }


        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }

    protected void lbtn6month_Click(object sender, EventArgs e)
    {
        //////    //SetColor();
        //////    dtpfrmdate.Visible = false;
        //////    dtpTodate.Visible = false;
        //////    btnFilter.Visible = false;
        //////    LinkButton lbtn = (LinkButton)sender;
        //////    strDateRange = lbtn.CommandArgument;
        //////    lbtn.BackColor = System.Drawing.Color.SkyBlue;
        //////    BindGrid();

    }

    //void SetColor()
    //{
    //    lbtn6month.BackColor = System.Drawing.Color.White;
    //    lbtn2year.BackColor = System.Drawing.Color.White;
    //    lbtn1year.BackColor = System.Drawing.Color.White;
    //    lbtnDateRange.BackColor = System.Drawing.Color.White;

    //}




    protected void PatientType_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            var ibtn = (ImageButton)sender;
            RepeaterItem item = (RepeaterItem)ibtn.NamingContainer;
            Label lblType = (Label)item.FindControl("lblType");

            if ((ibtn.ToolTip == "Case Sheet") || (lblType.Text == "OP"))
            {
                Label lblEncounter = (Label)item.FindControl("lblEncounterid");
                LinkButton lbtn = (LinkButton)item.FindControl("lbtnDoctorName");
                Label lblDate = (Label)item.FindControl("lblDate");


                Session["EncounterId"] = lblEncounter.Text;
                Session["EncounterDate"] = lblDate.Text;
                Session["UserID"] = lbtn.CommandArgument;
                Response.Redirect("Casesheet.aspx", false);

                // Response.Redirect("PatientDischarge.aspx", false);
            }
            else if (ibtn.ToolTip == "Patient is Admitted.")
            {
                Alert.ShowAjaxMsg("Patient is Admitted. Access records from hospital", this);

            }
            else
            {
                Label lblEncounter = (Label)item.FindControl("lblEncounterNo");
                Session["EncounterNo"] = lblEncounter.Text;



                Response.Redirect("PatientDischarge.aspx", false);
            }
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }

    }

    protected void lblStatus_Click(object sender, EventArgs e)
    {



        var ibtn = (ImageButton)sender;
        RepeaterItem item = (RepeaterItem)ibtn.NamingContainer;
        Label lblSampleId = (Label)item.FindControl("lblSampleId");
        LinkButton lbtn = (LinkButton)item.FindControl("lblStatus");
        Label lblServiceId = (Label)item.FindControl("lblServiceId");
        Label hdnSourcetype = (Label)item.FindControl("hdnSourcetype");





        //  HiddenField hdnSourcetype = (HiddenField)rptLabVisitHistory.Items[9].FindControl("hdnSourcetype");

        //CheckBox chk = (CheckBox)Repeater1.Items[count].FindControl("CheckBox1");



        //Label lblStationId = (Label)item.FindControl("lblStationId");
        string lblStationId = "3";
        string LabNo = lbtn.CommandArgument.ToString();

        string Source = hdnSourcetype.Text.Trim();



        StringBuilder objXML = new StringBuilder();
        ArrayList coll = new ArrayList();
        //coll.Add(68904);//SampleID INT
        coll.Add(common.myInt(lblSampleId.Text));//SampleID INT
        coll.Add("OPD");//Source VARCHAR
        objXML.Append(common.setXmlTable(ref coll));


        //  ViewState["EncId"] = 105418;

        ViewState["EncId"] = "";


        ////string PrintPdfforHTML = "PrintPdfforHTML.aspx?SOURCE=" + Source + "&LABNO=" 
        ////    + lbtn.CommandArgument.ToString() + "&ServiceIds=" + lblServiceId.Text + "&DiagSampleId=" + objXML.ToString() +
        ////    "&StationId=" + lblStationId.Text + "&RegId=" + ViewState["RegistrationId"] + "&EncId=" + ViewState["EncId"];


        ////string queryString = "PrintPdfforHTML.aspx?SOURCE=" + Source + "&LABNO="
        ////+ LabNo + "&ServiceIds=" + lblServiceId.Text + "&DiagSampleId=" + lblSampleId.Text +
        ////"&StationId=" + lblStationId.ToString() + "&RegId=" + common.myInt(Session["RegistrationId"]) + "&EncId=" + common.myInt(ViewState["EncId"]);


        ////string s = "window.open('" + queryString + "', 'popup_window', 'width=300,height=100,left=100,top=100,resizable=yes');";
        ////ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);







        string PrintPdfforHTML = "PrintPdfforHTML.aspx?SOURCE=" + Source + "&LABNO="
      + LabNo + "&ServiceIds=" + lblServiceId.Text + "&DiagSampleId=" + lblSampleId.Text +
      "&StationId=" + lblStationId.ToString() + "&RegId=" + common.myInt(Session["RegistrationId"]) + "&EncId=" + common.myInt(ViewState["EncId"]);



        ScriptManager.RegisterStartupScript(Page, this.Page.GetType(), "OpenWindow", "window.open('" + PrintPdfforHTML + "');", true);


    }

    protected void ibtnImage_Click(object sender, ImageClickEventArgs e)
    {
        var ibtn = (ImageButton)sender;
        RepeaterItem item = (RepeaterItem)ibtn.NamingContainer;
        Label lbl = (Label)item.FindControl("lblImagePath");

        //ClientScript.RegisterStartupScript(this.Page.GetType(), "OpenWindow", "window.open('"+lbl.Text+"');", true);
        ClientScript.RegisterStartupScript(this.Page.GetType(), "OpenWindow", "window.open('Document/18-Disabledfamilycoping.pdf');", true);

        //Response.Redirect("Document/18-Disabledfamilycoping.pdf");  
    }

    protected void ibtnLabResult_Click(object sender, ImageClickEventArgs e)
    {
        // var ibtn = (ImageButton)sender;
        GridViewRow row = (GridViewRow)(((ImageButton)sender).NamingContainer);
        Label lblEncounterId = (Label)row.FindControl("lblEncounterid");

        Label lblDate = (Label)row.FindControl("lblDate");
        Label lblType = (Label)row.FindControl("lblType");
        Label lblDoctorId = (Label)row.FindControl("lblDoctorId");
        Label lblEncounterNo = (Label)row.FindControl("lblEncounterNo");
        HiddenField hdnAppointmentID = (HiddenField)row.FindControl("hdnAppointmentID");
        HiddenField hdnRegistrationId = (HiddenField)row.FindControl("hdnRegistrationId");

        HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");
        HiddenField hdnDoctorId = (HiddenField)row.FindControl("hdnDoctorId");
        HiddenField hdnEncounterNo = (HiddenField)row.FindControl("hdnEncounterNo");

        //Session["EncounterId"] = common.myStr(hdnEncounterId.Value);
        //Session["DoctorID"] = common.myStr(hdnDoctorId.Value);
        //Session["OPIP"] = common.myStr(lblType.Text);
        //Session["EncounterDate"] = lblDate.Text;
        //Session["EncounterNo"] = common.myStr(hdnEncounterNo.Value);

        //Session["FollowUpDoctorId"] = common.myStr(lblDoctorId.Text);
        //Session["RegistrationID"] = hdnRegistrationId.Value;
        //Session["AppointmentID"] = hdnAppointmentID.Value;

        //if (common.myStr(lblType.Text).Equals("OP"))
        //{

        // RadWindowForNew.NavigateUrl = "~/Editor/WordProcessor.aspx?From=POPUP&DoctorId=" + common.myStr(Session["DoctorID"]) + "&OPIP=" + common.myStr(Session["OPIP"]) + "&EncounterDate=" + common.myDate(Session["EncounterDate"]).ToString("yyyy/MM/dd") + "&CloseButtonShow=No";
        //  RadWindowForNew.NavigateUrl = "~/Editor/WordProcessor.aspx?From=POPUP&DoctorId=" + common.myStr(hdnDoctorId.Value) + "&OPIP=" + common.myStr(lblType.Text)+ "&EncId=" + common.myStr(hdnEncounterId.Value)+ "&EncounterNo=" + common.myStr(hdnEncounterNo.Value) + "&EncounterDate=" + common.myDate(lblDate.Text).ToString("yyyy/MM/dd") + "&CloseButtonShow=No";


        if (common.myStr(lblType.Text).Equals("OP") || common.myStr(lblType.Text).Equals("ER"))
        {
            RadWindowForNew.NavigateUrl = "~/Editor/WordProcessor.aspx?From=POPUP&DoctorId=" + common.myStr(hdnDoctorId.Value)
                + "&OPIP=" + common.myStr(lblType.Text)
                + "&EncId=" + common.myStr(hdnEncounterId.Value)
                + "&RegId=" + common.myStr(hdnRegistrationId.Value)
                + "&EncNo=" + common.myStr(lblEncounterNo.Text)
                + "&RegNo=" + common.myStr(hdnRegistrationNo.Value)
                + "&EncounterDate=" + common.myDate(lblDate.Text).ToString("yyyy/MM/dd") + "&CloseButtonShow=No";
        }
        else if (common.myStr(lblType.Text).Equals("IP"))
        {
            RadWindowForNew.NavigateUrl = "~/Editor/VisitHistory.aspx?Regid=" + common.myStr(hdnRegistrationId.Value) + "&RegNo=" + common.myStr(hdnRegistrationNo.Value)
                + "&EncId=" + common.myStr(lblEncounterId.Text) + "&EncNo=" + common.myStr(lblEncounterNo.Text)
                + "&EncounterDate=" + common.myDate(lblDate.Text).ToString("yyyy/MM/dd")
                + "&FromWard=Y&Category=PopUp";
        }

        RadWindowForNew.Width = 1200;
        RadWindowForNew.Height = 630;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        //   RadWindowForNew.OnClientClose = "addTemplatesOnClientClose_All";
        RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
        RadWindowForNew.InitialBehavior = Telerik.Web.UI.WindowBehaviors.Maximize;
        //}
        //////Response.Redirect("VisitLabResultDetails.aspx");

    }


    protected void ibtnPrescribedMedicine_Click(object sender, ImageClickEventArgs e)
    {
        var ibtn = (ImageButton)sender;
        RepeaterItem item = (RepeaterItem)ibtn.NamingContainer;
        HiddenField hdnDetailsId = (HiddenField)item.FindControl("hdnDetailsId");
        HiddenField hdnItemId = (HiddenField)item.FindControl("hdnItemId");
        HiddenField hdnIndentId = (HiddenField)item.FindControl("hdnIndentId");


        //////Response.Redirect("VisitLabResultDetails.aspx");

    }

    protected void ibtnVitalsGraph_Click(object sender, ImageClickEventArgs e)
    {
        //////var ibtn = (ImageButton)sender;
        //////RepeaterItem item = (RepeaterItem)ibtn.NamingContainer;
        //////HiddenField hdnDetailsId = (HiddenField)item.FindControl("hdnDetailsId");
        //////HiddenField hdnItemId = (HiddenField)item.FindControl("hdnItemId");
        //////HiddenField hdnIndentId = (HiddenField)item.FindControl("hdnIndentId");


        //////Response.Redirect("VisitLabResultDetails.aspx");

    }

    //protected void rptLabvisitpaging_ItemCommand(object source, RepeaterCommandEventArgs e)
    //{

    //}
    //////protected void rbtnSearh_SelectedIndexChanged(object sender, EventArgs e)
    //////{
    //////    foreach (ListItem list in rbtnSearh.Items)
    //////    {
    //////        if (list.Selected)
    //////        {
    //////            list.Attributes.Add("Style", "color: white;background-color:skyblue;");// = "Red";
    //////        }
    //////    }

    //////    if (rbtnSearh.SelectedValue == "DateRange")
    //////    {
    //////        dtpfrmdate.Visible = true;
    //////        dtpTodate.Visible = true;
    //////        btnFilter.Visible = true;
    //////    }
    //////    else
    //////    {
    //////        dtpfrmdate.Visible = false;
    //////        dtpTodate.Visible = false;
    //////        btnFilter.Visible = false;

    //////        strDateRange = rbtnSearh.SelectedValue.ToString();
    //////        BindGrid();
    //////    }


    //////}
    protected void lbtnPage_Click(object sender, EventArgs e)
    {
        //LinkButton lbtn = (LinkButton)sender;
        //lbtn.BackColor = System.Drawing.Color.SkyBlue;
    }

    protected void rpfCategory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton btnDischargeSummary = (ImageButton)e.Row.FindControl("btnDischargeSummary");
            ImageButton btnOPSummary = (ImageButton)e.Row.FindControl("btnOPSummary");
            Label lblpatienttype = (Label)e.Row.FindControl("lblType");

            btnDischargeSummary.Visible = false;
            btnOPSummary.Visible = false;
            if (common.myStr(lblpatienttype.Text).Trim().ToUpper().Equals("IP"))
            {
                btnDischargeSummary.Visible = true;
            }
            else
            {
                btnOPSummary.Visible = true;
            }
        }
    }
    protected void rpfCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {


        if (e.Item.ItemType == ListItemType.Item)
        {

            GridViewRow row = (GridViewRow)(((ImageButton)sender).NamingContainer);
            HiddenField btnDischargeSummary = (HiddenField)row.FindControl("btnDischargeSummary");
            HiddenField btnOPSummary = (HiddenField)row.FindControl("btnOPSummary");

            Label lblpatienttype = (Label)e.Item.FindControl("lblType");
            Label lblEncounterid = (Label)e.Item.FindControl("lblEncounterid");
            Label lblEncounterNo = (Label)e.Item.FindControl("lblEncounterNo");
            Label lblDoctorId = (Label)e.Item.FindControl("lblDoctorId");
            lblEncounterid.Visible = false;
            lblEncounterNo.Visible = false;
            lblDoctorId.Visible = false;

            ImageButton lbtn = (ImageButton)e.Item.FindControl("PatientType");


            if (lblpatienttype.Text.Trim() == "OP")
            {
                btnDischargeSummary.Visible = false;
                btnOPSummary.Visible = true;

                lbtn.Enabled = false;
                lbtn.Attributes.CssStyle[HtmlTextWriterStyle.Visibility] = "hidden";

                //   lbtn.Visible = false;
                //   PatientType.Visible = false;

            }
            else
            {
                btnDischargeSummary.Visible = true;
                btnOPSummary.Visible = false;

                lbtn.Enabled = true;
                lbtn.Visible = true;
            }

        }


        else if (e.Item.ItemType == ListItemType.AlternatingItem)
        {

            Label lblpatienttype = (Label)e.Item.FindControl("lblType");

            ImageButton lbtn = (ImageButton)e.Item.FindControl("PatientType");
            Label lblEncounterid = (Label)e.Item.FindControl("lblEncounterid");
            Label lblEncounterNo = (Label)e.Item.FindControl("lblEncounterNo");
            Label lblDoctorId = (Label)e.Item.FindControl("lblDoctorId");
            lblEncounterid.Visible = false;
            lblEncounterNo.Visible = false;
            lblDoctorId.Visible = false;

            if (lblpatienttype.Text.Trim() == "OP")
            {
                lbtn.Enabled = false;
                lbtn.Attributes.CssStyle[HtmlTextWriterStyle.Visibility] = "hidden";

                //   lbtn.Visible = false;
                //   PatientType.Visible = false;

            }
            else
            {
                lbtn.Enabled = true;
                lbtn.Visible = true;
            }

            //ImageButton lbtn1 = (ImageButton)e.Item.FindControl("PatientType");

            //lbtn1.Visible = false;   

        }





    }
    protected void dvVitals_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        dvVitals.PageIndex = e.NewPageIndex;

        dvVitals.DataSource = ViewState["Vitals"];
        dvVitals.DataBind();

        //   ViewState["Vitals"] = dsTemp.Tables[3];

    }

    protected void gvPrescribedMedicine_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataSet dsOtherPatientDetail = (DataSet)ViewState["OtherPatientDetail"];

        if (dsOtherPatientDetail != null)
        {
            if (dsOtherPatientDetail.Tables.Count > 1)
            {
                gvPrescribedMedicine.PageIndex = e.NewPageIndex;

                gvPrescribedMedicine.DataSource = dsOtherPatientDetail.Tables[2];
                gvPrescribedMedicine.DataBind();
            }
        }

    }

    protected void gvPrescribedMedicine_DataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.Header))
            {
                e.Row.Visible = false;
            }

            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                Label lblPrescriptionDetail = (Label)e.Row.FindControl("lblPrescriptionDetail");

                lblPrescriptionDetail.Text = common.myStr(lblPrescriptionDetail.Text).Replace("\n", "\r\n").Replace("\r\n", "<br />");
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            //objException.HandleException(Ex);
        }
    }


    protected void dvVitals_DataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.Header) || e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                if (e.Row.RowType.Equals(DataControlRowType.DataRow))
                {
                    string HeaderColumn = string.Empty;

                    HyperLink hplVitalValue = (HyperLink)e.Row.FindControl("hplVitalValue");
                    Label lblDisplayName = (Label)e.Row.FindControl("lblDisplayName");


                    e.Row.BorderColor = System.Drawing.Color.LightGray;

                    if (common.myLen(hplVitalValue.Text) > 0)
                    {
                        hplVitalValue.Attributes.Add("onclick", "setVitalValue('" + common.myStr(hplVitalValue.Text) + "','" + common.myStr(lblDisplayName.Text) + "');");
                    }
                    ////if (common.myLen(lblWT.Text) > 0)
                    ////{
                    ////    lblWT.Attributes.Add("onclick", "setVitalValue('" + common.myStr(lblWT.Text) + "','WT');");
                    ////}
                    ////if (common.myLen(lblHC.Text) > 0)
                    ////{
                    ////    lblHC.Attributes.Add("onclick", "setVitalValue('" + common.myStr(lblHC.Text) + "','HC');");
                    ////}
                    ////if (common.myLen(lblT.Text) > 0)
                    ////{
                    ////    lblT.Attributes.Add("onclick", "setVitalValue('" + common.myStr(lblT.Text) + "','T');");
                    ////}
                    ////if (common.myLen(lblR.Text) > 0)
                    ////{
                    ////    lblR.Attributes.Add("onclick", "setVitalValue('" + common.myStr(lblR.Text) + "','R');");
                    ////}
                    ////if (common.myLen(lblP.Text) > 0)
                    ////{
                    ////    lblP.Attributes.Add("onclick", "setVitalValue('" + common.myStr(lblP.Text) + "','P');");
                    ////}
                    ////if (common.myLen(lblBPS.Text) > 0)
                    ////{
                    ////    lblBPS.Attributes.Add("onclick", "setVitalValue('" + common.myStr(lblBPS.Text) + "','BPS');");
                    ////}
                    ////if (common.myLen(lblBPD.Text) > 0)
                    ////{
                    ////    lblBPD.Attributes.Add("onclick", "setVitalValue('" + common.myStr(lblBPD.Text) + "','BPD');");
                    ////}
                    ////if (common.myLen(lblMAC.Text) > 0)
                    ////{
                    ////    lblMAC.Attributes.Add("onclick", "setVitalValue('" + common.myStr(lblMAC.Text) + "','MAC');");
                    ////}
                    ////if (common.myLen(lblSpO2.Text) > 0)
                    ////{
                    ////    lblSpO2.Attributes.Add("onclick", "setVitalValue('" + common.myStr(lblSpO2.Text) + "','SPO2');");
                    ////}
                    ////if (common.myLen(lblBMI.Text) > 0)
                    ////{
                    ////    lblBMI.Attributes.Add("onclick", "setVitalValue('" + common.myStr(lblBMI.Text) + "','BMI');");
                    ////}
                    ////if (common.myLen(lblBSA.Text) > 0)
                    ////{
                    ////    lblBSA.Attributes.Add("onclick", "setVitalValue('" + common.myStr(lblBSA.Text) + "','BSA');");
                    ////}

                    //for (int idx = 0; idx < e.Row.Cells.Count; idx++)
                    //{
                    //    if (e.Row.RowType.Equals(DataControlRowType.DataRow))
                    //    {
                    //        if (!common.myStr(this.gvVitals.HeaderRow.Cells[idx].Text).ToUpper().Equals("VITAL DATE"))
                    //        {
                    //            string HeaderColumn = common.myStr(this.gvVitals.HeaderRow.Cells[idx].Text);
                    //            e.Row.Cells[idx].Attributes.Add("onclick", "setVitalValue('" + common.myStr(e.Row.Cells[idx].Text) + "','" + HeaderColumn + "');");
                    //        }
                    //    }
                    //}
                }

                if (e.Row.RowType.Equals(DataControlRowType.DataRow))
                {
                    HiddenField hdnT_ABNORMAL_VALUE = (HiddenField)e.Row.FindControl("hdnT_ABNORMAL_VALUE");
                    if (common.myInt(hdnT_ABNORMAL_VALUE.Value).Equals(1))
                    {
                        Label lblT = (Label)e.Row.FindControl("lblT");
                        lblT.ForeColor = System.Drawing.Color.Red;
                    }
                    HiddenField hdnR_ABNORMAL_VALUE = (HiddenField)e.Row.FindControl("hdnR_ABNORMAL_VALUE");
                    if (common.myInt(hdnR_ABNORMAL_VALUE.Value).Equals(1))
                    {
                        Label lblR = (Label)e.Row.FindControl("lblR");
                        lblR.ForeColor = System.Drawing.Color.Red;
                    }
                    HiddenField hdnP_ABNORMAL_VALUE = (HiddenField)e.Row.FindControl("hdnP_ABNORMAL_VALUE");
                    if (common.myInt(hdnP_ABNORMAL_VALUE.Value).Equals(1))
                    {
                        Label lblP = (Label)e.Row.FindControl("lblP");
                        lblP.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }

        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            //objException.HandleException(Ex);
        }
    }

    //if (dvVitals.Rows.Count > 0)
    //{
    //    //string DataItem = ((DataRowView)dvVitals.DataItem).Row.ItemArray[2].ToString();
    //    string DataItem = (dvVitals.Rows[e.RowIndex].DataItem).Row.ItemArray[2].ToString();





    //    //     HyperLink hp = (HyperLink)(dvVitals.FindControl["hypHT"]);

    //    HyperLink hypHT = (HyperLink)dvVitals.FindControl("hypHT");
    //    //   hypHT.NavigateUrl = "~/EMR/Vitals/Vitalgraph.aspx?Name=HT";
    //    hypHT.Attributes.Add("onclick", "setVitalValue('" + common.myStr(hypHT.Text) + "','HT');");

    //    HyperLink hypWT = (HyperLink)dvVitals.FindControl("hypWT");
    //    //  hypWT.NavigateUrl = "~/EMR/Vitals/Vitalgraph.aspx?Name=WT";

    //    hypWT.Attributes.Add("onclick", "setVitalValue('" + common.myStr(hypWT.Text) + "','WT');");

    //    HyperLink hypHC = (HyperLink)dvVitals.FindControl("hypHC");
    //    //hypHC.NavigateUrl = "~/EMR/Vitals/Vitalgraph.aspx?Name=HC";

    //    hypHC.Attributes.Add("onclick", "setVitalValue('" + common.myStr(hypHC.Text) + "','HC');");

    //    HyperLink hypT = (HyperLink)dvVitals.FindControl("hypT");
    //    //  hypT.NavigateUrl = "~/EMR/Vitals/Vitalgraph.aspx?Name=T";
    //    hypT.Attributes.Add("onclick", "setVitalValue('" + common.myStr(hypT.Text) + "','T');");

    //    HyperLink hypR = (HyperLink)dvVitals.FindControl("hypR");
    //    //    hypR.NavigateUrl = "~/EMR/Vitals/Vitalgraph.aspx?Name=R";
    //    hypR.Attributes.Add("onclick", "setVitalValue('" + common.myStr(hypR.Text) + "','R');");


    //    HyperLink hypP = (HyperLink)dvVitals.FindControl("hypP");
    //    //   hypP.NavigateUrl = "~/EMR/Vitals/Vitalgraph.aspx?Name=P";
    //    hypP.Attributes.Add("onclick", "setVitalValue('" + common.myStr(hypP.Text) + "','P');");

    //    HyperLink hypBPS = (HyperLink)dvVitals.FindControl("hypBPS");
    //    //    hypBPS.NavigateUrl = "~/EMR/Vitals/Vitalgraph.aspx?Name=BPS";
    //    hypBPS.Attributes.Add("onclick", "setVitalValue('" + common.myStr(hypBPS.Text) + "','BPS');");


    //    HyperLink hypBPD = (HyperLink)dvVitals.FindControl("hypBPD");
    //    //      hypBPD.NavigateUrl = "~/EMR/Vitals/Vitalgraph.aspx?Name=BPD";
    //    hypBPD.Attributes.Add("onclick", "setVitalValue('" + common.myStr(hypBPD.Text) + "','BPD');");

    //    HyperLink hypMAC = (HyperLink)dvVitals.FindControl("hypMAC");
    //    //    hypMAC.NavigateUrl = "~/EMR/Vitals/Vitalgraph.aspx?Name=MAC";
    //    hypMAC.Attributes.Add("onclick", "setVitalValue('" + common.myStr(hypMAC.Text) + "','MAC');");

    //    HyperLink hypSPO2 = (HyperLink)dvVitals.FindControl("hypSPO2");
    //    //      hypSPO2.NavigateUrl = "~/EMR/Vitals/Vitalgraph.aspx?Name=SPO2";
    //    hypSPO2.Attributes.Add("onclick", "setVitalValue('" + common.myStr(hypSPO2.Text) + "','SPO2');");

    //    HyperLink hypBMI = (HyperLink)dvVitals.FindControl("hypBMI");
    //    //     hypBMI.NavigateUrl = "~/EMR/Vitals/Vitalgraph.aspx?Name=BMI";
    //    hypBMI.Attributes.Add("onclick", "setVitalValue('" + common.myStr(hypBMI.Text) + "','BMI');");

    //    HyperLink hypBSA = (HyperLink)dvVitals.FindControl("hypBSA");
    //    //   hypBSA.NavigateUrl = "~/EMR/Vitals/Vitalgraph.aspx?Name=BSA";
    //    hypBSA.Attributes.Add("onclick", "setVitalValue('" + common.myStr(hypBSA.Text) + "','BSA');");


    //    HiddenField hdnT_ABNORMAL_VALUE = (HiddenField)dvVitals.FindControl("hdnT_ABNORMAL_VALUE");
    //    if (common.myInt(hdnT_ABNORMAL_VALUE.Value).Equals(1))
    //    {
    //        hypT.ForeColor = System.Drawing.Color.Red;

    //    }
    //    HiddenField hdnR_ABNORMAL_VALUE = (HiddenField)dvVitals.FindControl("hdnR_ABNORMAL_VALUE");
    //    if (common.myInt(hdnR_ABNORMAL_VALUE.Value).Equals(1))
    //    {
    //        hypR.ForeColor = System.Drawing.Color.Red;
    //    }
    //    HiddenField hdnP_ABNORMAL_VALUE = (HiddenField)dvVitals.FindControl("hdnP_ABNORMAL_VALUE");
    //    if (common.myInt(hdnP_ABNORMAL_VALUE.Value).Equals(1))
    //    {
    //        hypP.ForeColor = System.Drawing.Color.Red;
    //    }

    void bindGraph()
    {
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        objbc1 = new BaseC.Patient(sConString);
        try
        {
            string strsql = "";
            string strsql1 = "";
            // RadChart1.DefaultType = Telerik.Charting.ChartSeriesType.Line;
            //RadChart4.DefaultType = Telerik.Charting.ChartSeriesType.Line;
            //RadChart5.DefaultType = Telerik.Charting.ChartSeriesType.Line;
            //RadChart6.DefaultType = Telerik.Charting.ChartSeriesType.Line;


            //RadChart1.Clear();
            //RadChart1.Legend.Items.Clear();
            //RadChart1.ClearSkin();
            //RadChart1.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 30;

            //RadChart4.Clear();
            //RadChart4.Legend.Items.Clear();
            //RadChart4.ClearSkin();
            //RadChart4.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 30;

            //RadChart5.Clear();
            //RadChart5.Legend.Items.Clear();
            //RadChart5.ClearSkin();
            //RadChart5.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 30;

            //RadChart6.Clear();
            //RadChart6.Legend.Items.Clear();
            //RadChart6.ClearSkin();
            //RadChart6.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 30;

            strsql = "select top 10 t2.RegistrationId, t1.MinValue,t1.MaxValue,t1.CriticalValue ,t1.EncodedDate  from DiagInvResultIP t1 " +
                             " inner join DiagSampleIPLabMain t2 on t2.DiagSampleId = t1.diagsampleid " +
                             " where fieldid = 1313 and t2.RegistrationId = 17895 order by encodeddate desc ";
            ds = dl.FillDataSet(CommandType.Text, strsql);


            //strsql1 = "select top 10 t2.RegistrationId,T1.ValueText ,t1.CriticalValue,convert(Varchar(10),t1.EncodedDate,103) EncodedDate from DiagInvResultIP t1 " +
            //          " inner join DiagSampleIPLabMain t2 on t2.DiagSampleId = t1.diagsampleid " +
            //           " where fieldid = 41 and t2.RegistrationId = 267791 order by convert(Varchar(10),t1.EncodedDate,103) desc ";


            ds1 = objbc1.DiagLabGraphForDashboard(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]));
            DataView DV = ds1.Tables[0].DefaultView;
            if (ds.Tables.Count > 0)
            {
                DataTable DT = DV.ToTable(false, new string[] { "ValueText", "EncodedDate", "CriticalValue", "GraphParameter" });

                //    // Haemoglobin
                //    if (ds.Tables[0].Rows.Count > 0)
                //    {
                //        //if (ddlField.SelectedItem != null)
                //        //{
                //        //    DataView dv1 = ds.Tables[0].DefaultView;
                //        //    dv1.RowFilter = "FieldId=" + common.myInt(ddlField.SelectedValue) + "";
                //        //    DataTable dt0 = dv1.ToTable();

                //        //    if (common.myStr(dt0.Rows[0]["UnitName"].ToString()) != "")
                //        //    {
                //        //        RadChart1.ChartTitle.TextBlock.Text = common.myStr(ddlField.SelectedItem.Text) + " (" + common.myStr(dt0.Rows[0]["UnitName"].ToString()) + ")";
                //        //    }
                //        //    else
                //        //    {
                //        //        RadChart1.ChartTitle.TextBlock.Text = common.myStr(ddlField.SelectedItem.Text);
                //        //    }

                //        //    if (common.myStr(dt0.Rows[0]["MinValue"].ToString()) != "" && common.myStr(dt0.Rows[0]["MaxValue"].ToString()) != "")
                //        //    {
                //        //        RadChart1.ChartTitle.TextBlock.Text += " (" + common.myDbl(dt0.Rows[0]["MinValue"]).ToString("F2") + common.myStr(dt0.Rows[0]["Symbol"]) + common.myDbl(dt0.Rows[0]["MaxValue"]).ToString("F2") + ")";
                //        //    }
                //        //}
                //        lblLabHeader.Text = common.myStr(DT.Rows[0]["GraphParameter"]);

                //        RadChart1.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 30;

                //        RadChart1.DataSource = DT;

                //        RadChart1.ChartTitle.Visible = false;
                //        RadChart1.Legend.Visible = false;

                //        // RadChart1.PlotArea.XAxis.AutoScale = false;

                //        //RadChart1.PlotArea.XAxis.DataLabelsColumn = common.myStr(DT.Rows[0]["EncodedDate"]); 
                //        RadChart1.PlotArea.XAxis.DataLabelsColumn = "EncodedDate";
                //        RadChart1.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Font = new System.Drawing.Font("Arial", 8);



                //        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "EncodedDate";
                //        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color =
                //        System.Drawing.Color.Red;
                //        RadChart1.PlotArea.XAxis.Appearance.Width = 2;
                //        RadChart1.PlotArea.XAxis.Appearance.Color = System.Drawing.Color.Red;
                //        RadChart1.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Color =
                //               System.Drawing.Color.BlueViolet;

                //        // Set text and line for Y axis



                //        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "%";
                //        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color =
                //        System.Drawing.Color.Red;
                //        RadChart1.PlotArea.YAxis.Appearance.Width = 2;
                //        RadChart1.PlotArea.YAxis.Appearance.Color = System.Drawing.Color.Red;

                //        RadChart1.PlotArea.YAxis.Appearance.TextAppearance.TextProperties.Color =
                //                   System.Drawing.Color.BlueViolet;


                //        RadChart1.DataBind();

                //        int m = 0;
                //        foreach (DataRow DR in DT.Rows)
                //        {
                //            //for (int i = 0; i <= DT.Rows.Count; i++)
                //            //{
                //            if (common.myBool(DR["CriticalValue"]) == true)
                //            {
                //                RadChart1.Series[0].Appearance.LabelAppearance.FillStyle.MainColor = System.Drawing.Color.Red;
                //                //RadChart1.Series[0].Appearance.LabelAppearance.LabelLocation = Telerik.Charting.Styles.StyleSeriesItemLabel.ItemLabelLocation.Inside;
                //                //RadChart1.Series[0].Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.BottomRight;

                //            }
                //            else
                //            {
                //                RadChart1.Series[0].Appearance.LabelAppearance.FillStyle.MainColor = System.Drawing.Color.Green;
                //            }
                //            // }
                //            m = m + 1;
                //        }

                //    }
            }



            // Creatinine
            DataView DV0 = ds1.Tables[0].DefaultView;
            DataTable DT0 = DV0.ToTable(false, new string[] { "ValueText", "EncodedDate", "CriticalValue", "GraphParameter" });

            if (DT0.Rows.Count > 0)
            {
                //  RadChart4.DefaultType = Telerik.Charting.ChartSeriesType.Line;
                //   Label1.Text = common.myStr(DT0.Rows[0]["GraphParameter"]);
                //    RadChart1.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 30;

                //    RadChart1.DataSource = DT0;

                //    RadChart1.ChartTitle.Visible = false;
                //    RadChart1.Legend.Visible = false;

                //    RadChart1.PlotArea.XAxis.DataLabelsColumn = "EncodedDate";
                //    RadChart1.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Font = new System.Drawing.Font("Arial", 8);
                //    RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = "EncodedDate";
                //    RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color =
                //System.Drawing.Color.Red;
                //    RadChart1.PlotArea.XAxis.Appearance.Width = 2;
                //    RadChart1.PlotArea.XAxis.Appearance.Color = System.Drawing.Color.Red;
                //    RadChart1.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Color =
                //       System.Drawing.Color.BlueViolet;

                //    // Set text and line for Y axis

                //    RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = "%";
                //    RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color =
                //System.Drawing.Color.Red;
                //    RadChart1.PlotArea.YAxis.Appearance.Width = 2;
                //    RadChart1.PlotArea.YAxis.Appearance.Color = System.Drawing.Color.Red;

                //    RadChart1.PlotArea.YAxis.Appearance.TextAppearance.TextProperties.Color =
                //           System.Drawing.Color.BlueViolet;
                //    RadChart1.DataBind();

                //    RadChart1.Series[0].Appearance.LabelAppearance.FillStyle.MainColor = System.Drawing.Color.Green;

            }



            // Creatinine
            DataView DV1 = ds1.Tables[1].DefaultView;
            DataTable DT1 = DV1.ToTable(false, new string[] { "ValueText", "EncodedDate", "CriticalValue", "GraphParameter" });

            if (DT1.Rows.Count > 0)
            {
                //  RadChart4.DefaultType = Telerik.Charting.ChartSeriesType.Line;
                //Label1.Text = common.myStr(DT1.Rows[0]["GraphParameter"]);
                //RadChart4.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 30;

                //RadChart4.DataSource = DT1;

                //RadChart4.ChartTitle.Visible = false;
                //RadChart4.Legend.Visible = false;

                //RadChart4.PlotArea.XAxis.DataLabelsColumn = "EncodedDate";
                //RadChart4.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Font = new System.Drawing.Font("Arial", 8);
                //RadChart4.PlotArea.XAxis.AxisLabel.TextBlock.Text = "EncodedDate";
                //RadChart4.PlotArea.XAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color =
                //System.Drawing.Color.Red;
                //RadChart4.PlotArea.XAxis.Appearance.Width = 2;
                //RadChart4.PlotArea.XAxis.Appearance.Color = System.Drawing.Color.Red;
                //RadChart4.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Color =
                //       System.Drawing.Color.BlueViolet;

                //// Set text and line for Y axis

                //RadChart4.PlotArea.YAxis.AxisLabel.TextBlock.Text = "%";
                //RadChart4.PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color =
                //System.Drawing.Color.Red;
                //RadChart4.PlotArea.YAxis.Appearance.Width = 2;
                //RadChart4.PlotArea.YAxis.Appearance.Color = System.Drawing.Color.Red;

                //RadChart4.PlotArea.YAxis.Appearance.TextAppearance.TextProperties.Color =
                //           System.Drawing.Color.BlueViolet;
                //RadChart4.DataBind();

                //RadChart4.Series[0].Appearance.LabelAppearance.FillStyle.MainColor = System.Drawing.Color.Green;

            }


            // Glucose
            DataView DV2 = ds1.Tables[2].DefaultView;
            DataTable DT2 = DV2.ToTable(false, new string[] { "ValueText", "EncodedDate", "CriticalValue", "GraphParameter" });

            if (DT2.Rows.Count > 0)
            {
                //Label22.Text = common.myStr(DT2.Rows[0]["GraphParameter"]);
                //RadChart5.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 30;
                //RadChart5.DataSource = DT2;
                //RadChart5.ChartTitle.Visible = false;
                //RadChart5.Legend.Visible = false;

                //RadChart5.PlotArea.XAxis.DataLabelsColumn = "EncodedDate";
                //RadChart5.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Font = new System.Drawing.Font("Arial", 8);
                //RadChart5.PlotArea.XAxis.AxisLabel.TextBlock.Text = "EncodedDate";
                //RadChart5.PlotArea.XAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color =
                //System.Drawing.Color.Red;
                //RadChart5.PlotArea.XAxis.Appearance.Width = 2;
                //RadChart5.PlotArea.XAxis.Appearance.Color = System.Drawing.Color.Red;
                //RadChart5.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Color =
                //       System.Drawing.Color.BlueViolet;

                //// Set text and line for Y axis

                //RadChart5.PlotArea.YAxis.AxisLabel.TextBlock.Text = "%";
                //RadChart5.PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color =
                //System.Drawing.Color.Red;
                //RadChart5.PlotArea.YAxis.Appearance.Width = 2;
                //RadChart5.PlotArea.YAxis.Appearance.Color = System.Drawing.Color.Red;

                //RadChart5.PlotArea.YAxis.Appearance.TextAppearance.TextProperties.Color =
                //           System.Drawing.Color.BlueViolet;
                //RadChart5.DataBind();
                //RadChart5.Series[0].Appearance.LabelAppearance.FillStyle.MainColor = System.Drawing.Color.Green;

            }


            // Sugar
            DataView DV3 = ds1.Tables[3].DefaultView;
            DataTable DT3 = DV3.ToTable(false, new string[] { "ValueText", "EncodedDate", "CriticalValue", "GraphParameter" });

            if (DT3.Rows.Count > 0)
            {
                //Label33.Text = common.myStr(DT3.Rows[0]["GraphParameter"]);
                //RadChart6.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 30;


                ////RadChart6.PlotArea.YAxis.AutoScale = false;
                ////RadChart6.PlotArea.YAxis.MinValue = common.myInt(ds1.Tables[3].Rows[0]["MinValue"]);
                ////RadChart6.PlotArea.YAxis.MaxValue = common.myInt(ds1.Tables[3].Rows[0]["MaxValue"]);
                ////RadChart6.PlotArea.YAxis.Step = common.myInt(ds1.Tables[3].Rows[0]["Step"]);


                //RadChart6.DataSource = DT3;
                //RadChart6.ChartTitle.Visible = false;
                //RadChart6.Legend.Visible = false;

                //RadChart6.PlotArea.XAxis.DataLabelsColumn = "EncodedDate";
                //RadChart6.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Font = new System.Drawing.Font("Arial", 8);
                //RadChart6.PlotArea.XAxis.AxisLabel.TextBlock.Text = "EncodedDate";
                //RadChart6.PlotArea.XAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color =
                //System.Drawing.Color.Red;
                //RadChart6.PlotArea.XAxis.Appearance.Width = 2;
                //RadChart6.PlotArea.XAxis.Appearance.Color = System.Drawing.Color.Red;
                //RadChart6.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Color =
                //       System.Drawing.Color.BlueViolet;

                //// Set text and line for Y axis


                //RadChart6.PlotArea.YAxis.AxisLabel.TextBlock.Text = "%";
                //RadChart6.PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color =
                //System.Drawing.Color.Red;
                //RadChart6.PlotArea.YAxis.Appearance.Width = 2;
                //RadChart6.PlotArea.YAxis.Appearance.Color = System.Drawing.Color.Red;

                //RadChart6.PlotArea.YAxis.Appearance.TextAppearance.TextProperties.Color =
                //           System.Drawing.Color.BlueViolet;
                //RadChart6.DataBind();
                //RadChart6.Series[0].Appearance.LabelAppearance.FillStyle.MainColor = System.Drawing.Color.Green;

            }
        }
        catch (Exception Ex)
        {

            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }
    void bindGraphView()
    {
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            string strsql = "";
            string strsql1 = "";
            //RadChart2.DefaultType = Telerik.Charting.ChartSeriesType.Line;
            //RadChart7.DefaultType = Telerik.Charting.ChartSeriesType.Line;
            //RadChart8.DefaultType = Telerik.Charting.ChartSeriesType.Line;
            //RadChart9.DefaultType = Telerik.Charting.ChartSeriesType.Line;


            //RadChart2.Clear();
            //RadChart2.Legend.Items.Clear();
            //RadChart2.ClearSkin();
            //RadChart2.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 30;

            //RadChart7.Clear();
            //RadChart7.Legend.Items.Clear();
            //RadChart7.ClearSkin();
            //RadChart7.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 30;

            //RadChart8.Clear();
            //RadChart8.Legend.Items.Clear();
            //RadChart8.ClearSkin();
            //RadChart8.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 30;

            //RadChart9.Clear();
            //RadChart9.Legend.Items.Clear();
            //RadChart9.ClearSkin();
            //RadChart9.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 30;





            //strsql = "select top 10 t2.RegistrationId, t1.MinValue,t1.MaxValue,t1.CriticalValue ,t1.EncodedDate  from DiagInvResultIP t1 " +
            //                 " inner join DiagSampleIPLabMain t2 on t2.DiagSampleId = t1.diagsampleid " +
            //                 " where fieldid = 41 and t2.RegistrationId = 267791 order by encodeddate desc ";
            //ds = dl.FillDataSet(CommandType.Text, strsql);

            //ds1 = objbc1.DiagLabGraphForDashboard(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]));

            //DataView DV = ds1.Tables[0].DefaultView;
            //if (ds.Tables.Count > 0)
            //{
            //    DataTable DT = DV.ToTable(false, new string[] { "ValueText", "EncodedDate", "CriticalValue", "GraphParameter" });

            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        Label77.Text = common.myStr(DT.Rows[0]["GraphParameter"]);
            //        RadChart2.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 30;
            //        RadChart2.DataSource = DT;
            //        RadChart2.ChartTitle.Visible = false;
            //        RadChart2.Legend.Visible = false;
            //        RadChart2.PlotArea.XAxis.DataLabelsColumn = "EncodedDate";
            //        RadChart2.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Font = new System.Drawing.Font("Arial", 8);
            //        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Text = "EncodedDate";
            //        RadChart2.PlotArea.XAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color =
            //        System.Drawing.Color.Red;
            //        RadChart2.PlotArea.XAxis.Appearance.Width = 2;
            //        RadChart2.PlotArea.XAxis.Appearance.Color = System.Drawing.Color.Red;
            //        RadChart2.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Color =
            //               System.Drawing.Color.BlueViolet;
            //        RadChart2.PlotArea.YAxis.AxisLabel.TextBlock.Text = "%";
            //        RadChart2.PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color =
            //        System.Drawing.Color.Red;
            //        RadChart2.PlotArea.YAxis.Appearance.Width = 2;
            //        RadChart2.PlotArea.YAxis.Appearance.Color = System.Drawing.Color.Red;

            //        RadChart2.PlotArea.YAxis.Appearance.TextAppearance.TextProperties.Color =
            //                   System.Drawing.Color.BlueViolet;

            //        RadChart2.DataBind();

            //        RadChart2.Series[0].Appearance.LabelAppearance.FillStyle.MainColor = System.Drawing.Color.Green;

            //    }
            //}



            //// Creatinine
            //DataView DV1 = ds1.Tables[1].DefaultView;
            //DataTable DT1 = DV1.ToTable(false, new string[] { "ValueText", "EncodedDate", "CriticalValue", "GraphParameter" });

            //if (DT1.Rows.Count > 0)
            //{
            //    Label44.Text = common.myStr(DT1.Rows[0]["GraphParameter"]);
            //    RadChart7.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 30;
            //    RadChart7.DataSource = DT1;
            //    RadChart7.ChartTitle.Visible = false;
            //    RadChart7.Legend.Visible = false;

            //    RadChart7.PlotArea.XAxis.DataLabelsColumn = "EncodedDate";
            //    RadChart7.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Font = new System.Drawing.Font("Arial", 8);
            //    RadChart7.PlotArea.XAxis.AxisLabel.TextBlock.Text = "EncodedDate";
            //    RadChart7.PlotArea.XAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color =
            //    System.Drawing.Color.Red;
            //    RadChart7.PlotArea.XAxis.Appearance.Width = 2;
            //    RadChart7.PlotArea.XAxis.Appearance.Color = System.Drawing.Color.Red;
            //    RadChart7.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Color =
            //           System.Drawing.Color.BlueViolet;

            //    // Set text and line for Y axis

            //    RadChart7.PlotArea.YAxis.AxisLabel.TextBlock.Text = "%";
            //    RadChart7.PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color =
            //    System.Drawing.Color.Red;
            //    RadChart7.PlotArea.YAxis.Appearance.Width = 2;
            //    RadChart7.PlotArea.YAxis.Appearance.Color = System.Drawing.Color.Red;

            //    RadChart7.PlotArea.YAxis.Appearance.TextAppearance.TextProperties.Color =
            //               System.Drawing.Color.BlueViolet;
            //    RadChart7.DataBind();
            //    RadChart7.Series[0].Appearance.LabelAppearance.FillStyle.MainColor = System.Drawing.Color.Green;

            //}


            //// Glucose
            //DataView DV2 = ds1.Tables[2].DefaultView;
            //DataTable DT2 = DV2.ToTable(false, new string[] { "ValueText", "EncodedDate", "CriticalValue", "GraphParameter" });

            //if (DT2.Rows.Count > 0)
            //{
            //    //    RadChart8.DefaultType = Telerik.Charting.ChartSeriesType.Line;
            //    Label55.Text = common.myStr(DT2.Rows[0]["GraphParameter"]);
            //    RadChart8.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 30;
            //    RadChart8.DataSource = DT2;
            //    RadChart8.ChartTitle.Visible = false;
            //    RadChart8.Legend.Visible = false;

            //    RadChart8.PlotArea.XAxis.DataLabelsColumn = "EncodedDate";
            //    RadChart8.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Font = new System.Drawing.Font("Arial", 8);
            //    RadChart8.PlotArea.XAxis.AxisLabel.TextBlock.Text = "EncodedDate";
            //    RadChart8.PlotArea.XAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color =
            //    System.Drawing.Color.Red;
            //    RadChart8.PlotArea.XAxis.Appearance.Width = 2;
            //    RadChart8.PlotArea.XAxis.Appearance.Color = System.Drawing.Color.Red;
            //    RadChart8.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Color =
            //           System.Drawing.Color.BlueViolet;

            //    // Set text and line for Y axis

            //    RadChart8.PlotArea.YAxis.AxisLabel.TextBlock.Text = "%";
            //    RadChart8.PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color =
            //    System.Drawing.Color.Red;
            //    RadChart8.PlotArea.YAxis.Appearance.Width = 2;
            //    RadChart8.PlotArea.YAxis.Appearance.Color = System.Drawing.Color.Red;

            //    RadChart8.PlotArea.YAxis.Appearance.TextAppearance.TextProperties.Color =
            //               System.Drawing.Color.BlueViolet;
            //    RadChart8.DataBind();
            //    RadChart8.Series[0].Appearance.LabelAppearance.FillStyle.MainColor = System.Drawing.Color.Green;

            //}


            //// Creatinine
            //DataView DV3 = ds1.Tables[3].DefaultView;
            //DataTable DT3 = DV3.ToTable(false, new string[] { "ValueText", "EncodedDate", "CriticalValue", "GraphParameter" });

            //if (DT3.Rows.Count > 0)
            //{
            //    Label66.Text = common.myStr(DT3.Rows[0]["GraphParameter"]);
            //    RadChart9.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 30;
            //    RadChart9.DataSource = DT3;
            //    RadChart9.ChartTitle.Visible = false;
            //    RadChart9.Legend.Visible = false;

            //    RadChart9.PlotArea.XAxis.DataLabelsColumn = "EncodedDate";
            //    RadChart9.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Font = new System.Drawing.Font("Arial", 8);
            //    RadChart9.PlotArea.XAxis.AxisLabel.TextBlock.Text = "EncodedDate";
            //    RadChart9.PlotArea.XAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color =
            //    System.Drawing.Color.Red;
            //    RadChart9.PlotArea.XAxis.Appearance.Width = 2;
            //    RadChart9.PlotArea.XAxis.Appearance.Color = System.Drawing.Color.Red;
            //    RadChart9.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Color =
            //           System.Drawing.Color.BlueViolet;

            //    // Set text and line for Y axis

            //    RadChart9.PlotArea.YAxis.AxisLabel.TextBlock.Text = "%";
            //    RadChart9.PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Color =
            //    System.Drawing.Color.Red;
            //    RadChart9.PlotArea.YAxis.Appearance.Width = 2;
            //    RadChart9.PlotArea.YAxis.Appearance.Color = System.Drawing.Color.Red;

            //    RadChart9.PlotArea.YAxis.Appearance.TextAppearance.TextProperties.Color =
            //               System.Drawing.Color.BlueViolet;
            //    RadChart9.DataBind();
            //    RadChart9.Series[0].Appearance.LabelAppearance.FillStyle.MainColor = System.Drawing.Color.Green;

            // }


        }
        catch (Exception Ex)
        {

            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }



    /// <summary>
    /// for past clinical notes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgPastClinicalNote_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridViewRow row = (GridViewRow)(((ImageButton)sender).NamingContainer);

            hiddenencounterid = (HiddenField)row.FindControl("hdnEncounterId");
            hiddenencounterno = (HiddenField)row.FindControl("hdnEncounterNo");


            //if (IsAllowPopup())
            //{
            //    return;
            //}
            //else
            //{
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "btnPastClinicalNote();", true);
            // }
        }
        catch (Exception ex)
        {
        }
    }

    HiddenField hdnEncounterId;
    protected void btnPrintReport_OnClick(object sender, EventArgs e)
    {
        ViewState["DoctorId"] = string.Empty;
        GridViewRow row = (GridViewRow)(((ImageButton)sender).NamingContainer);
        Label lblDate = (Label)row.FindControl("lblDate");
        HiddenField hdnDoctorId = (HiddenField)row.FindControl("hdnDoctorId");
        hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");
        ViewState["vsTmpEncounterId"] = hdnEncounterId.Value;
        // Session["DoctorId"] = hdnDoctorId.Value;
        string url = HttpContext.Current.Request.Url.AbsoluteUri;

        if (url.Contains("https://"))
        {
            path = url.Replace("https://", "");
            path = "https://" + path.Substring(0, path.IndexOf("/") + 1);
        }
        else
        {
            path = url.Replace("http://", "");
            path = "http://" + path.Substring(0, path.IndexOf("/") + 1);
        }
        clsIVF objivf = new clsIVF(sConString);

        try
        {
            ViewState["DoctorId"] = common.myInt(hdnDoctorId.Value).ToString();

            //if (common.myStr(ViewState["EMRFollowUpAppointmentOPSummaryValidation"]).ToUpper().Equals("Y"))
            //{
            //    objivf.SaveIsNoFollowUpRequired(common.myInt(Session["EncounterId"]), chkIsNoFollowUpRequired.Checked);

            //    if (!chkIsNoFollowUpRequired.Checked)
            //    {
            //        if (!IsallowFlowUpRequired())
            //        {

            //            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //            lblMessage.Text = "Kindly define the follow-up appointment !";

            //            Alert.ShowAjaxMsg(lblMessage.Text, this.Page);

            //            return;
            //        }
            //    }

            //}

            DataSet ds = new DataSet();
            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

            ds = objEMR.getReportFormatDetails(common.myInt(common.myInt(hdnDoctorId.Value)));
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ViewState["reportid"] = common.myStr(ds.Tables[0].Rows[0]["reportid"]);
                        ViewState["reportname"] = common.myStr(ds.Tables[0].Rows[0]["reportname"]);
                        ViewState["headerid"] = common.myStr(ds.Tables[0].Rows[0]["headerid"]);
                    }
                    else
                    {
                        Alert.ShowAjaxMsg("Report Format not tagged", this.Page);
                        return;
                    }
                }
                else
                {
                    Alert.ShowAjaxMsg("Report Format not tagged", this.Page);
                    return;
                }
            }
            else
            {
                Alert.ShowAjaxMsg("Report Format not tagged", this.Page);
                return;
            }
            getDoctorImage();
            generateReport(lblDate.Text);

            //hdnReportContent.Value = PrintReport(true);

            //if (common.myLen(hdnReportContent.ClientID) > 0 && common.myInt(ddlReport.SelectedValue) > 0)
            //{

            if (common.myLen(hdnReportContent.ClientID) > 0 && common.myInt(ViewState["reportid"]) > 0)
            {
                //Session["PrintReportWordProcessorWiseData"] = common.myStr(hdnReportContent.Value).Replace("<br/></span><br/>", "<br/>").Replace("<br/><br/>", "<br/>").Replace("<br /><br/>", "<br/>");
                //     Session["PrintReportWordProcessorWiseData"] = common.myStr(hdnReportContent.Value).Replace("<br /></span><br/>", "</span><br/>").Replace("<br/><br/>", "<br/>").Replace("<br /><br />", "<br/>").Replace("<br /><br/>", "<br/>").Replace("<br/><br />", "<br/>");
                Session["PrintReportWordProcessorWiseData"] = common.myStr(hdnReportContent.Value);

                //Session["PrintReportWordProcessorWiseData"] = common.myStr(hdnReportContent.Value);

                //ifrmpage.Attributes["src"] = path + "Editor/PrintReportWordProcessorWise.aspx?ReportId=" + common.myInt(ViewState["reportid"]) +
                //             "&HeaderId=" + common.myInt(ViewState["headerid"]) +
                //             "&RegistrationId=" + common.myInt(Session["RegistrationId"]);

                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "ShowPAgeO('Print Report');", true);

                string var = path + "Editor/PrintReportWordProcessorWise.aspx?ReportId=" + common.myInt(ViewState["reportid"]) +
                                     "&HeaderId=" + common.myInt(ViewState["headerid"]) +
                                     "&RegistrationId=" + common.myInt(Session["RegistrationId"]);

                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "ShowPAgePrint('" + var + "');", true);

                // RadWindow2.NavigateUrl = "~/Editor/PrintReportWordProcessorWise.aspx?ReportId=" + common.myInt(ViewState["reportid"]) +
                // "&HeaderId=" + common.myInt(ViewState["headerid"]) +
                // "&RegistrationId=" + common.myInt(Session["RegistrationId"]);
                //// ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "ShowPAgeO('Print Report');", true);


                // RadWindow2.Height = 500;
                // RadWindow2.Width = 1000;
                // RadWindow2.Top = 10;
                // RadWindow2.Left = 10;
                // RadWindow2.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                // RadWindow2.Modal = true;
                // RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                // RadWindow2.VisibleStatusbar = false;
            }
            else
            {
                Alert.ShowAjaxMsg("Data not found!", this.Page);
                return;
            }
        }
        catch (Exception Ex)
        {
            //  lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //  lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void getDoctorImage()
    {
        BaseC.clsLISPhlebotomy lis = new BaseC.clsLISPhlebotomy(sConString);
        BaseC.User user = new BaseC.User(sConString);
        DataSet ds = new DataSet();
        int intCheckImage = 0; // Check image from signed note signature
        Stream strm;
        Object img;
        DateTime SignatureDate;
        String UserName = "", ShowSignatureDate = "", UserDoctorId = "";
        String SignImage = "", SignNote = "";
        String DivStartTag = "<div id='dvDoctorImage' align='right'>";
        String SignedDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        StringBuilder strSQL = new StringBuilder();
        String strSingImagePath = "";
        String Education = string.Empty;
        String FileName = string.Empty;
        string strimgData = string.Empty;
        try
        {
            if (common.myInt(ViewState["DoctorID"]) > 0)
            {
                //ds = lis.getDoctorImageDetails(common.myInt(ViewState["DoctorId"]), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                //                                common.myInt(ViewState["EncounterId"]));
                ds = lis.getDoctorImageDetails(common.myInt(ViewState["DoctorID"]), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                              common.myInt(Session["EncounterId"]));
                if (ds.Tables[1].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[1].Rows[0] as DataRow;
                    if (common.myStr(dr["SignatureImage"]) != "")
                    {
                        SignedDate = common.myStr(dr["SignedDate"]);
                        FileName = common.myStr(dr["SignatureImageName"]);
                        ShowSignatureDate = " on " + SignedDate;
                        Education = common.myStr(dr["SignedProviderEducation"]);
                        img = dr["SignatureImage"];
                        UserName = common.myStr(ds.Tables[0].Rows[0]["EmployeeName"]);
                        Session["EmpName"] = common.myStr(ds.Tables[0].Rows[0]["EmployeeName"]).Trim();
                        SignatureDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["SignatureWithDateTime"].ToString());
                        UserDoctorId = common.myStr(ds.Tables[0].Rows[0]["DoctorId"]);

                        ShowSignatureDate = " on " + SignatureDate.ToString("dd/MM/yyyy hh:mm tt");
                        strm = new MemoryStream((byte[])img);
                        byte[] buffer = new byte[strm.Length];
                        int byteSeq = strm.Read(buffer, 0, common.myInt(strm.Length));
                        FileStream fs = new FileStream(Server.MapPath("~/PatientDocuments/DoctorImages/" + FileName), FileMode.Create, FileAccess.Write);

                        fs.Write(buffer, 0, byteSeq);
                        fs.Dispose();
                        //    RTF1.Snippets.Add("<img width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName.Trim() + "' />", "<img src='/PatientDocuments/DoctorImages/" + FileName + "' />");
                        //SignImage = "<img align='right' width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName + "' />";
                        strSingImagePath = Server.MapPath("~") + @"\PatientDocuments\DoctorImages\" + FileName;
                        SignImage = "<img align='right' width='100px' height='80px' src='" + strSingImagePath + "' />";
                        intCheckImage = 1;
                        strimgData = common.myStr(dr["ImageId"]);
                        SignNote = "Electronically signed by " + UserName.Trim() + " " + Education.Trim() + " " + ShowSignatureDate.Trim() + "</div>";
                    }
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (intCheckImage == 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0] as DataRow;
                        img = dr["SignatureImage"];
                        FileName = common.myStr(dr["ImageType"]);
                        UserName = common.myStr(dr["EmployeeName"]);
                        Session["EmpName"] = common.myStr(dr["EmployeeName"]).Trim();
                        SignatureDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["SignatureWithDateTime"].ToString());
                        UserDoctorId = common.myStr(dr["DoctorId"]);
                        ShowSignatureDate = " on " + SignatureDate.ToString("dd/MM/yyyy hh:mm tt");

                        if (common.myStr(dr["Education"]).Trim() != ""
                            && common.myStr(dr["Education"]).Trim() != "&nbsp;")
                        {
                            Education = common.myStr(dr["Education"]);
                        }
                        SignNote = "Electronically signed by " + UserName + " " + Education + " " + ShowSignatureDate + "</div>";

                        if (FileName != "")
                        {
                            //RTF1.Snippets.Add("<img width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName + "' />", "<img src='/PatientDocuments/DoctorImages/" + FileName + "' />");
                            //SignImage = "<img align='right' width='100px' height='80px' src='../PatientDocuments/DoctorImages/" + FileName + "' />";
                            strSingImagePath = Server.MapPath("~") + @"\PatientDocuments\DoctorImages\" + FileName;
                            SignImage = "<img align='right' width='100px' height='80px' src='" + strSingImagePath + "' />";
                            // SignImagetest= "<img align='right' width='100px' height='80px' src="''" 
                            strimgData = common.myStr(dr["ImageId"]);
                        }
                        else if (common.myStr(dr["SignatureImage"]) != "")
                        {
                            strm = new MemoryStream((byte[])img);
                            byte[] buffer = new byte[strm.Length];
                            int byteSeq = strm.Read(buffer, 0, common.myInt(strm.Length));
                            FileStream fs = new FileStream(Server.MapPath("~/PatientDocuments/DoctorImages/" + FileName), FileMode.Create, FileAccess.Write);

                            fs.Write(buffer, 0, byteSeq);
                            fs.Dispose();
                            //  RTF.Snippets.Add("<img width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName + "' />", "<img src='/PatientDocuments/DoctorImages/" + FileName + "' />");
                            //SignImage = "<img align='right' width='100px' height='80px' src='../PatientDocuments/DoctorImages/" + FileName + "' />";
                            strSingImagePath = Server.MapPath("~") + @"\PatientDocuments\DoctorImages\" + FileName;
                            SignImage = "<img align='right' width='100px' height='80px' src='" + strSingImagePath + "' />";
                            strimgData = common.myStr(dr["ImageId"]);
                        }
                    }
                }
                if (File.Exists(strSingImagePath))
                {
                    hdnDoctorImage.Value = DivStartTag + "<table align='right' border='0' cellpadding='0' cellspacing='0' style='font-size:10pt; font-family:Tahoma;'><tbody  align='right'><tr  align='right'><td align='right'>" + SignImage + "</td></tr></tbody></table><br/>";
                }
            }
        }
        catch (Exception ex)
        {
            //  lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //  lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
        finally
        {
            lis = null;
            ds.Dispose();
            strm = null;
            img = null;
            UserName = null;
            ShowSignatureDate = null;
            UserDoctorId = null;
            SignImage = null;
            SignNote = null;
            DivStartTag = null;
            SignedDate = null;
            strSQL = null;
            strSingImagePath = null;
        }
    }

    protected void generateReport(string visitDate)
    {
        bool IsPrintDoctorSignature = false;
        DataSet ds = new DataSet();
        clsIVF objivf = new clsIVF(sConString);
        try
        {
            if (common.myStr(Session["OPIP"]) == "I" && common.myBool(Session["EnablePrintCaseSheet"]) == false
            && common.myStr(Request.QueryString["callby"]) != "mrd")
            {
                if (common.myStr(Request.QueryString["OPIP"]) == "I")
                {
                    Alert.ShowAjaxMsg("You are not Authorise to print IP Clinical Data", Page);
                    return;
                }
            }
            if (common.myStr(Request.QueryString["OPIP"]) == "I" && common.myBool(Session["EnablePrintCaseSheet"]) == false
                && common.myStr(Request.QueryString["callby"]) == "mrd")
            {
                Alert.ShowAjaxMsg("You are not Authorise to print IP Clinical Data", Page);
                return;
            }
            hdnReportContent.Value = "";

            //if (common.myInt(ddlReport.SelectedValue) > 0)
            //{
            if (common.myInt(ViewState["reportid"]) > 0)
            {
                //ds = objivf.EditReportName(common.myInt(ddlReport.SelectedValue));
                ds = objivf.EditReportName(common.myInt(ViewState["reportid"]));

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        IsPrintDoctorSignature = common.myBool(ds.Tables[0].Rows[0]["IsPrintDoctorSignature"]);
                    }
                }
                //   Dashboard.PatientDashboardForDoctor emr_Dashboard_PatientDashboardForDoctor = new Dashboard.PatientDashboardForDoctor();
                hdnReportContent.Value = PrintReport(true, visitDate);

                //comment as follow-up appointment is check inside the printreport function --Saten
                StringBuilder sbD = new StringBuilder();
                sbD.Append("<table border='0' width='99%' cellpadding='0' cellspacing='0'>");
                //sbD.Append("<tr><td>Follow Up : </td></tr>");

                //string SignatureLabel = common.myStr(ddlReport.SelectedItem.Attributes["SignatureLabel"]).Trim();
                string SignatureLabel = string.Empty;
                if (IsPrintDoctorSignature.Equals(true))
                {
                    sbD.Append("<tr><td align='right'>" + PrintReportSignature(IsPrintDoctorSignature) + "</td></tr>");
                }
                else
                {
                    if (SignatureLabel == "")
                    {
                        sbD.Append("<tr><td align='right'><b>Consultant&nbsp;Signature</b></td></tr>");
                    }
                    else
                    {
                        sbD.Append("<tr><td align='right'><b>" + SignatureLabel + "</b></td></tr>");
                    }
                }
                //  sbD.Append("<tr><td align='right'> </td></tr>");
                sbD.Append("</table>");
                hdnReportContent.Value = "<div style='margin-left:3em; '>" + hdnReportContent.Value + sbD.ToString() + "</div>";
            }
            else
            {
                //btnPrintReport.Visible = false;
                return;
            }
            //btnPrintReport.Visible = true;
        }
        catch (Exception Ex)
        {
            //  lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //  lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    private string PrintReport(bool sign, string visitDate)
    {

        string strDisplayEnteredByInCaseSheet = common.myStr(Session["DisplayEnteredByInCaseSheet"]);
        if ((!common.myStr(Session["FacilityName"]).Contains("Venkateshwar")) || (!common.myBool(Session["IsReferralCase"]) && !common.myBool(Session["IsReferredCase"])))
        {
            Session["DisplayEnteredByInCaseSheet"] = string.Empty;
        }

        //   Session["DisplayEnteredByInCaseSheet"] = string.Empty;

        StringBuilder sb = new StringBuilder();
        StringBuilder sbTemplateStyle = new StringBuilder();
        StringBuilder TemplateString;
        DataSet ds = new DataSet();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        DataTable dtTemplate = new DataTable();
        DataView dvDataFilter = new DataView();
        DataTable dtEncounter = new DataTable();

        string Templinespace = "";
        BindNotes bnotes = new BindNotes(sConString);
        BaseC.DiagnosisDA fun = new BaseC.DiagnosisDA(sConString);
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        StringBuilder sbTemp = new StringBuilder();
        bool bAllergyDisplay = false;
        bool bPatientBookingDisplay = false;

        //sb.Append(getReportHeader(common.myInt(ddlReport.SelectedValue)));
        //   sb.Append(getReportHeader(common.myInt(ViewState["reportid"])));

        string getReportHeaderText = common.myStr(getReportHeader(common.myInt(ViewState["reportid"])));

        clsIVF objIVF = new clsIVF(sConString);
        string strPatientHeader = objIVF.getCustomizedPatientReportHeader(common.myInt(ViewState["headerid"]), string.Empty, (common.myInt(ViewState["vsTmpEncounterId"]) != 0 ? common.myInt(ViewState["vsTmpEncounterId"]) : common.myInt(Session["EncounterId"])));
        //string strPatientHeader = objIVF.getCustomizedPatientReportHeader(common.myInt(ViewState["headerid"]));
        if (common.myLen(strPatientHeader).Equals(0))
        {
            // sb.Append(getIVFPatient().ToString());
            Session["strPatientHeader"] = common.myStr(getReportHeaderText) + getIVFPatient().ToString();
        }
        else
        {
            Session["strPatientHeader"] = common.myStr(getReportHeaderText) + strPatientHeader;
            //sb.Append(strPatientHeader);
        }

        string sTemplateName = common.myStr("ALL") == "ALL" ? "" : common.myStr("ALL");

        DataSet dsTemplateData = new DataSet();
        BindCaseSheet BindCaseSheet = new BindCaseSheet(sConString);

        try
        {
            Session["InOPSummaryMedicationPRNConvertIfRequired"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                        common.myInt(Session["FacilityId"]), "InOPSummaryMedicationPRNConvertIfRequired", sConString);

            string EMRServicePrintSeperatedWithCommas = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                          common.myInt(Session["FacilityId"]), "EMRServicePrintSeperatedWithCommas", sConString);

            string DoctorId = fun.GetDoctorId(common.myInt(Session["HospitalLocationID"]), Convert.ToInt16(common.myInt(Session["UserID"])));
            dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));

            dsTemplateData = emr.getEMRPrintCaseSheetDate(common.myInt(Session["HospitalLocationId"]),
                                    common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]),
                                    (common.myInt(ViewState["vsTmpEncounterId"]) != 0 ? common.myInt(ViewState["vsTmpEncounterId"]) : common.myInt(Session["EncounterId"])),
                                    //  common.myInt(hdnEncounterId.Value),
                                    common.myDate(visitDate).ToString("yyyy/MM/dd"),
                                    common.myDate(visitDate).ToString("yyyy/MM/dd"),
                                    string.Empty, 0, string.Empty, false, common.myInt(ViewState["reportid"]));

            ViewState["vsTmpEncounterId"] = null;
            dvDataFilter = new DataView(dsTemplateData.Tables[21]);
            dtEncounter = dsTemplateData.Tables[22];
            for (int iEn = 0; iEn < dtEncounter.Rows.Count; iEn++)
            {
                if (dvDataFilter.ToTable().Rows.Count > 0)
                {
                    #region Template Wise
                    {
                        dtTemplate = dvDataFilter.ToTable();
                        TemplateString = new StringBuilder();
                        for (int i = 0; i < dtTemplate.Rows.Count; i++)
                        {
                            #region Admission Request
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "AdmissionRequest"
                                    && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S" && bPatientBookingDisplay == false)
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                string sBegin = "", sEnd = "";

                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();

                                if (common.myBool(dtTemplate.Rows[i]["IsBlankHeaderRequired"]))
                                {
                                    if (drTemplateStyle != null)
                                    {
                                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                                        {
                                            if (common.myLen(drTemplateStyle["StaticTemplateName"]) > 0)
                                            {
                                                TemplateString.Append(sBegin + common.myStr(drTemplateStyle["StaticTemplateName"]) + sEnd);
                                            }
                                            else
                                            {
                                                TemplateString.Append(sBegin + "ADMISSION ADVICE" + sEnd);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        TemplateString.Append(sBegin + "ADMISSION ADVICE" + sEnd);
                                    }

                                    if (chkIsNoFollowUpRequired.Checked)
                                    {
                                        TemplateString.Append("<br/>No follow-up required.<br/>");
                                    }
                                    else
                                    {
                                        TemplateString.Append("<br/>Adviced follow-up.<br/>");
                                    }
                                }
                                else
                                {
                                    #region Call Bind Patient Booking
                                    sbTemp = new StringBuilder();
                                    BindCaseSheet.BindPatientBooking(dsTemplateData.Tables[20], sbTemp, sbTemplateStyle, drTemplateStyle,
                                                       Page, common.myStr(dtTemplate.Rows[i]["PageId"]), "");
                                    if (sbTemp.ToString() != "")
                                    {
                                        TemplateString.Append(sbTemp + "<br/><br/>");
                                    }
                                    #endregion
                                }

                                bPatientBookingDisplay = true;
                            }
                            #endregion
                            #region Chief Complaints
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Chief Complaints"
                                 && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                string sBegin = "", sEnd = "";

                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;

                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();
                                //if ((common.myInt(0) == 0)
                                //     || (common.myInt(0) == 98))
                                //{

                                if (common.myBool(dtTemplate.Rows[i]["IsBlankHeaderRequired"]))
                                {
                                    if (drTemplateStyle != null)
                                    {
                                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                                        {
                                            if (common.myLen(drTemplateStyle["StaticTemplateName"]) > 0)
                                            {
                                                TemplateString.Append(sBegin + common.myStr(drTemplateStyle["StaticTemplateName"]) + sEnd);
                                            }
                                            else
                                            {
                                                TemplateString.Append(sBegin + "CHIEF COMPLAINTS" + sEnd);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        TemplateString.Append(sBegin + "CHIEF COMPLAINTS" + sEnd);
                                    }
                                    TemplateString.Append("<br/><br/>");
                                }
                                else
                                {
                                    #region Call Bind Problem data
                                    BindCaseSheet.BindProblemsHPI(dsTemplateData.Tables[0], common.myInt(ViewState["RegistrationId"]), common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle,
                                               Page, common.myStr(dtTemplate.Rows[i]["PageId"]), "", false);
                                    #endregion
                                    if (sbTemp.ToString() != "")
                                    {
                                        if (sbTemp.ToString().EndsWith("<br/>") || sbTemp.ToString().EndsWith("<br/></span>"))
                                        {
                                            TemplateString.Append(sbTemp);
                                        }
                                        else
                                        {
                                            TemplateString.Append(sbTemp + "<br/>");
                                        }
                                    }
                                }
                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}

                                drTemplateStyle = null;
                                Templinespace = "";
                            }
                            #endregion
                            #region Template History Type
                            if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
                               && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "D"
                               && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() == "HIS")
                            {
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["TemplateId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();
                                //if ((common.myInt(0) == 0)
                                //    || (common.myInt(0) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
                                //{
                                #region Assign Data and call History Type Dynamic Template
                                DataSet dsDymanicTemplateData = new DataSet();

                                DataView dvDyTable1 = new DataView(dsTemplateData.Tables[14]);
                                DataView dvDyTable2 = new DataView(dsTemplateData.Tables[15]);
                                DataView dvDyTable3 = new DataView(dsTemplateData.Tables[16]);
                                DataView dvDyTable4 = new DataView(dsTemplateData.Tables[17]);
                                DataView dvDyTable5 = new DataView(dsTemplateData.Tables[18]);
                                DataView dvDyTable6 = new DataView(dsTemplateData.Tables[19]);
                                DataView dvHistoryNonFreeTextHistoryTemplate = new DataView(dsTemplateData.Tables[14]);
                                DataTable dtDyTempTable = new DataTable();

                                dvDyTable1.ToTable().TableName = "TemplateSectionName";
                                dvDyTable6.ToTable().TableName = "TabularTemplateFieldStyle";
                                if (common.myInt(0) == 0)
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    dvDyTable4.Sort = "RecordId ASC";
                                }
                                else
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(0);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(0);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(0);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    dvDyTable4.Sort = "RecordId ASC";
                                }
                                string sSectionId = "0";
                                for (int iS = 0; iS < dvDyTable1.ToTable().Rows.Count; iS++)
                                {
                                    sSectionId = iS == 0 ? "'" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'"
                                        : sSectionId + ", '" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'";
                                }
                                dvDyTable2.ToTable().TableName = "FieldName";
                                dvDyTable2.RowFilter = "SectionId IN (" + sSectionId + ")";
                                dvDyTable6.RowFilter = "SectionId IN (" + sSectionId + ")";

                                dvDyTable3.ToTable().TableName = "PatientValue";
                                if (dvDyTable3.ToTable().Rows.Count > 0)
                                {
                                    dvDyTable3.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
                                }
                                if (dvDyTable4.ToTable().Rows.Count > 0)
                                {
                                    dvDyTable4.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
                                }

                                dsDymanicTemplateData.Tables.Add(dvDyTable1.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable2.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable3.ToTable());

                                if (dvDyTable4.ToTable().Rows.Count > 0)
                                {
                                    dsDymanicTemplateData.Tables.Add(dtDyTempTable);
                                }
                                else
                                {
                                    dsDymanicTemplateData.Tables.Add(dvDyTable4.ToTable());
                                }
                                dsDymanicTemplateData.Tables.Add(dvDyTable5.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable6.ToTable());
                                if (dsDymanicTemplateData.Tables[2].Rows.Count > 0 || dsDymanicTemplateData.Tables[3].Rows.Count > 0)
                                {
                                    bindData(dsDymanicTemplateData, common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp, "");
                                    if (sbTemp.Length > 20)
                                    {
                                        //dvHistoryNonFreeTextHistoryTemplate.RowFilter = "TemplateId=" + common.myStr(5896); 
                                        TemplateString.Append(sbTemp + "<br/>");

                                    }
                                }

                                sbTemp = null;
                                dsDymanicTemplateData.Dispose();
                                dvDyTable1.Dispose();
                                dvDyTable2.Dispose();
                                dvDyTable3.Dispose();
                                dvDyTable4.Dispose();
                                dvDyTable5.Dispose();
                                dvDyTable6.Dispose();
                                dtDyTempTable.Dispose();
                                sSectionId = "";
                                #endregion
                                //}

                                Templinespace = "";
                            }
                            #endregion
                            #region Allergy
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Allergies"
                                 && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S" && bAllergyDisplay == false)
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                drTemplateStyle = null;// = dv[0].Row;
                                string sBegin = "", sEnd = "";

                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();

                                //if ((common.myInt(0) == 0)
                                //     || (common.myInt(0) == 8))
                                //{
                                if (common.myBool(dtTemplate.Rows[i]["IsBlankHeaderRequired"]))
                                {
                                    if (drTemplateStyle != null)
                                    {
                                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                                        {
                                            if (common.myLen(drTemplateStyle["StaticTemplateName"]) > 0)
                                            {
                                                TemplateString.Append(sBegin + common.myStr(drTemplateStyle["StaticTemplateName"]) + sEnd);
                                            }
                                            else
                                            {
                                                TemplateString.Append(sBegin + "ALLERGIES" + sEnd);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        TemplateString.Append(sBegin + "ALLERGIES" + sEnd);
                                    }
                                    TemplateString.Append("<br/><br/>");
                                }
                                else
                                {
                                    #region Call Allergy template data
                                    BindCaseSheet.BindAllergies(dsTemplateData.Tables[1], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                           common.myStr(dtTemplate.Rows[i]["PageID"]), 0, "", false);
                                    #endregion
                                    if (sbTemp.ToString() != "")
                                    {
                                        TemplateString.Append("<br/>" + sbTemp + "<br/>");
                                        bAllergyDisplay = true;
                                    }
                                }
                                sbTemp = null;
                                sbTemplateStyle = null;
                            }
                            drTemplateStyle = null;
                            Templinespace = "";
                            //}
                            #endregion
                            #region Vital
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Vitals"
                                 && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                string sBegin = "", sEnd = "";

                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();

                                //if ((common.myInt(0) == 0)
                                //     || (common.myInt(0) == 14))
                                //{
                                if (common.myBool(dtTemplate.Rows[i]["IsBlankHeaderRequired"]))
                                {
                                    if (drTemplateStyle != null)
                                    {
                                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                                        {
                                            if (common.myLen(drTemplateStyle["StaticTemplateName"]) > 0)
                                            {
                                                TemplateString.Append(sBegin + common.myStr(drTemplateStyle["StaticTemplateName"]) + sEnd);
                                            }
                                            else
                                            {
                                                TemplateString.Append(sBegin + "VITALS" + sEnd);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        TemplateString.Append(sBegin + "VITALS" + sEnd);
                                    }
                                    TemplateString.Append("<br/>");
                                }
                                else
                                {
                                    #region Call Vital Template data
                                    //if (common.myStr(Session["FacilityName"]).Contains("Venkateshwar"))
                                    //{
                                    //    BindCaseSheet.BindVitalsVenkateshwar(dsTemplateData.Tables[11], sbTemp, sbTemplateStyle, drTemplateStyle,
                                    //                Page, common.myStr(dtTemplate.Rows[i]["PageId"]), 0, "", common.myInt(dtEncounter.Rows[iEn]["EncounterId"]));
                                    //}
                                    //else
                                    //{
                                    BindCaseSheet.BindVitals(dsTemplateData.Tables[11], sbTemp, sbTemplateStyle, drTemplateStyle,
                                               Page, common.myStr(dtTemplate.Rows[i]["PageId"]), 0, "", common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), false);
                                    //  }

                                    #endregion
                                    if (sbTemp.ToString() != "")
                                    {
                                        if (TemplateString.ToString() != string.Empty && !TemplateString.ToString().Contains("Allergies"))
                                        {
                                            TemplateString.Append("<br/>");
                                        }
                                        TemplateString.Append(sbTemp);
                                    }
                                }

                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}

                                drTemplateStyle = null;
                                Templinespace = "";

                            }
                            #endregion
                            #region All the Templates except Hitory and Plan of case
                            if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
                                && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "D"
                                && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "POC"
                                && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "HIS")
                            {
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["TemplateId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();
                                //if ((common.myInt(0) == 0)
                                //    || (common.myInt(0) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
                                //{
                                #region Assign Data and call all Dynamic Template Except Hostory and Plan of Care template

                                DataSet dsDymanicTemplateData = new DataSet();

                                DataView dvDyTable1 = new DataView(dsTemplateData.Tables[14]);
                                DataView dvDyTable2 = new DataView(dsTemplateData.Tables[15]);
                                DataView dvDyTable3 = new DataView(dsTemplateData.Tables[16]);
                                DataView dvDyTable4 = new DataView(dsTemplateData.Tables[17]);
                                DataView dvDyTable5 = new DataView(dsTemplateData.Tables[18]);
                                DataView dvDyTable6 = new DataView(dsTemplateData.Tables[19]);
                                DataTable dtDyTempTable = new DataTable();

                                dvDyTable1.ToTable().TableName = "TemplateSectionName";
                                dvDyTable6.ToTable().TableName = "TabularTemplateFieldStyle";
                                if (common.myInt(0) == 0)
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    dvDyTable4.Sort = "RecordId ASC";
                                }
                                else
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(0);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(0);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(0);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    dvDyTable4.Sort = "RecordId ASC";
                                }
                                string sSectionId = "0";
                                for (int iS = 0; iS < dvDyTable1.ToTable().Rows.Count; iS++)
                                {
                                    sSectionId = iS == 0 ? "'" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'"
                                        : sSectionId + ", '" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'";
                                }
                                dvDyTable2.ToTable().TableName = "FieldName";
                                dvDyTable2.RowFilter = "SectionId IN (" + sSectionId + ")";
                                dvDyTable6.RowFilter = "SectionId IN (" + sSectionId + ")";

                                dvDyTable3.ToTable().TableName = "PatientValue";
                                if (dvDyTable3.ToTable().Rows.Count > 0)
                                {
                                    dvDyTable3.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
                                }
                                if (dvDyTable4.ToTable().Rows.Count > 0)
                                {
                                    dvDyTable4.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
                                }

                                dsDymanicTemplateData.Tables.Add(dvDyTable1.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable2.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable3.ToTable());


                                if (dvDyTable4.ToTable().Rows.Count > 0)
                                {
                                    dsDymanicTemplateData.Tables.Add(dtDyTempTable);
                                }
                                else
                                {
                                    dsDymanicTemplateData.Tables.Add(dvDyTable4.ToTable());
                                }
                                dsDymanicTemplateData.Tables.Add(dvDyTable5.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable6.ToTable());
                                if (dsDymanicTemplateData.Tables[2].Rows.Count > 0 || dsDymanicTemplateData.Tables[3].Rows.Count > 0)
                                {
                                    bindData(dsDymanicTemplateData, common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp, "");
                                    if (sbTemp.Length > 20)
                                    {
                                        if (sbTemp.ToString().EndsWith("<br/>") || sbTemp.ToString().EndsWith("<br />"))
                                        {
                                            TemplateString.Append(sbTemp);
                                        }
                                        else
                                        {
                                            TemplateString.Append(sbTemp + "<br/>");
                                        }
                                    }
                                }
                                sbTemp = null;
                                dvDyTable1.Dispose();
                                dvDyTable2.Dispose();
                                dvDyTable3.Dispose();
                                dvDyTable4.Dispose();
                                dvDyTable5.Dispose();
                                dvDyTable6.Dispose();
                                dtDyTempTable.Dispose();
                                dsDymanicTemplateData.Dispose();
                                sSectionId = "";
                                #endregion
                                //}

                                Templinespace = "";
                            }
                            #endregion
                            #region Lab
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Lab Test Result"
                                && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                            {
                                string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                                strTemplateType = strTemplateType.Substring(0, 1);
                                //sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();

                                // bnotes.BindLabTestResult(common.myInt(Session["RegistrationId"]), HospitalId, EncounterId, UserId, DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myInt(Session["FacilityID"]), common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]));
                                TemplateString.Append(sbTemp);
                                drTemplateStyle = null;

                                sbTemp = null;
                                Templinespace = "";
                            }
                            #endregion
                            #region Diagnosis
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Diagnosis"
                               && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                string sBegin = "", sEnd = "";

                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();

                                //if ((common.myInt(0) == 0)
                                //     || (common.myInt(0) == 133))
                                //{
                                if (common.myBool(dtTemplate.Rows[i]["IsBlankHeaderRequired"]))
                                {
                                    if (drTemplateStyle != null)
                                    {
                                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                                        {
                                            if (common.myLen(drTemplateStyle["StaticTemplateName"]) > 0)
                                            {
                                                TemplateString.Append(sBegin + common.myStr(drTemplateStyle["StaticTemplateName"]) + sEnd);
                                            }
                                            else
                                            {
                                                TemplateString.Append(sBegin + "DIAGNOSIS" + sEnd);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        TemplateString.Append(sBegin + "DIAGNOSIS" + sEnd);
                                    }
                                    TemplateString.Append("<br/><br/>");
                                }
                                else
                                {
                                    #region Call Diagnosis Template Data
                                    BindCaseSheet.BindAssessments(dsTemplateData.Tables[4], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                           common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                           0, common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), "", common.myStr(ViewState["IsShowDiagnosisGroupHeading"]), false);
                                    #endregion
                                    if (sbTemp.ToString() != "")
                                        TemplateString.Append(sbTemp + "<br/>");
                                }
                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}
                                drTemplateStyle = null;
                                Templinespace = "";
                            }
                            #endregion
                            #region Provisional Diagnosis
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Provisional Diagnosis"
                                 && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                string sBegin = "", sEnd = "";
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();
                                //if ((common.myInt(0) == 0)
                                //     || (common.myInt(0) == 1085))
                                //{

                                if (common.myBool(dtTemplate.Rows[i]["IsBlankHeaderRequired"]))
                                {
                                    if (drTemplateStyle != null)
                                    {
                                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                                        {
                                            if (common.myLen(drTemplateStyle["StaticTemplateName"]) > 0)
                                            {
                                                TemplateString.Append(sBegin + common.myStr(drTemplateStyle["StaticTemplateName"]) + sEnd);
                                            }
                                            else
                                            {
                                                TemplateString.Append(sBegin + "PROVISIONAL DIAGNOSIS" + sEnd);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        TemplateString.Append(sBegin + "PROVISIONAL DIAGNOSIS" + sEnd);
                                    }
                                    TemplateString.Append("<br/><br/>");
                                }
                                else
                                {
                                    #region Call Provisional Diagnosis template data
                                    BindCaseSheet.BindPatientProvisionalDiagnosis(dsTemplateData.Tables[2],
                                           Request.QueryString["DoctorId"] != null ? Request.QueryString["DoctorId"].ToString() : DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                           common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                            0, common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), "", false);
                                    #endregion
                                    if (sbTemp.ToString() != "")
                                        TemplateString.Append(sbTemp + "<br/>");
                                }
                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}
                                drTemplateStyle = null;
                                Templinespace = "";
                            }
                            #endregion
                            #region Template Plan of Care
                            if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
                                 && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "D"
                                && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() == "POC")
                            {
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["TemplateId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();
                                //if ((common.myInt(0) == 0)
                                //    || (common.myInt(0) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
                                //{
                                #region Assign Data and call Dynamic Template Plan of Care
                                DataSet dsDymanicTemplateData = new DataSet();

                                DataView dvDyTable1 = new DataView(dsTemplateData.Tables[14]);
                                DataView dvDyTable2 = new DataView(dsTemplateData.Tables[15]);
                                DataView dvDyTable3 = new DataView(dsTemplateData.Tables[16]);
                                DataView dvDyTable4 = new DataView(dsTemplateData.Tables[17]);
                                DataView dvDyTable5 = new DataView(dsTemplateData.Tables[18]);
                                DataView dvDyTable6 = new DataView(dsTemplateData.Tables[19]);
                                DataTable dtDyTempTable = new DataTable();

                                dvDyTable1.ToTable().TableName = "TemplateSectionName";
                                dvDyTable6.ToTable().TableName = "TabularTemplateFieldStyle";
                                if (common.myInt(0) == 0)
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    dvDyTable4.Sort = "RecordId ASC";
                                }
                                else
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(0);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(0);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(0);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    dvDyTable4.Sort = "RecordId ASC";
                                }

                                string sSectionId = "0";
                                for (int iS = 0; iS < dvDyTable1.ToTable().Rows.Count; iS++)
                                {
                                    sSectionId = iS == 0 ? "'" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'"
                                        : sSectionId + ", '" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'";
                                }
                                dvDyTable2.ToTable().TableName = "FieldName";
                                dvDyTable2.RowFilter = "SectionId IN (" + sSectionId + ")";
                                dvDyTable6.RowFilter = "SectionId IN (" + sSectionId + ")";

                                dvDyTable3.ToTable().TableName = "PatientValue";
                                if (dvDyTable3.ToTable().Rows.Count > 0)
                                {
                                    dvDyTable3.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
                                }
                                if (dvDyTable4.ToTable().Rows.Count > 0)
                                {
                                    dvDyTable4.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
                                }

                                dsDymanicTemplateData.Tables.Add(dvDyTable1.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable2.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable3.ToTable());

                                if (dvDyTable4.ToTable().Rows.Count > 0)
                                {
                                    dsDymanicTemplateData.Tables.Add(dtDyTempTable);
                                }
                                else
                                {
                                    dsDymanicTemplateData.Tables.Add(dvDyTable4.ToTable());
                                }
                                dsDymanicTemplateData.Tables.Add(dvDyTable5.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable6.ToTable());
                                if (dsDymanicTemplateData.Tables[2].Rows.Count > 0 || dsDymanicTemplateData.Tables[3].Rows.Count > 0)
                                {
                                    bindData(dsDymanicTemplateData, common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp, "");
                                    if (sbTemp.Length > 20)
                                    {
                                        if (TemplateString.ToString().EndsWith("<br/>") && sbTemp.ToString().StartsWith("<br/>"))
                                        {
                                            sbTemp = sbTemp.Remove(0, 5);
                                            TemplateString.Append(sbTemp + "<br/>");
                                        }
                                        else
                                        {
                                            TemplateString.Append(sbTemp + "<br/>");
                                        }
                                    }
                                }
                                sbTemp = null;

                                dvDyTable1.Dispose();
                                dvDyTable2.Dispose();
                                dvDyTable3.Dispose();
                                dvDyTable4.Dispose();
                                dvDyTable5.Dispose();
                                dvDyTable6.Dispose();
                                dtDyTempTable.Dispose();
                                dsDymanicTemplateData.Dispose();
                                sSectionId = "";
                                #endregion
                                //}

                                Templinespace = "";
                            }
                            #endregion
                            #region Orders And Procedures
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Orders And Procedures"
                                 && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                string sBegin = "", sEnd = "";

                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();

                                //if ((common.myInt(0) == 0)
                                //     || (common.myInt(0) == 17))
                                //{

                                if (common.myBool(dtTemplate.Rows[i]["IsBlankHeaderRequired"]))
                                {
                                    if (drTemplateStyle != null)
                                    {
                                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                                        {
                                            if (common.myLen(drTemplateStyle["StaticTemplateName"]) > 0)
                                            {
                                                TemplateString.Append(sBegin + common.myStr(drTemplateStyle["StaticTemplateName"]) + sEnd);
                                            }
                                            else
                                            {
                                                TemplateString.Append(sBegin + "ORDERS" + sEnd);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        TemplateString.Append(sBegin + "ORDERS" + sEnd);
                                    }
                                    TemplateString.Append("<br/><br/>");
                                }
                                else
                                {
                                    #region Call Bind Order data
                                    BindCaseSheet.BindOrders(dsTemplateData.Tables[8], DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle,
                                               Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myInt(dtEncounter.Rows[iEn]["EncounterId"]),
                                               string.Empty, EMRServicePrintSeperatedWithCommas, false, false);
                                    #endregion

                                    if (sbTemp.ToString() != "")
                                    {
                                        if (sbTemp.ToString().EndsWith("<br/></span>") || sbTemp.ToString().EndsWith("<br/>"))
                                        {
                                            TemplateString.Append(sbTemp);
                                        }
                                        else
                                        {
                                            TemplateString.Append("<br/>" + sbTemp + "<br/>");
                                        }
                                    }
                                }

                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}

                                drTemplateStyle = null;
                                Templinespace = "";
                            }
                            #endregion
                            #region Prescription
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Prescription"
                                && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                string sBegin = "", sEnd = "";
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }

                                dv.Dispose();
                                sbTemp = new StringBuilder();

                                //if ((common.myInt(0) == 0)
                                //     || (common.myInt(0) == 153))
                                //{

                                if (common.myBool(dtTemplate.Rows[i]["IsBlankHeaderRequired"]))
                                {
                                    if (drTemplateStyle != null)
                                    {
                                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                                        {
                                            if (common.myLen(drTemplateStyle["StaticTemplateName"]) > 0)
                                            {
                                                TemplateString.Append(sBegin + common.myStr(drTemplateStyle["StaticTemplateName"]) + sEnd);
                                            }
                                            else
                                            {
                                                TemplateString.Append(sBegin + "MEDICATION" + sEnd);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        TemplateString.Append(sBegin + "MEDICATION" + sEnd);
                                    }
                                    TemplateString.Append("<br/><br/>");
                                }
                                else
                                {
                                    DataSet dsMedication = new DataSet();
                                    DataView dvTable1 = new DataView(dsTemplateData.Tables[10]);

                                    dvTable1.ToTable().TableName = "Item";
                                    dvTable1.RowFilter = "EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
                                    dsMedication.Tables.Add(dvTable1.ToTable());
                                    dvTable1.Dispose();

                                    #region Call Medication Template data
                                    BindCaseSheet.BindMedication(dsMedication, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(dtTemplate.Rows[i]["PageId"]),
                                                   common.myInt(Session["UserID"]).ToString(), "", 0, common.myStr(ViewState["PrescriptionPrintInTabularFormat"]), DpLanguageMasterPrint.SelectedValue, false);
                                    #endregion

                                    dsMedication.Dispose();
                                    if (sbTemp.ToString() != "")
                                        TemplateString.Append(sbTemp + "<br/>");

                                }
                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}

                                drTemplateStyle = null;
                                Templinespace = "";
                            }
                            #endregion
                            #region Non Drug Order
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Non Drug Order"
                                 && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                string sBegin = "", sEnd = "";

                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();
                                //if ((common.myInt(0) == 0)
                                //     || (common.myInt(0) == 1166))
                                //{
                                if (common.myBool(dtTemplate.Rows[i]["IsBlankHeaderRequired"]))
                                {
                                    if (drTemplateStyle != null)
                                    {
                                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                                        {
                                            if (common.myLen(drTemplateStyle["StaticTemplateName"]) > 0)
                                            {
                                                TemplateString.Append(sBegin + common.myStr(drTemplateStyle["StaticTemplateName"]) + sEnd);
                                            }
                                            else
                                            {
                                                TemplateString.Append(sBegin + "NON DRUG ORDER" + sEnd);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        TemplateString.Append(sBegin + "NON DRUG ORDER" + sEnd);
                                    }
                                    TemplateString.Append("<br/><br/>");
                                }
                                else
                                {
                                    #region Call Non Drug Order template data
                                    BindCaseSheet.BindNonDrugOrder(dsTemplateData.Tables[7], sbTemp, sbTemplateStyle, drTemplateStyle, Page, "0",
                                                  common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), "", false);
                                    #endregion
                                    if (sbTemp.ToString() != "")
                                        TemplateString.Append(sbTemp + "<br/>");
                                }
                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}
                                drTemplateStyle = null;
                                Templinespace = "";
                            }
                            #endregion
                            #region Diet Order
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Diet Order"
                                 && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                string sBegin = "", sEnd = "";

                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }

                                dv.Dispose();
                                sbTemp = new StringBuilder();

                                //if ((common.myInt(0) == 0)
                                //     || (common.myInt(0) == 1172))
                                //{
                                if (common.myBool(dtTemplate.Rows[i]["IsBlankHeaderRequired"]))
                                {
                                    if (drTemplateStyle != null)
                                    {
                                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                                        {
                                            if (common.myLen(drTemplateStyle["StaticTemplateName"]) > 0)
                                            {
                                                TemplateString.Append(sBegin + common.myStr(drTemplateStyle["StaticTemplateName"]) + sEnd);
                                            }
                                            else
                                            {
                                                TemplateString.Append(sBegin + "DIET ORDER" + sEnd);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        TemplateString.Append(sBegin + "DIET ORDER" + sEnd);
                                    }
                                    TemplateString.Append("<br/><br/>");
                                }
                                else
                                {
                                    #region Call Diet Order data
                                    BindCaseSheet.BindDietOrderInNote(dsTemplateData.Tables[9], sbTemp, sbTemplateStyle, drTemplateStyle, Page, "0", "",
                                    common.myInt(dtEncounter.Rows[iEn]["EncounterId"]));
                                    #endregion

                                    if (sbTemp.ToString() != "")
                                        TemplateString.Append(sbTemp + "<br/>");
                                }
                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}

                                drTemplateStyle = null;
                                Templinespace = "";
                            }
                            #endregion
                            #region Doctor Progress Note
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Doctor Progress Note"
                                 && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                string sBegin = "", sEnd = "";

                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();
                                //if ((common.myInt(0) == 0)
                                //     || (common.myInt(0) == 1013))
                                //{
                                if (common.myBool(dtTemplate.Rows[i]["IsBlankHeaderRequired"]))
                                {
                                    if (drTemplateStyle != null)
                                    {
                                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                                        {
                                            if (common.myLen(drTemplateStyle["StaticTemplateName"]) > 0)
                                            {
                                                TemplateString.Append(sBegin + common.myStr(drTemplateStyle["StaticTemplateName"]) + sEnd);
                                            }
                                            else
                                            {
                                                TemplateString.Append(sBegin + "DOCTOR PROGRESS NOTE" + sEnd);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        TemplateString.Append(sBegin + "DOCTOR PROGRESS NOTE" + sEnd);
                                    }
                                    TemplateString.Append("<br/><br/>");
                                }
                                else
                                {
                                    #region Call Doctor Progress Note template data
                                    BindCaseSheet.BindDoctorProgressNote(dsTemplateData.Tables[3], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                           common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]), "",
                                           common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), false);
                                    #endregion
                                    if (sbTemp.ToString() != "")
                                        TemplateString.Append(sbTemp + "<br/>");
                                }
                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}
                                Templinespace = "";
                            }
                            #endregion
                            #region Referal History
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Referral History"
                                 && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                string sBegin = "", sEnd = "";

                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();

                                //if ((common.myInt(0) == 0)
                                //     || (common.myInt(0) == 1081))
                                //{
                                if (common.myBool(dtTemplate.Rows[i]["IsBlankHeaderRequired"]))
                                {
                                    if (drTemplateStyle != null)
                                    {
                                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                                        {
                                            if (common.myLen(drTemplateStyle["StaticTemplateName"]) > 0)
                                            {
                                                TemplateString.Append(sBegin + common.myStr(drTemplateStyle["StaticTemplateName"]) + sEnd);
                                            }
                                            else
                                            {
                                                TemplateString.Append(sBegin + "REFERRAL HISTORY" + sEnd);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        TemplateString.Append(sBegin + "REFERRAL HISTORY" + sEnd);
                                    }
                                    TemplateString.Append("<br/><br/>");
                                }
                                else
                                {
                                    StringBuilder temp1 = new StringBuilder();
                                    #region Call Referral History Template Data
                                    BindCaseSheet.BindReferalHistory(dsTemplateData.Tables[5], sbTemp, sbTemplateStyle, drTemplateStyle, Page, "0",
                                        common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), "");
                                    #endregion
                                    if (sbTemp.ToString() != "")
                                        TemplateString.Append(sbTemp + "<br/>");
                                }
                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}
                                Templinespace = "";
                            }
                            #endregion
                            #region Current Medication
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Current Medication"
                                && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                string sBegin = "", sEnd = "";

                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    Templinespace = common.myStr(common.myInt(drTemplateStyle["TemplateSpaceNumber"]) - 1);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();

                                //if ((common.myInt(0) == 0)
                                //     || (common.myInt(0) == 10005))
                                //{
                                bnotes.BindMedication(common.myInt(ViewState["EncounterId"]), common.myInt(Session["HospitalLocationID"]), common.myInt(ViewState["RegistrationId"]), sbTemp, sbTemplateStyle, "C", drTemplateStyle,
                                                Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myInt(Session["UserID"]).ToString(),
                                                common.myDate(FromDate).ToString(),
                                                common.myDate(ToDate).ToString(), common.myStr(ViewState["OPIP"]), "");
                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}

                                drTemplateStyle = null;
                                Templinespace = "";
                            }
                            #endregion
                            #region Immunization
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Immunization"
                              && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();

                                //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //     || (common.myInt(ddlTemplatePatient.SelectedValue) == 113))
                                //{
                                BindCaseSheet.BindImmunization(dsTemplateData.Tables[13], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                            common.myStr(dtTemplate.Rows[i]["PageId"]), "", common.myInt(dtEncounter.Rows[iEn]["EncounterId"]));



                                if (sbTemp.ToString() != "")
                                    //TemplateString.Append(sbTemp + "<br/>");
                                    TemplateString.Append(sbTemp);
                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}

                                Templinespace = "";

                                StringBuilder sbImmunizationDueDate = new StringBuilder();
                                StringBuilder sbTemplateStyleImmunizationDueDate = new StringBuilder();
                                if (dsTemplateData.Tables.Count > 24)
                                {
                                    BindCaseSheet.BindImmunizationDueDate(dsTemplateData.Tables[24], sbImmunizationDueDate, sbTemplateStyleImmunizationDueDate, drTemplateStyle, Page,
                                           common.myStr(dtTemplate.Rows[i]["PageId"]), "", common.myInt(dtEncounter.Rows[iEn]["EncounterId"]));
                                    if (sbImmunizationDueDate.ToString() != "")
                                        TemplateString.Append(sbImmunizationDueDate + "<br/>");
                                    sbImmunizationDueDate = null;
                                    sbTemplateStyle = null;
                                }

                            }
                            #endregion
                            #region Daily Injection
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Daily Injections"
                                && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                string sBegin = "", sEnd = "";

                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }

                                dv.Dispose();
                                sbTemp = new StringBuilder();

                                //if ((common.myInt(0) == 0)
                                //     || (common.myInt(0) == 805))
                                //{
                                if (common.myBool(dtTemplate.Rows[i]["IsBlankHeaderRequired"]))
                                {
                                    if (drTemplateStyle != null)
                                    {
                                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                                        {
                                            if (common.myLen(drTemplateStyle["StaticTemplateName"]) > 0)
                                            {
                                                TemplateString.Append(sBegin + common.myStr(drTemplateStyle["StaticTemplateName"]) + sEnd);
                                            }
                                            else
                                            {
                                                TemplateString.Append(sBegin + "DAILY INJECTIONS" + sEnd);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        TemplateString.Append(sBegin + "DAILY INJECTIONS" + sEnd);
                                    }
                                    TemplateString.Append("<br/><br/>");
                                }
                                else
                                {
                                    BindCaseSheet.BindInjection(dsTemplateData.Tables[12], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                             common.myStr(dtTemplate.Rows[i]["PageId"]), "", common.myInt(dtEncounter.Rows[iEn]["EncounterId"]));

                                    TemplateString.Append(sbTemp + "<br/>");
                                }
                                sbTemp = null;
                                sbTemplateStyle = null;
                                //}
                                Templinespace = "";
                            }
                            #endregion
                            #region Follow-Up Appointment
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim().ToUpper().Equals("FOLLOW UP APPOINTMENT")
                                && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                string sBegin = "", sEnd = "";
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();

                                StringBuilder temp = new StringBuilder();
                                //if ((common.myInt(0) == 0)
                                //     || (common.myInt(0) == 919))
                                //{

                                if (common.myBool(dtTemplate.Rows[i]["IsBlankHeaderRequired"]))
                                {
                                    if (drTemplateStyle != null)
                                    {
                                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                                        {
                                            if (common.myLen(drTemplateStyle["StaticTemplateName"]) > 0)
                                            {
                                                TemplateString.Append(sBegin + common.myStr(drTemplateStyle["StaticTemplateName"]) + sEnd);
                                            }
                                            else
                                            {
                                                TemplateString.Append(sBegin + "Follow-up Appointment" + sEnd);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        TemplateString.Append(sBegin + "Follow-up Appointment" + sEnd);
                                    }
                                    TemplateString.Append("<br/><br/>");
                                }
                                else
                                {
                                    #region FollowUp Appointment
                                    BindCaseSheet.GetEncounterFollowUpAppointment(dsTemplateData.Tables[6],
                                           temp, sbTemplateStyle, drTemplateStyle, Page, "", common.myInt(dtEncounter.Rows[iEn]["EncounterId"]));
                                    #endregion

                                    TemplateString.Append(temp + "<br/>");
                                }

                                temp = null;
                                sbTemplateStyle = null;
                                //}
                                Templinespace = "";
                            }
                            #endregion
                        }
                        if (TemplateString.Length > 30)
                        {
                            //if (iEn == 0)
                            //{
                            //sb.Append("<span style='font-size:20px; font-family:Tohama;'>");
                            //sb.Append("<b><u>Initial Assessment</u></b><br/><br/>");
                            //sb.Append("</span>");
                            //}
                            sb.Append("<span style='" + String.Empty + "'>");
                            sb.Append(TemplateString);
                            sb.Append("</span><br/>");
                            //  sb.Append("</span>");
                            TemplateString = null;
                        }
                    }
                    #endregion
                }
            }
            Session["NoAllergyDisplay"] = null;
            if (sign == true)
            {
                //sb.Append("</span>");
                sb.Append(hdnDoctorImage.Value);
            }
            else if (sign == false)
            {
                if (RTF1Content != null)
                {
                    if (RTF1Content.Contains("dvDoctorImage") == true)
                    {
                        string signData = RTF1Content.Replace('"', '$');
                        string st = "<div id=$dvDoctorImage$>";
                        int start = signData.IndexOf(@st);
                        if (start > 0)
                        {
                            int End = signData.IndexOf("</div>", start);
                            StringBuilder sbte = new StringBuilder();
                            sbte.Append(signData.Substring(start, (End + 6) - start));
                            StringBuilder ne = new StringBuilder();
                            ne.Append(signData.Replace(sbte.ToString(), ""));
                            sb.Append(ne.Replace('$', '"').ToString());
                            sbte = null;
                            ne = null;
                            signData = "";
                            st = "";
                            start = 0;
                            End = 0;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //   lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //    lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
        finally
        {
            Session["DisplayEnteredByInCaseSheet"] = strDisplayEnteredByInCaseSheet;

            sbTemplateStyle = null;
            TemplateString = null;
            ds.Dispose();
            dsTemplateStyle.Dispose();
            dvDataFilter.Dispose();
            drTemplateStyle = null;
            dtTemplate.Dispose();
            Templinespace = "";
            bnotes = null;
            fun = null;
            emr = null;
            sbTemp = null;
            BindCaseSheet = null;
            dsTemplateData.Dispose();
        }
        return sb.ToString().Replace("&lt", "&lt;").Replace("&gt", "&gt;");
    }

    protected void MakeFontWithoutListStyle(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "Forecolor"]) != ""
            || common.myStr(item[typ + "FontSize"]) != ""
            || common.myStr(item[typ + "FontStyle"]) != "")
        {
            sBegin += "<span style='";
            if (common.myStr(item[typ + "FontSize"]) != "")
            {
                sBegin += " font-size:" + item[typ + "FontSize"] + ";";
            }
            else
            {
                sBegin += getDefaultFontSize();
            }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            {
                sBegin += " color: #" + item[typ + "Forecolor"] + ";";
            }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            {
                sBegin += GetFontFamily(typ, item);
            }
        }

        if (common.myStr(item[typ + "Bold"]) == "True")
        {
            sBegin += " font-weight: bold;";
        }
        if (common.myStr(item[typ + "Italic"]) == "True")
        {
            sBegin += " font-style: italic;";
        }
        if (common.myStr(item[typ + "Underline"]) == "True")
        {
            sBegin += " text-decoration: underline;";
        }
        aEnd.Add("</span>");
        for (int i = aEnd.Count - 1; i >= 0; i--)
        {
            sEnd += aEnd[i];
        }
        if (sBegin != "")
        {
            sBegin += " '>";
        }
    }

    public string getDefaultFontSize()
    {
        string sFontSize = "";
        string FieldValue = "";
        ClinicDefaults cd = new ClinicDefaults(Page);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FieldValue = cd.GetHospitalDefaults("DefaultFontSize", common.myInt(Session["HospitalLocationId"]).ToString());
        if (FieldValue != "")
        {
            sFontSize = fonts.GetFont("Size", FieldValue);
            if (sFontSize != "")
            {
                sFontSize = " font-size: " + sFontSize + ";";
            }
        }
        return sFontSize;
    }

    protected string GetFontFamily(string typ, DataRow item)
    {
        string FieldValue = "";
        string FontName = "";
        string sBegin = "";
        ClinicDefaults cd = new ClinicDefaults(Page);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FontName = fonts.GetFont("Name", common.myStr(item[typ + "FontStyle"]));
        ViewState["CurrentTemplateFontName"] = string.Empty;
        ViewState["CurrentTemplateFontName"] = FontName;
        if (FontName != "")
        {
            sBegin += " font-family: " + FontName + ";";

            //sBegin += " font-family: " + FontName + ", sans-serif;";
        }
        else
        {
            FieldValue = cd.GetHospitalDefaults("DefaultFontType", common.myInt(Session["HospitalLocationId"]).ToString());
            if (FieldValue != "")
            {
                FontName = fonts.GetFont("Name", FieldValue);
                if (FontName != "")
                {
                    sBegin += " font-family: " + FontName + ";";
                }
            }
        }

        return sBegin;
    }
    protected void bindData(DataSet dsDynamicTemplateData, string TemplateId, StringBuilder sb, string GroupingDate)
    {
        DataSet ds = new DataSet();
        DataSet dsAllNonTabularSectionDetails = new DataSet();
        DataSet dsAllTabularSectionDetails = new DataSet();
        DataSet dsAllFieldsDetails = new DataSet();

        DataTable dtFieldValue = new DataTable();
        DataTable dtEntry = new DataTable();
        DataTable dtFieldName = new DataTable();

        DataView dv = new DataView();
        DataView dv1 = new DataView();
        DataView dv2 = new DataView();

        DataRow dr3;

        StringBuilder objStrTmp = new StringBuilder();
        StringBuilder objStrSettings = new StringBuilder();
        StringBuilder str = new StringBuilder();
        string sEntryType = "V";
        string BeginList = string.Empty;
        string EndList = string.Empty;
        string BeginList2 = string.Empty;
        string BeginList3 = string.Empty;
        string EndList3 = string.Empty;
        string sBegin = string.Empty;
        string sEnd = string.Empty;

        int t = 0;
        int t2 = 0;
        int t3 = 0;
        int iRecordId = 0;
        DataView dvDyTable1 = new DataView();
        try
        {
            BeginList = string.Empty;
            EndList = string.Empty;
            BeginList2 = string.Empty;
            BeginList3 = string.Empty;
            EndList3 = string.Empty;

            t = 0;
            t2 = 0;
            t3 = 0;

            dvDyTable1 = new DataView(dsDynamicTemplateData.Tables[0]);
            DataView dvDyTable2 = new DataView(dsDynamicTemplateData.Tables[1]);
            DataView dvDyTable3 = new DataView(dsDynamicTemplateData.Tables[2]);

            dvDyTable1.ToTable().TableName = "TemplateSectionName";
            dvDyTable2.ToTable().TableName = "FieldName";
            dvDyTable3.ToTable().TableName = "PatientValue";
            dsAllNonTabularSectionDetails = new DataSet();
            if (dvDyTable3.ToTable().Rows.Count > 0)
            {
                dsAllNonTabularSectionDetails.Tables.Add(dvDyTable2.ToTable());
                dsAllNonTabularSectionDetails.Tables.Add(dvDyTable3.ToTable());
            }
            dvDyTable2.Dispose();
            dvDyTable3.Dispose();

            dsDynamicTemplateData.Dispose();
            #region Tabular
            DataView dvDyTable4 = new DataView(dsDynamicTemplateData.Tables[3]);
            DataView dvDyTable5 = new DataView(dsDynamicTemplateData.Tables[4]);
            DataView dvDyTable6 = new DataView(dsDynamicTemplateData.Tables[5]);

            dvDyTable4.ToTable().TableName = "TabularData";
            dvDyTable5.ToTable().TableName = "TabularColumnCount";
            dvDyTable6.ToTable().TableName = "TabularTemplateFieldStyle";

            dsAllTabularSectionDetails = new DataSet();
            if (dvDyTable4.ToTable().Rows.Count > 0)
            {
                dsAllTabularSectionDetails.Tables.Add(dvDyTable4.ToTable());
                dsAllTabularSectionDetails.Tables.Add(dvDyTable5.ToTable());
                dsAllTabularSectionDetails.Tables.Add(dvDyTable6.ToTable());
            }

            dvDyTable4.Dispose();
            dvDyTable5.Dispose();



            if (dsAllTabularSectionDetails.Tables.Count > 0 && dsAllTabularSectionDetails.Tables[1].Rows.Count > 0)
            {
                DataView dvTabular = new DataView(dvDyTable1.ToTable());
                dvTabular.RowFilter = "Tabular=1";
                if (dvTabular.ToTable().Rows.Count > 0)
                {
                    ds = new DataSet();
                    ds.Tables.Add(dvTabular.ToTable());//Section Name Table
                    dv = new DataView(dsAllTabularSectionDetails.Tables[0]);
                    dv.Sort = "RecordId DESC";
                    dtEntry = dv.ToTable(true, "RecordId");
                    iRecordId = 0;
                    dv.Dispose();
                    dvTabular.Dispose();
                    for (int it = 0; it < dtEntry.Rows.Count; it++)
                    {
                        if (common.myInt(dtEntry.Rows[it]["RecordId"]) != 0)
                        {
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                dv1 = new DataView(dsAllTabularSectionDetails.Tables[0]);
                                dv1.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
                                DataView dvFieldStyle = new DataView(dsAllTabularSectionDetails.Tables[2]);
                                dvFieldStyle.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
                                dtFieldName = dv1.ToTable();

                                if (dsAllTabularSectionDetails.Tables.Count > 1)
                                {
                                    dv2 = new DataView(dsAllTabularSectionDetails.Tables[1]);
                                    dv2.RowFilter = " SectionId=" + common.myStr(item["SectionId"]);
                                    dtFieldValue = dv2.ToTable();
                                    dv2.Dispose();
                                }

                                dsAllFieldsDetails = new DataSet();
                                dsAllFieldsDetails.Tables.Add(dtFieldName);
                                dsAllFieldsDetails.Tables.Add(dtFieldValue);

                                dsAllFieldsDetails.Tables.Add(dvDyTable6.ToTable());
                                dvDyTable6.Dispose();
                                dtFieldName.Dispose();
                                dtFieldValue.Dispose();
                                dv1.Dispose();

                                if (dsAllTabularSectionDetails.Tables[0].Rows.Count > 0)
                                {
                                    if (dsAllTabularSectionDetails.Tables.Count > 1)
                                    {
                                        if (dsAllTabularSectionDetails.Tables[0].Rows.Count > 0)
                                        {
                                            sBegin = string.Empty;
                                            sEnd = string.Empty;
                                            dr3 = dvFieldStyle.ToTable().Rows[0];
                                            getabulerFontSize("Fields", ref sBegin, ref sEnd, dr3);
                                            ViewState["iTemplateId"] = common.myInt(item["TemplateId"]);

                                            str = new StringBuilder();
                                            str.Append(CreateString(dsAllFieldsDetails, common.myInt(item["TemplateId"]), common.myStr(item["TemplateName"]),
                                                        common.myStr(item["Tabular"]), item["SectionId"].ToString(), common.myStr(item["EntryType"]),
                                                        common.myInt(dtEntry.Rows[it]["RecordId"]), GroupingDate, common.myBool(item["IsConfidential"])));

                                            str.Append("<br/> ");

                                            dr3 = null;
                                            dsAllTabularSectionDetails.Dispose();
                                            dsAllFieldsDetails.Dispose();

                                            if (common.myInt(ViewState["iPrevId"]).Equals(common.myInt(item["TemplateId"])))
                                            {
                                                if (iRecordId != common.myInt(dtEntry.Rows[it]["RecordId"]))
                                                {
                                                    if (sEntryType.Equals("M"))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                }
                                                if (t2.Equals(0))
                                                {
                                                    if (t3.Equals(0))//Template
                                                    {
                                                        t3 = 1;
                                                        if (common.myInt(item["SectionsListStyle"]).Equals(1))
                                                        {
                                                            BeginList3 = "<ul>";
                                                            EndList3 = "</ul>";
                                                        }
                                                        else if (common.myInt(item["SectionsListStyle"]).Equals(2))
                                                        {
                                                            BeginList3 = "<ol>";
                                                            EndList3 = "</ol>";
                                                        }
                                                    }
                                                }

                                                if (common.myStr(item["SectionsBold"]) != string.Empty
                                                    || common.myStr(item["SectionsItalic"]) != string.Empty
                                                    || common.myStr(item["SectionsUnderline"]) != string.Empty
                                                    || common.myStr(item["SectionsFontSize"]) != string.Empty
                                                    || common.myStr(item["SectionsForecolor"]) != string.Empty
                                                    || common.myStr(item["SectionsListStyle"]) != string.Empty)
                                                {
                                                    sBegin = string.Empty;
                                                    sEnd = string.Empty;
                                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))   //19June2010
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty))
                                                        {
                                                            objStrTmp.Append(BeginList3 + sBegin + common.myStr(item["SectionName"]) + sEnd); //    objStrTmp.Append("<br />" + BeginList3 + sBegin + item["SectionName"].ToString() + sEnd);
                                                        }
                                                    }
                                                    BeginList3 = string.Empty;
                                                }
                                                else
                                                {
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))    //19June
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty))
                                                        {
                                                            objStrTmp.Append(common.myStr(item["SectionName"])); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
                                                        }
                                                    }
                                                }

                                                if (!str.ToString().Trim().Equals(string.Empty))
                                                {
                                                    if (common.myInt(item["SectionsListStyle"]).Equals(3)
                                                        || common.myInt(item["TemplateListStyle"]).Equals(0))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                    objStrTmp.Append(str.ToString());
                                                }
                                            }
                                            else
                                            {
                                                if (t.Equals(0))
                                                {
                                                    t = 1;
                                                    if (common.myInt(item["TemplateListStyle"]).Equals(1))
                                                    {
                                                        BeginList = "<ul>"; EndList = "</ul>";
                                                    }
                                                    else if (common.myInt(item["TemplateListStyle"]).Equals(2))
                                                    {
                                                        BeginList = "<ol>"; EndList = "</ol>";
                                                    }
                                                }
                                                if (common.myStr(item["TemplateBold"]) != string.Empty
                                                    || common.myStr(item["TemplateItalic"]) != string.Empty
                                                    || common.myStr(item["TemplateUnderline"]) != string.Empty
                                                    || common.myStr(item["TemplateFontSize"]) != string.Empty
                                                    || common.myStr(item["TemplateForecolor"]) != string.Empty
                                                    || common.myStr(item["TemplateListStyle"]) != string.Empty)
                                                {
                                                    sBegin = string.Empty;
                                                    sEnd = string.Empty;
                                                    MakeFont("Template", ref sBegin, ref sEnd, item);
                                                    if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty))
                                                        {
                                                            if (sBegin.Contains("<br/>"))
                                                            {
                                                                sBegin = sBegin.Remove(0, 5);
                                                                objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd);
                                                            }
                                                            else
                                                            {
                                                                objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd);
                                                            }
                                                        }
                                                    }
                                                    if (sEntryType.Equals("M") && !str.ToString().Trim().Equals(string.Empty))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                    BeginList = string.Empty;
                                                }
                                                else
                                                {
                                                    if (common.myBool(item["TemplateDisplayTitle"]))
                                                    {
                                                        objStrTmp.Append(common.myStr(item["TemplateName"]));//Default Setting
                                                    }
                                                    if (sEntryType.Equals("M") && !str.ToString().Trim().Equals(string.Empty))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                }
                                                if (common.myInt(item["TemplateListStyle"]).Equals(3)
                                                    || common.myInt(item["TemplateListStyle"]).Equals(0))
                                                {
                                                    //  objStrTmp.Append("<br />");
                                                }

                                                objStrTmp.Append(EndList);
                                                if (t2.Equals(0))
                                                {
                                                    t2 = 1;
                                                    if (common.myInt(item["SectionsListStyle"]).Equals(1))
                                                    {
                                                        BeginList2 = "<ul>";
                                                        EndList3 = "</ul>";
                                                    }
                                                    else if (common.myInt(item["SectionsListStyle"]).Equals(2))
                                                    {
                                                        BeginList2 = "<ol>";
                                                        EndList3 = "</ol>";
                                                    }
                                                }
                                                if (common.myStr(item["SectionsBold"]) != string.Empty
                                                    || common.myStr(item["SectionsItalic"]) != string.Empty
                                                    || common.myStr(item["SectionsUnderline"]) != string.Empty
                                                    || common.myStr(item["SectionsFontSize"]) != string.Empty
                                                    || common.myStr(item["SectionsForecolor"]) != string.Empty
                                                    || common.myStr(item["SectionsListStyle"]) != string.Empty)
                                                {
                                                    sBegin = string.Empty;
                                                    sEnd = string.Empty;
                                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"])) // Comment On 19June2010 hit1
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                                        {
                                                            objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                                        }
                                                    }
                                                    BeginList2 = string.Empty;
                                                }
                                                else
                                                {
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))// Comment ON 19June2010
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                                        {
                                                            objStrTmp.Append(common.myStr(item["SectionName"])); //Comment On 19June2010
                                                        }
                                                    }
                                                }
                                                if (common.myInt(item["SectionsListStyle"]).Equals(3)
                                                    || common.myInt(item["SectionsListStyle"]).Equals(0))
                                                {
                                                    //objStrTmp.Append("<br />");
                                                }

                                                objStrTmp.Append(str.ToString());
                                            }
                                            if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                            {
                                                iRecordId = common.myInt(dtEntry.Rows[it]["RecordId"]);
                                                ViewState["iPrevId"] = common.myInt(item["TemplateId"]);
                                            }
                                        }
                                        str = null;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            #region Non Tabular
            if (dsAllNonTabularSectionDetails.Tables.Count > 0 && dsAllNonTabularSectionDetails.Tables[1].Rows.Count > 0)
            {
                DataView dvNonTabular = new DataView(dvDyTable1.ToTable());

                dvNonTabular.RowFilter = "Tabular=0";
                if (dvNonTabular.ToTable().Rows.Count > 0)
                {
                    ds = new DataSet();
                    ds.Tables.Add(dvNonTabular.ToTable());//Section Name Table

                    dv = new DataView(dsAllNonTabularSectionDetails.Tables[1]);

                    if (common.myStr(Session["FacilityName"]).Contains("Venkateshwar"))
                    {
                        dv.Sort = "RecordId ASC";
                    }
                    else
                    {
                        dv.Sort = "RecordId DESC";
                    }
                    //dv.Sort = "RecordId DESC";

                    dtEntry = dv.ToTable(true, "RecordId");
                    iRecordId = 0;
                    dv.Dispose();
                    dvNonTabular.Dispose();

                    for (int it = 0; it < dtEntry.Rows.Count; it++)
                    {
                        if (common.myInt(dtEntry.Rows[it]["RecordId"]) != 0)
                        {
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                dv1 = new DataView(dsAllNonTabularSectionDetails.Tables[0]);
                                dv1.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
                                dtFieldName = dv1.ToTable();

                                if (dsAllNonTabularSectionDetails.Tables.Count > 1)
                                {
                                    dv2 = new DataView(dsAllNonTabularSectionDetails.Tables[1]);
                                    dv2.RowFilter = "RecordId=" + common.myStr(dtEntry.Rows[it]["RecordId"]) + " AND SectionId=" + common.myStr(item["SectionId"]);
                                    dtFieldValue = dv2.ToTable();
                                    dv2.Dispose();
                                }

                                dsAllFieldsDetails = new DataSet();
                                dsAllFieldsDetails.Tables.Add(dtFieldName);
                                dsAllFieldsDetails.Tables.Add(dtFieldValue);

                                dtFieldName.Dispose();
                                dtFieldValue.Dispose();
                                dv1.Dispose();

                                if (dsAllNonTabularSectionDetails.Tables[0].Rows.Count > 0)
                                {
                                    if (dsAllNonTabularSectionDetails.Tables.Count > 1)
                                    {
                                        if (dsAllNonTabularSectionDetails.Tables[1].Rows.Count > 0)
                                        {
                                            sBegin = string.Empty;
                                            sEnd = string.Empty;

                                            dr3 = dsAllNonTabularSectionDetails.Tables[0].Rows[0];
                                            getabulerFontSize("Fields", ref sBegin, ref sEnd, dr3);
                                            ViewState["iTemplateId"] = common.myInt(item["TemplateId"]);

                                            str = new StringBuilder();
                                            //    if (!common.myInt(item["TemplateId"]).Equals(6048) && !flag)
                                            //   {
                                            //  str.Append("<br/>");
                                            // flag = true;
                                            //     }

                                            str.Append(CreateString(dsAllFieldsDetails, common.myInt(item["TemplateId"]), common.myStr(item["TemplateName"]),
                                                        common.myStr(item["Tabular"]), item["SectionId"].ToString(), common.myStr(item["EntryType"]),
                                                        common.myInt(dtEntry.Rows[it]["RecordId"]), GroupingDate, common.myBool(item["IsConfidential"])));



                                            //if(!common.myStr(item["TemplateName"].ToString()).Contains(common.myStr (ViewState["CheckTemplateName"])))
                                            //{
                                            //    str.Append("<br/> ");
                                            //}

                                            //ViewState["CheckTemplateName"] = common.myStr(item["TemplateName"]);
                                            //str.Append("<br/> ");

                                            //if (!common.myStr(ViewState["CheckCode"]).Equals(string.Empty))
                                            //{
                                            //    if (!common.myStr(item["Code"].ToString()).Equals(common.myStr(ViewState["CheckCode"])))
                                            //    {
                                            //          str.Append("<br/> ");
                                            //    }
                                            //}

                                            //ViewState["CheckCode"] = common.myStr(item["Code"]);

                                            //  str.Append("<br/> ");



                                            dr3 = null;
                                            dsAllNonTabularSectionDetails.Dispose();
                                            dsAllFieldsDetails.Dispose();
                                            string sBreak = common.myBool(item["IsConfidential"]) == true ? "<br/>" : "";
                                            if (common.myInt(ViewState["iPrevId"]).Equals(common.myInt(item["TemplateId"])))
                                            {
                                                if (iRecordId != common.myInt(dtEntry.Rows[it]["RecordId"]))
                                                {
                                                    if (sEntryType.Equals("M"))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                }
                                                if (t2.Equals(0))
                                                {
                                                    if (t3.Equals(0))//Template
                                                    {
                                                        t3 = 1;
                                                        if (common.myInt(item["SectionsListStyle"]).Equals(1))
                                                        {
                                                            BeginList3 = "<ul>";
                                                            EndList3 = "</ul>";
                                                        }
                                                        else if (common.myInt(item["SectionsListStyle"]).Equals(2))
                                                        {
                                                            BeginList3 = "<ol>";
                                                            EndList3 = "</ol>";
                                                        }
                                                    }
                                                }

                                                if (common.myStr(item["SectionsBold"]) != string.Empty
                                                    || common.myStr(item["SectionsItalic"]) != string.Empty
                                                    || common.myStr(item["SectionsUnderline"]) != string.Empty
                                                    || common.myStr(item["SectionsFontSize"]) != string.Empty
                                                    || common.myStr(item["SectionsForecolor"]) != string.Empty
                                                    || common.myStr(item["SectionsListStyle"]) != string.Empty)
                                                {
                                                    sBegin = string.Empty;
                                                    sEnd = string.Empty;
                                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))   //19June2010
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty))
                                                        {
                                                            objStrTmp.Append(BeginList3 + sBegin + common.myStr(item["SectionName"]) + sEnd); //    objStrTmp.Append("<br />" + BeginList3 + sBegin + item["SectionName"].ToString() + sEnd);
                                                        }
                                                    }
                                                    BeginList3 = string.Empty;
                                                }
                                                else
                                                {
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))    //19June
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty))
                                                        {
                                                            objStrTmp.Append(common.myStr(item["SectionName"])); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
                                                        }
                                                    }
                                                }

                                                if (!str.ToString().Trim().Equals(string.Empty))
                                                {
                                                    if (common.myInt(item["SectionsListStyle"]).Equals(3)
                                                        || common.myInt(item["TemplateListStyle"]).Equals(0))
                                                    {
                                                        ////// objStrTmp.Append("<br />"); //code commented  for Examination (SectonName and fieldname getting extra space)
                                                    }
                                                    objStrTmp.Append(str.ToString());
                                                }
                                            }
                                            else
                                            {
                                                if (t.Equals(0))
                                                {
                                                    t = 1;
                                                    if (common.myInt(item["TemplateListStyle"]).Equals(1))
                                                    {
                                                        BeginList = "<ul>"; EndList = "</ul>";
                                                    }
                                                    else if (common.myInt(item["TemplateListStyle"]).Equals(2))
                                                    {
                                                        BeginList = "<ol>"; EndList = "</ol>";
                                                    }
                                                }
                                                if (common.myStr(item["TemplateBold"]) != string.Empty
                                                    || common.myStr(item["TemplateItalic"]) != string.Empty
                                                    || common.myStr(item["TemplateUnderline"]) != string.Empty
                                                    || common.myStr(item["TemplateFontSize"]) != string.Empty
                                                    || common.myStr(item["TemplateForecolor"]) != string.Empty
                                                    || common.myStr(item["TemplateListStyle"]) != string.Empty)
                                                {
                                                    sBegin = string.Empty;
                                                    sEnd = string.Empty;
                                                    MakeFont("Template", ref sBegin, ref sEnd, item);
                                                    if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
                                                    {
                                                        if (sBegin.Contains("<br/>"))
                                                        {
                                                            // sBegin = sBegin.Remove(0, 5);

                                                            if (!common.myInt(item["TemplateId"]).Equals(6048) && !flag)
                                                            {
                                                                //  str.Append("<br/>");
                                                                // flag = true;
                                                                //  objStrTmp.Append("<br/>");
                                                                objStrTmp.Append(BeginList + sBegin + sBreak + common.myStr(item["TemplateName"]) + sEnd);
                                                                flag = true;
                                                            }
                                                            else
                                                            {
                                                                if (common.myInt(item["TemplateId"]).Equals(2))
                                                                {
                                                                    //objStrTmp.Append(BeginList + sBegin + sBreak + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                                                    objStrTmp.Append(BeginList + sBegin + sBreak + common.myStr(item["TemplateName"]) + sEnd);
                                                                }
                                                                else
                                                                {
                                                                    objStrTmp.Append(BeginList + sBegin + sBreak + common.myStr(item["TemplateName"]) + sEnd);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            objStrTmp.Append(BeginList + sBegin + sBreak + common.myStr(item["TemplateName"]) + sEnd);
                                                        }
                                                    }
                                                    if (sEntryType.Equals("M") && !str.ToString().Trim().Equals(string.Empty))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                    BeginList = string.Empty;
                                                }
                                                else
                                                {
                                                    if (common.myBool(item["TemplateDisplayTitle"]))
                                                    {
                                                        objStrTmp.Append(sBreak + common.myStr(item["TemplateName"]));//Default Setting
                                                    }
                                                    if (sEntryType.Equals("M") && !str.ToString().Trim().Equals(string.Empty))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                }
                                                if (common.myInt(item["TemplateListStyle"]).Equals(3)
                                                    || common.myInt(item["TemplateListStyle"]).Equals(0))
                                                {
                                                    //objStrTmp.Append("<br />");
                                                }

                                                objStrTmp.Append(EndList);
                                                if (t2.Equals(0))
                                                {
                                                    t2 = 1;
                                                    if (common.myInt(item["SectionsListStyle"]).Equals(1))
                                                    {
                                                        BeginList2 = "<ul>";
                                                        EndList3 = "</ul>";
                                                    }
                                                    else if (common.myInt(item["SectionsListStyle"]).Equals(2))
                                                    {
                                                        BeginList2 = "<ol>";
                                                        EndList3 = "</ol>";
                                                    }
                                                }
                                                if (common.myStr(item["SectionsBold"]) != string.Empty
                                                    || common.myStr(item["SectionsItalic"]) != string.Empty
                                                    || common.myStr(item["SectionsUnderline"]) != string.Empty
                                                    || common.myStr(item["SectionsFontSize"]) != string.Empty
                                                    || common.myStr(item["SectionsForecolor"]) != string.Empty
                                                    || common.myStr(item["SectionsListStyle"]) != string.Empty)
                                                {
                                                    sBegin = string.Empty;
                                                    sEnd = string.Empty;
                                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"])) // Comment On 19June2010 hit1
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                                        {

                                                            if (sBegin.StartsWith("<br/>"))
                                                            {
                                                                if (sBegin.Length > 5)
                                                                {

                                                                    //sBegin = sBegin.Remove(0, 5);
                                                                    //objStrTmp.Append(BeginList + sBegin + sBreak + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                                                    sBegin = sBegin.Substring(5, sBegin.Length - 5);
                                                                    objStrTmp.Append(BeginList2 + "<br/>" + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);

                                                            }

                                                            //if (sBegin.Contains("<br/>"))
                                                            //{
                                                            //    sBegin = sBegin.Remove(0, 5);
                                                            //    objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                                            //}
                                                            //else
                                                            //{

                                                            //    objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                                            //}

                                                        }
                                                    }
                                                    BeginList2 = string.Empty;
                                                }
                                                else
                                                {
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))// Comment ON 19June2010
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                                        {
                                                            objStrTmp.Append(common.myStr(item["SectionName"])); //Comment On 19June2010
                                                        }
                                                    }
                                                }
                                                if (common.myInt(item["SectionsListStyle"]).Equals(3)
                                                    || common.myInt(item["SectionsListStyle"]).Equals(0))
                                                {
                                                    //objStrTmp.Append("<br />");
                                                }

                                                objStrTmp.Append(str.ToString());
                                            }
                                            //if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                            //{
                                            iRecordId = common.myInt(dtEntry.Rows[it]["RecordId"]);
                                            ViewState["iPrevId"] = common.myInt(item["TemplateId"]);
                                            // }
                                        }
                                        str = null;
                                    }
                                }
                            }
                        }
                    }

                }
            }
            #endregion

            if (t2.Equals(1) && t3.Equals(1))
            {
                objStrTmp.Append(EndList3);
            }
            else
            {
                objStrTmp.Append(EndList);
            }
            if (GetPageProperty("1") != null)
            {
                objStrSettings.Append(objStrTmp.ToString());
                sb.Append(objStrSettings.ToString());
            }
            else
            {
                sb.Append(objStrTmp.ToString());
            }
        }
        catch (Exception ex)
        {
            // lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //  lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
        finally
        {
            ds.Dispose();
            dsAllNonTabularSectionDetails.Dispose();
            dsAllTabularSectionDetails.Dispose();
            dsAllFieldsDetails.Dispose();

            dtFieldValue.Dispose();
            dtEntry.Dispose();
            dtFieldName.Dispose();
            dvDyTable1.Dispose();
            dv.Dispose();
            dv1.Dispose();
            dv2.Dispose();

            dr3 = null;

            objStrTmp = null;
            objStrSettings = null;

            sEntryType = string.Empty;
            BeginList = string.Empty;
            EndList = string.Empty;
            BeginList2 = string.Empty;
            BeginList3 = string.Empty;
            EndList3 = string.Empty;
            sBegin = string.Empty;
            sEnd = string.Empty;
        }
    }
    protected void MakeFont(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "ListStyle"]) == "1")
        {
            sBegin += "<li>";
            //aEnd.Add("</li>");
        }
        else if (common.myStr(item[typ + "ListStyle"]) == "2")
        {
            sBegin += "<li>";
            // aEnd.Add("</li>");
        }
        else
        {
            if (common.myStr(ViewState["iTemplateId"]) != "163" && typ != "Fields")
            {
                sBegin += "<br/>";
            }
            else if (common.myStr(ViewState["iTemplateId"]) == "163" && typ == "Fields")
            {
                sBegin += "; ";
            }
            else
            {
                sBegin += "<br/>";
                //// //sBegin += "<br/>";
            }
        }

        if (common.myStr(item[typ + "Forecolor"]) != ""
            || common.myStr(item[typ + "FontSize"]) != ""
            || common.myStr(item[typ + "FontStyle"]) != "")
        {
            sBegin += "<span style='";
            if (common.myStr(item[typ + "FontSize"]) != "")
            {
                sBegin += " font-size:" + item[typ + "FontSize"] + ";";
            }
            else
            {
                sBegin += getDefaultFontSize();
            }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            {
                sBegin += " color: #" + item[typ + "Forecolor"] + ";";
            }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            {
                sBegin += GetFontFamily(typ, item);
            }
        }
        if (common.myStr(item[typ + "Bold"]) == "True")
        {
            sBegin += " font-weight: bold;";
        }
        if (common.myStr(item[typ + "Italic"]) == "True")
        {
            sBegin += " font-style: italic;";
        }
        if (common.myStr(item[typ + "Underline"]) == "True")
        {
            sBegin += " text-decoration: underline;";
        }

        aEnd.Add("</span>");
        for (int i = aEnd.Count - 1; i >= 0; i--)
        {
            sEnd += aEnd[i];
        }
        //sEnd += "<br/>";
        if (sBegin != "")
        {
            sBegin += " '>";
        }
    }
    protected DataSet GetPageProperty(string iFormId)
    {
        Hashtable hstInput = new Hashtable();
        if (common.myInt(Session["HospitalLocationID"]) > 0 && iFormId != "")
        {
            if (Cache[common.myInt(Session["HospitalLocationID"]).ToString() + "_" + iFormId + "_FormPageSettings"] == null)
            {
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hstInput.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
                hstInput.Add("@intFormId", iFormId);
                DataSet ds = null;//dl.FillDataSet(CommandType.StoredProcedure, "EMRGetFormPageSettingDetails", hstInput);
                //Cache.Insert(Session["HospitalLocationID"].ToString() + "_" + iFormId + "_FormPageSettings", ds, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
                return ds;
            }
            else
            {
                DataSet objDs = (DataSet)Cache[common.myInt(Session["HospitalLocationID"]).ToString() + "_" + iFormId + "_FormPageSettings"];
                return objDs;
            }
        }
        return null;
    }

    protected string CreateString(DataSet objDs, int iRootId, string iRootName, string TabularType,
         string sectionId, string EntryType, int RecordId, string GroupingDate, bool IsConfidential)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        StringBuilder objStr = new StringBuilder();
        DataView objDv = new DataView();
        DataTable objDt = new DataTable();
        DataSet dsMain = new DataSet();
        StringBuilder objStrTmp = new StringBuilder();
        DataSet dsTabulerTemplate = new DataSet();
        try
        {
            if (objDs != null)
            {
                if (IsConfidential == false)
                {
                    #region Tabular
                    if (bool.Parse(TabularType) == true)
                    {
                        DataView dvFilter = new DataView(objDs.Tables[0]);
                        if (objDs.Tables[0].Rows.Count > 0)
                        {
                            string sBegin = string.Empty;
                            string sEnd = string.Empty;
                            dvFilter.Sort = "RowNum ASC";
                            if (GroupingDate != "")
                            {
                                dvFilter.RowFilter = "ISNULL(RowCaptionName,'')='' AND RowNum > 2 AND RecordId<>0 AND GroupDate='" + GroupingDate + "' AND RecordId= " + RecordId;
                            }
                            else
                            {
                                dvFilter.RowFilter = "ISNULL(RowCaptionName,'')='' AND RowNum > 2 AND RecordId<>0 AND RecordId= " + RecordId;
                            }
                            DataTable dtNewTable = dvFilter.ToTable();
                            if (dtNewTable.Rows.Count > 0)
                            {
                                DataView dvRowCaption = new DataView(objDs.Tables[0]);
                                StringBuilder sbCation = new StringBuilder();
                                if (dvRowCaption.ToTable().Rows.Count > 0)
                                {
                                    dvRowCaption.RowFilter = "RowNum>0";
                                    DataTable dt = dvRowCaption.ToTable();
                                    dvRowCaption.Dispose();
                                    if (dt.Rows.Count > 0)
                                    {
                                        sbCation.Append("<br/><br/><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellspacing='3' ><tr align='center'>");
                                        DataView dvColumnCount = new DataView(objDs.Tables[1]);
                                        dvColumnCount.RowFilter = "SectionId=" + sectionId;

                                        int column = common.myInt(dvColumnCount.ToTable().Rows[0]["ColumnCount"]);
                                        int ColumnCount = 0;
                                        int count = 1;
                                        dvColumnCount.Dispose();
                                        for (int k = 0; k < column; k++)
                                        {
                                            sbCation.Append("<td>");
                                            sbCation.Append(common.myStr(dt.Rows[0]["Col" + count]));
                                            sbCation.Append("</td>");
                                            count++;
                                            ColumnCount++;
                                        }
                                        sbCation.Append("</tr>");

                                        DataView dvData = new DataView(dt);
                                        if (GroupingDate != "")
                                        {
                                            dvData.RowFilter = "RecordId=" + RecordId + " AND GroupDate='" + GroupingDate + "'";
                                        }
                                        else
                                        {
                                            dvData.RowFilter = "RecordId=" + RecordId;
                                        }

                                        for (int l = 1; l <= dvData.ToTable().Rows.Count; l++)
                                        {
                                            sbCation.Append("<tr>");
                                            for (int i = 1; i < ColumnCount + 1; i++)
                                            {
                                                if (dt.Rows[1]["Col" + i].ToString() == "IM")
                                                {
                                                    if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
                                                    {
                                                        sbCation.Append("<td align='center' ><img  id='dvImageType' runat='server'  alt='Image' width='30px' heigth='30px' src='" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "' /></td>");
                                                    }
                                                    else
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                    }
                                                }
                                                else
                                                {
                                                    if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "</td>");
                                                    }
                                                    else
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                    }
                                                }
                                            }
                                            sbCation.Append("</tr>");
                                        }
                                        dt.Dispose();
                                        dvData.Dispose();
                                    }
                                    sbCation.Append("</table>");
                                }
                                objStr.Append(sbCation);
                                dsTabulerTemplate.Dispose();
                                sbCation = null;

                            }
                            else
                            {
                                DataView dvRowCaption = new DataView(objDs.Tables[0]);
                                if (GroupingDate != "")
                                {
                                    dvRowCaption.RowFilter = "GroupDate='" + GroupingDate + "' AND RecordId= " + RecordId;
                                }
                                else
                                {
                                    dvRowCaption.RowFilter = "RecordId= " + RecordId;
                                }
                                if (dvRowCaption.ToTable().Rows.Count > 0)
                                {
                                    StringBuilder sbCation = new StringBuilder();
                                    dvRowCaption.RowFilter = "RowNum>0";
                                    DataTable dt = dvRowCaption.ToTable();
                                    // dvRowCaption.Dispose();
                                    if (dt.Rows.Count > 0)
                                    {
                                        sbCation.Append("<br/><br/><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'   cellspacing='3' ><tr align='center'>");
                                        DataView dvColumnCount = new DataView(objDs.Tables[1]);
                                        dvColumnCount.RowFilter = "SectionId=" + sectionId;

                                        int column = common.myInt(dvColumnCount.ToTable().Rows[0]["ColumnCount"]);
                                        int ColumnCount = 0;
                                        int count = 1;
                                        dvColumnCount.Dispose();

                                        for (int k = 0; k < column + 1; k++)
                                        {
                                            if (common.myStr(dt.Rows[0]["RowCaptionName"]) == ""
                                                && ColumnCount == 0)
                                            {
                                                sbCation.Append("<td>");
                                                sbCation.Append(" + ");
                                                sbCation.Append("</td>");
                                            }
                                            else
                                            {
                                                sbCation.Append("<td>");
                                                sbCation.Append(common.myStr(dt.Rows[0]["Col" + count]));
                                                sbCation.Append("</td>");
                                                count++;
                                            }
                                            ColumnCount++;
                                        }
                                        sbCation.Append("</tr>");

                                        DataView dvData = new DataView(dt);
                                        if (GroupingDate != "")
                                        {
                                            dvData.RowFilter = "RecordId=" + RecordId + " AND RowCaptionId>0 AND GroupDate='" + GroupingDate + "'";
                                        }
                                        else
                                        {
                                            dvData.RowFilter = "RecordId=" + RecordId + " AND RowCaptionId>0";
                                        }

                                        for (int l = 1; l <= dvData.ToTable().Rows.Count; l++)
                                        {
                                            sbCation.Append("<tr>");
                                            for (int i = 0; i < ColumnCount; i++)
                                            {
                                                if (i == 0)
                                                {
                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dvData.ToTable().Rows[l - 1]["RowCaptionName"]) + "</td>");
                                                }
                                                else
                                                {
                                                    if (dt.Rows[1]["Col" + i].ToString() == "IM")
                                                    {
                                                        if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
                                                        {
                                                            sbCation.Append("<td align='center' ><img id='dvImageType' runat='server'  alt='Image' width='30px' heigth='30px' src='" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "' /></td>");
                                                        }
                                                        else
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "</td>");
                                                        }
                                                        else
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                        }
                                                    }
                                                }
                                            }
                                            sbCation.Append("</tr>");
                                        }
                                        sbCation.Append("</table>");
                                        dvData.Dispose();
                                    }
                                    objStr.Append(sbCation);
                                    dt.Dispose();
                                    sbCation = null;
                                }
                            }
                        }
                    }
                    #endregion
                    #region Non Tabular
                    else // For Non Tabular Templates
                    {
                        string BeginList = "", EndList = "";
                        string sBegin = "", sEnd = "";
                        int t = 0;
                        string FieldId = "";
                        string sStaticTemplate = "";
                        string sEnterBy = "";
                        string sVisitDate = "";
                        foreach (DataRow item in objDs.Tables[0].Rows)
                        {
                            objDv = new DataView(objDs.Tables[1]);
                            objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                            objDt = objDv.ToTable();
                            if (t == 0)
                            {
                                t = 1;
                                if (common.myStr(item["FieldsListStyle"]) == "1")
                                {
                                    BeginList = "<ul>"; EndList = "</ul>";
                                }
                                else if (item["FieldsListStyle"].ToString() == "2")
                                {
                                    BeginList = "<ol>"; EndList = "</ol>";
                                }
                            }
                            if (common.myStr(item["FieldsBold"]) != ""
                                || common.myStr(item["FieldsItalic"]) != ""
                                || common.myStr(item["FieldsUnderline"]) != ""
                                || common.myStr(item["FieldsFontSize"]) != ""
                                || common.myStr(item["FieldsForecolor"]) != ""
                                || common.myStr(item["FieldsListStyle"]) != "")
                            {
                                //rafat1
                                if (objDt.Rows.Count > 0)
                                {
                                    sBegin = "";
                                    sEnd = "";

                                    MakeFont("Fields", ref sBegin, ref sEnd, item);
                                    if (common.myBool(item["DisplayTitle"]))
                                    {
                                        // if (EntryType != "M")
                                        // {


                                        ////if (sBegin.StartsWith("<br/>"))
                                        ////{
                                        ////    if (sBegin.Length > 5)
                                        ////    {
                                        ////        sBegin = sBegin.Substring(5, sBegin.Length - 5);

                                        ////    }
                                        ////}

                                        objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]).Replace("<", "&lt;").Replace(">", "&gt;"));
                                        //}
                                        //else
                                        //{
                                        //objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));
                                        //}
                                        // 28/08/2011
                                        //if (objDt.Rows.Count > 0)
                                        //{
                                        if (objStr.ToString() != "")
                                        {
                                            //  objStr.Append(sEnd + "</li>");
                                        }
                                        ViewState["sBegin"] = sBegin;
                                    }

                                    BeginList = "";
                                    sBegin = "";
                                    sEnd = "";

                                }

                            }
                            else
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    if (sStaticTemplate != "<br/><br/>")
                                    {
                                        objStr.Append(common.myStr(item["FieldName"]).Replace("<", "&lt;").Replace(">", "&gt;"));
                                    }
                                }
                            }
                            if (objDs.Tables.Count > 1)
                            {

                                objDv = new DataView(objDs.Tables[1]);
                                objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                                objDt = objDv.ToTable();
                                DataView dvFieldType = new DataView(objDs.Tables[0]);
                                dvFieldType.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                                DataTable dtFieldType = dvFieldType.ToTable("FieldType");
                                sBegin = "";
                                sEnd = "";

                                string sbeginTemp = string.Empty;
                                MakeFontWithoutBR("Fields", ref sBegin, ref sEnd, item);
                                // MakeFont("Fields", ref sBegin, ref sEnd, item);
                                for (int i = 0; i < objDv.ToTable().Rows.Count; i++)
                                {
                                    if (objDt.Rows.Count > 0)
                                    {

                                        sbeginTemp = common.myStr(ViewState["sBegin"]);
                                        if (sbeginTemp.StartsWith("<br/>"))
                                        {
                                            if (sbeginTemp.Length > 5)
                                            {
                                                sbeginTemp = sbeginTemp.Substring(0, 5);

                                                //objStrTmp.Append(sBegin + common.myStr(item["SectionName"]) + sEnd);
                                            }
                                        }



                                        string FType = common.myStr(dtFieldType.Rows[0]["FieldType"]);
                                        if (FType == "C")
                                        {
                                            FType = "C";
                                        }
                                        if (FType == "C" || FType == "D" || FType == "B" || FType == "R")
                                        {
                                            if (FType == "B")
                                            {
                                                //  objStr.Append(" : " + objDt.Rows[i]["TextValue"]);
                                                objStr.Append(" " + objDt.Rows[i]["TextValue"]);
                                            }
                                            else
                                            {
                                                //////BindDataValue(objDs, objDt, objStr, i, FType) //comeented by niraj , create and added below overloading methd
                                                BindDataValue(objDs, objDt, objStr, i, FType, sBegin, sEnd);
                                            }
                                        }
                                        else if (FType == "T" || FType == "M" || FType == "S" || FType == "W")
                                        {
                                            if (common.myStr(ViewState["iTemplateId"]) != "163")
                                            {
                                                if (i == 0)
                                                {
                                                    if (FType == "W")
                                                    {
                                                        objStr.Append(sBegin + " <br/> " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                                    }
                                                    else if (FType == "M")
                                                    {
                                                        //objStr.Append(sBegin + " : " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                                        if (common.myInt(objDt.Rows[i]["SectionId"]).Equals(2) && (!sEnd.ToString().Trim().Contains("<br/>")))
                                                        {
                                                            objStr.Append(sBegin + " " + common.myStr(objDt.Rows[i]["TextValue"]).Replace("\n\r", "<br/>").Replace("\n", "<br/>") + "<br/>" + sEnd);
                                                        }
                                                        else
                                                        {
                                                            objStr.Append(sBegin + " " + common.myStr(objDt.Rows[i]["TextValue"]).Replace("\n\r", "<br/>").Replace("\n", "<br/>") + sEnd);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //objStr.Append(sBegin + " : " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                                        objStr.Append(sBegin + " " + common.myStr(objDt.Rows[i]["TextValue"]).Replace("<", "&lt;").Replace(">", "&gt;") + sEnd);
                                                    }

                                                }
                                                else
                                                {
                                                    objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                                    //if (FType == "M" || FType == "W")
                                                    //{
                                                    //    objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                                    //}
                                                    //else
                                                    //{
                                                    //    objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);

                                                    //}

                                                }
                                            }
                                            else
                                            {
                                                if (i == 0)
                                                {
                                                    //objStr.Append(": " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                    objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                }
                                                else
                                                {
                                                    objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                }
                                            }
                                        }
                                        else if (FType == "L")
                                        {
                                            objStr.Append(BindStaticTemplates(common.myInt(objDt.Rows[0]["StaticTemplateId"]), common.myInt(objDt.Rows[0]["FieldId"])));
                                        }
                                        else if (FType == "IM")
                                        {
                                            objStr.Append(BindNonTabularImageTypeFieldValueTemplates(objDt));
                                        }
                                        if (common.myStr(item["FieldsListStyle"]) == "")
                                        {
                                            if (ViewState["iTemplateId"].ToString() != "163")
                                            {
                                                if (FType != "C")
                                                {

                                                    if (common.myStr(objDt.Rows[i]["StaticTemplateId"]) == null || common.myStr(objDt.Rows[i]["StaticTemplateId"]) == string.Empty || common.myInt(objDt.Rows[i]["StaticTemplateId"]) == 0)
                                                    {

                                                    }
                                                    else
                                                    {
                                                        objStr.Append("<br/>");
                                                    }

                                                }

                                            }
                                            else
                                            {
                                                if (FType != "C" && FType != "T")
                                                {
                                                    objStr.Append("<br/>");
                                                }
                                            }
                                        }





                                    }
                                    sEnterBy = objDt.Rows[i]["EnterBy"].ToString();
                                    sVisitDate = objDt.Rows[i]["VisitDateTime"].ToString();
                                    //if (EntryType == "M" && sEnterBy != "" && sVisitDate != "")
                                    //{
                                    //    objStr.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=' font-size:8pt;'>(Entered By: " + sEnterBy + " Date/Time: " + sVisitDate + ")</span>");
                                    //}
                                }
                                sBegin = "";
                                sEnd = "";
                                dvFieldType.Dispose();
                                dtFieldType.Dispose();

                                // Cmt 25/08/2011
                                //if (objDt.Rows.Count > 0)
                                //{
                                //    if (objStr.ToString() != "")
                                //        objStr.Append(sEnd + "</li>");
                                //}
                            }

                            //objStr.Append(" <span style=\" font-weight:bold; font-size:medium; color:Gray\">|</span> ");
                        }

                        if (objStr.ToString() != "")
                        {
                            objStr.Append(EndList);
                        }
                    }
                    #endregion
                }
                string sDisplayEnteredBy = common.myStr(Session["DisplayEnteredByInCaseSheet"]);
                if ((sDisplayEnteredBy == "Y") || (sDisplayEnteredBy == "N" && common.myStr(HttpContext.Current.Session["OPIP"]) == "I"))
                {
                    if ((objStr.ToString() != "" || IsConfidential == true) && bool.Parse(TabularType) == false)
                    {
                        DataView dvValues = new DataView(objDs.Tables[1]);
                        dvValues.RowFilter = "SectionId=" + common.myStr(sectionId);
                        if (dvValues.ToTable().Rows.Count > 0)
                        {
                            //objStr.Append("<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]) + "</span><br/>");

                            if (ViewState["CurrentTemplateFontName"] != null && !common.myStr(ViewState["CurrentTemplateFontName"]).Equals(string.Empty))
                            {
                                // objStr.Append("<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: " + common.myStr(ViewState["CurrentTemplateFontName"]) + "; '>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " Date/Time: " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]) + "</span><br/>");
                                objStr.Append("<b><br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: " + common.myStr(ViewState["CurrentTemplateFontName"]) + "; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]) + "</span></b>");

                            }
                            else
                            {
                                objStr.Append("<b><br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]) + "</span></b>");
                            }
                        }
                        dvValues.Dispose();
                    }
                    else
                    {
                        if ((objStr.ToString() != "" || IsConfidential == true) && bool.Parse(TabularType) == true)
                        {
                            DataView dvValues = new DataView(objDs.Tables[0]);
                            dvValues.RowFilter = "SectionId=" + common.myStr(sectionId) + " AND RecordId=" + RecordId + " AND IsData='D'";
                            if (dvValues.ToTable().Rows.Count > 0)
                            {
                                // objStr.Append("<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["EntryDate"]) + "</span><br/>");
                                if (ViewState["CurrentTemplateFontName"] != null && !common.myStr(ViewState["CurrentTemplateFontName"]).Equals(string.Empty))
                                {
                                    objStr.Append("<b><br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: " + common.myStr(ViewState["CurrentTemplateFontName"]) + "; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["EntryDate"]) + "</span></b>");
                                }
                                else
                                {
                                    objStr.Append("<b><br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["EntryDate"]) + "</span></b>");
                                }
                            }
                            dvValues.Dispose();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            //  lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //   lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objDv.Dispose();
            objDt.Dispose();
            dsMain.Dispose();
            objDs.Dispose();
            dsTabulerTemplate.Dispose();
        }
        return objStr.ToString();
    }

    private string BindStaticTemplates(int StaticTemplateId, int TemplateFieldId)
    {
        int RegId = 0;
        int EncounterId = 0;

        StringBuilder sb = new StringBuilder();
        StringBuilder sbStatic = new StringBuilder();
        StringBuilder sbTemplateStyle = new StringBuilder();
        DataSet dsTemplate = new DataSet();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        DataTable dtTemplate = new DataTable();
        Hashtable hst = new Hashtable();
        string Templinespace = "";
        BaseC.DiagnosisDA fun;

        BindNotes bnotes = new BindNotes(sConString);
        fun = new BaseC.DiagnosisDA(sConString);

        string DoctorId = fun.GetDoctorId(common.myInt(Session["HospitalLocationID"]), Convert.ToInt16(common.myInt(Session["UserID"])));

        dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));

        dsTemplate = bnotes.GetEMRTemplates(common.myInt(ViewState["EncounterId"]), common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EREncounterId"]).ToString());
        DataView dvFilterStaticTemplate = new DataView(dsTemplate.Tables[0]);
        dvFilterStaticTemplate.RowFilter = "PageId=" + StaticTemplateId;
        dtTemplate = dvFilterStaticTemplate.ToTable();

        sb.Append("<span style='" + string.Empty + "'>");

        if (dtTemplate.Rows.Count > 0)
        {
            if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Allergies"
                && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
            {
                string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                strTemplateType = strTemplateType.Substring(0, 1);
                sbTemplateStyle = new StringBuilder();
                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[0]["PageId"]);
                drTemplateStyle = null;// = dv[0].Row;
                if (dv.Count > 0)
                {
                    drTemplateStyle = dv[0].Row;
                    string sBegin = "", sEnd = "";
                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                }
                StringBuilder sbTemp = new StringBuilder();


                bnotes.BindAllergies(common.myInt(ViewState["RegistrationId"]), sbStatic, sbTemplateStyle, drTemplateStyle, Page, common.myInt(Session["HospitalLocationId"]).ToString(),
                            common.myInt(Session["UserID"]).ToString(), common.myStr(dtTemplate.Rows[0]["PageID"]),
                            common.myDate(FromDate).ToString(),
                            common.myDate(ToDate).ToString(), TemplateFieldId, "");

                // sb.Append(sbTemp + "<br/>");


                drTemplateStyle = null;
                Templinespace = "";
            }
            else if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Vitals"
                && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
            {
                string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                strTemplateType = strTemplateType.Substring(0, 1);
                sbTemplateStyle = new StringBuilder();
                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[0]["PageId"]);
                if (dv.Count > 0)
                {
                    drTemplateStyle = dv[0].Row;
                    string sBegin = "", sEnd = "";
                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                }
                StringBuilder sbTemp = new StringBuilder();


                bnotes.BindVitals(common.myInt(Session["HospitalLocationID"]).ToString(), common.myInt(ViewState["EncounterId"]), sbStatic, sbTemplateStyle, drTemplateStyle,
                                    Page, common.myStr(dtTemplate.Rows[0]["PageId"]), common.myInt(Session["UserID"]).ToString(),
                                    common.myDate(FromDate).ToString(),
                                    common.myDate(ToDate).ToString(), TemplateFieldId, common.myInt(ViewState["EREncounterId"]).ToString(), "");

                //sb.Append(sbTemp + "<br/>" + "<br/>");


                drTemplateStyle = null;
                Templinespace = "";

            }

            else if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Diagnosis"
                && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
            {
                string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                strTemplateType = strTemplateType.Substring(0, 1);
                sbTemplateStyle = new StringBuilder();
                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[0]["PageId"]);
                if (dv.Count > 0)
                {
                    drTemplateStyle = dv[0].Row;
                    string sBegin = "", sEnd = "";
                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                }
                StringBuilder sbTemp = new StringBuilder();


                bnotes.BindAssessments(common.myInt(ViewState["RegistrationId"]), common.myInt(Session["HospitalLocationID"]), common.myInt(ViewState["EncounterId"]), Convert.ToInt16(common.myInt(Session["UserID"])),
                            DoctorId, sbStatic, sbTemplateStyle, drTemplateStyle, Page,
                            common.myStr(dtTemplate.Rows[0]["PageId"]), common.myInt(Session["UserID"]).ToString(),
                            common.myDate(FromDate).ToString(),
                            common.myDate(ToDate).ToString(), TemplateFieldId, common.myInt(ViewState["EREncounterId"]).ToString(), "");

                //sb.Append(sbTemp + "<br/>");

                drTemplateStyle = null;
                Templinespace = "";
            }
            //sb.Append("</span>");
        }
        return "<br/>" + sbStatic.ToString();
    }

    private string BindNonTabularImageTypeFieldValueTemplates(DataTable dtIMTypeTemplate)
    {
        StringBuilder sb = new StringBuilder();
        if (dtIMTypeTemplate.Rows.Count > 0)
        {
            if (common.myStr(dtIMTypeTemplate.Rows[0]["ImagePath"]) != "")
            {
                sb.Append("<table id='dvImageType' runat='server'><tr><td>" + common.myStr(dtIMTypeTemplate.Rows[0]["TextValue"]) + "</td></tr><tr align='left'><td align='center'><img src='" + common.myStr(dtIMTypeTemplate.Rows[0]["ImagePath"]) + "' width='80px' height='80px' border='0' align='left' alt='Image' /></td></tr></table>");
            }
        }
        return sb.ToString();
    }

    protected void BindDataValue(DataSet objDs, DataTable objDt, StringBuilder objStr, int i, string FType, string sBegin, string sEnd)
    {
        if (i == 0)
        {
            //objStr.Append(sBegin + " : " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
            objStr.Append(sBegin + " " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
        }
        else
        {
            if (FType != "C")
            {
                objStr.Append(sBegin + ", " + sBegin + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
            }
            else
            {
                if (i == 0)
                {
                    objStr.Append(sBegin + " " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                }
                else if (i + 1 == objDs.Tables[1].Rows.Count)
                {
                    objStr.Append(sBegin + " and " + common.myStr(objDt.Rows[i]["TextValue"]) + "." + sEnd);
                }
                else
                {
                    objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                }
            }
        }
        //}
    }
    protected void MakeFontWithoutBR(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "ListStyle"]) == "1")
        {
            sBegin += "<li>";
            //aEnd.Add("</li>");
        }
        else if (common.myStr(item[typ + "ListStyle"]) == "2")
        {
            sBegin += "<li>";
            // aEnd.Add("</li>");
        }
        else
        {
            //if (common.myStr(ViewState["iTemplateId"]) != "163" && typ != "Fields")
            //{
            //    sBegin += "<br/>";
            //}
            //else if (common.myStr(ViewState["iTemplateId"]) == "163" && typ == "Fields")
            //{
            //    sBegin += "; ";
            //}
            //else
            //{
            //    sBegin += "<br/>";
            //}
        }

        if (common.myStr(item[typ + "Forecolor"]) != ""
            || common.myStr(item[typ + "FontSize"]) != ""
            || common.myStr(item[typ + "FontStyle"]) != "")
        {
            sBegin += "<span style='";
            if (common.myStr(item[typ + "FontSize"]) != "")
            {
                sBegin += " font-size:" + item[typ + "FontSize"] + ";";
            }
            else
            {
                sBegin += getDefaultFontSize();
            }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            {
                sBegin += " color: #" + item[typ + "Forecolor"] + ";";
            }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            {
                sBegin += GetFontFamily(typ, item);
            }
        }
        if (common.myStr(item[typ + "Bold"]) == "True")
        {
            sBegin += " font-weight: bold;";
        }
        if (common.myStr(item[typ + "Italic"]) == "True")
        {
            sBegin += " font-style: italic;";
        }
        if (common.myStr(item[typ + "Underline"]) == "True")
        {
            sBegin += " text-decoration: underline;";
        }

        aEnd.Add("</span>");
        for (int i = aEnd.Count - 1; i >= 0; i--)
        {
            sEnd += aEnd[i];
        }
        //sEnd += "<br/>";
        if (sBegin != "")
        {
            sBegin += " '>";
        }
    }

    protected string getabulerFontSize(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        sFontSize = string.Empty;

        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "Forecolor"]) != ""
            || common.myStr(item[typ + "FontSize"]) != ""
            || common.myStr(item[typ + "FontStyle"]) != "")
        {
            if (common.myStr(item[typ + "FontSize"]) != "")
            {
                sFontSize += " font-size:" + item[typ + "FontSize"] + ";";
            }
            else
            {
                sFontSize += getDefaultFontSize();
            }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            {
                sFontSize += " color: #" + item[typ + "Forecolor"] + ";";
            }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            {
                sFontSize += GetFontFamily(typ, item);
            };

            if (common.myStr(item[typ + "Bold"]) == "True")
            {
                sFontSize += " font-weight: bold;";
            }
            if (common.myStr(item[typ + "Italic"]) == "True")
            {
                sFontSize += " font-style: italic;";
            }
            if (common.myStr(item[typ + "Underline"]) == "True")
            {
                sFontSize += " text-decoration: underline;";
            }
        }

        return sFontSize;
    }
    private StringBuilder getReportHeader(int ReportId)
    {
        StringBuilder sb = new StringBuilder();
        try
        {

            DataSet ds = new DataSet();

            bool IsPrintHospitalHeader = false;
            clsIVF objivf = new clsIVF(sConString);
            ds = objivf.EditReportName(ReportId);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    IsPrintHospitalHeader = common.myBool(ds.Tables[0].Rows[0]["IsPrintHospitalHeader"]);
                }
            }

            ds = new DataSet();
            ds = objivf.getFacility(common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]));

            sb.Append("<div>");

            if (IsPrintHospitalHeader)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    sb.Append("<table border='0' width='100%' cellpadding='0' cellspacing='0' style='font-size:small'>");
                    for (int idx = 0; idx < ds.Tables[0].Rows.Count; idx++)
                    {
                        DataRow DR = ds.Tables[0].Rows[idx];

                        sb.Append("<tr>");

                        sb.Append("<td align ='center'>");
                        sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");
                        sb.Append("<tr>");
                        sb.Append("<td></td><td></td><td></td><td></td><td></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        //sb.Append("<td colspan='2' align ='right' valign='middle' style='font-size:9px'><img src='../Icons/SmallLogo.jpg' border='0' width='30px' height='25px'  alt='Image'/></td>");
                        sb.Append("<td colspan='2' align ='right' valign='middle' style='font-size:9px'><img src='" + Server.MapPath("/Icons/SmallLogo.jpg") + "' border='0' width='100px' height='25px'  alt='Image'/></td>");
                        sb.Append("<td colspan='3' align ='left' valign='middle' style='font-size:9px'><b>" + common.myStr(DR["FacilityName"]).Trim() + "</b></td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");
                        sb.Append("</td>");

                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td align ='center'  style='font-size:9px'>" + common.myStr(DR["Address1"]).Trim() + ", " + common.myStr(DR["Address2"]).Trim() + "</td>");
                        sb.Append("</tr>");

                        //sb.Append("<tr>");
                        //sb.Append("<td align ='center'>" + common.myStr(DR["CityName"]) + "(" + common.myStr(DR["PinNo"]) + "), " + common.myStr(DR["StateName"]) + "</td>");
                        //sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td align ='center'  style='font-size:9px'>Phone : " + common.myStr(DR["Phone"]) + " Fax : " + common.myStr(DR["Fax"]) + "</td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");
                }
            }
            else
            {
                //sb.Append("<br />");
                //sb.Append("<br />");
                //sb.Append("<br />");
            }

            // sb.Append("<br />");
            sb.Append("<table border='0' width='100%' style='text-align:center;'  cellpadding='2' cellspacing='3' ><tr>");
            //sb.Append("<td align=center><U>" + common.myStr(ddlReport.SelectedItem.Text) + "</U></td>");
            sb.Append("<td align=center><U> <b>" + common.myStr(ViewState["reportname"]) + " </b></U></td>");
            sb.Append("</tr></table></div>");

            return sb;
        }

        catch (Exception Ex)
        {
            objException.HandleException(Ex);
            sb = new StringBuilder();
            return sb;
        }
    }

    private StringBuilder getIVFPatient()
    {
        StringBuilder sb = new StringBuilder();
        try
        {

            DataSet ds = new DataSet();

            clsIVF objivf = new clsIVF(sConString);

            ds = objivf.getIVFPatient(common.myInt(ViewState["RegistrationId"]), 0);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataView DV = ds.Tables[0].Copy().DefaultView;
                DV.RowFilter = "RegistrationId=" + common.myInt(ViewState["RegistrationId"]);

                DataTable tbl = DV.ToTable();

                if (tbl.Rows.Count > 0)
                {
                    DataRow DR = tbl.Rows[0];

                    DataView DVSpouse = ds.Tables[0].Copy().DefaultView;
                    DVSpouse.RowFilter = "RegistrationId<>" + common.myInt(ViewState["RegistrationId"]);
                    DataTable tblSpouse = DVSpouse.ToTable();

                    sb.Append("<div><table border='0' width='100%' style='font-size:smaller; border-collapse:collapse;' cellpadding='2' cellspacing='3' ><tr valign='top'>");
                    //sb.Append("<td style='width: 72px;'>" + common.myStr(GetGlobalResourceObject("PRegistration", "ivfno")) + "</td><td>: " + common.myStr(Session["IVFNo"]) + "</td>");
                    sb.Append("<td style='width: 72px;'>" + common.myStr(GetGlobalResourceObject("PRegistration", "regno")) + "</td><td>: " + common.myStr(DR["RegistrationNo"]) + "</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr valign='top'>");
                    sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "PatientName")) + "</td><td>: " + common.myStr(DR["PatientName"]) + "</td>");
                    sb.Append("<td style='width: 109px;'>Age/Gender</td><td>: " + common.myStr(DR["Age/Gender"]) + "</td>");
                    sb.Append("</tr>");

                    if (tblSpouse.Rows.Count > 0)
                    {
                        sb.Append("<tr valign='top'>");
                        sb.Append("<td>Spouse</td><td>: " + common.myStr(tblSpouse.Rows[0]["PatientName"]) + "</td>");
                        sb.Append("<td>Spouse Age/Gender</td><td>: " + common.myStr(tblSpouse.Rows[0]["Age/Gender"]) + "</td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("<tr valign='top'>");
                    sb.Append("<td>Reg. Date</td><td>: " + common.myStr(DR["RegistrationDate"]) + "</td>");
                    sb.Append("<td>Occupation</td><td>: " + common.myStr(DR["Occupation"]) + "</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr valign='top'>");
                    sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "email")) + "</td><td>: " + common.myStr(DR["Email"]) + "</td>");
                    sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "phone")) + "</td><td>: " + common.myStr(DR["PhoneHome"]) + "</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr valign='top'>");
                    sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "Address")) + "</td><td>: " + common.myStr(DR["PatientAddress"]) + "</td>");
                    sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "mobile")) + "</td><td>: " + common.myStr(DR["MobileNo"]) + "</td>");
                    sb.Append("</tr>");

                    sb.Append("</table></div>");
                }

                sb.Append("<hr />");

            }
            return sb;
        }

        catch (Exception Ex)
        {
            objException.HandleException(Ex);
            sb = new StringBuilder();
            return sb;
        }
    }

    private string PrintReportSignature(bool Isdoctorsignature)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(getReportsSignature(Isdoctorsignature));
        return sb.ToString();
    }
    private StringBuilder getReportsSignature(bool IsPrintDoctorSignature)
    {
        StringBuilder sb = new StringBuilder();
        try
        {

            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            clsIVF objivf = new clsIVF(sConString);
            ds = new DataSet();
            dt = objivf.getDoctorSignatureDetails(common.myInt(ViewState["DoctorId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"])).Tables[0];
            if (IsPrintDoctorSignature)
            {
                sb.Append("<table border='0' width='100%' cellpadding='0' cellspacing='0' >");
                if (dt.Rows.Count > 0)
                {
                    if (common.myStr(dt.Rows[0]["DoctorName"]).Trim().Length > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align ='right'><b>" + common.myStr(dt.Rows[0]["DoctorName"]).Trim() + "</b></td>");
                        sb.Append("</tr>");
                    }
                    //if (common.myStr(dt.Rows[0]["Education"]).Trim().Length > 0)
                    //{
                    //    sb.Append("<tr>");
                    //    sb.Append("<td align ='right'><b>" + common.myStr(dt.Rows[0]["Education"]).Trim() + "</b></td>");
                    //    sb.Append("</tr>");
                    //}
                    //if (common.myStr(dt.Rows[0]["Designation"]).Trim().Length > 0)
                    //{
                    //    sb.Append("<tr>");
                    //    sb.Append("<td align ='right'><b>" + common.myStr(dt.Rows[0]["Designation"]).Trim() + "</b></td>");
                    //    sb.Append("</tr>");
                    //}
                    //if (common.myStr(dt.Rows[0]["UPIN"]).Trim().Length > 0)
                    //{
                    //    sb.Append("<tr>");

                    //    if (common.isNumeric(common.myStr(dt.Rows[0]["UPIN"]).Trim()))
                    //    {
                    //        sb.Append("<td align ='right'><b>Regn. No. : " + common.myStr(dt.Rows[0]["UPIN"]).Trim() + "</b></td>");
                    //    }
                    //    else
                    //    {
                    //        sb.Append("<td align ='right'><b>" + common.myStr(dt.Rows[0]["UPIN"]).Trim() + "</b></td>");
                    //    }

                    //    sb.Append("</tr>");
                    //}
                    if (common.myStr(dt.Rows[0]["SignatureLine1"]).Trim().Length > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align ='right'>" + common.myStr(dt.Rows[0]["SignatureLine1"]).Trim() + "</td>");
                        sb.Append("</tr>");
                    }
                    if (common.myStr(dt.Rows[0]["SignatureLine2"]).Trim().Length > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align ='right'>" + common.myStr(dt.Rows[0]["SignatureLine2"]).Trim() + "</td>");
                        sb.Append("</tr>");
                    }
                    if (common.myStr(dt.Rows[0]["SignatureLine3"]).Trim().Length > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align ='right'>" + common.myStr(dt.Rows[0]["SignatureLine3"]).Trim() + "</td>");
                        sb.Append("</tr>");
                    }
                    if (common.myStr(dt.Rows[0]["SignatureLine4"]).Trim().Length > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align ='right'>" + common.myStr(dt.Rows[0]["SignatureLine4"]).Trim() + "</td>");
                        sb.Append("</tr>");
                    }
                }
                sb.Append("</table>");
            }
            else
            {
                sb.Append("<br/>");
                sb.Append("<br/>");
                sb.Append("<br/>");
            }
            return sb;
        }

        catch (Exception Ex)
        {
            objException.HandleException(Ex);
            sb = new StringBuilder();
            return sb;
        }
    }
    protected void DpLanguageMasterPrint_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        clsIVF objI = new clsIVF(sConString);
        string UpdateStatus = objI.SetUpDateEMRToTranslateLanguage(common.myInt(Session["RegistrationID"]), DpLanguageMasterPrint.SelectedValue);

    }
    protected void btnPrintPDFNew_Click(object sender, EventArgs e)
    {

        try
        {
            GridViewRow row = (GridViewRow)(((ImageButton)sender).NamingContainer);

            hiddenencounterno = (HiddenField)row.FindControl("hdnEncounterNo");
            hiddenencounterid = (HiddenField)row.FindControl("hdnEncounterId");
            BindPatientDischargeSummary(hiddenencounterid.Value);

            Session["hdnSummaryID"] = hdnSummaryID.Value;

            hdnSummaryID.Value = common.myStr(Session["hdnSummaryID"]);
            Timer1.Enabled = false;

            if (common.myInt(hdnSummaryID.Value) == 0)
            {
                if (common.myStr(Request.QueryString["For"]) == "DthSum")
                {
                    Alert.ShowAjaxMsg("discharge summary Is Not Available !!", Page);
                }
                else
                {
                    Alert.ShowAjaxMsg("discharge summary Is Not Available !!", Page);
                }
                //   lblMessage.Text = "";
                Timer1.Enabled = true;

                return;
            }
            string strDthSum = "DISSUM";
            if (common.myStr(Request.QueryString["For"]) == "DthSum")
                strDthSum = common.myStr(Request.QueryString["For"]);




            //if (true)
            //{
            //    Response.Redirect("/EMRReports/PrintPdfNew.aspx?page=Ward&EncId=" + common.myStr(hiddenencounterid.Value) + "&RegId=" + common.myStr(hdnRegistrationID.Value) + "&ReportId=" + common.myStr(ViewState["SavedReportFormat"]) + "&For=" + strDthSum + "&Finalize=" + common.myStr(true) + "&ExportToWord=" + common.myBool(true), true);
            //}
            //else
            //{
            //RadWindow2.NavigateUrl = "/EMRReports/PrintPdfNew.aspx?page=Ward&EncId=" + common.myStr(ViewState["EncounterId"]) + "&RegId=" + common.myStr(ViewState["RegistrationId"]) + "&ReportId=" + common.myStr(ViewState["SavedReportFormat"]) + "&For=" + strDthSum + "&Finalize=" + common.myStr(hdnFinalizeDeFinalize.Value) + "&ExportToWord=" + common.myBool(chkExport.Checked);

            //RadWindow2.Height = 598;
            //RadWindow2.Width = 900;
            //RadWindow2.Top = 10;
            //RadWindow2.Left = 10;
            //RadWindow2.Modal = true;
            //RadWindow2.OnClientClose = "OnClientClose";
            //RadWindow2.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;

            //RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            //RadWindow2.VisibleStatusbar = false;

            // Response.Redirect("/ICM/PrintDischargeSummary.aspx?page=Ward&EncId=" + common.myStr(ViewState["EncounterId"]) + "&RegId=" + common.myStr(ViewState["RegistrationId"]) + "&ReportId=" + common.myStr(ViewState["SavedReportFormat"]) + "&For=" + strDthSum + "&Finalize=" + common.myStr(hdnFinalizeDeFinalize.Value) + "&ExportToWord=" + common.myBool(chkExport.Checked) + " ");

            var URLPath = "/ICM/PrintDischargeSummary.aspx?page=Ward&EncId=" + common.myStr(hiddenencounterid.Value) + "&RegId=" + common.myStr(hdnRegistrationID.Value) + "&ReportId=" + common.myStr(ViewState["SavedReportFormat"]) + "&For=" + strDthSum + "&Finalize=" + common.myStr(true) + "&ExportToWord=" + common.myBool(true) + " ";

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + URLPath.Replace("+", "%2B") + "','_blank')", true);

            //  }
        }
        catch (Exception ex)
        {
            // lblMessage.Text = ex.Message;
            //   lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    private void BindPatientDischargeSummary(string hiddenencounterid)
    {
        try
        {
            ObjIcm = new BaseC.ICM(sConString);

            ds = new DataSet();

            if (Request.QueryString["For"] != null)
            {
                if (common.myStr(Request.QueryString["For"]) == "DthSum")
                {

                    ds = ObjIcm.GetICMPatientSummaryDetails(common.myInt(Session["HospitalLocationID"]), common.myStr(hdnRegistrationID.Value)
                        , hiddenencounterid, 0, common.myInt(Session["FacilityId"]), "DE");
                }
                else
                {
                    ds = ObjIcm.GetICMPatientSummaryDetails(common.myInt(Session["HospitalLocationID"]), common.myStr(hdnRegistrationID.Value),
                                        hiddenencounterid, 0, common.myInt(Session["FacilityId"]), "DI");
                }
            }
            else
            {
                ds = ObjIcm.GetICMPatientSummaryDetails(common.myInt(Session["HospitalLocationID"]), common.myStr(hdnRegistrationID.Value),
                                    hiddenencounterid, 0, common.myInt(Session["FacilityId"]), "DI");
            }



            if (ds.Tables[0].Rows.Count > 0)
            {
                //dtpdate.SelectedDate = common.myDate(ds.Tables[0].Rows[0]["DOD"]);
                //if (!string.IsNullOrEmpty(common.myStr(common.myDate(ds.Tables[0].Rows[0]["DOD"]))))
                //    RadTimeFrom.SelectedDate = common.myDate(ds.Tables[0].Rows[0]["DOD"]);
                //if (RadTimeFrom.SelectedDate != null)
                //{
                //    string min = RadTimeFrom.SelectedDate.Value.Minute.ToString();
                //    min = min.Length == 1 ? "0" + min : min;
                //    RadComboBox1.SelectedIndex = RadComboBox1.Items.IndexOf(RadComboBox1.Items.FindItemByValue(min));
                //}

                DischargeSummaryCode.Value = common.myStr(ds.Tables[0].Rows[0]["PatientSummary"]);
                //  ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "$(function () { showDischargeSummary(); });", true);

                // RTF1.Content = common.myStr(ds.Tables[0].Rows[0]["PatientSummary"]);
                hdnTemplateData.Value = common.myStr(ds.Tables[0].Rows[0]["PatientSummary"]);
                hdnSummaryID.Value = common.myStr(ds.Tables[0].Rows[0]["SummaryID"]);

                hdnDoctorSignID.Value = common.myStr(ds.Tables[0].Rows[0]["SignDoctorID"]);
                hdnSignJuniorDoctorID.Value = common.myStr(ds.Tables[0].Rows[0]["SignJuniorDoctorID"]);

                hdnFinalize.Value = common.myStr(ds.Tables[0].Rows[0]["Finalize"]);
                hdnEncodedBy.Value = common.myStr(ds.Tables[0].Rows[0]["EncodedBy"]);
                //lblPreparedBy.Text = common.myStr(ds.Tables[0].Rows[0]["PreparedByName"]);
                //txtsynopsis.Text = common.myStr(ds.Tables[0].Rows[0]["Synopsis"]);

                //txtAddendum.Enabled = true;
                //txtAddendum.Text = common.myStr(ds.Tables[0].Rows[0]["Addendum"]);

                //chkIsMultiDepartmentCase.Checked = common.myBool(ds.Tables[0].Rows[0]["IsMultiDepartmentCase"]);
                //chkIsMultiDepartmentCase_OnCheckedChanged(null, null);
                //ddlDepartmentCase.SelectedIndex = ddlDepartmentCase.Items.IndexOf(ddlDepartmentCase.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["CaseId"])));

                ViewState["SavedReportFormat"] = common.myStr(ds.Tables[0].Rows[0]["FormatId"]).ToString();
                //ddlReportFormat.SelectedValue = common.myStr(ds.Tables[0].Rows[0]["FormatId"]).ToString();
                //ddlReportFormat.Enabled = false;
                //divFormat.Visible = false;
                //btnRefresh.Visible = false;

                //divFormatLabel.Visible = true;
                //lblReportFormat.Text = string.Empty;
                //lblReportFormat.Text = common.myStr(ds.Tables[0].Rows[0]["ReportName"]);




                //if (common.myInt(ds.Tables[0].Rows[0]["SignDoctorID"]) > 0)
                //    ddlDoctorSign.SelectedValue = common.myStr(ds.Tables[0].Rows[0]["SignDoctorID"]);
                //if (common.myInt(ds.Tables[0].Rows[0]["SignJuniorDoctorID"]) > 0)
                //    ddlJuniorDoctorSign.SelectedValue = common.myStr(ds.Tables[0].Rows[0]["SignJuniorDoctorID"]);


                //if (common.myBool(ds.Tables[0].Rows[0]["AllowEdit"]))
                //{
                //    ViewState["AllowEdit"] = true;
                //    //RTF1.EditModes = Telerik.Web.UI.EditModes.Design;

                //    //RTF1.Enabled = true;
                //    btnFinalize.Visible = true;
                //    btnSave.Visible = true;
                //    btnSave.Enabled = true;
                //}
                //else
                //{
                //    ViewState["AllowEdit"] = false;
                //    //  RTF1.EditModes = Telerik.Web.UI.EditModes.Preview;

                //    //RTF1.Enabled = false;
                //    btnSave.Enabled = false;
                //}

                //if (common.myBool(ViewState["AllowEdit"]))
                //{
                //    if (hdnFinalize.Value.Trim().Contains("1") && hdnDoctorSignID.Value != "0")
                //    {
                //        //  RTF1.EditModes = Telerik.Web.UI.EditModes.Preview;
                //        string script = "function f(){changeMode('" + "4" + "'); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                //        //btnSave.Enabled = false;
                //        dtpdate.Enabled = false;
                //        ddlDoctorSign.Enabled = false;
                //        ddlJuniorDoctorSign.Enabled = false;

                //        ddlReportFormat.Enabled = false;
                //        ddlTemplates.Enabled = false;
                //        chkDateWise.Enabled = false;
                //        spell1.Enabled = false;
                //        btnFinalize.Text = "Definalized (Ctrl+F2)";
                //        hdnFinalizeDeFinalize.Value = "DF";
                //    }
                //    else
                //    {
                //        string script = "function f(){changeMode('" + "1" + "'); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                //        btnFinalize.Text = "Finalized (Ctrl+F2)";
                //        hdnFinalizeDeFinalize.Value = "F";
                //        dtpdate.Enabled = true;
                //    }
                //}
                //else
                //{
                //string script = "function f(){changeMode('" + "4" + "'); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                //}
            }
            else
            {
                //  txtAddendum.Enabled = false;
                //dtpdate.SelectedDate = DateTime.Now;
                //  lblPreparedBy.Text = string.Empty;
                hdnSummaryID.Value = "0";
                //  ddlReportFormat_SelectedIndexChanged(null, null);

                ViewState["AllowEdit"] = true;
                //RTF1.EditModes = Telerik.Web.UI.EditModes.Design;
                string script = "function f(){changeMode('" + "1" + "'); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                //RTF1.Enabled = true;
                //btnFinalize.Visible = true;
                //btnSave.Visible = true;
                //btnSave.Enabled = true;

                //ddlReportFormat.Enabled = true;
                //divFormat.Visible = true;
                //btnRefresh.Visible = true;

                //divFormatLabel.Visible = false;
            }

            //if (ds.Tables.Count > 1)
            //{
            //    if (ds.Tables[1].Rows.Count > 0)
            //    {
            //        checkDepartments(ds.Tables[1]);
            //    }
            //}

            ds.Dispose();
            //if (common.myStr(Request.QueryString["For"]).Equals("DthSum"))  // Death summary date 
            //{
            //    //dtpdate.Enabled = false;
            //    dtpdate.Enabled = true;

            //    lblTime.Visible = true;
            //    RadTimeFrom.Visible = true;
            //    RadComboBox1.Visible = true;
            //    ltDateTime.Visible = true;
            //    if (dtpdate.SelectedDate == null)
            //    {
            //        dtpdate.SelectedDate = DateTime.Now;
            //    }
            //}
            //CheckDeFinalize();
        }
        catch (Exception ex)
        {
            // lblMessage.Text = ex.ToString();
            //  lblMessage.ForeColor = System.Drawing.Color.Red;

        }
    }
}