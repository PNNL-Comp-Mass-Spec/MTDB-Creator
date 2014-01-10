using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBFramework.Algorithms.RetentionTimePrediction
{
    public static class RetentionTimePedictionFactory
    {
        public static IRetentionTimePredictor Create(PredictionType type)
        {
            switch (type)
            {
                case PredictionType.Kangas: 
                    return new KangasPredictor();
                case PredictionType.Krohkin:
                    return new KrokhinPredictor();
                default:
                    return new KangasPredictor();
            }
        }

    }
}
