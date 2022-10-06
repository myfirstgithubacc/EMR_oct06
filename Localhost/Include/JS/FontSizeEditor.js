function FontSizeRadEditor(editor, args) {
    var tool = editor.getToolByName("RealFontSize");
    if (tool && !$telerik.isIE) {
        setTimeout(function() {
            var value = tool.get_value();

            switch (value) {
                case "8px":
                    value = value.replace("8px", "10px");
                    break;
                case "9px":
                    value = value.replace("9px", "10px");
                    break;
                case "10px":
                    value = value.replace("10px", "10px");
                    break;
                case "11px":
                    value = value.replace("11px", "10px");
                    break;
                case "12px":
                    value = value.replace("12px", "13px");
                    break;
                case "13px":
                    value = value.replace("13px", "13px");
                    break;
                case "14px":
                    value = value.replace("14px", "13px");
                    break;
                case "16px":
                    value = value.replace("16px", "16px");
                    break;
                case "18px":
                    value = value.replace("18px", "18px");
                    break;
                case "20px":
                    value = value.replace("20px", "18px");
                    break;
                case "22px":
                    value = value.replace("22px", "18px");
                    break;
                case "24px":
                    value = value.replace("24px", "24px");
                    break;
                case "26px":
                    value = value.replace("26px", "24px");
                    break;
                case "28px":
                    value = value.replace("28px", "24px");
                    break;
                case "32px":
                    value = value.replace("32px", "32px");
                    break;
                case "36px":
                    value = value.replace("36px", "32px");
                    break;
                case "48px":
                    value = value.replace("48px", "48px");
                    break;
                case "72px":
                    value = value.replace("72px", "48px");
                    break;
            }
            tool.set_value(value);
        }, 0);
    }
}