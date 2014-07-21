using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBFramework.Data
{
    public class PostTranslationalModification
    {
        private int m_id;
        private int m_location;
        private string m_formula;
        private double m_mass;
        private ConsensusTarget m_parent;

        public int Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        public int Location
        {
            get { return m_location; }
            set { m_location = value; }
        }

        public string Formula
        {
            get { return m_formula; }
            set { m_formula = value; }
        }

        public double Mass
        {
            get { return m_mass; }
            set { m_mass = value; }
        }

        public ConsensusTarget Parent
        {
            get { return m_parent; }
            set { m_parent = value; }
        }
    }
}
