var approve = (function () {
    "use strict";
    var mol = {};
    mol.origin = 'https://localhost:5001';
    mol.o = location.origin;
    mol.init = function () {
        var B = $('body');
        mol.page = 1, mol.size = 10;
        var languageId = 'vi-VN';
        mol.token = sessionStorage.getItem('token');
        // render minicart
        if (mol.token != null) {
            getReviews();

            B.delegate(".pagination1", "click", function () {
                if (mol.page != $(this).attr("id")) {
                    mol.page = $(this).attr("id");
                    getReviews();
                }
            })

            B.delegate(".pnext", "click", function () {
                if (typeof (mol.page) == "string")
                    mol.page = parseInt(mol.page)
                mol.page += 1;
                getReviews();
            })

            B.delegate(".pprev", "click", function () {
                if (typeof (mol.page) == "string")
                    mol.page = parseInt(mol.page)
                mol.page -= 1;
                getReviews();
            })

            B.delegate('#btn-approved', 'click', function () {
                const id = $(this).data('id');
                $('#approvedModal #btn-confirm-approved').data('id', id);
            })

            B.delegate('#btn-delete', 'click', function () {
                const id = $(this).data('id');
                $('#deleteModal #btn-confirm-delete').data('id', id);
            })

            B.delegate('#btn-confirm-approved', 'click', function () {
                const id = $(this).data('id');

                if (id) {
                    $.ajax({
                        url: mol.origin + '/api/Reviews/approved?reviewId=' + id,
                        type: 'PATCH',
                        headers: {
                            'accept': '*/*',
                            "Authorization": "Bearer " + mol.token
                        },
                        success: function (response) {
                            $(`#btn-approved[data-id="${id}"]`).closest('tr').remove();
                            $('#msgAlert').html(response.resultObj)
                            setTimeout(function () {
                                $('#msgAlert').fadeOut('slow');
                            }, 2000);

                            $('#table-danh-gia tbody tr').each(function (index) {
                                $(this).find('td:first').text(index + 1);
                            });
                        },
                        error: function (xhr, status, error) {
                            console.error('PATCH request failed. Status:', status);
                        }
                    });

                }
            })

            B.delegate('#btn-confirm-delete', 'click', function () {
                const id = $(this).data('id');
                if (id) {
                    $.ajax({
                        url: mol.origin + '/api/Reviews/disapproved?reviewId=' + id,
                        type: 'PATCH',
                        headers: {
                            'accept': '*/*',
                            "Authorization": "Bearer " + mol.token
                        },
                        success: function (response) {
                            $(`#btn-approved[data-id="${id}"]`).closest('tr').remove();
                            $('#msgAlert').html(response.resultObj)
                            setTimeout(function () {
                                $('#msgAlert').fadeOut('slow');
                            }, 2000);

                            $('#table-danh-gia tbody tr').each(function (index) {
                                $(this).find('td:first').text(index + 1);
                            });
                        },
                        error: function (xhr, status, error) {
                            console.error('DELETE request failed. Status:', status);
                        }
                    });

                }
            })

            B.delegate('#btn-info', 'click', function () {
                const id = $(this).data('id');
                if (id) {
                    $.ajax({
                        method: "GET",
                        url: `${mol.origin}/api/Reviews/${id}`,
                        headers: {
                            'accept': '*/*',
                            "Authorization": "Bearer " + mol.token
                        },
                        success: function (response) {
                            let html = `
                            <div class="row">
                                <label for="staticName" class="col-sm-3 col-form-label fw-bold">Sản phẩm</label>
                                <div class="col-sm-9 col-form-label">
                                    <span>${response.product}</span>
                                </div>
                            </div>
                            <div class="row">
                                <label for="staticDescription" class="col-sm-3 col-form-label fw-bold">Khách hàng</label>
                                <div class="col-sm-9 col-form-label">
                                    <span>${response.name}</span>
                                </div>
                            </div>
                            <div class="row">
                                <label for="staticPrice" class="col-sm-3 col-form-label fw-bold">Số điện thoại</label>
                                <div class="col-sm-9 col-form-label">
                                    <span>${response.phoneNumber}</span>
                                </div>
                            </div>
                            <div class="row">
                                <label for="staticStatus" class="col-sm-3 col-form-label fw-bold">Email</label>
                                <div class="col-sm-9 col-form-label">
                                    <span>${response.email}</span>
                                </div>
                            </div>
                            <div class="row">
                                <label for="staticUrl" class="col-sm-3 col-form-label fw-bold">Nội dung</label>
                                <div class="col-sm-9 col-form-label">
                                    <span>${response.content}</span>
                                </div>
                            </div>
                            <div class="row">
                                <label for="staticUrl" class="col-sm-3 col-form-label fw-bold">Số sao</label>
                                <div class="col-sm-9 col-form-label">
                                    <span>${response.rating}</span>
                                </div>
                            </div>
                            <div class="row">
                                <label for="staticStatus" class="col-sm-3 col-form-label fw-bold">Thời gian đánh giá</label>
                                <div class="col-sm-9 col-form-label">
                                    <span>${app.fmdate(response.dateCreated)}</span>
                                </div>
                            </div>
                            `;

                            if (isNaN(new Date(response.dateUpdated).getTime())) {
                                html += `<div class="row">
                                        <label for="staticStatus" class="col-sm-3 col-form-label fw-bold">Thời gian cập nhật</label>
                                        <div class="col-sm-9 col-form-label">
                                            <span>${app.fmdate(response.dateUpdated)}</span>
                                        </div>
                                    </div>`;
                            }

                            if (response.reviewImages) {
                                html += `<div class="row">
                                            <label for="staticStatus" class="col-sm-3 col-form-label fw-bold">Hình ảnh</label>
                                            <div class="col-sm-9 col-form-label">`;
                                response.reviewImages.map(x => {
                                    html += `<img src="${mol.origin}/user-content/${x.imagePath}" alt="${x.caption}" style="width: 300px; height: 300px; object-fit: cover;">`;
                                })
                                html += `</div></div>`;
                            }
                            $('#infoModal .modal-body').html(html);
                        },
                        error: function (error) {
                            console.log('Lỗi truy cập vào API: ', error);
                        }
                    })
                }
            })
        }

    }
    return mol;

    function getReviews() {
        $.ajax({
            url: mol.origin + `/api/Reviews/waitapproved?PageIndex=${mol.page}&PageSize=${mol.size}`,
            type: "GET",
            headers: {
                "accept": "*/*",
                "Authorization": "Bearer " + mol.token
            },
            success: function (response) {
                if (response.items.length > 0) {
                    var html = [];
                    let tt = 1;
                    response.items.map(i => {
                        html.push(`<tr>
                            <td>${tt++}</td>
                            <td class="clamp-lines">${i.product}</td>
                            <td>${i.rating}</td>
                            <td class="clamp-lines">${i.content}</td>
                            <td>${i.name}</td>
                            <td>${i.phoneNumber}</td>
                            <td>${app.fmdate(i.dateCreated)}</td>
                            <td style="white-space: nowrap;">
                                <a id="btn-info" data-id="${i.id}" data-bs-toggle="modal" data-bs-target="#infoModal">
                                    <i class='fa fa-info-circle' aria-hidden='true'></i>
                                </a> | 
                                <a id="btn-approved" data-id="${i.id}" data-bs-toggle="modal" data-bs-target="#approvedModal">
                                  <i class="fa fa-check-square" aria-hidden="true"></i>
                                </a> | 
                                <a id="btn-delete" data-id="${i.id}" data-bs-toggle="modal" data-bs-target="#deleteModal">
                                    <i class="fa fa-ban" aria-hidden="true"></i>
                                </a>
                            </td>
                        </tr>`)
                    });

                    $('#table-danh-gia > tbody').html(html.join());
                    $('#phan-trang').html(app.phantrang(mol.page, response.totalRecords, mol.size));
                } else {
                    $('#duyet-danh-gia').html('Không có sản phẩm cần duyệt.')
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                // Xử lý lỗi từ API
                console.error(xhr.responseText);
            }
        });
    }
})();
$(document).ready(function () {
    approve.init();

})