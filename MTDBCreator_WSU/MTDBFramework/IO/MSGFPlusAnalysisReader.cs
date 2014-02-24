using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using System.IO;


namespace MTDBFramework.IO
{
    /// <summary>
    /// Summary description for clsAnalysisReader.
    /// </summary>
    public class MsgfPlusAnalysisReader : TableDataReaderBase<MSGFPlusResult>, IAnalysisReader
    {
        public Options ReaderOptions { get; set; }

        public MsgfPlusAnalysisReader(Options options)
        {
            this.ReaderOptions = options;
        }

        public LcmsDataSet Read(string path)
        {
            List<MSGFPlusResult> results = new List<MSGFPlusResult>();
            MSGFPlusTargetFilter filter = new MSGFPlusTargetFilter(this.ReaderOptions);

            // Get Result to Sequence Map

            ResultToSequenceMapReader resultToSequenceMapReader = new ResultToSequenceMapReader();
            Dictionary<int, int> resultToSequenceDictionary = new Dictionary<int,int>();

            foreach (ResultToSequenceMap map in resultToSequenceMapReader.Read(path.Insert(path.LastIndexOf(".txt"), "_ResultToSeqMap")))
            {
                resultToSequenceDictionary.Add(map.ResultId, map.UniqueSequenceId);
            }

            // Get Sequence Info

            SequenceInfoReader sequenceInfoReader = new SequenceInfoReader();
            Dictionary<int, SequenceInfo> sequenceInfoDictionary = new Dictionary<int,SequenceInfo>();

            foreach (SequenceInfo info in sequenceInfoReader.Read(path.Insert(path.LastIndexOf(".txt"), "_SeqInfo")))
            {
                sequenceInfoDictionary.Add(info.Id, info);
            }

            // Get Targets

            using (StreamReader reader = new StreamReader(path))
            {
                this.SetHeaderIndices(reader.ReadLine());

                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    MSGFPlusResult result = this.ProcessLine(line);

                    if(!filter.ShouldFilter(result))
                    {
                        result.DataSet = new TargetDataSet() { Path = path };

                        if (resultToSequenceDictionary.ContainsKey(result.AnalysisId))
                        {
                            result.IsSeqInfoExist = 1;
                            result.ModificationCount = sequenceInfoDictionary[resultToSequenceDictionary[result.AnalysisId]].ModificationCount;
                            result.ModificationDescription = sequenceInfoDictionary[resultToSequenceDictionary[result.AnalysisId]].ModificationDescription;
                            result.SeqInfoMonoisotopicMass = sequenceInfoDictionary[resultToSequenceDictionary[result.AnalysisId]].MonoisotopicMass;
                        }

                        results.Add(result);
                    }
                }
            }

            AnalysisReaderHelper.CalculateObservedNet(results);
            AnalysisReaderHelper.CalculatePredictedNet(RetentionTimePredictorFactory.CreatePredictor(this.ReaderOptions.PredictorType), results);

            return new LcmsDataSet(Path.GetFileNameWithoutExtension(path), LcmsIdentificationTool.MsgfPlus, results);
        }

        protected override void SetHeaderIndices(string actualHeader)
        {
            DefaultHeaders header;

            string[] actualHeaders = actualHeader.Split(this.Delimiters, StringSplitOptions.None);

            for (int i = 0; i < actualHeaders.Length; i++)
            {
                bool result = Enum.TryParse(actualHeaders[i], true, out header);

                if (result)
                {
                    actualHeaderMaps.Add(header, i);
                }
            }
        }

        protected override MSGFPlusResult ProcessLine(string line)
        {
            string[] lineCells = line.Split(this.Delimiters, StringSplitOptions.None);

            MSGFPlusResult result = new MSGFPlusResult();

            // Fields in Target

            result.AnalysisId = Convert.ToInt32(lineCells[actualHeaderMaps[DefaultHeaders.ResultID]]);//column name changed
            result.Scan = Convert.ToInt32(lineCells[actualHeaderMaps[DefaultHeaders.Scan]]);//Column name changed
            result.Charge = Convert.ToInt16(lineCells[actualHeaderMaps[DefaultHeaders.Charge]]);//Column name changed
            result.MonoisotopicMass = Convert.ToDouble(lineCells[actualHeaderMaps[DefaultHeaders.MH]]);// Unchanged
            result.MultiProteinCount = Convert.ToInt16(lineCells[actualHeaderMaps[DefaultHeaders.IsotopeError]]);//IsotopeError? Need confirm
            result.Sequence = lineCells[actualHeaderMaps[DefaultHeaders.Peptide]];//Unchanged
            result.CleanPeptide = Target.CleanSequence(result.Sequence);//Unchanged
            result.Mz = result.MonoisotopicMass / result.Charge; //Unchanged
            result.PeptideInfo = new TargetPeptideInfo()
            {
                Peptide/*InfoSequence*/ = result.Sequence,
                /*PeptideInfo*/CleanPeptide = result.Sequence
            };

            //Fields in MsgfPlusResult

            result.Reference = lineCells[actualHeaderMaps[DefaultHeaders.Protein]]; //Need confirm?
            result.NumTrypticEnds = Convert.ToInt16(lineCells[actualHeaderMaps[DefaultHeaders.NTT]]);//Need confirm
            result.Fdr = Convert.ToDouble(lineCells[actualHeaderMaps[DefaultHeaders.QValue]]); //not sure, might be calculated?

            return result;
        }

        private readonly Dictionary<DefaultHeaders, int> actualHeaderMaps = new Dictionary<DefaultHeaders, int>();

        private enum DefaultHeaders
        {
            ResultID,// used
            Scan,// used
            FragMethod,// NOT USED
            SpecIndex,// NOT USED
            
            Charge,// used
            PrecursorMZ,// NOT USED
            DelM,// NOT USED
            DelM_PPM,// NOT USED
            MH, // used
            Peptide,// used
            Protein,//used        name of protein, reference?
            NTT,// Used        NumTrypticEnds?
            DeNovoScore,//
            MSGFScore,//
            MSGFDB_SpecEValue,//Goes in the specEValue of the result
            Rank_MSGFDB_SpecEValue,//
            Evalue,//
            QValue,// used as fdr
            PepQValue,//
            IsotopeError,//
            
        };

        //    mstrSeqToProteinMapExt = "msgfdb_syn_SeqToProteinMap.txt";
        //    mstrResultsExt = "msgfdb_syn.txt";
        //    mstrResultToSeqMapExt = "msgfdb_syn_ResultToSeqMap.txt";
        //    mstrSeqInfoExt = "msgfdb_syn_SeqInfo.txt";

        //    m_analysisMap.Add(mstrSeqToProteinMapExt, "");
        //    m_analysisMap.Add(mstrResultsExt, "");
        //    m_analysisMap.Add(mstrResultToSeqMapExt, "");
        //    m_analysisMap.Add(mstrSeqInfoExt, "");

        //}

        //protected override List<Target> ReadTargets(string path)
        //{
        //    /* Sequest File ----------------------------------------------------------------*/
        //    MsgfPlusResultsReader reader = new MsgfPlusResultsReader();
        //    List<MsgfPlusResult> results = reader.Read(path);
        //    List<Target> targets = new List<Target>();
        //    results.ForEach(x => targets.Add(x));
        //    return targets;
        //}
    }
}
