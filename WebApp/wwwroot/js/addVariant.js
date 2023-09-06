variantImages = [];

$('[name="SellingCost"], [name="MRP"]').numeric({ numericType: 'decimal', maxLength: '7' });
Q.initEditor('#specification');

const removeDuplicates = function (arr) {
    return arr.filter((item, index) => arr.indexOf(item) === index);
};

const isPartOfArray = (a, b) => {
    a = a.sort();
    b = b.sort();
    return a.join().toLocaleLowerCase().includes(b.join().toLocaleLowerCase())
};

const selectedColors = () => {
    let colorList = [];
    $('.attributeGrid').each(function () {
        colorList.push($(this).find('[data-attr-name="color"]').data()?.attrValue)
    })
    return removeDuplicates(colorList);
};

$(document).on('change', 'input', e => {
    let ele = $(e.currentTarget);
    ele.attr('value', ele.val())
});



$(document).on('click', '#SaveProduct', () => {
    let groupInfo = [];
    let attrInfo = [];
    let pictureInfo = [];
    
    let isImgUploaded = true;
    $('#attrColor').find('option').each(function () {
        let tdCounts = $(".pictureGrid tbody").find("td:contains('" + $(this).text() + "')").length;
        if (tdCounts == 0) {
            isImgUploaded = false;
            return;
        }
    });
    if (!isImgUploaded) {
        Q.notify(-1, 'Images are still pending for some colors');
        return
    }

    __attributes.map(x => x.attributes.map((y, i) => attrInfo.push({
        Id: i,
        AttributeId: y.id,
        AttributeValue: y.value,
        GroupId: x.groupId
    })));


    $('.tblgroupinfo tr').each(function (i) {
        let currentGrouptbl = $(this);
        var groupDetail = {
            id: i + 1,
            Title: currentGrouptbl.find("td:eq(1)").text(),
            MRP: parseInt(currentGrouptbl.find("td:eq(2)").text()),
            SellingCost: parseInt(currentGrouptbl.find("td:eq(3)").text()),
            Quantity: parseInt(currentGrouptbl.find("td:eq(4)").text()),
            ReturnInDays: parseInt(currentGrouptbl.find("td:eq(5)").text()),
            Warranty: parseInt(currentGrouptbl.find("td:eq(6)").text()),
            WarrantyUnit: currentGrouptbl.find("td:eq(7)").text(),
            IsFeatured: currentGrouptbl.find("td:eq(8)").text(),
            IsShowOnHome: currentGrouptbl.find("td:eq(9)").text(),
            Specification: currentGrouptbl.find("td:eq(10)").html()
        };
        groupInfo.push(groupDetail);
    });
    $('.pictureGrid tbody tr').each(function (i) {
        let pictureGroup = $(this).data();
        let pic = {
            Alt: pictureGroup.altValue,
            Title: pictureGroup.titleValue,
            DisplayOrder: pictureGroup.displayValue,
            AttrColor: pictureGroup.colorValue,
            GroupId: pictureGroup.groupId
        };
        pictureInfo.push(pic);
    });

    if (!isPartOfArray(pictureInfo.map(x => x.AttrColor), selectedColors())) {
        Q.notify(-1, 'Some Images of selected colores are missing.');
        return false;
    }

    let param = {
        ProductId: parseInt($('#ProductId').val()),
        AttributeInfo: attrInfo,
        GroupInfo: groupInfo,
        PictureInfo: pictureInfo
    };
    formData = new FormData();
    for (let i = 0; i < variantImages.length; i++) {
        formData.append(`req[${i}].file`, variantImages[i].File);
        formData.append(`req[${i}].Color`, variantImages[i].AttrColor);
        formData.append(`req[${i}].GroupId`, variantImages[i].GroupId);
        formData.append(`req[${i}].DisplayOrder`, variantImages[i].DisplayOrder);
        formData.append(`req[${i}].Title`, variantImages[i].Title);
        formData.append(`req[${i}].Alt`, variantImages[i].Alt);
    }
    formData.append('jsonObj', JSON.stringify(param));
    Q.btnLdr.Start($('#SaveProduct'));
    $.ajax({
        url: '/Product/SaveVariants',
        data: formData,
        processData: false,
        contentType: false,
        type: 'POST',
        success: function (response) {
            Q.notify(response.statusCode, response.responseText)
            Q.btnLdr.Stop($('#SaveProduct'));
            if (response.statusCode === 1) {
                setTimeout(function () {
                    location.reload();
                }, 600)
            }
        },
        error: function (xhr) {
            Q.renderError(xhr);
            let errors = [];
            let keys = Object.keys(xhr.responseJSON);
            for (let i = 0; i < keys.length; i++) {
                let isErrorsExists = 'errors' in xhr.responseJSON[keys[i]]
                if (isErrorsExists) {
                    let e = xhr.responseJSON.map(x => x.errors);
                    errors = [...e];
                }
                else {
                    errors.push(xhr.responseJSON[keys[i]]);
                }
            }
            Q.btnLdr.Stop($('#SaveProduct'));
            Q.notify(-1, errors.join('<br/>'))
        },
    });
})

