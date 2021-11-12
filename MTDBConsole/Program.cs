using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using MTDBFramework;
using PRISM;

namespace MTDBConsole
{
    internal class Program
    {
        // Ignore Spelling: Bryson, Degan

        private const string PROGRAM_DATE = "November 11, 2021";

        public static string GetAppVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version + " (" + PROGRAM_DATE + ")";
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

                ContactInfo = "Program written by Michael Degan and Bryson Gibbons for the Department of Energy (PNNL, Richland, WA)" +
                              Environment.NewLine + Environment.NewLine +
                              "E-mail: proteomics@pnnl.gov" + Environment.NewLine +
                              "Website: https://github.com/PNNL-Comp-Mass-Spec/ or https://panomics.pnnl.gov/ or https://www.pnnl.gov/integrative-omics",

                 UsageExamples = {
                     exeName + "/MzidListPath:MzidInfo.txt /DatabasePath:TestDatabase.mtdb",
                     exeName + "/m:MzidInfo.txt /d:TestDatabase.mtdb"
                }
            };

            var result = parser.ParseArgs(args);
            var options = result.ParsedResults;

            if (!result.Success)
            {
                if (parser.CreateParamFileProvided)
                {
                    return 0;
                }

                // Delay for 1500 msec in case the user double clicked this file from within Windows Explorer (or started the program via a shortcut)
                Thread.Sleep(1500);
                return -1;
            }

            if (!options.ValidateArgs(out var errorMessage))
            {
                parser.PrintHelp();

                Console.WriteLine();
                ConsoleMsgUtils.ShowWarning("Validation error:");
                ConsoleMsgUtils.ShowWarning(errorMessage);

                Thread.Sleep(1500);
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
