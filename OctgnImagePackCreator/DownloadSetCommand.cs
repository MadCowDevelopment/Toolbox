using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace OctgnImagePackCreator
{
    internal class DownloadSetCommand : Command
    {
        private readonly List<string> _setCodes;

        public DownloadSetCommand(string setCode)
        {
            _setCodes = new List<string> { setCode };
        }

        public DownloadSetCommand(IEnumerable<string> setCodes)
        {
            _setCodes = new List<string>(setCodes);
        }

        public override void Execute()
        {
            try
            {
                foreach (var setCode in _setCodes)
                {
                    try
                    {
                        CreateSet(setCode);
                    }
                    catch (InvalidOperationException)
                    {
                        Console.WriteLine("An error occurred while creating set '{0}': {1}", setCode);
                    }
                }

                var imagePackName = _setCodes.Count == 1 ? FindSet(_setCodes[0]).Name : "all";
                CreateImagePack(imagePackName);
            }
            finally
            {
                if (Directory.Exists(Constants.TempPath))
                    Directory.Delete(Constants.TempPath, true);
            }
        }

        private void CreateSet(string setCode)
        {
            Console.WriteLine("Creating set '{0}'...", setCode);
            var set = FindSet(setCode);
            var xmlInfo = ParseSetXml(set.Name);
            var cardsPath = CreateTempFolders(xmlInfo);
            DownloadImages(xmlInfo.Cards, cardsPath);
        }

        private static string CreateTempFolders(XmlSetInfo xmlInfo)
        {
            Directory.CreateDirectory(Path.Combine(Constants.TempPath, xmlInfo.GameId));
            var cardsPath = Path.Combine(Constants.TempPath, xmlInfo.GameId, "Sets", xmlInfo.SetId, "Cards");
            Directory.CreateDirectory(cardsPath);
            return cardsPath;
        }

        private XmlSetInfo ParseSetXml(string setName)
        {
            var setXmlPath = GetSetXmlPath(setName.ToLower());
            var setXml = XDocument.Load(setXmlPath);

            var gameId = setXml.Element("set").Attribute("gameId").Value;
            var setId = setXml.Element("set").Attribute("id").Value;
            var cards = setXml.Descendants("card");
            return new XmlSetInfo(gameId, setId, cards);
        }

        private AnrSet FindSet(string setCode)
        {
            var sets =
                    JsonConvert.DeserializeObject<IEnumerable<AnrSet>>(
                        File.ReadAllText(Constants.SetsFile));
            var set = sets.FirstOrDefault(s => s.Code == setCode);
            if (set == null)
            {
                throw new InvalidOperationException(string.Format("Could not find set with code '{0}'.", setCode));
            }

            return set;
        }

        private class XmlSetInfo
        {
            public XmlSetInfo(string gameId, string setId, IEnumerable<XElement> cards)
            {
                GameId = gameId;
                SetId = setId;
                Cards = cards;
            }

            public string GameId { get; private set; }
            public string SetId { get; private set; }
            public IEnumerable<XElement> Cards { get; private set; }
        }

        private static void CreateImagePack(string name)
        {
            var zipFileName = name + ".o8c";
            File.Delete(zipFileName);
            ZipFile.CreateFromDirectory(Constants.TempPath, zipFileName);
        }

        private void DownloadImages(IEnumerable<XElement> cardsXml, string cardsPath)
        {
            var cards =
                    JsonConvert.DeserializeObject<IEnumerable<AnrCard>>(File.ReadAllText(Constants.CardsFile)).ToList();
            var webClient = new WebClient();
            foreach (var cardXml in cardsXml)
            {
                var cardId = cardXml.Attribute("id").Value;
                var cardName = cardXml.Attribute("name").Value;
                var card = cards.FirstOrDefault(p => p.Title.ToLower() == cardName.ToLower());
                if (card == null)
                {
                    card = cards.FirstOrDefault(p => p.Title.ToLower().Contains(cardName.ToLower()));
                    if (card == null)
                    {
                        card = FindBestMatch(cardName, cards);
                    }

                    if (card == null)
                    {
                        Console.WriteLine("Couldn't find card: {0}", cardName);
                        continue;
                    }

                    Console.WriteLine("Best match for '{0}': {1}", cardName, card.Title);
                }

                var imageSrc = new Uri("http://netrunnerdb.com" + card.ImageSrc);
                webClient.DownloadFile(imageSrc, Path.Combine(cardsPath, cardId + ".png"));
            }
        }

        private string GetSetXmlPath(string name)
        {
            name = name.ToLower() == "core set" ? "core" : name;
            var folderName = Path.Combine(Constants.OctgnPath, "Sets");
            var setFolders = Directory.GetDirectories(folderName).Select(p => p.ToLower());
            var folder =
                setFolders.FirstOrDefault(
                    p => p.EndsWith(name) || (p.Contains("under and over") && name == "up and over"));
            if (folder == null)
            {
                throw new InvalidOperationException(string.Format("Could not find set.xml in folder '{0}'",
                    folderName));
            }

            return Path.Combine(folder, "set.xml");
        }

        private AnrCard FindBestMatch(string cardName, List<AnrCard> cards)
        {
            var distanceList = new List<Tuple<int, AnrCard>>();
            foreach (var anrCard in cards)
            {
                distanceList.Add(new Tuple<int, AnrCard>(LevenshteinDistance.Compute(cardName, anrCard.Title), anrCard));
            }

            var min = distanceList.Min(p => p.Item1);
            var closestMatch = distanceList.FirstOrDefault(p => p.Item1 == min);
            return closestMatch == null ? null : closestMatch.Item2;
        }
    }
}