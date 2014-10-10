using System;
using System.Collections.Generic;
using System.Windows.Data;

namespace MTDBCreator.Helpers
{
    public class OptionsParameterConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var results = new List<string>();

            results.Add((string)values[0]);
            results.Add((string)values[1]);

            // Regression Type
            results.Add(values[2].ToString());
            results.Add((string)values[3]);

            // Predictor Type
            results.Add(values[4].ToString());

            // Filtering Type
            results.Add(values[5].ToString());

            for(int i = 0; i < results.Count; i++)
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
