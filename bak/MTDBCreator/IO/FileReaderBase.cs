using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MTDBCreator.IO
{
    /// <summary>
    /// Reads a tabular based file format
    /// </summary>
    public abstract class FileReaderBase<T>
    {
        public FileReaderBase()
        {
            Delimiter = "\t";
        }

        /// <summary>
        /// Sets the header column data
        /// </summary>
        /// <param name="headers"></param>
        protected abstract void SetHeaderColumns(string [] headers);

        /// <summary>
        /// Processes a line of separated text.  Without spaces removed.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        protected abstract T ProcessLine(string[] line);

        /// <summary>
        /// Gets or sets the delimeter for the file.
        /// </summary>
        public string Delimiter { get; set; }

        /// <summary>
        /// Reads the sequences provided
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public List<T> Read(string filePath)
        {
            List<T> data = new List<T>();
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // Process the header
                    string headerLine = sr.ReadLine();

                    string [] delimiters = new string[] {Delimiter};
                    string [] headers    = headerLine.Split(delimiters, StringSplitOptions.None);                                                                                
                    SetHeaderColumns(headers);

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string [] columns   = line.Split(delimiters, StringSplitOptions.None);                                                                                
                        T item              = ProcessLine(columns);
                        data.Add(item);
                    }
                }
            }
            catch (System.IO.IOException ioException)
            {                
                throw ioException;
            }
            return data;
        }
    }
}
