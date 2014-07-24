using FluentNHibernate.Mapping;
using MTDBFramework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBFramework.Database
{
    class PostTranslationalModificationMap : ClassMap<PostTranslationalModification>
    {
        public PostTranslationalModificationMap()
        {
            Not.LazyLoad();
            Id(x => x.Id).Column("PostTranslationModId").GeneratedBy.Assigned();
            Map(x => x.Formula);
            Map(x => x.Mass);
            Map(x => x.Name);
            //Map(x => x.Location); // Move me to the join table(?)

            //References(x => x.Parent).Column("ConsensusId").Cascade.SaveUpdate();
        }
    }
}
