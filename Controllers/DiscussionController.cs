using DangerMap.Dtos;
using DangerMap.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DangerMap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscussionController : ControllerBase
    {
        /// <summary>
        /// 帳戶之服務
        /// </summary>
        private readonly EventDiscussionService discussionService;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="service">注入之服務</param>
        public DiscussionController(EventDiscussionService service)
        {
            discussionService = service;
        }

        /// <summary>
        /// 取得指定事件之所有留言
        /// </summary>
        /// <param name="EventId">事件 ID</param>
        /// <returns></returns>
        // GET api/<DiscussionController>/5
        [HttpGet("{EventId}")]
        public ActionResult Get(Guid EventId)
        {
            try
            {
                return Ok(discussionService.GetAllData(EventId));
            }
            catch (Exception)
            {
                return NotFound();
            }            
        }

        /// <summary>
        /// 新增一筆留言【需要有token(登入)】
        /// </summary>
        /// <param name="value">傳入格式:EventDiscussionDto【留言板資訊】</param>
        // POST api/<DiscussionController>
        [Authorize]
        [HttpPost]
        public ActionResult Post([FromBody] EventDiscussionDto value)
        {
            try
            {
                return Ok(discussionService.PutOneData(value));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
