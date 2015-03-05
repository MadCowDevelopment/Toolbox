using System;

namespace OctgnImagePackCreator
{
    internal class UsageCommand : Command
    {
        public override void Execute()
        {
            Console.WriteLine("Usage: OctgnImagePackCreater.exe command");
            Console.WriteLine();
            Console.WriteLine("Commands:");
            Console.WriteLine("update");
            Console.WriteLine(
                "   Downloads set and card data from netrunnerdb.");
            Console.WriteLine("sets");
            Console.WriteLine("   Lists all available sets that can be created.");
            Console.WriteLine("download [set code]");
            Console.WriteLine("   Downloads images for the specified set.");
        }
    }
}