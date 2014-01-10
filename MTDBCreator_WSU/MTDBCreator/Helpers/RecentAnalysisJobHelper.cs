#region Namespaces

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Policy;
using System.Text;
using MTDBCreator.Properties;
using MTDBCreator.ViewModels;
using MTDBFramework.Algorithms;
using MTDBFramework.Data;

#endregion

namespace MTDBCreator.Helpers
{
    internal static class RecentAnalysisJobHelper
    {
        internal static int RecentAnalysisJobCount
        {
            get
            {
                return Settings.Default.RecentAnalysisJobs.Count;
            }
        }

        internal static void AddRecentAnalysisJob(AnalysisJobViewModel analysisJobViewModel)
        {
            if (Settings.Default.RecentAnalysisJobs.Count == Settings.Default.RecentAnalysisJobLimit)
            {
                Settings.Default.RecentAnalysisJobs.RemoveAt(Settings.Default.RecentAnalysisJobLimit - 1);
            }

            string analysisJobString = GetRecentAnalysisJobString(analysisJobViewModel);

            RemoveRecentAnalysisJob(GetRecentAnalysisJobHash(analysisJobString));

            Settings.Default.RecentAnalysisJobs.Insert(0, analysisJobString);
            Settings.Default.Save();
        }

        internal static void RemoveRecentAnalysisJob(string hash)
        {
            string recentAnalysisJobString = null;

            foreach (string s in Settings.Default.RecentAnalysisJobs)
            {
                if (s.StartsWith(hash))
                {
                    recentAnalysisJobString = s;

                    break;
                }
            }

            if (!String.IsNullOrEmpty(recentAnalysisJobString))
            {
                Settings.Default.RecentAnalysisJobs.Remove(recentAnalysisJobString);
            }
        }

        internal static string GetRecentAnalysisJobTitle(string analysisJobString)
        {
            int startIndex = analysisJobString.IndexOf("|"); // | after hash
            startIndex = analysisJobString.IndexOf("|", startIndex + 1); // | after no.
            int endIndex = analysisJobString.IndexOf("|", startIndex + 1); // | after title

            return analysisJobString.Substring(startIndex + 1, endIndex - startIndex - 1);
        }

        internal static string GetRecentAnalysisJobHash(string analysisJobString)
        {
            return analysisJobString.Substring(0, analysisJobString.IndexOf("|"));
        }

        internal static AnalysisJobViewModel GetRecentAnalysisJobItem(string analysisJobString)
        {
            // Format: <Hash>|<No>|<Title>|<Workflow>|<FileName 1>|<Format 1>|<FileName 2>|<Format 2>|<FileName 3>|<Format 3>|...

            string[] strs = analysisJobString.Split('|');

            AnalysisJobViewModel analysisJobViewModel = new AnalysisJobViewModel()
            {
                Title = strs[2]
            };

            analysisJobViewModel.Options.TargetFilterType = (TargetWorkflowType)Enum.Parse(typeof(TargetWorkflowType), strs[3]);

            for (int i = 4; i < strs.Length; i += 2)
            {
                analysisJobViewModel.AnalysisJobItems.Add(new AnalysisJobItem(strs[i], (LcmsIdentificationTool)Enum.Parse(typeof(LcmsIdentificationTool), strs[i + 1])));
            }

            return analysisJobViewModel;
        }

        private static string GetRecentAnalysisJobString(AnalysisJobViewModel analysisJobViewModel)
        {
            StringBuilder sb = new StringBuilder();

            // Format: <Hash>|<No>|<Title>|<Workflow>|<FileName 1>|<Format 1>|<FileName 2>|<Format 2>|<FileName 3>|<Format 3>|...

            sb.Append("|");
            sb.Append(analysisJobViewModel.Id.ToString()); // Get new recent analysis job item no.
            sb.Append("|");
            sb.Append(analysisJobViewModel.Title);

            sb.Insert(0, HashHelper.ComputeStringHash(sb.ToString()));

            sb.Append("|");
            sb.Append(analysisJobViewModel.Options.TargetFilterType.ToString());

            foreach (AnalysisJobItem analysisJobItem in analysisJobViewModel.AnalysisJobItems)
            {
                sb.Append("|");
                sb.Append(analysisJobItem.FilePath);
                sb.Append("|");
                sb.Append(analysisJobItem.Format.ToString());
            }

            return sb.ToString();
        }
    }
}