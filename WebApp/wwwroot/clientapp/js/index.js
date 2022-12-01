$(document).ready(function () {
    loadMainCategory().then((res) => {
        setTimeout(function () {
            console.log('tab', $('#tab1').length)
            $('#tab1').css({ 'display': 'block' });
            $(".product-slide-6").slick({
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
        }, 800);
    });
});

const loadMainCategory = async function () {
    //"/LoadTopCategory"
     await $.post(baseURL + "/Category/TopCategory").done(res => {
        console.log(res);
        if (res.statusCode === 1) {
            let TopCategory = $('#secTopCategory');
            TopCategory.html('');
            $.each(res.result, async function (i, v) {
                let current = i == 0 ? "class='current'" : "";
                TopCategory.append(`<li ${current}><a href="tab${v.categoryId}">${v.categoryName}</a></li >`);
                loadTopCategoryProduct(v.categoryId,i);
            })
         }
    }).fail(xhr => {
        an.title = "Oops! Error";
        an.content = xhr.responseText;
        an.alert(an.type.failed);
    }).always(() => "");
    return true;
}
const loadTopCategoryProduct = async function (cId,i) {
    //"/LoadTopCategory"
    let item = {
        OrderBy: "",
        Top: 10,
        MoreFilters: cId
    };
    console.log("item : ", item);
    $.ajax({
        type: 'POST',
        url: baseURL + "/Home/ByCategoryProduct",
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(item),
        success: result => {
            let current = i == 0 ? "active default" : "";
            let htmlbody = `<div id="tab${cId}" class="tab-content ${current}">
              <div class="product-slide-6 product-m no-arrow">`;
            $.each(result.result, async function (i, v) {
                htmlbody = htmlbody + `<div>
                  <div class="product-box">
                    <div class="product-imgbox">
                      <div class="product-front">
                        <a href="product-page(left-sidebar).html">
                          <img src="${v.imagePath}" class="img-fluid  " alt="product">
                        </a>
                      </div>
                      <div class="product-back">
                        <a href="product-page(left-sidebar).html">
                          <img src="${v.imagePath}" class="img-fluid  " alt="product">
                        </a>
                      </div>
                      <div class="product-icon icon-inline">
                        <button  onclick="openCart()" class="tooltip-top" data-tippy-content="Add to cart" >
                          <i  data-feather="shopping-cart"></i>
                        </button>
                        <a href="javascript:void(0)"  class="add-to-wish tooltip-top"  data-tippy-content="Add to Wishlist" >
                          <i  data-feather="heart"></i>
                        </a>
                        <a href="javascript:void(0)" data-bs-toggle="modal" data-bs-target="#quick-view" class="tooltip-top"  data-tippy-content="Quick View">
                          <i  data-feather="eye"></i>
                        </a>
                        <a href="compare.html" class="tooltip-top" data-tippy-content="Compare">
                          <i  data-feather="refresh-cw"></i>
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
                          <a href="product-page(left-sidebar).html">
                            <h6 class="price-title">
                              sony xperia m5
                            </h6>
                          </a>
                        </div>
                        <div class="detail-right">
                          <div class="check-price">
                            ₹ ${v.mrp}
                          </div>
                          <div class="price">
                            <div class="price">
                              ₹ ${v.sellingCost}
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
            console.log(result)
        },
        error: result => {
            console.log(result);
        }
    });
}