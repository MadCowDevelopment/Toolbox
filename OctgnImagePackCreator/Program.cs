using System;
using System.IO;

namespace OctgnImagePackCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                InitializeEnvironment();

                var command = ParseArgs(new[] { Constants.Download, "uao" });
                command.Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error: " + e.Message);
            }
        }

        private static Command ParseArgs(string[] args)
        {
            if (args.Length <= 0 || args.Length > 2) return new UsageCommand();
            var command = args[0].ToLower();
            switch (command)
            {
                case Constants.Update:
                    return new UpdateCommand();
                case Constants.Sets:
                    return new ListSetsCommand();
                case Constants.Download:
                    if (args.Length == 1) return new DownloadAllSetsCommand();
                    return new DownloadSetCommand(args[1]);
            }

            return new UsageCommand();
        }

        private static void InitializeEnvironment()
        {
            Directory.CreateDirectory(Constants.DataPath);
        }
    }
}
