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
                    alignmentFilter = new MSGFPlusAlignmentFilter(options);
                    break;
                case LcmsIdentificationTool.MzIdentMl:
                    //TODO: MsgfPlusTargetFilter
                    alignmentFilter = new MSGFPlusAlignmentFilter(options);
                    break;
                case LcmsIdentificationTool.Sequest:
                    alignmentFilter = new SequestAlignmentFilter(options);
                    break;
                case LcmsIdentificationTool.XTandem:
                    alignmentFilter = new XTandemAlignmentFilter(options);
                    break;
                case LcmsIdentificationTool.MSAlign:
                    alignmentFilter = new MSAlignAlignmentFilter(options);
                    break;
            }

            return alignmentFilter;
        }
    }
}
