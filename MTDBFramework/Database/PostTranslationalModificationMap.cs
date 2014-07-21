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
            Id(x => x.Id).Column("PostTranslationModId").GeneratedBy.Identity();
            Map(x => x.Formula);
            Map(x => x.Mass);
            Map(x => x.Location);

            References(x => x.Parent).Column("ConsensusId").Cascade.SaveUpdate();
        }
    }
}
