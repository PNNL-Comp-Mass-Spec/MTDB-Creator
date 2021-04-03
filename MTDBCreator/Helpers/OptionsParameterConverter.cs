using System;
using System.Collections.Generic;
using System.Windows.Data;

namespace MTDBCreator.Helpers
{
    public class OptionsParameterConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var results = new List<string>
            {
                // Max MSGF SpecProb
                (string)values[0],

                // Max MSGF QValue
                (string)values[1],

                // Regression Type (AlignmentComboBox)
                values[2].ToString(),

                // OrderTextBox
                (string)values[3],

                // Predictor Type (KangasRadioButton)
                values[4].ToString(),

                // Filtering Type (SpectralValueRadioButton)
                values[5].ToString()
            };

            for(var i = 0; i < results.Count; i++)
            {
                if (results[i] == "")
                {
                    results[i] = "0";
                }
            }

            return results;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
