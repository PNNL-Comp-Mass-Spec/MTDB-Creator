using System;
using MTDBFramework.Data;
using MTDBFrameworkBase.Data;
using PHRPReader;

namespace MTDBFramework.IO
{
    /// <summary>
    /// Wrapper for reading input files, specifically PHRP-generated files
    /// </summary>
    public static class PhrpReaderFactory
    {
        /// <summary>
        /// Entry point for creating an LCMS Dataset using the PHRP Reader.
        /// </summary>
        /// <remarks>
        /// If the path ends with "msgfplus.mzid" or "mzid.gz" then it's an MZIdentML file, but if it's not, then
        /// it determines the type of reader to return based on the extension of the file, using that to
        /// determine if it is a Sequest, XTandem, MSAlign or MSGF+ file
        /// The if statement is due to the PHRP Reader not meaning to be used on .mzid files
        /// </remarks>
        /// <param name="path">File to be read</param>
        /// <param name="options">Reader options</param>
        /// <returns>An appropriate reader for the file</returns>
        public static PHRPReaderBase Create(string path, Options options)
        {
            PHRPReaderBase reader;
            var resultType = clsPHRPReader.AutoDetermineResultType(path);

            if (path.EndsWith("mzid", StringComparison.OrdinalIgnoreCase) ||
                path.EndsWith("mzid.gz", StringComparison.OrdinalIgnoreCase))
            {
                reader = new MzIdentMlReader(options);
                return reader;
            }

            switch (resultType)
            {
                case clsPHRPReader.PeptideHitResultTypes.XTandem:
                    reader = new XTandemPhrpReader(options);
                    break;

                case clsPHRPReader.PeptideHitResultTypes.Sequest:
                    reader = new SequestPhrpReader(options);
                    break;

                case clsPHRPReader.PeptideHitResultTypes.MSGFPlus:
                    reader = new MsgfPlusPhrpReader(options);
                    break;

                case clsPHRPReader.PeptideHitResultTypes.MSAlign:
                    reader = new MsAlignPhrpReader(options);
                    break;
                default:
                    //Unsupported format!! Oh no!!
                    reader = null;
                    break;

            }
            return reader;
        }
    }

}
