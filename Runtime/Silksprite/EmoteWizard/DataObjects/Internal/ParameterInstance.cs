using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Platforms;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class ParameterInstance
    {
        public readonly string Name;
        public readonly ParameterItemKind ItemKind;
        public readonly bool Saved;
        public readonly ParameterValue DefaultValue;
        public readonly bool Synced;
        public readonly ICollection<string> ReferenceUsages;
        public readonly ICollection<ParameterWriteUsage> WriteUsages;
        public readonly ICollection<ParameterReadUsage> ReadUsages;

        public bool IsDense(IPlatformFeatures platformFeatures) => UsageIsDense(WriteUsages.Select(usage => usage.Value.AsInt(platformFeatures)));

        static bool UsageIsDense(IEnumerable<int> usages)
        {
            var usage = usages.GroupBy(value => value)
                .Select(g => g.Key)
                .OrderBy(key => key)
                .ToArray();
            return usage.SequenceEqual(Enumerable.Range(0, usage.Length));
        }

        public ParameterInstance(string name, ParameterItemKind itemKind, bool saved, ParameterValue defaultValue, bool synced,
            IEnumerable<string> referenceUsages,
            IEnumerable<ParameterWriteUsage> writeUsages,
            IEnumerable<ParameterReadUsage> readUsages)
        {
            Name = name;
            ItemKind = itemKind;
            Saved = saved;
            DefaultValue = defaultValue;
            Synced = synced;
            ReferenceUsages = referenceUsages.ToList();
            WriteUsages = writeUsages.ToList();
            ReadUsages = readUsages.ToList();
        }

        public ParameterValueKind ValueKind
        {
            get
            {
                switch (ItemKind)
                {
                    case ParameterItemKind.Auto:
                        if (WriteUsages.Any(usage => usage.WriteUsageKind == ParameterWriteUsageKind.Float)) return ParameterValueKind.Float;
                        return WriteUsages.Count(usage => usage.WriteUsageKind != ParameterWriteUsageKind.Default) > 1 ? ParameterValueKind.Int : ParameterValueKind.Bool;
                    case ParameterItemKind.Bool:
                        return ParameterValueKind.Bool;
                    case ParameterItemKind.Int:
                        return ParameterValueKind.Int;
                    case ParameterItemKind.Float:
                        return ParameterValueKind.Float;
                    case ParameterItemKind.HandSign:
                        return ParameterValueKind.HandSign;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public ParameterWriteSourceKind WriteSourceKind => WriteUsages.Select(usage => usage.WriteSourceKind)
                .FirstOrDefault(writeUsage => !(writeUsage is ParameterWriteSourceKind.NoUI));
    }
}