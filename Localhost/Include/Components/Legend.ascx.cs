using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

public partial class Include_Components_Legend : System.Web.UI.UserControl
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsLISPhlebotomy objval;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }
    }

    private DataTable GetLegend(string sStatusType, string sCode)
    {
        DataTable DT = new DataTable();
        try
        {
            objval = new BaseC.clsLISPhlebotomy(sConString);
            DataSet ds = objval.getStatus(common.myInt(Session["HospitalLocationID"]), sStatusType, "");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataView dv = new DataView(ds.Tables[0]);
                    if (sCode != "")
                    {
                        dv.RowFilter = "Code IN(" + sCode + ")";
                    }
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        DT = dv.ToTable();
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return DT;
    }

    public void loadLegend(string sStatusType, string sCode)
    {
        try
        {
            DataTable dt = GetLegend(sStatusType, sCode);
            if (dt.Rows.Count < 1)
            {
                return;
            }

            Label LBL;

            TableRow tr;
            TableCell td;

            int ROWS = 1;
            int COLS = dt.Rows.Count;
            DataRow DR;

            for (int rowIdx = 0; rowIdx < ROWS; rowIdx++)
            {
                tr = new TableRow();

                for (int colIdx = 0; colIdx < COLS; colIdx++)
                {
                    DR = dt.Rows[colIdx];

                    td = new TableCell();
                    LBL = new Label();
                    LBL.BorderWidth = Unit.Pixel(1);
                    LBL.ID = "LabelStatusColor" + colIdx;
                    LBL.BackColor = System.Drawing.Color.FromName(common.myStr(DR["StatusColor"]));
                    LBL.SkinID = "label";
                    LBL.Width = Unit.Pixel(15);
                    LBL.Height = Unit.Pixel(14);

                    td.Controls.Add(LBL);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    LBL = new Label();
                    LBL.ID = "LabelStatus" + colIdx;
                    LBL.Text = common.myStr(DR["Status"]).Replace(" ", "&nbsp;");
                    LBL.Font.Size = 8;

                    LBL.SkinID = "label";

                    td.Controls.Add(LBL);
                    tr.Cells.Add(td);
                }

                tblLegend.Rows.Add(tr);
            }
        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
}
