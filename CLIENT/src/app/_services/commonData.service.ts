import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import * as moment from 'moment';
import { Observable } from 'rxjs';

const baseUrl = `${environment.apiURL}/api/nhomnd`;

@Injectable({
    providedIn: 'root'
})

export class commonDataService {
    chuc: any;
    donvi: any;
    tram: any;
    nghin: any;
    trieu: any;
    ty: any;
    mangso = ['không', 'một', 'hai', 'ba', 'bốn', 'năm', 'sáu', 'bảy', 'tám', 'chín'];

    constructor(private http: HttpClient) { }

    isDate(value) {
        var re = /^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-.\/])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$/;
        var flag = re.test(value);
        return flag;
    };

    check_date(value) {
        if (value.length == 4) {
            var const_value = '01/01/' + value
            var flag = this.isDate(const_value);
            return flag;
        } else if (value.length == 7) {
            var const_value = '01/' + value
            var flag = this.isDate(const_value);
            return flag;
        } else if (value.length == 10) {
            var flag = this.isDate(value);
            return flag;
        } else {
            var const_value = '01' + value
            var flag = this.isDate(const_value);
            return flag;
        }
    }

    ///---------------------------------------------------------------------------------
    ///Convet số thành chữ
    ///---------------------------------------------------------------------------------
    dochangchuc(so, daydu) {
        var chuoi = "";
        this.chuc = Math.floor(so / 10);
        this.donvi = so % 10;
        if (this.chuc > 1) {
            chuoi = " " + this.mangso[this.chuc] + " mươi";
            if (this.donvi == 1) {
                chuoi += " mốt";
            }
        } else if (this.chuc == 1) {
            chuoi = " mười";
            if (this.donvi == 1) {
                chuoi += " một";
            }
        } else if (daydu && this.donvi > 0) {
            chuoi = " lẻ";
        }
        if (this.donvi == 5 && this.chuc > 1) {
            chuoi += " lăm";
        } else if (this.donvi > 1 || (this.donvi == 1 && this.chuc == 0)) {
            chuoi += " " + this.mangso[this.donvi];
        }
        return chuoi;
    }

    docblock(so, daydu) {
        var chuoi = "";
        this.tram = Math.floor(so / 100);
        so = so % 100;
        if (daydu || this.tram > 0) {
            chuoi = " " + this.mangso[this.tram] + " trăm";
            chuoi += this.dochangchuc(so, true);
        } else {
            chuoi = this.dochangchuc(so, false);
        }
        return chuoi;
    }

    dochangtrieu(so, daydu) {
        var chuoi = "";
        this.trieu = Math.floor(so / 1000000);
        so = so % 1000000;
        if (this.trieu > 0) {
            chuoi = this.docblock(this.trieu, daydu) + " triệu";
            daydu = true;
        }
        this.nghin = Math.floor(so / 1000);
        so = so % 1000;
        if (this.nghin > 0) {
            chuoi += this.docblock(this.nghin, daydu) + " nghìn";
            daydu = true;
        }
        if (so > 0) {
            chuoi += this.docblock(so, daydu);
        }
        return chuoi;
    }

    docsosangchu = function docso(so) {
        if (so == 0) return this.mangso[0];
        var chuoi = "", hauto = "";
        do {
            this.ty = so % 1000000000;
            so = Math.floor(so / 1000000000);
            if (so > 0) {
                chuoi = this.dochangtrieu(this.ty, true) + hauto + chuoi;
            } else {
                chuoi = this.dochangtrieu(this.ty, false) + hauto + chuoi;
            }
            hauto = " tỷ";
        } while (so > 0);
        return chuoi + " đồng chẵn";
    }

    ///---------------------------------------------------------------------------------
    ///Phục vụ bắt lỗi form
    ///---------------------------------------------------------------------------------

    check_delete_item(list: any) {
        let kq = list.filter(i => i.TRANGTHAI == 2);
        return kq.length == list.length;
    }

    format_ngay_gio_pick(ngay, gio, phut) {
        return (
            moment(ngay + ' ' + gio + ':' + phut, 'YYYY/MM/DD HH:mm')
        );
    }

    format_ngay_gio_string(ngay, gio, phut) {
        return (
            moment(ngay + ' ' + gio + ':' + phut, 'DD/MM/YYYY HH:mm')
        );
    }

    format_ngay_gio(ngay, gio, phut) {
        return (
            moment(ngay + ' ' + gio + ':' + phut, ['YYYY/MM/DD HH:mm', 'DD/MM/YYYY HH:mm']).format()
        );
    }

    ///---------------------------------------------------------------------------------
    ///Mặc định
    ///---------------------------------------------------------------------------------
    macdinh() {
        var tt = [
            { ma: 0, ten: 'Không' },
            { ma: 1, ten: 'Mặc định' },
        ];
        return tt;
    }
    ///---------------------------------------------------------------------------------
    ///Hình thức huye QĐ XP VPHC
    ///---------------------------------------------------------------------------------
    huy_xpvphc() {
        var tt = [
            { ma: 1, ten: 'Một phần' },
            { ma: 2, ten: 'Toàn bộ' },
        ];
        return tt;
    }

    lydo_tieuhuy() {
        var tt = [
            { ma: 1, ten: 'Không ra quyết định xử phạt vi phạm hành chính theo các trường hợp quy định tại các Điểm a, b, c và d Khoản 1 Điều 65 Luật Xử lý vi phạm hành chính' },
            { ma: 2, ten: 'Tang vật, phương tiện vi phạm hành chính không còn giá trị sử dụng hoặc không bán đấu giá được quy định tại Khoản 1 Điều 82 Luật Xử lý vi phạm hành chính' },
            { ma: 3, ten: 'Tang vật, phương tiện vi phạm hành chính là hàng hóa, vật phẩm gây hại cho sức khỏe con người, vật nuôi, cây trồng và môi trường, văn hóa phẩm độc hại quy định tại Khoản 5 Điều 126 Luật Xử lý vi phạm hành chính' },
            { ma: 4, ten: 'Tang vật vi phạm hành chính là các chất ma túy và những vật thuộc loại cấm lưu hành quy định tại Khoản 6 Điều 126 Luật Xử lý vi phạm hành chính' }
        ]
        return tt;
    }

    printDiv(divName) {
        var printContents = document.getElementById(divName).innerHTML;
        var originalContents = document.body.innerHTML;
        var newWin = window.open('', 'Print-Window');
        var head = '<head><link href="/assets/custom.css"  rel="stylesheet"></head>';
        newWin.document.open();
        newWin.document.write('<html>' + head + '<body onload="window.print()">' + printContents + '</body></html>');
        newWin.document.close();
        setTimeout(function () { newWin.close(); }, 10);

    }

    previewDiv(divName) {
        var printContents = document.getElementById(divName).innerHTML;
        var originalContents = document.body.innerHTML;
        var head = '<head><link href="/assets/custom.css"  rel="stylesheet"></head>';
        window.document.write('<html>' + head + '<body">' + printContents + '</body></html>');
    }
}
