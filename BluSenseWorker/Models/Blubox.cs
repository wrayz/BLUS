using System;

namespace BluSenseWorker.Models
{
    public class Blubox
    {
        public int? Id { get; set; }
        public string Model { get; set; }
        public string SN { get; set; }
        public string CountryCode { get; set; }
        public string SwVersion { get; set; }
        public string UseStatus { get; set; }
        public string UseType { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}