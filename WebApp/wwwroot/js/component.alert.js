var alertNormal = {
    title: '',
    content: '',
    color: { green: 'alert-success', red: 'alert-danger', blue: 'alert-info', warning: 'alert-warning' },
    linkClass: 'alert-link',
    iclass: { failed: 'fa fa-times-circle', warning: 'fa fa-exclamation-triangle', success: 'fa fa-check-circle', info: 'fa fa-info-circle' },
    type: { failed: -1, warning: 0, success: 1, info: 2 },
    parent: $('#alertmsg'),
    id: 'alert',
    div: '<div id={id} class="alert {color} alert-dismissible fade position-fixed alert-custom r-t" role="alert">'
        + '<strong > <i class="{iclass}"></i> {title}!</strong> {content}'
        + '<button type="button" class= "close pr-2" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button ></div>',
    alert: function (type) {
        var cls = this.color.blue;
        if (type === this.type.success) cls = this.color.green;
        else if (type === this.type.failed) cls = this.color.red;
        else if (type === this.type.warning) cls = this.color.warning;
        var icls = this.iclass.info;
        if (type === this.type.success) icls = this.iclass.success;
        else if (type === this.type.failed) icls = this.iclass.failed;
        else if (type === this.type.warning) icls = this.iclass.warning;
        this.parent.html(this.div.replace('{id}', this.id).replace('{title}', this.title).replace('{content}', this.content).replace('{color}', cls).replace('{iclass}', icls));
        this.show();
        if (this.autoClose > 0) {
            setTimeout(function () {
                alertNormal.close();
            }, this.autoClose * 1000);
        }
    },
    close: function () {
        $('#' + this.id).removeClass('show');
    },
    show: function () {
        $('#' + this.id).addClass('show');
    },
    autoClose: 0,
    remove: function () {
        $('#' + this.id).remove();
    }
};

var Alerts = function (m, t) {
    errorMsg.removeClass('text-danger text-success text-info');
    errorMsg.removeClass('hide');
    errorMsg.addClass(t === 0 ? 'text-danger' : t === 1 ? 'text-success' : 'text-info');
    errorMsg.text(m);

};

var alertContent = {
    title: '',
    content: '',
    color: { green: 'alert-success', red: 'alert-danger', blue: 'alert-info', warning: 'alert-warning' },
    linkClass: 'alert-link',
    type: { failed: -1, warning: 0, success: 1, info: 2 },
    parent: $('#alertmsg'),
    id: 'alert',
    div: `<div id={id} class="alert {color} alert-dismissible fade" role="alert">
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
            <span aria-hidden="true">&times;</span></button>
            <h4 class="alert-heading">{title}</h4 >
            <p>{content}</p>
          </div>`,
    alert: function (type) {
        var cls = this.color.blue;
        if (type === this.type.success)
            cls = this.color.green;
        else if (type === this.type.failed)
            cls = this.color.red;
        else if (type === this.type.warning)
            cls = this.color.warning;
        this.parent.html(this.div.replace('{id}', this.id).replace('{title}', this.title).replace('{content}', this.content).replace('{color}', cls));
        this.show();
    },
    close: function () {
        $('#' + this.id).removeClass('show');
    },
    show: function () {
        $('#' + this.id).addClass('show');
    }
};

var an = alertNormal;
var ac = alertContent;