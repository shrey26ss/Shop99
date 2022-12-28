var dialog;
((dialog) => {
    dialog.category = function (Id) {
        $.get('/Category/Create', { Id: Id })
            .done(result => {
                Q.alert({
                    title: 'Category',
                    body: result,
                    onClose: () => { dialog.bindCategory() }
                });
            }).fail(xhr => Q.renderError(xhr)).always(() => Q.preloader.remove());
    };
    dialog.brand = function (Id) {
        $.get('/Brand/Create', { Id: Id })
            .done(result => {
                Q.alert({
                    title: 'Brand',
                    body: result,
                    onClose: () => { __bind.dropDown.category(0, "#CategoryId") }
                });
            }).fail(xhr => Q.renderError(xhr)).always(() => Q.preloader.remove());
    };
    dialog.bindBrands = function () {
        $.post('/Product/GetBrands').done((result) => {
            var unique = $.map($('#BrandId option'), function (option) {
                return parseInt(option.value);
            });
            $.each(result, function (i, j) {
                if (!unique.includes(parseInt(j.id))) {
                    $('#BrandId').append(`<option value="${j.id}">${j.name}</option>`);
                }
            });
        });
    };
})(dialog || (dialog = {}));

var __bind;
((__bind) => {
    __bind.dropDown = {
        category : function (id, selector) {
            $.post('/Category/CategoryJSON', { Id: id }).done(function (result) {
                if (selector) {
                    $(selector).html(`<option> Select Category </option>`).append(result.map(x => { return `<option value="${x.categoryId}"> ${x.categoryName} </option>` }))
                }
            }).fail(function (xhr) {
                console.log(xhr.responseText);
                alert('something went wrong');
            })
        }
    }
})(__bind || (__bind = {}));