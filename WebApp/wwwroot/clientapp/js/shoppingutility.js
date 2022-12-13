$(document).ready(function () {
    cartWishListCount();
});
$(document).on('click', '.addtowishlist', (e) => {
    let vId = $(e.currentTarget).data()?.variantId ?? 0;
    $.post("/AddWishList", { VariantID: vId }).done(res => {
        Q.notify(res.statusCode, res.responseText)
        cartWishListCount();
    }).fail(xhr => Q.notify(-1, xhr.responseText)).always(() => "")
});
$(document).on('click', '.addtocart', (e) => {
    let vId = $(e.currentTarget).data()?.variantId ?? 0;
    $.post("/AddToCart", { VariantID: vId }).done(res => {
        Q.notify(res.statusCode, res.responseText)
        if (res.statusCode == 1) {
            loadCartDetails();
            loadCartSlide();
        }
    }).fail(xhr => Q.notify(-1, xhr.responseText)).always(() => "")

});
$(document).on('click', '.move-to-cart', (e) => {
    let wId = $(e.currentTarget).data()?.wishlistId ?? 0;
    $.post("/WishListToCart", { id: wId }).done(res => {
        Q.notify(res.statusCode, res.responseText)
        if (res.statusCode == 1) {
            cartWishListCount();
            loadWishListSlide();
        }
    }).fail(xhr => Q.notify(-1, xhr.responseText)).always(() => "")
});
$(document).on('click', '.qty-minus', (e) => {
    let vId = $(e.currentTarget).data()?.variantId ?? 0;
    $.post("/AddToCart", { VariantID: vId, Qty: -1 }).done(res => {

        Q.notify(res.statusCode, res.responseText = res.statusCode == 1 ? "Cart Item Removed" : res.responseText)
        if (res.statusCode == 1) {
            loadCartSlide();
            loadCartDetails();
        }
    }).fail(xhr => Q.notify(-1, xhr.responseText)).always(() => "")
});

$(document).on('click', '.delete-cart', (e) => {
    let vId = $(e.currentTarget).data()?.variantId ?? 0;
    Q.confirm("Are Your Sure Delete Cart Items", () => {
        $.post("/DeleteCart", { VariantID: vId, Qty: -1 }).done(res => {
            Q.notify(res.statusCode, res.responseText = res.statusCode == 1 ? "Cart Item Removed" : res.responseText)
            if (res.statusCode == 1) {
                loadCartDetails();
                loadCartSlide();
            }
        }).fail(xhr => Q.notify(-1, xhr.responseText)).always(() => "")
    })

});
$(document).on('click', '.delete-cart-slide', (e) => {
    let vId = $(e.currentTarget).data()?.variantId ?? 0;
    $.post("/DeleteCart", { VariantID: vId, Qty: -1 }).done(res => {
        Q.notify(res.statusCode, res.responseText = res.statusCode == 1 ? "Cart Item Removed" : res.responseText)
        if (res.statusCode == 1) {
            loadCartDetails();
            loadCartSlide();
        }
    }).fail(xhr => Q.notify(-1, xhr.responseText)).always(() => "")
});

$(document).on('click', '.openWishList', (e) => {
    loadWishListSlide();
});
$(document).on('click', '.openCartSlide', (e) => {
    loadCartSlide();
    $('#cart_side')?.addClass('open-side');
});
const loadCartSlide = function () {
    $.post("/CartSlide").done(res => {
        $('#cart_side').html(res);
        //document.getElementById("cart_side").classList.add('open-side');
        cartWishListCount();
    }).fail(xhr => Q.notify(-1, xhr.responseText)).always(() => "");
};


const loadCartDetails = async function () {
    await $.post("/_CartDetails").done(res => {
        $('#dvCartDetails').html(res);
        cartWishListCount();
    }).fail(xhr => Q.notify(-1, xhr.responseText)).always(() => "");
}

const cartWishListCount = function () {
    $.post("/CartWishListCount").done(res => {
        if (res.statusCode == 1) {
            $.each(res.result, async function (i, v) {
                if (v.type == 'C') {
                    $('.cart-count').html(v.items);
                }
                if (v.type == 'W') {
                    $('.wishlist-count').html(v.items);
                }
            });
        }
        else {
            $('.cart-count').html(0);
            $('.wishlist-count').html(0);
        }
       
    }).fail(xhr => Q.notify(-1, xhr.responseText)).always(() => "");
};
const loadWishListSlide = function () {
    $.post("/WishListSlide").done(res => {
        $('#wishlist_side').html(res);
        document.getElementById("wishlist_side").classList.add('open-side');
    }).fail(xhr => Q.xhrError(xhr)).always(() => "");
};
