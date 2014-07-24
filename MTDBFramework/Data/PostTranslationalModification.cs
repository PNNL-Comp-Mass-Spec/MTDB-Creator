namespace MTDBFramework.Data
{
	/// <summary>
	/// Post Translational Modification data container
	/// </summary>
    public class PostTranslationalModification
    {
        private int m_id;
        private int m_location;
        private string m_formula;
        private double m_mass;
        private string m_name;
        private ConsensusTarget m_parent;

		/// <summary>
		/// Id
		/// </summary>
        public int Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

		/// <summary>
		/// Modification Location
		/// </summary>
        public int Location
        {
            get { return m_location; }
            set { m_location = value; }
        }

		/// <summary>
		/// Modification Formula
		/// </summary>
        public string Formula
        {
            get { return m_formula; }
            set { m_formula = value; }
        }

		/// <summary>
		/// Modification Mass
		/// </summary>
        public double Mass
        {
            get { return m_mass; }
            set { m_mass = value; }
        }

        /// <summary>
        /// Short name of the PTM
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        /// <summary>
		/// Target containing the modification
		/// </summary>
        public ConsensusTarget Parent
        {
            get { return m_parent; }
            set { m_parent = value; }
        }

		/// <summary>
		/// Overloaded string for debugging output
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			var temp = "Name:" + m_name + ";";
			temp += "Mass:" + m_mass + ";";
			temp += "Formula:" + m_formula;
			return temp;
		}
    }
}
