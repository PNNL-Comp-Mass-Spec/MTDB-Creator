﻿using MTDBFrameworkBase.Data;

namespace MTDBFramework.Data
{
    /// <summary>
    /// Target filtering for X!Tandem Workflows
    /// </summary>
    public class XTandemTargetFilter : ITargetFilter
    {
        /// <summary>
        /// Options
        /// </summary>
        public Options FilterOptions { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public XTandemTargetFilter(Options options)
        {
            FilterOptions = options;
        }

        /// <summary>
        /// Determine whether the given evidence should be filtered out
        /// </summary>
        /// <param name="evidence">Peptide evidence</param>
        /// <returns>True if the evidence should be filtered out (i.e. does not pass filters); false to keep it</returns>
        public bool ShouldFilter(Evidence evidence)
        {
            var result = evidence as XTandemResult;

            if (result == null)
            {
                return true;
            }

            return ShouldFilter(result.LogPeptideEValue, result.SpecProb);
        }

        /// <summary>
        /// Determine whether the given evidence should be filtered out
        /// </summary>
        /// <param name="logPepEValue">X!Tandem Log EValue</param>
        /// <param name="specProb">MSGF+ SpecProb</param>
        /// <returns>True if the evidence should be filtered out (i.e. does not pass filters); false to keep it</returns>
        public bool ShouldFilter(double logPepEValue, double specProb)
        {
            if (logPepEValue > FilterOptions.MaxLogEValForXTandemAlignment)
            {
                return true;
            }

            if (specProb > FilterOptions.MaxMsgfSpecProb)
            {
                return true;
            }

            return false;
        }
    }
}