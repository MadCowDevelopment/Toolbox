using System;
using System.Drawing;
using System.IO;

namespace ColorToTransparent
{
    internal class ColorToTransparencyConverter
    {
        public void Convert(string filename, Color color)
        {
            try
            {
                using (var bitmap = new Bitmap(filename))
                {
                    bitmap.MakeTransparent(color);
                    bitmap.Save(filename);
                }
            }
            catch (FileNotFoundException)
            {
                ConsoleOutput.PrintError("File {0} was not found.", filename);
            }
            catch (Exception e)
            {
                ConsoleOutput.PrintError(e.Message);
            }
        }
    }
}