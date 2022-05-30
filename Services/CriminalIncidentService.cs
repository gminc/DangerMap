using DangerMap.Models.db;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DangerMap.Services
{
    public class CriminalIncidentService
    {
        /// <summary>
        /// 資料庫
        /// </summary>
        private readonly uploadtestv1Context database;
        public CriminalIncidentService(uploadtestv1Context db)
        {
            database = db;
        }

        /// <summary>
        /// 指定行政區之犯罪資料
        /// </summary>
        /// <param name="longitude">使用者經度</param>
        /// <param name="latitude">使用者緯度</param>
        /// <returns>犯罪資料List</returns>
        public List<CriminalIncident> getDataByArea(double longitude, double latitude)
        {
            string area = ProcessRequest(latitude, longitude);
            var result = database.CriminalIncidents.Where(x => x.Location.Contains(area));
            return result.ToList();
        }

        /// <summary>
        /// 座標轉行政區
        /// </summary>
        /// <param name="lat">使用者緯度</param>
        /// <param name="lng">使用者經度</param>
        /// <returns>行政區</returns>
        public static string ProcessRequest(double lat, double lng)
        {

            string latlng = $"{lat},{lng}";

            if (!string.IsNullOrEmpty(latlng))
            {
                string url =
                "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + latlng + "&sensor=true&key=AIzaSyDuuWKb6Lw97hwx4oME2rG9413eJ6mZD7A";
                string json = String.Empty;

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                //指定語言，否則Google預設回傳英文
                request.Headers.Add("Accept-Language", "zh-tw");

                using (var response = request.GetResponse())
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {

                    json = sr.ReadToEnd();

                }
                JObject obj = (JObject)JsonConvert.DeserializeObject(json);
                string status = obj["status"].ToString();
                if (status == "OK")
                {
                    JArray ary = (JArray)obj["results"];
                    ary = (JArray)ary[0]["address_components"];


                    var q = from o in ary
                            where o["types"].Count() >= 2 && o["types"][1].ToString() == "political"
                            select o;
                    var a = q.SkipLast(1);
                    string district = a.LastOrDefault()["long_name"].ToString() + a.FirstOrDefault()["long_name"].ToString();

                    return district;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
