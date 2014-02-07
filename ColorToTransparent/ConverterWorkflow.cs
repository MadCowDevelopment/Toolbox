namespace ColorToTransparent
{
    internal class ConverterWorkflow
    {
        public void Convert(string[] args)
        {
            var options = new CommandLineParser().ParseCommandLine(args);
            foreach (var file in options.FilesToConvert)
            {
                new ColorToTransparencyConverter().Convert(file, options.ColorToMakeTransparent);
            }
        }
    }
}