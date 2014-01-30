using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MTDBCreator.Data;

namespace MTDBCreator.Forms
{
    public partial class DatabaseControl : UserControl
    {
        private const int CONST_FLAME    = 0;
        private const int CONST_PROTEIN  = 1;
        private const int CONST_DATABASE = 2;
        private const int CONST_MASSTAG = 3;
        private const int CONST_MASS_TAG_RED = 4;
        private const int CONST_PROPERTY = 5;


        public DatabaseControl()
        {
            InitializeComponent();
        }

        public void UpdateDatabaseView(MassTagDatabase database)
        {
            mtree_proteins.Nodes.Clear();
            if (database == null)
            {
                return;
            }

            
            List<ConsensusTarget> targets = database.ConsensusTargets;
            TreeNode rootNode = new TreeNode(string.Format("Mass Tags ({0})", targets.Count));
            rootNode.ImageIndex = CONST_DATABASE;

            targets.Sort(delegate(ConsensusTarget p, ConsensusTarget p2)
            {
                return p.Sequence.CompareTo(p2.Sequence);
            });

            long count = 0;
            
            foreach (ConsensusTarget target in targets)
            {

                if (count++ > 100)
                {
                    count = 0;
                    Application.DoEvents(); // I hate doing this but we should "draw" every N nodes (100 in this case)
                }

                // Only for predicted targets
                bool skip = false;
                foreach (Target t in target.Targets)
                {
                    if (!t.IsPredicted)
                    {
                        skip = true;
                    }
                }
                if (skip) continue;

                TreeNode targetNode = new TreeNode(
                                        string.Format("{0} ({1})",
                                                                target.CleanSequence,
                                                                target.Targets.Count
                                                                ));
                targetNode.ImageIndex = CONST_MASS_TAG_RED;
                targetNode.Nodes.Add(string.Format("Avg NET: {0:F2}", target.GaNetAverage));
                targetNode.Nodes.Add(string.Format("Stdev. NET: {0:F2}", target.GaNetStdev));
                targetNode.Nodes.Add(string.Format("Min NET: {0:F2}", target.GaNetMinium));
                targetNode.Nodes.Add(string.Format("MaxNET: {0:F2}", target.GaNetMax));
                // Cheating...
                targetNode.Nodes[0].ImageIndex = CONST_PROPERTY;
                targetNode.Nodes[1].ImageIndex = CONST_PROPERTY;
                targetNode.Nodes[2].ImageIndex = CONST_PROPERTY;
                targetNode.Nodes[3].ImageIndex = CONST_PROPERTY;


                TreeNode targetSubNode   = new TreeNode("Peptides");
                targetSubNode.ImageIndex = CONST_FLAME;
                targetNode.Nodes.Add(targetSubNode);

                foreach (Target t in target.Targets)
                {
                    TreeNode node   = new TreeNode(t.Sequence);
                    node.ImageIndex = CONST_FLAME;
                    targetSubNode.Nodes.Add(node);
                    node.Nodes.Add(string.Format("Mass: {0:F2}", t.MonoisotopicMass));
                    node.Nodes.Add(string.Format("Scan: {0}", t.Scan));
                    node.Nodes.Add(string.Format("NET Pred: {0:F2}", t.NetPredicted));
                    node.Nodes.Add(string.Format("NET Align: {0:F2}", t.NetAligned));
                    node.Nodes[0].ImageIndex = CONST_PROPERTY;
                    node.Nodes[1].ImageIndex = CONST_PROPERTY;
                    node.Nodes[2].ImageIndex = CONST_PROPERTY;
                    node.Nodes[3].ImageIndex = CONST_PROPERTY;                    
                }


                TreeNode proteinSubNode     = new TreeNode("Proteins");
                proteinSubNode.ImageIndex   = CONST_PROTEIN;
                targetNode.Nodes.Add(proteinSubNode);

                foreach (Protein p in target.GetProteins())
                {
                    int matchedTargets      = p.GetMappedTargets().Count;

                    TreeNode proteinNode    = new TreeNode(
                                                    string.Format("{0} ({1})",
                                                            p.Reference,
                                                            matchedTargets)); 

                    proteinNode.ImageIndex          = CONST_PROTEIN;
                    proteinNode.Nodes.Add(string.Format("Matching Mass Tags: {0}", matchedTargets));
                    proteinNode.Nodes[0].ImageIndex = CONST_PROTEIN;
                    proteinSubNode.Nodes.Add(proteinNode);                      
                }
                rootNode.Nodes.Add(targetNode);                
            }
            mtree_proteins.Nodes.Add(rootNode);
            SetNodeSelectImage(rootNode);
            rootNode.Expand();
        }

        private void SetNodeSelectImage(TreeNode node)
        {
            node.SelectedImageIndex = node.ImageIndex;
            foreach (TreeNode subNode in node.Nodes)
                SetNodeSelectImage(subNode);
        }
                      
    }
}
