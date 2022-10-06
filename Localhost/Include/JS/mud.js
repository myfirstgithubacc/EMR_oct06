function mu_quality_measures() {
    radopen("MUQualityMeasures.aspx", "RadWindow1");
}
function UpdateItemCountField(sender, args) {
    //set the footer text
    sender.get_dropDownElement().lastChild.innerHTML = "A total of " + sender.get_items().get_count() + " items";
}
function CloseWindow() {
    var CreateNumeratorWindow = GetRadWindow();
    CreateNumeratorWindow.Close();
}
function GetRadWindow() {
    var CreateNumeratorWindow = null;
    if (window.radWindow) CreateNumeratorWindow = window.radWindow;
    else if (window.frameElement.radWindow) CreateNumeratorWindow = window.frameElement.radWindow;
    return CreateNumeratorWindow;
}    