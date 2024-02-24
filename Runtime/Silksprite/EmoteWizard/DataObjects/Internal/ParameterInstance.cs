using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    [Serializable]
    public class ParameterInstance
    {
        [SerializeField] public string Name;
        [SerializeField] public ParameterItemKind ItemKind;
        [SerializeField] public bool Saved = true;
        [SerializeField] public float DefaultValue;
        [SerializeField] public bool Synced = true;
        [SerializeField] public List<ParameterWriteUsage> WriteUsages;
        [SerializeField] public List<ParameterReadUsage> ReadUsages;

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