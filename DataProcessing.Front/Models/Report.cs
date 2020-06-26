using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProcessing.Front.Models
{
    public class T
    {
        public double v { get; set; }
    }

    public class City
    {
        public IList<double> geo { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Time
    {
        public string s { get; set; }
        public string tz { get; set; }
        public int v { get; set; }
    }

    public class Value
    {
        public City city { get; set; }
        public string dominentpol { get; set; }
        public Dictionary<string,T> iaqi { get; set; }
        public Time time { get; set; }
        public int idx { get; set; }
    }

    public class Report
    {
        public IList<Value> values { get; set; }
        public string id { get; set; }
        public int _ts { get; set; }
    }
}
