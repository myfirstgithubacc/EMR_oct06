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


public partial class EMR_Vitals_calculator : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (rdohwt.SelectedValue == "HT" && rdomeasurment.SelectedValue == "ENG")
            {
                lblMessage.Text = "";
                Clear();

                txtfeet.Enabled = true;
                txtInch.Enabled = true;
                txtcentimeter.Enabled = false;
                txtPound.Enabled = true;
                txtkilogram.Enabled = false;
                txtFahrenheit.Enabled = true;
                txtCelcius.Enabled = false;

                trheight1.Visible = true;
                trheight2.Visible = true;
                trweight1.Visible = false;
                trweight2.Visible = false;
                trtempreture1.Visible = false;
                trtempreture2.Visible = false;
            }
            else
            {
                txtfeet.Enabled = false;
                txtInch.Enabled = false;
                txtcentimeter.Enabled = true;
                txtkilogram.Enabled = true;
                txtFahrenheit.Enabled = false;
                txtCelcius.Enabled = true;
                txtPound.Enabled = false;
            }
        }
    }
    protected void btnCalculate_Click(object sender, EventArgs e)
    {
        try
        {
            double inch, feet, centimeter;
            double pound, kg;
            double fahren, celcious;
            // Covert Height to inches and feet into centimeter and centimeter into inches and feet
            if (rdohwt.SelectedValue == "HT")
            {
                if (rdomeasurment.SelectedValue == "ENG")
                {


                    inch = common.myDbl(txtInch.Text);
                    feet = common.myDbl(txtfeet.Text);

                    if (common.myInt(txtInch.Text) != 0 || common.myInt(txtfeet.Text) != 0)
                    {
                        lblMessage.Text = "";
                        centimeter = inch * 2.54 + feet * 30.5;
                        txtcentimeter.Text = common.myDbl(centimeter).ToString("F" + 2); //common.myStr(centimeter);
                    }
                    //else if (common.myInt(txtfeet.Text) != 0)
                    //{
                    //    lblMessage.Text = "";
                    //    centimeter = feet * 30.5;
                    //    txtcentimeter.Text = common.myStr(centimeter);
                    //}
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Please enter value of feet or inches !";
                    }
                }
                else
                {
                    inch = common.myDbl(txtInch.Text);
                    feet = common.myDbl(txtfeet.Text);
                    centimeter = common.myDbl(txtcentimeter.Text);
                    if (common.myInt(txtcentimeter.Text) != 0)
                    {
                        lblMessage.Text = "";
                        inch = centimeter * 0.394;
                        feet = inch / 12;
                       //feet = inch * 0.083333;
                     //  feet = centimeter * 0.0328084;
                     //   txtInch.Text = common.myDbl(inch).ToString("F" + 2);//common.myStr(inch);
                        string[] str = common.myDbl(feet).ToString("F" + 2).Split('.');
                        //txtfeet.Text = common.myDbl(feet).ToString("F" + 2);//common.myStr(feet);
                        if (str.Length > 1)
                        {
                            decimal inc;
                            decimal m;
                            if (str[0] != "")
                            {
                                txtfeet.Text = str[0];
                            }
                            if (str[1] != "")
                            {
                                inc = 12 * common.myDec("." + str[1]);
                                m = Math.Round(inc);
                                txtInch.Text = common.myStr(m);
                            }
                        }
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Please enter value of centimeter !";
                    }
                }
            }
            // Convert Weight to Pound into kilogram and kilogram to pound
            else if (rdohwt.SelectedValue == "WT")
            {
                if (rdomeasurment.SelectedValue == "ENG")
                {
                    
                    if (common.myInt(txtPound.Text) != 0)
                    {
                      pound= common.myDbl(txtPound.Text);

                      kg = (common.myDbl(txtPound.Text) / 2.2046);
                      txtkilogram.Text = common.myDbl(kg).ToString("F" + 2);// common.myStr(kg);
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Please enter value of Kilogram !";
                    } 

                }
                else
                {
                    if (common.myInt(txtkilogram.Text) != 0)
                    {
                        kg = common.myDbl(txtkilogram.Text);

                        pound = (common.myDbl(txtkilogram.Text) * 2.2046);
                       // txtPound.Text = common.myStr(pound);
                        txtPound.Text = common.myDbl(pound).ToString("F" + 2);
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Please enter value of Kilogram !";
                    }

                }
            }

            // Convert Weight to Fahrenheight into Celcius and Celcius to fahrenheight
            else if (rdohwt.SelectedValue == "TR")
            {
                if (rdomeasurment.SelectedValue == "ENG")
                {
                    if (common.myInt(txtFahrenheit.Text) != 0)
                    {
                        fahren = common.myDbl(txtFahrenheit.Text);

                        celcious = (common.myDbl(txtFahrenheit.Text) - 32) / 1.8;
                        txtCelcius.Text = common.myDbl(celcious).ToString("F" + 2);// common.myStr(celcious);
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Please enter value of Fahrenheight !";
                    }

                }
                else
                {
                    if (common.myInt(txtCelcius.Text) != 0)
                    {
                        celcious = common.myDbl(txtCelcius.Text);

                        fahren = (1.8 * (common.myDbl(txtCelcius.Text))) + 32;
                        txtFahrenheit.Text = common.myDbl(fahren).ToString("F" + 2); //common.myStr(fahren);

                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Please enter value of Celcius !";
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void rdomeasurment_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdomeasurment.SelectedValue == "ENG")
        {
            lblMessage.Text = "";
            Clear();

            txtfeet.Enabled = true;
            txtInch.Enabled = true;
            txtcentimeter.Enabled = false;
            txtPound.Enabled = true;
            txtkilogram.Enabled = false;
            txtFahrenheit.Enabled = true;
            txtCelcius.Enabled = false;

        }
        else
        {
            lblMessage.Text = "";
            Clear();

            txtfeet.Enabled = false;
            txtInch.Enabled = false;
            txtcentimeter.Enabled = true;
            txtkilogram.Enabled = true;
            txtFahrenheit.Enabled = false;
            txtCelcius.Enabled = true;
            txtPound.Enabled = false;
        }
    }

    void Clear()
    {
        txtcentimeter.Text = "";
        txtfeet.Text = "";
        txtInch.Text = "";
        txtPound.Text = "";
        txtkilogram.Text = "";
        txtFahrenheit.Text = "";
        txtCelcius.Text = "";

    }

    protected void btnnew_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        Clear();
    }
    protected void rdohwt_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdohwt.SelectedValue == "HT")
        {
            lblMessage.Text = "";
            Clear();

            trheight1.Visible = true;
            trheight2.Visible = true;
            trweight1.Visible = false;
            trweight2.Visible = false;
            trtempreture1.Visible = false;
            trtempreture2.Visible = false;
        }
        else if (rdohwt.SelectedValue == "WT")
        {
            lblMessage.Text = "";
            Clear();

            trheight1.Visible = false;
            trheight2.Visible = false;
            trweight1.Visible = true;
            trweight2.Visible = true;
            trtempreture1.Visible = false;
            trtempreture2.Visible = false;
        }
        else if (rdohwt.SelectedValue == "TR")
        {
            lblMessage.Text = "";
            Clear();

            trheight1.Visible = false;
            trheight2.Visible = false;
            trweight1.Visible = false;
            trweight2.Visible = false;
            trtempreture1.Visible = true;
            trtempreture2.Visible = true;
        }
    }
}
