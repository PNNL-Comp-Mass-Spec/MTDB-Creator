using System;
using System.Data;
using System.Globalization;
using PRISM;

namespace MTDBCreator.DmsExporter.IO
{
    static class DbValueUtilities
    {
        public static string CurrentQueryInfo { get; set; } = string.Empty;

        /// <summary>
        /// Get the date as a string
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        public static string DbDateToString(IDataRecord reader, int colIndex)
        {
            if (Convert.IsDBNull(reader.GetValue(colIndex)))
                return string.Empty;

            return reader.GetDateTime(colIndex).ToString(PRISM.Logging.LogMessage.DATE_TIME_FORMAT_YEAR_MONTH_DAY_12H);
        }

        /// <summary>
        /// Get the float (C# double) field value as a string, with a fixed number of digits after the decimal place
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="colIndex"></param>
        /// <param name="valueIfMissing"></param>
        /// <param name="digitsAfterDecimal"></param>
        /// <returns></returns>
        public static string DbValueToStringFixedDecimals(IDataRecord reader, int colIndex, double valueIfMissing, byte digitsAfterDecimal)
        {
            return StringUtilities.DblToString(GetDbFieldValue(reader, colIndex, valueIfMissing), digitsAfterDecimal);
        }

        /// <summary>
        /// Get the tinyint field value as a string
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="colIndex"></param>
        /// <param name="valueIfMissing"></param>
        /// <returns></returns>
        public static string DbValueToString(IDataRecord reader, int colIndex, byte valueIfMissing)
        {
            return GetDbFieldValue(reader, colIndex, valueIfMissing).ToString();
        }

