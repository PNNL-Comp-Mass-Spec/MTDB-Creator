using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBCreator.Data
{
    public class ProteinMapper
    {

        /// <summary>
        /// Maps the sequences onto proteins.
        /// </summary>
        /// <param name="sequences"></param>
        /// <returns></returns>
        public static List<Protein> MapProteins(List<Target> targets,  List<SequenceToProteinMap> maps)
        {
            List<Protein> proteins = new List<Protein>();

            Dictionary<int, List<Target>> targetMap = new Dictionary<int, List<Target>>();

            foreach(Target t in targets)
            {
                if (t.SequenceData != null)
                {
                    if (!targetMap.ContainsKey(t.SequenceData.Id))
                    {
                        targetMap.Add(t.SequenceData.Id, new List<Target>());
                    }
                    targetMap[t.SequenceData.Id].Add(t);
                }
            }

            Dictionary<string, Protein> proteinMap = new Dictionary<string, Protein>();

            foreach (SequenceToProteinMap map in maps)
            {
                bool hasSequence = targetMap.ContainsKey(map.UniqueSequenceId);
                if (hasSequence)
                {                    

                    if (!proteinMap.ContainsKey(map.ProteinName))
                    {
                        Protein p       = new Protein();
                        p.Reference     = map.ProteinName;
                        p.EValue        = map.ProteinEValue;
                        p.IntensityLog  = map.ProteinIntensityLog;
                        p.TerminusState = map.TerminusState;
                        p.CleavageState = map.CleavageState;
                        proteinMap.Add(p.Reference, p);

                        proteins.Add(p);
                    }
                    Protein protein = proteinMap[map.ProteinName];

                    // We dont want to map this target onto any proteins yet
                    // because we would rather map the consensus target onto it later.
                    foreach (Target t in targetMap[map.UniqueSequenceId])
                    {
                        t.Proteins.Add(protein);
                    }
                }
            }

            return proteins;
        }
    }
}
