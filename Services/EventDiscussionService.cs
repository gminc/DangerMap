using DangerMap.Dtos;
using DangerMap.Models.db;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DangerMap.Services
{
    public class EventDiscussionService
    {
        /// <summary>
        /// 資料庫
        /// </summary>
        private readonly uploadtestv1Context database;

        public EventDiscussionService(uploadtestv1Context db)
        {
            database = db;
        }

        /// <summary>
        /// 取得指定事件之所有留言
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <returns>留言List</returns>
        public List<EventDiscussion> GetAllData(Guid eventID)
        {
            return database.EventDiscussions.Where(x => x.EventId == eventID).ToList();
        }

        /// <summary>
        /// 新增一筆留言
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <returns>留言List</returns>
        public async Task<ActionResult> PutOneData(EventDiscussionDto comment)
        {
            var result = new EventDiscussion()
            {
                EventId = comment.EventId,
                AccountId = comment.AccountId,
                CommentTime = DateTime.Now,
                Comment = comment.Comment
            };

            await database.EventDiscussions.AddAsync(result);

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
