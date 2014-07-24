#region Namespaces

using MTDBFramework.Data;

#endregion

namespace MTDBFramework.Algorithms.Alignment
{
	/// <summary>
	/// Alignment Filtering for MSGF+ Workflows
	/// </summary>
    public class MsgfPlusAlignmentFilter : ITargetFilter
    {
		/// <summary>
		/// Options
		/// </summary>
        public Options FilterOptions { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="options"></param>
        public MsgfPlusAlignmentFilter(Options options)
        {
            FilterOptions = options;
        }

        /// <summary>
        /// Determine whether the given evidence should be filtered out
        /// </summary>
        /// <param name="evidence"></param>
        /// <returns>True if the evidence should be filtered out (i.e. does not pass filters); false to keep it</returns>
        public bool ShouldFilter(Evidence evidence)
        {
            // In the alignment we will only use the unmodified peptides
            if (evidence.ModificationCount > FilterOptions.MaxModsForAlignment)
            {
                return true;
            }

            return false;
        }
    }
}
