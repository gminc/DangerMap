using DangerMap.Models.db;
using DangerMap.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DangerMap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotInstantController : ControllerBase
    {
        /// <summary>
        /// 事件之服務
        /// </summary>
        private readonly CriminalIncidentService criminalIncidentService;

        /// <summary>
        /// 事故之服務
        /// </summary>
        private readonly TrafficAccidentService trafficAccidentService;

        public NotInstantController(CriminalIncidentService service, TrafficAccidentService service2)
        {
            criminalIncidentService = service;
            trafficAccidentService = service2;
        }

        /// <summary>
        /// 取得特定範圍犯罪事件
        /// </summary>
        /// <param name="longitude">使用者座標經度</param>
        /// <param name="latitude">使用者座標緯度</param>
        /// <returns>犯罪事件List</returns>
        // GET: api/<ValuesController>
        [HttpGet("CriminalIncident")]
        public ActionResult<List<CriminalIncident>> GetAreaCriminal([FromQuery] double longitude, double latitude)
        {
            var result = criminalIncidentService.getDataByArea(longitude, latitude);
            if (result.Count != 0)
            {
                return Ok(result);
            }
            return NotFound();
        }

        /// <summary>
        /// 取得特定範圍, 半徑 400 公尺內 交通事故
        /// </summary>
        /// <param name="longitude">使用者座標經度</param>
        /// <param name="latitude">使用者座標緯度</param>
        /// <returns>犯罪事件List</returns>
        // GET: api/<ValuesController>
        [HttpGet("TrafficAccident")]
        public ActionResult<List<CriminalIncident>> GetAreaAccident([FromQuery] double longitude, double latitude)
        {
            var result = trafficAccidentService.getDataByRange(latitude, longitude);
            if (result.Count != 0)
            {
                return Ok(result);
            }
            return NotFound();
        }
    }
}