        /// <summary>
        /// Get the decimal field value as a string
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="colIndex"></param>
        /// <param name="valueIfMissing"></param>
        /// <returns></returns>
        public static string DbValueToString(IDataRecord reader, int colIndex, decimal valueIfMissing)
        {
            return GetDbFieldValue(reader, colIndex, valueIfMissing).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Get the float (C# double) field value as a string
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="colIndex"></param>
        /// <param name="valueIfMissing"></param>
        /// <param name="digitsOfPrecision"></param>
        /// <returns></returns>
        public static string DbValueToString(IDataRecord reader, int colIndex, double valueIfMissing, byte digitsOfPrecision)
        {
            return StringUtilities.ValueToString(GetDbFieldValue(reader, colIndex, valueIfMissing), digitsOfPrecision);
        }

        /// <summary>
        /// Get the real (C# float) field value as a string
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="colIndex"></param>
        /// <param name="valueIfMissing"></param>
        /// <returns></returns>
        public static string DbValueToString(IDataRecord reader, int colIndex, float valueIfMissing)
        {
            return GetDbFieldValue(reader, colIndex, valueIfMissing).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Get the integer field value as a string
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="colIndex"></param>
        /// <param name="valueIfMissing"></param>
        /// <returns></returns>
        public static string DbValueToString(IDataRecord reader, int colIndex, int valueIfMissing)
        {
            return GetDbFieldValue(reader, colIndex, valueIfMissing).ToString();
        }

        /// <summary>
        /// Get the smallint field value as a string
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="colIndex"></param>
        /// <param name="valueIfMissing"></param>
        /// <returns></returns>
        public static string DbValueToString(IDataRecord reader, int colIndex, short valueIfMissing)
        {
            return GetDbFieldValue(reader, colIndex, valueIfMissing).ToString();
        }

        /// <summary>
        /// Get the tinyint field value
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="colIndex"></param>
        /// <param name="valueIfMissing"></param>
        /// <returns></returns>
        public static byte GetDbFieldValue(IDataRecord reader, int colIndex, byte valueIfMissing)
        {
            try
            {
                if (Convert.IsDBNull(reader.GetValue(colIndex)))
                    return valueIfMissing;

                return reader.GetByte(colIndex);
            }
            catch (Exception ex)
            {
                try
                {
                    var valueText = reader.GetValue(colIndex).ToString();
                    if (byte.TryParse(valueText, out var value))
                        return value;

                    ConsoleMsgUtils.ShowWarning("ColIndex {0} for {1} is not an integer: {2}", colIndex, CurrentQueryInfo, valueText);
                    return valueIfMissing;
                }
                catch (Exception)
                {
                    ConsoleMsgUtils.ShowWarning("Error parsing colIndex {0} for {1} as an integer: {2}", colIndex, CurrentQueryInfo, ex.Message);
                    return valueIfMissing;
                }
            }
        }

        /// <summary>
        /// Get the decimal field value
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="colIndex"></param>
        /// <param name="valueIfMissing"></param>
        /// <returns></returns>
        public static decimal GetDbFieldValue(IDataRecord reader, int colIndex, decimal valueIfMissing)
        {
            try
            {
                if (Convert.IsDBNull(reader.GetValue(colIndex)))
                    return valueIfMissing;

                return reader.GetDecimal(colIndex);
            }
            catch (Exception)
            {
                var valueAsDouble = GetDbFieldValue(reader, colIndex, (double)valueIfMissing);
                return (decimal)valueAsDouble;
            }
        }

        /// <summary>
        /// Get the float (C# double) field value
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="colIndex"></param>
        /// <param name="valueIfMissing"></param>
        /// <returns></returns>
        public static double GetDbFieldValue(IDataRecord reader, int colIndex, double valueIfMissing)
        {
            try
            {
                if (Convert.IsDBNull(reader.GetValue(colIndex)))
                    return valueIfMissing;

                return reader.GetDouble(colIndex);
            }
            catch (Exception ex)
            {
                try
                {
                    var valueText = reader.GetValue(colIndex).ToString();
                    if (double.TryParse(valueText, out var value))
                        return value;

                    ConsoleMsgUtils.ShowWarning("ColIndex {0} for {1} is not numeric: {2}", colIndex, CurrentQueryInfo, valueText);
                    return valueIfMissing;
                }
                catch (Exception)
                {
                    ConsoleMsgUtils.ShowWarning("Error parsing colIndex {0} for {1} as a double: {2}", colIndex, CurrentQueryInfo, ex.Message);
                    return valueIfMissing;
                }
            }
        }

        /// <summary>
        /// Get the real (C# float) field value
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="colIndex"></param>
        /// <param name="valueIfMissing"></param>
        /// <returns></returns>
        public static float GetDbFieldValue(IDataRecord reader, int colIndex, float valueIfMissing)
        {
            try
            {
                if (Convert.IsDBNull(reader.GetValue(colIndex)))
                    return valueIfMissing;

                return reader.GetFloat(colIndex);
            }
            catch (Exception)
            {
                var valueAsDouble = GetDbFieldValue(reader, colIndex, (double)valueIfMissing);
                return (float)valueAsDouble;
            }
        }

        /// <summary>
        /// Get the integer field value
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="colIndex"></param>
        /// <param name="valueIfMissing"></param>
        /// <returns></returns>
        public static int GetDbFieldValue(IDataRecord reader, int colIndex, int valueIfMissing)
        {
            try
            {
                if (Convert.IsDBNull(reader.GetValue(colIndex)))
                    return valueIfMissing;

                return reader.GetInt32(colIndex);
            }
            catch (Exception ex)
            {
                try
                {
                    var valueText = reader.GetValue(colIndex).ToString();
                    if (int.TryParse(valueText, out var value))
                        return value;

                    ConsoleMsgUtils.ShowWarning("ColIndex {0} for {1} is not an integer: {2}", colIndex, CurrentQueryInfo, valueText);
                    return valueIfMissing;
                }
                catch (Exception)
                {
                    ConsoleMsgUtils.ShowWarning("Error parsing colIndex {0} for {1} as an integer: {2}", colIndex, CurrentQueryInfo, ex.Message);
                    return valueIfMissing;
                }
            }
        }

        /// <summary>
        /// Get the smallint field value
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="colIndex"></param>
        /// <param name="valueIfMissing"></param>
        /// <returns></returns>
        public static short GetDbFieldValue(IDataRecord reader, int colIndex, short valueIfMissing)
        {
            try
            {
                if (Convert.IsDBNull(reader.GetValue(colIndex)))
                    return valueIfMissing;

                return reader.GetInt16(colIndex);
            }
            catch (Exception ex)
            {
                try
                {
                    var valueText = reader.GetValue(colIndex).ToString();
                    if (short.TryParse(valueText, out var value))
                        return value;

                    ConsoleMsgUtils.ShowWarning("ColIndex {0} for {1} is not an integer: {2}", colIndex, CurrentQueryInfo, valueText);
                    return valueIfMissing;
                }
                catch (Exception)
                {
                    ConsoleMsgUtils.ShowWarning("Error parsing colIndex {0} for {1} as an integer: {2}", colIndex, CurrentQueryInfo, ex.Message);
                    return valueIfMissing;
                }
            }
        }

        /// <summary>
        /// Get the value as a string
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="colIndex"></param>
        /// <param name="valueIfMissing"></param>
        /// <returns></returns>
        public static string GetDbFieldValue(IDataRecord reader, int colIndex, string valueIfMissing = "")
        {
            try
            {
                if (Convert.IsDBNull(reader.GetValue(colIndex)))
                    return valueIfMissing;

                return reader.GetString(colIndex);
            }
            catch (Exception ex)
            {
                try
                {
                    var valueText = reader.GetValue(colIndex).ToString();
                    return valueText;
                }
                catch (Exception)
                {
                    ConsoleMsgUtils.ShowWarning("Error parsing colIndex {0} for {1}: {2}", colIndex, CurrentQueryInfo, ex.Message);
                    return valueIfMissing;
                }
            }
        }

        public static string PossiblyQuoteString(string fieldValue, string separator)
        {
            if (!fieldValue.Contains(separator))
                return fieldValue;

            return string.Format(@"""{0}""", fieldValue);
        }
    }
}
