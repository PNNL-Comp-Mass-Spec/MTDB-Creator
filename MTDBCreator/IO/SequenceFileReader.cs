using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBCreator.IO
{
    public class SequenceFileReader : FileReaderBase<Sequence>
    {
        private const string mstrDefaultHeader              = "Unique_Seq_ID	Mod_Count	Mod_Description	Monoisotopic_Mass";
        private static string mstrHeader_unique_seq_id       = "";
        private static string mstrHeader_mod_count = "";
        private static string mstrHeader_mod_description = "";
        private static string mstrHeader_mono_mass = "";

        private short mshortColNum_unique_seq_id      = 0;
        private short mshortColNum_mod_count          = 1;
        private short mshortColNum_mod_description    = 2;
        private short mshortColNum_mono_mass          = 3;
        
        public SequenceFileReader()
        {
            SetHeaderNames();
        }

        protected override Sequence ProcessLine(string[] column)
        {            
            double mass         = Convert.ToDouble(column[mshortColNum_mono_mass]);
            short modCount      = Convert.ToInt16(column[mshortColNum_mod_count]);
            int seqId           = Convert.ToInt32(column[mshortColNum_unique_seq_id]);
            string description  = column[mshortColNum_mod_description];
            Sequence sequence   = new Sequence(seqId, modCount, description, mass);
            return sequence;
        }

        protected void SetHeaderNames()
        {
            string[] colNames           = mstrDefaultHeader.Split(new char[] { '\t' });
            mstrHeader_unique_seq_id    = colNames[mshortColNum_unique_seq_id];
            mstrHeader_mod_count        = colNames[mshortColNum_mod_count];
            mstrHeader_mod_description  = colNames[mshortColNum_mod_description];
            mstrHeader_mono_mass        = colNames[mshortColNum_mono_mass];
        }

        protected override void SetHeaderColumns(string [] headers)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                if (headers[i] == mstrHeader_unique_seq_id)
                    mshortColNum_unique_seq_id = (short)i;
                else if (headers[i] == mstrHeader_mod_count)
                    mshortColNum_mod_count = (short)i;
                else if (headers[i] == mstrHeader_mod_description)
                    mshortColNum_mod_description = (short)i;
                else if (headers[i] == mstrHeader_mono_mass)
                    mshortColNum_mono_mass = (short)i;
            }
        }
    }
}
