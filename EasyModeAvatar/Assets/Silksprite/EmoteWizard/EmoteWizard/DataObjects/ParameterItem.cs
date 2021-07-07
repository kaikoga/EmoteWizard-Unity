using System;
using System.Collections.Generic;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace EmoteWizard.DataObjects
{
    [Serializable]
    public class ParameterItem
    {
        public string name;
        public ParameterValueKind valueKind;
        public bool saved = true;
        public float defaultValue;
        public bool defaultParameter;
        public List<ParameterUsage> usages;

        public VRCExpressionParameters.ValueType VrcValueType
        {
            get
            {
                switch (valueKind)
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
                    valueKind = ParameterValueKind.Int,
                    defaultParameter = true
                }/*,
                new ParameterItem
                {
                    // for default FX controller "vrc_AvatarV3FaceLayer", unused because we overwrite FX layer
                    defaultValue = 0,
                    name = "VRCFaceBlendH",
                    saved = false,
                    valueType = VRCExpressionParameters.ValueType.Float,
                    defaultParameter = true
                },
                new ParameterItem
                {
                    // for default FX controller 
                    defaultValue = 0,
                    name = "VRCFaceBlendV",
                    saved = false,
                    valueType = VRCExpressionParameters.ValueType.Float,
                    defaultParameter = true
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