
var shop = (function () {
    "use strict";
    var mol = {};
    mol.origin = 'https://localhost:5001';
    mol.o = location.origin;
    mol.init = function () {
        var B = $('body');
        mol.page = 1, mol.size = 9, mol.language = 'vi-VN';
        let token = app.getcookie('Token');
        const path = window.location.pathname;
        const parts = path.split('/');
        mol.category = parts[parts.length - 1] != 'san-pham' ? parts[parts.length - 1] : null;
        $('.categori-checkbox.categories li > input[data-category="' + mol.category +'"]:first').prop('checked', true);
        getProducts();

        B.delegate(".pagination1", "click", function () {
            if (mol.page != $(this).attr("id")) {
                mol.page = $(this).attr("id");
                getProducts();
            }
        })

        B.delegate(".pnext", "click", function () {
            if (typeof (mol.page) == "string")
                mol.page = parseInt(mol.page)
            mol.page += 1;
            getProducts();
        })

        B.delegate(".pprev", "click", function () {
            if (typeof (mol.page) == "string")
                mol.page = parseInt(mol.page)
            mol.page -= 1;
            getProducts();
        })

        B.delegate('.product-select-box .list .option:not(.selected)', 'click', function () {
            mol.order = $(this).data('value')
            mol.page = 1;
            getProducts();
        })

        B.delegate('.add-cart', 'click', function () {
            if (token == null) {
                window.location.assign(`${mol.o}/${language}/account/login`);
            } else {
                let productId = $(this).attr('data-id');
                $.ajax({
                    method: "POST",
                    url: mol.origin + `/api/Carts/${productId}`,
                    headers: {
                        Authorization: `Bearer ${token}`,
                        accept: '*/*',
                    },
                    data: '',
                    success: function (response) {
                        if (response) {
                            index.toast({
                                title: "Thành công",
                                message: "Đã thêm sản phẩm vào giỏ hàng.",
                                type: "success",
                                duration: 3000
                            })
                        } else {
                            index.toast({
                                title: "Thất bại",
                                message: "Thêm sản phẩm thất bại.",
                                type: "error",
                                duration: 3000
                            })
                        }
                    },
                    error: function (error) {
                        index.toast({
                            title: "Thất bại",
                            message: "Thêm sản phẩm thất bại.",
                            type: "error",
                            duration: 3000
                        })
                        console.log('Lỗi truy cập vào API: ', error);
                    }
                })
            }
        });

        B.delegate('.categori-checkbox.categories li > input', 'click', function () {
            if ($(this).is(':checked')) {
                mol.category = $(this).data('category');
                $('.categori-checkbox.categories li > input').not(this).prop('checked', false);
                
            } else {
                delete mol.category;
            }
            mol.page = 1;
            var newUrl = `${mol.o}/${mol.language}${mol.category ? '/' + mol.category : ''}`;
            window.history.pushState({ path: newUrl }, '', newUrl);
            getProducts();
        })

        B.delegate('.price-range > button', 'click', function () {
            let f = app.fmNumberToNumber($('.price-range #price-from').val());
            let t = app.fmNumberToNumber($('.price-range #price-to').val());

            if (f > t || (f == t && f == 0)) {
                alert('Vui lòng điền khoảng giá phù hợp');
                return;
            }

            f ? mol.pricefrom = f : delete mol.pricefrom;
            t ? mol.priceto = t : delete mol.priceto;
            mol.page = 1;
            getProducts();
        });

        B.delegate('.categori-checkbox.evaluate > ul > li', 'click', function () {
            $('.categori-checkbox.evaluate > ul > li.active').removeClass('active');
            $(this).addClass('active');

            mol.rating = parseInt($(this).data('rating'));
            mol.page = 1;
            getProducts();
        })

        B.delegate('.btn-clear-all, .search-empty-result-section__button', 'click', function () {
            $('.categori-checkbox.categories li > input:checked').prop('checked', false);
            $('.price-range input').val('')
            $('.categori-checkbox.evaluate > ul > li.active').removeClass('active');
            delete mol.category;
            delete mol.pricefrom;
            delete mol.priceto;
            delete mol.rating;
            mol.page = 1;
            getProducts();
        })

        B.delegate('.price-range input', 'input', function (event) {
            var inputValue = app.fmNumberToNumber($(this).val());
            var numericValue = inputValue.replace(/[^0-9]/g, '');

            /// Kiểm tra nếu người dùng xóa hết giá trị (chuỗi rỗng)
            if (numericValue === '') {
                numericValue = '0'; // Đặt lại giá trị mặc định là 0 nếu rỗng
            }

            // Xóa tất cả các số 0 ở đầu (nếu có)
            numericValue = numericValue.replace(/^0+/, '');

            // Nếu tất cả các số đã bị xóa, đặt giá trị thành '0'
            if (numericValue === '') {
                numericValue = '0';
            }

            $(this).val(app.fmnumber(numericValue));
        });
    }

    
    return mol;

    function getProducts() {
        var url = `${mol.origin}/api/Products/paging?LanguageId=${mol.language}&PageIndex=${mol.page}&PageSize=${mol.size}` +
            (mol.order ? `&OrderBy=${mol.order}` : '') +
            (mol.category ? `&CategoryId=${mol.category}` : '') +
            (mol.pricefrom ? `&PriceFrom=${mol.pricefrom}` : '') +
            (mol.priceto ? `&PriceTo=${mol.priceto}` : '') +
            (mol.rating ? `&Rating=${mol.rating}` : '');

        $.ajax({
            method: "GET",
            url: url
        })
            .done(data => {
                mol.data = data;
                $('.pagination-box').html(app.phantrang(data.pageIndex, data.totalRecords, data.pageSize))
                if (data.items.length > 0) {
                    $('.search-empty-result-section').html('');
                    $('.toolbar-amount').html(`<span>Hiển thị ${data.pageIndex * data.pageSize - data.pageSize + 1} - ${data.pageIndex * data.pageSize < data.totalRecords ? data.pageIndex * data.pageSize : data.totalRecords} trên ${data.totalRecords} sản phẩm</span>`)
                    var p1 = [], p2 = [];
                    data.items.map(i => {
                        let rating = '';

                        for (let r = 1; r <= 5; r++) {
                            if (r <= i.rating) {
                                rating += '<li><i class="fa fa-star" aria-hidden="true"></i></li>'
                            } else {
                                rating += '<li class="no-star"><i class="fa fa-star" aria-hidden="true"></i></li>'
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
                                        <h4><a class="product_name" href="${mol.o}/vi-VN/san-pham/${i.id}">${i.name}</a></h4>
                                        <div class="price-box">
                                            <span class="old-price">&#8363;${app.fmnumber(i.originalPrice)}</span>
                                            <span class="new-price new-price-2">&#8363;${app.fmnumber(i.price)}</span>
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
                                                        <span class="old-price">&#8363;${app.fmnumber(i.originalPrice)}</span>
                                                        <span class="new-price new-price-2">&#8363;${app.fmnumber(i.price)}</span>
                                                    </div>
                                                    <p>${i.description}</p>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-4">
                                            <div class="shop-add-action mb-xs-30">
                                                <ul class="add-actions-link">
                                                    <li class="add-cart" data-id="${i.id}"><a>Thêm vào giỏ hàng</a></li>
                                                    <li class="wishlist"><a href="wishlist.html"><i class="fa fa-heart-o"></i>Add to wishlist</a></li>
                                                    <li><a class="quick-view" data-toggle="modal" data-target="#exampleModalCenter"><i class="fa fa-eye"></i>Quick view</a></li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>`);

                        $('.product-area.shop-product-area').html('<div class="row">' + p1.join('') + '</div > ');
                        $('#list-view').html('<div class="row"><div class="col">' + p2.join('') + '</div></div>');


                    })
                } else {
                    $('.product-area.shop-product-area').html('');
                    $('#list-view').html('');
                    $('.toolbar-amount').html(`<span>Không có sản phẩm</span>`)
                    $('.search-empty-result-section').html(`
                        <img src="/images/search-empty-result.png" class="search-empty-result-section__icon">
                            <div class= "search-empty-result-section__hint" > Hix.Không có sản phẩm nào.Bạn thử tắt điều kiện lọc và tìm lại nhé ?</div>
                        <div class="search-empty-result-section__or">or</div>
                        <div class="search-empty-result-section__button">
                            <button type="button" class="btn btn-custom">Xóa bộ lọc</button>
                        </div>
                    `);
                }
            });
    }
})();
$(document).ready(function () {
    shop.init();

})