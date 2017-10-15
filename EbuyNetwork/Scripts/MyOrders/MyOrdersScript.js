$(document).ready(function () {
    $('#startDate,#endDate').datepicker({
        format: 'yyyy-mm-dd'
    });
});




var cancelOrder = function (orderId,itemId,userId) {
    BootstrapDialog.show({
        title: 'Confirm cancel order',
        message: 'Are you sure you want to mark this order as cancelled?<br/>This action cannot be undone',
        buttons: [{
            label: 'Yes, I need to cancel',
            cssClass: 'btn-danger',
            action: function (dialog) {
                $.ajax({
                    url: 'AjaxCancelOrder',
                    type: 'POST',
                    data: {
                        "order_id": orderId,
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

