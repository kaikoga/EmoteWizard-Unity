using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizardSupport.Collections.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.Tools.PropertyDrawerUITools;

namespace Silksprite.EmoteWizard.Collections
{
    public class ParameterItemListDrawerBase : ListDrawerBase
    {
        public override string HeaderName => "Parameter Items";
        public override string PagerItemName(SerializedProperty property, int index)
        {
            var name = property.FindPropertyRelative("name").stringValue;
            var kind = (ParameterItemKind)property.FindPropertyRelative("itemKind").intValue;
            return $"{name} ({kind})";
        }

        protected override void DrawHeaderContent(Rect position)
        {
            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                GUI.Label(position.UISlice(0.10f, 0.35f, 0), "Name");
                GUI.Label(position.UISlice(0.45f, 0.20f, 0), "Type");
                GUI.Label(position.UISlice(0.65f, 0.20f, 0), "Default");
                GUI.Label(position.UISlice(0.85f, 0.15f, 0), "Saved");

                GUI.Label(position.UISlice(0.00f, 0.20f, 1), "Value");
            }
        }

        public override float GetHeaderHeight()
        {
            return BoxHeight(LineHeight(2f));
        }
    }
}