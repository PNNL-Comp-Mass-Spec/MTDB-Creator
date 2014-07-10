using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBFramework.IO
{
    class AccessDatabaseTableCreator
    {
        public static List<ADOX.Table> CreateTables()
        {
            var tableList = new List<ADOX.Table>();

            tableList.Add(CreateConsensusTargetTable());
            tableList.Add(CreateConsensusProteinPairTable());
            tableList.Add(CreateEvidenceTable());
            tableList.Add(CreateDatasetTable());
            tableList.Add(CreateOptionsTable());
            tableList.Add(CreateProteinTable());

            return tableList;
        }

        private static ADOX.Table CreateConsensusTargetTable()
        {
            var consensusTable = new ADOX.Table();

            consensusTable.Name = "ConsensusTargets";
            consensusTable.Columns.Append("ConsensusId", ADOX.DataTypeEnum.adInteger);
            consensusTable.Columns.Append("Net", ADOX.DataTypeEnum.adDouble);
            consensusTable.Columns.Append("StdevNet", ADOX.DataTypeEnum.adDouble);
            consensusTable.Columns.Append("PredictedNet", ADOX.DataTypeEnum.adDouble);
            consensusTable.Columns.Append("TheoreticalMonoIsotopicMass", ADOX.DataTypeEnum.adDouble);
            consensusTable.Columns.Append("PrefixResidue", ADOX.DataTypeEnum.adVarWChar);
            consensusTable.Columns.Append("SuffixResidue", ADOX.DataTypeEnum.adVarWChar);
            consensusTable.Columns.Append("Sequence", ADOX.DataTypeEnum.adVarWChar);
            consensusTable.Columns.Append("ModificationCount", ADOX.DataTypeEnum.adInteger);
            consensusTable.Columns.Append("ModificationDescription", ADOX.DataTypeEnum.adVarWChar);
            consensusTable.Columns.Append("MultiProteinCount", ADOX.DataTypeEnum.adInteger);

            return consensusTable;
        }

        private static ADOX.Table CreateConsensusProteinPairTable()
        {
            var CPPTable = new ADOX.Table();

            CPPTable.Name = "ConsensusProteinPair";
            CPPTable.Columns.Append("PairId", ADOX.DataTypeEnum.adInteger);
            CPPTable.Columns.Append("ConsensusId", ADOX.DataTypeEnum.adInteger);
            CPPTable.Columns.Append("ProteinId", ADOX.DataTypeEnum.adInteger);
            CPPTable.Columns.Append("CleavageState", ADOX.DataTypeEnum.adInteger);
            CPPTable.Columns.Append("TerminusState", ADOX.DataTypeEnum.adInteger);
            CPPTable.Columns.Append("ResidueStart", ADOX.DataTypeEnum.adInteger);
            CPPTable.Columns.Append("ResidueEnd", ADOX.DataTypeEnum.adInteger);

            return CPPTable;
        }

        private static ADOX.Table CreateEvidenceTable()
        {
            var evidenceTable = new ADOX.Table();

            evidenceTable.Name = "Evidence";
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
            var datasetTable = new ADOX.Table();

            datasetTable.Name = "TargetDataSet";
            datasetTable.Columns.Append("DatasetId", ADOX.DataTypeEnum.adInteger);
            datasetTable.Columns.Append("Name", ADOX.DataTypeEnum.adVarWChar);
            datasetTable.Columns.Append("SearchTool", ADOX.DataTypeEnum.adVarWChar);

            return datasetTable;
        }

        private static ADOX.Table CreateOptionsTable()
        {
            var optionsTable = new ADOX.Table();

            optionsTable.Name = "Options";
            optionsTable.Columns.Append("OptionsId", ADOX.DataTypeEnum.adInteger);
            optionsTable.Columns.Append("RegressionType", ADOX.DataTypeEnum.adVarWChar);
            optionsTable.Columns.Append("RegressionOrder", ADOX.DataTypeEnum.adInteger);
            optionsTable.Columns.Append("TargetFilterType", ADOX.DataTypeEnum.adVarWChar);
            optionsTable.Columns.Append("PredictorType", ADOX.DataTypeEnum.adVarWChar);
            optionsTable.Columns.Append("MaxModsForAlignment", ADOX.DataTypeEnum.adInteger);
            optionsTable.Columns.Append("MinObservationsForExport", ADOX.DataTypeEnum.adInteger);
            optionsTable.Columns.Append("ExportTryptic", ADOX.DataTypeEnum.adVarWChar);
            optionsTable.Columns.Append("ExportPartiallyTryptic", ADOX.DataTypeEnum.adVarWChar);
            optionsTable.Columns.Append("ExportNonTryptic", ADOX.DataTypeEnum.adVarWChar);
            optionsTable.Columns.Append("MinXCorrForExportTryptic", ADOX.DataTypeEnum.adVarWChar);
            optionsTable.Columns.Append("MinXCorrForExportPartiallyTryptic", ADOX.DataTypeEnum.adVarWChar);
            optionsTable.Columns.Append("MinXCorrForExportNonTryptic", ADOX.DataTypeEnum.adVarWChar);
            optionsTable.Columns.Append("MinXCorrForAlignment", ADOX.DataTypeEnum.adDouble);
            optionsTable.Columns.Append("UseDelCn", ADOX.DataTypeEnum.adVarWChar);
            optionsTable.Columns.Append("MaxDelCn", ADOX.DataTypeEnum.adDouble);
            optionsTable.Columns.Append("MaxLogEValForXTandemAlignment", ADOX.DataTypeEnum.adDouble);
            optionsTable.Columns.Append("MaxLogEValForXTandemExport", ADOX.DataTypeEnum.adDouble);
            optionsTable.Columns.Append("MaxRankForExport", ADOX.DataTypeEnum.adInteger);

            return optionsTable;
        }

        private static ADOX.Table CreateProteinTable()
        {
            var proteinTable = new ADOX.Table();

            proteinTable.Name = "ProteinInformation";
            proteinTable.Columns.Append("ProteinId", ADOX.DataTypeEnum.adInteger);
            proteinTable.Columns.Append("ProteinName", ADOX.DataTypeEnum.adVarWChar);

            return proteinTable;
        }
    }
}
