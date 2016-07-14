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
            Id(x => x.Id).Column("OptionsId").GeneratedBy.Native();

            // Regression
            Map(x => x.RegressionType);
            Map(x => x.RegressionOrder);

            // NET Prediction
            Map(x => x.PredictorType).Column("NetPredictionType");

            // Peptides
            Map(x => x.MsgfFilter).Column("MsgfFilterParameter");
            Map(x => x.MaxMsgfSpecProb);
            Map(x => x.MsgfQValue).Column("MaxMsgfQValue");
        }
    }
}
