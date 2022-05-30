using DangerMap.Dtos;
using DangerMap.Models.db;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DangerMap.Services
{
    public class InstantEventService
    {
        /// <summary>
        /// 事件編號
        /// </summary>

        /// <summary>
        /// 資料庫
        /// </summary>
        private readonly uploadtestv1Context database;

        public InstantEventService(uploadtestv1Context context)
        {
            database = context;
        }

        /// <summary>
        /// 全部事件
        /// </summary>
        /// <returns>事件List</returns>
        public List<InstantEvent> getAllData()
        {
            return database.InstantEvents.ToList();
        }

        /// <summary>
        /// 定點的一定範圍內之事件
        /// </summary>
        /// <param name="longitude">使用者經度</param>
        /// <param name="latitude">使用者緯度</param>
        /// <returns>事件List</returns>
        public List<InstantEvent> getDataByDistance(double longitude, double latitude)
        {
            var result = database.InstantEvents.Where((Func<InstantEvent, bool>)Global.PredicateForDistance(longitude, latitude));
            return result.ToList();
        }

        /// <summary>
        /// 指定會員所上傳過的事件
        /// </summary>
        /// <param name="accountID">指定會員帳號</param>
        /// <returns>事件List</returns>
        public List<InstantEvent> getDataByAccount(string accountID)
        {
            var result = database.InstantEvents.Where(x => x.AccountId == accountID);
            return result.ToList();
        }

        /// <summary>
        /// 新增事件至資料庫
        /// </summary>
        /// <param name="input">事件</param>
        public ActionResult UploadData(InstantEventInputDto input)
        {

            var result = new InstantEvent()
            {
                EventId = Guid.NewGuid(),
                Description = input.Description,
                ShotLink = input.ShotLink,
                LocationDetails = input.LocationDetails,
                AccountId = input.AccountId,
                Title = input.Title,
                UploadTime = input.UploadTime,
                Longitude = input.Longitude,
                Latitude = input.Latitude,
                Type = input.Type
            };
            database.InstantEvents.Add(result);
            try
            {
                database.SaveChanges();
                return new NoContentResult();
            }
            catch
            {
                return new BadRequestResult();
            }
        }

        /// <summary>
        /// 刪除超過兩天的事件
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteExpiredData()
        {
            var delete = from a in database.EventDiscussions
                         where a.CommentTime.Subtract(DateTime.Now).Days > 2
                         select a;

            if (delete != null)
            {
                foreach (var item in delete)
                {
                    database.EventDiscussions.Remove(item);
                }
            }
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
    }
}
