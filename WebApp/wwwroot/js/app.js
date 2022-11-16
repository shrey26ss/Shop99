(() => {
    $('body').on('click', 'input[type="checkbox"]', function () {
        if ($(this).prop('checked')) {
            $(this).attr('value', true);
        } else {
            $(this).attr('value', false);
        }
    });
    $('body').on('submit', 'form', function () {
        ajaxFormSubmit(this)
    });
})();
var dialog;
((dialog) => {
    dialog.emailConfig = function (id = 0) {
        $.post('/EmailConfigiration/Edit', { id: id }).done(result => {
            Q.alert({
                title: "Email Configiration",
                body: result,
                width: '450px',
            });
        }).fail(xhr => Q.renderError(xhr)).always(() => Q.preloader.remove());
    };
})(dialog || (dialog = {}));
var services;
var serviceProperty = {
    Add: {},
    Delete: {},
    Change: {},
    Detail: {},
    Dropdown: {}
};

((services) => {
    services.scheduleJob = function ({ startDate = '', endDate = '' } = { startDate: null, endDate: null }) {
        $.post('/Task/ScheduleStatusCheck')
            .done(result => { Q.notify(1, 'Job Scheduled successfully.'); }).fail(xhr => {
                console.log(xhr.responseText);
                Q.notify(-1, 'An error occurred.');
            }).always(() => { });
    };
    services.scheduleTestJob = function () {
        $.post('/Task/ScheduleTestTask')
            .done(result => { Q.notify(1, 'Job Scheduled successfully.'); }).fail(xhr => {
                console.log(xhr.responseText);
                Q.notify(-1, 'An error occurred.');
            }).always(() => { });
    };
    services.pauseTask = function () {
        $.post('/Task/PauseTask')
            .done(result => { Q.notify(1, 'Job paused successfully.'); }).fail(xhr => {
                console.log(xhr.responseText);
                Q.notify(-1, 'An error occurred.');
            }).always(() => { });
    };
    services.resumeTask = function () {
        $.post('/Task/ResumeTask')
            .done(result => { Q.notify(1, 'Job resumed successfully.'); }).fail(xhr => {
                console.log(xhr.responseText);
                Q.notify(-1, 'An error occurred.');
            }).always(() => { });
    };
    services.Dropdown = {
        users: param => new Promise((resolve, reject) => {
            param = $.extend({
                id: 0,
                role: 'all'
            }, param);
            $.post('/User/Dropdown', param).done(result => resolve(result)).fail(xhr => reject(new Error(xhr)));//'/Account/UsersDropdown'
        }),
    };
})(services || (services = serviceProperty));

var s = services;
var Dropdown = services.Dropdown;