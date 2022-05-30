using DangerMap.Dtos;
using DangerMap.Models;
using DangerMap.Models.db;
using DangerMap.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DangerMap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsTickerController : ControllerBase
    {
        private readonly NewsTickerService newsTickerService;

        public NewsTickerController(NewsTickerService service)
        {
            newsTickerService = service;
        }

        /// <summary>
        /// 抓取全部跑馬燈內容
        /// </summary>
        /// <returns></returns>
        // GET: api/<NewsTicker>
        [HttpGet]
        public ActionResult<List<NewsTicker>> Get()
        {
            return Ok(newsTickerService.GetAllNews());
        }

        /// <summary>
        /// 新增一筆跑馬燈資料
        /// </summary>
        /// <param name="value">傳入格式:跑馬燈資訊</param>
        /// <returns></returns>
        // POST api/<NewsTicker>
        [HttpPost]
        public ActionResult Post([FromBody] NewsTickerDto value)
        {
            return Ok(newsTickerService.PutNews(value));
        }

        /// <summary>
        /// 刪除資料庫內所有跑馬燈資料
        /// </summary>
        /// <returns></returns>
        // DELETE api/<NewsTicker>/
        [HttpDelete]
        public ActionResult Delete()
        {
            newsTickerService.DeleteAllNews();
            return Ok();
        }
    }
}
