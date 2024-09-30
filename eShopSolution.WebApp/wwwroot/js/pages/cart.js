var cart = (function () {
    "use strict";
    var mol = {};
    mol.origin = 'https://localhost:5001';
    mol.o = location.origin;
    mol.init = function () {
        var B = $('body')
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
                    mol.data = response.items;
                    if (mol.data.length == 0) window.location.href = '/';
                    let tr = [];
                    response.items.map(i => {
                        tr.push(`<tr id=${i.id}>
                                    <td class="li-product-remove">
                                        <a data-toggle="modal" data-target="#confirmRemoveCart"><i class="fa fa-times"></i></a>
                                    </td>
                                    <td class="li-product-thumbnail"><a href="${mol.o}/vi-VN/san-pham/${i.productId}"><img src="${mol.origin}/user-content/${i.imagePath}" alt="${i.name}"></a></td>
                                    <td class="li-product-name"><a href="${mol.o}/vi-VN/san-pham/${i.productId}">${i.name}</a></td>
                                    <td class="li-product-price"><span class="amount">${app.fmnumber(i.price)}</span></td>
                                    <td class="quantity">
                                        <div class="cart-plus-minus">
                                            <input class="cart-plus-minus-box" aria-valuenow="0" value="${app.fmnumber(i.quantity)}" type="text">
                                            <div class="dec qtybutton"><i class="fa fa-angle-down"></i></div>
                                            <div class="inc qtybutton"><i class="fa fa-angle-up"></i></div>
                                        </div>
                                    </td>
                                    <td class="product-subtotal"><span class="amount">${app.fmnumber(i.price * i.quantity)}</span></td>
                                </tr>`)
                    })
                    $('.table tbody').html(tr.join(''))

                    updateTotal(response.items);
                },
                error: function (error) {
                    console.log('Lỗi truy cập vào API: ', error);
                }
            })
        }

        B.delegate('.cart-plus-minus-box', 'blur', function () {
            let cart = $(this).closest('tr');
            let cartId = cart.attr('id');
            let quantity = $(this).val();
            let price = parseFloat(cart.find('.li-product-price .amount').html().replace(/\./g, "").replace(",", ".")); 
            cart.find('.product-subtotal .amount').html(app.fmnumber(quantity * price))
            if (quantity > 0) {
                $.ajax({
                    method: "PUT",
                    url: mol.origin + `/api/Carts?cartId=${cartId}&quantity=${quantity}`,
                    headers: {
                        Authorization: `Bearer ${token}`,
                        accept: '*/*',
                    },
                    success: function (response) {

                    },
                    error: function (error) {
                        alert('Vui lòng thử lại sau');
                    }
                })
            } else {
                $(this).val(1);
                cart.find('.product-subtotal .amount').html(app.fmnumber(price))
            }
            updateTotal();
        });

        B.delegate('.cart-plus-minus-box', 'input', function () {
            validateInput($(this));
            updateTotal();
        });


        B.delegate('.coupon input.button', 'click', function () {
            index.toast({
                title: "Thông tin",
                message: "Chức năng đang được phát triển.",
                type: "info",
                duration: 3000
            });
        })
      
        /*----------------------------------------*/
        /* 22. Cart Plus Minus Button
        /*----------------------------------------*/
        $(".cart-plus-minus").append('<div class="dec qtybutton"><i class="fa fa-angle-down"></i></div><div class="inc qtybutton"><i class="fa fa-angle-up"></i></div>');
        $('body').delegate(".qtybutton", "click", function () {
            var $button = $(this);
            var oldValue = $button.parent().find("input").val();
            if ($button.hasClass('inc')) {
                var newVal = parseFloat(oldValue) + 1;
            } else {
                // Don't allow decrementing below zero
                if (oldValue > 1) {
                    var newVal = parseFloat(oldValue) - 1;
                } else {
                    newVal = 1;
                    $(".li-product-remove > a").click();
                }
            }
            $button.parent().find("input").val(newVal);
            let cart = $(this).closest('tr');
            let cartId = cart.attr('id');
            let quantity = cart.find('.cart-plus-minus-box').val();
            let price = parseFloat(cart.find('.li-product-price .amount').html().replace(/\./g, "").replace(",", "."));
            console.log(price)
            cart.find('.product-subtotal .amount').html(app.fmnumber(quantity * price))
            $.ajax({
                method: "PUT",
                url: mol.origin + `/api/Carts?cartId=${cartId}&quantity=${quantity}`,
                headers: {
                    Authorization: `Bearer ${token}`,
                    accept: '*/*',
                },
                success: function (response) {
                    updateTotal();
                },
                error: function (error) {
                    alert('Vui lòng thử lại sau');
                }
            })
        });

        B.delegate('.li-product-remove a', 'click', function () {
            let cart = $(this).closest('tr');
            let cartId = cart.attr('id');
            let nameProduct = mol.data.find(i => i.id == cartId).name;
            $('#confirmRemoveCart .modal-body').html(nameProduct)
            $('#confirmRemoveCart').attr('data-id', cartId);
            updateTotal();
            if (nameProduct) {
                mol.data = mol.data.filter(i => i.id != cartId);
            }
        });

        B.delegate('.btn-accept', 'click', function () {
            let cartId = $(this).closest('#confirmRemoveCart').attr('data-id');
            if (cartId) {
                $.ajax({
                    method: "DELETE",
                    url: mol.origin + `/api/Carts?cartId=${cartId}`,
                    headers: {
                        Authorization: `Bearer ${token}`,
                        accept: '*/*',
                    },
                    success: function (response) {
                        if (response >= 0) {
                            console.log(response)
                            $(`#${cartId}`).remove();
                            $('.hm-minicart-trigger .cart-item-count').html(response);
                            updateTotal();
                            if (response == 0) window.location.href = '/';
                        }
                    },
                    error: function (error) {
                        alert('Vui lòng thử lại sau');
                    }
                })
            }
        });
    }

    return mol;

    function validateInput(input) {
        var value = input.val();
        value = value.replace(/[^0-9]/g, '');

        if (value.length > 0 && parseInt(value) > 0) {
            input.val(value);
        } else {
            input.val('');
        }
    }

    function updateTotal() {
        let total = 0;
        $('.product-subtotal .amount').map(function () {
            total += parseFloat($(this).html().replace(/\./g, "").replace(",", "."));
        });
        let subtotal = parseFloat($('.cart-page-total .subtotal span').html());
        $('.cart-page-total .total-payment span').html('&#8363;' + app.fmnumber(total - subtotal));
    }
})();
$(document).ready(function () {
    cart.init();

})