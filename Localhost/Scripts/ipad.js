$(document).ready(function () {
    var isiPad = navigator.userAgent.match(/iPad/i) != null;
    if (isiPad) {
        ChangeHeight();
        $(window).resize(function () {
            ChangeHeight();
        });
    }
    function ChangeHeight() {
        var height = $(window).height();
        var width = $(window).width();

        //Ipad_hp
        if (width > height) {
            $('#Ipad_hp').css('height', '430px');
        }
        else {
            $('#Ipad_hp').css('height', '900px');
        }
        $('#Ipad_hp').css('overflow', 'auto');

        //Ipad_Anesthesia
      
        if (width > height) {
            $('#Ipad_Anesthesia').css('height', '380px');
        } else {
            $('#Ipad_Anesthesia').css('height', '870px');
        }
        $('#Ipad_Anesthesia').css('overflow', 'auto');
     
        //Ipad_AdmissionForm
        if (width > height) {
            $('#Ipad_AdmissionForm').css('height', '420px');
        } else {
            $('#Ipad_AdmissionForm').css('height', '900px');
        }
        $('#Ipad_AdmissionForm').css('overflow', 'auto');


        //Ipad_SurgeryEstimate
        if (width > height) {
            $('#Ipad_SurgeryEstimate').css('height', '420px');
        } else {
            $('#Ipad_SurgeryEstimate').css('height', '950px');
        }
        $('#Ipad_SurgeryEstimate').css('overflow', 'auto');

        //Ipad_OrderSet
        if (width > height) {
            $('#Ipad_OrderSet').css('height', '380px');
        } else {
            $('#Ipad_OrderSet').css('height', '850px');
        }
        $('#Ipad_OrderSet').css('overflow', 'auto');

        //Ipad_ViewPatientScore
        if (width > height) {
            $('#Ipad_ViewPatientScore').css('height', '200px');
        } else {
            $('#Ipad_ViewPatientScore').css('height', '500px');
        }
        $('#Ipad_ViewPatientScore').css('overflow', 'auto');

        //Ipad_OPDResearch
        if (width > height) {
            $('#Ipad_OPDResearch').css('height', '440px');
        } else {
            $('#Ipad_OPDResearch').css('height', '850px');
        }
        $('#Ipad_OPDResearch').css('overflow', 'auto');
    }


});