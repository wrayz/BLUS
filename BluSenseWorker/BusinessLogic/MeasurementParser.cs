using System;
using System.Collections.Generic;
using System.IO;
using BluSenseWorker.Models;
using ServiceStack.Text;

namespace BluSenseWorker.BusinessLogic
{
    public class MeasurementParser
    {
        private string _path;

        public List<Measurement> Measurements { get; private set; }

        public MeasurementParser(string path)
        {
            _path = path;
        }

        public void Execute()
        {
            var data = GetDictionaries();
            SetMeasurements(data);
        }

        private List<Dictionary<string, string>> GetDictionaries()
        {
            var list = new List<Dictionary<string, string>>();

            var data = ParsingCsvData();

            for (int r = 0; r < data.rows.Count; r++)
            {
                var dic = new Dictionary<string, string>();
                foreach (var item in data.properties) dic.Add(item, "");

                var cells = data.rows[r];
                for (int c = 0; c < cells.Length; c++)
                {
                    dic[data.properties[c]] = string.IsNullOrWhiteSpace(cells[c]) ? "" : cells[c];
                }

                list.Add(dic);
            }

            return list;
        }

        private (string[] properties, List<string[]> rows) ParsingCsvData()
        {
            var csv = File.ReadAllText(_path);

            string[] propNames = null;
            List<string[]> rows = new List<string[]>();

            foreach (var line in CsvReader.ParseLines(csv))
            {
                string[] strArray = CsvReader.ParseFields(line).ToArray();
                if (propNames == null)
                    propNames = strArray;
                else
                    rows.Add(strArray);
            }

            Console.WriteLine($"PropNames={string.Join(",", propNames)}");
            return (propNames, rows);
        }

        private void SetMeasurements(List<Dictionary<string, string>> data)
        {
            Measurements = new List<Measurement>();
            data.ForEach(dic => Measurements.Add(new Measurement
            {
                ID = dic.Get<int>("ID"),
                Date = dic.Get("Date"),
                Time = dic.Get("Time"),
                PatientID = dic.Get("Patinet ID"),
                SampleType = dic.Get("Sample"),
                TestItem = dic.Get("Test Item"),
                Note = dic.Get("Note"),
                CameraCheckPassed = dic.Get<bool>("Camera Check Passed"),
                ElectronicsCheckPassed = dic.Get<bool>("Electronics Check Passed"),
                MechanicsCheckPassed = dic.Get<bool>("Mechanics Check Passed"),
                OpticsCheckPassed = dic.Get<bool>("Optics Check Passed"),
                OpticsCheckValue = dic.Get<int>("Optics Check Value"),
                ErrorCode = dic.Get("Error Code,Model"),
                Model = dic.Get("Model"),
                SN = dic.Get("SN"),
                LotNumber = dic.Get("Lot Number"),
                CartridgeSN = dic.Get<int>("Cartridge SN"),
                TestItemID = dic.Get<int>("Test Item ID"),
                SWVersion = dic.Get("SW Version"),
                Test1Type = dic.Get("Test 1 Type"),
                Test1Result = dic.Get("Test 1 Result"),
                Test1Value = dic.Get<decimal>("Test 1 Value"),
                Test1LowerCutoffValue = dic.Get<decimal>("Test 1 Lower Cut-off Value"),
                Test1HigherCutoffValue = dic.Get<decimal>("Test 1 Higher Cut-off Value"),
                Test1QCPassed = dic.Get("Test 1 QC Passed"),
                Test1QCCode = dic.Get<int>("Test 1 QC Code"),
                Test2Type = dic.Get("Test 2 Type"),
                Test2Result = dic.Get("Test 2 Result"),
                Test2Value = dic.Get<decimal>("Test 2 Value"),
                Test2LowerCutoffValue = dic.Get<decimal>("Test 2 Lower Cut-off Value"),
                Test2HigherCutoffValue = dic.Get<decimal>("Test 2 Higher Cut-off Value"),
                Test2QCPassed = dic.Get("Test 2 QC Passed"),
                Test2QCCode = dic.Get<int>("Test 2 QC Code"),
                Test3Type = dic.Get("Test 3 Type"),
                Test3Result = dic.Get("Test 3 Result"),
                Test3Value = dic.Get<decimal>("Test 3 Value"),
                Test3LowerCutoffValue = dic.Get<decimal>("Test 3 Lower Cut-off Value"),
                Test3HigherCutoffValue = dic.Get<decimal>("Test 3 Higher Cut-off Value"),
                Test3QCPassed = dic.Get("Test 3 QC Passed"),
                Test3QCCode = dic.Get<int>("Test 3 QC Code"),
                Test4Type = dic.Get("Test 4 Type"),
                Test4Result = dic.Get("Test 4 Result"),
                Test4Value = dic.Get<decimal>("Test 4 Value"),
                Test4LowerCutoffValue = dic.Get<decimal>("Test 4 Lower Cut-off Value"),
                Test4HigherCutoffValue = dic.Get<decimal>("Test 4 Higher Cut-off Value"),
                Test4QCPassed = dic.Get("Test 4 QC Passed"),
                Test4QCCode = dic.Get<int>("Test 4 QC Code"),
                MeasurementTests = GetMeasurementTests()
            }));
        }

