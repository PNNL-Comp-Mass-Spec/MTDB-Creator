using System;
using System.Collections.Generic;
using System.Windows.Controls;
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
            results.Add((string)values[2]);
            results.Add((string)values[3]);

            // Regression Type
            results.Add(values[4].ToString());
            results.Add((string)values[5]);

            // Predictor Type
            results.Add(values[6].ToString());

            // Tryptic Peptides
            results.Add(values[7].ToString());
            results.Add((string)values[8]);
            results.Add((string)values[9]);
            results.Add((string)values[10]);

            // Partially Tryptic Peptides
            results.Add(values[11].ToString());
            results.Add((string)values[12]);
            results.Add((string)values[13]);
            results.Add((string)values[14]);

            // Non Tryptic Peptides
            results.Add(values[15].ToString());
            results.Add((string)values[16]);
            results.Add((string)values[17]);
            results.Add((string)values[18]);

            // Sequest dlCN
            results.Add(values[19].ToString());
            results.Add((string)values[20]);

            // X!Tandem Export
            results.Add((string)values[21]);

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
