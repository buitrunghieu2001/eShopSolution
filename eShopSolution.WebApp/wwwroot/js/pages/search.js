
var search = (function () {
    "use strict";
    var mol = {};
    mol.origin = 'https://localhost:5001';
    mol.o = location.origin;
    mol.init = function () {
        var B = $('body');
        let page = 1, size = 9, language = 'vi-VN', orderBy = null;
        let token = app.getcookie('Token');
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

        B.delegate('.product-select-box .list .option:not(.selected)', 'click', function () {
            orderBy = $(this).data('value')
            getProducts(language, page, size, orderBy);
        })
    }


    return mol;

    
})();
$(document).ready(function () {
    search.init();

})