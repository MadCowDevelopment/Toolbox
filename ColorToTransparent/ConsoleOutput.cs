using System;

namespace ColorToTransparent
{
    internal static class ConsoleOutput
    {
        public static void PrintHelp()
        {
            Console.WriteLine("Usage: ");
            Console.WriteLine("ColorToTransparent.exe source color");
            Console.WriteLine();
            Console.WriteLine(" source\t\tSpecifies the file or folder to be converted.");
            Console.WriteLine(" color\t\tSpecifies the color that will be made transparent.");
        }

        public static void PrintError(string format, params object[] args)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(format, args);
            Console.ForegroundColor = previousColor;
        }
    }
}