        private List<MeasurementTest> GetMeasurementTests()
        {
            var list = new List<MeasurementTest>();

            foreach (var item in Measurements)
            {
                list.Add(new MeasurementTest
                {
                    No = 1,
                    Model = item.Model,
                    SN = item.SN,
                    PatientID = item.PatientID,
                    TestType = item.Test1Type,
                    TestQCCode = item.Test1QCCode,
                    TestValue = item.Test1Value,
                    TestResult = item.Test1Result,
                    TestHigherCutoffValue = item.Test1HigherCutoffValue,
                    TestLowerCutoffValue = item.Test1LowerCutoffValue,
                    TestQCPassed = item.Test1QCPassed,
                    TestDate = item.GetDateTime()
                });
                list.Add(new MeasurementTest
                {
                    No = 2,
                    PatientID = item.PatientID,
                    Model = item.Model,
                    SN = item.SN,
                    TestType = item.Test2Type,
                    TestQCCode = item.Test2QCCode,
                    TestValue = item.Test2Value,
                    TestResult = item.Test2Result,
                    TestHigherCutoffValue = item.Test2HigherCutoffValue,
                    TestLowerCutoffValue = item.Test2LowerCutoffValue,
                    TestQCPassed = item.Test2QCPassed,
                    TestDate = item.GetDateTime()
                });
                list.Add(new MeasurementTest
                {
                    No = 3,
                    PatientID = item.PatientID,
                    Model = item.Model,
                    SN = item.SN,
                    TestType = item.Test3Type,
                    TestQCCode = item.Test3QCCode,
                    TestValue = item.Test3Value,
                    TestResult = item.Test3Result,
                    TestHigherCutoffValue = item.Test3HigherCutoffValue,
                    TestLowerCutoffValue = item.Test3LowerCutoffValue,
                    TestQCPassed = item.Test3QCPassed,
                    TestDate = item.GetDateTime()
                });
                list.Add(new MeasurementTest
                {
                    No = 4,
                    PatientID = item.PatientID,
                    Model = item.Model,
                    SN = item.SN,
                    TestType = item.Test4Type,
                    TestQCCode = item.Test4QCCode,
                    TestValue = item.Test4Value,
                    TestResult = item.Test4Result,
                    TestHigherCutoffValue = item.Test4HigherCutoffValue,
                    TestLowerCutoffValue = item.Test4LowerCutoffValue,
                    TestQCPassed = item.Test4QCPassed,
                    TestDate = item.GetDateTime()
                });
            }

            return list;
        }

    }
}
