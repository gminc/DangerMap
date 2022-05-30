using DangerMap.Dtos;
using DangerMap.Models.db;
using DangerMap.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DangerMap.Controllers
{   /// <summary>
    ///// 事件與會員之控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class InstantController : ControllerBase
    {
        /// <summary>
        /// 事件之服務
        /// </summary>
        private readonly InstantEventService eventService;

        /// <summary>
        /// 帳戶之服務
        /// </summary>
        private readonly AccountProfileService accountService;

        /// <summary>
        /// 目擊之服務
        /// </summary>
        private readonly EventWitnessService witnessService;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="service">事件服務</param>
        /// <param name="service2">會員服務</param>
        /// <param name="service3">目擊服務</param>
        public InstantController(InstantEventService service, AccountProfileService service2, EventWitnessService service3)
        {
            eventService = service;
            accountService = service2;
            witnessService = service3;
        }

        /// <summary>
        /// 取得全部事件
        /// </summary>
        /// <returns>事件List</returns>
        // GET: api/<InstantController>AllEvent
        [HttpGet("AllEvent")]
        public List<InstantEvent> GetAllEvent()
        {
            return eventService.getAllData();
        }

        /// <summary>
        /// 取得特定範圍事件
        /// </summary>
        /// <param name="longitude">使用者座標經度</param>
        /// <param name="latitude">使用者座標緯度</param>
        /// <returns>事件List</returns>
        // GET api/<InstantController>/Event
        [HttpGet("RangeEvent")]
        public ActionResult<List<InstantEvent>> GetEventByRange([FromQuery] double longitude, double latitude)
        {
            var result = eventService.getDataByDistance(longitude, latitude);
            if (result.Count == 0)
            {
                return NotFound();
            }
            return result;
        }

        /// <summary>
        /// 取得特定事件目擊數
        /// </summary>
        /// <param name="Id">事件 ID</param>
        /// <returns>目擊數</returns>
        // GET api/<InstantController>/Event
        [HttpGet("Witness")]
        public ActionResult<int> GetWitness([FromQuery] Guid Id)
        {
            try
            {
                return Ok(witnessService.GetWitnessAmount(Id));
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

        /// <summary>
        /// 取得特定事件未目擊數
        /// </summary>
        /// <param name="Id">事件 ID</param>
        /// <returns>未目擊數</returns>
        // GET api/<InstantController>/Event
        [HttpGet("NoWitness")]
        public ActionResult<int> GetNoWitness([FromQuery] Guid Id)
        { 
            try
            {
                return Ok(witnessService.GetNoWitnessAmount(Id));
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

        /// <summary>
        /// 上傳事件【需要有token(登入)+已驗證】
        /// </summary>
        /// <param name="value">傳入格式:InstantEventInputDto【事件資訊】</param>
        // POST api/<InstantController>/Event
        [Authorize]
        [HttpPost("Event")]
        public ActionResult PostEvent([FromBody] InstantEventInputDto value)
        {
            if (!IsValidation())
            {
                return Unauthorized();
            }
            try
            {
                eventService.UploadData(value);
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// 是否目擊事件, 如是則 Witness = 1, 否則 Witness = 0【需要有token(登入)+已驗證】
        /// </summary>
        /// <param name="value">傳入格式:WitnessDto【目擊資訊】</param>
        /// <returns>是否成功</returns>
        // POST api/<InstantController>/Event
        [Authorize]
        [HttpPost("Witness")]
        public ActionResult PostWitness([FromBody] WitnessDto value)
        {
            if (!IsValidation())
            {
                return Unauthorized();
            }
            return witnessService.PostWitness(value);
        }

        /// <summary>
        /// 刪除超過兩天的事件
        /// </summary>
        /// <returns></returns>
        // DELETE api/<NewsTicker>
        [HttpDelete]
        public ActionResult Delete()
        {
            return eventService.DeleteExpiredData();
        }

        /// <summary>
        /// 用token判斷用戶是否驗證
        /// </summary>
        /// <returns>有"1"或沒有"0"驗證</returns>
        private bool IsValidation()
        {
            return HttpContext.User.FindFirstValue("Validation") == "True";
        }
    }
}
