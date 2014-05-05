using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBCreator.IO
{
    public class ResultsToSequenceMapReader: FileReaderBase<ResultToSequenceMap>
	{
        private static string mstrDefaultHeader ="Result_ID	Unique_Seq_ID" ;

        private static string mstrHeader_result_id      = "";
        private static string mstrHeader_unique_seq_id  = "";
        private static short mshortColNum_result_id     = 0;
        private static short mshortColNum_unique_seq_id = 1;

        public ResultsToSequenceMapReader()
        {
            SetHeaderNames();
        }

        protected override void SetHeaderColumns(string[] headers)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                if (headers[i] == mstrHeader_result_id)
                    mshortColNum_result_id = (short)i;
                else if (headers[i] == mstrHeader_unique_seq_id)
                    mshortColNum_unique_seq_id = (short)i;
            }
                        
        }

        public void SetHeaderNames()
        {
            string[] colNames        = mstrDefaultHeader.Split(new char[] { '\t' });
            mstrHeader_result_id     = colNames[mshortColNum_result_id];
            mstrHeader_unique_seq_id = colNames[mshortColNum_unique_seq_id];
        }

        protected override ResultToSequenceMap ProcessLine(string[] line)
        {
            ResultToSequenceMap map = new ResultToSequenceMap();
            map.ResultId            = Convert.ToInt32(line[mshortColNum_result_id]);
            map.UniqueSequenceId    = Convert.ToInt32(line[mshortColNum_unique_seq_id]);  
            return map;
        }	
	}
}
