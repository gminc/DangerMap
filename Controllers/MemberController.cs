using DangerMap.Dtos;
using DangerMap.JWT;
using DangerMap.Models.db;
using DangerMap.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DangerMap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        /// <summary>
        /// 帳戶之服務
        /// </summary>
        private readonly AccountProfileService accountService;
        private readonly JwtAuthenticationManager jwtService;
        /// <summary>
        /// 取得回傳值之服務
        /// </summary>
        private readonly IHttpContextAccessor httpContextAccessor;
        /// <summary>
        /// 資料庫
        /// </summary>
        private readonly uploadtestv1Context database;
        /// <summary>
        /// 建構子
        /// </summary>
        public MemberController(AccountProfileService service, JwtAuthenticationManager jwtService, uploadtestv1Context db, IHttpContextAccessor httpContextAccessor)
        {
            accountService = service;
            this.jwtService = jwtService;
            database = db;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="idAndPassword">帳密:AccountLoginDto</param>
        /// <returns>成功給token，失敗回傳帳密錯誤</returns>
        [HttpPost("Login")]
        public ActionResult<string> Login([FromBody] AccountLoginDto idAndPassword)
        {
            var token = jwtService.Authenticate(idAndPassword).JwtToken;
            if (token == null)
            {
                return BadRequest("帳號或密碼有誤");
            }
            return token;
        }

        /// <summary>
        /// 上傳會員資料(註冊)
        /// </summary>
        /// <param name="value">傳入格式:AccountProfileInputDto【註冊所需資料】</param>
        // POST api/Member/Register
        [HttpPost("Register")]
        public ActionResult PostMember([FromBody] AccountProfileInputDto value)
        {
            return accountService.UploadData(value).Result;
        }

        /// <summary>
        /// 換發token
        /// </summary>
        /// <param name="token">AccountID、舊的Token</param>
        /// <returns>400:token給錯 401:請重新登入 200: 回傳新的token</returns>
        [HttpPost("RefreshToken")]
        public ActionResult<string> Refresh([FromBody] TokenDto token)
        {
            var accountID = token.AccountID;
            var oldRefreshToken = database.AccountProfiles.Where(x => x.AccountId == accountID).Select(x => x.RefreshToken).FirstOrDefault();
            if (oldRefreshToken == null)
            {
                return BadRequest();
            }
            var result = jwtService.Refresh(new AuthenticationResponse { JwtToken = token.Token, RefreshToken = oldRefreshToken });
            if (result == null)
            {
                return Unauthorized();
            }
            return result.JwtToken;
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns>成功or失敗</returns>
        [HttpGet("Logout")]
        public ActionResult Logout()
        {
            var accountID = httpContextAccessor.HttpContext.User.FindFirstValue("ID");
            return jwtService.CleanRefreshToken(accountID);
        }

        /// <summary>
        /// 將用戶變成已驗證
        /// </summary>
        /// <param name="id">用戶ID</param>
        /// <returns>是否成功</returns>
        // GET api/Login/Verify/id
        [HttpGet("Verify/{id}")]
        public ActionResult Verify([FromRoute] string id)
        {
            return accountService.Verify(id);
        }

        /// <summary>
        /// 寄出驗證信
        /// </summary>
        /// <param name="id">用戶ID</param>
        /// <returns>是否成功</returns>
        [HttpGet("Mail/{id}")]
        public ActionResult SendValidationMail([FromRoute] string id)
        {
            return accountService.SendMailByGmail(id);
        }
    }
}
