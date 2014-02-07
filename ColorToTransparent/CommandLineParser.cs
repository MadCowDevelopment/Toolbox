using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ColorToTransparent
{
    internal class CommandLineParser
    {
        public ConverterOptions ParseCommandLine(string[] args)
        {
            if (args.Length < 2)
            {
                ConsoleOutput.PrintHelp();
                Environment.Exit(1);
            }

            var files = ParseSourceArgument(args[0]);
            if (files.Count == 0)
            {
                ConsoleOutput.PrintError(
                    "File or directory does not exist or does not contain any supported image files.");
                Environment.Exit(1);
            }

            var color = ParseColorArgument(args[1]);
            if (color == Color.Empty)
            {
                ConsoleOutput.PrintError("The provided color does not exist.");
                Environment.Exit(1);
            }

            return new ConverterOptions(files, color);
        }

        private Color ParseColorArgument(string color)
        {
            KnownColor result;
            if (Enum.TryParse(color, out result))
            {
                return Color.FromName(color);
            }

            try
            {
                var splitColors = color.Split(',').Select(int.Parse).ToList();
                if (splitColors.Count == 3)
                {
                    return Color.FromArgb(splitColors[0], splitColors[1], splitColors[2]);
                }
            }
            catch (Exception e)
            {
                ConsoleOutput.PrintError("Exception: {0}", e.Message);
            }

            return Color.Empty;
        }

        private List<string> ParseSourceArgument(string source)
        {
            if (File.Exists(source))
            {
                return new List<string> { source };
            }

            if (Directory.Exists(source))
            {
                return
                    new List<string>(
                        Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories)
                            .Where(file => file.ToLower().EndsWith(".png")));
            }

            return new List<string>();
        }
    }
}