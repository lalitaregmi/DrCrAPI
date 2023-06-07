﻿using Dapper;
using Helper.Dapper;
using Models.Model;
using Service.Interface;

namespace Service.Service
{
    public class CreateAccountService:ICreateAccount
    {

        public String convertToNepali(String engDate)
        {

            return engDate;
        }
        public async Task<dynamic> CreateAcc(Account a)
        {
            var res = new ResValues();
            
            {
                var sql = "sp_tbl_com";
                var parameters = new DynamicParameters();
                parameters.Add("@comid", a.ComID);
                parameters.Add("@userid", a.UserID);
                parameters.Add("@flag", a.Flag);
                
                parameters.Add("@particulars", a.Particulars);
                parameters.Add("@dramt", a.DrAmt);
                parameters.Add("@cramt", a.CrAmt);
               
                parameters.Add("@date", a.Date);
                parameters.Add("@nepdate", convertToNepali(a.Date));

            

                var data = await DbHelper.RunProc<dynamic>(sql, parameters); // it run the stored procedure with the help of DbHelper and pass result to the data.
                if (data.Count() != 0 && data.FirstOrDefault().Message == null) //yei condition true since data contain (sql and parameter) 
                {
                    res.Values = data.ToList();
                    res.StatusCode = 200;
                    res.Message = "Success";

                }
                else if (data.Count() == 1 && data.FirstOrDefault().Message != null)
                {
                    res.Values = null;
                    res.StatusCode = data.FirstOrDefault().Message.StatusCode;
                    res.Message = data.FirstOrDefault().Message;

                }
                else
                {
                    res.Values = null;
                    res.StatusCode = 400;
                    res.Message = "no data";

                }
            }
            return res;
        }

    }
}
