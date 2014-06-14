namespace MTDBFramework.Data
{
    public class MsAlignResult : Evidence
    {
        #region Private Fields

        private double m_eValue;

        #endregion

        #region Public Properties

        /// <summary>
        /// MSAlign EValue
        /// </summary>
        public double EValue
        {
            get { return m_eValue; }
            set
            {
                m_eValue = value;
                OnPropertyChanged("EValue");
            }
        }

        #endregion
    }
}