using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    [Serializable]
    public class ParameterInstance
    {
        [SerializeField] public string name = "";
        [SerializeField] public ParameterItemKind itemKind;
        [SerializeField] public bool saved = true;
        [SerializeField] public float defaultValue;
        [SerializeField] public bool synced = true;
        [SerializeField] public List<string> referenceUsages = new List<string>();
        [SerializeField] public List<ParameterWriteUsage> writeUsages = new List<ParameterWriteUsage>();
        [SerializeField] public List<ParameterReadUsage> readUsages = new List<ParameterReadUsage>();

        public ParameterValueKind ValueKind
        {
            get
            {
                switch (itemKind)
                {
                    case ParameterItemKind.Auto:
                        if (writeUsages.Any(usage => usage.writeUsageKind == ParameterWriteUsageKind.Float)) return ParameterValueKind.Float;
                        return writeUsages.Count(usage => usage.writeUsageKind != ParameterWriteUsageKind.Default) > 1 ? ParameterValueKind.Int : ParameterValueKind.Bool;
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

        public ParameterWriteSourceKind WriteSourceKind => writeUsages.Select(usage => usage.writeSourceKind)
                .FirstOrDefault(writeUsage => !(writeUsage is ParameterWriteSourceKind.NoUI));
    }
}