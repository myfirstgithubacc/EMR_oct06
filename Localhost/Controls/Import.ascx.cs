using System;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI.Dictionaries;

public partial class Controls_Import : System.Web.UI.UserControl
{
    private Dictionaries dictionaries = new Dictionaries(HttpContext.Current);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                FillFilesGrid();
            }
            catch (UnauthorizedAccessException)
            {
                ReportError("Access denied to the web application's folder.  Please grant the ASP.NET user account write permissions to the folder.");
            }
            catch (Exception unknownError)
            {
                ReportError(unknownError.Message);
            }
        }

        errorPanel.Visible = false;
    }

    private void FillFilesGrid()
    {
        ArrayList names = dictionaries.DictionaryNames();

        importedFiles.DataSource = names;
        importedFiles.DataBind();
    }

    public static string BaseName(string fullName)
    {
        FileInfo file = new FileInfo(fullName);
        return file.Name;
    }

    private void ReportError(string message)
    {
        errorPanel.Visible = true;
        errorMessage.Text = message;
    }

    private void ImportUploadedFile()
    {
        using (StreamReader imported = new StreamReader(importedFile.PostedFile.InputStream))
        {
            DictionaryImporter importer = new DictionaryImporter();
            importer.Load(imported);

            importer.Save(dictionaries.AbsoluteDictionaryFileName(BaseName(importedFile.PostedFile.FileName)));
        }
    }

    protected void importedFiles_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        DataGridItem item = e.Item;

        HyperLink link = item.FindControl("fileLink") as HyperLink;
        if (link != null)
        {
            link.NavigateUrl = dictionaries.RelativeUrl(item.DataItem.ToString());
            link.Text = item.DataItem.ToString();
        }
    }

    protected void importedFiles_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        DataGridItem item = e.Item;

        try
        {
            HyperLink link = item.FindControl("fileLink") as HyperLink;
            if (link != null)
            {
                string fileName = link.Text;
                File.Delete(dictionaries.AbsoluteDictionaryFileName(fileName));
            }

            FillFilesGrid();
        }
        catch (UnauthorizedAccessException)
        {
            ReportError(string.Format("Access denied to the {0} folder.  Please grant the ASP.NET user account write permissions to the folder.", dictionaries.DictionaryPath));
        }
        catch (Exception unknownError)
        {
            ReportError(unknownError.Message);
        }
    }

    protected void importButton_Click(object sender, EventArgs e)
    {
        if (importedFile.PostedFile == null)
            return;

        if (!importedFile.PostedFile.FileName.ToLower().EndsWith(".tdf"))
        {
            ReportError("Please upload .TDF files only.");
            return;
        }

        try
        {
            ImportUploadedFile();
            FillFilesGrid();
        }
        catch (UnauthorizedAccessException)
        {
            ReportError(string.Format("Access denied to the {0} folder.  Please grant the ASP.NET user account write permissions to the folder.", dictionaries.DictionaryPath));
        }
        catch (Exception unknownError)
        {
            ReportError(unknownError.Message);
        }
    }
}
