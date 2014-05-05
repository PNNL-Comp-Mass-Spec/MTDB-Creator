#region Namespaces

using System.Collections.ObjectModel;
using MTDBFramework.Data;

#endregion

namespace MTDBFramework.Database
{
    public class TargetDatabase
    {
        public ObservableCollection<ConsensusTarget> ConsensusTargets { get; set; }
    }
}