namespace MTDBFramework.Algorithms.Clustering
{
	/// <summary>
	/// Tools for clustering targets
	/// </summary>
    public static class TargetClustererFactory
    {
		/// <summary>
		/// Configure and return a Target Clusterer
		/// </summary>
		/// <param name="workflowType"></param>
		/// <returns></returns>
        public static ITargetClusterer Create(TargetWorkflowType workflowType)
        {
            ITargetClusterer clusterer = null;

            switch (workflowType)
            {
                case TargetWorkflowType.TOP_DOWN:
                    clusterer = new SequenceTargetClusterer();
                    break;
                case TargetWorkflowType.BOTTOM_UP:
                    clusterer = new SequenceTargetClusterer();
                    break;
                case TargetWorkflowType.SPECTRAL:
                    clusterer = new SpectralTargetClusterer();
                    break;
            }

            return clusterer;
        }
    }
}
