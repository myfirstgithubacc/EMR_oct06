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

public partial class EMR_Default : System.Web.UI.Page
{
    BaseC.Patient objbc1;
    private static string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //////   SetSectionVisibility();
            BindGrid();
            BindPatientData();
            LabVisitHistory();
            HideFields();
            //  PrescribedMedicineHistory();
        }
    }
    private void HideFields()
    {
    
        dvVitals.Columns[0].Visible = false;
        dvVitals.Columns[1].Visible = false;
        dvVitals.Columns[2].Visible = false;
        dvVitals.Columns[4].Visible = false;

        rpfCategory.Columns[0].Visible = false;



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
                        divDoctorsNotification.Visible = true;
                    }
                    DV.RowFilter = string.Empty;

                    DV.RowFilter = "SectionCode='COM'";
                    if (DV.ToTable().Rows.Count > 0)
                    {
                        divProblemList.Visible = true;
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
                        divSugarGraph.Visible = true;
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
                    divSugarGraph.Visible = true;
                    divViatalsGraph.Visible = true;
                    divRadiologyTest.Visible = true;
                    divMedicinesGiven.Visible = true;
                    divProblemList.Visible = true;
                    divDoctorsNotification.Visible = true;



                }
            }
            else
            {
                divLabTest.Visible = true;
                divLastPatientVisit.Visible = true;
                divSugarGraph.Visible = true;
                divViatalsGraph.Visible = true;
                divRadiologyTest.Visible = true;
                divMedicinesGiven.Visible = true;
                divProblemList.Visible = true;
                divDoctorsNotification.Visible = true;



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
                    if (dsOtherPatientDetail.Tables[1].Rows.Count > 0)
                    {
                        rptProblemList.DataSource = dsOtherPatientDetail.Tables[1];
                        rptProblemList.DataBind();

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


                    if (dsOtherPatientDetail.Tables[4].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsOtherPatientDetail.Tables[4].Rows.Count; i++)
                        {
                            if (i <= (common.myInt(dsOtherPatientDetail.Tables[4].Rows.Count) - 2))
                            {
                                lblAllergy.Text =common.myStr( lblAllergy.Text)+common.myStr ( dsOtherPatientDetail.Tables[4].Rows[i]["AllergyName"]) + " , ";
                            }
                            else if (i.Equals((common.myInt(dsOtherPatientDetail.Tables[4].Rows.Count) - 1)))
                            {
                                lblAllergy.Text = common.myStr(lblAllergy.Text) + common.myStr(dsOtherPatientDetail.Tables[4].Rows[i]["AllergyName"]);
                            }
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
                        if (common.myBool(dsOtherPatientDetail.Tables[5].Rows[0]["NoAllergies"]))
                        {
                            lblAllergy.Text = "No Allergy";
                        }
                    }


                    if (dsOtherPatientDetail.Tables[2].Rows.Count > 0)
                    {
                        rptPrescribedMedicine.DataSource = dsOtherPatientDetail.Tables[2];
                        rptPrescribedMedicine.DataBind();

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
                }
            }
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            dsOtherPatientDetail.Dispose();
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
                        rptPrescribedMedicine.DataSource = dsPrescribedMedicine;
                        rptPrescribedMedicine.DataBind();

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

                        lblUHID.Text = dr1["RegistrationNo"].ToString();
                        lblName.Text = dr1["PatientName"].ToString();
                        lblAge.Text = dr1["AgeGender"].ToString();
                        lblContact.Text = dr1["MobileNo"].ToString();



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

                                imgPatient.ImageUrl = "/PatientDocuments/PatientImages/" + dr1["ImageType"].ToString() + "";
                                //imgPatient.ImageUrl = "/PatientDocuments/PatientImages/" + UnquieID + ".jpg"";
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            imgPatient.ImageUrl = "~/imagesHTML/blank1.png";
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

    protected void ibtnBookAppointment_Click(object sender, ImageClickEventArgs e)
    {
        var ibtn = (ImageButton)sender;
        RepeaterItem item = (RepeaterItem)ibtn.NamingContainer;

        Label lblspecialised = (Label)item.FindControl("lblSpecialisedId");
        LinkButton lbtn = (LinkButton)item.FindControl("lbtnDoctorName");

        Session["DoctorId"] = lbtn.CommandArgument;
        Response.Redirect("BookAppointment.aspx?SpecializedId=" + lblspecialised.Text);


    }

    protected void ibtnLabResult_Click(object sender, ImageClickEventArgs e)
    {
        var ibtn = (ImageButton)sender;
        RepeaterItem item = (RepeaterItem)ibtn.NamingContainer;
        Label lblEncounterId = (Label)item.FindControl("lblEncounterid");
        Label lblDate = (Label)item.FindControl("lblDate");
        Label lblType = (Label)item.FindControl("lblType");
        Label lblDoctorId = (Label)item.FindControl("lblDoctorId");


        // Session["EncounterId"] = common.myStr(lblEncounterId.Text);
        //Session["DoctorID"] = common.myStr(lblDoctorId.Text);
        //Session["OPIP"] = common.myStr(lblType.Text);
        //Session["EncounterDate"] = lblDate.Text;

        if (common.myStr ( lblType.Text).Equals ("OP"))
        { 


        // RadWindowForNew.NavigateUrl = "~/Editor/WordProcessor.aspx?From=POPUP&DoctorId=" + common.myStr(Session["DoctorID"]) + "&OPIP=" + common.myStr(Session["OPIP"]) + "&EncounterDate=" + common.myDate(Session["EncounterDate"]).ToString("yyyy/MM/dd") + "&CloseButtonShow=No";
        RadWindowForNew.NavigateUrl = "~/Editor/WordProcessor.aspx?From=POPUP&DoctorId=" + common.myStr(lblDoctorId.Text) + "&OPIP=" + common.myStr(lblType.Text) + "&EncounterDate=" + common.myDate(lblDate.Text).ToString("yyyy/MM/dd") + "&CloseButtonShow=No";

        RadWindowForNew.Width = 1200;
        RadWindowForNew.Height = 630;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        //   RadWindowForNew.OnClientClose = "addTemplatesOnClientClose_All";
        RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
        RadWindowForNew.InitialBehavior = Telerik.Web.UI.WindowBehaviors.Maximize;
        }
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

    protected void rpfCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {


        if (e.Item.ItemType == ListItemType.Item)
        {


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




}