using System;
using System.IO;
using System.Net;

namespace OctgnImagePackCreator
{
    internal class UpdateCommand : Command
    {
        public override void Execute()
        {
            var web = new WebClient();
            Console.WriteLine("Updating A:NR set info...");
            var setDataString = web.DownloadString(new Uri("http://netrunnerdb.com/api/sets"));
            File.WriteAllText(Constants.SetsFile, setDataString);

            Console.WriteLine("Updating A:NR card info...");
            var cardDataString = web.DownloadString(new Uri("http://netrunnerdb.com/api/cards"));
            File.WriteAllText(Constants.CardsFile, cardDataString);
        }
    }
}