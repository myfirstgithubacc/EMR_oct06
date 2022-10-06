var lastFocusedControlId = "";
var selectionEnd = 0;
var range = 0;

function focusHandler(e) {
    document.activeElement = e.originalTarget;
}

function appInit() {
    if (typeof (window.addEventListener) !== "undefined") {
        window.addEventListener("focus", focusHandler, true);
    }
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoading(pageLoadingHandler);
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(pageLoadedHandler);
}

function pageLoadingHandler(sender, args) {
    lastFocusedControlId = typeof (document.activeElement) === "undefined" ? "" : document.activeElement.id;
    selectionEnd = typeof (document.activeElement) === "undefined" ? 0 : document.activeElement.selectionEnd;
}

function focusControl(targetControl) {
    if (Sys.Browser.agent === Sys.Browser.InternetExplorer) {
        var focusTarget = targetControl;
        if (focusTarget && (typeof (focusTarget.contentEditable) !== "undefined")) {
            oldContentEditableSetting = focusTarget.contentEditable;
            focusTarget.contentEditable = false;
        }
        else {
            focusTarget = null;
        }
        try {
            range = targetControl.value.length;
            if (selectionEnd != 0) {

                targetControl.setSelectionRange(selectionEnd, selectionEnd);
            }
            else {
                targetControl.setSelectionRange(targetControl.value.length, targetControl.value.length);
            }

            targetControl.focus();
        }
        catch (error) { }

        if (focusTarget) {
            focusTarget.contentEditable = oldContentEditableSetting;
        }
    }
    else {
        //if (targetControl.value.length !== "undefined" || targetControl.value.length !== null)
        //targetControl.setSelectionRange(targetControl.value.length, targetControl.value.length);
        //targetControl.focus();
        try {
            range = targetControl.value.length;
            if (selectionEnd != 0) {

                targetControl.setSelectionRange(selectionEnd, selectionEnd);
            }
            else {
                targetControl.setSelectionRange(targetControl.value.length, targetControl.value.length);
            }
            targetControl.focus();
        }
        catch (error) { }
    }
}

function pageLoadedHandler(sender, args) {
    if (typeof (lastFocusedControlId) !== "undefined" && lastFocusedControlId != "") {
        var newFocused = $get(lastFocusedControlId);
        if (newFocused) {
            focusControl(newFocused);
        }
    }
}

Sys.Application.add_init(appInit);
