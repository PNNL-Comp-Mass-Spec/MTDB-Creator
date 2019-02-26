using FluentNHibernate.Mapping;
using MTDBFrameworkBase.Data;

namespace MTDBFrameworkBase.Database
{
    class PostTranslationalModificationMap : ClassMap<PostTranslationalModification>
    {
        public PostTranslationalModificationMap()
        {
            Not.LazyLoad();
            Id(x => x.Id).Column("PostTranslationModId").GeneratedBy.Native();
            Map(x => x.Formula);
            Map(x => x.Mass);
            Map(x => x.Name);
        }
    }
}
