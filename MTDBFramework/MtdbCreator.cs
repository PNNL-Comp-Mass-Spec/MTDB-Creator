using MTDBFramework.Algorithms;
using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace MTDBFramework
{
	/// <summary>
	/// API for using MTDBCreator in other applications
	/// </summary>
    public class MtdbCreator
    {
		/// <summary>
		/// Create a MTDB with the given files
		/// </summary>
		/// <param name="paths">Paths to the files to process</param>
		/// <param name="dbFileName">Name of MTDB to create</param>
		/// <returns></returns>
        public TargetDatabase CreateDB(List<string> paths, string dbFileName)
        {
            var options = new Options();
            var analysisProcessor = new AnalysisJobProcessor(options);
            var mtdbProcessor = new MtdbProcessor(options);
            var bWorker = new BackgroundWorker();
            var writer = new SqLiteTargetDatabaseWriter();

            var reader = new SqLiteTargetDatabaseReader();
            var existingDatabase = new TargetDatabase();

            IEnumerable<LcmsDataSet> priorDataSets = new List<LcmsDataSet>();

            if (File.Exists(dbFileName))
            {
                priorDataSets = reader.Read(dbFileName);
                existingDatabase = reader.ReadDB(dbFileName);
            }
            var priorDatasetList = priorDataSets.Select(x => x.Name).ToList();
            var listJobs = new List<AnalysisJobItem>();

            foreach (var path in paths)
            {
                if (File.Exists(path))
                {
                    var tool = DetermineTool(path);
                    var jobName = path.Split('\\').Last();
                    if (tool == LcmsIdentificationTool.NOT_SUPPORTED)
                    {
                        Console.WriteLine(jobName + " is not a supported LCMS format for MTDBCreator;\nExcluding this file from MTDB creation\n");
                    }
                    else if (priorDatasetList.Any(x => jobName.Contains(x)))
                    {
                        Console.WriteLine(jobName + " is part of prior analysis;\nExcluding from reading portion of MTDB creation\n");
                    }
                    else
                    {
                        var individualJob = new AnalysisJobItem(path, tool);
                        listJobs.Add(individualJob);
                    }
                }
                else
                {
                    Console.WriteLine(path + " does not exist;\nExcluding this file in MTDB creation\n");
                }
            }
            if (listJobs.Count != 0)
            {
                var processedjobs = analysisProcessor.Process(listJobs, bWorker);

                var datasetList = processedjobs.Select(job => job.DataSet).ToList();
                if (priorDataSets != null)
                {
                    datasetList.AddRange(priorDataSets);
                }
                var database = mtdbProcessor.Process(datasetList, bWorker);

                writer.Write(database, options, dbFileName);

                return database;
            }
            else
            {
                return existingDatabase;
            }
        }

        private LcmsIdentificationTool DetermineTool(string path)
        {
            LcmsIdentificationTool tool = LcmsIdentificationTool.NOT_SUPPORTED;
            if (path.EndsWith("msgfdb_syn.txt"))
            {
                tool = LcmsIdentificationTool.MsgfPlus;
            }
            else if (path.EndsWith("_syn.txt"))
            {
                tool = LcmsIdentificationTool.Sequest;
            }
            else if (path.EndsWith("_xt.txt"))
            {
                tool = LcmsIdentificationTool.XTandem;
            }
            else if (path.EndsWith("msgfplus.mzid"))
            {
                tool = LcmsIdentificationTool.MZIdentML;
            }
            else if (path.EndsWith("msalign_syn.txt"))
            {
                tool = LcmsIdentificationTool.MSAlign;
            }

            return tool;
        }
    }
}
