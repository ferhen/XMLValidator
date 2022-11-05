using XMLValidator.Core;
using CommandLine;

namespace XMLValidator.CLI
{
    internal class Program
    {
        public class Options
        {
            [Option('i', "input", Required = false, HelpText = "The raw XML to be validated.", SetName = "input")]
            public string? Input { get; set; }

            [Option('f', "file", Required = false, HelpText = "The path to the XML to be validated.", SetName = "file")]
            public string? File { get; set; }
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options =>
                {
                    if (string.IsNullOrWhiteSpace(options.Input) && string.IsNullOrWhiteSpace(options.File))
                    {
                        Console.WriteLine("Please provide either input or file.");
                        Environment.Exit(1);
                    }

                    var xml = string.Empty;

                    if (!string.IsNullOrEmpty(options.Input))
                    {
                        xml = options.Input;
                    } else if (!string.IsNullOrEmpty(options.File))
                    {
                        xml = File.ReadAllText(options.File);
                    }

                    var validator = new Validator();
                    var result = validator.DetermineSxml(xml) ? "XML is valid" : "XML is invalid";

                    Console.WriteLine(result);

                    Environment.Exit(0);
                });
        }
    }
}