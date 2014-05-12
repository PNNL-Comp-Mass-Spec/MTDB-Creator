#region Namespaces

using System.Collections.Generic;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using MTDBFramework.Data;
using PNNLOmics.Algorithms.Regression;

#endregion

namespace MTDBFramework.Algorithms.Alignment
{
    public class PeptideTargetAligner : ITargetAligner
    {
        #region Public Properties

        public Options Options { get; set; }
        public IRegressorAlgorithm<LinearRegressionResult> Regressor { get; set; }
        public IRetentionTimePredictor Predictor { get; set; }

        #endregion

        public PeptideTargetAligner(Options options)
        {
            Options = options;
        }

        public LinearRegressionResult AlignTargets(List<Evidence> evidences, List<Evidence> baseline)
        {
            var observed = new List<double>();
            var predicted = new List<double>();

            foreach (var evidence in baseline)
            {
                Predictor = RetentionTimePredictorFactory.CreatePredictor(Options.PredictorType);
                evidence.PredictedNet = Predictor.GetElutionTime(Evidence.CleanSequence(evidence.Sequence));

                observed.Add(evidence.ObservedNet);
                predicted.Add(evidence.PredictedNet);
            }

            Regressor = LinearRegressorFactory.Create(Options.RegressionType);            
            var result = Regressor.CalculateRegression(observed, predicted);

            foreach (var evidence in evidences)
            {
                evidence.ObservedNet = Regressor.Transform(result, evidence.ObservedNet);
            }

            return result;
        }
    }
}
