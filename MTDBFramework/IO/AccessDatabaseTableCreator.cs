using System.Collections.Generic;

namespace MTDBFramework.IO
{
    class AccessDatabaseTableCreator
    {
        public static List<ADOX.Table> CreateTables()
        {
            var tableList = new List<ADOX.Table>
            {
                CreateConsensusTargetTable(),
                CreateConsensusProteinPairTable(),
                CreateEvidenceTable(),
                CreateDatasetTable(),
                CreateOptionsTable(),
                CreateProteinTable()
            };

            return tableList;
        }

        private static ADOX.Table CreateConsensusTargetTable()
        {
            var consensusTable = new ADOX.Table {Name = "ConsensusTargets"};

            consensusTable.Columns.Append("ConsensusId", ADOX.DataTypeEnum.adInteger);
            consensusTable.Columns.Append("Net", ADOX.DataTypeEnum.adDouble);
            consensusTable.Columns.Append("StdevNet", ADOX.DataTypeEnum.adDouble);
            consensusTable.Columns.Append("PredictedNet", ADOX.DataTypeEnum.adDouble);
            consensusTable.Columns.Append("TheoreticalMonoIsotopicMass", ADOX.DataTypeEnum.adDouble);
            consensusTable.Columns.Append("PrefixResidue");
            consensusTable.Columns.Append("SuffixResidue");
            consensusTable.Columns.Append("Sequence");
            consensusTable.Columns.Append("ModificationCount", ADOX.DataTypeEnum.adInteger);
            consensusTable.Columns.Append("ModificationDescription");
            consensusTable.Columns.Append("MultiProteinCount", ADOX.DataTypeEnum.adInteger);

            return consensusTable;
        }

        private static ADOX.Table CreateConsensusProteinPairTable()
        {
            var conProtPairTable = new ADOX.Table {Name = "ConsensusProteinPair"};

            conProtPairTable.Columns.Append("PairId", ADOX.DataTypeEnum.adInteger);
            conProtPairTable.Columns.Append("ConsensusId", ADOX.DataTypeEnum.adInteger);
            conProtPairTable.Columns.Append("ProteinId", ADOX.DataTypeEnum.adInteger);
            conProtPairTable.Columns.Append("CleavageState", ADOX.DataTypeEnum.adInteger);
            conProtPairTable.Columns.Append("TerminusState", ADOX.DataTypeEnum.adInteger);
            conProtPairTable.Columns.Append("ResidueStart", ADOX.DataTypeEnum.adInteger);
            conProtPairTable.Columns.Append("ResidueEnd", ADOX.DataTypeEnum.adInteger);

            return conProtPairTable;
        }

        private static ADOX.Table CreateEvidenceTable()
        {
            var evidenceTable = new ADOX.Table {Name = "Evidence"};

            evidenceTable.Columns.Append("EvidenceId", ADOX.DataTypeEnum.adInteger);
            evidenceTable.Columns.Append("Charge", ADOX.DataTypeEnum.adInteger);
            evidenceTable.Columns.Append("ObservedNet", ADOX.DataTypeEnum.adDouble);
            evidenceTable.Columns.Append("PredictedNet", ADOX.DataTypeEnum.adDouble);
            evidenceTable.Columns.Append("Mz", ADOX.DataTypeEnum.adDouble);
            evidenceTable.Columns.Append("Scan", ADOX.DataTypeEnum.adInteger);
            evidenceTable.Columns.Append("DelM", ADOX.DataTypeEnum.adDouble);
            evidenceTable.Columns.Append("DelMPpm", ADOX.DataTypeEnum.adDouble);
            evidenceTable.Columns.Append("SpecProb", ADOX.DataTypeEnum.adDouble);
            evidenceTable.Columns.Append("DatasetId", ADOX.DataTypeEnum.adInteger);
            evidenceTable.Columns.Append("ConsensusId", ADOX.DataTypeEnum.adInteger);

            return evidenceTable;
        }

        private static ADOX.Table CreateDatasetTable()
        {
            var datasetTable = new ADOX.Table {Name = "TargetDataSet"};

            datasetTable.Columns.Append("DatasetId", ADOX.DataTypeEnum.adInteger);
            datasetTable.Columns.Append("Name");
            datasetTable.Columns.Append("SearchTool");

            return datasetTable;
        }

        private static ADOX.Table CreateOptionsTable()
        {
            var optionsTable = new ADOX.Table {Name = "Options"};

            optionsTable.Columns.Append("OptionsId", ADOX.DataTypeEnum.adInteger);
            optionsTable.Columns.Append("RegressionType");
            optionsTable.Columns.Append("RegressionOrder", ADOX.DataTypeEnum.adInteger);
            optionsTable.Columns.Append("TargetFilterType");
            optionsTable.Columns.Append("PredictorType");
            optionsTable.Columns.Append("MaxModsForAlignment", ADOX.DataTypeEnum.adInteger);
            optionsTable.Columns.Append("MinObservationsForExport", ADOX.DataTypeEnum.adInteger);
            optionsTable.Columns.Append("ExportTryptic");
            optionsTable.Columns.Append("ExportPartiallyTryptic");
            optionsTable.Columns.Append("ExportNonTryptic");
            optionsTable.Columns.Append("MinXCorrForExportTryptic");
            optionsTable.Columns.Append("MinXCorrForExportPartiallyTryptic");
            optionsTable.Columns.Append("MinXCorrForExportNonTryptic");
            optionsTable.Columns.Append("MinXCorrForAlignment", ADOX.DataTypeEnum.adDouble);
            optionsTable.Columns.Append("UseDelCn");
            optionsTable.Columns.Append("MaxDelCn", ADOX.DataTypeEnum.adDouble);
            optionsTable.Columns.Append("MaxLogEValForXTandemAlignment", ADOX.DataTypeEnum.adDouble);
            optionsTable.Columns.Append("MaxLogEValForXTandemExport", ADOX.DataTypeEnum.adDouble);
            optionsTable.Columns.Append("MaxRankForExport", ADOX.DataTypeEnum.adInteger);

            return optionsTable;
        }

        private static ADOX.Table CreateProteinTable()
        {
            var proteinTable = new ADOX.Table {Name = "ProteinInformation"};

            proteinTable.Columns.Append("ProteinId", ADOX.DataTypeEnum.adInteger);
            proteinTable.Columns.Append("ProteinName");

            return proteinTable;
        }
    }
}
