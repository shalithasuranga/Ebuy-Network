$(document).ready(function () {

});

var ValidateForm = function () {

    var name = $('#jname').val();
    var category = $('#jcategory').val();
    var price = $('#jprice').val();
    var description = $('#jdescription').val();
    var image= $('#jimage').val();

    if (name == "") {
        BootstrapDialog.alert('Please enter product name');
        return false;
    }
    else if (category == "0") {
        BootstrapDialog.alert('Please select category');
        return false;
    }

    else if (parseFloat(price)<=0 || isNaN(parseFloat(price))) {
        BootstrapDialog.alert('Please enter item price');
        return false;
    }
    else if (description=="") {
        BootstrapDialog.alert('Please enter description');
        return false;
    }
    else if (image == "") {
        BootstrapDialog.alert('Please select image');
        return false;
    }
    else if (!$('#jacceptterms').prop("checked")) {
        BootstrapDialog.alert('Please accept our terms and conditions before post item');
        return false;
    }
    else {
        return true;
    }
};
