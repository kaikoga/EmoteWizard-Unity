using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.L10n
{
    public readonly struct LocalizedProperty
    {
        public readonly SerializedProperty Property;
        public readonly LocalizedContent Loc;

        public GUIContent GUIContent => Loc.GUIContent;

        public LocalizedProperty(SerializedProperty property, LocalizedContent loc)
        {
            Property = property;
            Loc = loc;
        }
    }
}