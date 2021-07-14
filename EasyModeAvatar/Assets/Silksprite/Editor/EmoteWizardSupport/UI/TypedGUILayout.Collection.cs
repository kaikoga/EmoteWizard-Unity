using System.Collections.Generic;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static partial class TypedGUILayout
    {
        public static List<T> ListField<T>(string label, ref List<T> value, params GUILayoutOption[] options)
        {
            return value;
        }
    }
}