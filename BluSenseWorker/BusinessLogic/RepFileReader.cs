using System.Collections.Generic;
using System.IO;
using BluSenseWorker.Interfaces;
using ServiceStack.Text;

namespace BluSenseWorker.BusinessLogic
{
    public class RepFileReader : IReader
    {
        private string _path;

        public RepFileReader(string path)
        {
            this._path = path;
        }

        public List<Dictionary<string, string>> GetDictionaries()
        {
            var list = new List<Dictionary<string, string>>();

            var data = ReadCsvFile();

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

        private (string[] properties, List<string[]> rows) ReadCsvFile()
        {
            var csv = File.ReadAllText(_path);
            string[] propName = null;

            var rows = new List<string[]>();

            foreach (var line in CsvReader.ParseLines(csv))
            {
                var strArray = CsvReader.ParseFields(line).ToArray();

                if (propName == null)
                    propName = strArray;
                else
                    rows.Add(strArray);
            }

            return (propName, rows);
        }
    }
}