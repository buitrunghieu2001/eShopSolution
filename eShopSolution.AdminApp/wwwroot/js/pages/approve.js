var approve = (function () {
    "use strict";
    var mol = {};
    mol.origin = 'https://localhost:5001';
    mol.o = location.origin;
    mol.init = function () {
        var B = $('body');
        mol.page = 1, mol.size = 10;
        var token = app.getcookie("Token");
        var languageId = 'vi-VN';
        const token = sessionStorage.getItem('token');
        // render minicart
        if (token != null) {
            $.ajax({
                url: mol.origin + `/api/Reviews/waitapproved?PageIndex=${mol.page}&PageSize=${mol.size}`,
                type: "GET",
                headers: {
                    "accept": "*/*",
                    "Authorization": "Bearer " + token
                },
                success: function (response) {
                    if (response.items.length > 0) {
                        var html = [];
                        response.items.map(i => {
                            html.push(`<tr>
                            <td>${i.id}</td>
                            <td>${i.productId}</td>
                            <td>${i.content}</td>
                            <td>${i.rating}</td>
                            <td>${i.name}</td>
                            <td>${i.phoneNumber}</td>
                            <td>test</td>
                            <td>
                                <a href=""><i class="fa fa-pencil-square" aria-hidden="true"></i></a> |
                                <a href=""><i class='fa fa-info-circle' aria-hidden='true'></i></a> |
                                <a href=""><i class='fa fa-trash' aria-hidden='true'></i></a> |
                                <a href=""><i class='fa fa-cog' aria-hidden='true'></i></a>
                            </td>
                        </tr>`)
                        });
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    // Xử lý lỗi từ API
                    console.error(xhr.responseText);
                }
            });

        }
    }

    


    return mol;
})();
$(document).ready(function () {
    approve.init();

})