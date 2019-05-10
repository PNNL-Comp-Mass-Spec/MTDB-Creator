using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using MTDBFramework;
using PRISM;

namespace MTDBConsole
{
    class Program
    {
        private const string PROGRAM_DATE = "May 9, 2019";

        public static string GetAppVersion()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version + " (" + PROGRAM_DATE + ")";

            return version;
        }

        static int Main(string[] args)
        {
            Console.WriteLine("************MTDB Creator************");
            Console.WriteLine("-> Creating Database");

            var asmName = typeof(Program).GetTypeInfo().Assembly.GetName();
            var exeName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
            var version = GetAppVersion();

            var parser = new CommandLineParser<CommandOptions>(asmName.Name, version)
            {
                ProgramInfo = "This program creates an AMT tag database.",

                ContactInfo = "Program written by Michael Degan and Bryson Gibbons for the Department of Energy (PNNL, Richland, WA) in 2014" +
                              Environment.NewLine + Environment.NewLine +
                              "E-mail: proteomics@pnnl.gov" + Environment.NewLine +
                              "Website: http://panomics.pnnl.gov/ or http://omics.pnl.gov or http://www.sysbio.org/resources/staff/",

                 UsageExamples = {
                     exeName + "/MzidListPath:MzidInfo.txt /DatabasePath:TestDatabase.mtdb",
                     exeName + "/m:MzidInfo.txt /d:TestDatabase.mtdb"
                }
            };

            var parseResults = parser.ParseArgs(args);
            var options = parseResults.ParsedResults;

            if (!parseResults.Success)
            {
                System.Threading.Thread.Sleep(1500);
                return -1;
            }

            if (!options.ValidateArgs(out var errorMessage))
            {
                parser.PrintHelp();

                Console.WriteLine();
                ConsoleMsgUtils.ShowWarning("Validation error:");
                ConsoleMsgUtils.ShowWarning(errorMessage);

                System.Threading.Thread.Sleep(1500);
                return -1;
            }

            options.OutputSetOptions();

            var mzidFiles = ReadMzidList(options);

            MtdbCreator.CreateDB(mzidFiles, options.DatabasePath);

            Console.WriteLine("MTDB Creator Complete!");

            return 0;
        }

        static List<string> ReadMzidList(CommandOptions options)
        {
            var pathList = new List<string>();

            using (var input = new StreamReader(options.MzidListPath))
            {
                while (input.Peek() > -1)
                {
                    var entry = input.ReadLine();
                    pathList.Add(entry);
                }
            }

            return pathList;
        }
    }
}
