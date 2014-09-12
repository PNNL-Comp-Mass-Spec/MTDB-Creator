using FluentNHibernate.Mapping;
using MTDBFramework.Data;

namespace MTDBFramework.Database
{
	/// <summary>
	/// NHibernate mapping for the Options class
	/// </summary>
    public class OptionsMap : ClassMap<Options>
    {
		/// <summary>
		/// Constructor
		/// </summary>
        public OptionsMap()
        {
            Not.LazyLoad();
            Id(x => x.Id).Column("OptionsId").GeneratedBy.Identity();

            // Regression
            Map(x => x.RegressionType);
            Map(x => x.RegressionOrder);

            // General
            //Map(x => x.TargetFilterType);

            //// NET Prediction
            Map(x => x.PredictorType).Column("NetPredictionType");

            //// Peptides
            // Map(x => x.MaxModsForAlignment); Should always be 0
            // Map(x => x.MinObservationsForExport); Never used
		    // Map(x => x.MinimumObservedNet); Should always be 0
		    // Map(x => x.MaximumObservedNet); Should always be 1

		    Map(x => x.MaxMsgfSpecProb);
		    Map(x => x.MsgfFdr).Column("MaxMsgfFDR");
        }
    }
}
