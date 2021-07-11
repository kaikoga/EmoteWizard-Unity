using System;
using System.Collections.Generic;
using System.Linq;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class ParameterItem
    {
        public string name;
        public ParameterItemKind itemKind;
        public bool saved = true;
        public float defaultValue;
        public List<ParameterUsage> usages;

        public ParameterValueKind ValueKind
        {
            get
            {
                switch (itemKind)
                {
                    case ParameterItemKind.Auto:
                        if (usages.Any(usage => usage.usageKind == ParameterUsageKind.Float)) return ParameterValueKind.Float;
                        return usages.Count(usage => usage.usageKind != ParameterUsageKind.Default) > 1 ? ParameterValueKind.Int : ParameterValueKind.Bool;
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

        public VRCExpressionParameters.ValueType VrcValueType
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

        public static List<ParameterItem> VrcDefaultParameters =>
            new List<ParameterItem>
            {
                new ParameterItem
                {
                    // for default Action controller "vrc_AvatarV3ActionLayer"
                    defaultValue = 0,
                    name = "VRCEmote",
                    saved = false,
                    itemKind = ParameterItemKind.Int
                }/*,
                new ParameterItem
                {
                    // for default FX controller "vrc_AvatarV3FaceLayer", unused because we overwrite FX layer
                    defaultValue = 0,
                    name = "VRCFaceBlendH",
                    saved = false,
                    valueType = VRCExpressionParameters.ValueType.Float
                },
                new ParameterItem
                {
                    // for default FX controller 
                    defaultValue = 0,
                    name = "VRCFaceBlendV",
                    saved = false,
                    valueType = VRCExpressionParameters.ValueType.Float
                }*/
            };

        public VRCExpressionParameters.Parameter ToParameter()
        {
            return new VRCExpressionParameters.Parameter
            {
                name = name,
                saved = saved,
                defaultValue = defaultValue,
                valueType = VrcValueType
            };
        }
    }
}