namespace MTDBFramework.Data
{
	/// <summary>
	/// Target filter determination and configuration
	/// </summary>
    public static class TargetFilterFactory
    {
		/// <summary>
		/// Configure and return the appropriate target filter
		/// </summary>
		/// <param name="tool"></param>
		/// <param name="options"></param>
		/// <returns></returns>
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