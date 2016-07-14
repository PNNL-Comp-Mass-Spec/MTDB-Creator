namespace MTDBFramework.Algorithms.RetentionTimePrediction
{
    /// <summary>
    /// Determination and configuration of a Retention Time Predictor
    /// </summary>
    public class RetentionTimePredictorFactory
    {
        /// <summary>
        /// Configure and return the appropriate Retention Time Predictor
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IRetentionTimePredictor CreatePredictor(RetentionTimePredictionType type)
        {
            IRetentionTimePredictor predictor = null;
            switch (type)
            {
                case RetentionTimePredictionType.KROKHIN:
                    predictor = new KrokhinPredictor();
                    break;
                case RetentionTimePredictionType.KANGAS:
                    predictor = new KangasPredictor();
                    break;
            }

            return predictor;
        }
    }
}
