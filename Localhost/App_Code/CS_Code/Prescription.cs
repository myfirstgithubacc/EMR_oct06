using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class PrescriptionFavoriteItem
{
    public int? FavoriteId { get; set; }
    public string CIMSItemId { get; set; }
    public string CIMSType { get; set; }
    public int? ClosingBalance { get; set; }
    public string DDCCode { get; set; }
    public int? Dose { get; set; }
    public int? Duration { get; set; }
    public string DurationType { get; set; }
    public int? FoodRelationshipId { get; set; }
    public int? FormulationId { get; set; }
    public int? FrequencyId { get; set; }
    public int? GenericId { get; set; }
    public string Instructions { get; set; }
    public int? ItemId { get; set; }
    public string ItemName { get; set; }
    public string ItemNo { get; set; }
    public string ItemSubCategoryShortName { get; set; }
    public string ItemWithStock { get; set; }
    public int? RouteId { get; set; }
    public int? StrengthId { get; set; }
    public string StrengthValue { get; set; }
    public int? UnitId { get; set; }
    public string VIDALItemId { get; set; }

    public string GenericName { get; set; }

}

public class CurrentMedicationItem
{
    public string ItemId { get; set; }
    public string ItemName { get; set; }
    public string GenericId { get; set; }
    public string GenericName { get; set; }
    public string CIMSItemId { get; set; }
    public string CIMSType { get; set; }
    public string VIDALItemId { get; set; }
}