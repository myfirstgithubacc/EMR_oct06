function keyDown(txtCtl,btnCtl) {
    if (event.keyCode == 13) {
            $get(btnCtl).click();
            return false;
      }
}
function Focus(ths) {
    ths.style.backgroundColor = '#EFF3FB';
}
function Blur(ths) {
    ths.style.backgroundColor = 'white';
}
function Tab() {
    if (event.keyCode == 13)
        event.keyCode = 9;
}
