using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBFramework.Data
{
    public class PostTranslationalModification
    {
        private int m_location;
        private string m_formula;
        private double m_mass;

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
    }
}