$(document).on('click', '.rmComb', e => $(e.currentTarget).parents('div.row').remove());

$(document).on('click', '.rmRow', (e) => {
    let data = $(e.currentTarget).closest('tr').data();
    if (data) {
        let param = {
            AttributeId: data.attrId,
            AttributeText: data.attrValue,
            AttributeName: data.attrName
        };
        $(e.currentTarget).closest('tr').remove();
        if (param.AttributeName == "color") {
            let _colors = selectedColors();
            if (!_colors.includes(param.AttributeText)) {
                $(`#attrColor option[value="${param.AttributeText}"]`).remove();
            }
        }
        if ($('#attrColor option').length <= 0) {
            let _option = `<option value="Default" data-group-id="1">Default</option>`;
            $('#attrColor').append(_option);
        }
    }
});

$(document).on('click', '.btnCloneAttrGroup', e => {
    let _count = $('.attrCombination').length;
    let __html = $('.attrCombination:eq(0)').html();
    __html = __html.replaceAll('id="IsFeatured"', 'id="IsFeatured' + _count + '"')
        .replaceAll('for="IsFeatured"', 'for="IsFeatured' + _count + '"')
        .replaceAll('id="IsShowOnHome"', 'id="IsShowOnHome' + _count + '"')
        .replaceAll('for="IsShowOnHome"', 'for="IsShowOnHome' + _count + '"');
    $('attrGroup')
        .append(`<div class="row"><h4 class="col-sm-12">Combination-${_count + 1}<span class="float-right text-danger fa fa-times rmComb"></span>  <hr /></h4><div class="col-md-12 attrCombination" id="clone_${_count}">${__html}</div></div>`);
});

$(document).on('click', '#AddProductImg', () => {
    let __files = $('#PictureFile')[0].files;
    let totalFiles = __files.length;
    if (totalFiles == 0) {
        Q.notify(-1, 'Please select a image.');
        return false;
    }
    for (let i = 0; i < totalFiles; i++) {
        //HightWidthValidatio
        var _URL = window.URL || window.webkitURL;
        var file, img;
        if ((file = __files[i])) {
            img = new Image();
            img.onload = function () {
            };
            img.src = _URL.createObjectURL(file);
        }
        //HightWidthValidatio
        let param = {
            File: __files[i],
            Alt: $('#Alt').val(),
            Title: $('#Title').val(),
            DisplayOrder: $('#DisplayOrder').val(),
            AttrColor: $("#attrColor option:selected").text(),
            GroupId: $("#attrColor option:selected").data()?.groupId
        };
        if (!param.File || !param.Alt || !param.Title || !param.AttrColor) {
            Q.notify(-1, 'All Image fileds are required');
            return false;
        }
        let pictureGrid = $(`.pictureGrid`);
        let count = pictureGrid.find('tr').length;
        variantImages.push(param);
        let _src = URL.createObjectURL(param.File);
        param.DisplayOrder = $('.pictureGrid tr').length;
        let _html = `<tr data-group-Id="${param.GroupId}" data-alt-value="${param.Alt}" data-title-value="${param.Title}" data-display-value="${param.DisplayOrder}" data-color-value="${param.AttrColor}">
          <td>${count}</td>
          <td><img src="${_src}" alt="image" style="width:100px"/></td>
          <td>${param.DisplayOrder}</td>
          <td>${param.Alt}</td>
          <td>${param.Title}</td>
          <td>${param.AttrColor}</td>
          <td><span class="btn btn-danger rmImage" ><i class="fa fa-times"> </i><span></td>
          </tr>`;
        pictureGrid.append(_html);
        Q.notify(1, 'Image Added Successfully');
        $('.rmImage').unbind().click(e => {
            let _ele = $(e.currentTarget).closest('tr');
            let __index = _ele.index();
            variantImages.splice(__index, 1);
            _ele.remove();
        });
    }
});



$(document).on('change', '[name="SellingCost"],[name="MRP"]', e => {
    let __parent = $(e.currentTarget).parents('.attrCombination');
    let sellingPrice = __parent.find('[name="SellingCost"]').val() === '' ? 0 : __parent.find('[name="SellingCost"]').val()
    let mrp = __parent.find('[name="MRP"]').val() === '' ? 0 : __parent.find('[name="MRP"]').val();

    if (parseFloat(sellingPrice) > parseFloat(mrp)) {
        __parent.find('[name="SellingCost"]').val(mrp).change();
        Q.notify('Selling Price cannot be greater than MRP');
    }
});

$(document).on('change', '[name="Title"]', e => {
    let _title = $(e.currentTarget).val();
    $('[name="Alt"]').val(_title);
    $('#pictureSection').find('[name="Title"]').val(_title);
});
$('[name="Quantity"], [name="DisplayOrder"]').numeric({ numericType: 'number', maxLength: '7' });

