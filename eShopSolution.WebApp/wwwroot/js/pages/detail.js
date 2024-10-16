﻿var detail = (function () {
    "use strict";
    var token = app.getcookie("Token");
    var mol = {};
    mol.origin = 'https://localhost:5001';
    mol.o = location.origin;
    mol.productId = location.href.split('/')[location.href.split('/').length - 1];
    mol.init = function () {
        var B = $('body');
        let language = 'vi-VN';
        getProduct(language, mol.productId);

        renderReviews();

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
                            index.getCart(language, token);
                            index.toast({
                                title: "Thành công",
                                message: "Đã thêm sản phẩm vào giỏ hàng.",
                                type: "success",
                                duration: 3000
                            })
                        }
                    },
                    error: function (error) {
                        console.log('Lỗi truy cập vào API: ', error);
                    }
                })
            }
        });

        B.delegate('.review-links', 'click', function () {
            $.ajax({
                url: mol.origin + `/api/Users/info`,
                type: 'GET',
                headers: {
                    'accept': '*/*',
                    'Authorization': 'Bearer ' + token
                },
                data: {
                    languageId: 'vi-VN'
                },
                success: function (response) {
                    if (response.isSuccessed) {
                        const d = response.resultObj;
                        $('#author').val(d.firstName + ' ' + d.lastName);
                        $('#phone').val(d.phoneNumber);
                        $('#email').val(d.email);
                    }
                    console.log(response);
                },
                error: function (xhr, textStatus, error) {
                    console.log(error);
                }
            });
        })

        B.delegate('.feedback-btn .send', 'click', function () {
            if (validateName() && validatePhoneNumber()) {
                var formData = new FormData();
                formData.append('Rating', $('.br-widget .br-selected.br-current').data('rating-value'));
                formData.append('ProductId', mol.productId);
                formData.append('UserId', '');
                formData.append('Content', $('#feedback').val());
                formData.append('Name', mol.name);
                formData.append('Email', $('#email').val());
                formData.append('PhoneNumber', mol.phonenumber);
                formData.append('ImagePath', $('#upload_file')[0].files[0]);
                $.ajax({
                    url: mol.origin + '/api/Reviews',
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false,
                    headers: {
                        accept: '*/*',
                        Authorization: `Bearer ${token}`,
                    },
                    success: function (response) {
                        if (response.isSuccessed) {
                            index.toast({
                                title: "Thành công",
                                message: response.resultObj,
                                type: "success",
                                duration: 3000
                            })
                            $('.close').click();
                        } else {
                            index.toast({
                                title: "Thất bại",
                                message: response.message,
                                type: "error",
                                duration: 3000
                            })
                        }
                    },
                    error: function (error) {
                        console.error('Failed to submit review:', error);
                    }
                });

            }

        })
    }


    return mol;

    function validatePhoneNumber() {
        mol.phonenumber = $('#phone').val().trim();
        if (mol.phonenumber.length === 0) {
            alert('Số điện thoại không được để trống')
            return false;
        }
        const regex = /^(0\d{9})$/;
        if (!regex.test(mol.phonenumber)) {
            alert('Số điện thoại không hợp lệ')
            return false;
        }
        
        return true
    }

    function validateName() {
        mol.name = $('#author').val().trim();
        if (mol.name.length === 0) {
            alert('Tên không được để trống.');
            return false;
        }
        if (mol.name.length < 2 || mol.name.length > 50) {
            alert('Tên không hợp lệ.');
            return false;
        }
        var invalidCharacters = /[^\p{L}\s'-]/gu;
        if (invalidCharacters.test(mol.name)) {
            alert('Tên không hợp lệ.');
            return false;
        }
        return true;
    }


    function renderReviews() {
        $.ajax({
            method: "GET",
            url: mol.origin + `/api/Reviews/product?ProductId=${mol.productId}&PageIndex=1&PageSize=10`,
            headers: {
                accept: '*/*',
            },
            success: function (response) {
                if (response.items.length > 0) {
                    var html = [];
                    
                    response.items.map(i => {
                        let rating = '';
                        let imgs = '';

                        if (i.reviewImage) {
                            i.reviewImage.map(image => {
                                imgs += `<img class=" lazyloaded" data-src="${mol.origin}/user-content/${image.imagePath}" alt="" onclick="goToRSlideByAttID(277809)" src="${mol.origin}/user-content/${image.imagePath}">`
                            })
                        }

                        for (let r = 1; r <= 5; r++) {
                            if (r <= i.rating) {
                                rating += '<li><i class="fa fa-star" aria-hidden="true"></i></li>'
                            } else {
                                rating += '<li class="no-star"><i class="fa fa-star" aria-hidden="true"></i></li>'
                            }
                        }

                        html.push(`<div id="reviews-item">
                            <div class="comment-author-infos">
                                <span class="name">${i.name}</span>
                                <span class="made_a_purchase ml-10">
                                    <i class="fa fa-check-circle" aria-hidden="true"></i>
                                    Đã mua hàng
                                </span>
                            </div>
                            <div class="comment-review">
                                <ul class="rating">
                                    ${rating}
                                </ul>
                                <em class="ml-10">${app.fmdate(i.dateCreated)}</em>
                            </div>
                            <div class="comment-details">
                                <p>${i.content}</p>
                                ${imgs}
                            </div>
                        </div>`);
                    });

                    $('#product-reviews-items').html(html.join(''));
                } else {

                }
            },
            error: function (error) {
                window.location.href = "/pages/404.html";
                console.log('Lỗi truy cập vào API: ', error);
            }
        })
    }

    function getProduct(language, productId) {
        var url = mol.origin + `/api/Products/${language}/${productId}`;
        $.ajax({
            method: "GET",
            url: url
        })
            .done(data => {
                if (data) {
                    mol.data = data;
                    let rating = '';

                    for (let r = 1; r <= 5; r++) {
                        if (r <= data.rating) {
                            rating += '<li><i class="fa fa-star" aria-hidden="true"></i></li>'
                        } else {
                            rating += '<li class="no-star"><i class="fa fa-star" aria-hidden="true"></i></li>'
                        }
                    }

                    let img = `<div class="product-details-images slider-navigation-1">
                            <div class="lg-image">
                                <img src="${mol.origin}/user-content/${data.thumbnailImage}" alt="${data.thumbnailImage}">
                            </div>
                        </div>`;



                    let pro = `<h2>${data.name}</h2>

                                <div class="price-box pt-20 ml-20">
                                    <span class="old-price">&#8363;${app.fmnumber(data.originalPrice)}</span>
                                    <span class="new-price new-price-2">&#8363;${app.fmnumber(data.price)}</span>
                                </div>
                            <ul class="list-group list-group-flush">
                                <li class="list-group-item">
                                    <span>Danh mục</span>
                                    <span class="product-details-ref">${data.categories[0]}</span>
                                </li>
                                <li class="list-group-item">
                                    <span>Thương hiệu</span>
                                    <span class="product-details-ref">${data.brand.name}</span>
                                </li>
                                <li class="list-group-item">
                                    <span>Bảo hành</span>
                                    <span class="product-details-ref">${data.warranty}</span>
                                </li>
                                <li class="list-group-item">
                                    <span>Xuất xứ</span>
                                    <span class="product-details-ref">${data.origin}</span>
                                </li>
                                <li class="list-group-item">
                                    <span>Đánh giá</span>
                                    <div class="rating-box d-inline-block">
                                        <ul class="rating rating-with-review-item">
                                            ${rating}
                                        </ul>
                                    </div>
                                </li>
                        </ul>
                        
                        <div class="single-add-to-cart mt-30">
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

                    $('.li-review-product').html(`<img src="${mol.origin}/user-content/${data.thumbnailImage}" alt="${data.thumbnailImage}">
                        <div class="li-review-product-desc">
                            <p class="li-product-name">${data.name}</p>
                            <p>
                                <span>${data.description}</span>
                            </p>
                        </div>`);
                    $('.product-details-left').html(img);
                    $('.product-info').html(pro);
                    $('#description').html(des);
                    $('#product-details').html(det);

                    $.ajax({
                        method: "GET",
                        url: mol.origin + `/api/Products/brand?brandId=${data.brand.id}&take=4&languageId=${language}`,
                        headers: {
                            accept: '*/*',
                        },
                        success: function (response) {
                            if (response.length > 0) {
                                var html = [];
                                response.map(p => {

                                    let rating = '';

                                    for (let r = 1; r <= 5; r++) {
                                        if (r <= p.rating) {
                                            rating += '<li><i class="fa fa-star" aria-hidden="true"></i></li>'
                                        } else {
                                            rating += '<li class="no-star"><i class="fa fa-star" aria-hidden="true"></i></li>'
                                        }
                                    }

                                    html.push(`
                                    <div class="col-lg-3">
                                        <div class="single-product-wrap">
                                            <div class="product-image">
                                                <a href="${mol.o}/vi-VN/san-pham/${p.id}">
                                                    <img src="${mol.origin}/user-content/${p.thumbnailImage}" alt="${p.thumbnailImage}">
                                                </a>
                                                <span class="sticker">New</span>
                                            </div>
                                            <div class="product_desc">
                                                <div class="product_desc_info">
                                                    <div class="product-review">
                                                        <h5 class="manufacturer">
                                                            <a href="#">${p.categories[0]}</a>
                                                        </h5>
                                                        <div class="rating-box">
                                                            <ul class="rating">
                                                                ${rating}
                                                            </ul>
                                                        </div>
                                                    </div>
                                                    <h4><a class="product_name" href="${mol.o}/vi-VN/san-pham/${p.id}">${p.name}</a></h4>
                                                    <div class="price-box">
                                                        <span class="new-price new-price-2">&#8363;${app.fmnumber(p.price)}</span>
                                                        <span class="old-price">&#8363;${app.fmnumber(p.originalPrice)}</span>
                                                        <span class="discount-percentage">${Math.round(((p.price - p.originalPrice) / p.originalPrice) * 100)}%</span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    `)
                                })

                                $('.owl-carousel').html(html.join(''));
                            } else {
                                console.error('Có lỗi khi cập nhật lượt xem.');
                            }
                        },
                        error: function (error) {
                            console.log('Lỗi truy cập vào API: ', error);
                        }
                    })
                }
            })
            .error(error => {
                console.log('Lỗi truy cập vào API: ', error);
            })

        $.ajax({
            method: "GET",
            url: mol.origin + `/api/Orders/check-purchase?productId=${productId}`,
            headers: {
                accept: '*/*',
                Authorization: `Bearer ${token}`,
            },
            success: function (response) {
                if (response.isSuccessed && response.resultObj) {
                    $('.review-btn').removeClass('d-none');
                } else {
                    $('.review-btn').addClass('d-none');
                }
            },
            error: function (error) {
                console.log('Lỗi truy cập vào API: ', error);
            }
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

    var btnUpload = $("#upload_file"),
        btnOuter = $(".button_outer");
    btnUpload.on("change", function (e) {
        var ext = btnUpload.val().split('.').pop().toLowerCase();
        if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
            $(".error_msg").text("Định dạng ảnh sai...");
        } else {
            $(".error_msg").text("");
            var uploadedFile = URL.createObjectURL(e.target.files[0]);
            setTimeout(function () {
                $("#uploaded_view").find("img").remove();
                $("#uploaded_view").append('<img src="' + uploadedFile + '" />').addClass("show");
            }, 1000);
        }
    });
    $(".file_remove").on("click", function (e) {
        $("#uploaded_view").removeClass("show");
        $("#uploaded_view").find("img").remove();
    });
})