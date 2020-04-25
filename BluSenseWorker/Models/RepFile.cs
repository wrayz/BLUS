using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace BluSenseWorker.Models
{
    public class RepFile
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string SN { get; set; }
        public string TestItem { get; set; }
        public string Result { get; set; }
        public string SWVersion { get; set; }
        public string REP { get; set; }
        public string LOG { get; set; }
        public string IMG { get; set; }
        public string Use { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public DateTime? LogDateTime
        {
            get => GetDateTime();
        }
        public DateTime? GetDateTime()
        {
            var format = "yyyy-MM-dd HH:mm:ss";
            var cultureInfo = new CultureInfo("en-US");
            var dateString = $"{this.Date} {this.Time}";
            return DateTime.ParseExact(dateString, format, cultureInfo);
        }
    }

    public class RepFileComparer : IEqualityComparer<RepFile>
    {
        public bool Equals([AllowNull] RepFile x, [AllowNull] RepFile y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.SN == y.SN && x.Date == y.Date && x.Time == y.Time;
        }

        public int GetHashCode([DisallowNull] RepFile obj)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(obj, null)) return 0;

            int hasSn = obj.SN == null ? 0 : obj.SN.GetHashCode();

            int hasDate = obj.Date == null ? 0 : obj.Date.GetHashCode();

            int hasTime = obj.Time == null ? 0 : obj.Time.GetHashCode();

            //Calculate the hash code for the product.
            return hasSn ^ hasDate ^ hasTime;
        }
    }
}