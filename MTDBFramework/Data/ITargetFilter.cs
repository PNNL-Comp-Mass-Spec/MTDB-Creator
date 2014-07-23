namespace MTDBFramework.Data
{
	/// <summary>
	/// Filter interface for alignment
	/// </summary>
    public interface ITargetFilter
    {
		/// <summary>
		/// Options
		/// </summary>
        Options FilterOptions { get; set; }

		/// <summary>
		/// Whether an evidence should be filtered
		/// </summary>
		/// <param name="evidence"></param>
		/// <returns></returns>
        bool ShouldFilter(Evidence evidence);
    }
}