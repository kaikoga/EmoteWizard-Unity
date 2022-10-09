using System;
using System.Collections.Generic;
using System.Linq;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.DataObjects.Internal
{
    public class ParameterInstance
    {
        public string Name;
        public ParameterItemKind ItemKind;
        public bool Saved = true;
        public float DefaultValue;
        public List<ParameterUsage> Usages;

        public ParameterValueKind ValueKind
        {
            get
            {
                switch (ItemKind)
                {
                    case ParameterItemKind.Auto:
                        if (Usages.Any(usage => usage.usageKind == ParameterUsageKind.Float)) return ParameterValueKind.Float;
                        return Usages.Count(usage => usage.usageKind != ParameterUsageKind.Default) > 1 ? ParameterValueKind.Int : ParameterValueKind.Bool;
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

        VRCExpressionParameters.ValueType VrcValueType
        {
            get
            {
                switch (ValueKind)
                {
                    case ParameterValueKind.Bool:
                        return VRCExpressionParameters.ValueType.Bool;
                    case ParameterValueKind.Int:
                        return VRCExpressionParameters.ValueType.Int;
                    case ParameterValueKind.Float:
                        return VRCExpressionParameters.ValueType.Float;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public VRCExpressionParameters.Parameter ToParameter()
        {
            return new VRCExpressionParameters.Parameter
            {
                name = Name,
                saved = Saved,
                defaultValue = DefaultValue,
                valueType = VrcValueType
            };
        }
    }
}