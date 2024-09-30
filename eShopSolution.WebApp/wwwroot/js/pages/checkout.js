var checkout = (function () {
    "use strict";
    var mol = {};
    mol.origin = 'https://localhost:5001';
    mol.o = location.origin;
    mol.init = function () {
        var B = $('body');
        var token = app.getcookie("Token");
        var languageId = 'vi-VN';


        var Parameter = {
            url: "https://raw.githubusercontent.com/kenzouno1/DiaGioiHanhChinhVN/master/data.json",
            method: "GET",
            responseType: "application/json",
        };

        var promise = axios(Parameter);
        promise.then(function (result) {
            renderCity(result.data);
        });

        $.ajax({
            url: mol.origin + '/api/Carts',
            type: 'GET',
            headers: {
                'accept': '*/*',
                'Authorization': 'Bearer ' + token
            },
            data: {
                languageId: 'vi-VN'
            },
            success: function (response) {
                if (response.items.length > 0) {
                    var html = [];
                    let total = 0;
                    response.items.map(i => {
                        html.push(`
                        <tr class="cart_item">
                            <td class="cart-product-name">
                                <span title="${i.name}">${i.name}</span>
                                <strong class="product-quantity"> × ${i.quantity}</strong></td>
                            <td class="cart-product-total"><span class="amount">&#8363;${app.fmnumber(i.price * i.quantity)}</span></td>
                        </tr>
                        `)
                        total += i.price * i.quantity;
                    })
                    $('.your-order-table table tbody').html(html.join(''));
                    $('.your-order-table table tfoot .cart-subtotal .amount').html('&#8363;' + app.fmnumber(total));
                    $('.your-order-table table tfoot .order-total .amount').html('&#8363;' + app.fmnumber(total));
                }
                else {
                    window.location.href = '/';
                }
                console.log(response);
            },
            error: function (xhr, textStatus, error) {
                // Xử lý lỗi trong trường hợp gọi API không thành công
                console.log(error);
            }
        });


        B.delegate('.order-button-payment > input', 'click', function () {
            if (validateName() && validateShipProvince() && validateShipDistrict() && validateShipCommune() && validateAddress() && validateEmail() && validatePhoneNumber()) {
                var data = JSON.stringify({
                    "shipName": mol.name,
                    "shipAddress": mol.address,
                    "shipEmail": mol.email,
                    "shipPhoneNumber": mol.phonenumber,
                    "shipProvince": mol.province,
                    "shipDistrict": mol.district,
                    "shipCommune": mol.commune,
                    "notes": $('#Notes').val()
                });

                $.ajax({
                    url: mol.origin + '/api/Orders',
                    type: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + token
                    },
                    data: data,
                    success: function (response) {
                        if (response.isSuccessed) {
                            index.toast({
                                title: "Thành công",
                                message: response.resultObj,
                                type: "success",
                                duration: 3000
                            });
                            setTimeout(function () {
                                window.location.href = location.origin;
                            }, 1000);
                        } else {
                            index.toast({
                                title: "Thất bại",
                                message: response.message,
                                type: "error",
                                duration: 3000
                            });

                        }

                    },
                    error: function (xhr, status, error) {
                        // Xử lý khi gọi API không thành công
                        console.log('API call không thành công');
                        console.log(xhr.responseText);
                    }
                });
            }
        }) 

    }

    return mol;

    function validateShipProvince() {
        mol.province = $('#ShipProvince').val();

        if (mol.province === '') {
            alert('Vui lòng chọn tỉnh/thành phố.');
            return false;
        }

        return true;
    }

    function validateShipDistrict() {
        mol.district = $('#ShipDistrict').val();

        if (mol.district === '') {
            alert('Vui lòng chọn quận/huyện.');
            return false;
        }

        return true;
    }

    function validateShipCommune() {
        mol.commune = $('#ShipCommune').val();

        if (mol.commune === '') {
            alert('Vui lòng chọn phường/xã.');
            return false;
        }

        return true;
    }


    function validatePhoneNumber() {
        mol.phonenumber = $('#ShipPhoneNumber').val();
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

    function validateAddress() {
        mol.address = $('#ShipAddress').val();

        if (mol.address.length < 5) {
            alert('Địa chỉ phải chứa ít nhất 5 ký tự.');
            return false;
        }

        var specialChars = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]+/;
        if (specialChars.test(mol.address)) {
            alert('Địa chỉ không được chứa ký tự đặc biệt.');
            return false;
        }
        return true;
    }


    function validateEmail() {
        mol.email = $('#ShipEmail').val();
        var emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;

        if (mol.email.length === 0) {
            alert('Email không được để trống.');
            return false;
        }
        if (!emailRegex.test(mol.email)) {
            alert('Email không hợp lệ.')
            return false;
        }
        return true;
    }

    function validateName() {
        mol.name = $('#ShipName').val();
        if (mol.name.length === 0) {
            alert('Họ và tên không được để trống.');
            return false;
        }
        if (mol.name.length < 2 || mol.name.length > 50) {
            alert('Họ và tên không hợp lệ.');
            return false;
        }
        var invalidCharacters = /[^\p{L}\s'-]/gu;
        if (invalidCharacters.test(mol.name)) {
            alert('Họ và tên không hợp lệ.');
            return false;
        }
        return true;
    }

    function renderCity(data) {
        var citis = document.getElementById("ShipProvince");
        var districts = document.getElementById("ShipDistrict");
        var wards = document.getElementById("ShipCommune");

        for (const x of data) {
            citis.options[citis.options.length] = new Option(x.Name, x.Id);
        }
        $('#ShipProvince').niceSelect('update');
        citis.onchange = function () {
            districts.length = 1;
            wards.length = 1;
            if (this.value != "") {
                const result = data.filter(n => n.Id === this.value);

                for (const k of result[0].Districts) {
                    districts.options[districts.options.length] = new Option(k.Name, k.Id);
                }
                $('#ShipDistrict').niceSelect('update');
            }
        };
        districts.onchange = function () {
            wards.length = 1;
            const dataCity = data.filter((n) => n.Id === citis.value);
            if (this.value != "") {
                const dataWards = dataCity[0].Districts.filter(n => n.Id === this.value)[0].Wards;

                for (const w of dataWards) {
                    wards.options[wards.options.length] = new Option(w.Name, w.Id);
                }
                $('#ShipCommune').niceSelect('update');
            }
        };
    }
})();
$(document).ready(function () {
    checkout.init();
})