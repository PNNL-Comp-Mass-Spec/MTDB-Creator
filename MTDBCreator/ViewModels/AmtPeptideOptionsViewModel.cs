using System.Collections.ObjectModel;
using MTDBCreator.DmsExporter.Data;

namespace MTDBCreator.ViewModels
{
    public class AmtPeptideOptionsViewModel
    {
        public ObservableCollection<AmtPeptideOptions> QualityStats { get; set; }

        public AmtPeptideOptions SelectedStats { get; set; }

        public AmtPeptideOptionsViewModel()
        {
            QualityStats = new ObservableCollection<AmtPeptideOptions>();
        }
    }
}
