namespace MTDBFramework.Data
{
    public class MsAlignResult : Evidence
    {
        #region Private Fields

        private double m_eScore;

        #endregion

        #region Public Properties

        public double EScore
        {
            get { return m_eScore; }
            set
            {
                m_eScore = value;
                OnPropertyChanged("EScore");
            }
        }

        #endregion
    }
}