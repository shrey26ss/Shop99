$(document).on('click', '.addtowishlist', (e) => {
    let vId = $(e.currentTarget).data()?.variantId ?? 0;
     $.post("/AddWishList", { VariantID: vId }).done(res => {
        Q.notify(res.statusCode, res.responseText)
    }).fail(xhr => Q.notify(-1, xhr.responseText)).always(() => "")
});
