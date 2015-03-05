using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace OctgnImagePackCreator
{
    internal class ListSetsCommand : Command
    {
        public override void Execute()
        {
            var setDataString = File.ReadAllText(Path.Combine(Constants.SetsFile));
            var setData = JsonConvert.DeserializeObject<IEnumerable<AnrSet>>(setDataString);
            foreach (var set in setData.Where(p=>p.Available != null))
            {
                Console.WriteLine("{0} - {1}", set.Code, set.Name);
            }
        }
    }
}