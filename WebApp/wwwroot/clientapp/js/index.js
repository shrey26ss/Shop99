$(document).ready(function () {
    loadTopBrands();
    loadTopBannerSec();
    loadofferBanner();
    loadNewProducts();
    loadFeatureProducts();
    loadBestSeller();
    loadOnSale();
    loadHotDeals();
    loadHotDealsNewProduct();
    loadMainCategory();

});
const loadTopBrands = function () {
    let item = {
        OrderBy: "",
        Top: 10
    };
    $.ajax({
        type: 'POST',
        url: baseURL + "/Home/TopBrands",
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(item),
        success: result => {
            let htmlbody = ``;
            $.each(result.result, async function (i, v) {
                htmlbody = htmlbody + `<li><a href="/Brand/${v.id}">${v.name}</a></li>`;
            });
            $('#litopbrand').append(htmlbody);
        },
        error: result => {

        }
    });
}
const loadMainCategory = function () {
    $.post(baseURL + "/Category/TopCategory").done(res => {
        if (res.statusCode === 1) {
            let TopCategory = $('#secTopCategory');
            TopCategory.html('');
            let _categoryCount = res.result.length;
            $.each(res.result, async function (i, v) {
                let current = i == 0 ? "class='current'" : "";
                TopCategory.append(`<li ${current}><a href="tab${i + 1}">${v.categoryName}</a></li >`);
                loadTopCategoryProduct(v.categoryId, i + 1);
                if (_categoryCount === (i + 1)) {
                    setTimeout(() => {
                        $('.tab-content').css({ 'display': 'none' });
                    }, 1100);
                }
            })
        }
    });
}
const loadTopCategoryProduct = function (cId, i) {
    let item = {
        OrderBy: "",
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
            let htmlbody = `<div id="tab${i}" class="tab-content ${current}"><div class="product-slide-${i} product-m no-arrow">`;
            $.each(result.result, async function (i, v) {
                htmlbody = htmlbody + `<div>
                  <div class="product-box">
                    <div class="product-imgbox">
                      <div class="product-front">
                        <a href="/productdetails/${v.variantID}">
                          <img src="${v.imagePath}"   onerror="this.onerror=null;this.src='/assets/images/noimage.jpg'" class="img-fluid  " alt="product">
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
                            <img src="/Image/icons/wishlist.svg" class="svg"/>
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
                          <div class="rating-star">
                            <i class="fa fa-star"></i>
                            <i class="fa fa-star"></i>
                            <i class="fa fa-star"></i>
                            <i class="fa fa-star"></i>
                            <i class="fa fa-star"></i>
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
            if ($(`.product-slide-${i}`).hasClass('slick-initialized')) {
                $(`.product-slide-${i}`).slick('unslick')
            }
            $(`.product-slide-${i}`).slick({
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
const loadNewProducts = async function () {
    await $.post("/ProductSection", { id: 1 }).done(res => {
        $('#secDvProduct').html(res);
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
const loadHotDealsNewProduct = async function () {
    await $.post("/HotDealsNewProduct").done(res => {
        $('#dvHotDealNewProduct').append(res);
    });
}



