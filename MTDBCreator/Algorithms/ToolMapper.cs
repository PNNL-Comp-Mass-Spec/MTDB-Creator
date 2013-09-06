using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBCreator.Data;

namespace MTDBCreator.Algorithms
{
    public class ToolMapper
    {
        /// <summary>
        /// Determines if the tool is supported based on its name
        /// </summary>
        /// <param name="datasetName"></param>
        /// <returns></returns>
        public static SupportedTools ToolName(string datasetName)
        {
            string name = datasetName.Trim().ToLower();

            if (name == "sequest")
                return SupportedTools.Sequest;
            else if (name == "xtandem")
                return SupportedTools.XTandem;
            else return SupportedTools.NotSupported;
        }
    }
}
