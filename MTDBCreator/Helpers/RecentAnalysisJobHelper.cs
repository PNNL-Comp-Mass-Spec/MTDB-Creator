#region Namespaces

using System;
using System.Globalization;
using System.Linq;
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
        internal static int RecentAnalysisJobCount => Settings.Default.RecentAnalysisJobs.Count;

        internal static void AddRecentAnalysisJob(AnalysisJobViewModel analysisJobViewModel)
        {
            if (Settings.Default.RecentAnalysisJobs.Count == Settings.Default.RecentAnalysisJobLimit)
            {
                Settings.Default.RecentAnalysisJobs.RemoveAt(Settings.Default.RecentAnalysisJobLimit - 1);
            }

            var analysisJobString = GetRecentAnalysisJobString(analysisJobViewModel);

            RemoveRecentAnalysisJob(GetRecentAnalysisJobHash(analysisJobString));

            Settings.Default.RecentAnalysisJobs.Insert(0, analysisJobString);
            Settings.Default.Save();
        }

        internal static void RemoveRecentAnalysisJob(string hash)
        {
            var recentAnalysisJobString = Settings.Default.RecentAnalysisJobs.
                                                Cast<string>().FirstOrDefault(s => s.StartsWith(hash));

            if (!String.IsNullOrEmpty(recentAnalysisJobString))
            {
                Settings.Default.RecentAnalysisJobs.Remove(recentAnalysisJobString);
            }
        }

        internal static string GetRecentAnalysisJobTitle(string analysisJobString)
        {
            var startIndex = analysisJobString.IndexOf("|", StringComparison.Ordinal); // | after hash
            startIndex = analysisJobString.IndexOf("|", startIndex + 1, StringComparison.Ordinal); // | after no.
            var endIndex = analysisJobString.IndexOf("|", startIndex + 1, StringComparison.Ordinal); // | after title

            return analysisJobString.Substring(startIndex + 1, endIndex - startIndex - 1);
        }

        internal static string GetRecentAnalysisJobHash(string analysisJobString)
        {
            return analysisJobString.Substring(0, analysisJobString.IndexOf("|", StringComparison.Ordinal));
        }

        internal static AnalysisJobViewModel GetRecentAnalysisJobItem(string analysisJobString)
        {
            // Format: <Hash>|<No>|<Title>|<Workflow>|<FileName 1>|<Format 1>|<FileName 2>|<Format 2>|<FileName 3>|<Format 3>|...

            var dataParts = analysisJobString.Split('|');

            var analysisJobViewModel = new AnalysisJobViewModel
            {
                Title = dataParts[2],
                Options = {TargetFilterType = (TargetWorkflowType) Enum.Parse(typeof (TargetWorkflowType), dataParts[3])}
            };

            for (var i = 4; i < dataParts.Length; i += 2)
            {
                var idTool = (LcmsIdentificationTool)Enum.Parse(typeof(LcmsIdentificationTool), dataParts[i + 1]);
                analysisJobViewModel.AnalysisJobItems.Add(new AnalysisJobItem(dataParts[i], idTool));
            }

            return analysisJobViewModel;
        }

        private static string GetRecentAnalysisJobString(AnalysisJobViewModel analysisJobViewModel)
        {
            var sb = new StringBuilder();

            // Format: <Hash>|<No>|<Title>|<Workflow>|<FileName 1>|<Format 1>|<FileName 2>|<Format 2>|<FileName 3>|<Format 3>|...

            sb.Append("|");
            sb.Append(analysisJobViewModel.Id.ToString(CultureInfo.InvariantCulture)); // Get new recent analysis job item no.
            sb.Append("|");
            sb.Append(analysisJobViewModel.Title);

            sb.Insert(0, HashHelper.ComputeStringHash(sb.ToString()));

            sb.Append("|");
            sb.Append(analysisJobViewModel.Options.TargetFilterType);

            foreach (var analysisJobItem in analysisJobViewModel.AnalysisJobItems)
            {
                sb.Append("|");
                sb.Append(analysisJobItem.FilePath);
                sb.Append("|");
                sb.Append(analysisJobItem.Format);
            }

            return sb.ToString();
        }
    }
}