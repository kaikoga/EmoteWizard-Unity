using System;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public class ParameterItem
    {
        public bool enabled = true;
        [ParameterName(false,true)]
        public string name;
        public ParameterItemKind itemKind;
        public bool saved = true;
        public float defaultValue;
        public bool synced = true;

        public bool IsValid => !ParameterNameAttribute.IsInvalidParameterInput(name, false);
    }
}