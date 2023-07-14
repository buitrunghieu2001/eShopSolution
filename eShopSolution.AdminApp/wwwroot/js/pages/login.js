var login = (function () {
    "use strict";
    var mol = {};
    mol.origin = 'https://localhost:5001';
    mol.o = location.origin;
    mol.init = function () {
        var B = $('body');
        var languageId = 'vi-VN';

        B.delegate('#btn-dangnhap', 'click', function (e) {
            e.preventDefault();
            mol.rememberme = $('#rememberme').prop("checked");
            if (validateUserName() && validatePassword()) {
                $.ajax({
                    url: mol.origin + "/api/Users/authenticate",
                    type: "POST",
                    headers: {
                        "accept": "*/*",
                        "Content-Type": "application/json"
                    },
                    data: JSON.stringify({
                        "userName": mol.username,
                        "password": mol.password,
                        "rememberMe": mol.rememberme
                    }),
                    success: function (response) {
                        if (response.isSuccessed) {
                            sessionStorage.setItem('token', response.resultObj);
                            setTimeout(function () {
                                $('#login').submit();
                            }, 500);
                        } else {
                            index.toast({
                                title: "Thất bại",
                                message: response.message,
                                type: "error",
                                duration: 3000
                            })
                        }
                        console.log(response);
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        
                        console.error(xhr.responseText);
                    }
                });
            }

            return false;
        })
    }


    return mol;

    function validateUserName() {
        var userNameRegex = /^[a-zA-Z0-9_-]{3,16}$/;
        mol.username = $('#username').val();
        if (username.length === 0) {
            alert('Tên tài khoản không được để trống');
            return false;
        }
        if (!userNameRegex.test(mol.username)) {
            alert( 'Tên tài khoản tồn tại');
            return false;
        }
        return true
    }

    function validatePassword() {
        mol.password = $('#password').val();
        if (mol.password.length === 0) {
            alert('Mật khẩu không được để trống.')
            return false;
        }
        if (mol.password.length < 8) {
            alert('Mật khẩu không đúng.')
            return false;
        }

        var uppercaseRegex = /[A-Z]/;
        var lowercaseRegex = /[a-z]/;
        var numberRegex = /[0-9]/;
        var specialCharRegex = /[!@#$%^&*]/;

        if (!uppercaseRegex.test(mol.password) || !lowercaseRegex.test(mol.password) ||
            !numberRegex.test(mol.password) || !specialCharRegex.test(mol.password)) {
            alert('Mật khẩu không đúng.');
            return false;
        }
        return true;
    }
})();
$(document).ready(function () {
    login.init();

})