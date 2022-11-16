var modalAlert = {
    title: '',
    content: '',
    confirmContent: '<h5>Are you sure?</h5>',
    parent: $('body'),
    id: 'mymodal',
    size: { small: 'modal-sm', large: 'modal-lg', xlarge: 'modal-xl', xxlarge: 'modal-xxl', xxlargeM: 'modal-xxl-m', default: '' },
    bodyCls: '',
    div: `<div class="modal fade" id={id} tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
            <div class= "modal-dialog modal-dialog-centered" role="document"><div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title"></h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                </div>
                <div class="modal-body"></div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary">Save changes</button>
                </div>
                </div>
            </div>
          </div>`,
    divAlert: '<div class="modal fade" id={id} tabindex="-1" role="dialog" aria-hidden="true">'
        + '<div class= "modal-dialog modal-dialog-centered" role="document">'
        + '<div class="modal-content"><div class="modal-body {bodyCls}"></div></div></div></div >',
    show: function (size, pos) {
        var mdlId = this.id;
        this.parent.append(this.div.replace('{id}', mdlId));
        $('#' + mdlId + ' .modal-title').html(this.title);
        $('#' + mdlId + ' .modal-body').html(this.content);
        $('#' + mdlId + ' .modal-dialog').addClass(size);
        $('#' + mdlId).modal(this.options);
    },
    alert: function (size) {
        var mdlId = this.id;
        this.parent.append(this.divAlert.replace('{id}', mdlId).replace('{bodyCls}', this.bodyCls));
        $('#' + mdlId + ' .modal-body').html(this.content);
        $('#' + mdlId + ' .modal-dialog').addClass(size);
        $('#' + mdlId).modal(this.options);
    },
    isHeaderBorder: true,
    headerClass: 'h5',
    callBack: '',
    modal: function (size, callBack) {
        var _html = `<div class="modal fade" id="${this.id}" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
            <div class= "modal-dialog modal-dialog-centered ${size}" role="document">
                <div class="modal-content">
                    <div class="${this.isHeaderBorder ? 'modal-header' : 'pl-3 pr-3 mt-2'} custome">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h3 class="${this.headerClass} modal-title">${this.title === "" ? 'Alert' : this.title}</h3> 
                    </div>
                    <div class="modal-body">${this.content}</div>
                </div>
            </div>
          </div>`
        this.parent.append(_html);
        $(`#${this.id}`).modal(this.options);
        $('button.close').unbind().click(() => {
            mdlA.dispose();
            if (callBack !== undefined) {
                callBack();
            }
        });
    },
    reset: function () {
        this.isHeaderBorder = true;
        this.title = "";
        this.headerClass = "h5";
        this.bodyCls = '';
    },
    options: { backdrop: 'static', keyboard: true, focus: true, show: true },
    dispose: function (f) {
        this.reset();
        //this.bodyCls = '';
        var mdlId = this.id;
        $('#' + mdlId + ' .modal-content').animate({ opacity: 0 }, 500, function () {
            $('#' + mdlId + ',.modal-backdrop').remove();
            $('body').removeClass('modal-open').removeAttr('style');
            if (f !== undefined)
                f();
        });
    },
    anim: function (ms) {
        $('#' + this.id + ' .modal-content').animate({ opacity: 0 }, ms);
        $('#' + this.id + ' .modal-content').animate({ opacity: 1 }, ms);
    },
    confirm: function () {
        return `<div class="col-md-12" id="dvpopup">
                    <button type = "button" class="close" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    ${this.confirmContent}
                    <div class="form-group">
                        <button class="btn btn-outline-success mr-2" id="btnOK">Yes</button>
                        <button class="btn btn-outline-danger" id="mdlCancel">No</button>
                    </div>
                </div>`;
    }
};

var modalStack = {
    title: '',
    content: '',
    confirmContent: '',
    parent: $('body'),
    id: 'mystack',
    size: { small: 'modal-sm', large: 'modal-lg', xlarge: 'modal-xl', xxlarge: 'modal-xxl', default: '' },
    div: '<div class="modal fade" id={id} tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">'
        + '<div class= "modal-dialog modal-dialog-centered" role="document"><div class="modal-content"><div class="modal-header">'
        + '<h5 class="modal-title"></h5><button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button></div><div class="modal-body"></div><div class="modal-footer">'
        + '<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>'
        + '<button type="button" class="btn btn-primary">Save changes</button></div></div></div></div >',
    divAlert: '<div class="modal fade" id={id} tabindex="-1" role="dialog" aria-hidden="true">'
        + '<div class= "modal-dialog modal-dialog-centered" role="document">'
        + '<div class="modal-content"><div class="modal-body"></div></div></div></div >',
    show: function (size, pos) {
        var mdlId = this.id;
        this.parent.append(this.div.replace('{id}', mdlId));
        $('#' + mdlId + ' .modal-title').html(this.title);
        $('#' + mdlId + ' .modal-body').html(this.content);
        $('#' + mdlId + ' .modal-dialog').addClass(size);
        $('#' + mdlId).modal(this.options);
    },
    alert: function (size) {
        var mdlId = this.id;
        this.parent.append(this.divAlert.replace('{id}', mdlId));
        $('#' + mdlId + ' .modal-body').html(this.content);
        $('#' + mdlId + ' .modal-dialog').addClass(size);
        $('#' + mdlId).modal(this.options);
    },
    options: { backdrop: true, keyboard: true, focus: true, show: true },
    dispose: function (f) {
        var mdlId = this.id;
        $('#' + mdlId + ' .modal-content').animate({ opacity: 0 }, 500, function () {
            $('#' + mdlId + ',.modal-backdrop').remove();
            $('body').removeClass('modal-open').removeAttr('style');
            if (f !== undefined)
                f();
        });
    },
    disposeConfirm: function (f) {
        var mdlId = this.id;
        $('#' + mdlId + ' .modal-content').animate({ opacity: 0 }, 500, function () {
            $('#' + mdlId).remove();
        });
    },
    anim: function (ms) {
        $('#' + this.id + ' .modal-content').animate({ opacity: 0 }, ms);
        $('#' + this.id + ' .modal-content').animate({ opacity: 1 }, ms);
    },
    confirm: function (confirmContent) {
        return `<div class="col-md-12" id="dvpopup">
            <button type = "button" class="close" aria-label="Close">
            <span aria-hidden="true">&times;</span></button>
                ${confirmContent}
            <div class="form-group"></div> <button class="btn btn-outline-success mr-2" id="btnOK">Yes</button>
            <button class="btn btn-outline-danger" id="mdlCancel">No</button></div>`
    },
    confirmStack: function (id, confirmCont) {
        return `<div class="col-md-12" id="${id}">
                     ${confirmCont}
                <div class="form-group"></div>
                <button class="btn btn-outline-success mr-2" id="btnOK">Yes</button>
                <button class="btn btn-outline-danger" id="mdlCancel${id}">No</button></div>`
    }
};

var mdlA = modalAlert;