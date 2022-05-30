using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DangerMap.Models.db
{
    public partial class NewsTicker
    {
        public DateTime UploadTime { get; set; }
        public string Title { get; set; }
        [Url]
        public string InfoLink { get; set; }
    }
}
