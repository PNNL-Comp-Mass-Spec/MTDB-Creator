namespace MTDBFramework.Algorithms.RetentionTimePrediction
{
    public class RetentionTimePredictorFactory
    {
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
