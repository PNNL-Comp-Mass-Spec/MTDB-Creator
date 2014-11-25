using FluentNHibernate.Automapping;

namespace MTDBFramework.IO
{
    /// <summary>
    /// Interface for converting text file to a database file
    /// </summary>
    public interface ITextToDbConverter
    {
        /// <summary>
        /// Interface for converting temporarily saved .txt files to database
        /// at designated path
        /// </summary>
        /// <param name="path">Location of saved database</param>
        void ConvertToDbFormat(string path);
    }
}
