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

    $.ajax({
        type: 'POST',
        url: baseURL + "/Home/ByCategoryProduct",
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(item),
        success: result => {
            let current = i == 1 ? "active default" : "";
            let htmlbody = `<div id="tab${i}" class="cat-tab-content ${current}"><div class="cat-product-slide-${i} product-m no-arrow">`;
            $.each(result.result.data, async function (i, v) {
                htmlbody = htmlbody + `<div>
                  <div class="product-box">
                    <div class="product-imgbox">
                      <div class="product-front">
                        <a href="/productdetails/${v.variantID}">
                          <img src="${v.imagePath}" onerror="this.onerror=null;this.src='/assets/images/noimage.jpg'" class="img-fluid  " alt="product">
                        </a>
                      </div>
                      <div class="product-back">
                        <a href="/productdetails/${v.variantID}">
                          <img src="${v.imagePath}" class="img-fluid  " onerror="this.onerror=null;this.src='/assets/images/noimage.jpg'" alt="product">
                        </a>
                      </div>
                      <div class="product-icon icon-inline">
                        <button  class="tooltip-top addtocart" data-variant-id="${v.variantID}" data-tippy-content="Add to cart" >
                            <img src="/Image/icons/cart-mini.svg" class="svg"/>
                        </button>
                        <a href="javascript:void(0)"  class="add-to-wish tooltip-top addtowishlist" data-variant-id="${v.variantID}"   data-tippy-content="Add to Wishlist" >
                            <svg viewBox="0 -28 512.001 512" xmlns="http://www.w3.org/2000/svg"><path d="m256 455.515625c-7.289062 0-14.316406-2.640625-19.792969-7.4375-20.683593-18.085937-40.625-35.082031-58.21875-50.074219l-.089843-.078125c-51.582032-43.957031-96.125-81.917969-127.117188-119.3125-34.644531-41.804687-50.78125-81.441406-50.78125-124.742187 0-42.070313 14.425781-80.882813 40.617188-109.292969 26.503906-28.746094 62.871093-44.578125 102.414062-44.578125 29.554688 0 56.621094 9.34375 80.445312 27.769531 12.023438 9.300781 22.921876 20.683594 32.523438 33.960938 9.605469-13.277344 20.5-24.660157 32.527344-33.960938 23.824218-18.425781 50.890625-27.769531 80.445312-27.769531 39.539063 0 75.910156 15.832031 102.414063 44.578125 26.191406 28.410156 40.613281 67.222656 40.613281 109.292969 0 43.300781-16.132812 82.9375-50.777344 124.738281-30.992187 37.398437-75.53125 75.355469-127.105468 119.308594-17.625 15.015625-37.597657 32.039062-58.328126 50.167969-5.472656 4.789062-12.503906 7.429687-19.789062 7.429687zm-112.96875-425.523437c-31.066406 0-59.605469 12.398437-80.367188 34.914062-21.070312 22.855469-32.675781 54.449219-32.675781 88.964844 0 36.417968 13.535157 68.988281 43.882813 105.605468 29.332031 35.394532 72.960937 72.574219 123.476562 115.625l.09375.078126c17.660156 15.050781 37.679688 32.113281 58.515625 50.332031 20.960938-18.253907 41.011719-35.34375 58.707031-50.417969 50.511719-43.050781 94.136719-80.222656 123.46875-115.617188 30.34375-36.617187 43.878907-69.1875 43.878907-105.605468 0-34.515625-11.605469-66.109375-32.675781-88.964844-20.757813-22.515625-49.300782-34.914062-80.363282-34.914062-22.757812 0-43.652344 7.234374-62.101562 21.5-16.441406 12.71875-27.894532 28.796874-34.609375 40.046874-3.453125 5.785157-9.53125 9.238282-16.261719 9.238282s-12.808594-3.453125-16.261719-9.238282c-6.710937-11.25-18.164062-27.328124-34.609375-40.046874-18.449218-14.265626-39.34375-21.5-62.097656-21.5zm0 0"></path></svg>
                        </a>
                        <a href="/productdetails/${v.variantID}" class="tooltip-top" data-tippy-content="Quick View">
                            <img src="/Image/icons/info.svg" class="svg">
                        </a>
                        <a href="#" class="tooltip-top" data-tippy-content="Compare">
                           <img src="/Image/icons/compare.svg" class="svg"/>
                        </a>
                      </div>
                      <div class="new-label1">
                        <div>new</div>
                      </div>
                      <div class="on-sale1">
                        on sale
                      </div>
                    </div>
                    <div class="product-detail detail-inline ">
                      <div class="detail-title">
                        <div class="detail-left">
                          <div class="rating-star">`
                for (var i = 0; i < v.rating; i++) {
                    htmlbody = htmlbody + `<i class="fa fa-star"></i>`;
                }
                for (var i = 0; i < 5 - v.rating; i++) {
                    htmlbody = htmlbody + `<i class="fa fa-star-o"></i>`;
                }
                htmlbody = htmlbody + `
                          </div>
                          <a href="/productdetails/${v.variantID}">
                            <h6 class="price-title">
                              ${v.title}
                            </h6>
                          </a>
                        </div>
                        <div class="detail-right">
                          <div class="check-price">
                           &#8377; ${v.mrp}
                          </div>
                          <div class="price">
                            <div class="price">
                              &#8377; ${v.sellingCost}
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>`;
            });
            htmlbody = htmlbody + ` </div>
            </div>`;
            $('#secDvPRodByCat').append(htmlbody);
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

            //if (i !== 1) {
            //    $(`#tab${i}`).css({ 'display': 'none' });
            //}
        },
        error: result => {

        }
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
