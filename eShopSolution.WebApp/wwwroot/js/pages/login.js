var login = (function () {
    "use strict";
    var mol = {};
    mol.origin = 'https://localhost:5001';
    mol.o = location.origin;
    mol.init = function () {
        var B = $('body');
        var token = app.getcookie("Token");
        var languageId = 'vi-VN';

        $('.register-button').click(function (e) {
            e.preventDefault();
            mol.rememberme = $('#remember_me').val();
            if (validateUserName() && validatePassword()) {
                $.ajax({
                    url: mol.origin + "/api/Users/authenticate",
                    type: "POST",
                    headers: {
                        "Accept": "*/*",
                        "Content-Type": "application/json"
                    },
                    data: JSON.stringify({
                        "userName": mol.username,
                        "password": mol.password,
                        "rememberMe": Boolean(mol.rememberme)
                    }),
                    success: function (response) {
                        if (response.isSuccessed) {
                            $('#form-login').submit();
                        } else {
                            index.toast({
                                title: "Thất bại",
                                message: response.message,
                                type: "error",
                                duration: 3000
                            })
                        }
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        index.toast({
                            title: "Thất bại",
                            message: xhr.responseJSON.message,
                            type: "error",
                            duration: 3000
                        })
                    }
                });
            }
        })
    }
    return mol;



    function validateUserName() {
        var userNameRegex = /^[a-zA-Z0-9_-]{3,16}$/;
        mol.username = $('#username').val();
        if (username.length === 0) {
            altError('input-username', 'Tên tài khoản không được để trống');
            return false;
        }
        if (!userNameRegex.test(mol.username)) {
            altError('input-username', 'Tên tài khoản không hợp lệ');
            return false;
        }
        remError('input-username');
        return true
    }

    function validatePassword() {
        mol.password = $('#password').val();
        if (mol.password.length === 0) {
            altError('input-password', 'Mật khẩu không được để trống.')
            return false;
        }
        if (mol.password.length < 8) {
            altError('input-password', 'Mật khẩu tối thiểu 8 ký tự.')
            return false;
        }

        var uppercaseRegex = /[A-Z]/;
        var lowercaseRegex = /[a-z]/;
        var numberRegex = /[0-9]/;
        var specialCharRegex = /[!@#$%^&*]/;

        if (!uppercaseRegex.test(mol.password) || !lowercaseRegex.test(mol.password) ||
            !numberRegex.test(mol.password) || !specialCharRegex.test(mol.password)) {
            altError('input-password', 'Mật khẩu tối thiểu 8 ký tự.Mật khẩu bao gồm: chữ hoa, chữ thường, số và ký tự đặc biệt.');
            return false;
        }
        remError('input-password')
        return true;
    }

    function altError(id, err) {
        $(`#${id}`).addClass('error')
        $(`#${id}`).find('span').html(err)
    }

    function remError(id) {
        $(`#${id}`).removeClass('error')
        $(`#${id}`).find('span').html('')
    }
})();
$(document).ready(function () {
    login.init();
})