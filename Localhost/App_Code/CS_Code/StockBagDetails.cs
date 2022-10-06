using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for StockBagDetails
/// </summary>
public class StockBagDetails
{
    public StockBagDetails()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public StockBagDetails(int StockId, string BagNumber, string ComponentName, DateTime ExpiryDate, int QtyReceived, string BloodGroupName)
    {
        _stockId = StockId;
        _expiryDate = ExpiryDate;
        _qtyReceived = QtyReceived;
        _bagNumber = BagNumber;
        _componentName = ComponentName;
        _bloodGroupName = BloodGroupName;
    }
    public int _stockId
    {
        get;
        set;
    }

    public string _bagNumber
    {
        get;
        set;
    }

    public string _componentName
    {
        get;
        set;
    }

    public DateTime _expiryDate
    {
        get;
        set;
    }
    public int _qtyReceived
    {
        get;
        set;
    }
    public string _bloodGroupName
    {
        get;
        set;
    }

}
