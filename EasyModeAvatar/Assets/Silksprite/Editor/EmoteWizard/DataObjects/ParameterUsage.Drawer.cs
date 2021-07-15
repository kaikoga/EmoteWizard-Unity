using Silksprite.EmoteWizardSupport.Collections.Generic.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [CustomPropertyDrawer(typeof(ParameterUsage))]
    public class ParameterUsageDrawer : HybridDrawerBase<ParameterUsage>
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUI.PropertyField(position.UISliceH(0.0f, 0.6f), property.FindPropertyRelative("usageKind"), new GUIContent("Value"));
                using (new HideLabelsScope())
                {
                    EditorGUI.PropertyField(position.UISliceH(0.6f, 0.4f), property.FindPropertyRelative("value"));
                }
            }
        }

        public override void OnGUI(Rect position, ref ParameterUsage item, GUIContent label)
        {
            using (new EditorGUI.IndentLevelScope())
            {
                TypedGUI.EnumPopup(position.UISliceH(0.0f, 0.6f), "Value", ref item.usageKind);
                using (new HideLabelsScope())
                {
                    TypedGUI.FloatField(position.UISliceH(0.6f, 0.4f), " ", ref item.value);
                }
            }
        }

    }
}