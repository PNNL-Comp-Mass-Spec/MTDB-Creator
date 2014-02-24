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
        public static IPHRPReader Create(string path, Options options)
        {
            IPHRPReader reader = null;
            var ResultType = clsPHRPReader.AutoDetermineResultType(path);

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
