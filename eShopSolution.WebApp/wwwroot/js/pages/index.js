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
                    if (response.items.length > 0) {
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
                        $('.minicart-product-list').html(li.join(''))

                    } else {
                        $('.minicart').html(`
                            <div class="cart-empty">
                                <div class="cart-empty-img"></div>
                                <div class="cart-empty-mess">Giỏ hàng của bạn còn trống</div>
                            </div>
                        `);
                    }
                    $('.hm-minicart-trigger .cart-item-count').html(response.items.length)
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

    function showSuccessToast(config) {
        mol.toast(config);
    }

    function showErrorToast(config) {
        mol.toast(config);
    }

    mol.toast = function ({ title = "", message = "", type = "info", duration = 3000 }) {
        const main = document.getElementById("toast");
        if (main) {
            const toast = document.createElement("div");

            // Auto remove toast
            const autoRemoveId = setTimeout(function () {
                main.removeChild(toast);
            }, duration + 1000);

            // Remove toast when clicked
            toast.onclick = function (e) {
                if (e.target.closest(".toast__close")) {
                    main.removeChild(toast);
                    clearTimeout(autoRemoveId);
                }
            };

            const icons = {
                success: "fa fa-check-circle",
                info: "fa fa-info-circle",
                warning: "fa fa-exclamation-circle",
                error: "fa fa-times-circle"
            };
            const icon = icons[type];
            const delay = (duration / 1000).toFixed(2);

            toast.classList.add("toast", `toast--${type}`);
            toast.style.animation = `slideInLeft ease .3s, fadeOut linear 1s ${delay}s forwards`;

            toast.innerHTML = `
                    <div class="toast__icon">
                        <i class="${icon}"></i>
                    </div>
                    <div class="toast__body">
                        <h3 class="toast__title">${title}</h3>
                        <p class="toast__msg">${message}</p>
                    </div>
                    <div class="toast__close">
                        <i class="fa fa-times" aria-hidden="true"></i>
                    </div>
                `;
            main.appendChild(toast);
        }
    }


    return mol;
})();
$(document).ready(function () {
    index.init();

})