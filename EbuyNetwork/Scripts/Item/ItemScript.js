$(document).ready(function () {

});

var removeItem = function (itemId) {
    BootstrapDialog.show({
        title: 'Confirm remove',
        message: 'Are you sure you want to remove this item?',
        buttons: [{
            label: 'Yes, remove',
            cssClass: 'btn-danger',
            action: function (dialog) {
                $.ajax({
                    url: 'AjaxRemoveItem',
                    type : 'POST',
                    data: {
                        "remove_id": itemId
                    },
                    type: 'POST',
                    success: function (r) {
                        window.location = 'MyItems';
                    }
                });
            }
        }, {
            label: 'No',
            action: function (dialog) {
                dialog.close();
            }
        }]
    });
};


var orderItem = function (item_id,user_id) {

    var paymentcode = $('#jpaymentcode').val();
    if (paymentcode == "") {
        BootstrapDialog.alert('Please enter payment code');
    }
    else {

        $.ajax({
            url: 'AjaxOrderItem',
            data: {
                "item_id": item_id,
                "payment_code": paymentcode,
                "user_id":user_id
            },
            type: 'POST',
            success: function (r) {
                window.location = 'MyOrders';
            }
        });
    }
};


