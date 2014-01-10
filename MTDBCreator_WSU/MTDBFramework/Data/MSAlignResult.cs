#region Namespaces

using System;

#endregion

namespace MTDBFramework.Data
{
    public class MSAlignResult : Target
    {
        #region Private Fields

        private double m_EScore;

        #endregion

        #region Public Properties

        public double EScore
        {
            get { return m_EScore; }
            set
            {
                m_EScore = value;
                OnPropertyChanged("EScore");
            }
        }

        #endregion
    }
}