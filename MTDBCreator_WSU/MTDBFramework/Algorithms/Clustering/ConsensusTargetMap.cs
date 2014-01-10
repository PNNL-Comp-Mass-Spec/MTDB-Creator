using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

//This map will not be needed if I can get the Automapper to work properly.
namespace MTDBFramework.Algorithms.Clustering
{
    class ConsensusTargetMap : ClassMap<ConsensusTarget>
    {
        public ConsensusTargetMap()
        {
            Id(x => x.Id);
            Map(x => x.Net);
            Map(x => x.StdevNet);
            Map(x => x.Mass);
            Map(x => x.StdevMass);
            Map(x => x.Sequence);
            HasMany(x => x.Targets);
        }
    }
}
