$(document).on('click', '.addtowishlist', (e) => {
    let vId = $(e.currentTarget).data()?.variantId ?? 0;
     $.post("/AddWishList", { VariantID: vId }).done(res => {
        Q.notify(res.statusCode, res.responseText)
    }).fail(xhr => Q.notify(-1, xhr.responseText)).always(() => "")
});
$(document).on('click', '.addtocart', (e) => {
    let vId = $(e.currentTarget).data()?.variantId ?? 0;
    $.post("/AddToCart", { VariantID: vId }).done(res => {
        Q.notify(res.statusCode, res.responseText)
    }).fail(xhr => Q.notify(-1, xhr.responseText)).always(() => "")
});
$(document).on('click', '.openWishList', (e) => {
      $.post("/WishListSlide").done(res => {
          $('#wishlist_side').append(res);
          console.log(res);
        document.getElementById("wishlist_side").classList.add('open-side');
    }).fail(xhr => Q.notify(-1, xhr.responseText)).always(() => "");
});

//const openWishlist = async function () {
//    await $.post("/WishListSlide").done(res => {
//        $('#dvHotDealNewProduct').append(res);
//        document.getElementById("wishlist_side").classList.add('open-side');
//    }).fail(xhr => Q.notify(-1, xhr.responseText)).always(() => "");
//}