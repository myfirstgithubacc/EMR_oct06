

    $('#AddToFavorites').click(function () {
        $.ajax({
            url: "Services/Cpoe_CommonServices.asmx/SaveFavoritePage",
            data: '{MenuName:"Diagnosis",PageUrl:"PatientICDCodes.aspx"}',
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (mydata) {


                if (mydata.d == "True") {
                    $('#Msg').text('Page Add To Favorites Menu');
                }
                else {
                    $('#Msg').text('Error');
                }
            }

        });
    });


    function AddPage(Pageurl, MenuName) {
        $.ajax({
            url: "Services/Cpoe_CommonServices.asmx/SaveFavoritePage",
            data: '{MenuName:"' + MenuName + '",PageUrl:"' + Pageurl + '"}',
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (mydata) {
                if (mydata.d == "True") {
                    alert('Page Add To Favorites Menu');
                    window.parent.parent.frames[0].Menu.location.reload();
                }
                else {
                    alert('Error');
                }
            }

        });
    }
