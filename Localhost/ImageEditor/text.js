// Copyright 2009  LLC All Rights Reserved.

/**
* @fileoverview This file contains function to get the mouse position and write text at the specific location
* '$' sign defines jQuery
* (selector) is used to "query (or find)" HTML elements.Selectors used here are the id's #txt and #canvas
* jQuery action() defines the function to be performed on the element or elements.
* @author prabhakar.ranjan@.com (Prabhakar Ranjan)
*/

$(document).ready(function () {  //To prevent any jQuery code from running before the document is finished loading or is ready.
    $("#txt").click(function () {
        var drawtext = false;
        $('#canvas').mousedown(function () { drawtext = true; });
        $('#canvas').mousedown(function (e) {
            undoSave();
            if (drawtext) {
                var x, y;
                //Return the x and y coordinates
                x = e.layerX;
                y = e.layerY;
                context.font = " " + document.getElementById("textsize").value + "pt Arial ";
                context.fillStyle = "Black";
                context.fillText(document.getElementById('txt').value, x, y);

            }
        });

    });
});