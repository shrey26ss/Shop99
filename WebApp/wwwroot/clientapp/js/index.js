$(document).ready(function () {
    loadTopBrands();
    // loadRoundCategory();
    loadTopBannerSec();
    loadofferBanner();

    loadHotDeals();
    loadHotDealsNewProduct();
    loadMainCategory();
    loadTabs();
});



const loadTopBrands = function () {
    let item = {
        OrderBy: 0,
        Top: 10
    };
    $.ajax({
        type: 'POST',
        url: baseURL + "/Home/TopBrands",
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(item),
        success: result => {
            let brandLength = result.result.length;
            if (brandLength >= 1) {
                $('.topBrandPanel').removeClass('d-none');
                let htmlbody = ``;
                $.each(result.result, async function (i, v) {
                    htmlbody = htmlbody + `<li><a href="/products/brand/${v.id}">${v.name}</a></li>`;
                });
                $('#litopbrand').append(htmlbody);
            }
            else {
                $('.topBrandPanel').addClass('d-none');
            }

        },
        error: result => {

        }
    });
}
const loadMainCategory = function () {

    var settings = {
        "url": baseURL + "/Category/TopCategory",
        "method": "POST",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": JSON.stringify({
            "Top": 5
        }),
    };

    $.ajax(settings).done(function (res) {
        if (res.statusCode === 1) {
            let TopCategory = $('#secTopCategory');
            TopCategory.html('');
            let _categoryCount = res.result.length;
            $.each(res.result, async function (i, v) {
                let current = i == 0 ? "class='current'" : "";
                TopCategory.append(`<li ${current}><a href="tab${i + 1}">${v.categoryName}</a></li >`);
                loadTopCategoryProduct(v.categoryId, i + 1, 'H');
                if (_categoryCount === (i + 1)) {
                    setTimeout(() => {
                        $('.cat-tab-content').css({ 'display': 'none' });
                        $('#tab1').css({ 'display': 'block' });
                    }, 1100);
                }
            })
        }
    });
}
const loadTopCategoryProduct = function (cId, i, calledFrom = 'A') {
    let item = {
        CalledFrom: calledFrom,
        OrderBy: 0,
        Top: 10,
        MoreFilters: {
            CategoryId: cId
        }
    };
    $.post('/Home/CategoryProductPartial', item).done(result => {
        $('#secDvPRodByCat').append(result);

        $(`#tab${i}`).css({ 'display': 'block' });

        if ($(`.cat-product-slide-${i}`).hasClass('slick-initialized')) {
            $(`.cat-product-slide-${i}`).slick('unslick')
        }

        $(`.cat-product-slide-${i}`).slick({
            arrows: true,
            dots: false,
            infinite: false,
            speed: 300,
            slidesToShow: 6,
            slidesToScroll: 6,
            responsive: [
                {
                    breakpoint: 1700,
                    settings: {
                        slidesToShow: 5,
                        slidesToScroll: 5,
                        infinite: true
                    }
                },
                {
                    breakpoint: 1200,
                    settings: {
                        slidesToShow: 4,
                        slidesToScroll: 4,
                        infinite: true
                    }
                },
                {
                    breakpoint: 991,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 3,
                        infinite: true
                    }
                },
                {
                    breakpoint: 576,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 2
                    }
                }
            ]
        });
    }).fail(xhr => {

    });
}
const loadTopBannerSec = async function () {
    await $.post("/LoadTopBanner").done(res => {
        $('#secDvBanner').html(res);
    });

}
const loadofferBanner = function () {
    $.get(baseURL + "/Home/OfferBanner").done(res => {
        let htmlbody = ``;
        if (res != null && res.result != null) {
            $.each(res.result, function (i, v) {
                htmlbody = htmlbody + `<div class="collection-img">
            <img src="${v.bannerPath}" class="bg-img  " alt="banner">
          </div>
          <div class="collection-banner-contain ">
            <div class="sub-contain">
              <h3>${v.title}</h3>
              <h4>${v.subtitle}</h4>
              <div class="shop">
                <a class="btn btn-normal" target="_blank" href="${v.backLinkURL}">
                  ${v.backLinkText}
                </a>
              </div>
            </div>
          </div>`;
            });
            $('#dvofferbanner').append(htmlbody);
        }
    })
}
const loadNewProducts = async function loadNewProducts() {
    await $.post("/ProductSection", { id: 1 }).done(res => {
        $('#secDvProduct').append(res);
    });
}
const loadFeatureProducts = async function () {
    await $.post("/ProductSection", { id: 2 }).done(res => {
        $('#secDvProduct').append(res);
    });
}
const loadBestSeller = async function () {
    await $.post("/ProductSection", { id: 3 }).done(res => {

        $('#secDvProduct').append(res);
    });
}
const loadOnSale = async function () {
    await $.post("/ProductSection", { id: 4 }).done(res => {
        $('#secDvProduct').append(res);
    });
}
const loadHotDeals = async function () {
    await $.post("/HotDeals").done(res => {
        $('#dvHotDeals').append(res);
    });
}
const loadRoundCategory = async function () {
    await $.post("/RoundedCategory").done(res => {
        $('#dvroundCategory').append(res);
    });
}

const loadHotDealsNewProduct = async function () {
    await $.post("/HotDealsNewProduct").done(res => {
        $('#dvHotDealNewProduct').append(res);
    });
}

const loadTabs = async () => {
    $('#secDvProduct').html('');
    await loadNewProducts();
    await loadFeatureProducts();
    await loadBestSeller();
    await loadOnSale();

    $('.media-slide-5').slick({
        dots: false,
        infinite: true,
        speed: 300,
        slidesToShow: 5,
        centerPadding: '15px',
        responsive: [
            {
                breakpoint: 1470,
                settings: {
                    slidesToShow: 4,
                    slidesToScroll: 4,
                    infinite: true
                }
            },
            {
                breakpoint: 992,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 3,
                    infinite: true
                }
            },
            {
                breakpoint: 820,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 2,
                    infinite: true
                }
            },
            {
                breakpoint: 576,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    infinite: true
                }
            }
        ]
    });

    $('.tab-content.active.default').css("display", "block");

    $(".tabs li a").on('click', function () {
        event.preventDefault();
        //$('.tab_product_slider').slick('unslick');
        //$('.media-slide-5').slick('unslick');
        $(this).parent().parent().find("li").removeClass("current");
        $(this).parent().addClass("current");
        var currunt_href = $(this).attr("href");
        $('#' + currunt_href).show();
        console.log('currunt_href : ', currunt_href);
        $(this).parent().parent().parent().find(".tab-content").not('#' + currunt_href).css("display", "none");
        if ($('.media-slide-5').hasClass('slick-initialized')) {
            $('.media-slide-5').slick('unslick')
        }
        $('.media-slide-5').slick({
            dots: false,
            infinite: true,
            speed: 300,
            slidesToShow: 5,
            centerPadding: '15px',
            responsive: [
                {
                    breakpoint: 1470,
                    settings: {
                        slidesToShow: 4,
                        slidesToScroll: 4,
                        infinite: true
                    }
                },
                {
                    breakpoint: 992,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 3,
                        infinite: true
                    }
                },
                {
                    breakpoint: 820,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 2,
                        infinite: true
                    }
                },
                {
                    breakpoint: 576,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                        infinite: true
                    }
                }
            ]
        });
    });
}
