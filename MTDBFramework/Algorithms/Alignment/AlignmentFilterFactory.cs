#region Namespaces

using MTDBFramework.Data;
using MTDBFrameworkBase.Data;

#endregion

namespace MTDBFramework.Algorithms.Alignment
{
    /// <summary>
    /// Tools for establishing alignment filters
    /// </summary>
    public class AlignmentFilterFactory
    {
        /// <summary>
        /// Create an alignment filter
        /// </summary>
        /// <param name="tool">Format of input data</param>
        /// <param name="options">Options</param>
        /// <returns>Alignment filter</returns>
        public static ITargetFilter Create(LcmsIdentificationTool tool, Options options)
        {
            ITargetFilter alignmentFilter = null;

            switch (tool)
            {
                case LcmsIdentificationTool.MsgfPlus:
                    alignmentFilter = new MsgfPlusAlignmentFilter(options);
                    break;
                case LcmsIdentificationTool.MZIdentML:
                    alignmentFilter = new MsgfPlusAlignmentFilter(options);
                    break;
                case LcmsIdentificationTool.Sequest:
                    alignmentFilter = new SequestAlignmentFilter(options);
                    break;
                case LcmsIdentificationTool.XTandem:
                    alignmentFilter = new XTandemAlignmentFilter(options);
                    break;
                case LcmsIdentificationTool.MSAlign:
                    alignmentFilter = new MsAlignAlignmentFilter(options);
                    break;
            }

            return alignmentFilter;
        }
    }
}
