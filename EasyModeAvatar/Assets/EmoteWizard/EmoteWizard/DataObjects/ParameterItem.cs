using System;
using System.Collections.Generic;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace EmoteWizard.DataObjects
{
    [Serializable]
    public class ParameterItem
    {
        public string name;
        public VRCExpressionParameters.ValueType valueType;
        public bool saved = true;
        public float defaultValue;
        public bool defaultParameter;
        public List<ParameterState> states;

        public static List<ParameterItem> VrcDefaultParameters =>
            new List<ParameterItem>
            {
                new ParameterItem
                {
                    defaultValue = 0,
                    name = "VRCEmote",
                    saved = false,
                    valueType = VRCExpressionParameters.ValueType.Int,
                    defaultParameter = true
                },
                new ParameterItem
                {
                    defaultValue = 0,
                    name = "VRCFaceBlendH",
                    saved = false,
                    valueType = VRCExpressionParameters.ValueType.Float,
                    defaultParameter = true
                },
                new ParameterItem
                {
                    defaultValue = 0,
                    name = "VRCFaceBlendV",
                    saved = false,
                    valueType = VRCExpressionParameters.ValueType.Float,
                    defaultParameter = true
                }
            };

        public VRCExpressionParameters.Parameter ToParameter()
        {
            return new VRCExpressionParameters.Parameter
            {
                name = name,
                saved = saved,
                defaultValue = defaultValue,
                valueType = valueType
            };
        }
    }
}