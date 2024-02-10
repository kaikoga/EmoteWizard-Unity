using System;
using System.Collections.Generic;
using System.Linq;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class ParameterInstance
    {
        public string Name;
        public ParameterItemKind ItemKind;
        public bool Saved = true;
        public float DefaultValue;
        public bool Synced = true;
        public List<ParameterWriteUsage> WriteUsages;
        public List<ParameterReadUsage> ReadUsages;

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
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}