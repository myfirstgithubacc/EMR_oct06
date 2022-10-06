using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using Telerik.Web.UI;
using System.Text;
using System.Net;
using System.IO;
using System.Web;

public partial class EMR_DisplayImage : System.Web.UI.Page
{
    private string FileFolderx = ConfigurationManager.AppSettings["FileFoldertemp"];
    private string ftppath = ConfigurationManager.AppSettings["FTP"];
    private string Rootfolder = ConfigurationManager.AppSettings["FileFolder"];
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    Hashtable hstInput;
    string[] strdocType = { ".pdf", ".doc", ".xls", ".docx", ".xlsx", ".txt", ".zip" };
    bool dateStatus = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //if (Request.QueryString["id"] != null)
            //{
            bindImage();

            //}
            BindStatus();
            Checkstatus();
            BindProvider();
            CheckReviewedby();
            for (int i = 0; i < 60; i++)
            {
                if (i.ToString().Length == 1)
                {

                    RadComboBox1.Items.Add(new RadComboBoxItem("0" + i.ToString(), "0" + i.ToString()));
                }
                else
                {
                    RadComboBox1.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                }
            }
            int iMinute = DateTime.Now.Minute;
            RadComboBoxItem rcbItem = (RadComboBoxItem)RadComboBox1.Items.FindItemByText(iMinute.ToString());
            if (rcbItem != null)
            {
                rcbItem.Selected = true;
            }

        }
    }
    //protected void Page_PreRender(object sender, EventArgs e)
    //{
    //    if (dateStatus == true)
    //    {
    //        RadDateTimePicker1.SelectedDate = DateTime.Now;
    //    }
    //}
    protected void RadComboBox1_OnSelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(RadDateTimePicker1.SelectedDate.Value.ToString());
        sb.Remove(RadDateTimePicker1.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
        sb.Insert(RadDateTimePicker1.SelectedDate.Value.ToString().IndexOf(":") + 1, RadComboBox1.Text);
        RadDateTimePicker1.SelectedDate = Convert.ToDateTime(sb.ToString());
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            BaseC.Patient bc = new BaseC.Patient(sConString);
            Hashtable hsinput = new Hashtable();
            hsinput.Add("@chvRemarks", txtremarks.Text);
            hsinput.Add("@intReviewedProvider", ddlprovider.SelectedValue);
            hsinput.Add("@intStatusId", ddlreview.SelectedValue);
            object ob;
            string dt = "";
            if (RadDateTimePicker1.SelectedDate != null)
            {
                dt = bc.FormatDate(RadDateTimePicker1.SelectedDate.Value.ToString(), Session["OutputDateFormat"].ToString(), "dd/MM/yyyy");
                ob = dt;
            }
            else
            {
                ob = DBNull.Value;
            }
            hsinput.Add("@chrReviewDate", ob);
            hsinput.Add("@intDocumentId", Request.QueryString["id"].ToString());
            hsinput.Add("@intEncodedBy", Session["UserID"].ToString());
            objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspModifyPatientDocuments", hsinput);
            lblmsg.Text = "Record Updated";
            dateStatus = true;
        }
        catch (Exception ex)
        {

        }
    }


    private void bindImage()
    {
        if (Request.QueryString["regno"] != null && Request.QueryString["hosid"] != null)
        {
            DataSet objDr;
            string strSQL = "";
            hstInput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            BaseC.Patient bc = new BaseC.Patient(sConString);
            hstInput.Add("Id", Request.QueryString["id"].ToString());
            strSQL = "select Id, case Type when '.doc' then '/Images/WordThumbnail.png' when '.docx' then '/Images/WordThumbnail.png'";
            strSQL += " when '.xls' then '/Images/Excel.png' when '.xlsx' then '/Images/XLSX.png' when '.pdf' then '/Images/pdf.jpg' when '.zip' then 'winzip-icon.jpg'";
            strSQL += " else ImagePath end as ImagePath, ImageName,CompressedFileName, isnull(Description, '') as Description, Isnull(Remarks, '') as Remarks ,";
            strSQL += " Type, ROW_NUMBER() over( order by id ) as slno, ReviewDate from EMRPatientDocuments";
            strSQL += " where ID = @Id  AND Active=1";

            objDr = objDl.FillDataSet(CommandType.Text, strSQL, hstInput);
            if (objDr.Tables[0].Rows.Count > 0)
            {
                imgdetail.ImageUrl = objDr.Tables[0].Rows[0].ItemArray[1].ToString();
                lnkDownLoad.CommandName = objDr.Tables[0].Rows[0]["ImageName"].ToString();
                displayimage();
                txtremarks.Text = objDr.Tables[0].Rows[0].ItemArray[5].ToString();
                if (objDr.Tables[0].Rows[0].ItemArray[8].ToString().Trim() != "")
                {
                    RadDateTimePicker1.SelectedDate = Convert.ToDateTime(objDr.Tables[0].Rows[0].ItemArray[8].ToString());
                }
                else
                {
                    RadDateTimePicker1.SelectedDate = DateTime.Now;
                }
            }
        }
    }
    private bool CheckDocument()
    {
        if (Request.QueryString["regno"] != null && Request.QueryString["hosid"] != null)
        {
            if (Request.QueryString["id"] != null)
            {
                hstInput = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hstInput.Add("Id", Request.QueryString["id"].ToString());

                string Type = (string)objDl.ExecuteScalar(CommandType.Text, "select Type from EMRPatientDocuments where id = @Id ", hstInput);
                if (strdocType.Contains<string>(Type))
                {
                    return true;
                }
                else
                    return false;
            }
        }
        return false;
    }
    void displayimage()
    {

        try
        {
            frm.Attributes.Add("src", "viewDocs.aspx");
            if (lnkDownLoad.CommandName.Contains("pdf"))
            {
                // Response.ContentType = "Application/pdf";
            }
            else if (lnkDownLoad.CommandName.Contains("jpg") || lnkDownLoad.CommandName.Contains("png") || lnkDownLoad.CommandName.Contains("bmp"))
            {
                //Response.ContentType = "image/png";
            }
            else
            {
                frm.Attributes.Add("src", "viewDocs.aspx");
                return;
            }


            string Splitter = ConfigurationManager.AppSettings["Split"];
            if (common.myLen(Splitter).Equals(0))
            {
                Splitter = "!";
            }

            var csplitter = Splitter.ToCharArray();


            //Create FTP Request.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftppath.Split(csplitter)[0].ToString() + Rootfolder + Request.QueryString["hosid"].ToString() + "/" + Request.QueryString["regno"] + "/" + lnkDownLoad.CommandName);
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            //Enter FTP Server credentials.
            request.Credentials = new NetworkCredential(ftppath.Split(csplitter)[1].ToString(), ftppath.Split(csplitter)[2].ToString());
            request.UsePassive = true;
            request.UseBinary = true;
            request.EnableSsl = false;

            //Fetch the Response and read it into a MemoryStream object.
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            using (MemoryStream stream = new MemoryStream())
            {

                //Download the File.
                response.GetResponseStream().CopyTo(stream);
                //Response.AddHeader("content-disposition", "attachment;filename=" + lnkDownLoad.CommandName);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                string outfile = "";

                //-------------------------------------------------------------------------------------------add by shaivee
                FileFolderx = "PatientDocuments/";
                string docPath = Server.MapPath("~/PatientDocuments/" + lnkDownLoad.CommandName);
                common.ByteArrayToFile(docPath, stream.ToArray(), out outfile);
                //--------------------------------------------------------------------------------------------end by shaivee

                //common.ByteArrayToFile(FileFolderx + lnkDownLoad.CommandName, stream.ToArray(), out outfile);


                Session["FileName"] = outfile;
                frm.Attributes.Add("src", "viewDocs.aspx");
                //Response.BinaryWrite(stream.ToArray());
                //if (lnkDownLoad.CommandName.Contains("pdf"))
                //{
                //    Response.ContentType = "Application/pdf";
                //}
                //else if (lnkDownLoad.CommandName.Contains("jpg") || lnkDownLoad.CommandName.Contains("png") || lnkDownLoad.CommandName.Contains("bmp"))
                //{
                //    Response.ContentType = "image/png";
                //}
                //imgdetail.ImageUrl = outfile;
                //Response.WriteFile(outfile);
                //Response.End();
            }
        }
        catch (WebException ex)
        {
            throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
        }

    }
    protected void slider1_TextChanged(object sender, EventArgs e)
    {
        //img.Width = System.Web.UI.WebControls.Unit.Pixel(Convert.ToInt32(Slider1.Text));
    }

    protected void slider2_TextChanged(object sender, EventArgs e)
    {
        //img.Height = System.Web.UI.WebControls.Unit.Pixel(Convert.ToInt32(TextBox1.Text));
    }



    private int? _initialID;

    public int? InitialId
    {
        get
        {
            if (_initialID == null)
            {
                _initialID = !string.IsNullOrEmpty(Request["id"]) ? int.Parse(Request["id"]) : 0;
            }
            return _initialID;
        }
    }

    protected string SetInitialIndex(RadRotatorItem container)
    {
        if ((int)DataBinder.Eval(container.DataItem, "Id") == InitialId)
        {
            ((RadRotator)container.NamingContainer).InitialItemIndex = container.Index;
        }
        return string.Empty;
    }

    public void BindProvider()
    {
        ddlprovider.Items.Clear();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable hsinput = new Hashtable();
        hsinput.Add("@HospitalLocationId", Session["HospitalLocationID"].ToString());
        hsinput.Add("intFacilityId", common.myInt(Session["FacilityId"]));
        DataSet objDs = dl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorList", hsinput);
        if (objDs.Tables[0].Rows.Count > 0)
        {

            ddlprovider.DataSource = objDs;
            ddlprovider.DataValueField = "DoctorId";
            ddlprovider.DataTextField = "DocTorName";
            ddlprovider.DataBind();
        }
        ddlprovider.Items.Insert(0, "");
        ddlprovider.Items[0].Value = "0";
        //if( Session["UserID"]!=null)
        //{

        //    ddlprovider.SelectedValue = Session["UserID"].ToString();
        //}
    }

    private void CheckReviewedby()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        ds = dl.FillDataSet(CommandType.Text, "select ReviewedBy from EMRPatientDocuments where  Id=" + Request.QueryString["id"].ToString() + " and Active=1");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlprovider.SelectedValue = ds.Tables[0].Rows[0]["ReviewedBy"].ToString();

        }


    }

    private void BindStatus()
    {
        ddlreview.Items.Clear();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        ds = dl.FillDataSet(CommandType.Text, "Select StatusId,Status,Code,SequenceNo  from StatusMaster where StatusType='DocumentStatus' and Active=1 order by Status");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlreview.DataSource = ds;
            ddlreview.DataValueField = "StatusId";
            ddlreview.DataTextField = "Status";
            ddlreview.DataBind();
        }
    }
    private void Checkstatus()
    {
        //BindStatus();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        //string sa= "select pd.StatusId  from StatusMaster sm inner join EMRPatientDocuments pd on sm.StatusId=pd.StatusId where StatusType='DocumentStatus' and Id="+ Request.QueryString["id"].ToString() + "and pd.Active=1";
        ds = dl.FillDataSet(CommandType.Text, "select pd.StatusID from StatusMaster sm inner join EMRPatientDocuments pd on sm.StatusId=pd.StatusId where StatusType='DocumentStatus' and Id=" + Request.QueryString["id"].ToString() + "and pd.Active=1");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlreview.SelectedValue = ds.Tables[0].Rows[0]["StatusID"].ToString().Trim();
        }
        else
        {
            BindStatus();
        }
    }

    protected void RadComboBox1_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(RadDateTimePicker1.SelectedDate.Value.ToString());
        sb.Remove(RadDateTimePicker1.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
        sb.Insert(RadDateTimePicker1.SelectedDate.Value.ToString().IndexOf(":") + 1, RadComboBox1.Text);
        RadDateTimePicker1.SelectedDate = Convert.ToDateTime(sb.ToString());
    }
    protected void lnkDownLoad_Click(object sender, EventArgs e)
    {
        try
        {
            string Splitter = ConfigurationManager.AppSettings["Split"];
            if (common.myLen(Splitter).Equals(0))
            {
                Splitter = "!";
            }

            var csplitter = Splitter.ToCharArray();

            //Create FTP Request.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftppath.Split(csplitter)[0].ToString() + Rootfolder + Request.QueryString["hosid"].ToString() + "/" + Request.QueryString["regno"] + "/" + lnkDownLoad.CommandName);
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            //Enter FTP Server credentials.
            request.Credentials = new NetworkCredential(ftppath.Split(csplitter)[1].ToString(), ftppath.Split(csplitter)[2].ToString());
            request.UsePassive = true;
            request.UseBinary = true;
            request.EnableSsl = false;

            //Fetch the Response and read it into a MemoryStream object.
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            using (MemoryStream stream = new MemoryStream())
            {

                //Download the File.
                response.GetResponseStream().CopyTo(stream);
                Response.AddHeader("content-disposition", "attachment;filename=" + lnkDownLoad.CommandName);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                // string outfile = "";
                //common.ByteArrayToFile(FileFolderx + lnkDownLoad.CommandName, stream.ToArray(), out outfile);
                Response.BinaryWrite(stream.ToArray());
                //if (lnkDownLoad.CommandName.Contains("pdf"))
                //{
                //    Response.ContentType = "Application/pdf";
                //}
                //else if (lnkDownLoad.CommandName.Contains("jpg") || lnkDownLoad.CommandName.Contains("png") || lnkDownLoad.CommandName.Contains("bmp"))
                //{
                //    Response.ContentType = "image/png";
                //}
                //Response.WriteFile(outfile);
                Response.End();
            }
        }
        catch (WebException ex)
        {
            throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
        }
    }
    //protected void lnkDownLoad_Click(object sender, EventArgs e)
    //{
    //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    hstInput = new Hashtable();
    //    hstInput.Add("Id", Request.QueryString["id"].ToString());
    //   DataSet objDr = objDl.FillDataSet(CommandType.Text, "select ImageName, ImagePath from EMRPatientDocuments where id = @Id ", hstInput);
    //    //SqlDataReader Dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "select dd.Data, pd.ImageName from EMRPatientDocuments pd INNER JOIN EMRPatientDocumentData dd on pd.Id = dd.DocumentId where pd.id = @Id ", hstInput);
    //    //if (Dr.Read())

    //    if (objDr.Tables[0].Rows.Count > 0)
    //    {
    //        string filename = objDr.Tables[0].Rows[0]["ImagePath"].ToString();
    //        string path = Server.MapPath(filename);
    //        System.IO.FileInfo file = new System.IO.FileInfo(path);
    //        if (file.Exists)
    //        {
    //            Response.Clear();
    //            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
    //            Response.AddHeader("Content-Length", file.Length.ToString());
    //            Response.ContentType = "application/octet-stream";
    //            Response.WriteFile(file.FullName);
    //            Response.End();
    //        }
    //    }
    //}
    protected void btnclose_Click(object sender, EventArgs e)
    {
        string telrikstr = " var oWindow = null; if (window.radWindow) oWindow = window.radWindow; else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; oWindow.close(); ";
        ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", telrikstr, true);
    }
}
