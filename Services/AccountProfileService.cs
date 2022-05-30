using DangerMap.Dtos;
using System.Net.Mail;
using DangerMap.Models.db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DangerMap.Services
{
    public class AccountProfileService
    {
        /// <summary>
        /// 資料庫
        /// </summary>
        private readonly uploadtestv1Context database;
     
        public AccountProfileService(uploadtestv1Context db)
        {
            database = db;  
        }

        /// <summary>
        /// 上傳會員資料(註冊)
        /// </summary>
        public async Task<ActionResult> UploadData(AccountProfileInputDto value)
        {
            var name = value.AccountName;
            //沒有輸入名字的話，預設暱稱等於帳號ID
            if (value.AccountName == string.Empty)
            {
                name = value.AccountId;
            }
            var result = new AccountProfile()
            {
                PropicLink = value.PropicLink,
                Password = ToMD5(value.Password),
                AccountEmail = value.AccountEmail,
                AccountId = value.AccountId,
                AccountName = name,
                Validation = false
            };
            await database.AccountProfiles.AddAsync(result);
            try
            {
                database.SaveChanges();
                return new NoContentResult();
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }


        /// <summary>
        /// 取得一筆特定會員資料
        /// </summary>
        /// <param name="AccountID">要查詢會員的帳號</param>
        /// <returns>回傳該會員資料或null</returns>
        public AccountProfile GetAccountProfile(string AccountID)
        {
            var result = database.AccountProfiles.Where(self => self.AccountId == AccountID).ToList().FirstOrDefault();
            return result;
        }

        /// <summary>
        /// 驗證用戶
        /// </summary>
        /// <param name="accountID">要驗證的用戶ID</param>
        /// <returns>是否成功</returns>
        public ActionResult Verify(string accountID)
        {
            var result = database.AccountProfiles.Where(x => x.AccountId == accountID).SingleOrDefault();
            if (result == null)
            {
                return new NotFoundResult();
            }
            result.Validation = true;
            database.AccountProfiles.Update(result);
            try
            {
                database.SaveChanges();
                return new OkObjectResult("你已完成驗證");
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        /// <summary>
        /// 寄出驗證信
        /// </summary>
        /// <param name="accountID">要驗證的用戶ID</param>
        /// <returns>是否成功</returns>
        public ActionResult SendMailByGmail(string accountID)
        {
            var mail = database.AccountProfiles.Where(x => x.AccountId == accountID).Select(x => x.AccountEmail).SingleOrDefault();
            if (mail == null)
            {
                return new NotFoundResult();
            }

            using (MailMessage msg = new MailMessage())
            {
                //收件者，以逗號分隔不同收件者 ex "test@gmail.com,test2@gmail.com"
                msg.To.Add(mail);
                msg.From = new MailAddress("email@gmail.com", "測試郵件", System.Text.Encoding.UTF8);
                //郵件標題 
                msg.Subject = "危機地圖會員認證信";
                //郵件標題編碼  
                msg.SubjectEncoding = System.Text.Encoding.UTF8;
                //郵件內容
                msg.Body = $"請點擊https://localhost:44312/api/Login/Verify/{accountID}，來認證您的身分。\n謝謝。";
                msg.IsBodyHtml = true;
                msg.BodyEncoding = System.Text.Encoding.UTF8;//郵件內容編碼 
                msg.Priority = MailPriority.Normal;//郵件優先級 

                //建立 SmtpClient 物件 並設定 Gmail的smtp主機及Port 
                #region 其它 Host
                /*
                 *  outlook.com smtp.live.com port:25
                 *  yahoo smtp.mail.yahoo.com.tw port:465
                */
                #endregion
                using (SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    //設定你的帳號密碼
                    MySmtp.Credentials = new System.Net.NetworkCredential("DangerMap2022@gmail.com", "ourdangermap2022");
                    //Gmial 的 smtp 使用 SSL
                    MySmtp.EnableSsl = true;
                    MySmtp.Send(msg);
                }
            }
            return new NoContentResult();
        }

        /// <summary>
        /// 簡單字串加密(MD5)
        /// </summary>
        /// <param name="strs">密碼</param>
        /// <returns>加密後之字串</returns>
        public static string ToMD5(string strs)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.Default.GetBytes(strs);//將要加密的字串轉換為位元組陣列
            byte[] encryptdata = md5.ComputeHash(bytes);//將字串加密後也轉換為字元陣列
            return Convert.ToBase64String(encryptdata);//將加密後的位元組陣列轉換為加密字串
        }
    }
}
