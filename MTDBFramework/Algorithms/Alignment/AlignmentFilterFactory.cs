#region Namespaces

using MTDBFramework.Data;

#endregion

namespace MTDBFramework.Algorithms.Alignment
{
    public class AlignmentFilterFactory
    {
        public static ITargetFilter Create(LcmsIdentificationTool tool, Options options)
        {
            ITargetFilter alignmentFilter = null;

            switch (tool)
            {
                case LcmsIdentificationTool.MsgfPlus:
                    //TODO: MsgfPlusTargetFilter
                    alignmentFilter = new MsgfPlusAlignmentFilter(options);
                    break;
                case LcmsIdentificationTool.MZIdentML:
                    //TODO: MsgfPlusTargetFilter
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
