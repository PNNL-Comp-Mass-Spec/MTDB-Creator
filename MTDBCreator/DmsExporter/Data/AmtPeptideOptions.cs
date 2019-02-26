namespace MTDBCreator.DmsExporter.Data
{
    public class AmtPeptideOptions
    {
        public decimal PmtQualityScore { get; set; }

        public int MtCountPassing { get; set; }

        public int FilterSetId { get; set; }

        public string FilterSetName { get; set; }

        public string FilterSetDescription { get; set; }

        public override string ToString()
        {
            return string.Format("PMT QS >= {0:F1}, MTCountPassing >= {1}, FilterSetID {2}: {3}",
                                 PmtQualityScore, MtCountPassing, FilterSetId, FilterSetName ?? string.Empty);
        }
    }
}
