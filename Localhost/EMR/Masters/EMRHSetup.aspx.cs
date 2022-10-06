using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;


public partial class EMR_Masters_EMRHSetup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if(!IsPostBack)
        {
            bindGrid();
        }
       
    }

    string cs = ConfigurationManager.ConnectionStrings["EMRConnectionString"].ConnectionString;
    Hashtable HshIn = new Hashtable();
    Hashtable HshOut = new Hashtable();
    Hashtable output = new Hashtable();
    protected void Button1_Click(object sender, EventArgs e)
    {
        savebtn.Text = "Save";
    }

    protected void Button2_Click(object sender, EventArgs e)
    {

        if (idtxtbox.Text.Equals(""))
        {
            lblMsg.Text = "Enter Id First!";
            lblMsg.ForeColor = System.Drawing.Color.Red;
            lblMsg.Visible = true;
            return;
        }
        else
        {
            lblMsg.Visible = false;
        }
        if (flagtxtbox.Text.Equals(""))
        {
            lblMsg.Text = "Enter Id Flag Name!";
            lblMsg.ForeColor = System.Drawing.Color.Red;
            lblMsg.Visible = true;
            return;
        }
        else
        {
            lblMsg.Visible = false;
        }
        if (statusdropdown.SelectedItem.Text.Equals("Select"))
        {
            lblMsg.Text = "Select Status First!";
            lblMsg.ForeColor = System.Drawing.Color.Red;
            lblMsg.Visible = true;
            return;
        }
        else
        {
            lblMsg.Visible = false;
        }

        if (idtxtbox.Text.Equals("") && flagtxtbox.Text.Equals("") && pagetxtbox.Text.Equals("") && statusdropdown.SelectedItem.Text.Equals("Select"))
        {
            lblMsg.Text = "Enter Records First!";
            lblMsg.ForeColor = System.Drawing.Color.Red;
            lblMsg.Visible = true;
            return;
        }

        else
        {
            lblMsg.Visible = false;
            if (savebtn.Text.Equals("Save"))
            {
                SqlConnection con = new SqlConnection(cs);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, cs);
                // save
                int stat = 0;
                if (statusdropdown.SelectedItem.Text.Equals("Active"))
                    stat = 1;
                else
                    stat = 0;
                HshIn.Add("@id", Convert.ToInt32(idtxtbox.Text));
                HshIn.Add("@flag", flagtxtbox.Text.Trim());
                HshIn.Add("@module", moduledropdown1.SelectedItem.Text);
                HshIn.Add("@page", pagetxtbox.Text.Trim());
                HshIn.Add("@status", stat);
                HshOut.Add("@output", SqlDbType.VarChar);
                try
                {
                    con.Open();
                    output = objDl.getOutputParametersValues(CommandType.StoredProcedure, "saveHospitalSetup", HshIn, HshOut);
                    if (output["@output"].ToString().Equals("inserted"))
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Inserted !')</script>");
                        bindGrid();
                    }
                    else
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Not Inserted !')</script>");
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    con.Close();
                    HshIn = null;
                    HshOut = null;
                    output = null;
                }


            }
            if (savebtn.Text.Equals("Update"))
            {
                    // update
                    SqlConnection con = new SqlConnection(cs);
                    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, cs);
                    int stat = 0;
                    if (statusdropdown.SelectedItem.Text.Equals("Active"))
                        stat = 1;
                    else
                        stat = 0;
                    HshIn.Add("@id", Convert.ToInt32(idtxtbox.Text));
                    HshIn.Add("@flag", flagtxtbox.Text.Trim());
                    HshIn.Add("@module", moduledropdown1.SelectedItem.Text);
                    HshIn.Add("@page", pagetxtbox.Text.Trim());
                    HshIn.Add("@status", stat);
                    HshOut.Add("@output", SqlDbType.VarChar);
                    try
                    {
                        con.Open();
                        output = objDl.getOutputParametersValues(CommandType.StoredProcedure, "updateEMRRecord", HshIn, HshOut);
                    if (output["@output"].ToString().Equals("updated"))
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Flag updated! !')</script>");
                        bindGrid();
                    }
                    else
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Flag not updated !')</script>");

                }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        con.Close();
                        HshIn = null;
                        HshOut = null;
                        output = null;
                    }
                    
            }
        }
    }

    protected void Button3_Click1(object sender, EventArgs e)
    {
        savebtn.Text = "Save";
        moduledropdrown.SelectedIndex = 0;
        idtxtbox.Text = "";
        flagtxtbox.Text = "";
        pagetxtbox.Text = "";
        moduledropdown1.SelectedIndex = 0;
        statusdropdown.SelectedIndex = 0;
        datagrid.DataSource = null;
        datagrid.DataBind();
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        savebtn.Text = "Update";
        int rowindex = ((GridViewRow)(sender as Control).NamingContainer).RowIndex;

        idtxtbox.Text = datagrid.Rows[rowindex].Cells[0].Text; //id
        flagtxtbox.Text = datagrid.Rows[rowindex].Cells[1].Text; // flag

        string modulename = datagrid.Rows[rowindex].Cells[2].Text;// module
        if (modulename.Length==0)
            moduledropdown1.Items.Insert(0, new ListItem("Select", ""));
        else
            moduledropdown1.SelectedItem.Text = datagrid.Rows[rowindex].Cells[2].Text; 

        string pagename = datagrid.Rows[rowindex].Cells[3].Text;// page name
        if (pagename.Length == 0)
            pagetxtbox.Text = "";
        else
        pagetxtbox.Text = datagrid.Rows[rowindex].Cells[3].Text; 

        string activestatus = datagrid.Rows[rowindex].Cells[4].Text;   //active

        if (activestatus.Equals("1"))
            statusdropdown.SelectedIndex = 0;
        else
            statusdropdown.SelectedIndex = 1;
    }

    protected void datagrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        datagrid.PageIndex = e.NewPageIndex;
        bindGrid();
    }

    //bind grid
    private void bindGrid()
    {
        SqlConnection con = new SqlConnection(cs);
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, cs);
        string sendQuery = "All";
        con.Open();
        if (moduledropdrown.Text.Equals("EMR"))
            sendQuery = "EMR";

        else if (moduledropdrown.Text.Equals("OT"))
            sendQuery = "OT";

        else if (moduledropdrown.Text.Equals("NB"))
            sendQuery = "Nurse";

        else if (moduledropdrown.Text.Equals("MRD"))
            sendQuery = "MRD";

        HshIn.Add("@QUERY", sendQuery);
        try
        {
            datagrid.DataSource = objDl.FillDataSet(CommandType.StoredProcedure, "flagSearch", HshIn);
            datagrid.DataBind();
        }
        catch(Exception ex)
        {

        }
        finally
        {
            con.Close();
            HshIn = null;
        }

    }

    protected void moduledropdrown_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindGrid();
    }
}