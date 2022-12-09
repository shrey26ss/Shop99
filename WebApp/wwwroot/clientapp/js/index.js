$(document).ready(function () {
    loadTopBrands();
    loadMainCategory();
    loadTopBannerSec();
    loadofferBanner();
    loadNewProducts();
    loadFeatureProducts();
    loadBestSeller();
    loadOnSale();
    loadHotDeals();
    loadHotDealsNewProduct();
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
                htmlbody = htmlbody + `<li><a href="/topbrands/${v.id}">${v.name}</a></li>`;
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
            $.each(res.result, async function (i, v) {
                let current = i == 0 ? "class='current'" : "";
                TopCategory.append(`<li ${current}><a href="tab${i + 1}">${v.categoryName}</a></li >`);
                loadTopCategoryProduct(v.categoryId, i + 1);
            })
        }
    }).fail(xhr => Q.notify(-1, xhr.responseText)).always(() => "");
}
const loadTopCategoryProduct = function (cId, i) {
    let item = {
        OrderBy: "",
        Top: 10,
        MoreFilters: {
            CategoryId: cId
        }
    };
    console.log(item);
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
                        <a href="/productdetail/${v.productID}">
                          <img src="${v.imagePath}"   onerror="this.onerror=null;this.src='/assets/images/noimage.jpg'" class="img-fluid  " alt="product">
                        </a>
                      </div>
                      <div class="product-back">
                        <a href="/productdetail/${v.productID}">
                          <img src="${v.imagePath}" class="img-fluid  " onerror="this.onerror=null;this.src='/assets/images/noimage.jpg'" alt="product">
                        </a>
                      </div>
                      <div class="product-icon icon-inline">
                        <button  onclick="openCart()" class="tooltip-top" data-tippy-content="Add to cart" >
                          <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-shopping-cart"><circle cx="9" cy="21" r="1"></circle><circle cx="20" cy="21" r="1"></circle><path d="M1 1h4l2.68 13.39a2 2 0 0 0 2 1.61h9.72a2 2 0 0 0 2-1.61L23 6H6"></path></svg>
                        </button>
                        <a href="javascript:void(0)"  class="add-to-wish tooltip-top"  data-tippy-content="Add to Wishlist" >
                          <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-heart"><path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"></path></svg>
                        </a>
                        <a href="javascript:void(0)" data-bs-toggle="modal" data-bs-target="#quick-view" class="tooltip-top"  data-tippy-content="Quick View">
                          <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-heart"><path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"></path></svg>
                        </a>
                        <a href="compare.html" class="tooltip-top" data-tippy-content="Compare">
                          <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-refresh-cw"><polyline points="23 4 23 10 17 10"></polyline><polyline points="1 20 1 14 7 14"></polyline><path d="M3.51 9a9 9 0 0 1 14.85-3.36L23 10M1 14l4.64 4.36A9 9 0 0 0 20.49 15"></path></svg>
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
                          <a href="/productdetail/${v.productID}">
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
            if (i !== 1) {
                $(`#tab${i}`).css({ 'display': 'none' });
            }
        },
        error: result => {

        }
    });
}
const loadTopBannerSec = async function () {
    await $.post("/LoadTopBanner").done(res => {
        $('#secDvBanner').html(res);
    }).fail(xhr => Q.notify(-1, xhr.responseText)).always(() => "");
   
}
const loadofferBanner = async function () {
    await $.get(baseURL + "/Home/OfferBanner").done(res => {
        console.log('Offer Banner');
        let htmlbody = ``;
        $.each(res.result, async function (i, v) {
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
       
    }).fail(xhr => Q.notify(-1, xhr.responseText)).always(() => "");
}
const loadNewProducts = async function () {
    await $.post("/ProductSection", { id: 1 }).done(res => {
        
        $('#secDvProduct').html(res);
    }).fail(xhr => Q.notify(-1, xhr.responseText)).always(() => "");
    
}
const loadFeatureProducts = async function () {
    await $.post("/ProductSection", { id: 2 }).done(res => {
       
        $('#secDvProduct').append(res);
    }).fail(xhr => Q.notify(-1, xhr.responseText)).always(() => "");
}
const loadBestSeller = async function () {
    await $.post("/ProductSection", { id: 3 }).done(res => {
      
        $('#secDvProduct').append(res);
    }).fail(xhr => Q.notify(-1, xhr.responseText)).always(() => "");
}
const loadOnSale = async function () {
    await $.post("/ProductSection", { id: 4 }).done(res => {
      
        $('#secDvProduct').append(res);
    }).fail(xhr => Q.notify(-1, xhr.responseText)).always(() => "");
}
const loadHotDeals = async function () {
    await $.post("/HotDeals").done(res => {
        $('#dvHotDeals').append(res);
    }).fail(xhr => Q.notify(-1, xhr.responseText)).always(() => "");
}
const loadHotDealsNewProduct = async function () {
    await $.post("/HotDealsNewProduct").done(res => {
        $('#dvHotDealNewProduct').append(res);
    }).fail(xhr => Q.notify(-1, xhr.responseText)).always(() => "");
}
