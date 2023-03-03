export class GlobalConstants {
    //  #################################  DEFAULT  ##################################
    public static MATINH: string = "45";
    public static title_hethong: string = "Hệ thống quản lý xử phạt hành chính"
    public static donvi_quanly: string = "Công an Tỉnh Quảng Trị"
    //  #################################  SUCCESS  ##################################
    public static success_addnew: string = "Thêm mới thành công!";
    public static success_update: string = "Cập nhật thành công!";
    public static success_delete: string = "Xóa bỏ thành công!";
    public static success_toggle: string = "Chuyển trạng thái thành công!";
    public static success_upload: string = "Upload file thành công!";
    public static success_active: string = "Kích hoạt thành công!";
    public static success_lock_p: string = "Khóa phiếu thành công!";

    //  #################################  WARNING  ##################################
    public static warn_trangthai: string = "Trạng thái của hiện đang khóa không thể thực hiện việc điều chỉnh";
    public static warn_missing_donvi: string = "Anh/chị chưa chọn đơn vị để thực hiện!";
    public static warn_dulieu_macdinh: string ="Dữ liệu mặc định, không được phép chỉnh sửa!";
    public static warn_thutu_valid: string ="Số thứ tự phải lớn hơn 0!";
    public static warn_nghidinh: string ="Anh/chị chưa chọn nghị định để thực hiện";

    //  #################################  FORM TITLE  ##################################
    public static THEMMOI: string = "Thêm mới";
    public static DIEUCHINH: string = "Cập nhật thông tin";
    public static XOA: string = "Xóa thông tin";
    public static UYQUYEN: string = "Ủy quyền xử phạt";
    public static HUYUYQUYEN: string = "Hủy ủy quyền xử phạt";
    public static DIEUCHINHQUYEN: string = "Điều chỉnh quyền";

    //  #################################  Canh bao bat loi cac form  ##################################
    public static CANCUBIENBAN_MISSING: string = 'Anh/chị chưa nhập nội dung căn cứ biên bản/quyết định';
    public static NGHIDINHVIPHAM_MISSING: string = 'Anh/chị chưa nhập nghị định vi phạm';
    public static TANGVAT_MISSING: string = 'Anh/chị chưa nhập tang vật';
    public static NGUOI_LAPBB: string = 'Anh/chị chưa nhập nội dung người lập biên bản';
    //  #################################  REGEX  ##################################
    // public static DDMMYYYY_PATTERN : string = "(0?[1-9]|[12][0-9]|3[01])/(0?[1-9]|1[012])/((?:19|20)[0-9][0-9])";
    // public static MMYYYY_PATTERN : string = "(0?[1-9]|1[012])/((?:19|20)[0-9][0-9])";
    // public static YYYY_PATTERN : string = "((?:19|20)[0-9][0-9])";

    //  #################################  route form  ##################################
    public static MS_01: string = '/quyetdinh-xuphat/ms01-qdxp-kobb';
    public static MS_02: string = '/quyetdinh-xuphat/ms02-qdxp-cobb';
    public static MS_03: string = '/quyetdinh-xuphat/ms03-hoanthihanh-phattien';
    public static MS_04: string = '/quyetdinh-xuphat/ms04-miengiam-phattien';
    public static MS_05: string = '/quyetdinh-xuphat/ms05-nopphattien_nhieulan';
    public static MS_11: string = '/quyetdinh-xuphat/ms11-tichthu_tangvat';
    public static MS_13: string = '/quyetdinh-xuphat/ms13-tieuhuy_tangvat';    
    public static MS_18: string = '/quyetdinh-xuphat/ms18-tamgiu_tangvat';
    public static MS_24: string = '/quyetdinh-xuphat/ms24-truycuu-tnhs';
    public static MS_25: string = '/quyetdinh-xuphat/ms25-tamdinhchi-qdxp';
    public static MS_26: string = '/quyetdinh-xuphat/ms26-chamdut-tamdinhchi-qdxp';
    public static MS_28: string = '/quyetdinh-xuphat/ms28-huybo-qdxpvphc';
    public static MS_43: string = '/quyetdinh-xuphat/ms43-lap-bienban';
    public static MS_44: string = '/quyetdinh-xuphat/ms44-bb-xacminh';
    public static MS_45: string = '/quyetdinh-xuphat/ms45-giaitrinh-tructiep';
    public static MS_50: string = '/quyetdinh-xuphat/ms50-bb-tamgiu-tangvat';
    public static MS_64: string = '/quyetdinh-xuphat/ms64-niemphong-tangvat';
    public static MS_65: string = '/quyetdinh-xuphat/ms65-moniemphong-tangvat';
    
}
