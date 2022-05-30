using System;
using System.Collections.Generic;

#nullable disable

namespace DangerMap.Models.db
{
    public partial class InstantEventNewId
    {
        public Guid EventId { get; set; }
        public string AccountId { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string LocationDetails { get; set; }
        public DateTime WitnessTime { get; set; }
        public string ShotLink { get; set; }
    }
}
