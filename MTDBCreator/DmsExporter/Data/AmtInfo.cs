namespace MTDBCreator.DmsExporter.Data
{
    public class AmtInfo
    {
        private string m_name;
        private string m_description;
        private string m_organism;
        private string m_campaign;
        private string m_state;
        private int m_stateId;
        private string m_server;

        public string Name
        {
            get { return m_name; }
            set
            {
                m_name = value;
            }
        }

        public string Description
        {
            get { return m_description; }
            set
            {
                m_description = value;

            }
        }

        public string Organism
        {
            get { return m_organism; }
            set
            {
                m_organism = value;

            }
        }

        public string Campaign
        {
            get { return m_campaign; }
            set
            {
                m_campaign = value;

            }
        }

        public string State
        {
            get { return m_state; }
            set
            {
                m_state = value;

            }
        }

        public int StateId
        {
            get { return m_stateId; }
            set
            {
                m_stateId = value;

            }
        }

        public string Server
        {
            get { return m_server; }
            set
            {
                m_server = value;

            }
        }

    }
}
