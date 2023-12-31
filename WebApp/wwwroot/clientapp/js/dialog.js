﻿var dialog;
((dialog) => {
    dialog.category = function (Id) {
        $.get('/Category/Create', { Id: Id })
            .done(result => {
                Q.alert({
                    title: 'Category',
                    body: result,
                    onClose: () => { __bind.dropDown.category(0, "#CategoryId")}
                });
            }).fail(xhr => Q.renderError(xhr)).always(() => Q.preloader.remove());
    };
    dialog.offer = function (Id) {
        $.get('/Offer/OfferCreate', { Id: Id })
            .done(result => {
                Q.alert({
                    title: 'Offer',
                    body: result,
                    onClose: () => { __bind.dropDown.category(0, "#CategoryId") }
                });
            }).fail(xhr => Q.renderError(xhr)).always(() => Q.preloader.remove());
    };
    dialog.brand = function (Id) {
        $.get('/Brand/Create', { Id: Id })
            .done(result => {
                Q.alert({
                    title: 'Brand',
                    body: result,
                    onClose: () => { __bind.dropDown.brand(0, "#BrandId") }
                });
            }).fail(xhr => Q.renderError(xhr)).always(() => Q.preloader.remove());
    };
    dialog.categoryAttributeMap = function (Id) {
        $.get('/CategoryAttributeMapping/_CategoryAttribute', { Id })
            .done(result => {
                Q.alert({
                    title: 'Category Attribute Mapping',
                    body: result,
                    onClose: () => { $('.ui-dialog-titlebar-close').click(); $('.btnAddNewAttribute').click(); }
                });
                $('.ui-dialog-titlebar-max:last').click();
            }).fail(xhr => Q.renderError(xhr)).always(() => Q.preloader.remove());
    };
    dialog.AddAttribute = function (Id) {
        $.get('/Attribute/Create', { Id: Id })
            .done(result => {
                Q.alert({
                    title: 'Attribute',
                    body: result,
                    onClose: () => { }
                });
            }).fail(xhr => Q.renderError(xhr)).always(() => Q.preloader.remove());
    };
    dialog.coupon = function (CouponId) {
        $.get('/Coupon/CreateCoupon', { CouponId: CouponId })
            .done(result => {
                Q.alert({
                    title: 'Coupon',
                    body: result
                });
            }).fail(xhr => Q.renderError(xhr)).always(() => Q.preloader.remove());
    };
    dialog.getAllcoupon = (paymentmode) => {
        $.post('/GetAllCoupon', { paymentmode: paymentmode })
            .done(result => {
                Q.alert({
                    title: 'COUPONS',
                    body: result,
                    //onClose: () => {
                    //    loadPlaceOrderCart();
                    //}
                });
            }).fail(xhr => Q.renderError(xhr)).always(() => Q.preloader.remove());
    };
})(dialog || (dialog = {}));

var __bind;
((__bind) => {
    __bind.dropDown = {
        category: function (id, selector) {
            $.post('/Category/CategoryJSON', { Id: id }).done(function (result) {
                if (selector) {
                    $(selector).html(`<option> Select Category </option>`).append(result.map(x => { return `<option value="${x.categoryId}"> ${x.categoryName} </option>` }))
                }
            }).fail(function (xhr) {
                console.log(xhr.responseText);
                alert('something went wrong');
            })
        },
        brand: function (id, selector) {
            $.post('/Brand/BrandJSON', { Id: id }).done(function (result) {
                if (selector) {
                    $(selector).html(`<option> Select Brand </option>`).append(result.map(x => { return `<option value="${x.id}"> ${x.name} </option>` }))
                }
            }).fail(function (xhr) {
                console.log(xhr.responseText);
                alert('something went wrong');
            })
        }
    }
})(__bind || (__bind = {}));