let loadColorAttr = () => {
    imageColorDDL = $('#attrColor');
    imageColorDDL.empty();
    setTimeout(function () {
        if (__attributes.length > 0) {
            __attributes.map(x => {
                let groupId = x.groupId;
                x.attributes.map(y => {
                    if (y.name === 'COLOR' || y.name === 'COLOUR') {
                        imageColorDDL.append(`<option value="${y.value}" data-group-Id="${groupId}">${y.value}</option>`)
                    }
                })
            });
        }
        else {
            imageColorDDL.append(`<option value="Default" data-group-id="1">Default</option>`);
        }
    }, 600);
}

$(document).on('click', '.deleteVariantRow', (e) => {
    let currentRow = $(e.currentTarget).closest('tr');
    let currentIndex = currentRow.index() + 1;
    console.log('currentIndex:', currentIndex);
    currentRow.remove();
    __attributes = __attributes.filter(x => x.groupId !== currentIndex);
    loadColorAttr();
    let trs = $('.tblgroupinfo').has('tr');
    if (trs.length == 0) {
        $('#dataTable').addClass('d-none');
        $('#btnProceed').addClass('d-none');
    }
});

$('#btnProceed').unbind().click(() => $('#pictureTab').click());

var __attributes = [];
let recordsintable = () => {
    let _isReturn = validateInputs();
    if (!_isReturn) {
        return false;
    }
    let __attr = [];
    $("#dataTable1 tbody tr").each(function () {
        let _currentEle = $(this);
        let attrName = _currentEle.data().attributeName?.toUpperCase();
        __attr.push({
            id: _currentEle.data().attributeId,
            name: attrName,
            value: _currentEle.find("input").val()
        });
        if (attrName === 'COLOR' || attrName === 'COLOUR') {
            $('tr[data-color-value="Default"]').remove();
            loadColorAttr();
        }
    });

    __attributes.push({
        groupId: __attributes.length + 1,
        attributes: __attr
    });

    //if (__attributes.map(x => x.value).join() == "") {
    //    $('#alertText').text('Please add attribute first');
    //    return;
    //}


    $('#alertText').text('');
    let pictureGrid = $(`#dataTable`);
    let count = pictureGrid.find('tr').length;
    let dataObj = {
        Title: $("#titlewords").val(),
        MRP: $("#MRP").val(),
        SellingPrice: $("#SellingCst").val(),
        Quantity: $("#quantity").val(),
        Return: $("#returnhere").val(),
        Warranty: $("#warranty").val(),
        WarrantyUnit: $("#yearmonth").val(),
        Specification: tinymce.get("specification").getContent(),
        IsFeatured: $("#IsFeatured").prop("checked"),
        IsShowOnHome: $("#IsShowOnHome").prop("checked")
    };
    var newRow = "<tr>" +
        "<td>" + count + "</td>" +
        "<td>" + dataObj.Title + "</td>" +
        "<td>" + dataObj.MRP + "</td>" +
        "<td>" + dataObj.SellingPrice + "</td>" +
        "<td>" + dataObj.Quantity + "</td>" +
        "<td>" + dataObj.Return + "</td>" +
        "<td>" + dataObj.Warranty + "</td>" +
        "<td>" + dataObj.WarrantyUnit + "</td>" +
        "<td>" + dataObj.IsFeatured + "</td>" +
        "<td>" + dataObj.IsShowOnHome + "</td>" +
        "<td>" + dataObj.Specification + "</td>" +
        "<td>" + __attributes[count - 1].attributes.map(x => x.value).join() + "</td>" +
        `<td class='text-nowrap'><i class='fa fa-trash text-danger deleteVariantRow' style="cursor: pointer"></i></td>` +
        "</tr>";

    let _isExists = duplicateExists();

    // Append the new row to the table body
    if (!_isExists) {
        $('#dataTable').removeClass('d-none');
        $('#btnProceed').removeClass('d-none');
        $('#alertexists').text('');
        $("#dataTable tbody").append(newRow);
    }
    else {
        $('#alertexists').text('Variant is already in queue.Please try with new attribute combination');
    }

};
let validateInputs = () => {
    $('[data-valmsg-for]').remove();
    $('input.form-control.text-box.single-line[required]').removeClass('is-invalid');
    $('input.form-control.text-box.single-line[required]').addClass('is-valid');
    let validationErrors = $('input.form-control.text-box.single-line[required]').filter(function () {
        return $(this).val() == '' || $(this).val() == '0'
    });
    let _is = !(validationErrors.length > 0);
    if (!_is) {
        for (var i = 0; i < validationErrors.length; i++) {
            if (validationErrors[i].value == '' || validationErrors[i].value == '0') {
                let inpuId = validationErrors[i].id;
                $(`#${inpuId}`).addClass('is-invalid');
            }
        }
    }
    return _is;
};
let duplicateExists = () => {
    let duplicateExists = false;
    $('#dataTable tbody tr').each(function () {
        let AttributeValue = $(this).find('td:eq(14)').text();
        AttributeValue = AttributeValue.split(',')[0];
        if ($('#attrValue').val() == AttributeValue) {
            duplicateExists = true;
        }
    });
    return duplicateExists;
};