using DangerMap.Dtos;
using DangerMap.Models.db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DangerMap.Services
{
    public class EventWitnessService
    {
        /// <summary>
        /// 資料庫
        /// </summary>
        private readonly uploadtestv1Context database;
        public EventWitnessService(uploadtestv1Context db)
        {
            database = db;
        }

        /// <summary>
        /// 取得特定事件目擊數
        /// </summary>
        /// <param name="EventID">要查詢事件的ID</param>
        /// <returns>目擊數量</returns>
        public int GetWitnessAmount(Guid EventID)
        {
            var result = database.EventWitnesses.Count(self => self.EventId == EventID && self.Witness == true);
            return result;
        }

        /// <summary>
        /// 取得特定事件未目擊數
        /// </summary>
        /// <param name="EventID">要查詢事件的ID</param>
        /// <returns>未目擊數量</returns>
        public int GetNoWitnessAmount(Guid EventID)
        {
            var result = database.EventWitnesses.Count(self => self.EventId == EventID && self.Witness == false);
            return result;
        }

        /// <summary>
        /// 按下目擊或沒目擊
        /// </summary>
        /// <param name="input">對該事件目擊或沒目擊</param>
        /// <returns>成功或失敗</returns>
        public ActionResult PostWitness(WitnessDto input)
        {
            var dataInDb = database.EventWitnesses.Where(x => x.EventId == input.EventId && x.AccountId == input.AccountId).FirstOrDefault();

            //沒按過，新增
            if (dataInDb == null)
            {
                var result = new EventWitness()
                {
                    EventId = input.EventId,
                    AccountId = input.AccountId,
                    Witness = input.Witness
                };
                database.EventWitnesses.Add(result);
            }
            else
            {
                //按過該事件之目前狀態
                var isWitness = dataInDb.Witness;
                //重複按，刪除(收回)
                if (isWitness == input.Witness)
                {
                    database.EventWitnesses.Remove(dataInDb);
                }
                else //按另一個 (更新)
                {
                    dataInDb.Witness = input.Witness;
                    database.EventWitnesses.Update(dataInDb);
                }
            }
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
    }
}
