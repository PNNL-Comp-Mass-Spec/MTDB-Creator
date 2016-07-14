using MTDBAccessIO;
using MTDBFramework.IO;

namespace MTDBCreator.DmsExporter.IO
{
    /// <summary>
    /// Factory to determine the database format to convert to
    /// </summary>
    public class TextToDbConverterFactory
    {
        /// <summary>
        /// Method to create the appropriate converter
        /// </summary>
        /// <param name="path">Output path for the written database</param>
        /// <returns>Interface for Database conversion from text files</returns>
        public static ITextToDbConverter Create(string path)
        {
            ITextToDbConverter converter = null;

            if (path.EndsWith(".mdb"))
            {
                converter = new TextToAccessConverter();
            }
            else if (path.EndsWith(".mtdb"))
            {
                converter = new TextToMtdbConverter();
            }

            return converter;
        }
    }
}
