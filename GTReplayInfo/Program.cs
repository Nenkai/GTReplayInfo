using System;
using System.IO;

using CommandLine.Text;
using CommandLine;

using PDTools.Structures;
using PDTools.SpecDB;

namespace GTReplayInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("== MReplayInfo Viewer ==");
            Parser.Default.ParseArguments<ReadArgs, ExtractArgs>(args)
                .WithParsed<ReadArgs>(Read)
                .WithParsed<ExtractArgs>(Extract);

            Console.WriteLine("Press any key to exit..");
            Console.ReadKey();
        }

        static void Read(ReadArgs options)
        {
            var rply = MReplayInfo.LoadFromFile(options.InputReplay);
            PrintReplayInfoToFile(options.OutputPath, rply);
            Console.WriteLine("Replay successfully parsed.");
        }

        static void Extract(ExtractArgs options)
        {
            string fileName = options.InputReplay;

            var rply = MReplayInfo.LoadFromFile(options.InputReplay);
            PrintReplayInfo(rply);
            Console.WriteLine();

            File.WriteAllBytes(fileName + ".gp", rply.GameParameterBuffer);

            Console.WriteLine("Saved Game Parameter.");

            if (rply.GameParameter.Events.Count > 0)
            {
                for (int i = 0; i < rply.GameParameter.Events.Count; i++)
                {
                    var evnt = rply.GameParameter.Events[i];
                    if (evnt.Course.CustomCourse != null)
                    {
                        File.WriteAllBytes(fileName + $".EVENT{i + 1}.ted", rply.GameParameterBuffer);
                        Console.WriteLine($"Found & Extracted TED file from event #{i + 1}.");
                    }
                }
            }
            File.WriteAllBytes(fileName + ".gp", rply.GameParameterBuffer);
        }

        private static void PrintReplayInfoToFile(string outputPath, MReplayInfo replay)
        {
            StreamWriter sw = new StreamWriter(outputPath);
            sw.WriteLine($"MReplayInfo Information");
            sw.WriteLine($"- Replay Version: {replay.Version}");
            sw.WriteLine($"- Recorded: {replay.RecordedDateTime}");
            sw.WriteLine($"- PFS Version: {replay.FilesystemVersion}");
            sw.WriteLine($"- GTBehavior Version: {replay.GTBehaviorVersion}");
            sw.WriteLine($"- SpecDB Name: {(!string.IsNullOrEmpty(replay.SpecDBName) ? replay.SpecDBName : "<none provided>")}");
            sw.WriteLine($"- SpecDB Version: {replay.SpecDBVersion}");
            sw.WriteLine($"- Race Completed: {replay.RaceCompleted}");
            sw.WriteLine($"- Is One Lap: {replay.OneLap}");
            sw.WriteLine($"- Score: {replay.Score}");
            sw.WriteLine($"- Entries Count: {replay.EntryNum}");
            sw.WriteLine($"- Total Frame Count: {replay.TotalFrameCount}");
            sw.WriteLine($"- Game Parameter Buffer Size: {replay.GameParameterBuffer.Length}");
        }

        private static void PrintReplayInfo(MReplayInfo replay)
        {
            Console.WriteLine($"MReplayInfo Information");
            Console.WriteLine($"- Replay Version: {replay.Version}");
            Console.WriteLine($"- Recorded: {replay.RecordedDateTime}");
            Console.WriteLine($"- PFS Version: {replay.FilesystemVersion}");
            Console.WriteLine($"- GTBehavior Version: {replay.GTBehaviorVersion}");
            Console.WriteLine($"- SpecDB Name: {(!string.IsNullOrEmpty(replay.SpecDBName) ? replay.SpecDBName : "<none provided>")}");
            Console.WriteLine($"- SpecDB Version: {replay.SpecDBVersion}");
            Console.WriteLine($"- Race Completed: {replay.RaceCompleted}");
            Console.WriteLine($"- Is One Lap: {replay.OneLap}");
            Console.WriteLine($"- Score: {replay.Score}");
            Console.WriteLine($"- Entries Count: {replay.EntryNum}");
            Console.WriteLine($"- Total Frame Count: {replay.TotalFrameCount}");
            Console.WriteLine($"- Game Parameter Buffer Size: {replay.GameParameterBuffer.Length}");
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
