using System;
using System.Collections.Generic;
using System.Globalization;

namespace BluSenseWorker.Models
{
    public class Measurement
    {
        public int ID { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string CertificateType { get; set; }
        public string TestItem { get; set; }
        public string LotNumber { get; set; }
        public string PatientID { get; set; }
        public string Note { get; set; }
        public string SampleType { get; set; }
        public string Test1Type { get; set; }
        public string Test1Result { get; set; }
        public decimal Test1Value { get; set; }
        public decimal Test1LowerCutoffValue { get; set; }
        public decimal Test1HigherCutoffValue { get; set; }
        public string Test1QCPassed { get; set; }
        public int Test1QCCode { get; set; }
        public string Test2Type { get; set; }
        public string Test2Result { get; set; }
        public decimal Test2Value { get; set; }
        public decimal Test2LowerCutoffValue { get; set; }
        public decimal Test2HigherCutoffValue { get; set; }
        public string Test2QCPassed { get; set; }
        public int Test2QCCode { get; set; }
        public string Test3Type { get; set; }
        public string Test3Result { get; set; }
        public decimal Test3Value { get; set; }
        public decimal Test3LowerCutoffValue { get; set; }
        public decimal Test3HigherCutoffValue { get; set; }
        public string Test3QCPassed { get; set; }
        public int Test3QCCode { get; set; }
        public string Test4Type { get; set; }
        public string Test4Result { get; set; }
        public decimal Test4Value { get; set; }
        public decimal Test4LowerCutoffValue { get; set; }
        public decimal Test4HigherCutoffValue { get; set; }
        public string Test4QCPassed { get; set; }
        public int Test4QCCode { get; set; }
        public bool InternalQCPassed { get; set; }
        public bool CameraCheckPassed { get; set; }
        public bool ElectronicsCheckPassed { get; set; }
        public bool MechanicsCheckPassed { get; set; }
        public bool OpticsCheckPassed { get; set; }
        public int OpticsCheckValue { get; set; }
        public string ErrorCode { get; set; }
        public string SN { get; set; }
        public string Model { get; set; }
        public int CartridgeSN { get; set; }
        public int TestItemID { get; set; }
        public string SWVersion { get; set; }
        public List<MeasurementTest> MeasurementTests { get; set; }

        public DateTime? GetDateTime()
        {
            var format = "yyyy-MM-dd HH:mm";
            var cultureInfo = new CultureInfo("en-US");
            var dateString = $"{this.Date} {this.Time}";
            return DateTime.ParseExact(dateString, format, cultureInfo);
        }
    }
}
