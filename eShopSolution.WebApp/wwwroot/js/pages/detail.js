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