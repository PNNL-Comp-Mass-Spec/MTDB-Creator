using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBCreator.IO
{
    public class MassTagDatabaseWriterFactory
    {
        public static IMassTagDatabaseWriter CreateWriter(MtdbWriterType type)
        {
            IMassTagDatabaseWriter writer = null;
            switch (type)
            {
                case MtdbWriterType.Sqlite:
                    writer = new MultiAlignSqliteMtdbWriter();
                    break;
                default:
                    break;
            }

            return writer;
        }
    }

    public enum MtdbWriterType
    {
        Sqlite
    }
}
