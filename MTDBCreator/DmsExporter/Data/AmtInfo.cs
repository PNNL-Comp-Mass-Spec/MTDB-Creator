﻿namespace MTDBCreator.DmsExporter.Data
{
    public class AmtInfo
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Organism { get; set; }

        public string Campaign { get; set; }

        public string State { get; set; }

        public int StateId { get; set; }

        public string Server { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Server ?? "NoServer", Name ?? string.Empty);
        }
    }
}
