namespace MTDBFramework.Data
{
    public class SequenceToProteinMap
    {
        public int UniqueSequenceId { get; set; }
        public short CleavageState { get; set; }
        public short TerminusState { get; set; }
        public string ProteinName { get; set; }
        public double ProteinEValue { get; set; }
        public double ProteinIntensityLog { get; set; }
    }
}