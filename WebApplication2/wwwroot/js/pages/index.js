var index = (function () {
    "use strict";
    var mol = {};
    mol.origin = 'https://localhost:5001';
    mol.o = location.origin;
    mol.init = function () {
        var B = $('body');
        var token = app.getcookie("Token");
        var languageId = 'vi-VN';
        // render minicart
        if (token != null) {
            $.ajax({
                method: "GET",
                url: mol.origin + `/api/Carts?languageId=${languageId}`,
                headers: {
                    Authorization: `Bearer ${token}`,
                    accept: '*/*',
                },
                success: function (response) {
                    mol.cart = response;
                    let li = [];
                    for (var i = 0; i < (response.items.length < 2 ? response.items.length : 2); i++) {
                        li.push(`
                            <li>
                                <a href="${mol.o}/vi-VN/san-pham/${response.items[i].productId}" class="minicart-product-image">
                                    <img src="${mol.origin}/user-content/${response.items[i].imagePath}" alt="${response.items[i].name}">
                                </a>
                                <div class="minicart-product-details">
                                    <h6><a href="${mol.o}/vi-VN/san-pham/${response.items[i].productId}">${response.items[i].name}</a></h6>
                                    <span>đ${response.items[i].price} x ${response.items[i].quantity}</span>
                                </div>
                            </li>
                        `)
                    }
                    $('.hm-minicart-trigger .cart-item-count').html(response.items.length)
                    $('.minicart-product-list').html(li.join(''))
                },
                error: function (error) {
                    console.log('Lỗi truy cập vào API: ', error);
                }
            })
        }

        B.delegate('#btn-logout', 'click', function () {
            app.deletecookie('Token');
        });
    }

    return mol;

})();
$(document).ready(function () {
    index.init();

})