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
using System.Data.SqlClient;
using System.Text;
using System.Web.Configuration;
using System.Security.Cryptography;

public partial class HospitalSetUp_ResetPassword : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (Session["UserID"] != null)
        {
            this.MasterPageFile = "/Include/Master/EMRMaster.master";// amend as
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindHospitals();
            rdOrganization.SelectedIndex = 0;
            rdOrganization_SelectedIndexChanged(this, null);
        }
    }

    protected void gvEmployees_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select1")
            {
                BaseC.EncryptDecrypt en = new BaseC.EncryptDecrypt();
                HiddenField hdnempid = (HiddenField)e.Item.FindControl("hdnEmployeeID");
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hsInput = new Hashtable();
                hsInput.Add("EmpId", hdnempid.Value);
                string UserName = (string)objDl.ExecuteScalar(CommandType.Text, "SELECT UserName FROM Users WHERE EmpID = @EmpId", hsInput);
                txtUserName.Text = en.Decrypt(UserName, en.getKey(sConString), true);
                Random rn = new Random();
                int i = rn.Next(6, 20);
                txtOldPassword.Text = "";
                for (int j = 0; j <= i; j++)
                {
                    txtOldPassword.Text += "*";
                }
                ViewState["UserId"] = hdnempid.Value;
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void bindHospitals()
    {
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet objDs = objDl.FillDataSet(CommandType.Text, "select Id as HospitalLocationId, Name as HospitalName from HospitalLocation");
            rdOrganization.DataSource = objDs;
            rdOrganization.DataTextField = "HospitalName";
            rdOrganization.DataValueField = "HospitalLocationId";
            rdOrganization.DataBind();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void rdOrganization_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rdOrganization.SelectedValue != "")
            {
                Hashtable hsInput = new Hashtable();
                hsInput.Add("HospitalLocationId", rdOrganization.SelectedValue);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                DataSet objDs = objDl.FillDataSet(CommandType.Text, "select ID as EmployeeId, FirstName + ' ' + MiddleName + ' ' + LastName as EmployeeName from Employee where HospitalLocationId = @HospitalLocationId Order by FirstName", hsInput);
                gvEmployee.DataSource = objDs;
                gvEmployee.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void cmdSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtUserName.Text.Trim() != "")
            {
                DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                BaseC.EncryptDecrypt eN = new BaseC.EncryptDecrypt();
                BaseC.ParseData parse = new BaseC.ParseData();
                Hashtable hstInput;
                hstInput = new Hashtable();
                hstInput.Add("UserName", eN.Encrypt(txtUserName.Text, eN.getKey(sConString), true, ""));
                SqlDataReader objDr = (SqlDataReader)obj.ExecuteReader(CommandType.Text, "SELECT UserName, PrivateKey FROM Users u INNER JOIN UserPrivateKey up ON u.ID = up.UserId WHERE UserName = @UserName", hstInput);
                string sUserName = "", sPrivateKey = "";

                if (objDr.Read())
                {
                    sUserName = objDr["UserName"].ToString();
                    sPrivateKey = objDr["PrivateKey"].ToString();
                }

                //byte[] bPrivateKey = UTF8Encoding.UTF8.GetBytes(sPrivateKey);

                if (IsValid)
                {
                    BaseC.User valUser = new BaseC.User(sConString);
                    string strpassword = eN.Encrypt(txtPassword.Text, eN.getKey(sConString), true, sPrivateKey);

                    if (ViewState["UserId"] != null)
                    {
                        //string srtupdt = "update Users set Password='" + strpassword.Trim() + "' where ID=" + usercode.Value + "";
                        hstInput = new Hashtable();
                        hstInput.Add("Password", strpassword.Trim());
                        hstInput.Add("Id", ViewState["UserId"]);
                        string srtupdt = "update Users set Password = @Password, NextExpiryDate = DATEADD(\"DAY\", Convert(int, PasswordExpiryPeriod), getdate()) where EmpID = @Id ";
                        int i = obj.ExecuteNonQuery(CommandType.Text, srtupdt, hstInput);

                        Alert.ShowAjaxMsg("Password Reset!", Page);
                        txtPassword.Text = "";
                        txtConfirmPassword.Text = "";
                        ViewState["UserId"] = null;
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

    protected void lnkCreateNewOrganization_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("HospitalSetUp.aspx", false);
    }

    protected void gvEmployees_PreRender(object sender, System.EventArgs e)
    {
        rdOrganization_SelectedIndexChanged(this, null);
    }

    protected void gvEmployees_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        rdOrganization_SelectedIndexChanged(this, null);
    }

    protected void gvEmployees_SortCommand(object source, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        rdOrganization_SelectedIndexChanged(this, null);
    }

    protected void gvEmployees_PageSizeChanged(object source, GridPageSizeChangedEventArgs e)
    {
        rdOrganization_SelectedIndexChanged(this, null);
    }

    protected void gvEmployees_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BaseC.EncryptDecrypt en = new BaseC.EncryptDecrypt();
            HiddenField hdnempid = (HiddenField)gvEmployee.SelectedItems[0].FindControl("hdnEmployeeID");
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hsInput = new Hashtable();
            hsInput.Add("EmpId", hdnempid.Value);
            string UserName = (string)objDl.ExecuteScalar(CommandType.Text, "SELECT UserName FROM Users WHERE EmpID = @EmpId", hsInput);
            txtUserName.Text = en.Decrypt(UserName, en.getKey(sConString), true);
            Random rn = new Random();
            int i = rn.Next(6, 20);
            txtOldPassword.Text = "";
            for (int j = 0; j <= i; j++)
            {
                txtOldPassword.Text += "*";
            }
            ViewState["UserId"] = hdnempid.Value;
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

}