
var register = (function () {
    "use strict";
    var mol = {};
    mol.origin = 'https://localhost:5001';
    mol.o = location.origin;
    mol.init = function () {
        var B = $('body');
        let page = 1, size = 9, language = 'vi-VN', orderBy = null;
        let token = app.getcookie('Token');


        B.delegate('#phonenumber', 'input', function () {
            var phoneNumber = $('#phonenumber').val();
            var validPhoneNumber = phoneNumber.replace(/[^\d]/g, "");

            if (validPhoneNumber.length > 10) {
                validPhoneNumber = validPhoneNumber.slice(0, 10);
            }
            $('#phonenumber').val(validPhoneNumber);
        });

        B.delegate('#phonenumber', 'blur', function () {
            validatePhoneNumber();
        });

        B.delegate('#firstname', 'blur', function () {
            validateFirstName();
        });

        B.delegate('#lastname', 'blur', function () {
            validateLastName();
        });

        B.delegate('#email', 'blur', function () {
            validateEmail();
        });

        B.delegate('#password', 'blur', function () {
            validatePassword();
        });

        B.delegate('#confirm-password', 'blur', function () {
            validateConfirmPassword();
        });

        B.delegate('#dob', 'blur', function () {
            validateDateOfBirth();
        });

        B.delegate('#username', 'blur', function () {
            validateUserName();
        });

        B.delegate('.register-button', 'click', function () {

        })
        $('#form-register').submit(function (e) {
            e.preventDefault();
            if (validateFirstName() && validateLastName() && validateEmail() && validatePhoneNumber() && validateDateOfBirth() && validatePassword() && validateConfirmPassword() && validateUserName()) {
                $.ajax({
                    url: "https://localhost:5001/api/Users",
                    type: "POST",
                    headers: {
                        "Accept": "*/*",
                        "Content-Type": "application/json"
                    },
                    data: JSON.stringify({
                        "firstName": mol.firstname,
                        "lastName": mol.lastname,
                        "dob": mol.dob,
                        "email": mol.email,
                        "phoneNumber": mol.phonenumber,
                        "userName": mol.username,
                        "password": mol.password,
                        "confirmPassword": mol.confirmpassword
                    }),
                    success: function (response) {
                        if (response.isSuccessed) {
                            index.toast({
                                title: "Thành công",
                                message: "Đăng ký thành công.",
                                type: "success",
                                duration: 3000
                            })
                            setTimeout(function () {
                                window.location.href = "https://localhost:5003/vi-VN/account/login";
                            }, 1500);
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


    function validateDateOfBirth() {
        var currentDate = new Date();
        mol.dob = $('#dob').val();
        var date = new Date(mol.dob);

        if (isNaN(date.getTime())) {
            altError('input-dob', 'Ngày sinh không được để trống');
            return false;
        }

        if (date > currentDate) {
            altError('input-dob', 'Ngày sinh không hợp lệ');
            return false;
        }
        remError('input-dob');
        return true;
    }


    function validatePhoneNumber() {
        mol.phonenumber = $('#phonenumber').val();
        if (mol.phonenumber.length === 0) {
            altError('input-phonenumber', 'Số điện thoại không được để trống')
            return false;
        }
        const regex = /^(0\d{9})$/;
        if (!regex.test(mol.phonenumber)) {
            altError('input-phonenumber', 'Số điện thoại không hợp lệ')
            return false;
        }
        remError('input-phonenumber');
        return true
    }


    function validateEmail() {
        mol.email = $('#email').val();
        var emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;

        if (mol.email.length === 0) {
            altError('input-email', 'Email không được để trống.');
            return false;
        }
        if (!emailRegex.test(mol.email)) {
            altError('input-email', 'Email không hợp lệ.')
            return false;
        } 
        remError('input-email')
        return true;
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

    function validateConfirmPassword() {
        mol.password = $('#password').val();
        mol.confirmpassword = $('#confirm-password').val();
        if (mol.confirmpassword.length === 0) {
            altError('input-confirm-password', 'Vui lòng xác nhận mật khẩu.');
            return false;
        }
        if (mol.confirmpassword != mol.password) {
            altError('input-confirm-password', 'Mật khẩu không trùng khớp.');
            return false;
        }
        remError('input-confirm-password');
        return true;
    }

    function validateLastName() {
        mol.lastname = $('#lastname').val();
        if (mol.lastname.length === 0) {
            altError('input-lastname', 'Họ không được để trống.');
            return false;
        }
        if (mol.lastname.length < 2 || mol.lastname.length > 50) {
            altError('input-lastname', 'Họ không hợp lệ.');
            return false;
        }
        var invalidCharacters = /[^\p{L}\s'-]/gu;
        if (invalidCharacters.test(mol.lastname)) {
            altError('input-lastname', 'Họ không hợp lệ.');
            return false;
        }
        remError('input-lastname');
        return true;
    }

    function validateFirstName() {
        mol.firstname = $('#firstname').val();
        if (mol.firstname.length === 0) {
            altError('input-firstname', 'Tên không được để trống.');
            return false;
        }
        if (mol.firstname.length < 2 || mol.firstname.length > 50) {
            altError('input-firstname', 'Tên không hợp lệ.');
            return false;
        }
        var invalidCharacters = /[^\p{L}\s'-]/gu;
        if (invalidCharacters.test(mol.firstname)) {
            altError('input-firstname', 'Tên không hợp lệ.');
            return false;
        }
        remError('input-firstname');
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
    register.init();

})