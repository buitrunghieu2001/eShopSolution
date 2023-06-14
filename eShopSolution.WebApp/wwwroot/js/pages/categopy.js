var index = (function () {
    "use strict";
    var mol = {};
    mol.origin = 'https://localhost:5001';
    mol.o = location.origin;
    mol.init = function () {
        var B = $('body');
        var token = app.getcookie("Token");
        var languageId = 'vi-VN';


    }

    return mol;

})();
$(document).ready(function () {
    index.init();

})