using System;
using System.Linq;

namespace MTDBFramework.Database
{
    public class TargetDataSet
    {
        private string m_path;     
   
        public int Id { get; set; }
        // Path only used to get the name of the data set for the database
        public string Path { get; set; }
        /// <summary>
        /// Gets or sets the name of the target dataset.
        /// </summary>
        public string Name { get; set; }        
    }

}
