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
                        location.reload();
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


var deliverItem = function (itemId,userId) {
    BootstrapDialog.show({
        title: 'Confirm deliver',
        message: 'Are you sure you want to mark this item as delivered?<br/>This action cannot be undone',
        buttons: [{
            label: 'Yes, item was delivered',
            cssClass: 'btn-success',
            action: function (dialog) {
                $.ajax({
                    url: 'AjaxDeliverItem',
                    type: 'POST',
                    data: {
                        "item_id": itemId,
                        "user_id" : userId
                    },
                    type: 'POST',
                    success: function (r) {
                        location.reload();
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


var showPaymentInfo = function (item_id) {

    $('#paymentInfoDialog').modal('show');

    $.ajax({
        url: 'AjaxGetPaymentInfo',
        type: 'POST',
        data: {
            "item_id" : item_id
        },
        success: function (r) {
            $('#jpaymentInfoCon').html(r);
        }
    });
};