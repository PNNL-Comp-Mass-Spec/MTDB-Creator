namespace MTDBFramework.Algorithms.RetentionTimePrediction
{
    public class RetentionTimePredictorFactory
    {
        public static IRetentionTimePredictor CreatePredictor(RetentionTimePredictionType type)
        {
            IRetentionTimePredictor predictor = null;
            switch (type)
            {
                case RetentionTimePredictionType.Krokhin:
                    predictor = new KrokhinPredictor();
                    break;
                case RetentionTimePredictionType.Kangas:
                    predictor = new KangasPredictor();
                    break;
                default:
                    break;
            }

            return predictor;
        }
    }
}
