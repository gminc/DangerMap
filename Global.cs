using DangerMap.Models;
using DangerMap.Models.db;
using System;
using System.Text.RegularExpressions;

namespace DangerMap
{
    public class Global
    {
        /// <summary>
        /// 能使用於 AccountID的 字元, 不能超過 10 個字元, 不能出現以下提到之外的字元
        /// </summary>
        public static readonly Regex LegalIdAndPassword = new Regex(@"^[a-zA-Z0-9_]{0,10}$");

        /// <summary>
        /// 搜尋範圍(半徑)
        /// </summary>
        public static readonly int SEARCH_RANGE = 400;

        /// <summary>
        /// from Google Map 腳本
        /// <para>出處：http://windperson.wordpress.com/2011/11/01/由兩點經緯度數值計算實際距離的方法/ </para>
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lng1"></param>
        /// <param name="lat2"></param>
        /// <param name="lng2"></param>
        /// <returns>回傳單位 公尺</returns>
        public static double GetDistance_Google(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }

        /// <summary>
        /// 自訂表達式
        /// </summary>
        /// <param name="lng">使用者座標</param>
        /// <param name="lat">使用者座標</param>
        /// <returns>是否在範圍內之函式</returns>
        public static Func<InstantEvent, bool> PredicateForDistance(double lng, double lat)
        {
            return x => (GetDistance_Google(x.Latitude, x.Longitude, lat, lng) <= Global.SEARCH_RANGE);
        }

        /// <summary>
        /// 地球半徑
        /// </summary>
        private const double EARTH_RADIUS = 6378.137;
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }
    }
}
