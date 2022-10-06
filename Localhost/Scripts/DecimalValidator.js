$(document).ready(function () 
{
    if ($(".ItDoseTextinputNum")) {
        $(".ItDoseTextinputNum").keypress(function (e) {         
            strLen = $(this).val().length;
            strVal = $(this).val();
            hasDec = false;
            e = (e) ? e : (window.event) ? event : null;
            if (e) {
                var charCode = (e.charCode) ? e.charCode :
                                ((e.keyCode) ? e.keyCode :
                                ((e.which) ? e.which : 0));
                
                if ((charCode == 46) || (charCode == 110) || (charCode == 190) ||(charCode==8)) {
                    for (var i = 0; i < strLen; i++) {
                        hasDec = (strVal.charAt(i) == '.');
                        if (hasDec)
                            return false;
                    }
                }
            }
            return true;
        });
        $(".ItDoseTextinputNum").keyup(function (e) {
            var keycode = e.keyCode ? e.keyCode : e.which;
            var keynum;
            if (window.event) {
                keynum = e.keyCode;
            }
            else if (e.which) {
                keynum = e.which;
            }
            if (keycode === 8) { // backspace
                if ($(this).val() == '0') {
                    $(this).val('');
                    return false;
                }
            }
            if (keycode === 46) { // delete
                if ($(this).val() == "0.") {
                    $(this).val('');
                    return false;
                }
            }
            if (($(this).val().charAt(0) == ".")) {
                $(this).val('0.');
                return false;
            }
            
            
            if ((keycode >= "48" && keycode <= "57") ||(keycode ===190)) {


                var DigitsAfterDecimal = 2;
                val = $(this).val();
                var valIndex = val.indexOf(".");
                if (valIndex > "0") {


                    if (val.length - (val.indexOf(".") + 1) > DigitsAfterDecimal) {
                        alert("Please Enter Valid Discount Percent, Only " + DigitsAfterDecimal + " Digits Are Allowed After Decimal");
                        $(this).val($(this).val().substring(0, ($(this).val().length - 1)));
                        return false;

                    }
                }
            }
        });
    }

});