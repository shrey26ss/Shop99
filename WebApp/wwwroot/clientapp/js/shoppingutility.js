$(document).ready(function () {
    cartWishListCount();
});
$(document).on('click', '.addtowishlist', (e) => {
    let vId = $(e.currentTarget).data()?.variantId ?? 0;
    addToWishList(vId);
});
$(document).on('click', '.addtocart', (e) => {
    let vId = $(e.currentTarget).data()?.variantId ?? 0;
    addToCart(vId);
});


const addToWishList = (vId) => {
    $.post("/AddWishList", { VariantID: vId }).done(res => {
        Q.notify(res.statusCode, res.responseText)
        cartWishListCount();
    }).fail(xhr => Q.xhrError(xhr)).always(() => "")
}

const addToCart = (vId, Qty) => {
    let param = { VariantID: vId };
    if (Qty) {
        param["Qty"] = Qty
    };
    $.post("/AddToCart", param).done(res => {
        Q.notify(res.statusCode, res.statusCode == -1 ? res.responseText : Qty < 0 ? "Item removed from cart." : "Item Added in cart")
        if (res.statusCode == 1) {
            loadCartDetails();
            loadCartSlide();
        }
    }).fail(xhr => Q.xhrError(xhr)).always(() => "")
}

$(document).on('click', '.move-to-cart', (e) => {
    let wId = $(e.currentTarget).data()?.wishlistId ?? 0;
    $.post("/WishListToCart", { id: wId }).done(res => {
        Q.notify(res.statusCode, res.responseText)
        if (res.statusCode == 1) {
            cartWishListCount();
            loadWishListSlide();
        }
    }).fail(xhr => Q.xhrError(xhr)).always(() => "")
});

$(document).on('click', '.qty-minus-cart', (e) => {
    let vId = $(e.currentTarget).data()?.variantId ?? 0;
    if (vId === 0) {
        vId = $('#hdVid').val();
    }
    addToCart(vId, -1);
});

$(document).on('click', '.delete-cart', (e) => {
    let vId = $(e.currentTarget).data()?.variantId ?? 0;
    Q.confirm("Are Your Sure Delete Cart Items", () => {
        $.post("/DeleteCart", { VariantID: vId, Qty: -1 }).done(res => {
            Q.notify(res.statusCode, res.responseText = res.statusCode == 1 ? "Cart Items Removed" : res.responseText)
            if (res.statusCode == 1) {
                loadCartDetails();
                loadCartSlide();
            }
        }).fail(xhr => Q.xhrError(xhr)).always(() => "")
    })

});
$(document).on('click', '.delete-cart-slide', (e) => {
    let vId = $(e.currentTarget).data()?.variantId ?? 0;
    $.post("/DeleteCart", { VariantID: vId, Qty: -1 }).done(res => {
        Q.notify(res.statusCode, res.responseText = res.statusCode == 1 ? "Cart Items Removed" : res.responseText)
        if (res.statusCode == 1) {
            loadCartDetails();
            loadCartSlide();
        }
    }).fail(xhr => Q.xhrError(xhr)).always(() => "")
});

$(document).on('click', '.openWishList', (e) => {
    loadWishListSlide();
});
$(document).on('click', '.openCartSlide', (e) => {
    loadCartSlide();
    $('#cart_side')?.addClass('open-side');
});
const DeleteWishListTitem = (wId) => {
    $.post("/DeleteWishList", { id: wId }).done(res => {
        Q.notify(res.statusCode, res.responseText)
        if (res.statusCode == 1) {
            window.location.reload();
        }
    }).fail(xhr => Q.xhrError(xhr)).always(() => "")
}
const MoveToAll = () => {
    $.post("/MoveAllToCart").done(res => {
        Q.notify(res.statusCode, res.responseText)
        if (res.statusCode == 1) {
            setTimeout(window.location.reload(),4000);
        }
    }).fail(xhr => Q.xhrError(xhr)).always(() => "")
}
const loadCartSlide = function () {
    $.post("/CartSlide").done(res => {
        $('#cart_side').html(res);
        //document.getElementById("cart_side").classList.add('open-side');
        cartWishListCount();
    }).fail(xhr => Q.xhrError(xhr)).always(() => "");
};


const loadCartDetails = async function () {
    await $.post("/_CartDetails").done(res => {
        $('#dvCartDetails').html(res);
        cartWishListCount();
    }).fail(xhr => Q.xhrError(xhr)).always(() => "");
}

const cartWishListCount = function () {
    $.post("/CartWishListCount").done(res => {
        if (res.statusCode == 1) {
            $.each(res.result, async function (i, v) {
                if (v.type == 'C') {
                    $('.cart-count').html(v.items == 0 ? "" : v.items);
                    $('.cart-count').addClass(v.items == 0 ? "" : 'item-count-contain');

                }
                if (v.type == 'W') {

                    $('.wishlist-count').html(v.items == 0 ? "" : v.items);
                    $('.wishlist-count').addClass(v.items == 0 ? "" : 'item-count-contain');
                }
            });
        }
        else {
            $('.cart-count').removeClass('item-count-contain');
            $('.wishlist-count').removeClass('item-count-contain');
        }

    }).fail(xhr => Q.xhrError(xhr, false)).always(() => "");
};

const loadWishListSlide = function () {
    $.post("/WishListSlide").done(res => {
        $('#wishlist_side').html(res);
        document.getElementById("wishlist_side").classList.add('open-side');
    }).fail(xhr => Q.xhrError(xhr)).always(() => "");
};

