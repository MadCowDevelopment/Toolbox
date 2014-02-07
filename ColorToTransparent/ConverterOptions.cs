using System.Collections.Generic;
using System.Drawing;

namespace ColorToTransparent
{
    internal class ConverterOptions
    {
        public ConverterOptions(List<string> filesToConvert, Color colorToMakeTransparent)
        {
            FilesToConvert = filesToConvert;
            ColorToMakeTransparent = colorToMakeTransparent;
        }

        public List<string> FilesToConvert { get; private set; }
        public Color ColorToMakeTransparent { get; private set; }
    }
}