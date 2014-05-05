using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBCreator.Data
{
    /// <summary>
    /// Class that maintains a cache of the datasets that have been analyzed
    /// </summary>
    public class AnalysisManager: IEnumerable<Analysis>
    {
        /// <summary>
        /// The map 
        /// </summary>
        private Dictionary<string, Analysis> m_analysisMap;        

        public AnalysisManager()
        {
            m_analysisMap = new Dictionary<string, Analysis>();
        }

        /// <summary>
        /// Adds the dataset to the list 
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="analysis"></param>
        public void AddDataset(string dataset, Analysis analysis)
        {
            if (m_analysisMap.ContainsKey(dataset))
                m_analysisMap[dataset] = analysis;
            else
                m_analysisMap.Add(dataset, analysis);
        }
        /// <summary>
        /// Retrieves the analysis if it exists.  Null if not.
        /// </summary>
        /// <param name="dataset"></param>
        /// <returns></returns>
        public Analysis GetAnalysis(string dataset)
        {
            Analysis analysis = null;
            if (m_analysisMap.ContainsKey(dataset))
            {
                analysis = m_analysisMap[dataset];
            }
            return analysis;
        }

        /// <summary>
        /// Retrieves a list of analysis.
        /// </summary>
        /// <returns></returns>
        public List<Analysis> GetAnalysis()
        {
            List<Analysis> analysis = new List<Analysis>();
            foreach (Analysis a in m_analysisMap.Values)
            {
                analysis.Add(a);
            }
            return analysis;
        }

        #region IEnumerable<Analysis> Members

        public IEnumerator<Analysis> GetEnumerator()
        {
            foreach (Analysis value in m_analysisMap.Values)
            {
                yield return value;
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (Analysis value in m_analysisMap.Values)
            {
                yield return value;
            }
        }

        #endregion
    }
}
