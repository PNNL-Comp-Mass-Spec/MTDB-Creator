using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBCreator.DmsExporter.IO;

namespace MTDBCreator.ViewModels
{
    public class DmsExporterViewModel: ViewModelBase
    {
        private DmsLookupUtility m_lookup;

        public AmtListViewModel AmtListViewModel { get; set; }
        public AmtPeptideOptionsViewModel AmtPeptideOptionsViewModel { get; set; }

        public DmsExporterViewModel()
        {
            m_lookup = new DmsLookupUtility();
            AmtListViewModel = new AmtListViewModel();
            AmtPeptideOptionsViewModel = new AmtPeptideOptionsViewModel();
        }
    }
}
