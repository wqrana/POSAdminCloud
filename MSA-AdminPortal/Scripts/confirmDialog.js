function showConfirmDialog(confirmationText, btn1Text, btn2Text, btn1Function, btn2Function, headertext) {

    //Resetting Button Events
    $("#btn1").off('click');
    $("#btn2").off('click');

    //Assigning Text
    $('#h4text').html(confirmationText);
    $('#btn1').html(btn1Text);
    $('#btn2').html(btn2Text);
    $('#h4text').html(confirmationText);

    if (typeof headertext != "undefined") {
        $('#span-del-title').html(headertext);
    }

    //Assigning Button Events
    $("#btn1").on('click', btn1Function)
    $("#btn2").on('click', btn2Function)

    //Event Execute on this page to close the Modal
    //$("#btn1").on('click', closeModal)
    //$("#btn2").on('click', closeModal)



    $('#ConfirmDialog').modal('show');
}

//function closeModal() {
//    $('#ConfirmDialog').modal('hide');
//}