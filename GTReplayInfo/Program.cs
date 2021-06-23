using System;

using CommandLine.Text;
using CommandLine;

using PDTools;
using PDTools.SpecDB.Core;

namespace GTReplayInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<ReadArgs, ExtractArgs>(args)
                .WithParsed<ReadArgs>(Read)
                .WithParsed<ExtractArgs>(Extract);
        }

        static void Read(ReadArgs options)
        {
            var rply = MReplayInfo.LoadFromFile(options.InputReplay);

            var specdb = SpecDB.LoadFromSpecDBFolder(options.InputSpecDB, false);
            rply.PrintToFile(options.OutputPath, specdb);
        }

        static void Extract(ExtractArgs options)
        {
            var rply = MReplayInfo.LoadFromFile(options.InputReplay);
        }
    }

    [Verb("printinfo")]
    public class ReadArgs
    {
        [Option('i', "input", Required = true, HelpText = "Input replay.")]
        public string InputReplay { get; set; }

        [Option('s', "specdb", Required = true, HelpText = "SpecDB input path")]
        public string InputSpecDB { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output path.")]
        public string OutputPath { get; set; }
    }

    [Verb("extract")]
    public class ExtractArgs
    {
        [Option('i', "input", Required = true, HelpText = "Input replay.")]
        public string InputReplay { get; set; }


    }
}
