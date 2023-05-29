
var category = (function () {
    "use strict";
    var mol = {};
    mol.origin = 'https://localhost:5001';
    mol.o = location.origin;
    mol.init = function () {
        var B = $('body');
        let page = 1, size = 4, language = 'vi-VN';
        getProducts(language, page, size);

        B.delegate(".pagination1", "click", function () {
            if (page != $(this).attr("id")) {
                page = $(this).attr("id");
                getProducts(language, page, size);
            }
        })

        B.delegate(".pnext", "click", function () {
            if (typeof (page) == "string")
                page = parseInt(page)
            page += 1;
            getProducts(language, page, size);
        })

        B.delegate(".pprev", "click", function () {
            if (typeof (page) == "string")
                page = parseInt(page)
            page -= 1;
            getProducts(language, page, size);
        })
    }

    
    return mol;

    function getProducts(language, pageIndex, pageSize) {
        var url = mol.origin + `/api/Products/paging?LanguageId=${language}&PageIndex=${pageIndex}&PageSize=${pageSize}`;
        $.ajax({
            method: "GET",
            url: url
        })
            .done(data => {
                mol.data = data;
                $('.toolbar-amount').html(`<span>Hiển thị ${data.pageIndex * pageSize - pageSize + 1} - ${data.pageIndex * pageSize} trên ${data.totalRecords} sản phẩm</span>`)
                $('.pagination-box').html(app.phantrang(pageIndex, data.totalRecords, pageSize))
                var p1 = [], p2 = [];
                data.items.map(i => {
                    let rating = '';

                    for (let r = 1; r <= 5; r++) {
                        if (r <= i.rating) {
                            rating += '<li><i class="fa fa-star-o"></i></li>'
                        } else {
                            rating += '<li class="no-star"><i class="fa fa-star-o"></i></li>'
                        }
                    }

                    p1.push(`<div class="col-lg-4 col-md-4 col-sm-6 mt-40">
                            <div class="single-product-wrap">
                                <div class="product-image">
                                    <a href="${mol.o}/vi-VN/san-pham/${i.id}">
                                        <img src="${mol.origin}/user-content/${i.thumbnailImage}" alt="${i.name}">
                                    </a>
                                    <span class="sticker">New</span>
                                </div>
                                <div class="product_desc">
                                    <div class="product_desc_info">
                                        <div class="product-review">
                                            <h5 class="manufacturer">
                                                <a href="product-details.html">${i.categories[0]}</a>
                                            </h5>
                                            <div class="rating-box">
                                                <ul class="rating">
                                                    ${rating}
                                                </ul>
                                            </div>
                                        </div>
                                        <h4><a class="product_name" href="single-product.html">${i.name}</a></h4>
                                        <div class="price-box">
                                            <span class="old-price">${app.fmnumber(i.price)} VND</span>
                                            <span class="new-price">${app.fmnumber(i.originalPrice)} VND</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div >`
                    )

                    p2.push(`<div class="row product-layout-list">
                                        <div class="col-lg-3 col-md-5 ">
                                            <div class="product-image">
                                                <a href="${mol.o}/vi-VN/san-pham/${i.id}">
                                                    <img src="${mol.origin}/user-content/${i.thumbnailImage}" alt="${i.name}">
                                                </a>
                                                <span class="sticker">New</span>
                                            </div>
                                        </div>
                                        <div class="col-lg-5 col-md-7">
                                            <div class="product_desc">
                                                <div class="product_desc_info">
                                                    <div class="product-review">
                                                        <h5 class="manufacturer">
                                                            <a href="product-details.html">${i.categories[0]}</a>
                                                        </h5>
                                                        <div class="rating-box">
                                                            <ul class="rating">
                                                                ${rating}
                                                            </ul>
                                                        </div>
                                                    </div>
                                                    <h4><a class="product_name" href="${mol.o}/vi-VN/san-pham/${i.id}">${i.name}</a></h4>
                                                    <div class="price-box">
                                                        <span class="old-price">${app.fmnumber(i.price)} VND</span>
                                                        <span class="new-price">${app.fmnumber(i.originalPrice)} VND</span>
                                                    </div>
                                                    <p>${i.description}</p>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-4">
                                            <div class="shop-add-action mb-xs-30">
                                                <ul class="add-actions-link">
                                                    <li class="add-cart"><a href="#">Add to cart</a></li>
                                                    <li class="wishlist"><a href="wishlist.html"><i class="fa fa-heart-o"></i>Add to wishlist</a></li>
                                                    <li><a class="quick-view" data-toggle="modal" data-target="#exampleModalCenter" href="#"><i class="fa fa-eye"></i>Quick view</a></li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>`);

                    $('.product-area.shop-product-area').html('<div class="row">' + p1.join('') + '</div > ');
                    $('#list-view').html('<div class="row">' + p2.join('') + '</div > ');
                })
            });
    }
})();
$(document).ready(function () {
    category.init();

})