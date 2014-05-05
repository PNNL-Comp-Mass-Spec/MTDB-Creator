namespace MTDBFramework.IO
{
    public abstract class TableDataReaderBase<T>
    {
        protected readonly string[] Delimiters = { "\t" };

        /// <summary>
        /// Set header indices according to the actual header sequence from the analysis file
        /// </summary>
        /// <param name="actualHeader">The header line from the analysis file</param>
        protected abstract void SetHeaderIndices(string actualHeader);

        /// <summary>
        /// Process one data line from the analysis file
        /// </summary>
        /// <param name="line">A data line from the analysis file</param>
        protected abstract T ProcessLine(string line);
    }
}
