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
            results.Add((string)values[6]);

            // Regression Type
            results.Add(values[7].ToString());
            results.Add((string)values[8]);

            // Predictor Type
            results.Add(values[9].ToString());

            // Tryptic Peptides
            results.Add(values[10].ToString());
            results.Add((string)values[11]);
            results.Add((string)values[12]);
            results.Add((string)values[13]);

            // Partially Tryptic Peptides
            results.Add(values[14].ToString());
            results.Add((string)values[15]);
            results.Add((string)values[16]);
            results.Add((string)values[17]);

            // Non Tryptic Peptides
            results.Add(values[18].ToString());
            results.Add((string)values[19]);
            results.Add((string)values[20]);
            results.Add((string)values[21]);

            // Sequest dlCN
            results.Add(values[22].ToString());
            results.Add((string)values[23]);

            // X!Tandem Export
            results.Add((string)values[24]);

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
