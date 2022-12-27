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
                    onClose: () => { dialog.bindBrands() }
                });
            }).fail(xhr => Q.renderError(xhr)).always(() => Q.preloader.remove());
    };
    dialog.bindCategory = function () {
        $.post('/Product/GetCategories').done((result) => {
            var unique = $.map($('#CategoryId option'), function (option) {
                return parseInt(option.value);
            });
            $.each(result, function (i, j) {
                if (!unique.includes(parseInt(j.categoryId))) {
                    $('#CategoryId').append(`<option value="${j.categoryId}">${j.categoryName}</option>`);
                }
            });
        });
    },
        dialog.bindBrands = function () {
        $.post('/Product/GetBrands').done((result) => {
            var unique = $.map($('#BrandId option'), function (option) {
                return parseInt(option.value);
            });
            $.each(result, function (i,j) {
                if (!unique.includes(parseInt(j.id))) {
                    $('#BrandId').append(`<option value="${j.id}">${j.name}</option>`);
                }
            });
        });
    }
})(dialog || (dialog = {}));