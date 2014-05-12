﻿#region Namespaces

using FluentNHibernate.Mapping;
using MTDBFramework.Data;

#endregion

namespace MTDBFramework.Database
{
    //This is our map for the ConsensusTarget class. Note the "Not.LazyLoad"
    public class ConsensusTargetMap : ClassMap<ConsensusTarget>
    {
        public ConsensusTargetMap()
        {
            Not.LazyLoad();
            Id(x => x.Id)
                .Column("ConsensusId").GeneratedBy.Assigned();
            Map(x => x.Net);
            Map(x => x.StdevNet);
            Map(x => x.PredictedNet);
            Map(x => x.TheoreticalMonoIsotopicMass);
            Map(x => x.Sequence);

            HasMany(x => x.Evidences).Cascade.All();
            HasManyToMany(x => x.Proteins).Table("ConsensusProteins")
                                          .ParentKeyColumn("ConsensusId")
                                          .ChildKeyColumn("ProteinID")
                                          .Cascade.All();
        }
    }
}