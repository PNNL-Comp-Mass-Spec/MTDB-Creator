#region Namespaces

using System.Collections.Generic;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using MTDBFramework.Data;
using Regressor.Algorithms;

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
            this.Options = options;
        }

        public LinearRegressionResult AlignTargets(List<Evidence> evidences, List<Evidence> baseline)
        {
            List<double> observed = new List<double>();
            List<double> predicted = new List<double>();

            foreach (Evidence evidence in baseline)
            {
                Predictor = RetentionTimePredictorFactory.CreatePredictor(Options.PredictorType);
                evidence.PredictedNet = Predictor.GetElutionTime(Evidence.CleanSequence(evidence.Sequence));

                observed.Add(evidence.ObservedNet);
                predicted.Add(evidence.PredictedNet);
            }

            // TODO:
            if (Options.RegressionType == RegressionType.LinearEm)
            {
                Regressor = new LinearModelEm();
            }
            else if (Options.RegressionType == RegressionType.MixtureRegression)
            {
                Regressor = new MixtureModelEM();
            }

            LinearRegressionResult result = Regressor.CalculateRegression(observed, predicted);

            foreach (Evidence evidence in evidences)
            {
                evidence.ObservedNet = Regressor.Transform(result, evidence.ObservedNet);
            }

            return result;
        }
    }
}
