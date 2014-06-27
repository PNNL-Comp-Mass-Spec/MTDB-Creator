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
            results.Add((string)values[4]);
            results.Add((string)values[5]);

            // Regression Type
            results.Add(values[6].ToString());
            results.Add((string)values[7]);

            // Predictor Type
            results.Add(values[8].ToString());

            // Tryptic Peptides
            results.Add(values[9].ToString());
            results.Add((string)values[10]);
            results.Add((string)values[11]);
            results.Add((string)values[12]);

            // Partially Tryptic Peptides
            results.Add(values[13].ToString());
            results.Add((string)values[14]);
            results.Add((string)values[15]);
            results.Add((string)values[16]);

            // Non Tryptic Peptides
            results.Add(values[17].ToString());
            results.Add((string)values[18]);
            results.Add((string)values[19]);
            results.Add((string)values[20]);

            // Sequest dlCN
            results.Add(values[21].ToString());
            results.Add((string)values[22]);

            // X!Tandem Export
            results.Add((string)values[23]);

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
