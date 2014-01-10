using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBFramework.Algorithms;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using Regressor.Algorithms;

namespace MTDBFramework.Database
{
    public class SaveableOptions
    {
        public int Id;
        // General
        public RegressionType RegressionType { get; set; }
        public TargetWorkflowType TargetFileterType { get; set; }

        public IRetentionTimePredictor Predictor { get; set; }
        public RetentionTimePredictionType PredictorType { get; set; }

        // Peptides
        public int MaxModsForAlignment { get; set; }
        public short MinObservationsForExport { get; set; }

        public bool ExportTryptic { get; set; }
        public bool ExportPartiallyTryptic { get; set; }
        public bool ExportNonTryptic { get; set; }

        // TODO: These three are arrays of length 3. Try to do error detection
        public IList<double> MinXCorrForExportTryptic { get; set; }
        public double[] MinXCorrForExportPartiallyTryptic { get; set; }
        public double[] MinXCorrForExportNonTryptic { get; set; }

        // Sequest
        public double MinXCorrForAlignment { get; set; }
        public bool UseDelCN { get; set; }
        public double MaxDelCN { get; set; }

        // Xtandem
        public double MaxLogEValForXTandemAlignment { get; set; }
        public double MaxLogEValForXTandemExport { get; set; }

        // Other
        public short MaxRankForExport { get; set; }

    }
}
