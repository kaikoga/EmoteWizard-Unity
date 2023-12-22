using System;
using Silksprite.EmoteWizard.DataObjects.Internal;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class ParameterItem
    {
        public bool enabled = true;
        public string name;
        public ParameterItemKind itemKind;
        public bool saved = true;
        public float defaultValue;

        public static ParameterItem Build(string parameter, ParameterItemKind itemKind)
        {
            return new ParameterItem
            {
                enabled = true,
                name = parameter,
                itemKind = itemKind,
                saved = false,
                defaultValue = 0
            };
        }

        public ParameterValueKind ValueKind => ToInstance().ValueKind;

        public ParameterInstance ToInstance()
        {
            return new ParameterInstance
            {
                Name = name,
                Saved = saved,
                DefaultValue = defaultValue,
                ItemKind = itemKind,
            };
        }

        public VRCExpressionParameters.Parameter ToParameter()
        {
            return ToInstance().ToParameter();
        }
    }
}