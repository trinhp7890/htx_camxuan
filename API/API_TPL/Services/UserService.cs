using API_TPL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API_TPL.DAL;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Web.Http;

namespace API_TPL.Services
{
    public class UserService
    {
        DBHelper helper = new DBHelper();
        public User ValidateUser(string username, string password)
        {
            
            try
            {
                // get thong tin user login
                string _password = HashHelper.GetMd5Hash(password).ToUpper();
                DataTable kq = getUserLogin(username, _password);

                // lay số lần login fail trong table config
                int loginfail = Solan_lockAccount();
                User item = new User();
                if (kq!= null)
                {
                    DataRow row = kq.Rows[0];

                    item.Id = Int32.Parse(row["ID_ND"].ToString());
                    item.Name = row["TEN_ND"].ToString();
                    item.UserName = row["MA_ND"].ToString();
                    item.Password = row["PASSWORD"].ToString();
                    item.Email = row["EMAIL"].ToString();
                    item.No_lock_Acount = loginfail;
                    item.Madonvi = row["MA_DV"].ToString();
                    item.Manhanvien = row["MA_NV"].ToString();
                    //trinhp update
                    int useractive = int.Parse(string.IsNullOrEmpty(row["ACTIVE"].ToString()) ? "0" : row["ACTIVE"].ToString());

                    if (useractive == 0)
                    {
                        // kiểm tra đã quá ngày acctive lại tài khoản chua     
                        int songaylockaccount = Songay_lockAccount();
                        string dateloginfail = row["LAST_LOGIN_FAIL"].ToString();
                        if (!string.IsNullOrEmpty(dateloginfail))
                        {
                            DateTime lastloginfail = DateTime.Parse(dateloginfail);
                            TimeSpan subngay = DateTime.Now.Subtract(lastloginfail);
                            if (subngay.TotalDays >= songaylockaccount)
                            {
                                // nếu đã quá số ngày lock account thì active lại account đó active = 1
                                Update_NoLoginFail(username, 0, loginfail);
                                item.No_login_fail = 0;
                                return item;
                            }
                        }
                        item.No_login_fail = item.No_lock_Acount + 1;
                    }
                    else
                    {
                        item.No_login_fail = 0;
                        // UPDATE NO_LOGIN_FAIL = 0
                        Update_NoLoginFail(username, 0, loginfail);
                    }

                    return item;
                }
                else
                { // trường hợp đăng nhập không thành công
                    kq = getUserbyND(username);
                    item = new User();
                    if (kq!= null)
                    {
                        DataRow row = kq.Rows[0];
                        item.Name = row["TEN_ND"].ToString();
                        string nologinfail_get = string.IsNullOrEmpty(row["NO_LOGIN_FAIL"].ToString()) ? "0" : row["NO_LOGIN_FAIL"].ToString();
                        item.No_login_fail = int.Parse(nologinfail_get);
                        item.No_login_fail = item.No_login_fail + 1;
                        item.No_lock_Acount = loginfail;
                        if (item.No_login_fail > loginfail)
                        {
                            // cập nhật trạng thái active của user = 0
                            Update_NoLoginFail(username, (item.No_login_fail + 1), loginfail);
                        }
                        else
                        {

                            Update_NoLoginFail(username, item.No_login_fail, loginfail);

                        }
                        return item;
                    }
                }
                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<User> GetUserList()
        {
            // Create the list of user and return 
            string query_str = "SELECT * FROM HETHONG_NGUOIDUNG where XOA !=1";

            List<User> list = new List<User>();
            try
            {
                DataTable kq = helper.ExecuteQueryString(query_str);
                if (kq.Rows.Count > 0)
                {
                    foreach (DataRow row in kq.Rows)
                    {
                        User item = new User();
                        item.Id = Int32.Parse(row["ID_ND"].ToString());
                        item.Name = row["TEN_ND"].ToString();
                        item.UserName = row["MA_ND"].ToString();
                        item.Password = row["PASSWORD"].ToString();
                        item.Email = row["MA_ND"].ToString();
                        list.Add(item);
                    }
                }
                return list;

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public string getRoles(String userName)
        {
            string query_str = "HETHONG_QUYEN_GETBY_ND_LOGIN";

            object[] aParams = new object[2];
            try
            {
                aParams[0] = helper.BuildParameter("prmUSERNAME", userName, OracleDbType.Varchar2, ParameterDirection.Input);

                OracleParameter resultParam = new OracleParameter("results", OracleDbType.RefCursor);
                resultParam.Direction = ParameterDirection.Output;

                aParams[1] = resultParam;

                DataTable kq = helper.ExecuteQueryStoreProcedure(query_str, aParams);
                if (kq.Rows.Count > 0)
                    return kq.Rows[0][0].ToString();
                else return "";
            }
            catch (Exception ex)
            {
                string err = ex.Message.Substring(0, ex.Message.IndexOf("\n", 0)).Substring(ex.Message.IndexOf(":") + 2).Trim();
                return "";
            }
        }

        public DataTable get_dsrole(String userName)
        {
            string query_str = "HETHONG_QUYEN_GETBY_ND";

            object[] aParams = new object[2];
            try
            {
                aParams[0] = helper.BuildParameter("prmUSERNAME", userName, OracleDbType.Varchar2, ParameterDirection.Input);

                OracleParameter resultParam = new OracleParameter("results", OracleDbType.RefCursor);
                resultParam.Direction = ParameterDirection.Output;

                aParams[1] = resultParam;

                DataTable kq = helper.ExecuteQueryStoreProcedure(query_str, aParams);
                if (kq.Rows.Count > 0)
                    return kq;
                else
                    return null;
            }
            catch (Exception ex)
            {
                string err = ex.Message.Substring(0, ex.Message.IndexOf("\n", 0)).Substring(ex.Message.IndexOf(":") + 2).Trim();
                return null;
            }
        }
        public string Update_NoLoginFail(string prmUSERNAME, int noLoginfail, int solanlock)
        {
            string query_str = "SP_UPDATE_NO_LOGIN_FAIL";

            object[] aParams = new object[3];

            try
            {
                aParams[0] = helper.BuildParameter("prmUSERNAME", prmUSERNAME, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmNoLoginFail", noLoginfail, OracleDbType.Int16, ParameterDirection.Input);
                aParams[2] = helper.BuildParameter("prmSolanlock", solanlock, OracleDbType.Int16, ParameterDirection.Input);
                DataTable kq = helper.ExecuteQueryStoreProcedure(query_str, aParams);
                return "";
            }
            catch (Exception ex)
            {
                string err = ex.Message.Substring(0, ex.Message.IndexOf("\n", 0)).Substring(ex.Message.IndexOf(":") + 2).Trim();
                return err;
            }
        }
        public DataTable getUserLogin(string prmUSERNAME, string prmPassword)
        {
            string query_str = "HETHONG_NGUOIDUNG_LOGIN";

            object[] aParams = new object[3];

            try
            {
                aParams[0] = helper.BuildParameter("prmNSD", prmUSERNAME, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmPassword", prmPassword, OracleDbType.Varchar2, ParameterDirection.Input);
                OracleParameter resultParam = new OracleParameter("results", OracleDbType.RefCursor);
                resultParam.Direction = ParameterDirection.Output;

                aParams[2] = resultParam;
                DataTable kq = helper.ExecuteQueryStoreProcedure(query_str, aParams);
                if (kq.Rows.Count > 0)
                    return kq;
                else
                    return null;
            }
            catch (Exception ex)
            {
                string err = ex.Message.Substring(0, ex.Message.IndexOf("\n", 0)).Substring(ex.Message.IndexOf(":") + 2).Trim();
                return null;
            }
        }
        public DataTable getUserbyND(String prmUSERNAME)
        {
            string query_str = "HETHONG_NGUOIDUNG_GETBY_MAND";

            object[] aParams = new object[2];
            try
            {
                aParams[0] = helper.BuildParameter("prmUSERNAME", prmUSERNAME, OracleDbType.Varchar2, ParameterDirection.Input);

                OracleParameter resultParam = new OracleParameter("results", OracleDbType.RefCursor);
                resultParam.Direction = ParameterDirection.Output;

                aParams[1] = resultParam;

                DataTable kq = helper.ExecuteQueryStoreProcedure(query_str, aParams);
                if (kq.Rows.Count > 0)
                    return kq;
                else
                    return null;
            }
            catch (Exception ex)
            {
                string err = ex.Message.Substring(0, ex.Message.IndexOf("\n", 0)).Substring(ex.Message.IndexOf(":") + 2).Trim();
                return null;
            }
        }
        public int Songay_lockAccount()
        {
            // số ngày lock account mặc định = 1
            int songaykhoataikhoan = 1;
            string query_str = "SELECT TRANGTHAI FROM CONFIG where MANHOM='SONGAY_KHOA_ACCOUNT'";
            try
            {
                DataTable kq = helper.ExecuteQueryString(query_str);
                if (kq.Rows.Count > 0)
                {
                    DataRow row = kq.Rows[0];
                    songaykhoataikhoan = int.Parse(row["TRANGTHAI"].ToString());
                }
                return songaykhoataikhoan;
            }
            catch (Exception e)
            {
                // lấy giá trị mặc định = 1
                return 1;
            }

        }
        public int Solan_lockAccount()
        {
            // số lần đăng nhập sai lock account mặc định = 3
            int sonlankhoataikhoan = 3;
            string query_str = "SELECT TRANGTHAI FROM CONFIG where MANHOM='SOLAN_LOGIN_LOI'";
            try
            {
                DataTable kq = helper.ExecuteQueryString(query_str);
                if (kq.Rows.Count > 0)
                {
                    DataRow row = kq.Rows[0];
                    sonlankhoataikhoan = int.Parse(row["TRANGTHAI"].ToString());
                }
                return sonlankhoataikhoan;
            }
            catch (Exception e)
            {
                // lấy giá trị mặc định = 3
                return 3;
            }

        }
        public string INSERT_LOGIN(string prmUSERNAME)
        {
            string query_str = "INSERT_LOGIN";

            object[] aParams = new object[1];

            try
            {
                aParams[0] = helper.BuildParameter("prmNSD", prmUSERNAME, OracleDbType.Varchar2, ParameterDirection.Input);
                DataTable kq = helper.ExecuteQueryStoreProcedure(query_str, aParams);
                return "";
            }
            catch (Exception ex)
            {
                string err = ex.Message.Substring(0, ex.Message.IndexOf("\n", 0)).Substring(ex.Message.IndexOf(":") + 2).Trim();
                return err;
            }
        }

        public string HETHONG_NGUOIDUNG_INSERT_FROM_FACEBOOK(string TEN_ND,string SO_DT,string Email)
        {
            string query_str = "HETHONG_NGUOIDUNG_INSERT_FROM_FACEBOOK";

            object[] aParams = new object[3];

            try
            {
                aParams[0] = helper.BuildParameter("prmTEN_ND", TEN_ND, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmSO_DT", SO_DT, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[2] = helper.BuildParameter("prmEmail", Email, OracleDbType.Varchar2, ParameterDirection.Input);
                DataTable kq = helper.ExecuteQueryStoreProcedure(query_str, aParams);
                return "1";
            }
            catch (Exception ex)
            {
                string err = ex.Message.Substring(0, ex.Message.IndexOf("\n", 0)).Substring(ex.Message.IndexOf(":") + 2).Trim();
                return err;
            }
        }
    }
}