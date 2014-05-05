namespace MTDBFramework.Algorithms.Clustering
{
    public static class TargetClustererFactory
    {
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
