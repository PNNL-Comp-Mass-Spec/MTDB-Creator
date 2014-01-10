using MTDBFramework.Data;

namespace MTDBFramework.Algorithms.Alignment
{
    public static class TargetAlignmentFactory
    {
        public static ITargetAligner Create(Options options)
        {
            ITargetAligner aligner = null;

            switch (options.TargetFilterType)
            {
                case TargetWorkflowType.TopDown:
                    aligner = new ProteinTargetAligner();
                    break;
                case TargetWorkflowType.BottomUp:
                    aligner = new PeptideTargetAligner(options);
                    break;
                case TargetWorkflowType.Spectral:
                    aligner = new SpectralTargetAligner();
                    break;
            }

            return aligner;
        }
    }
}
