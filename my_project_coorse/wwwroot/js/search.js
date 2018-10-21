//var img = $('.placeholder-img');
//Holder.setResizeUpdate(img, true);

$("#search-bar").keyup(function () { // this isn't nessesery event
    const searchText = $('#search-bar').val();

    $.ajax({
        type: "GET",
        dataType: "json",
        url: "../api/ajax/getGoods", // replace 'PHP-FILE.php with your php file
        data: { search: searchText },
        success: function (data) {
            if (data == null) {
                $('.goods-not-found').show();
                $('#goods-append').html('');
                console.log("clear data");
                return
            } else {
                $('.goods-not-found').hide();

                $('#goods-append').html('');
                data.forEach(function (index, preparation) {
                    AddTo($('#goods-append'), preparation);
                });
            }
},
    error: function () {
        $('.goods-not-found').show();
        console.log('Some error occurred! search text = ' + searchText);
    }
    });
});

function AddTo(block, elem) {
    //    var div = document.createElement('div');
    //    //div.className = "alert alert-success";
    //    div.innerHTML = `
    //`;

    block.append(`<div class="col col-4 good">
                         <div class="card mb-3 shadow-sm">
                             <div class="card-header">
                                ` + elem.Name + `
                             </div>
                             @if (good.Product.ImageURL == "")
                             {
                             <img class="card-img-top" style="width: 100%; height: 225px; display: block;" src="holder.js/300x225?auto=yes&text=picture%20is%20absent">
                             }
                             else
                             {
                             <img class="card-img-top" style="width: 100%; height: 225px; display: block;" src="good.Product.ImageURL">
                             }
                             <div class="card-body">
                                 <p class="card-text">This is a wider card with supporting text below as a natural lead-in to additional content. This content is a little bit longer.</p>
                                 <div class="d-flex justify-content-between align-items-center">
                                     <div class="btn-group">
                                         <button class="btn btn-sm btn-outline-secondary" type="button">View</button>
                                         <button class="btn btn-sm btn-outline-secondary" type="button">Edit</button>
                                     </div>
                                     <small class="text-muted">9 mins</small>
                                 </div>
                             </div>
                         </div>
                     </div>`);
}