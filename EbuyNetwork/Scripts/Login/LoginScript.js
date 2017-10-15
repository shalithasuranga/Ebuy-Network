$(document).ready(function () {

});

var ValidateForm = function () {

    var emailAddress= $('#jemailaddress').val();
    var password = $('#jpassword').val();
    var regx = /[\w]{2,}@[\w]{2,}\.[\w]{2,}/;


    if (!regx.test(emailAddress)) {
        BootstrapDialog.alert('Please enter valid email address');
        return false;
    }
    else if (password=="") {
        BootstrapDialog.alert('Please enter login password');
        return false;
    }
    else {
        return true;
    }
};
