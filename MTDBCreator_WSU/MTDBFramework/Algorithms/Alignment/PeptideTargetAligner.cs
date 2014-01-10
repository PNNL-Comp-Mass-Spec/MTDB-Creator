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

        public LinearRegressionResult AlignTargets(List<Target> targets, List<Target> baseline)
        {
            List<double> observed = new List<double>();
            List<double> predicted = new List<double>();

            foreach (Target target in baseline)
            {
                Predictor = RetentionTimePredictorFactory.CreatePredictor(Options.PredictorType);
                target.PredictedNet = Predictor.GetElutionTime(Target.CleanSequence(target.Sequence));

                observed.Add(target.ObservedNet);
                predicted.Add(target.PredictedNet);
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

            foreach (Target target in targets)
            {
                target.ObservedNet = Regressor.Transform(result, target.ObservedNet);
            }

            return result;
        }
    }
}
