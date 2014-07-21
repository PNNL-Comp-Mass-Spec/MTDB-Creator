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
    public class MtdbCreator
    {
        public TargetDatabase CreateDB(List<string> paths, string dbFileName)
        {
            var options = new Options();
            var analysisProcessor = new AnalysisJobProcessor(options);
            var mtdbProcessor = new MtdbProcessor(options);
            var bWorker = new BackgroundWorker();
            var writer = new SqLiteTargetDatabaseWriter();

            SqLiteTargetDatabaseReader reader = new SqLiteTargetDatabaseReader();
            IEnumerable<LcmsDataSet> test = new List<LcmsDataSet>();
            if (File.Exists(dbFileName))
            {
                test = reader.Read(dbFileName);
                var testDB = reader.ReadDB(dbFileName);
            }

            var listJobs = new List<AnalysisJobItem>();

            foreach (var path in paths)
            {
                if (File.Exists(path))
                {
                    var tool = DetermineTool(path);
                    if (tool == LcmsIdentificationTool.NOT_SUPPORTED)
                    {
                        var jobName = path.Split('\\').Last();
                        Console.WriteLine(jobName + " is not a supported LCMS format for MTDBCreator;\nExcluding this file from MTDB creation\n");
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

            var processedjobs = analysisProcessor.Process(listJobs, bWorker);

            var datasetList = processedjobs.Select(job => job.DataSet).ToList();
            if (test != null)
            {
                datasetList.AddRange(test);
            }
            var database = mtdbProcessor.Process(datasetList, bWorker);

            writer.Write(database, options, dbFileName);

            return database;
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
