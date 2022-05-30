using DangerMap.ValidationAttributes;
using System;

namespace DangerMap.Dtos
{   
    /// <summary>
    /// 目擊資訊
    /// </summary>
    public class WitnessDto
    {   
        /// <summary>
        /// 事件ID
        /// </summary>
        public Guid EventId { get; set; }
        /// <summary>
        /// 按是否目擊者之用戶ID
        /// </summary>
        [AccountID]
        public string AccountId { get; set; }
        /// <summary>
        /// 是否目擊 1=true,0=false
        /// </summary>
        public bool Witness { get; set; }
    }
}
