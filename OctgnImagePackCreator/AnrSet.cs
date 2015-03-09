using System;

namespace OctgnImagePackCreator
{
    public class AnrSet
    {
        public string Name { get; set; }
        public string Code { get; set;}
        public int Number { get; set; }
        public int Cyclenumber { get; set; }
        public DateTime? Available { get; set; }
        public int Known { get; set; }
        public int Total { get; set; }
        public Uri Url { get; set; }
    }
}