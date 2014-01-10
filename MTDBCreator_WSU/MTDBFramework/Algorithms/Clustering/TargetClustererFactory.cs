namespace MTDBFramework.Algorithms.Clustering
{
    public static class TargetClustererFactory
    {
        public static ITargetClusterer Create(TargetWorkflowType workflowType)
        {
            ITargetClusterer clusterer = null;

            switch (workflowType)
            {
                case TargetWorkflowType.TopDown:
                    clusterer = new SequenceTargetClusterer();
                    break;
                case TargetWorkflowType.BottomUp:
                    clusterer = new SequenceTargetClusterer();
                    break;
                case TargetWorkflowType.Spectral:
                    clusterer = new SpectralTargetClusterer();
                    break;
            }

            return clusterer;
        }
    }
}
