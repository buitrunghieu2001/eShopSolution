
var app = (function () {
    "use strict";
    var mol = {};
    mol.init = function () {
        var B = $('body');
    }

    mol.fmnumber = function (v) {
        v = String(v);
        var s = v.split('.');
        var s0 = s[0];
        if (s0 < 4) return v;
        var a = [];
        var j = 0;
        for (var i = s0.length - 1; i > -1; i--) {
            var b = s0[i];
            j++;
            if (j % 3 == 0 && i > 0) {
                a.push(b);
                a.push(',');
            }
            else a.push(b);
        }
        var c = a.reverse().join('');
        if (s.length == 2) c += '.' + s[1];
        return c.replace(/\./g, '!').replace(/\,/g, '.').replace(/\!/g, ',');
    }

    mol.fmNumberToNumber = function (v) {
        return v.replace(/\./g, '').replace(/,/g, '.');
    }

    mol.phantrang = function (iPage, sum, row) {
        var stemp = "";
        if (sum > row) {
            var sotrang = Math.ceil(sum / row);
            stemp += " <li " + (iPage == 1 ? "class='disabled'" : "class='pprev'") + "><i class='fa fa-chevron-left'></i> Previous</li>";
            //(sotrang > row ? row : sotrang)
            if (iPage < 4 || sotrang < 5) {
                var n = sotrang < 5 ? sotrang : 5;
                for (var i = 0; i < n; i++) { stemp += "<li class='pagination1" + (i == iPage - 1 ? " active" : "") + "' id ='" + (i + 1) + "'><a >" + (i + 1) + "</a></li>"; }
            }
            else {
                if (iPage < sotrang - 2) {
                    for (var i = iPage - 3; i < Number(iPage) + Number(2); i++) { stemp += "<li class='pagination1" + (i == iPage - 1 ? " active" : "") + "' id ='" + (i + 1) + "'><a >" + (i + 1) + "</a></li>"; }
                }
                else {
                    for (var i = sotrang - 5; i < sotrang; i++) {
                        //  alert(i);
                        stemp += "<li class='pagination1" + (i == iPage - 1 ? " active" : "") + "' id ='" + (i + 1) + "'><a >" + (i + 1) + "</a></li>";
                    }
                }
            }
            stemp += " <li " + (iPage == sotrang ? "class='disabled'" : "class='pnext'") + "> Next <i class='fa fa-chevron-right'></i></li>";
        }
        return stemp;
    }

    mol.setcookie = function (name, value, days) {
        var expirationDate = new Date();
        expirationDate.setDate(expirationDate.getDate() + days);

        var cookie = encodeURIComponent(name) + '=' + encodeURIComponent(value) + '; expires=' + expirationDate.toUTCString() + '; path=/';
        document.cookie = cookie;
    }

    mol.getcookie = function (name) {
        var cookieName = encodeURIComponent(name) + '=';
        var cookieArray = document.cookie.split(';');

        for (var i = 0; i < cookieArray.length; i++) {
            var cookie = cookieArray[i];

            while (cookie.charAt(0) === ' ') {
                cookie = cookie.substring(1);
            }

            if (cookie.indexOf(cookieName) === 0) {
                return decodeURIComponent(cookie.substring(cookieName.length));
            }
        }

        return null;
    }

    mol.deletecookie = function (name) {
        document.cookie = encodeURIComponent(name) + '=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
    }

    mol.fmdate = function (d) {
        if (d == null || d == "" || d == 'null') return "";
        var a = d.split('T')[0].split('-');
        return a[2] + '/' + a[1] + '/' + a[0];
    }

    return mol;
})();
$(document).ready(function () {

    app.init();

})