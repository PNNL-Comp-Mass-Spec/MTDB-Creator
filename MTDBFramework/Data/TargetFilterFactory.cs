namespace MTDBFramework.Data
{
    public static class TargetFilterFactory
    {
        public static ITargetFilter Create(LcmsIdentificationTool tool, Options options)
        {
            ITargetFilter targetFilter = null;

            switch (tool)
            {
                case LcmsIdentificationTool.MsgfPlus:
                    targetFilter = new MsgfPlusTargetFilter(options);
                    break;
                case LcmsIdentificationTool.MZIdentML:
                    targetFilter = new MsgfPlusTargetFilter(options);
                    break;
                case LcmsIdentificationTool.Sequest:
                    targetFilter = new SequestTargetFilter(options);
                    break;
                case LcmsIdentificationTool.XTandem:
                    targetFilter = new XTandemTargetFilter(options);
                    break;
                case LcmsIdentificationTool.MSAlign:
                    targetFilter = new MsAlignTargetFilter(options);
                    break;
            }

            return targetFilter;
        }
    }
}