using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBCreator.Data;

namespace MTDBCreator.Algorithms
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
