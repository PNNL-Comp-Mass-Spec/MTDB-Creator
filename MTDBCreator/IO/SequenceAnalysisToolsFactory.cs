using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MTDBCreator.Data;

namespace MTDBCreator.IO
{
    public class SequenceAnalysisToolsFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IAnalysisReader CreateReader(SupportedTools tool)
        {            
            IAnalysisReader reader  = null;
            switch (tool)
            {
                case SupportedTools.Sequest:
                    reader = new SequestAnalysisReader();
                    break;
                case SupportedTools.XTandem:
                    reader = new XTandemAnalysisReader();
                    break;
                case SupportedTools.MsgfPlus:
                    reader = new MsgfPlusAnalysisReader();
                    break;
                case SupportedTools.NotSupported:
                    break;
                default:
                    break;
            }
            return reader;
        }
    }
}
