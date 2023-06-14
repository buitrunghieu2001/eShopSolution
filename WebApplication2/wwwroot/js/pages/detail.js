var detail = (function () {
    "use strict";
    var mol = {};
    mol.origin = 'https://localhost:5001';
    mol.o = location.origin;
    mol.productId = location.href.split('/')[location.href.split('/').length - 1];
    mol.init = function () {
        var B = $('body');
        let language = 'vi-VN';
        getProduct(language, mol.productId);
        var token = app.getcookie("Token");

        // tăng lượt xem sản phẩm
        setTimeout(() => {
            $.ajax({
                method: "PATCH",
                url: mol.origin + `/api/Products/${mol.productId}/viewCount`,
                headers: {
                    accept: '*/*',
                },
                success: function (response) {
                    if (response.isSuccessed) {
                        console.log(response);
                    } else {
                        console.error('Có lỗi khi cập nhật lượt xem.');
                    }
                },
                error: function (error) {
                    console.log('Lỗi truy cập vào API: ', error);
                }
            })
        }, 900000);


        B.delegate('.add-to-cart', 'click', function () {
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
                            $('#msgAlert').html('Thêm vào giỏ hàng thành công');
                            $('#msgAlert').show()
                            setTimeout(function () {
                                $('#msgAlert').fadeOut('slow');
                            }, 2000);
                            $('.hm-minicart-trigger .cart-item-count').html(response);
                        }
                    },
                    error: function (error) {
                        console.log('Lỗi truy cập vào API: ', error);
                    }
                })
            }
        });
    }


    return mol;

    function getProduct(language, productId) {
        var url = mol.origin + `/api/Products/${language}/${productId}`;
        $.ajax({
            method: "GET",
            url: url
        })
            .done(data => {
                mol.data = data;
                let rating = '';

                for (let r = 1; r <= 5; r++) {
                    if (r <= data.rating) {
                        rating += '<li><i class="fa fa-star-o"></i></li>'
                    } else {
                        rating += '<li class="no-star"><i class="fa fa-star-o"></i></li>'
                    }
                }

                let img = `<div class="product-details-images slider-navigation-1">
                            <div class="lg-image">
                                <img src="${mol.origin}/user-content/${data.thumbnailImage}" alt="${data.thumbnailImage}">
                            </div>
                        </div>`;

                let pro = `<h2>${data.name}</h2>
                        <span class="product-details-ref">${data.categories[0]}</span>
                        <div class="rating-box pt-20">
                            <ul class="rating rating-with-review-item">
                                ${rating}
                            </ul>
                        </div>
                        <div class="price-box pt-20">
                            <span class="old-price">đ${app.fmnumber(data.originalPrice)}</span>
                            <span class="new-price new-price-2">đ${app.fmnumber(data.price)}</span>
                        </div>
                        <div class="product-desc">
                            <p>
                                <span>
                                    ${data.description}
                                </span>
                            </p>
                        </div>
                        <div class="single-add-to-cart">
                            <div class="cart-quantity">
                                <button class="add-to-cart" type="button" data-id="${data.id}">Thêm vào giỏ hàng</button>
                            </div>
                        </div>`;

                let des = `<div class="product-description">
                                <span>${data.description}</span>
                            </div>`;
                let det = `<div class="product-details-manufacturer">
                                ${data.details}
                            </div>`;

                $('.product-details-left').html(img);
                $('.product-info').html(pro);
                $('#description').html(des);
                $('#product-details').html(det);
            })
    }

    function sliderProduct() {
        // cắt từ file main.js
        /*----------------------------------------*/
        /* 21. Modal Menu Active
        /*----------------------------------------*/
        $('.product-details-images').each(function () {
            var $this = $(this);
            var $thumb = $this.siblings('.product-details-thumbs, .tab-style-left');
            $this.slick({
                arrows: false,
                slidesToShow: 1,
                slidesToScroll: 1,
                autoplay: false,
                autoplaySpeed: 5000,
                dots: false,
                infinite: true,
                centerMode: false,
                centerPadding: 0,
                asNavFor: $thumb,
            });
        });
        $('.product-details-thumbs').each(function () {
            var $this = $(this);
            var $details = $this.siblings('.product-details-images');
            $this.slick({
                slidesToShow: 4,
                slidesToScroll: 1,
                autoplay: false,
                autoplaySpeed: 5000,
                dots: false,
                infinite: true,
                focusOnSelect: true,
                centerMode: true,
                centerPadding: 0,
                prevArrow: '<span class="slick-prev"><i class="fa fa-angle-left"></i></span>',
                nextArrow: '<span class="slick-next"><i class="fa fa-angle-right"></i></span>',
                asNavFor: $details,
            });
        });
        $('.tab-style-left, .tab-style-right').each(function () {
            var $this = $(this);
            var $details = $this.siblings('.product-details-images');
            $this.slick({
                slidesToShow: 3,
                slidesToScroll: 1,
                autoplay: false,
                autoplaySpeed: 5000,
                dots: false,
                infinite: true,
                focusOnSelect: true,
                vertical: true,
                centerPadding: 0,
                prevArrow: '<span class="slick-prev"><i class="fa fa-angle-down"></i></span>',
                nextArrow: '<span class="slick-next"><i class="fa fa-angle-up"></i></span>',
                asNavFor: $details,
            });
        });
    }
})();
$(document).ready(function () {
    detail.init();

})