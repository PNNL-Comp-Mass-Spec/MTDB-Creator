﻿using MTDBFramework.Algorithms;
using MTDBFramework.Data;
using MTDBFramework.IO;
using PRISM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using MTDBFrameworkBase.Data;
using MTDBFrameworkBase.Database;
using MTDBFrameworkBase.IO;

namespace MTDBFramework
{
    /// <summary>
    /// API for using MTDBCreator in other applications
    /// </summary>
    public static class MtdbCreator
    {
        /// <summary>
        /// Create a SQLite-based MTDB with the given files
        /// </summary>
        /// <param name="paths">Paths to the files to process</param>
        /// <param name="dbFileName">Name of MTDB to create</param>
        /// <returns></returns>
        public static TargetDatabase CreateDB(List<string> paths, string dbFileName)
        {
            try
            {
                var options = new Options
                {
                    ConsoleMode = true
                };

                var analysisProcessor = new AnalysisJobProcessor(options);
                var mtdbProcessor = new MtdbProcessor(options);
                var bWorker = new BackgroundWorker();
                var writer = new SqLiteTargetDatabaseWriter();

                var reader = new SqLiteTargetDatabaseReader();
                var existingDatabase = new TargetDatabase();

                IEnumerable<LcmsDataSet> priorDataSets = new List<LcmsDataSet>();

                options.DatabaseType = DatabaseType.SQLite;
                if (dbFileName.EndsWith(".mdb", StringComparison.OrdinalIgnoreCase))
                {
                    ConsoleMsgUtils.ShowWarning("The console version of MtdbCreator only supports SQLite files; changing the output file extension to .mtdb");
                    dbFileName = Path.ChangeExtension(dbFileName, ".mtdb");
                }

                if (File.Exists(dbFileName))
                {
                    priorDataSets = reader.Read(dbFileName);
                    existingDatabase = reader.ReadDb(dbFileName);
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
                    var processedJobs = analysisProcessor.Process(listJobs, bWorker);

                    var datasetList = processedJobs.Select(job => job.DataSet).ToList();
                    datasetList.AddRange(priorDataSets);
                    var database = mtdbProcessor.Process(datasetList, bWorker);

                    writer.Write(database, options, dbFileName);

                    return database;
                }

                return existingDatabase;
            }
            catch (Exception ex)
            {
                ConsoleMsgUtils.ShowError("Exception creating the AMT Tag DB", ex);
                return new TargetDatabase();
            }
        }

        /// <summary>
        /// Load an existing MTDB database into a TargetDatabase
        /// </summary>
        /// <param name="path">Path to MTDB file</param>
        /// <returns></returns>
        public static TargetDatabase LoadDB(string path)
        {
            if (File.Exists(path) && Path.GetExtension(path) == ".mtdb")
            {
                var mtdbReader = new SqLiteTargetDatabaseReader();
                return mtdbReader.ReadDb(path);
            }
            return new TargetDatabase();
        }

        private static LcmsIdentificationTool DetermineTool(string path)
        {
            var tool = LcmsIdentificationTool.NOT_SUPPORTED;

            var pathLower = path.ToLower();

            if (pathLower.EndsWith("msgfdb_syn.txt") || pathLower.EndsWith("msgfplus_syn.txt"))
            {
                tool = LcmsIdentificationTool.MsgfPlus;
            }
            else if (pathLower.EndsWith("_syn.txt"))
            {
                tool = LcmsIdentificationTool.Sequest;
            }
            else if (pathLower.EndsWith("_xt.txt"))
            {
                tool = LcmsIdentificationTool.XTandem;
            }
            else if (pathLower.EndsWith(".mzid") || pathLower.EndsWith(".mzid.gz"))
            {
                tool = LcmsIdentificationTool.MZIdentML;
            }
            else if (pathLower.EndsWith("msalign_syn.txt"))
            {
                tool = LcmsIdentificationTool.MSAlign;
            }

            return tool;
        }
    }
}
