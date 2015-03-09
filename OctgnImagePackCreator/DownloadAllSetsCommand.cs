using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace OctgnImagePackCreator
{
    internal class DownloadAllSetsCommand : Command
    {
        public override void Execute()
        {
            var sets =
                JsonConvert.DeserializeObject<IEnumerable<AnrSet>>(
                    File.ReadAllText(Constants.SetsFile));

            try
            {
                new DownloadSetCommand(sets.Where(p => p.Available != null).Select(p => p.Code)).Execute();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }
    }
}