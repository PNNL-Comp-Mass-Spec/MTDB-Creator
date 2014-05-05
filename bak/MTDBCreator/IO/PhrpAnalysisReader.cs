using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBCreator.Data;
using System.IO;

namespace MTDBCreator.IO
{

    public abstract class PhrpAnalysisReader
    {
        protected string mstrSeqToProteinMapExt = "_xt_SeqToProteinMap.txt";
        protected string mstrResultsExt = "_xt.txt";
        protected string mstrResultToSeqMapExt = "_xt_ResultToSeqMap.txt";
        protected string mstrSeqInfoExt = "_xt_SeqInfo.txt";


        /// <summary>
        /// Maps the file extension to the path of the file.
        /// </summary>
        protected readonly Dictionary<string, string> m_analysisMap;

        public PhrpAnalysisReader()
        {
            m_analysisMap = new Dictionary<string, string>();
        }

        /// <summary>
        /// Maps the paths 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        protected void MapPaths(string path, string name)
        {
            // Simple error checking...
            bool isDirectory = Directory.Exists(path);
            if (!isDirectory)
            {
                throw new DirectoryNotFoundException(string.Format("The directory path {0} does not exist.", path));
            }

            // Find the files associated with this analysis given the name
            string[] files = Directory.GetFiles(path, string.Format("{0}*", name), SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                bool found = false;
                string extensionToUse = "";
                foreach (string extension in m_analysisMap.Keys)
                {
                    if (file.EndsWith(extension))
                    {
                        extensionToUse = extension;
                        found = true;
                        break;
                    }
                }
                if (found)
                    m_analysisMap[extensionToUse] = file;
            }
        }
        /// <summary>
        /// Maps the sequence data onto a list of targets
        /// </summary>
        /// <param name="targets"></param>
        /// <param name="sequences"></param>
        /// <param name="resultsMap"></param>
        protected void MapSequencesOntoTargets(List<Target> targets,
                                                                    List<Sequence> sequences,
                                                                    List<ResultToSequenceMap> resultsMap)
        {
            // map the sequences into the results            
            Dictionary<int, Sequence> sequenceMap = new Dictionary<int, Sequence>();
            foreach (Sequence sequence in sequences)
            {
                sequenceMap.Add(sequence.Id, sequence);
            }

            Dictionary<int, Target> targetMap = new Dictionary<int, Target>();
            int id = 0;
            foreach (Target target in targets)
            {
                target.Id = id++;
                targetMap.Add(target.Id, target);
            }

            // Finally we can get rid of that result to sequence map BS.
            foreach (ResultToSequenceMap map in resultsMap)
            {
                bool existsInTargets = targetMap.ContainsKey(map.ResultId);
                bool existsInSequences = sequenceMap.ContainsKey(map.UniqueSequenceId);

                if (existsInSequences && existsInTargets)
                {
                    Target target       = targetMap[map.ResultId];
                    Sequence sequence   = sequenceMap[map.UniqueSequenceId];
                    target.SequenceData = sequence;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targets"></param>
        /// <param name="sequenceMaps"></param>
        /// <returns></returns>
        protected virtual List<Protein> MapProteins(List<Target> targets, List<SequenceToProteinMap> sequenceMaps)
        {
            return ProteinMapper.MapProteins(targets, sequenceMaps);
        }
        /// <summary>
        /// Filters targets organized through the PHRP reader
        /// </summary>
        /// <param name="targets"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        protected List<Target> FilterTargets(List<Target> targets, ITargetFilter filter)
        {
            // Map the sequences first for each target
            //TODO: Convert to LINQ maybe...O(N) here, then O(lgN) -> O(N + lgN) 
            Dictionary<string, List<Target>> xmap = new Dictionary<string, List<Target>>();
            foreach (Target target in targets)
            {
                if (!xmap.ContainsKey(target.Sequence))
                {
                    xmap.Add(target.Sequence, new List<Target>());
                }
                xmap[target.Sequence].Add(target);
            }

            List<Target> filteredTargets = new List<Target>();
            foreach (string key in xmap.Keys)
            {
                // Find the first occurrence.
                List<Target> xTargets = xmap[key];
                xTargets.Sort(delegate(Target x, Target y)
                {
                    return y.Scan.CompareTo(x.Scan);
                });

                // Then make sure it passes the filters
                Target targetOfInterest = xTargets[0];
                if (!filter.ShouldFilter(targetOfInterest))
                {
                    filteredTargets.Add(targetOfInterest);
                }
            }

            return filteredTargets;
        }
        /// <summary>
        /// Read the analysis path.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Analysis Read(string path, string name)
        {
            Analysis result = new Analysis();
            MapPaths(path, name);

            /* Sequence to Protein File ----------------------------------------------------------------*/
            string seqToProteinMapFile = m_analysisMap[mstrSeqToProteinMapExt];
            SequenceToProteinMapReader seqToProteinMapReader = new SequenceToProteinMapReader();
            List<SequenceToProteinMap> sequenceMaps = seqToProteinMapReader.Read(seqToProteinMapFile);

            /* Results File */
            string resultsFile = m_analysisMap[mstrResultsExt];
            List<Target> targets = ReadTargets(resultsFile);

            /* Results to Sequence Map File ----------------------------------------------------------------*/
            string resultsToSeqMapFile = m_analysisMap[mstrResultToSeqMapExt];
            ResultsToSequenceMapReader resultsToSeqMapReader = new ResultsToSequenceMapReader();
            List<ResultToSequenceMap> resultsMap = resultsToSeqMapReader.Read(resultsToSeqMapFile);

            /* Sequest Info File ----------------------------------------------------------------*/
            string seqInfoFile = m_analysisMap[mstrSeqInfoExt];
            SequenceFileReader seqInfoReader = new SequenceFileReader();
            List<Sequence> sequences = seqInfoReader.Read(seqInfoFile);

            // Then map all of the data onto thee....
            MapSequencesOntoTargets(targets, sequences, resultsMap);
            result.AddTargets(targets);
            result.Proteins = MapProteins(result.Targets, sequenceMaps);

            return result;
        }

        /// <summary>
        /// Required method for reading the targets specific to a search engine.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected abstract List<Target> ReadTargets(string path);
    }
}
