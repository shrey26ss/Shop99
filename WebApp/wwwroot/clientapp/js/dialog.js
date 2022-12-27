var dialog;
((dialog) => {
    dialog.category = function (Id) {
        $.get('/Category/Create', { Id: Id })
            .done(result => {
                Q.alert({
                    title: 'Category',
                    body: result
                });
            }).fail(xhr => Q.renderError(xhr)).always(() => Q.preloader.remove());
    };
    dialog.brand = function (Id) {
        $.get('/Brand/Create', { Id: Id })
            .done(result => {
                Q.alert({
                    title: 'Brand',
                    body: result
                });
            }).fail(xhr => Q.renderError(xhr)).always(() => Q.preloader.remove());
    };
    dialog.categoryAttributeMap = function (Id) {
        $.get('/CategoryAttributeMapping/_CategoryAttribute')
            .done(result => {
                Q.alert({
                    title: 'Brand',
                    body: result
                });
                $('.ui-dialog-titlebar-max:last').click();
            }).fail(xhr => Q.renderError(xhr)).always(() => Q.preloader.remove());
    };
})(dialog || (dialog = {}));