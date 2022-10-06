using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CardData
/// </summary>
public class CardData
{
 //'ByVal APILoginID As String, ByVal TransactionKey As String, ByVal Track1 As String, ByVal Track2 As String
    private string _APILogin;
    public string APILoginID
    {
        get
        {
            return _APILogin;
        }
        set
        {
            _APILogin = value;
        }
    }
    private string _TransactionKey;
    public string TransactionKey
    {
        get
        {
            return _TransactionKey;
        }
        set
        {
            _TransactionKey = value;
        }
    }
    private string _Track1;
    public string Track1
    {
        get
        {
            return _Track1;
        }
        set
        {
            _Track1 = value;
        }
    }
    private string _Track2;
    public string Track2
    {
        get
        {
            return _Track2;
        }
        set
        {
            _Track2 = value;
        }
    }
    private bool _CardPresent;
    public bool CardPresent
    {
        get
        {
            return _CardPresent;
        }
        set
        {
            _CardPresent = value;
        }
    }
    private string _CardNum;
    public string CardNum
    {
        get
        {
            return _CardNum ;
        }
        set
        {
            _CardNum = value;
        }
    }
    private string _ExpDate;
    public string ExpDate
    {
        get
        {
            return _ExpDate;
        }
        set
        {
            _ExpDate = value;
        }
    }
    private string _Amount;
    public string Amount
    {
        get
        {
            return _Amount;
        }
        set
        {
            _Amount = value;
        }
    }
    private string _PurchaseDesc;
    public string PurchaseDesc
    {
        get
        {
            return _PurchaseDesc ;
        }
        set
        {
            _PurchaseDesc = value;
        }
    }
    private string _FirstName;
    public string FirstName
    {
        get
        {
            return _FirstName;
        }
        set
        {
            _FirstName = value;
        }
    }
    private string _LastName;
    public string LastName
    {
        get
        {
            return _LastName;
        }
        set
        {
            _LastName = value;
        }
    }
    private string _Street;
    public string Street
    {
        get
        {
            return _Street;
        }
        set
        {
            _Street = value;
        }
    }
    private string _State;
    public string State
    {
        get
        {
            return _State;
        }
        set
        {
            _State = value;
        }
    }
    private string _Zip;
    public string Zip
    {
        get
        {
            return _Zip;
        }
        set
        {
            _Zip = value;
        }
    }
    private string _PostUrl;
    public string PostUrl
    {
        get
        {
            return _PostUrl;
        }
        set
        {
            _PostUrl = value;
        }
    }
    private string _TransactionType;
    public string TransactionType
    {
        get
        {
            return _TransactionType;
        }
        set
        {
            _TransactionType = value;
        }
    }

	}
