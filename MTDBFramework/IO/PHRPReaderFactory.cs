using MTDBFramework.Data;
using PHRPReader;

namespace MTDBFramework.IO
{
    public static class PhrpReaderFactory
    {
		// Entry point for creating an LCMS Dataset using the PHRP Reader. Takes the options from
		// the Analysis Job Processor's options and the file path for the job item and returns a reader
		// for the appropriate data file.
		// If the path ends with "msgfplus.mzid" then it's an MZIdentML file, but if it's not, then
		// it determines the type of reader to return based on the extension of the file, using that to 
		// determine if it is a Sequest, XTandem, MSAlign or MSGF+ file
		// The if statement is due to the PHRP Reader not meaning to be used on .mzid files
        public static PHRPReaderBase Create(string path, Options options)
        {
            PHRPReaderBase reader = null;
            var resultType = clsPHRPReader.AutoDetermineResultType(path);

            if (path.EndsWith("msgfplus.mzid"))
			{
			    reader = new MzIdentMlReader(options);
				return reader;
			}

            switch (resultType)
            {
                case clsPHRPReader.ePeptideHitResultType.XTandem:
                    reader = new XTandemPhrpReader(options);
                    break;

                case clsPHRPReader.ePeptideHitResultType.Sequest:
                    reader = new SequestPhrpReader(options);
                    break;
                    
                case clsPHRPReader.ePeptideHitResultType.MSGFDB:
                    reader = new MsgfPlusPhrpReader(options);
                    break;
                    
                case clsPHRPReader.ePeptideHitResultType.MSAlign:
                    reader = new MsAlignPhrpReader(options);
                    break;
					
					
            }
            return reader;
        }
    }

}
