//$(function () {
//    $('[data-toggle="popover"]').popover();
//});

//$('.popover-dismiss').popover({
//    trigger: 'focus'
//});

$(function () {
    var products = $('.removeProduct');
    $(products).each(function () {
        var prod = this;
        $(this).confirm({
            theme: 'modern',
            icon: 'fa fa-smile-o text-info',
            title: 'Confirm!',
            draggable: true,
            dragWindowBorder: true,
            content: 'Simple confirm!',
            buttons: {
                confirm: {
                    text: 'delete',
                    btnClass: 'btn-danger',
                    keys: ['enter', 'shift'],
                    action: function () {
                        removeProduct($(prod));
                        $.alert('OK');
                    }
                },
                cancel: function () {
                    //$.alert('Canceled!');
                }
            }
        });
    });
});


function removeProduct(productButton) {
    var button = productButton[0];
    var cartRow = $(button).parents('li')[0];
    console.log("I'm HERE! HELP ME PLEASE!");
    var prodName = getProdName(cartRow);
    var prodSupp = getProdSupp(cartRow);
    var prodSuppAddress = getProdSuppAddress(cartRow);

    var model = {
        productName: prodName,
        supplier: prodSupp,
        addressSupplier: prodSuppAddress
    };
    console.log(model);

    $.ajax({
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        url: "../api/ajax/removeProduct", // replace 'PHP-FILE.php with your php file
        data: JSON.stringify(model),
        success: function (data) {
            console.log('vlid!');
            $(cartRow).remove();
        },
        error: function () {
            console.log('Some error occurred!');
        }
    });
}


function getProdName(block) {
    var name = $(block).data("productname");
    return name;
}


function getProdSupp(block) {
    let name = $(block).data("suppliername");
    return name;
}


function getProdSuppAddress(block) {
    let name = $(block).data("supplieraddress");
    return name;
}