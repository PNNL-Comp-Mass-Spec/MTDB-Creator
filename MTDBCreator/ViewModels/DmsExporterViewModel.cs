﻿using MTDBCreator.DmsExporter.IO;
using MTDBCreator.Helpers;

namespace MTDBCreator.ViewModels
{
    public class DmsExporterViewModel: ObservableObject
    {
        private DmsLookupUtility m_lookup;

        public AmtListViewModel AmtListViewModel { get; set; }

        public DmsExporterViewModel()
        {
            m_lookup = new DmsLookupUtility();
            AmtListViewModel = new AmtListViewModel();
        }
    }
}
