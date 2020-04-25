using System;

namespace BluSenseWorker.Models
{
    public class MeasurementTest
    {
        public string PatientID { get; set; }
        public string SN { get; internal set; }
        public string Model { get; internal set; }
        public int No { get; set; }
        public string TestType { get; set; }
        public string TestResult { get; set; }
        public decimal TestValue { get; set; }
        public decimal TestLowerCutoffValue { get; set; }
        public decimal TestHigherCutoffValue { get; set; }
        public string TestQCPassed { get; set; }
        public int TestQCCode { get; set; }
        public DateTime? TestDate { get; set; }
    }
}
