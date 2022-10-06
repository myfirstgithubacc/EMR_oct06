using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;

public partial class MPages_ChangePassword : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

    //Changing the Master Page if it is open as Popup window
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["From"]) == "Login")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void ibtnChangePassword_Click(object sender, EventArgs e)
    {
        try
        {
            DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            BaseC.EncryptDecrypt eN = new BaseC.EncryptDecrypt();
            Hashtable hstInput;
            hstInput = new Hashtable();
            hstInput.Add("UserName", eN.Encrypt(txtchUserName.Text, eN.getKey(sConString), true, ""));
            SqlDataReader objDr = (SqlDataReader)obj.ExecuteReader(CommandType.Text, "SELECT UserName, PrivateKey FROM Users u INNER JOIN UserPrivateKey up ON u.ID = up.UserId WHERE UserName = @UserName", hstInput);
            string sUserName = "", sPrivateKey = "";
            if (objDr.HasRows)
            {
                if (objDr.Read())
                {
                    sUserName = common.myStr(objDr["UserName"]);
                    sPrivateKey = common.myStr(objDr["PrivateKey"]);
                }
            }
            objDr.Close();
            byte[] bPrivateKey = UTF8Encoding.UTF8.GetBytes(sPrivateKey);

            if (IsValid)
            {
                if (!common.myStr(Session["HospitalLocationID"]).Equals(""))
                {
                    BaseC.User valUser = new BaseC.User(sConString);
                    if (valUser.ValidateUser(txtchUserName.Text, txtoldpassword.Text, common.myStr(Session["HospitalLocationID"])))
                    {
                        DataSet ds = new DataSet();
                        ds = valUser.ValidateUserName(txtchUserName.Text, txtoldpassword.Text);
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string strpassword = eN.Encrypt(txtnewpassword.Text.Trim(), eN.getKey(sConString), true, sPrivateKey);
                                usercode.Value = common.myStr(ds.Tables[0].Rows[0]["UserId"]);
                                if (usercode.Value != "0")
                                {
                                     hstInput = new Hashtable();
                                    hstInput.Add("Password", common.myStr(strpassword).Trim());
                                    hstInput.Add("Id", common.myStr(usercode.Value));
                                   
                                    string srtupdt = "update Users set Password=@Password, NextExpiryDate = DATEADD(dd, isnull(PasswordExpiryPeriod,0), getdate()) where ID = @Id   ";
                                    obj.ExecuteNonQuery(CommandType.Text, srtupdt, hstInput);

                                    lblMessage.Text = "Password changed successfuly!";
                                    lblMessage.ForeColor = System.Drawing.Color.Green;

                                    txtoldpassword.Text = "";
                                    txtnewpassword.Text = "";
                                    txtConfirmPassword.Text = "";
                                }
                                else
                                {
                                    lblMessage.Text = "User name  or password you entered is incorrect.";
                                    lblMessage.ForeColor = System.Drawing.Color.Red;
                                    txtchUserName.Text = "";
                                    txtoldpassword.Text = "";
                                    txtnewpassword.Text = "";
                                    txtConfirmPassword.Text = "";
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnpasshelp_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["pwd"] = txtnewpassword.Text.Trim();
            txtnewpassword.Attributes.Add("value", common.myStr(ViewState["pwd"]));
            txtConfirmPassword.Attributes.Add("value", common.myStr(ViewState["pwd"]));
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

}