using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public enum AutoSaveCommand
{
    Save,
    Delete,
    GetTempData
}
/// <summary>
/// To save the control values automatically.
/// </summary>
public class AutoSaveEMRSingleScreen : Entity, IDisposable
{
    public AutoSaveEMRSingleScreen()
    {
        //
        // TODO: Add constructor logic here
        //
        TextBoxes = new List<MyControls>();
        RadEdtors = new List<MyControls>();
        CheckBoxes = new List<MyControls>();
        RadioButtons = new List<MyControls>();
        DropDowns = new List<MyControls>();
        CreatedDate = DateTime.UtcNow.Date;
    }
    public string PatientDocumentId { get; set; }
    public List<MyControls> TextBoxes { get; set; }
    public List<MyControls> RadEdtors { get; set; }
    public List<MyControls> CheckBoxes { get; set; }
    public List<MyControls> RadioButtons { get; set; }
    public List<MyControls> DropDowns { get; set; }
    public DateTime CreatedDate { get; set; }

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            disposedValue = true;
        }
    }

    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    // ~clsAutoSave() {
    //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
    //   Dispose(false);
    // }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        Dispose(true);
        // TODO: uncomment the following line if the finalizer is overridden above.
        // GC.SuppressFinalize(this);
    }
    #endregion

}
public class AutoSaveProgressNote:Entity,IDisposable
{
    public AutoSaveProgressNote()
    {
        ProgressNotes = new List<MyControls>();
        CreatedDate = DateTime.UtcNow.Date;
    }
    public string PatientDocumentId { get; set; }
    public List<MyControls> ProgressNotes { get; set; }
    public DateTime CreatedDate { get; set; }

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            disposedValue = true;
        }
    }

    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    // ~AutoSaveProgressNote() {
    //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
    //   Dispose(false);
    // }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        Dispose(true);
        // TODO: uncomment the following line if the finalizer is overridden above.
        // GC.SuppressFinalize(this);
    }
    #endregion

}
public class MyControls
{
    public string ControlId { get; set; }
    public string ControlValue { get; set; }
}