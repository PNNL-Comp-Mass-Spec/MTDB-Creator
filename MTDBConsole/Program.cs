using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using MTDBFramework;

namespace MTDBConsole
{
	class Program
	{
		public static CommandOptions CommandOptions;

		static Program()
		{
			CommandOptions = new CommandOptions();
		}

		static void Main(string[] args)
		{
			Console.WriteLine("************MTDB Creator************");
			Console.WriteLine("-> Creating Database");
			Parser.Default.ParseArguments(args, CommandOptions);

			MtdbCreator.CreateDB(ReadMzidList(), CommandOptions.DatabasePath);
			Console.WriteLine("MTDB Creator Complete!");
		}

		static List<string> ReadMzidList()
		{
			List<string> pathList = new List<string>();

			using (StreamReader input = new StreamReader(CommandOptions.MzidListPath))
			{
				while (input.Peek() > -1)
				{
					string entry = input.ReadLine();
					pathList.Add(entry);
				}
			}

			return pathList;
		}
	}
}
