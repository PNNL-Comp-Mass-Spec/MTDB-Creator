using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBFramework.Algorithms.Clustering;
using MTDBFramework.UI;

namespace MTDBCreator.ViewModels.TreeView
{
    public class ConsensusTargetViewModel : ObservableObject
    {
        #region Private Fields

        private ConsensusTarget m_ConsensusTarget;

        #endregion

        public ConsensusTargetViewModel(ConsensusTarget consensusTarget) : this(consensusTarget, null)
        {   
        }

        private ConsensusTargetViewModel(ConsensusTarget consensusTarget, ConsensusTargetViewModel parent)
        {
            m_ConsensusTarget = consensusTarget;

        }

        public ConsensusTarget ConsensusTarget
        {
            get
            {
                return m_ConsensusTarget;
            }
            set
            {
                m_ConsensusTarget = value;
                OnPropertyChanged("ConsensusTarget");
            }
        }

        public string Name
        {
            get
            {
                return "fuck";
            }
        }

    }
}
