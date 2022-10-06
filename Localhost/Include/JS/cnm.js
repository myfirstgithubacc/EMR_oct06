
function openWin() {
    var oWnd = radopen("SelectICD.aspx", "RadWindow1");
}

function OnClientClose(oWnd, args) {
    //get the transferred arguments
    var arg = args.get_argument();
    if (arg) {

        var selICD = arg.selICD;
        $get("LabelICD").value = selICD;
    }
}
function openWin1() {
    var oWnd1 = radopen("SelectCPT.aspx", "RadWindow2");
}

function OnClientClose1(oWnd1, args) {
    //get the transferred arguments
    var arg = args.get_argument();
    if (arg) {

        var selCPT = arg.selCPT;
        $get("LabelCPT").value = selCPT;
    }
}
function openWin2() {
    var oWnd2 = radopen("SelectLOINC.aspx", "RadWindow3");
}

function OnClientClose2(oWnd2, args) {
    //get the transferred arguments
    var arg = args.get_argument();
    if (arg) {

        var selCPT = arg.selCPT;
        $get("LabelLOINC").value = selLOINC;
    }
}
function openWin3() {
    var oWnd3 = radopen("CreateRuleCondition.aspx", "RadWindow4");
}

function OnClientClose3(oWnd3, args) {
    //get the transferred arguments
    var arg = args.get_argument();
    if (arg) {

        var selRule = arg.selRule;
        $get("LabelRule").value = selRule;
    }
}

function CloseWindow() {
    var AddNewRuleWindow = GetRadWindow();
    AddNewRuleWindow.Close();
}
function GetRadWindow() {
    var AddNewRuleWindow = null;
    if (window.radWindow) AddNewRuleWindow = window.radWindow;
    else if (window.frameElement.radWindow) AddNewRuleWindow = window.frameElement.radWindow;
    return AddNewRuleWindow;
}
function ruleType_QM() {
    radopen("RuleTypeQM.aspx", "RadWindow5");
}
function ruleType_HM() {
    radopen("RuleTypeHM.aspx", "RadWindow6");
}
function ruleType_DS() {
    radopen("RuleTypeDS.aspx", "RadWindow7");
}

        function CloseWindow1() {
            var AddQualityMesureWindow = GetRadWindow1();
            AddQualityMesureWindow.Close();
        }
        function GetRadWindow1() {
            var AddQualityMesureWindow = null;
            if (window.radWindow) AddQualityMesureWindow = window.radWindow;
            else if (window.frameElement.radWindow) AddQualityMesureWindow = window.frameElement.radWindow;
            return AddQualityMesureWindow;
        }
        

     
 
    
         function UpdateItemCountField(sender, args) {
             //set the footer text
             sender.get_dropDownElement().lastChild.innerHTML = "A total of " + sender.get_items().get_count() + " items";
         }
   
        function CloseWindow2() {
            var AddQualityMesureWindow = GetRadWindow2();
            AddQualityMesureWindow.Close();
        }
        function GetRadWindow2() {
            var AddQualityMesureWindow = null;
            if (window.radWindow) AddQualityMesureWindow = window.radWindow;
            else if (window.frameElement.radWindow) AddQualityMesureWindow = window.frameElement.radWindow;
            return AddQualityMesureWindow;
        }


        function CloseWindow() {
            var CreateNumeratorWindow = GetRadWindow3();
            CreateNumeratorWindow.Close();
        }
        function GetRadWindow3() {
            var CreateNumeratorWindow = null;
            if (window.radWindow) CreateNumeratorWindow = window.radWindow;
            else if (window.frameElement.radWindow) CreateNumeratorWindow = window.frameElement.radWindow;
            return CreateNumeratorWindow;
        }   
   
        function GetRadWindow4()
        {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }
        function returnToParent()
        {
            //create the argument that will be returned to the parent page
            var oArg = new Object();  
            //get a reference to the current RadWindow
            oArg.selRule = document.getElementById("TextBox1").value;
            var oWnd3 = GetRadWindow();
            //Close the RadWindow and send the argument to the parent page
            if (oArg.selRule)
            {
                oWnd3.close(oArg);
            }
            else
            {
                alert("Please Create Rule Condition");
            }
        }
        
       
        function CloseWindow5() {
            var AddQualityMesureWindow = GetRadWindow5();
            AddQualityMesureWindow.Close();
        }
        function GetRadWindow5() {
            var AddQualityMesureWindow = null;
            if (window.radWindow) AddQualityMesureWindow = window.radWindow;
            else if (window.frameElement.radWindow) AddQualityMesureWindow = window.frameElement.radWindow;
            return AddQualityMesureWindow;
        }    

        function CloseWindow6() {
            var PatientLettersWindow = GetRadWindow6();
            PatientLettersWindow.Close();
        }
        function GetRadWindow6() {
            var PatientLettersWindow = null;
            if (window.radWindow) PatientLettersWindow = window.radWindow;
            else if (window.frameElement.radWindow) PatientLettersWindow = window.frameElement.radWindow;
            return PatientLettersWindow;
        }
        

        function add_quality_measure() {
            radopen("AddQualityMeasure.aspx", "RadWindow1");
        }
        function addNewRule_denominator() {
            radopen("AddNewRule.aspx", "RadWindow2");
        }
        function addNewRule_numerator() {
            radopen("CreateNumerator.aspx","RadWindow3");
        }
        function patient_registry() {
            radopen("PatientRegistry.aspx", "RadWindow4");
        }

        function GetRadWindow7()
        {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }
        function returnToParent1()
        {
            //create the argument that will be returned to the parent page
            var oArg = new Object();  
            //get a reference to the current RadWindow
            oArg.selCPT = document.getElementById("TextBox1").value;
            var oWnd1 = GetRadWindow7();
            //Close the RadWindow and send the argument to the parent page
            if (oArg.selCPT)
            {
                oWnd1.close(oArg);
            }
            else
            {
                alert("Please Select CPTs");
            }
        }

        function GetRadWindow8()
        {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }
        function returnToParent2()
        {
            //create the argument that will be returned to the parent page
            var oArg = new Object();

          
            //get a reference to the current RadWindow
            oArg.selICD = document.getElementById("TextBox1").value;
            var oWnd = GetRadWindow8();
            //Close the RadWindow and send the argument to the parent page
            if (oArg.selICD)
            {
                oWnd.close(oArg);
            }
            else
            {
                alert("Please Select ICDs");
            }
        }

        function GetRadWindow9()
        {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }
        function returnToParent3()
        {
            //create the argument that will be returned to the parent page
            var oArg = new Object();  
            //get a reference to the current RadWindow
            oArg.selLOINC = document.getElementById("TextBox1").value;
            var oWnd2 = GetRadWindow9();
            //Close the RadWindow and send the argument to the parent page
            if (oArg.selLOINC)
            {
                oWnd2.close(oArg);
            }
            else
            {
                alert("Please Select LOINC Codes");
            }
        }
