using MTDBFramework.Data;

namespace MTDBFramework.Algorithms.Alignment
{
	/// <summary>
	/// Target alignment configuration
	/// </summary>
    public static class TargetAlignmentFactory
    {
		/// <summary>
		/// Configure and return the appropriate target aligner
		/// </summary>
		/// <param name="options"></param>
		/// <returns></returns>
        public static ITargetAligner Create(Options options)
        {
            ITargetAligner aligner = null;

            switch (options.TargetFilterType)
            {
                case TargetWorkflowType.TOP_DOWN:
                    aligner = new ProteinTargetAligner();
                    break;
                case TargetWorkflowType.BOTTOM_UP:
                    aligner = new PeptideTargetAligner(options);
                    break;
                case TargetWorkflowType.SPECTRAL:
                    aligner = new SpectralTargetAligner();
                    break;
            }

            return aligner;
        }
    }
}
