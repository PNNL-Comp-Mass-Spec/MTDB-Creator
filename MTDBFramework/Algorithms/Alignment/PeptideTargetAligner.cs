#region Namespaces

using System.Collections.Generic;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using MTDBFramework.Data;
using PNNLOmics.Algorithms.Regression;

#endregion

namespace MTDBFramework.Algorithms.Alignment
{
	/// <summary>
	/// Target alignment for peptides
	/// </summary>
    public class PeptideTargetAligner : ITargetAligner
    {
        #region Public Properties

		/// <summary>
		/// Options
		/// </summary>
        public Options Options { get; set; }

		/// <summary>
		/// Linear Regression Algorithm Data
		/// </summary>
        public IRegressorAlgorithm<LinearRegressionResult> Regressor { get; set; }

		/// <summary>
		/// RetentionTimePredictor
		/// </summary>
        public IRetentionTimePredictor Predictor { get; set; }

        #endregion

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="options"></param>
        public PeptideTargetAligner(Options options)
        {
            Options = options;
        }

		/// <summary>
		/// Perform target alignment
		/// </summary>
		/// <param name="evidences">Evidences to align</param>
		/// <param name="baseline">Baseline evidences</param>
		/// <returns></returns>
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
