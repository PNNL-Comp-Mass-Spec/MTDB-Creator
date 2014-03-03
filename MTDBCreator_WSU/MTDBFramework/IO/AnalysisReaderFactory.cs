#region Namespaces

using MTDBFramework.Data;

#endregion

namespace MTDBFramework.IO
{
    public static class AnalysisReaderFactory
    {
        public static IAnalysisReader Create(LcmsIdentificationTool tool, Options options)
        {
            IAnalysisReader reader = null;

            switch (tool)
            {
                case LcmsIdentificationTool.MsgfPlus:
                    //TODO: MsgfPlusTargetFilter
                    reader = new MsgfPlusAnalysisReader(options);
                    break;
                case LcmsIdentificationTool.Sequest:
                    reader = new SequestAnalysisReader(options);
                    break;
                case LcmsIdentificationTool.XTandem:
                    reader = new XTandemAnalysisReader(options);
                    break;
                case LcmsIdentificationTool.MSAlign:
                    reader = new MSAlignAnalysisReader(options);
                    break;
                case LcmsIdentificationTool.MzIdentMl:
                    //reader = new MZIdentReader(options);
                    break;
            }

            return reader;
        }
    }
}