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

            
            List<Protein> proteins = database.Proteins;
            TreeNode rootNode = new TreeNode(string.Format("Proteins ({0})", proteins.Count));
            rootNode.ImageIndex = CONST_DATABASE;
            
            proteins.Sort(delegate(Protein p, Protein p2)
            {
                return p.Reference.CompareTo(p2.Reference);
            });

            foreach (Protein p in proteins)
            {
                List<ConsensusTarget> targets = p.GetMappedTargets();

                TreeNode proteinNode = new TreeNode(
                                            string.Format("{0} ({1})",
                                                            p.Reference,
                                                            targets.Count));
                proteinNode.ImageIndex = CONST_PROTEIN;                
                proteinNode.Nodes.Add(string.Format("Id: {0}", p.Id));
                proteinNode.Nodes[0].ImageIndex = CONST_PROPERTY;

                TreeNode massTagNode    = new TreeNode("Mass Tags");
                massTagNode.ImageIndex = CONST_MASS_TAG_RED;
                proteinNode.Nodes.Add(massTagNode);

                foreach (ConsensusTarget target in targets)
                {
                    TreeNode targetNode = new TreeNode(
                                            string.Format("{0} ({1})",
                                                                    target.CleanSequence,
                                                                    target.Targets.Count
                                                                    ));
                    targetNode.ImageIndex = CONST_MASS_TAG_RED;
                    targetNode.Nodes.Add(string.Format("Avg NET: {0:0.00}", target.GaNetAverage));                    
                    targetNode.Nodes.Add(string.Format("Stdev. NET: {0:0.00}", target.GaNetStdev));
                    targetNode.Nodes.Add(string.Format("Min NET: {0:0.00}", target.GaNetMinium));
                    targetNode.Nodes.Add(string.Format("MaxNET: {0:0.00}", target.GaNetMax));
                    // Cheating...
                    targetNode.Nodes[0].ImageIndex = CONST_PROPERTY;
                    targetNode.Nodes[1].ImageIndex = CONST_PROPERTY;
                    targetNode.Nodes[2].ImageIndex = CONST_PROPERTY;
                    targetNode.Nodes[3].ImageIndex = CONST_PROPERTY;  

                  
                    TreeNode targetSubNode = new TreeNode("Peptides");
                    targetSubNode.ImageIndex = CONST_FLAME;
                    targetNode.Nodes.Add(targetSubNode);

                    foreach (Target t in target.Targets)
                    {
                        TreeNode node   = new TreeNode(t.Sequence);
                        node.ImageIndex = CONST_FLAME;
                        targetSubNode.Nodes.Add(node);
                        node.Nodes.Add(string.Format("Mass: {0}", t.MonoisotopicMass)); 
                        node.Nodes.Add(string.Format("Scan: {0}", t.Scan));
                        node.Nodes.Add(string.Format("NET Pred: {0:0.00}", t.NetPredicted));
                        node.Nodes.Add(string.Format("NET Align: {0:0.00}", t.NetAligned));
                        node.Nodes[0].ImageIndex = CONST_PROPERTY;
                        node.Nodes[1].ImageIndex = CONST_PROPERTY;
                        node.Nodes[2].ImageIndex = CONST_PROPERTY;
                        node.Nodes[3].ImageIndex = CONST_PROPERTY;                        
                    }
                    massTagNode.Nodes.Add(targetNode);
                }
                rootNode.Nodes.Add(proteinNode);
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
