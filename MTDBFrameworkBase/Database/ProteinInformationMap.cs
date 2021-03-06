﻿using FluentNHibernate.Mapping;
using MTDBFrameworkBase.Data;

namespace MTDBFrameworkBase.Database
{
    /// <summary>
    /// NHibernate mapping for the ProteinInformation class
    /// </summary>
    public class ProteinInformationMap : ClassMap<ProteinInformation>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProteinInformationMap()
        {
            Not.LazyLoad();
            Id(x => x.Id).Column("ProteinId").GeneratedBy.Native();
            Map(x => x.ProteinName);
        }
    }
}
