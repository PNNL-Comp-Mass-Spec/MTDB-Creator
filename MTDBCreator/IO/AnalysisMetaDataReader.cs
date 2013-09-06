using System;
using System.IO ; 
using System.Collections ;
using System.Collections.Generic;
using MTDBCreator.IO;
using MTDBCreator.Data;
using MTDBCreator.Algorithms; 

namespace MTDBCreator.IO
{    
	/// <summary>
	/// Summary description for clsAnalysisDescriptionReader.
	/// </summary>
	public class AnalysisMetaDataReader: IAnalysisMetaDataReader
	{
        /// <summary>
        /// Reads the analysis meta data
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<Analysis> ReadMetaData(string path)
        {
            List<Analysis> metaData = new List<Analysis>();
		    string [] lines  = File.ReadAllLines(path);            
            for(int i = 1; i < lines.Length; i++)
            {
                string line         = lines[i];
                Analysis data       = new Analysis();
                string [] lineData  = line.Split('\t');
                if (lineData.Length != 3)
                    continue;
                                    
                data.Tool       = ToolMapper.ToolName(lineData[0]);                               
                data.Name       = lineData[1];
                data.FilePath   = lineData[2]; 

                metaData.Add(data);
            }							
            return metaData;
		}
    }
}
