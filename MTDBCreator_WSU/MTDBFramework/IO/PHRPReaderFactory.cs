using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using MTDBFramework.Data;
using MTDBFramework.Database;
using PHRPReader;

namespace MTDBFramework.IO
{
    public static class PHRPReaderFactory
    {
		// Entry point for creating an LCMS Dataset using the PHRP Reader. Takes the options from
		// the Analysis Job Processor's options and the file path for the job item and returns a reader
		// for the appropriate data file.
		// If the path ends with "msgfplus.mzid" then it's an MZIdentML file, but if it's not, then
		// it determines the type of reader to return based on the extension of the file, using that to 
		// determine if it is a Sequest, XTandem, MSAlign or MSGF+ file
		// The if statement is due to the PHRP Reader not meaning to be used on .mzid files
        public static IPHRPReader Create(string path, Options options)
        {
            IPHRPReader reader = null;
            var ResultType = clsPHRPReader.AutoDetermineResultType(path);

            if (path.EndsWith("msgfplus.mzid"))
			{
			    reader = new MZIdentMLReader(options);
				return reader;
			}

            switch (ResultType)
            {
                case clsPHRPReader.ePeptideHitResultType.XTandem:
                    reader = new XTandemPHRPReader(options);
                    break;

                case clsPHRPReader.ePeptideHitResultType.Sequest:
                    reader = new SequestPHRPReader(options);
                    break;
                    
                case clsPHRPReader.ePeptideHitResultType.MSGFDB:
                    reader = new MSGFPlusPHRPReader(options);
                    break;
                    
                case clsPHRPReader.ePeptideHitResultType.MSAlign:
                    reader = new MSAlignPHRPReader(options);
                    break;
					
					
            }
            return reader;
        }
    }

}
