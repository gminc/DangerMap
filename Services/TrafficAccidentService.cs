using DangerMap.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DangerMap.Services
{
    public class TrafficAccidentService
    {
        /// <summary>
        /// 資料庫
        /// </summary>
        private readonly uploadtestv1Context database;
        public TrafficAccidentService(uploadtestv1Context db)
        {
            database = db;
        }

        /// <summary>
        /// 指定座標範圍內之交通事故資料
        /// </summary>
        /// <param name="longitude">使用者經度</param>
        /// <param name="latitude">使用者緯度</param>
        /// <returns>交通事故List</returns>
        public List<TrafficAccident> getDataByRange(double longitude, double latitude)
        {
            var result = database.TrafficAccidents.Where(PredicateForDistance(longitude, latitude));
            return result.ToList();
        }

        /// <summary>
        /// 自訂表達式
        /// </summary>
        /// <param name="lng">使用者座標</param>
        /// <param name="lat">使用者座標</param>
        /// <returns>是否在範圍內之函式</returns>
        public static Func<TrafficAccident, bool> PredicateForDistance(double lng, double lat)
        {
            return x => (Global.GetDistance_Google(x.Latitude, x.Longitude, lat, lng) <= Global.SEARCH_RANGE);
        }
    }
}
